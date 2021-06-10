using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FlightPlotter
{
    public partial class Form2 : Form
    {
        //TimeSpan F;
        //TimeSpan T;

        public Form2(ModeTime MT)
        { 
            InitializeComponent();

            double InitBlock;
            double WaitBlock;
            double EngBlock;
            double FltBlock;
            double PostEngBlock;
            double PostBlock;

            //Flight order -- Auto,Atti,GPS,Wing,TO,TI, MFW, MGPS

            lblOnTime.Text = Span2String(TimeSpan.FromSeconds(MT.TotalTime));

            lblInitTime.Text = SumPositions(MT.Stops[1],MT.Starts[1]);
            Debug.WriteLine($"Init Len {MT.Stops[1].Count} start {MT.Starts[1].First()} stop {MT.Stops[1].First()} % {((MT.Stops[1].First() - MT.Starts[1].First()) / MT.TotalTime) * 100}");
            Debug.WriteLine($"MGPS Len {MT.Stops[14].Count} start {MT.Starts[14].First()} stop {MT.Stops[14].First()} % {((MT.Stops[14].First() - MT.Starts[14].First()) / MT.TotalTime) * 100}");
            InitBlock = ((MT.Stops[1].First() - MT.Starts[1].First()) / MT.TotalTime) * 100;

            double AirTime = SumPositionsD(MT.Stops[15], MT.Starts[15]);

            double FlightTime =  
                SumPositionsD(MT.Stops[4], MT.Starts[4]) + 
                SumPositionsD(MT.Stops[5], MT.Starts[5]) +
                SumPositionsD(MT.Stops[6], MT.Starts[6]) +
                SumPositionsD(MT.Stops[7], MT.Starts[7]) +
                SumPositionsD(MT.Stops[8], MT.Starts[8]) + 
                SumPositionsD(MT.Stops[9], MT.Starts[9]) +
                SumPositionsD(MT.Stops[10], MT.Starts[10]) +
                SumPositionsD(MT.Stops[11], MT.Starts[11]) +
                SumPositionsD(MT.Stops[12], MT.Starts[12]) +
                SumPositionsD(MT.Stops[13], MT.Starts[13]) +
                SumPositionsD(MT.Stops[14], MT.Starts[14]);
            double HoverTime =
                SumPositionsD(MT.Stops[4], MT.Starts[4]) +
                SumPositionsD(MT.Stops[6], MT.Starts[6]) +
                SumPositionsD(MT.Stops[8], MT.Starts[8]) +
                SumPositionsD(MT.Stops[9], MT.Starts[9]) +
                SumPositionsD(MT.Stops[7], MT.Starts[7]) +
                SumPositionsD(MT.Stops[14], MT.Starts[14]);

            double WingTime =
                //SumPositionsD(MT.Stops[10], MT.Starts[10]) +
                //SumPositionsD(MT.Stops[11], MT.Starts[11]) +
                SumPositionsD(MT.Stops[12], MT.Starts[12]) +
                SumPositionsD(MT.Stops[13], MT.Starts[13]);
            //Debug.WriteLine($"Flt {FlightTime} AIR {AirTime}");
            if (MT.Stops[16].Count > 0 && FlightTime > 0)
            {
                //string tt = $"{Span2String(TimeSpan.FromSeconds(Tach.Item1.Last() - Tach.Item2.Last()))} {Math.Round(TimeSpan.FromSeconds(Tach.Item1.Last() - Tach.Item2.Last()).TotalHours, 2)}";
                lblTachTime.Text = $"{Span2String(TimeSpan.FromSeconds(MT.Stops[16].Last() - MT.Starts[16].Last()))} --- {Math.Round(TimeSpan.FromSeconds(MT.Stops[16].Last() - MT.Starts[16].Last()).TotalHours, 2)} H";
                //lblFltTime.Text = $"{Span2String(TimeSpan.FromSeconds(Flight[0].Item1.Last() - Flight[0].Item2.Last()))} --- {Math.Round(TimeSpan.FromSeconds(Flight[0].Item1.Last() - Flight[0].Item2.Last()).TotalHours, 2)} H";
                lblFltTime.Text = $"{TimeSpan.FromSeconds(FlightTime)} --- {Math.Round(TimeSpan.FromSeconds(FlightTime).TotalHours, 2)} H";
                lblHovTime.Text = $"{TimeSpan.FromSeconds(HoverTime)} --- {Math.Round(TimeSpan.FromSeconds(HoverTime).TotalHours, 2)} H";
                lblWingTime.Text = $"{TimeSpan.FromSeconds(WingTime)} --- {Math.Round(TimeSpan.FromSeconds(WingTime).TotalHours, 2)} H";
                lblAttiHover.Text = SumPositions(MT.Stops[8], MT.Starts[8]);
                lblMGPS.Text = SumPositions(MT.Stops[14], MT.Starts[14]);
                lblAGPS.Text = SumPositions(MT.Stops[9], MT.Starts[9]);
                TimeSpan WT = TimeSpan.FromSeconds(MT.Starts[16].First() - MT.Starts[2].First());
                //Debug.WriteLine($"Waiting starts {MT.Starts[2].First()} Tach Starts{MT.Starts[16].First()}");
                if (WT.Minutes > 15)
                {
                    lblWaitTime.Text = Span2String(WT);
                    lblWaitTime.ForeColor = Color.Red;
                }
                else
                {
                    lblWaitTime.Text = Span2String(TimeSpan.FromSeconds(MT.Starts[16].First() - MT.Starts[2].First()));
                }
                FltBlock = ((MT.Stops[15].Last() - MT.Starts[15].Last()) / MT.TotalTime) * 100;
                EngBlock = ((MT.Starts[15].Last() - MT.Starts[16].Last()) / MT.TotalTime) * 100;
                WaitBlock = ((MT.Starts[16].First() - MT.Starts[2].First()) / MT.TotalTime) * 100;
                //Debug.WriteLine($"Post eng block last {MT.Stops[16].Last()} Tflight last {MT.Stops[15].Last()}");
                PostEngBlock = ((MT.Stops[16].Last() - MT.Stops[15].Last()) / MT.TotalTime) * 100;
                if (PostEngBlock <= 0.0)
                {
                    PostEngBlock = 0.0;
                }
                
                PostBlock = 100 - (InitBlock + WaitBlock + EngBlock + PostEngBlock + FltBlock);
                //Debug.WriteLine($"init {InitBlock} wait {WaitBlock} eng {EngBlock} flt {FltBlock} posteng {PostEngBlock} post {PostBlock}");


                //Debug.WriteLine($"Init % {InitBlock}");
                //Debug.WriteLine($"Wait % {WaitBlock}");
                //Debug.WriteLine($"Eng On Pre % {EngBlock}");
                //Debug.WriteLine($"Flight % {FltBlock}");
                //Debug.WriteLine($"Eng On Post % {PostEngBlock}");
                //Debug.WriteLine($"Post % {PostBlock}");
                chart2.Hide();
                chart3.Hide();
                ChartOverview(InitBlock, WaitBlock, EngBlock, FltBlock, PostEngBlock, PostBlock, "Flight Overview");
                //ChartControl(MT.Stops[17], MT.Starts[17], MT.TotalTime, "GCS/EP Control");
                //FullRC(MT.Stops[18], MT.Starts[18], MT.TotalTime, "Manual/Full RC Control");
                return;
            }
            else if (MT.Stops[16].Count > 0 && FlightTime <= 0)
            {
                lblTachTime.Text = $"{Span2String(TimeSpan.FromSeconds(MT.Stops[16].Last() - MT.Starts[16].Last()))} --- {Math.Round(TimeSpan.FromSeconds(MT.Stops[16].Last() - MT.Starts[16].Last()).TotalHours, 2)} H";
                
                TimeSpan WT = TimeSpan.FromSeconds(MT.Starts[16].First() - MT.Starts[2].First());
                lblWaitTime.Text = Span2String(WT);
                EngBlock = ((MT.Starts[15].Last() - MT.Starts[16].Last()) / MT.TotalTime) * 100;
                WaitBlock = ((MT.Starts[16].First() - MT.Starts[2].First()) / MT.TotalTime) * 100;
                PostEngBlock = 0.0;
                FltBlock = 0.0;
                PostBlock = 100 - (InitBlock + WaitBlock + EngBlock + FltBlock);

                Debug.WriteLine("No flt time");
                Debug.WriteLine($"Init % {InitBlock}");
                Debug.WriteLine($"Wait % {WaitBlock}");
                //Debug.WriteLine($"Eng On % {EngBlock} stop {Tach.Item1.Last()} start {Tach.Item2[Tach.Item1.Count - 1]}");
                Debug.WriteLine($"Eng On % {EngBlock} stop {MT.Stops[16].Count} start {MT.Starts[16].Count}");
                Debug.WriteLine($"Flight % {FltBlock}");
                Debug.WriteLine($"Post % {PostBlock}");

                ChartOverview(InitBlock, WaitBlock, EngBlock, FltBlock, PostEngBlock, PostBlock, "Ground Run Overview");

            }
            else if (MT.Stops[16].Count <= 0 && FlightTime <= 0)
            {
                TimeSpan WT = TimeSpan.FromSeconds(MT.Starts[16].First() - MT.Starts[2].First());
                lblWaitTime.Text = Span2String(WT);
                EngBlock = 0.0;
                FltBlock = 0.0;
                WaitBlock = ((MT.Starts[16].First() - MT.Starts[2].First()) / MT.TotalTime) * 100;
                PostEngBlock = 0.0;
                PostBlock = 100 - (InitBlock + WaitBlock + EngBlock + FltBlock);

                Debug.WriteLine("No flt or tach time");
                Debug.WriteLine($"Init % {InitBlock}");
                Debug.WriteLine($"Wait % {WaitBlock}");
                Debug.WriteLine($"Eng On % {EngBlock}");
                Debug.WriteLine($"Flight % {FltBlock}");
                Debug.WriteLine($"Post % {PostBlock}");

                ChartOverview(InitBlock, WaitBlock, EngBlock, FltBlock, PostEngBlock, PostBlock, "System Check Overview");

            }

        }

        public double SumPositionsD(List<double> Stops, List<double> Starts)
        {
            double s1 = 0.0;
            double s2 = 0.0;
            if (Stops.Count > 0 && Starts.Count > 0)
            {
                for (int i = 0; i < Stops.Count; i++)
                {
                    s1 += Stops[i];
                    s2 += Starts[i];
                    return s1 - s2;
                }
            }

            return 0.0;
        }

        public string SumPositions(List<double> Stops, List<double> Starts)
        {
            double s1 = 0.0;
            double s2 = 0.0;
            if (Stops.Count > 0 && Starts.Count > 0)
            {
                for (int i = 0; i < Stops.Count; i++)
                {
                    s1 += Stops[i];
                    s2 += Starts[i];
                    return Span2String(TimeSpan.FromSeconds(s1 - s2));
                }
            }
            
            return "No Data";
        }

        public void FullRC(List<double> Stops, List<double> Starts, double Total, String title)
        {
            double T = 0;
            double TT = 0;
            chart2.Titles.Add(title);
            int max = 0;
            if (Starts.Count < Stops.Count)
            {
                max = Starts.Count - 1;
            }
            else if (Stops.Count < Starts.Count)
            {
                max = Stops.Count - 1;
            }
            Debug.WriteLine($"Control len of stops {Stops[max]}, starts {Starts[max]}");
            for (int i = 0; i < max; i++)
            {
                Debug.WriteLine($"Adding FRC Control {i}");
                Series data = new Series
                {
                    Name = $"GCS {i}",
                    Color = Color.FromArgb(0, 192, 0),
                    ChartType = SeriesChartType.StackedBar100
                };
                Debug.WriteLine($"{Stops[i]} - {Starts[i]} / {Total} * 100 = {((Stops[i] - Starts[i]) / Total) * 100}");
                T = ((Stops[i] - Starts[i]) / Total) * 100;
                data.Points.AddXY("Event Time", T);
                chart3.Series.Add(data);
                TT += T;

                if (Starts[max] > i + 1)
                {
                    if (i < Stops[max])
                    {
                        Debug.WriteLine($"Adding Manual Control {i}");
                        Series data1 = new Series
                        {
                            Name = $"EP {i}",
                            Color = Color.Orange,
                            ChartType = SeriesChartType.StackedBar100
                        };
                        Debug.WriteLine($"{Starts[i + 1]} - {Stops[i]} / {Total} * 100 = {((Stops[i] - Starts[i]) / Total) * 100}");
                        T = ((Starts[i + 1] - Stops[i]) / Total) * 100;
                        data1.Points.AddXY("Event Time", T);
                        chart3.Series.Add(data1);
                        TT += T;
                    }
                }
                Debug.WriteLine($"Manual/ Full RC Block Total {TT}");
            }
        }

        public void ChartControl(List<double> Stops, List<double> Starts, double Total,  String title)
        {
            double T = 0;
            double TT = 0;
            chart2.Titles.Add(title);
            int max = 0;
            if (Starts.Count < Stops.Count)
            {
                max = Starts.Count - 1;
            }
            else if (Stops.Count < Starts.Count)
            {
                max = Stops.Count - 1;
            }
            Debug.WriteLine($"Control len of stops {Stops[max]}, starts {Starts[max]}");
            for (int i = 0; i < max ; i++)
            {
                Debug.WriteLine($"Adding GCS Control {i}");
                Series data = new Series
                {
                    Name = $"GCS {i}",
                    Color = Color.FromArgb(0, 192, 0),
                    ChartType = SeriesChartType.StackedBar100
                };
                Debug.WriteLine($"{Stops[i]} - {Starts[i]} / {Total} * 100 = {((Stops[i] - Starts[i]) / Total) * 100}");
                T = ((Stops[i] - Starts[i]) / Total) * 100;
                data.Points.AddXY("Event Time", T);
                chart2.Series.Add(data);
                TT += T;

                if (Starts[max] > i +1)
                {
                    if (i < Stops[max])
                    {
                        Debug.WriteLine($"Adding EP Control {i}");
                        Series data1 = new Series
                        {
                            Name = $"EP {i}",
                            Color = Color.Orange,
                            ChartType = SeriesChartType.StackedBar100
                        };
                        Debug.WriteLine($"{Starts[i + 1]} - {Stops[i]} / {Total} * 100 = {((Stops[i] - Starts[i]) / Total) * 100}");
                        T = ((Starts[i + 1] - Stops[i]) / Total) * 100;
                        data1.Points.AddXY("Event Time", T);
                        chart2.Series.Add(data1);
                        TT += T;
                    }
                }
                
                Debug.WriteLine($"EP/GCS Block Total {TT}");
            }
        }

        public void ChartOverview(double InitBlock, double WaitBlock, double EngBlock, double FltBlock, double PostEngBlock, double PostBlock, string title)
        {
            chart1.Titles.Add(title);
            chart1.Series["Initializing"].Points.AddXY("Event Time", InitBlock);
            chart1.Series["Waiting"].Points.AddXY("Event Time", WaitBlock);
            chart1.Series["Engine On Pre-Launching"].Points.AddXY("Event Time", EngBlock);
            chart1.Series["Flight"].Points.AddXY("TiEvent Timeme", FltBlock);
            chart1.Series["Engine On Post Landing"].Points.AddXY("Event Time", PostEngBlock);
            chart1.Series["Post Flight Power On"].Points.AddXY("Event Time", PostBlock);
        }
        public string Span2String(TimeSpan t, bool decimalTime = false)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds);
            
            
        }
        public string Span2StringDec(TimeSpan t)
        {
            double m = t.Minutes / 60;
            return string.Format($"{t.TotalHours}");


        }

        private void chart2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void chart2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
    
}
