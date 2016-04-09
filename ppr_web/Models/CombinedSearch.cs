// model class for combined search page

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ppr_web.DatabaseConn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ppr_web.Models
{
    public class CombinedSearch
    {
        public IList<Line> lineList { get; set; }
        [DisplayName("check box if you do not want properties that did not sell for the market price included in the search")]
        public bool takeOutNoMarket { get; set; }

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
                List<string> linesString = new List<string>();
                // get data needed for charts
                foreach (Line line in lineList)
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
                    // if market price check box clicked remove from list
                    if (takeOutNoMarket)
                    {
                        temp = list.Where(i => i.NotFullMP.Equals('N')).ToList();
                        list.Clear();
                        list.AddRange(temp);
                        temp.Clear();
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
                        Console.WriteLine(e.ToString());
                    }
                    // add to list
                    allLists.Add(list);
                    // get line details for charts
                    string lineString = line.County;
                    try
                    {
                        if (!line.Area.Equals(""))
                        {
                            lineString += ", ";
                            lineString += line.Area;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    lineString += ", ";
                    lineString += line.Year;
                    linesString.Add(lineString);
                }
                // calculate data
                List<object> min = new List<object>();
                List<object> max = new List<object>();
                List<object> median = new List<object>();
                List<object> newDwelling = new List<object>();
                List<object> secondDwelling = new List<object>();
                foreach (List<ListObject> list in allLists)
                {
                    // max
                    double maxValue = list.Max(i => i.Price);
                    max.Add(maxValue);
                    // min
                    double minValue = list.Min(i => i.Price);
                    min.Add(minValue);
                    // median
                    list.OrderBy(i => i.Price).ToList();
                    double medianValue = list.ElementAt((list.Count-1)/2).Price;
                    median.Add(medianValue);
                    // new/second dwelling
                    // market yes/no
                    int newD = 0;
                    int secD = 0;
                    foreach(var l in list)
                    {
                        if (l.Description.Equals('N'))
                        {
                            newD++;
                        }
                        else
                        {
                            secD++;
                        }
                    }
                    newDwelling.Add(newD);
                    secondDwelling.Add(secD);
                }
                // create chart for min,max,median values
                Highcharts chart = new Highcharts("chart1")
                    .InitChart(new Chart { Type = ChartTypes.Column, Inverted = true })
                    .SetXAxis(new XAxis
                    {
                        Categories = linesString.ToArray()
                    })
                    .SetSeries(new[]
                           {
                               new Series
                                {
                                    Name = "Minimum",
                                    Data = new Data(min.ToArray())
                                },
                                new Series
                                {
                                    Name = "Maximum",
                                    Data = new Data(max.ToArray())
                                },
                                new Series
                                {
                                    Name = "Median",
                                    Data = new Data(median.ToArray())
                                }
                                
                                
                           });
                Highcharts chart2 = new Highcharts("chart")
                    .InitChart(new Chart { Type = ChartTypes.Column })
                    .SetTitle(new Title { Text = "New and Second Hand Dwellings in Chosen Region" })
                    .SetXAxis(new XAxis
                    {
                        Categories = linesString.ToArray()
                    })
                    .SetYAxis(new YAxis
                    {
                        Title = new YAxisTitle { Text = "Percent" }
                    })
                    .SetTooltip(new Tooltip { Formatter = "function() { return Highcharts.numberFormat(this.percentage, 1) +'% ('+ Highcharts.numberFormat(this.y, 0, ',') +' dwellings)'; }" })
                    .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Stacking = Stackings.Percent } })
                    .SetSeries(new[]
                    {
                        new Series
                                {
                                    Name = "new",
                                    Data = new Data(newDwelling.ToArray())
                                },
                        new Series
                                {
                                    Name = "second",
                                    Data = new Data(secondDwelling.ToArray())
                                }
                    });
                

                chartList.Add(chart2);
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
