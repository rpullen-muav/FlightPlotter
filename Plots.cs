using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FlightPlotter
{
    
    public partial class Plots : Form
    {
        public ReportConfig RCFG;
        public List<string> LB;
        public string GraphTitle { get; set; }
        private Form1 form1;
        bool arrowKeyDown = false;
        bool textKeyDown = false;
        bool clearKeyDown = false;
        int AnnotationCounter = 0;
        Chart chart;
        
        Dictionary<int, Color> ColorLookup = new Dictionary<int, Color>()
        {
            { 0, Color.Orange },
            { 1, Color.Lime },
            { 2,Color.Purple },
            { 3, Color.Cyan },
            { 4, Color.IndianRed },
            { 5, Color.Goldenrod },
            { 6, Color.MidnightBlue },
            { 7, Color.Black }
        };


        public Plots(Form1 form1, List<string> L)
        {
            this.form1 = form1;
            LB = new List<string>(L);
            InitializeComponent();
        }
        public Color GetRandomColor()
        {
            Random rand = new Random();
            int r = rand.Next(256);
            int g = rand.Next(256);
            //int b = rand.Next(256);
            return Color.FromArgb(255, g, 255);
        }
        private void Plots_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.RCFG.ChartList.Clear();
            chart.Annotations.Clear();
        }

        public static List<T> Transform<T>(IEnumerable lst)
        {
            List<T> L = new List<T>();
            foreach (var item in lst)
            {
                L.Add((T)Convert.ChangeType(item, typeof(T)));
            }
            
            return L;
        }

        public void DrawAnnotation<T>(Chart chart, List<int> xList, string typeOfA, string threshOfA, string Operator, string freq, string name, List<T> yList, string c2)
        {
            
            if (typeOfA == "Vertical")
            {

                Debug.WriteLine(Operator);
                //int Val = Convert.ToInt32(item);
                switch (Operator)
                {

                    case ">=":
                        double Threshhold = double.Parse(threshOfA);
                        //double Val = 0;
                        if (c2 == "d")
                        {
                            Threshhold = double.Parse(threshOfA);
                        }
                        int count = 0;
                        List<int> ValList = new List<int>();
                        foreach (var item in yList)
                        {
                            if (Convert.ToDouble(item) >= Threshhold)
                            {
                                //Debug.WriteLine($"Should be annotating val {Convert.ToDouble(item)} count {count} thresh {Threshhold}");
                                ValList.Add(count);
                                //break;
                            }
                            ++count;
                        }
                        if (ValList.Count > 0)
                        {
                            if (freq == "First" || freq == "")
                            {
                                VerticalLine(chart, xList[ValList[1]], name, Color.Red);
                            }
                            else if (freq == "First+Last")
                            {
                                VerticalLine(chart, xList[ValList.First()], $"{name}_{ValList[1]}", Color.Red);
                                VerticalLine(chart, xList[ValList.Last()], $"{name}_{ValList[ValList.Count - 2]}", Color.Red);
                            }
                            else if (freq == "Last")
                            {
                                VerticalLine(chart, xList[ValList[ValList.Count - 2]], $"{name}_{ValList[ValList.Count - 2]}", Color.Red);
                            }
                        }
                        break;

                    case "==":
                        double Threshhold2 = double.Parse(threshOfA);
                        //double Val2 = 0;
                        int count2 = 0;
                        List<int> ValList2 = new List<int>();
                        foreach (var item in yList)
                        {
                            if (Convert.ToDouble(item) == Threshhold2)
                            {
                                ValList2.Add(count2);
                            }
                            ++count2;
                        }
                        if (ValList2.Count > 0)
                        {
                            if (freq == "First" || freq == "")
                            {
                                VerticalLine(chart, xList[ValList2[0]], name, Color.Red);
                            }
                            else if (freq == "First+Last")
                            {
                                VerticalLine(chart, xList[ValList2.First()], $"{name}_{ValList2.First()}", Color.Red);
                                VerticalLine(chart, xList[ValList2.Last()], $"{name}_{ValList2.Last()}", Color.Red);
                            }
                            else if (freq == "Last")
                            {
                                VerticalLine(chart, xList[ValList2.Last()], $"{name}_{ValList2.Last()}", Color.Red);
                            }
                            else if (freq == "All")
                            {
                                for (int i = 0; i < ValList2.Count; i++)
                                {
                                    VerticalLine(chart, xList[ValList2[i]], $"{name}_{i}", Color.Red);
                                }
                            }
                        }
                        break;

                    case "<=":
                        double Threshhold3 = double.Parse(threshOfA);
                        //double Val3 = 0;
                        if (c2 == "d")
                        {
                            Threshhold3 = double.Parse(threshOfA);
                        }
                        int count3 = 0;
                        List<int> ValList3 = new List<int>();
                        foreach (var item in yList)
                        {
                            if (Convert.ToDouble(item) <= Threshhold3)
                            {
                                ValList3.Add(count3);
                                //break;
                            }
                            ++count3;
                        }
                        if(ValList3.Count > 0)
                        {
                            if(freq == "First" || freq == "")
                            {
                                VerticalLine(chart, xList[ValList3[0]], name, Color.Red);
                            }
                            else if (freq == "First+Last")
                            {
                                VerticalLine(chart, xList[ValList3.First()], $"{name}_{ValList3.First()}", Color.Red);
                                VerticalLine(chart, xList[ValList3.Last()], $"{name}_{ValList3.Last()}", Color.Red);
                            }
                            else if (freq == "Last")
                            {
                                VerticalLine(chart, xList[ValList3.Last()], $"{name}_{ValList3.Last()}", Color.Red);
                            }
                        }
                        break;
                }
            }
            if (typeOfA == "Horizontal")
            {
                double Threshhold = double.Parse(threshOfA);
                HorizontalLine(chart, Threshhold, name, Color.Red);
            }
        }

        public void ModSeries(Series s, Tuple<IEnumerable,object> YInfo, List<int> XList, SeriesConfig SC)
        {
            Random rnd = new Random();
            int num = rnd.Next();
            if (YInfo.Item2.ToString() == "System.Collections.Generic.List`1[System.Double]" || YInfo.Item2.ToString() == "System.Collections.Generic.List`1[System.Single]")
            {
                
                List<double> DoubleY = new List<double>(Transform<double>(YInfo.Item1));
                DrawAnnotation<double>(
                    chart,
                    XList,
                    SC.AnnotationType,
                    SC.AnnotationThreshold,
                    SC.AnnotationOperator,
                    SC.AnnotationFrequency,
                    $"{SC.Name}_{num}",
                    DoubleY,
                    "d"
                    );
                s.Points.DataBindXY(XList, DoubleY);
            }
            if (YInfo.Item2.ToString() == "System.Collections.Generic.List`1[System.Byte]")
            {

                List<byte> ByteY = new List<byte>(Transform<byte>(YInfo.Item1));
                DrawAnnotation<byte>(
                    chart,
                    XList,
                    SC.AnnotationType,
                    SC.AnnotationThreshold,
                    SC.AnnotationOperator,
                    SC.AnnotationFrequency,
                    $"{SC.Name}_{num}",
                    ByteY,
                    "b"
                    );
                s.Points.DataBindXY(XList, ByteY);
            }
        }

        private void Plots_Load(object sender, EventArgs e)
        {
            List<int> X_list_int1;// = Form1.C.linux_time_s.ConvertAll(Convert.ToInt32);
            List<int> X_list_int2;// = Form1.G2.linux_time_s.ConvertAll(Convert.ToInt32);
            List<int> X_list_int3;// = Form1.G1.linux_time_s.ConvertAll(Convert.ToInt32);
            List<int> X_list_int4;// = Form1.M.linux_time_s.ConvertAll(Convert.ToInt32);
            List<int> X_list_int5;// = Form1.N.linux_time_s.ConvertAll(Convert.ToInt32);
            List<int> X_list_int6;// = Form1.S.linux_time_s.ConvertAll(Convert.ToInt32);
            List<List<int>> X_list = new List<List<int>>();
            List<object> OL = new List<object>(); // = new List<object> { Form1.C, Form1.G2, Form1.G1, Form1.M, Form1.N, Form1.S };
            if (form1.cbbLogType.SelectedItem.ToString() == "1.62a")
            {
                //OL = new List<object> { Form1.C, Form1.G2, Form1.G1, Form1.M, Form1.N, Form1.S };
                //OL = new List<object> { Form1.C, Form1.G2, Form1.G1, Form1.M, Form1.N, Form1.S };
                OL.Add(Form1.C);
                OL.Add(Form1.G2);
                OL.Add(Form1.G1);
                OL.Add(Form1.M);
                OL.Add(Form1.N);
                OL.Add(Form1.S);
                X_list_int1 = Form1.C.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int2 = Form1.G2.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int3 = Form1.G1.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int4 = Form1.M.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int5 = Form1.N.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int6 = Form1.S.linux_time_s.ConvertAll(Convert.ToInt32);

                X_list.Add(X_list_int1);
                X_list.Add(X_list_int2);
                X_list.Add(X_list_int3);
                X_list.Add(X_list_int4);
                X_list.Add(X_list_int5);
                X_list.Add(X_list_int6);
            }
            else if(form1.cbbLogType.SelectedItem.ToString() == "1.70")
            {
                //OL = new List<object> { Form1.C17, Form1.G217, Form1.G117, Form1.M17, Form1.N17, Form1.S17 };
                OL.Add(Form1.C17);
                OL.Add(Form1.G217);
                OL.Add(Form1.G117);
                OL.Add(Form1.M17);
                OL.Add(Form1.N17);
                OL.Add(Form1.S17);
                X_list_int1 = Form1.C17.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int2 = Form1.G217.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int3 = Form1.G117.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int4 = Form1.M17.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int5 = Form1.N17.linux_time_s.ConvertAll(Convert.ToInt32);
                X_list_int6 = Form1.S17.linux_time_s.ConvertAll(Convert.ToInt32);

                X_list.Add(X_list_int1);
                X_list.Add(X_list_int2);
                X_list.Add(X_list_int3);
                X_list.Add(X_list_int4);
                X_list.Add(X_list_int5);
                X_list.Add(X_list_int6);
            }
            else if (form1.cbbLogType.SelectedItem.ToString() == "1.7+")
            {
                //OL = new List<object> { Form1.Ctrl171, Form1.GCS171, Form1.Guid171, Form1.Mode171, Form1.Nav171, Form1.Snsr171 };
                OL.Add(Form1.Ctrl171);
                OL.Add(Form1.GCS171);
                OL.Add(Form1.Guid171);
                OL.Add(Form1.Mode171);
                OL.Add(Form1.Nav171);
                OL.Add(Form1.Snsr171);

                X_list_int1 = Form1.Ctrl171.linux_time.ConvertAll(Convert.ToInt32);
                X_list_int2 = Form1.GCS171.linux_time.ConvertAll(Convert.ToInt32);
                X_list_int3 = Form1.Guid171.linux_time.ConvertAll(Convert.ToInt32);
                X_list_int4 = Form1.Mode171.linux_time.ConvertAll(Convert.ToInt32);
                X_list_int5 = Form1.Nav171.linux_time.ConvertAll(Convert.ToInt32);
                X_list_int6 = Form1.Snsr171.linux_time.ConvertAll(Convert.ToInt32);

                X_list.Add(X_list_int1);
                X_list.Add(X_list_int2);
                X_list.Add(X_list_int3);
                X_list.Add(X_list_int4);
                X_list.Add(X_list_int5);
                X_list.Add(X_list_int6);
            }

            //List<List<int>> X_list = new List<List<int>>()
            //{
            //    X_list_int1,
            //    X_list_int2,
            //    X_list_int3,
            //    X_list_int4,
            //    X_list_int5,
            //    X_list_int6,
            //};



            int ChartSizeY = form1.RCFG.ChartY;
            int ChartSizeX = form1.RCFG.ChartX;
            int SeriesLineWidth = 1;
            int ChartBorderWidth = 1;
            Color ChartBorderColor = Color.LightGray;
            Color ChartBG = Color.DarkGray;
            Color ChartAreaBG = Color.DarkGray;
            Color LegendBG = Color.DarkGray;
            //Docking Top = Docking.Top;
            //Docking Bottom = Docking.Bottom;
            //Docking Left = Docking.Left;
            //Docking Right = Docking.Right;
            Docking Docked = Docking.Top;
            if (form1.cbWhiteBG.Checked)
            {
                ChartBorderColor = Color.White;
                ChartBG = Color.White;
                ChartAreaBG = Color.White;
                LegendBG = Color.White;
            }
            //List<IEnumerable> X_list = new List<IEnumerable> { form1.CD.linux_time_s, form1.GCSD.linux_time_s, form1.GD.linux_time_s, form1.MC.linux_time_s, form1.Nav.linux_time_s, form1.SP.linux_time_s }; // needed if float time is needed for x axis
            //List<object> OL = new List<object> { Form1.C, Form1.G2, Form1.G1, Form1.M, Form1.N, Form1.S };
            bool started = false;
            
            //int count = 0;
            for (int i = 0; i < form1.RCFG.ChartList.Count; i++) // iterate through chart groups in report config
            {
                //Debug.WriteLine($"Chart group {i} of {form1.RCFG.ChartList.Count-1}");
                for (int j = 0; j < form1.RCFG.ChartList[i].SeriesList.Count; j++)// iterate through series in chart groups
                {
                    //Debug.WriteLine($"Series group {j} of {form1.RCFG.ChartList[i].SeriesList.Count - 1}");
                    if (form1.RCFG.ChartList[i].SeriesList[j].ChartID == 0)
                    {
                        //Debug.WriteLine($"Series group {form1.RCFG.ChartList[i].SeriesList[j].ChartID} ");
                        chart = new Chart();
                        
                        chart.ChartAreas.Add(new ChartArea());
                        if (!string.IsNullOrWhiteSpace(form1.RCFG.ChartList[i].SeriesList[j].GroupName))
                        {
                            chart.Titles.Add(form1.RCFG.ChartList[i].SeriesList[j].GroupName);
                        }

                        chart.Name = form1.RCFG.ChartList[i].SeriesList[j].Name;
                        
                        if (form1.cbLinkXZoom.Checked)
                        {
                            chart.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.AxisViewChanged);
                        }

                        chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                        chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true; // leave here to make selecting y an option
                        if (form1.cbZoomableY.Checked)
                        {
                            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
                            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true; // leave here to make selecting y an option
                        }
                        
                        ChartDashStyle Border = ChartDashStyle.Solid;
                        chart.BorderlineDashStyle = Border;
                        chart.BorderlineColor = ChartBorderColor;
                        chart.BorderlineWidth = ChartBorderWidth;
                        chart.ChartAreas[0].AxisX.Title = "Linux Time S";
                        chart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chart.ChartAreas[0].BackColor = ChartAreaBG;
                        chart.BackColor = ChartBG;
                        
                        if (form1.cbMouseDP.Checked)
                        {
                            chart.MouseMove += new System.Windows.Forms.MouseEventHandler(chart_MouseMove);
                        }
                        chart.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(chart_MouseDoubleClick);
                        chart.KeyDown += chart_KeyDown;
                        chart.KeyUp += chart_KeyUp;


                        if (!string.IsNullOrWhiteSpace(form1.RCFG.ChartList[i].SeriesList[j].Alias))
                        {
                            chart.ChartAreas[0].AxisY.Title = form1.RCFG.ChartList[i].SeriesList[j].Alias;
                        }
                        else
                        {
                            chart.ChartAreas[0].AxisY.Title = form1.RCFG.ChartList[i].SeriesList[j].Name.Replace('_', ' ');
                        }

                        int index = LB.FindIndex(a => a.Contains(form1.RCFG.ChartList[i].SeriesList[j].Parent));

                        // setup series
                        Series data = new Series();
                        Tuple<IEnumerable, object> NewY;
                        //if (form1.cbbLogType.SelectedItem.ToString() == "1.62a" || form1.cbbLogType.SelectedItem.ToString() == "1.70")
                        //{
                        //    NewY = GetPropValue(OL[index], form1.RCFG.ChartList[i].SeriesList[j].Name);
                        //}
                        //else
                        //{
                        //    NewY = GetPropValue(OL[index], form1.RCFG.ChartList[i].SeriesList[j].Name);
                        //}
                        NewY = GetPropValue(OL[index], form1.RCFG.ChartList[i].SeriesList[j].Name);
                        //Debug.WriteLine($"Got prop and Val type: {form1.RCFG.ChartList[i].SeriesList[j].Name} {NewY.Item2}");
                        if (form1.RCFG.ChartList[i].SeriesList[j].AnnotationType != "")
                        {
                            ModSeries(data, NewY, X_list[index], form1.RCFG.ChartList[i].SeriesList[j]);
                        }
                        else
                        {
                            //if (form1.RCFG.ChartList[i].SeriesList[j].Name == "flight_state_tlm")
                            //{

                            //    chart.Customize += new System.EventHandler(this.chart_Customize);
                            //    //data.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;

                            //    data.Points.DataBindXY(X_list[index], NewY.Item1);
                            //}
                            //else
                            //{
                            //    data.Points.DataBindXY(X_list[index], NewY.Item1);
                            //}
                            data.Points.DataBindXY(X_list[index], NewY.Item1);
                        }

                        data.ChartType = SeriesChartType.Line;
                        data.Color = ColorLookup[0]; // GetRandomColor();
                        data.BorderWidth = SeriesLineWidth;
                        data.Enabled = true;
                        //data.Name = form1.RCFG.ChartList[i].SeriesList[j].Name;
                        chart.Series.Add(data);
                        //chart.Visible = true;
                        chart.Size = new Size(ChartSizeX, ChartSizeY);
                        //Debug.WriteLine($"Position: {chart.ChartAreas[0].Position.Auto}");
                        //chart.ChartAreas[0].InnerPlotPosition = new ElementPosition(5, 5, 94, 75);
                        
                        pnlPlots.Controls.Add(chart);
                    }
                    else if (form1.RCFG.ChartList[i].SeriesList[j].ChartID >= 1)
                    {
                        //Debug.WriteLine($"Series group {form1.RCFG.ChartList[i].SeriesList[j].ChartID}, Name : {form1.RCFG.ChartList[i].SeriesList[j].Name} ");
                        if (!started)
                        {
                            chart = new Chart();
                            started = true;
                            ////Debug.WriteLine($"Series group {form1.RCFG.ChartList[i].SeriesList[j].ChartID} ");
                            chart.ChartAreas.Add(new ChartArea());
                            if (!string.IsNullOrWhiteSpace(form1.RCFG.ChartList[i].SeriesList[j].GroupName))
                            {
                                chart.Titles.Add(form1.RCFG.ChartList[i].SeriesList[j].GroupName);
                            }

                            chart.Name = form1.RCFG.ChartList[i].SeriesList[j].Name;

                            if (form1.cbLinkXZoom.Checked)
                            {
                                chart.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.AxisViewChanged);
                            }

                            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true; // leave here to make selecting y an option
                            if (form1.cbZoomableY.Checked)
                            {
                                chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                                chart.ChartAreas[0].CursorY.IsUserEnabled = true;
                                chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true; // leave here to make selecting y an option
                            }
                            
                            ChartDashStyle Border = ChartDashStyle.Solid;
                            chart.BorderlineDashStyle = Border;
                            chart.BorderlineColor = ChartBorderColor;
                            chart.BorderlineWidth = ChartBorderWidth;
                            chart.ChartAreas[0].AxisX.Title = "Linux Time";
                            chart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                            chart.KeyDown += chart_KeyDown;
                            chart.KeyUp += chart_KeyUp;
                            chart.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(chart_MouseDoubleClick);
                            
                            if(form1.cbMouseDP.Checked)
                            {
                                chart.MouseMove += new System.Windows.Forms.MouseEventHandler(chart_MouseMove);
                            }
                            

                            if (!string.IsNullOrWhiteSpace(form1.RCFG.ChartList[i].SeriesList[j].Alias))
                            {
                                chart.Legends.Add(form1.RCFG.ChartList[i].SeriesList[j].Alias);
                            }
                            else
                            {
                                chart.Legends.Add(form1.RCFG.ChartList[i].SeriesList[j].Name.Replace('_', ' '));
                            }
                            chart.Legends[0].BackColor = LegendBG;
                            chart.Legends[0].Docking = Docked;
                            int index = LB.FindIndex(a => a.Contains(form1.RCFG.ChartList[i].SeriesList[j].Parent));
                            //Debug.WriteLine($"Parent: {form1.RCFG.ChartList[i].SeriesList[j].Parent}, Name: {form1.RCFG.ChartList[i].SeriesList[j].Name}, Index: {index}");

                            // setup series
                            Series data = new Series();
                            Tuple<IEnumerable, object> NewY = GetPropValue(OL[index], form1.RCFG.ChartList[i].SeriesList[j].Name);
                            //Debug.WriteLine($"Got prop and Val type: {form1.RCFG.ChartList[i].SeriesList[j].Name} {NewY.Item2}");
                            if (form1.RCFG.ChartList[i].SeriesList[j].AnnotationType != "")
                            {
                                ModSeries(data, NewY, X_list[index], form1.RCFG.ChartList[i].SeriesList[j]);
                            }
                            else
                            {
                                data.Points.DataBindXY(X_list[index], NewY.Item1);
                            }
                            //data.Points.DataBindXY(X_list[index], NewY.Item1);
                            data.ChartType = SeriesChartType.Line;
                            data.Color = ColorLookup[j];
                            data.BorderWidth = SeriesLineWidth;
                            data.IsVisibleInLegend = true;
                        }
                        if (started)
                        {
                            int index = LB.FindIndex(a => a.Contains(form1.RCFG.ChartList[i].SeriesList[j].Parent));
                            //Debug.WriteLine($"Parent: {form1.RCFG.ChartList[i].SeriesList[j].Parent}, Name: {form1.RCFG.ChartList[i].SeriesList[j].Name}, Index: {index}");

                            // setup series
                            Series data = new Series();
                            //VerticalLine(chart, 1589, data.Name, Color.Red);
                            Tuple<IEnumerable, object> NewY = GetPropValue(OL[index], form1.RCFG.ChartList[i].SeriesList[j].Name);
                            //Debug.WriteLine($"Got prop and Val type: {form1.RCFG.ChartList[i].SeriesList[j].Name} {NewY.Item2}");
                            if (form1.RCFG.ChartList[i].SeriesList[j].AnnotationType != "")
                            {
                                ModSeries(data, NewY, X_list[index], form1.RCFG.ChartList[i].SeriesList[j]);
                            }
                            else
                            {
                                data.Points.DataBindXY(X_list[index], NewY.Item1);
                            }

                            //data.Points.DataBindXY(X_list[index], NewY.Item1);
                            data.ChartType = SeriesChartType.Line;
                            data.Color = ColorLookup[j];
                            data.BorderWidth = SeriesLineWidth;
                            data.IsVisibleInLegend = true;
                            if(!string.IsNullOrWhiteSpace(form1.RCFG.ChartList[i].SeriesList[j].Alias))
                            {
                                data.Name = $"{form1.RCFG.ChartList[i].SeriesList[j].Alias}";
                            }
                            else
                            {
                                data.Name = $"{form1.RCFG.ChartList[i].SeriesList[j].Name}";
                            }
                            
                            //Debug.WriteLine("Series Added");
                            //chart1.Series.Add(data);
                            chart.Size = new Size(ChartSizeX, ChartSizeY);

                            if (form1.RCFG.ChartList[i].SeriesList[j].AuxYAxis)// && j == 1)
                            {
                                    data.YAxisType = AxisType.Secondary;
                                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                                    chart.ChartAreas[0].AxisY2.LineColor = data.Color;
                                    chart.Series.Add(data);
                            }
                            else
                            {
                                chart.Series.Add(data);
                            }

                            //Debug.WriteLine($"loop # {j}, Count: {form1.RCFG.ChartList[i].SeriesList.Count - 1}");
                            if (j == form1.RCFG.ChartList[i].SeriesList.Count - 1)
                            {
                                //Debug.WriteLine("making Multi series chart");
                                started = false;
                                //chart1.ChartAreas[0].InnerPlotPosition = new ElementPosition(5, 5, 97 - xPos, 75);
                                chart.ChartAreas[0].BackColor = ChartBG;
                                chart.BackColor = ChartBG;
                                //chart1.Size = new Size(ChartSizeX, ChartSizeY);
                               
                                pnlPlots.Controls.Add(chart);
                            }
                        }
                    }
                }
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void chart_Customize(object sender, EventArgs e)
        {
            Dictionary<byte, string> il = new Dictionary<byte, string>()
            {
                //{ 0,"" },
                { 1,"Initialize 1" },
                { 2,  "Startup 2" },
                { 3, "Manual 3" },
                { 4, "Launching 4" },
                { 5, "Climbing 5" },
                { 6, "Moving Base Land 6" },
                { 7, "Landing 7" },
                { 8, "Attitude Hover 8" },
                { 9, "GPS Hover 9" },
                { 10, "Transition Out 10" },
                { 11, "Transition In 11" },
                { 12, "Fixed Wing 12" },
                { 13, "Manual Fixed Wing 13" },
                {14, "Manual GPS Hover 14" }
                //{15, "" },
                //{16, "" },
            };
            chart.ChartAreas[0].AxisY.Minimum = 1;
            chart.ChartAreas[0].AxisY.Maximum = 14;
            foreach (var label in chart.ChartAreas[0].AxisY.CustomLabels)
            {
                Debug.WriteLine($"Label Val pre string {label.Text} ");
                if (byte.Parse(label.Text) > 0 && byte.Parse(label.Text) < 15)
                {
                    Debug.WriteLine($"Label Val string {label.Text} ");
                    label.Text = il[byte.Parse(label.Text)];
                }
                
            }


        }

        public void VerticalLine(Chart chart, int position, string name, Color color)
        {
            ChartArea CA = chart.ChartAreas[0];
            VerticalLineAnnotation VA = new VerticalLineAnnotation
            {
                AxisX = CA.AxisX,
                IsInfinitive = true,
                ClipToChartArea = CA.Name,
                Name = $"Annotation_{name}",
                LineColor = color,
                LineWidth = 1,         // use your numbers!
                X = position,
                ToolTip = name
            };
            chart.Annotations.Add(VA);
        }
        public void HorizontalLine(Chart chart, double position, string name, Color color)
        {
            ChartArea CA = chart.ChartAreas[0];
            HorizontalLineAnnotation HA = new HorizontalLineAnnotation
            {
                AxisY = CA.AxisY,
                IsInfinitive = true,
                ClipToChartArea = CA.Name,
                Name = $"Annotation_{name}",
                LineColor = color,
                LineWidth = 1,         // use your numbers!
                Y = position,
                ToolTip = name
            };
            chart.Annotations.Add(HA);
        }

        public void ArrowLine(Chart chart, PointD pos, int idx, string name, Color color)
        {
            AnnotationCounter += 1;
            // todo input list of flags to be drawn with this method
            ChartArea CA = chart.ChartAreas[0];
            ArrowAnnotation AA = new ArrowAnnotation
            {
                AxisY = CA.AxisY,
                AxisX = CA.AxisX,
                Name = $"Annotation_{name}_{AnnotationCounter}",
                ToolTip = name,
                AllowAnchorMoving = true,
                AllowMoving = true,
                AllowPathEditing = true,
                AllowResizing = true,
                AllowSelecting = true,
                AnchorDataPoint = chart.Series[0].Points[idx],
            //AnchorX = pos.X,
            //AnchorY = pos.Y,
            Height = 2,
                Width = -10,
                LineWidth = 0,
                ArrowSize = 3,
                BackColor = color,
                LineColor = color
            };
            chart.Annotations.Add(AA);
        }

        public void TextBox(Chart chart, PointD pos, int idx, string name)
        {
            AnnotationCounter += 1;
            // todo input list of flags to be drawn with this method
            ChartArea CA = chart.ChartAreas[0];
            TextAnnotation TA = new TextAnnotation
            {
                AxisY = CA.AxisY,
                AxisX = CA.AxisX,
                Name = $"Annotation_{name}_{AnnotationCounter}",
                ToolTip = name,
                AllowAnchorMoving = true,
                AllowMoving = true,
                AllowPathEditing = true,
                AllowResizing = true,
                AllowSelecting = true,
                AllowTextEditing = true,
                AnchorDataPoint = chart.Series[0].Points[idx],
                //AnchorX = pos.X,
                //AnchorY = pos.Y,
                Height = 20,
                Width = 20,
                LineWidth = 4,
                BackColor = Color.White,
                ForeColor = Color.Black,
                LineColor = Color.Black,
                Text = "Custom Annotation"
            };
            chart.Annotations.Add(TA);
        }

        public static string FindFocusedControl(Control control)
        {
            var container = control as IContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as IContainerControl;
            }
            return $"Control with focus {control.Name}";
        }

        public static void GPV(object src, string propName)
        {
            Debug.WriteLine(src.GetType().GetProperty(propName).GetValue(src, null));
            //return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        

        public static Tuple<IEnumerable, object> GetPropValue(object src, string propName)
        {
            //var ret = src.GetType().GetProperty(propName).GetValue(src, null) as IEnumerable;
            //return src.GetType().GetProperty(propName).GetValue(src, null);

            //Type t0 = src.GetType().GetProperty(propName).GetType();
            var ret = src.GetType().GetProperty(propName).GetValue(src, null) as IEnumerable;
            var t1 = src.GetType();
            var t2 = t1.GetProperty(propName);
            object t3 = t2.GetValue(src, null);
            //Debug.WriteLine($"Type: {t1}");
            //Debug.WriteLine($"Prop: {t2}");
            //Debug.WriteLine($"Val: {t3}");
            //Debug.WriteLine($"Prop Type: {t0}");
            Tuple<IEnumerable, object> Tout = Tuple.Create(ret, t3);
            return Tout;
        }
        public List<string> GetProperty<T>() where T : class
        {

            List<string> propList = new List<string>();

            // get all public static properties of MyClass type
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(T).GetProperties(BindingFlags.Public |
                                                            BindingFlags.Instance);
            // sort properties by name
            Array.Sort(propertyInfos,
                    delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2) { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //Debug.WriteLine(propertyInfo.Name);
                propList.Add(propertyInfo.Name);
            }

            return propList;
        }

        private void AxisViewChanged(object sender, ViewEventArgs e)
        {
            foreach (Chart tempchart in pnlPlots.Controls)
            {
                //Debug.WriteLine($"Chart names: {tempchart.Name}");
                Chart chart = (Chart)sender;
                if (chart.Name != tempchart.Name)
                {
                    for (int i = 0; i < tempchart.ChartAreas.Count; i++)
                    {
                        //Debug.WriteLine($"Charts to be adj: {tempchart.Name}");
                        tempchart.ChartAreas[i].AxisX.ScaleView.Size = e.Axis.ScaleView.Size;
                        tempchart.ChartAreas[i].AxisX.ScaleView.Position = e.Axis.ScaleView.Position;
                    }
                }

            }
        }
        public struct PointD
        {
            public double X;
            public double Y;
            public PointD(double X, double Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        void chart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                arrowKeyDown = true;
                //Debug.WriteLine("arrow key true");
            }
            if (e.KeyCode == Keys.T)
            {
                textKeyDown = true;
                //Debug.WriteLine("text key true");
            }
            if (e.KeyCode == Keys.Z)
            {
                clearKeyDown = true;
                //Debug.WriteLine("text key true");
            }
        }

        void chart_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                arrowKeyDown = false;
                //Debug.WriteLine("arrow key false");
            }
            if (e.KeyCode == Keys.T)
            {
                textKeyDown = false;
                //Debug.WriteLine("text key false");
            }
            if (e.KeyCode == Keys.Z)
            {
                clearKeyDown = false;
                //Debug.WriteLine("text key true");
            }
        }



        private void chart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Chart chartz = (Chart)sender;
            HitTestResult clicked = chartz.HitTest(e.X, e.Y);
            //Debug.WriteLine($"Chart that was double clicked {chartz.Name}");
            //Debug.WriteLine(FindFocusedControl(chart));
            var pos = LocationInChart(e.X, e.Y, chartz);
            if (arrowKeyDown)
            {
                ArrowLine(chartz, pos, clicked.PointIndex, "Arrow", Color.Black);
            }
            if (textKeyDown)
            {
                TextBox(chartz, pos, clicked.PointIndex, "Text");
            }
            if (clearKeyDown)
            {
                chartz.Annotations.Clear();
            }
            //Debug.WriteLine("Double Clicked ({0}, {1}) ... ({2}, {3})", e.X, e.Y, pos.X, pos.Y);
            //lblCoords.Text = string.Format("({0}, {1}) ... ({2}, {3})", e.X, e.Y, pos.X, pos.Y);
        }
        

        private PointD LocationInChart(double xMouse, double yMouse, Chart chart)
        {
            var ca = chart.ChartAreas[0];

            //Position inside the control, from 0 to 100
            var relPosInControl = new PointD
            (
              ((double)xMouse / (double)chart.Width) * 100,
              ((double)yMouse / (double)chart.Height) * 100
            );

            //Verify we are inside the Chart Area
            if (relPosInControl.X < ca.Position.X || relPosInControl.X > ca.Position.Right
            || relPosInControl.Y < ca.Position.Y || relPosInControl.Y > ca.Position.Bottom) return new PointD(double.NaN, double.NaN);

            //Position inside the Chart Area, from 0 to 100
            var relPosInChartArea = new PointD
            (
              ((relPosInControl.X - ca.Position.X) / ca.Position.Width) * 100,
              ((relPosInControl.Y - ca.Position.Y) / ca.Position.Height) * 100
            );

            //Verify we are inside the Plot Area
            if (relPosInChartArea.X < ca.InnerPlotPosition.X || relPosInChartArea.X > ca.InnerPlotPosition.Right
            || relPosInChartArea.Y < ca.InnerPlotPosition.Y || relPosInChartArea.Y > ca.InnerPlotPosition.Bottom) return new PointD(double.NaN, double.NaN);

            //Position inside the Plot Area, 0 to 1
            var relPosInPlotArea = new PointD
            (
              ((relPosInChartArea.X - ca.InnerPlotPosition.X) / ca.InnerPlotPosition.Width),
              ((relPosInChartArea.Y - ca.InnerPlotPosition.Y) / ca.InnerPlotPosition.Height)
            );

            var X = relPosInPlotArea.X * (ca.AxisX.Maximum - ca.AxisX.Minimum) + ca.AxisX.Minimum;
            var Y = (1 - relPosInPlotArea.Y) * (ca.AxisY.Maximum - ca.AxisY.Minimum) + ca.AxisY.Minimum;

            return new PointD(X, Y);
        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        void chart_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chartz = (Chart)sender;
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chartz.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show("X=" + prop.XValue + ", Y=" + (int)prop.YValues[0], chartz,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }


        //private void btnZoomReset_Click(object sender, EventArgs e)
        //{
        //    foreach (var item in chart1.ChartAreas)
        //    {
        //        item.AxisX.ScaleView.ZoomReset(1);
        //        item.AxisY.ScaleView.ZoomReset(1);
        //    }
        //    foreach (var item in chart2.ChartAreas)
        //    {
        //        item.AxisX.ScaleView.ZoomReset(1);
        //        item.AxisY.ScaleView.ZoomReset(1);
        //    }
        //    foreach (var item in chart3.ChartAreas)
        //    {
        //        item.AxisX.ScaleView.ZoomReset(1);
        //        item.AxisY.ScaleView.ZoomReset(1);
        //    }
        //}
        //public void ResetChartArea(Chart chart)
        //{
        //    // Set default chart areas
        //    foreach (Chart tempchart in this.Controls.OfType<Chart>())
        //    {
        //        for (int i = 0; i < tempchart.ChartAreas.Count; i++)
        //        {
        //            tempchart.Series[i].ChartArea = "Default";
        //        }
        //    }

        //    // Remove newly created series and chart areas
        //    while (chart.Series.Count > 3)
        //    {
        //        chart.Series.RemoveAt(3);
        //    }
        //    while (chart.ChartAreas.Count > 1)
        //    {
        //        chart.ChartAreas.RemoveAt(1);
        //    }
        //    // Set default chart are position to Auto
        //    chart.ChartAreas[0].Position.Auto = true;
        //    chart.ChartAreas[0].InnerPlotPosition.Auto = true;
        //}
        //public void VerticalLine(Chart chart, int position, string name, Color color)
        //{
        //    // todo input list of flags to be drawn with this method
        //    ChartArea CA = chart.ChartAreas[0];
        //    // factors to convert values to pixels
        //    double xFactor = 0.03;         // use your numbers!
        //    double yFactor = 0.02;        // use your numbers!
        //    // the vertical line
        //    VerticalLineAnnotation VA = new VerticalLineAnnotation();
        //    VA.AxisX = CA.AxisX;
        //    //VA.AllowMoving = true;
        //    VA.IsInfinitive = true;
        //    VA.ClipToChartArea = CA.Name;
        //    VA.Name = name;
        //    VA.LineColor = color;
        //    VA.LineWidth = 2;         // use your numbers!
        //    VA.X = position;
        //    VA.ToolTip = name;
        //    chart.Annotations.Add(VA);
        //}
        //public void Box(Chart chart, int position, string name, Color color)
        //{
        //    // todo input list of flags to be drawn with this method
        //    ChartArea CA = chart.ChartAreas[0];

        //    // the rectangle
        //    RectangleAnnotation RA = new RectangleAnnotation();
        //    Debug.WriteLine($"Min {chart.ChartAreas[0].AxisY.Minimum}");
        //    RA.AxisX = CA.AxisX;
        //    RA.IsSizeAlwaysRelative = false;
        //    RA.Width = 1000;       // use your numbers! bound to x axis scale, get creative to keep it constant-ish
        //    RA.Height = 5;        // use your numbers!
        //    RA.Name = name + "box";
        //    RA.LineColor = color;
        //    RA.BackColor = color;
        //    RA.AxisY = CA.AxisY;
        //    RA.Y = 0;
        //    RA.X = position - RA.Width / 2;

        //    RA.Text = name;
        //    RA.ForeColor = Color.Black;
        //    RA.Font = new System.Drawing.Font("Arial", 8f);
        //    chart.Annotations.Add(RA);
        //}
        //switch (NewY.Item2.ToString())
        //{
        //    case "System.Collections.Generic.List`1[System.Double]":
        //        List<double> YDouble = NewY.Item1.Cast<double>().ToList();
        //        chart.ChartAreas[0].AxisY.Minimum = YDouble.Min();
        //        chart.ChartAreas[0].AxisY.Maximum = YDouble.Max();
        //        data.Points.DataBindXY(X_list[index], YDouble);
        //        Debug.WriteLine("Double type cast");
        //        break;
        //    case "System.Collections.Generic.List`1[System.Byte]":
        //        List<byte> YByte = NewY.Item1.Cast<byte>().ToList();
        //        chart.ChartAreas[0].AxisY.Minimum = YByte.Min();
        //        chart.ChartAreas[0].AxisY.Maximum = YByte.Max();
        //        data.Points.DataBindXY(X_list[index], YByte);
        //        Debug.WriteLine("Byte type cast");
        //        break;
        //    case "System.Collections.Generic.List`1[System.SByte]":
        //        List<sbyte> YSByte = NewY.Item1.Cast<sbyte>().ToList();
        //        chart.ChartAreas[0].AxisY.Minimum = YSByte.Min();
        //        chart.ChartAreas[0].AxisY.Maximum = YSByte.Max();
        //        data.Points.DataBindXY(X_list[index], YSByte);
        //        Debug.WriteLine("SByte type cast");
        //        break;
        //    case "System.Collections.Generic.List`1[System.Single]":
        //        List<Single> YSingle = NewY.Item1.Cast<Single>().ToList();
        //        chart.ChartAreas[0].AxisY.Minimum = YSingle.Min();
        //        chart.ChartAreas[0].AxisY.Maximum = YSingle.Max();
        //        data.Points.DataBindXY(X_list[index], YSingle);
        //        Debug.WriteLine("Single type cast");
        //        break;
        //    case "System.Collections.Generic.List`1[System.Boolean]":
        //        List<int> YInt = NewY.Item1.Cast<int>().ToList();
        //        chart.ChartAreas[0].AxisY.Minimum = YInt.Min();
        //        chart.ChartAreas[0].AxisY.Maximum = YInt.Max();
        //        data.Points.DataBindXY(X_list[index], YInt);
        //        Debug.WriteLine("Bool to Int type cast");
        //        break;
        //    default:
        //        //MessageBox.Show("Not all types have been added");
        //        data.Points.DataBindXY(X_list[index], NewY.Item1);
        //        break;
        //}

        public void CreateYAxis(Chart chart, ChartArea area, Series series,
                        float axisX, float axisWidth, float labelsSize, bool alignLeft)
        {

            chart.ApplyPaletteColors();  // (*)



            // Create new chart area for original series
            ChartArea areaSeries = chart.ChartAreas.Add("CAs_" + series.Name);

            areaSeries.AxisX.ScaleView.Zoomable = true;
            areaSeries.AxisY.ScaleView.Zoomable = true;
            areaSeries.CursorX.IsUserEnabled = true;
            //areaSeries.CursorY.IsUserEnabled = true;
            areaSeries.CursorX.IsUserSelectionEnabled = true;
            //areaSeries.CursorY.IsUserSelectionEnabled = true;

            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            // associate series with new ca
            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            //if (chart.ChartAreas.IsUniqueName("CA_AxY_" + series.ChartArea))
            //{
            //    ChartArea areaAxis = chart.ChartAreas.Add("CA_AxY_" + series.ChartArea);
            //}
            ChartArea areaAxis = chart.ChartAreas.Add("CA_AxY_" + series.ChartArea);

            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            RectangleF oRect = area.Position.ToRectangleF();
            areaAxis.Position = new ElementPosition(oRect.X, oRect.Y, axisWidth, oRect.Height);
            areaAxis.InnerPlotPosition
                    .FromRectangleF(areaSeries.InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            seriesCopy.YAxisType = alignLeft ? AxisType.Primary : AxisType.Secondary;  // (**)

            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }
            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // Disable grid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;

            Axis areaAxisAxisY = alignLeft ? areaAxis.AxisY : areaAxis.AxisY2;   // (**)
            areaAxisAxisY.MajorGrid.Enabled = false;
            areaAxisAxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaAxisAxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

            //areaAxisAxisY.Title = series.Name;
            areaAxisAxisY.LineColor = series.Color;    // (*)
            //areaAxisAxisY.TitleForeColor = Color.DarkCyan;  // (*)

            // Adjust area position
            areaAxis.Position.X = axisX;
            areaAxis.InnerPlotPosition.X += labelsSize;
        }

        private void btnSaveChartsImage_Click(object sender, EventArgs e)
        {
            var chartList = new List<Chart>();
            foreach (Chart tempchart in pnlPlots.Controls)
            {
                chartList.Add(tempchart); ;
            }
            var imageList = ChartsToImages(chartList);
            var finalImage = MergeImages(imageList);
            string PicDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string n = string.Format("Chart_test_{0:yyyy-MM-dd_hh-mm-ss-tt}.png", DateTime.Now);
            finalImage.Save(@$"{PicDir}\{n}", ImageFormat.Png);

            using Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
            bmp.Save(@$"{PicDir}\Form_{n}", ImageFormat.Png); // make sure path exists!
        }
        public List<Image> ChartsToImages(List<Chart> charts)
        {
            var imageList = new List<Image>();
            foreach (var c in charts)
            {
                using (var ms = new MemoryStream())
                {
                    c.SaveImage(ms, ChartImageFormat.Png);
                    var bmp = System.Drawing.Bitmap.FromStream(ms);
                    imageList.Add(bmp);
                }
            }
            return imageList;
        }
        private static Image MergeImages(List<Image> imageList)
        {
            var finalSize = new Size();
            foreach (var image in imageList)
            {
                if (image.Width > finalSize.Width)
                {
                    finalSize.Width = image.Width;
                }
                finalSize.Height += image.Height;
            }
            var outputImage = new Bitmap(finalSize.Width, finalSize.Height);
            using (var gfx = Graphics.FromImage(outputImage))
            {
                var y = 0;
                foreach (var image in imageList)
                {
                    gfx.DrawImage(image, 0, y);
                    y += image.Height;
                }
            }
            return outputImage;
        }
    }

    //Declaring a Generic Handler Class which will actually give Property Name,Value for any given class.
    public class GenericPropertyFinder<TModel> where TModel : class
    {
        public void PrintTModelPropertyAndValue(TModel tmodelObj)
        {
            //Getting Type of Generic Class Model
            Type tModelType = tmodelObj.GetType();

            //We will be defining a PropertyInfo Object which contains details about the class property 
            PropertyInfo[] arrayPropertyInfos = tModelType.GetProperties();

            //Now we will loop in all properties one by one to get value
            foreach (PropertyInfo property in arrayPropertyInfos)
            {
                Console.WriteLine("Name of Property is\t:\t" + property.Name);
                Console.WriteLine("Value of Property is\t:\t" + property.GetValue(tmodelObj).ToString());
                Console.WriteLine(Environment.NewLine);
            }
        }
    }

    public static class Utility
    {
        public static Color GetRandomColor()
        {
            Random rand = new Random();
            int r = rand.Next(256);
            int g = rand.Next(256);
            int b = rand.Next(256);
            return Color.FromArgb(r, g, b);
        }

        public static T[] SliceArray<T>(T[] array, int start, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ((start + i) < array.Length) ? array[start + i] : default(T);
            }
            return result;
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> collection, int start, int count)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return collection.Skip(start).Take(count);
        }

        public static List<int> SliceListInt(List<int> li, int start, int end)
        {
            if (start < 0)    // support negative indexing
            {
                start = li.Count + start;
            }
            if (end < 0)    // support negative indexing
            {
                end = li.Count + end;
            }
            if (start > li.Count)    // if the start value is too high
            {
                start = li.Count;
            }
            if (end > li.Count)    // if the end value is too high
            {
                end = li.Count;
            }
            var count = end - start;             // calculate count (number of elements)
            return li.GetRange(start, count);    // return a shallow copy of li of count elements
        }
        public static List<string> SliceListString(List<string> li, int start, int end)
        {
            if (start < 0)    // support negative indexing
            {
                start = li.Count + start;
            }
            if (end < 0)    // support negative indexing
            {
                end = li.Count + end;
            }
            if (start > li.Count)    // if the start value is too high
            {
                start = li.Count;
            }
            if (end > li.Count)    // if the end value is too high
            {
                end = li.Count;
            }
            var count = end - start;             // calculate count (number of elements)
            return li.GetRange(start, count);    // return a shallow copy of li of count elements
        }
    }
    public class TrackedItem
    {
        public string Name { get; set; }
        public bool Started { get; set; } = false;
        public List<string> StartTime { get; set; } // start/stop are line numbers in log file
        public List<string> StopTime { get; set; }
        public List<int> StartLine { get; set; } // start/stop are line numbers in log file
        public List<int> StopLine { get; set; }
        public List<double> Duration { get; set; }

    }
}
