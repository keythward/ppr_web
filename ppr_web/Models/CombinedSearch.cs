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
using System.Globalization;
using System.Linq;
namespace ppr_web.Models
{
    public class CombinedSearch
    {
        // list of dynamically created line objects
        public IList<Line> lineList { get; set; }
        // used for check box on view for market value being included in search
        [DisplayName("check box if you do not want properties that did not sell for the market price included in the search")]
        public bool takeOutNoMarket { get; set; }
        // different lists required for charts
        List<Highcharts> chartList;
        List<List<ListObject>> allLists;
        List<string> linesString;
        List<object> median;
        List<object> newDwelling;
        List<object> secondDwelling;
        List<object> newPercentages;
        List<object> secPercentages;
        List<List<object>> newValues;
        List<List<object>> secValues;
        List<object> finalNewData;
        List<object> finalSecData;
        Series[] objectArrayForListChart;
        string[] categoriesDrilldown = { "Minimum", "Median", "Maximum" };
        Dictionary<string, List<double>> areaDictionary;
        List<string> dictionaryListString;
        List<object> dictionaryListDouble;
        List<List<string>> listOfDictionaryListsString;
        List<List<object>> listOfDictionaryListsDouble;
        bool[] areaPicked;
        // constructor needed to initialize for dynamically created line objects on view
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
                string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                // list to hold all charts to be returned to view
                chartList = new List<Highcharts>();
                // get data needed for charts from returned lists
                getChartData();
                // calculate data needed for charts
                calculateChartData();
                // line chart with median values per month per region
                Highcharts lineChart = new Highcharts("lineChart")
                    .InitChart(new Chart { Type = ChartTypes.Line })
                    .SetOptions(new GlobalOptions { Lang = new DotNet.Highcharts.Helpers.Lang().SetAndUseCulture("en-IE"), Global = new Global { UseUTC = false } })
                    .SetTitle(new Title { Text = "Median Property Prices Per Month Per Region" })
                    .SetCredits(new Credits { Enabled = false })
                    .SetSubtitle(new Subtitle { Text = "(Median Property Price For The Whole Year)" })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Median Property Values" } })
                    .SetTooltip(new Tooltip { Formatter = "function() { return this.x +'<br/>'+ '<b>'+'€'+parseFloat(this.y.toFixed(0)).toLocaleString()+'</b>';}" })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels { Enabled = true,Color=Color.Black } }
                    })
                    .SetXAxis(new XAxis
                    {
                        Categories = months
                    })
                    .SetSeries(objectArrayForListChart);
                chartList.Add(lineChart);
                // new/second hand dwellings percentage per region
                // drilldown for min/max/median of column
                Data newData = new Data(finalNewData.ToArray());
                Data secData = new Data(finalSecData.ToArray());
                const string NAME = "New/Second Hand Properties";
                Highcharts chart = new Highcharts("chart")
                    .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                    .SetCredits(new Credits { Enabled = false })
                    .SetOptions(new GlobalOptions { Lang = new DotNet.Highcharts.Helpers.Lang().SetAndUseCulture("en-IE"), Global = new Global { UseUTC = false } })
                    .SetTitle(new Title { Text = "Percentage of New and Second Hand Properties per Region" })
                    .SetSubtitle(new Subtitle { Text = "Click the columns to view min/median/max values. Click again to view percentages." })
                    .SetXAxis(new XAxis { Categories = linesString.ToArray() })
                    .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Percentage" } })
                    .SetLegend(new Legend { Enabled = false })
                    .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn
                        {
                            Cursor = Cursors.Pointer,
                            Point = new PlotOptionsColumnPoint { Events = new PlotOptionsColumnPointEvents { Click = "ColumnPointClick" } },
                            DataLabels = new PlotOptionsColumnDataLabels
                            {
                                Enabled = true,
                                Color = Color.Black,
                                Formatter = "labelFormatter",
                                Style = "fontWeight: 'bold'"
                            }
                        }
                    })
                    .SetSeries(new[]
                                           {new Series
                                            {
                                                Name = "new dwellings",
                                                Data = newData,
                                                Color = Color.White
                                            },
                                            new Series
                                            {
                                                Name = "second hand dwellings",
                                                Data = secData,
                                                Color = Color.White
                                            }
                                           })
                    .SetExporting(new Exporting { Enabled = false })
                    .AddJavascripFunction(
                        "TooltipFormatter",
                        @"var point = this.point, s = '';
                                  if (point.drilldown) {
                                    s += '<b>'+this.y +'% of '+ this.series.name +' in region</b><br/>' + 'Click to view min/max/median values';
                                  } else {
                                    s += this.x +': €<b>'+ this.y +' value</b><br/>'+'Click to return to region percentages';
                                  }
                                  return s;"
                    )
                    .AddJavascripFunction(
                        "labelFormatter",
                        @"var point = this.point, s = this.y +'%';
                                  if (point.drilldown) {
                                    
                                  } else {
                                    s = '€' + this.y;
                                  }
                                  return s;
                        "
                    )
                    .AddJavascripFunction(
                        "ColumnPointClick",
                        @"var drilldown = this.drilldown;
                                  if (drilldown) { // drill down
                                    setChart(drilldown.name, drilldown.categories, drilldown.data.data, drilldown.color);
                                  } else { // restore
                                    chart.xAxis[0].setCategories(categories);
                                    while(chart.series.length > 0) {
                                    chart.series[0].remove(true);
                                    }
                                    for (var i = 0; i < data.length ; i++)
                                    {
                                        
                                         chart.addSeries({
                                         name: name,
                                         data: data[i].data,
                                         color:'white'
                                         });
                                    }
                                  }"
                    )
                    .AddJavascripFunction(
                        "setChart",
                        @"chart.xAxis[0].setCategories(categories);
                            chart.yAxis[0].setTitle({ text: 'Property Values' });
                                  while (chart.series.length > 0) {
                      chart.series[0].remove(true);
                  }
                                  chart.addSeries({
                                     name: name,
                                     data: data,
                                     color: color || 'white'
                                  });",
                        "name", "categories", "data", "color"
                    )
                    .AddJavascripVariable("colors", "Highcharts.getOptions().colors")
                    .AddJavascripVariable("name", "'{0}'".FormatWith(NAME))
                    .AddJavascripVariable("categories", JsonSerializer.Serialize(linesString.ToArray()))
                    .AddJavascripVariable("data", JsonSerializer.Serialize(new[] { newData, secData }));
                chartList.Add(chart);
                // break down of every line into its areas with median of each
                // bar charts
                int barCount = 1;
                string barName = "";
                for(int i=0;i<listOfDictionaryListsString.Count;i++)
                {
                    barName ="barChart" + barCount.ToString();
                    Highcharts chartBar = new Highcharts(barName)
                    .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                    .SetCredits(new Credits { Enabled = false })
                    .SetOptions(new GlobalOptions { Lang = new DotNet.Highcharts.Helpers.Lang().SetAndUseCulture("en-IE"), Global = new Global { UseUTC = false } })
                    .SetTitle(new Title { Text = "Areas within the region" })
                    .SetSubtitle(new Subtitle { Text = linesString[i] })
                    .SetXAxis(new XAxis
                    {
                        Categories = listOfDictionaryListsString.ElementAt(i).ToArray(),
                        Title = new XAxisTitle { Text = "Region Areas" },
                        Labels = new XAxisLabels
                        {
                            Rotation = 45
                        }
                    })
                    .SetYAxis(new YAxis
                    {
                        AllowDecimals = false,
                        LineWidth=5,
                        MinPadding=5,
                        StartOnTick=true,
                        
                        Title = new YAxisTitle
                        {
                            Text = "Median Property Price Values",
                            Align = AxisTitleAligns.Middle
                        }
                    })
                    .SetTooltip(new Tooltip { Formatter = "function() { return this.x + '</br>' + 'Median Price:€'+this.y; }" })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn
                        {
                            DataLabels = new PlotOptionsColumnDataLabels { Enabled = true,Rotation=90,Crop=false,Overflow="none",
                                Padding =0,Inside=true,Style= "fontWeight: 'bold'",VerticalAlign=VerticalAligns.Top,
                                Color = Color.Black,
                                Align = HorizontalAligns.Right,Formatter= "function(){return '€'+parseFloat(this.y.toFixed(0)).toLocaleString();}"
                            }
                        }
                    })
                    
                    .SetSeries(new[]
                    {
                        new Series { Name="Median Values",Data = new Data(listOfDictionaryListsDouble.ElementAt(i).ToArray()) }

                    });
                    chartList.Add(chartBar);
                    barCount++;
                }
                

                return chartList;
            }
        }

        // return a list of records for the chosen search county and year
        public List<ListObject> GetLists(string county,string year)
        {
            List<ListObject> list = new List<ListObject>();
            string doc_id = "";
            DBRecord test = null;
            // depending on what months are required based on monthCount
            int monthCount = getMonthCount(year);
            if (county.Equals("Dublin"))
            {
                if (monthCount >= 1)
                {
                    doc_id = county + year + "_1";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                }
                if (monthCount >= 2)
                {
                    doc_id = county + year + "_2";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 3)
                {
                    doc_id = county + year + "_3";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 4)
                {
                    doc_id = county + year + "_4";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 5)
                {
                    doc_id = county + year + "_5";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 6)
                {
                    doc_id = county + year + "_6";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 7)
                {
                    doc_id = county + year + "_7";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 8)
                {
                    doc_id = county + year + "_8";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 9)
                {
                    doc_id = county + year + "_9";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 10)
                {
                    doc_id = county + year + "_10";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 11)
                {
                    doc_id = county + year + "_11";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
                if (monthCount >= 12)
                {
                    doc_id = county + year + "_12";
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                    test = null;
                }
            }
            else // rest of ireland (not dublin)
            {
                if ((monthCount <= 6)||(monthCount == 12))
                {
                    doc_id = county + year + "_A"; // first part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list = test.records;
                    test = null;
                }
                if (monthCount >= 7)
                {
                    doc_id = county + year + "_B"; // second part of year
                    test = DatabaseConnect2.ReadDocument(doc_id);
                    list.AddRange(test.records);
                }
            }
            return list;
        }

        // get the data needed for charts
        public void getChartData()
        {
            linesString = new List<string>();
            allLists = new List<List<ListObject>>();
            areaPicked = new bool[lineList.Count];
            int areaCounter = 0;
            foreach (Line line in lineList)
            {
                List<ListObject> list = new List<ListObject>();
                List<ListObject> temp = new List<ListObject>();
                list = GetLists(line.County, line.Year);
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
                        areaPicked[areaCounter] = true;
                    }
                }
                catch (Exception e)
                {
                    areaPicked[areaCounter] = false;
                }
                // add to list
                allLists.Add(list);

                // get line details for charts (county,area,year)
                string lineString = line.County;
                if ((line.County.Equals("dublin"))&&(!line.PostCode.Equals("All")))
                {
                    lineString += " ";
                    lineString += line.PostCode;
                }
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
                lineString += line.Year+" ";
                linesString.Add(lineString);
                areaCounter++;
            }
        }

        // calculate all data that will be needed by the different charts
        public void calculateChartData()
        {
            median = new List<object>();
            objectArrayForListChart = new Series[allLists.Count];
            int counter = 0;
            newDwelling = new List<object>();
            secondDwelling = new List<object>();
            newPercentages = new List<object>();
            secPercentages = new List<object>();
            newValues = new List<List<object>>();
            secValues = new List<List<object>>();
            finalNewData = new List<object>();
            finalSecData = new List<object>();
            listOfDictionaryListsString = new List<List<string>>();
            listOfDictionaryListsDouble = new List<List<object>>();
            foreach (List<ListObject> list in allLists)
            {
                areaDictionary = new Dictionary<string, List<double>>();
                dictionaryListString = new List<string>();
                dictionaryListDouble = new List<object>();
                // data for line chart
                List<ListObject> temp = new List<ListObject>();
                double medianValue = 0;
                // get whole year median
                list.OrderBy(i => i.Price).ToList();
                double medianValueWholeYear = list.ElementAt((list.Count - 1) / 2).Price;
                // get month by month median
                for (int i = 0; i < 12; i++)
                {
                    temp = list.Where(m => m.SoldOn.Month == (i+1)).ToList();
                    if (temp.Count == 0)
                    {
                        medianValue = 0;
                    }
                    else
                    {
                        temp.OrderBy(m => m.Price).ToList();
                        medianValue = temp.ElementAt((temp.Count - 1) / 2).Price;
                    }
                    median.Add(medianValue);
                    temp.Clear();
                }
                objectArrayForListChart[counter] = new Series { Name = linesString[counter]+"  ("+String.Format(new CultureInfo("en-IE"), "{0:C}",(int)Math.Round(medianValueWholeYear,0))+")", Data = new Data(median.ToArray()) };
                median.Clear();
                // data for new/second chart
                // new/second dwelling AND // min,max,median per new/second column
                temp = list.Where(i => i.Description.Equals('N')).ToList();
                newDwelling.Add(temp.Count);
                double newPer = ((double)temp.Count / (double)list.Count) * 100;
                newPercentages.Add(newPer);
                double minValueNew = temp.Min(i => i.Price);
                double maxValueNew = temp.Max(i => i.Price);
                temp.OrderBy(i => i.Price).ToList();
                double medianValueNew = temp.ElementAt((temp.Count - 1) / 2).Price;
                List<object> newTemp = new List<object>();
                newTemp.Add(Math.Round(minValueNew));
                newTemp.Add(Math.Round(medianValueNew));
                newTemp.Add(Math.Round(maxValueNew));
                newValues.Add(newTemp);
                temp.Clear();

                temp = list.Where(i => i.Description.Equals('S')).ToList();
                secondDwelling.Add(temp.Count);
                double secPer = ((double)temp.Count / (double)list.Count) * 100;
                secPercentages.Add(secPer);
                double minValueSec = temp.Min(i => i.Price);
                double maxValueSec = temp.Max(i => i.Price);
                temp.OrderBy(i => i.Price).ToList();
                double medianValueSec = temp.ElementAt((temp.Count - 1) / 2).Price;
                List<object> secTemp = new List<object>();
                secTemp.Add(Math.Round(minValueSec));
                secTemp.Add(Math.Round(medianValueSec));
                secTemp.Add(Math.Round(maxValueSec));
                secValues.Add(secTemp);
                temp.Clear();

                // dictionary for break down of every area with price values
                foreach (ListObject l in list)
                {
                    // get the area (key)
                    string[] breakDown = l.Address.Split(',');
                    string key;
                    try
                    {
                        // if true area was given so take second last line
                        // if false no area given so take last line
                        bool well = areaPicked[counter];
                        if (well)
                        {
                            key = breakDown[breakDown.Length - 2];
                        }
                        else
                        {
                            key = breakDown[breakDown.Length - 1];
                        }
                        // add to dictionary
                        if (!areaDictionary.ContainsKey(key))
                        {
                            //add key
                            areaDictionary.Add(key, new List<double>());
                        }
                        areaDictionary[key].Add(l.Price);
                    }
                    catch (Exception e)
                    {

                    }
                }
                // list with key and value (median value of key)
                // if list for key is smaller than 3 entries ignore it (spelling mistakes)
                foreach (KeyValuePair<string, List<double>> entry in areaDictionary)
                {
                    if (entry.Value.Count > 2)
                    {
                        entry.Value.OrderBy(i => i).ToList();
                        double median= entry.Value.ElementAt((entry.Value.Count - 1) / 2);
                        dictionaryListDouble.Add(median);
                        dictionaryListString.Add(entry.Key);
                    }
                }
                listOfDictionaryListsDouble.Add(dictionaryListDouble);
                listOfDictionaryListsString.Add(dictionaryListString);
                counter++;

            }
            // calculate data arrays for new/second chart
            for(int i=0;i<allLists.Count;i++)
            {
                // new
                DotNet.Highcharts.Options.Point temp1 = new DotNet.Highcharts.Options.Point
                {
                    Y= Math.Round((double)newPercentages.ElementAt(i)) ,
                    Color=Color.FromName("colors[0]"),
                    Drilldown=new Drilldown
                    {
                        Name= "Min/Median/Max",
                        Categories=categoriesDrilldown,
                        Data=new Data(newValues.ElementAt(i).ToArray()),
                        Color=Color.FromName("colors[0]")
                    }
                };
                // second
                DotNet.Highcharts.Options.Point temp2 = new DotNet.Highcharts.Options.Point
                {
                    Y = Math.Round((double)secPercentages.ElementAt(i)),
                    Color = Color.FromName("colors[2]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Min/Median/Max",
                        Categories = categoriesDrilldown,
                        Data = new Data(secValues.ElementAt(i).ToArray()),
                        Color = Color.FromName("colors[2]")
                    }
                };
                finalNewData.Add(temp1);
                finalSecData.Add(temp2);
            }
        }
        // calculate month count needed for search
        // if search is this year (eg:2016) get month that database is updated to
        // if not this year return 12 (every month)
        public int getMonthCount(string year)
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

    }

    // line object
    public class Line
    {
        public string County { get; set; }
        [DisplayName("Post Code")]
        public string PostCode { get; set; }
        public string Area { get; set; }
        public string Year { get; set; }
    }
}
