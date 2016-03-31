// add altered records to proper database documents

using System;
using System.Collections.Generic;
using WebJobWorker.DatabaseConnections;
using WebJobWorker.Records;

namespace WebJobWorker.Data
{
    public class AddToDatabase
    {
        public List<AlteredRecord> templist;
        public List<AlteredRecord> templist1;
        public List<AlteredRecord> templist2;

        public AddToDatabase()
        {
            templist = new List<AlteredRecord>();
            templist1 = new List<AlteredRecord>();
            templist2 = new List<AlteredRecord>();
        }
        // list of counties
        public enum County
        {
            Kerry, Cork, Limerick, Tipperary, Waterford, Kilkenny, Wexford, Laois, Carlow, Kildare, Wicklow,
            Offaly, Dublin, Meath, Westmeath, Louth, Monaghan, Cavan, Longford, Donegal, Leitrim, Sligo, Roscommon, Mayo, Galway, Clare
        };

        // divide list up into counties and add to database document
        public void AddList(List<AlteredRecord> list, DateTime date) // date parameter is were updated to
        {
            DateTime lastDate = date;
            // find date the update file is updated to
            foreach (AlteredRecord ar in list)
            {
                // get the last date in the list so know were database has been updated to
                if (ar.SoldOn > lastDate)
                {
                    lastDate = ar.SoldOn;
                }
            }
            // set date updated too as last date in list
            Functions.dateTimeUpdatedTo = lastDate;
            // loop through every county 1 at a time
            for (County co = County.Kerry; co <= County.Clare; co++)
            {
                if (co == County.Dublin) // Dublin is too large and needs to be uploaded to database in months
                {
                    foreach (AlteredRecord ar in list)
                    {
                        if (ar.County.Equals(co.ToString()))
                        {
                            templist.Add(ar);
                        }
                    }
                    for(int i = date.Month; i < (lastDate.Month+1); i++)
                    {
                        foreach (AlteredRecord ar in templist)
                        {
                            if (ar.SoldOn.Month == i)
                            {
                                templist1.Add(ar);
                            }
                        }
                        // create database connection with list and county
                        DatabaseConnect dba = new DatabaseConnect(co.ToString(), templist1);
                        // update the document with new data
                        dba.ModifyDocumentDublin(date.Year.ToString(), i);
                        templist1.Clear();
                    }
                    // clear the lists
                    templist.Clear();
                    templist1.Clear();
                }
                else // rest of Ireland
                {
                    foreach (AlteredRecord ar in list)
                    {
                        if (ar.County.Equals(co.ToString()))
                        {
                            templist.Add(ar);
                        }
                    }
                    foreach (AlteredRecord ar in templist) // divide in 2
                    {
                        if (ar.SoldOn.Month == 1 || ar.SoldOn.Month == 2 || ar.SoldOn.Month == 3 || ar.SoldOn.Month == 4 ||
                            ar.SoldOn.Month == 5 || ar.SoldOn.Month == 6)
                        {
                            templist1.Add(ar);
                        }
                        else
                        {
                            templist2.Add(ar);
                        }
                    }
                    // if first 6 months of year
                    if (date.Month < 7)
                    {
                        DatabaseConnect db = new DatabaseConnect(co.ToString(), templist1);
                        db.ModifyDocumentBoggers(date.Year.ToString(), 'A');
                    }
                    // if some records for last 6 months of year
                    if (lastDate.Month > 6)
                    {
                        DatabaseConnect db2 = new DatabaseConnect(co.ToString(), templist2);
                        db2.ModifyDocumentBoggers(date.Year.ToString(), 'B');
                    }
                    // clear temp lists
                    templist.Clear();
                    templist1.Clear();
                    templist2.Clear();
                }
            }
            // everything went well so make true
            Functions.everyPartSuccessful = true;
        }
    }
}
