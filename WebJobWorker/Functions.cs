using HtmlAgilityPack; // html agility pack
using System.IO;
using Microsoft.Azure.WebJobs;
using System;
using WebJobWorker.Model;
using WebJobWorker.DatabaseConnections;
using System.Net;
using WebJobWorker.Data;
using System.Collections.Generic;
using WebJobWorker.Records;

namespace WebJobWorker
{
    public class Functions
    {
        public static DateTime dateTimeLastUpdated; // date website last updated
        public static DateTime dateTimeUpdatedTo; // date documents updated too 
        public static bool everyPartSuccessful = false; // if all parts have been successful

        // list of counties
        public enum County
        {
            Kerry, Cork, Limerick, Tipperary, Waterford, Kilkenny, Wexford, Laois, Carlow, Kildare, Wicklow,
            Offaly, Dublin, Meath, Westmeath, Louth, Monaghan, Cavan, Longford, Donegal, Leitrim, Sligo, Roscommon, Mayo, Galway, Clare
        };

        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log)
        {
            // get dates from database document
            UpdateDates update = DatabaseDates.ReadDatesDocument().Result;
            dateTimeLastUpdated = update.lastUpdate;
            dateTimeUpdatedTo = update.updatedTo;
            // check them against the websites dates to see if update available
            Console.WriteLine("checking for website update");
            bool updateAvailable = false;
            string url = "https://www.propertypriceregister.ie/website/npsra/pprweb.nsf/PPRDownloads?OpenForm";
            try
            {
                var Webget = new HtmlWeb();
                var doc = Webget.Load(url);
                var node = doc.DocumentNode.SelectSingleNode("//span[@id='LastUpdated']");
                if (node != null)
                {
                    var innerText = node.InnerText;
                    DateTime lastTimeWebsiteUpdated = Convert.ToDateTime(innerText);
                    if (!DateTime.Equals(lastTimeWebsiteUpdated, dateTimeLastUpdated))
                    {
                        updateAvailable = true;
                        Console.WriteLine("website update found");
                        dateTimeLastUpdated = lastTimeWebsiteUpdated; // change date of last website update
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: node for updated website date is empty");
                }
            }
            catch (WebException ex) // exception thrown if url not found
            {
                Console.WriteLine("URL not found exception: " + ex.Message);
            }
            catch (FormatException ex) // format is not datetime
            {
                Console.WriteLine("ERROR: node format does not match a DateTime format: " + ex.Message);
            }
            if (updateAvailable)
            {
                log.WriteLine("checked for update: found");
                // the main tasks of an update
                // download file
                DownloadFile dlf = new DownloadFile(dateTimeUpdatedTo.Year.ToString());
                string success = dlf.Download(); // returns null if no download or filename if success
                if (success != null)
                {
                    Console.WriteLine("PPR file downloaded successful");
                    List<Record> recordList = dlf.ConvertFile(success); // returns list of record objects for sorting
                    if (recordList != null || recordList.Count > 0) // list is null if has thrown exception or could be empty
                    {
                        CleanUpRecords cur = new CleanUpRecords();
                        List<AlteredRecord> alteredRecords = cur.CleanRecords(recordList); // clean records and return an altered record list
                        AddToDatabase atb = new AddToDatabase();
                        atb.AddList(alteredRecords, dateTimeUpdatedTo); // send list of to be broken down into counties and added to database documents
                        // change the dates in the update document to the new dates
                        if (everyPartSuccessful)
                        {
                            UpdateDates updateNew = new UpdateDates();
                            updateNew.lastUpdate = dateTimeLastUpdated;
                            updateNew.updatedTo = dateTimeUpdatedTo;
                            updateNew.id = "update_dates";
                            if (updateNew.updatedTo.Month == 12 && updateNew.updatedTo.Day == 31) // if 31st december push to 1st january
                            {
                                updateNew.updatedTo = updateNew.updatedTo.AddDays(1);
                            }
                            DatabaseDates.ModifyDatesDocument(updateNew);
                            log.WriteLine("success");
                        }
                        else
                        {
                            log.WriteLine("failed");
                        }
                    }
                }
            }
            else
            {
                log.WriteLine("checked for update: none");
            }
        }
    }
}
