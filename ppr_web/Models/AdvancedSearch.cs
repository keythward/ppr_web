// model object for advanced search (integrated mapping) page

using ppr_web.DatabaseConn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ppr_web.Models
{
    public class AdvancedSearch
    {
        public string API_KEY = "AIzaSyC1YdK2EZU8p7EA760nPD06iL39rOdA5kg";
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Area { get; set; }
        static int monthCount;
        List<double> medianValues;
        Dictionary<string, List<ListObject>> dictionaryList;

        // default constructor needed to calculate what month database is updated to on current year
        public AdvancedSearch()
        {
            monthCount = getMonthCount(DateTime.Now.Year.ToString());
            dictionaryList = new Dictionary<string, List<ListObject>>();
            medianValues = new List<double>();
        }

        // a dictionary of years and a list of data objects for county/postcode if dublin/area if given 1
        public void GetLists()
        {
            string doc_id = "";
            DBRecord test = null;
            int thisMonthCount = monthCount;
            int yearCount = 6;
            if (County.Equals("Dublin"))
            {
                for(int i = 0; i <7; i++) // all 12 months of year
                {
                    List<ListObject> list = new List<ListObject>();
                    List<ListObject> temp = new List<ListObject>();
                    string Year = "201" + yearCount.ToString();
                    if (thisMonthCount >= 1)
                    {
                        doc_id = County + Year + "_1";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list = test.records;
                        test = null;
                    }
                    if (thisMonthCount >= 2)
                    {
                        doc_id = County + Year + "_2";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 3)
                    {
                        doc_id = County + Year + "_3";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 4)
                    {
                        doc_id = County + Year + "_4";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 5)
                    {
                        doc_id = County + Year + "_5";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 6)
                    {
                        doc_id = County + Year + "_6";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 7)
                    {
                        doc_id = County + Year + "_7";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 8)
                    {
                        doc_id = County + Year + "_8";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 9)
                    {
                        doc_id = County + Year + "_9";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 10)
                    {
                        doc_id = County + Year + "_10";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 11)
                    {
                        doc_id = County + Year + "_11";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    if (thisMonthCount >= 12)
                    {
                        doc_id = County + Year + "_12";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                        test = null;
                    }
                    // take out postcode
                    if (!PostCode.Equals("All"))
                    {
                        string pc = "";
                        if (PostCode.Equals("county dublin"))
                        {
                            pc = PostCode;
                            foreach (var r in list)
                            {
                                if (r.PostCode.Equals(pc))
                                {
                                    temp.Add(r);
                                }
                            }
                        }
                        else
                        {
                            pc = "dublin " + PostCode;
                            foreach (var r in list)
                            {
                                if (r.PostCode.Equals(pc))
                                {
                                    temp.Add(r);
                                }
                            }
                        }
                        list.Clear();
                        list.AddRange(temp);
                        temp.Clear();
                    }
                    // take out area
                    try
                    {
                        if (!Area.Equals(""))
                        {
                            temp = list.Where(m => m.Address.Contains(Area.ToLower())).ToList();
                            list.Clear();
                            list.AddRange(temp);
                            temp.Clear();
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                    // add to dictionary
                    dictionaryList.Add(Year, list);
                    yearCount--;
                    thisMonthCount = 12;
                }
            }
            else // rest of ireland (not dublin)
            {
                for (int i = 0; i < 7; i++)
                {
                    List<ListObject> list = new List<ListObject>();
                    List<ListObject> temp = new List<ListObject>();
                    string Year = "201" + yearCount.ToString();
                    if ((thisMonthCount <= 6)||(thisMonthCount>=7))// first 6 months
                    {
                        doc_id = County + Year + "_A";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list = test.records;
                        test = null;
                    }
                    if ((thisMonthCount >= 7))// last 6 months
                    {
                        doc_id = County + Year + "_B";
                        test = DatabaseConnect2.ReadDocument(doc_id);
                        list.AddRange(test.records);
                    }
                    // take out area
                    try
                    {
                        if (!Area.Equals(""))
                        {
                            temp = list.Where(m => m.Address.Contains(Area.ToLower())).ToList();
                            list.Clear();
                            list.AddRange(temp);
                            temp.Clear();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    // add to dictionary
                    dictionaryList.Add(Year, list);
                    yearCount--;
                    thisMonthCount = 12;
                }
            }
            getMedians();
        }

        // get median values for every year
        public void getMedians()
        {
            foreach (KeyValuePair<string, List<ListObject>> entry in dictionaryList)
            {
                entry.Value.OrderBy(i => i.Price).ToList();
                double median = entry.Value.ElementAt((entry.Value.Count - 1) / 2).Price;
                medianValues.Add(median);
            }
        }

        // calculate month count needed for search
        // if search is this year (eg:2016) get month that database is updated to
        // if not this year return 12 (every month)
        public static int getMonthCount(string year)
        {
            int month;
            // get update date from database if this year
            if (DateTime.Now.Year.ToString().Equals(year))
            {
                month = DatabaseConnect2.ReadUpdateDate();
            }
            else
            {
                month = 12;
            }
            return month;
        }

        // return the dictionary
        public Dictionary<string, List<ListObject>> getDictionary
        {
            get
            {
                return dictionaryList;
            }
        }

        // return the median list
        public List<double> getMedianList
        {
            get
            {
                return medianValues;
            }
        }
    }
}
