// model class for combined search page

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ppr_web.DatabaseConn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ppr_web.Models
{
    public class CombinedSearch
    {
        public IList<Line> lineList { get; set; }

        public CombinedSearch()
        {
            lineList = new List<Line>();
            lineList.Add(new Line());
        }

        // a chart for all line objects
        public List<Highcharts> createChartAll
        {
            get
            {
                List<Highcharts> chartList = new List<Highcharts>();
                List < List < ListObject >> allLists = new List<List<ListObject>>();
                List<object> min = new List<object>();
                List<object> max = new List<object>();
                List<object> median = new List<object>();
                // get data needed for charts
                foreach(Line line in lineList)
                {
                    List<ListObject> list = new List<ListObject>();
                    List<ListObject> temp = new List<ListObject>();
                    list = GetLists(line.County,line.Year,line.Date);
                    // if dublin take out postcode picked
                    if (line.County.Equals("Dublin"))
                    {
                        if (!line.PostCode.Equals("All"))
                        {
                            string pc = "";
                            if (line.PostCode.Equals("county dublin"))
                            {
                                pc = line.PostCode;
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
                                pc = "dublin " + line.PostCode;
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
                    }
                    // take out every entry that matches the area picked
                    try
                    {
                        if (!line.Area.Equals(""))
                        {
                            temp = list.Where(i => i.Address.Contains(line.Area.ToLower())).ToList();
                            list.Clear();
                            list.AddRange(temp);
                            temp.Clear();
                        }
                    }
                    catch(Exception e)
                    {

                    }
                    // add to list
                    allLists.Add(list);
                }
                // calculate data
                foreach (List<ListObject> list in allLists)
                {
                    double maxValue = list.Max(i => i.Price);
                    max.Add(maxValue);
                    double minValue = list.Min(i => i.Price);
                    min.Add(minValue);
                    // get median
                    list.OrderBy(i => i.Price).ToList();
                    double medianValue = list.ElementAt((list.Count-1)/2).Price;
                    median.Add(medianValue);
                }
                // create chart for min,max,median values
                Highcharts chart = new Highcharts("chart1")
                    .SetXAxis(new XAxis
                    {
                        Categories = new[] { "Line 1", "Line 2" }
                    })
                    .SetSeries(new[]
                           {
                               new Series
                                {
                                    Type = ChartTypes.Column,
                                    Name = "Minimum",
                                    Data = new Data(min.ToArray())
                                },
                                new Series
                                {
                                    Type = ChartTypes.Column,
                                    Name = "Maximum",
                                    Data = new Data(max.ToArray())
                                },
                                new Series
                                {
                                    Type = ChartTypes.Column,
                                    Name = "Median",
                                    Data = new Data(median.ToArray())
                                }
                           });



                chartList.Add(chart);
                return chartList;
            }
        }

        // return a list of records for the chosen search county and year
        public List<ListObject> GetLists(string county,string year,string dates)
        {
            List<ListObject> list = new List<ListObject>();
            string doc_id = "";
            DBRecord test = null;
            if (county.Equals("Dublin"))
            {
                if (dates.Equals("All Year"))
                {
                    doc_id = county + year + "_1";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                    doc_id = county + year + "_2";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_3";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_4";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_5";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_6";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_7";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_8";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_9";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_10";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_11";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                    doc_id = county + year + "_12";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                else if (dates.Equals("January"))
                {
                    doc_id = county + year + "_1";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("February"))
                {
                    doc_id = county + year + "_2";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("March"))
                {
                    doc_id = county + year + "_3";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("April"))
                {
                    doc_id = county + year + "_4";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("May"))
                {
                    doc_id = county + year + "_5";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("June"))
                {
                    doc_id = county + year + "_6";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("July"))
                {
                    doc_id = county + year + "_7";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("August"))
                {
                    doc_id = county + year + "_8";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("September"))
                {
                    doc_id = county + year + "_9";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("October"))
                {
                    doc_id = county + year + "_10";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else if (dates.Equals("November"))
                {
                    doc_id = county + year + "_11";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else // december
                {
                    doc_id = county + year + "_12";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
            }
            else // rest of ireland (not dublin)
            {
                if (dates.Equals("All Year"))
                {
                    doc_id = county + year + "_A"; // first part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                    doc_id = county + year + "_B"; // second part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                }
                else if (dates.Equals("First 6 Months"))
                {
                    doc_id = county + year + "_A";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
                else // last 6 months
                {
                    doc_id = county + year + "_B";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                }
            }
            return list;
        }

    }

    // line object
    public class Line
    {
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Area { get; set; }
        [DisplayName("Date Range")]
        public string Date { get; set; }
        public string Year { get; set; }
    }
}
