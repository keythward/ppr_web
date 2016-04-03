﻿// model class for the search page

using ppr_web.DatabaseConn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ppr_web.Models
{
    public class SearchDB
    {

        public string County { get; set; }
        public string Year { get; set; }
        [DisplayName("Max Property Value (€)")]
        [Range(0, Double.MaxValue, ErrorMessage = "must be positive value")]
        public double PriceValue { get; set; }
        [DisplayName("Dwelling Type")]
        public string Dwelling { get; set; }
        [DisplayName("Market Price Value Met")]
        public string MarketPrice { get; set; }
        public string PostCode { get; set; }
        [DisplayName("Dates Range")]
        public string Dates { get; set; }

        // list of counties
        public static string[] Counties
        {
            get
            {
                return new string[] { "Kerry", "Cork", "Limerick", "Tipperary", "Waterford", "Kilkenny", "Wexford", "Laois",
                                      "Carlow", "Kildare", "Wicklow","Offaly", "Dublin", "Meath", "Westmeath", "Louth",
                                      "Monaghan", "Cavan", "Longford", "Donegal", "Leitrim", "Sligo", "Roscommon", "Mayo",
                                      "Galway", "Clare" };
            }
        }

        // list of years
        public static string[] Years
        {
            get
            {
                return new string[] { "2010", "2011", "2012", "2013", "2014", "2015", "2016" };
            }
        }

        // property dwelling types
        public static string[] DwellingTypes
        {
            get
            {
                return new string[] { "All Types", "New Property", "Second Hand Property" };
            }
        }

        // was the property sold at market price or below it
        public static string[] MarketPriceDecision
        {
            get
            {
                return new string[] { "Yes and No", "Yes", "No" };
            }
        }

        // list of postal codes for dublin
        public static string[] PostalCodes
        {
            get
            {
                return new string[] { "All","1", "2","3","4","5","6","6w","7","8","9","10","11","12","13","14","15","16","17","18",
                                      "20","22","24","county dublin" };
            }
        }

        // dates range for all but dublin
        public static string[] DatesBetween
        {
            get
            {
                return new string[] { "All Year", "First 6 Months", "Last 6 Months" };
            }
        }

        // dates range for only dublin
        public static string[] DatesBetweenDublin
        {
            get
            {
                return new string[] { "All Year", "January", "February", "March", "April", "May", "June", "July", "August", "September",
                                        "October", "November", "December" };
            }
        }

        // return a list of records for the chosen search county and year
        public List<ListObject> GetLists()
        {
            List<ListObject> list = new List<ListObject>();
            string doc_id = "";
            DBRecord test = null;
            if (County.Equals("Dublin"))
            {
                if (Dates.Equals("All Year"))
                {
                    doc_id = County + Year + "_1";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                    doc_id = County + Year + "_2";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_3";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_4";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_5";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_6";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_7";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_8";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_9";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_10";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_11";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = County + Year + "_12";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                else if (Dates.Equals("January"))
                {
                    doc_id = County + Year + "_1";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("February"))
                {
                    doc_id = County + Year + "_2";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("March"))
                {
                    doc_id = County + Year + "_3";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("April"))
                {
                    doc_id = County + Year + "_4";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("May"))
                {
                    doc_id = County + Year + "_5";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("June"))
                {
                    doc_id = County + Year + "_6";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("July"))
                {
                    doc_id = County + Year + "_7";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("August"))
                {
                    doc_id = County + Year + "_8";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("September"))
                {
                    doc_id = County + Year + "_9";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("October"))
                {
                    doc_id = County + Year + "_10";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (Dates.Equals("November"))
                {
                    doc_id = County + Year + "_11";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else // december
                {
                    doc_id = County + Year + "_12";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
            }
            else // rest of ireland (not dublin)
            {
                if (Dates.Equals("All Year"))
                {
                    doc_id = County + Year + "_A"; // first part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                    doc_id = County + Year + "_B"; // second part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                }
                else if (Dates.Equals("First 6 Months"))
                {
                    doc_id = County + Year + "_A";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else // last 6 months
                {
                    doc_id = County + Year + "_B";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
            }
            return list;
        }

        // sort the list based on the refines from the page choices
        public List<ListObjectSorted> SortList(List<ListObject> listIn)
        {
            List<ListObjectSorted> listOut = new List<ListObjectSorted>();
            List<ListObjectSorted> temp = new List<ListObjectSorted>();

            // change listin to listout
            foreach (var r in listIn)
            {
                ListObjectSorted newObj = new ListObjectSorted();
                newObj.Address = r.Address;
                newObj.Description = r.Description.ToString(); // char to string
                if (newObj.Description.Equals("N")) // expand out string
                {
                    newObj.Description = "New";
                }
                else
                {
                    newObj.Description = "Second Hand";
                }
                newObj.NotFullMP = r.NotFullMP.ToString(); // char to string
                if (newObj.NotFullMP.Equals("N")) // expand out string
                {
                    newObj.NotFullMP = "No";
                }
                else
                {
                    newObj.NotFullMP = "Yes";
                }
                newObj.PostCode = r.PostCode;
                newObj.Price = r.Price;
                newObj.SoldOn = r.SoldOn;
                listOut.Add(newObj);
            }
            // if new property dwelling
            if (Dwelling.Equals("New Property"))
            {
                foreach (var r in listOut)
                {
                    if (r.Description.Equals("New"))
                    {
                        temp.Add(r);
                    }
                }
                listOut.Clear();
                listOut.AddRange(temp);
                temp.Clear();
            }
            // if second hand property dwelling
            if (Dwelling.Equals("Second Hand Property"))
            {
                foreach (var r in listOut)
                {
                    if (r.Description.Equals("Second Hand"))
                    {
                        temp.Add(r);
                    }
                }
                listOut.Clear();
                listOut.AddRange(temp);
                temp.Clear();
            }
            // if market price yes
            if (MarketPrice.Equals("Yes"))
            {
                foreach (var r in listOut)
                {
                    if (r.NotFullMP.Equals("Yes"))
                    {
                        temp.Add(r);
                    }
                }
                listOut.Clear();
                listOut.AddRange(temp);
                temp.Clear();
            }
            // if market price no
            if (MarketPrice.Equals("No"))
            {
                foreach (var r in listOut)
                {
                    if (r.NotFullMP.Equals("No"))
                    {
                        temp.Add(r);
                    }
                }
                listOut.Clear();
                listOut.AddRange(temp);
                temp.Clear();
            }
            // if county is dublin sort by postal code
            if (County.Equals("Dublin"))
            {
                if (!PostCode.Equals("All"))
                {
                    string pc = "";
                    if (PostCode.Equals("county dublin"))
                    {
                        pc = PostCode;
                        foreach (var r in listOut)
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
                        foreach (var r in listOut)
                        {
                            if (r.PostCode.Equals(pc))
                            {
                                temp.Add(r);
                            }
                        }
                    }
                    listOut.Clear();
                    listOut.AddRange(temp);
                    temp.Clear();
                }
            }
            // take out any with price greater than max price
            foreach (var r in listOut)
            {
                if (r.Price <= PriceValue)
                {
                    temp.Add(r);
                }
            }
            listOut.Clear();
            listOut.AddRange(temp);
            temp.Clear();
            // return list
            return listOut;
        }

        // format the list before returning it to the table...look better in table
        public List<ListObjectFormatted> FormatList(List<ListObjectSorted> list)
        {
            // sort dates in order
            list.Sort((x, y) => DateTime.Compare(x.SoldOn, y.SoldOn));
            List<ListObjectFormatted> listForm = new List<ListObjectFormatted>();
            foreach (var r in list)
            {
                ListObjectFormatted obj = new ListObjectFormatted();
                // format price
                int noDecimal = (int)r.Price;
                string price = noDecimal.ToString("c0", CultureInfo.CurrentCulture);
                obj.Price = price;
                // format date
                obj.SoldOn = r.SoldOn.ToString("dd/MM/yyyy");
                // format address
                string add = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(r.Address.ToLower());
                obj.Address = Regex.Replace(add, "(?<=,)(?!\\s)", " ");
                // the rest
                obj.Description = r.Description;
                obj.NotFullMP = r.NotFullMP;
                obj.PostCode = r.PostCode;
                listForm.Add(obj);
            }
            return listForm;
        }


        public List<ListObjectFormatted> FetchResults
        {
            get
            {
                List<ListObject> list = new List<ListObject>();
                List<ListObjectSorted> listSorted = new List<ListObjectSorted>();
                List<ListObjectFormatted> listForm = new List<ListObjectFormatted>();
                list = GetLists();
                listSorted = SortList(list);
                listForm = FormatList(listSorted);
                return listForm;
            }
        }
    }

    // object to populate sorted list
    // this is needed because the database list (ListObject) has chars where need strings
    public class ListObjectSorted
    {
        public DateTime SoldOn { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public double Price { get; set; }
        public string NotFullMP { get; set; } // was char
        public string Description { get; set; }// was char
    }

    // object to populate filtered list
    // this is needed because the sorted list (ListObjectSorted) needs to be formated for display 
    public class ListObjectFormatted
    {
        public string SoldOn { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Price { get; set; }
        public string NotFullMP { get; set; } // was char
        public string Description { get; set; }// was char
    }
}
