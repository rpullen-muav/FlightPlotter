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
    public partial class Form3 : Form
    {
        List<double> lat;
        List<double> lon;
        public ModeTime MT;
        public Form3(List<double> x, List<double> y, ModeTime NT)
        {
            InitializeComponent();
            MT = NT;
            lat = new List<double>();
            lon = new List<double>();
            for (int i = 0; i < x.Count; i++)
            {
                //Debug.WriteLine($"X {x[i]}");
                if (x[i] != 0)
                {
                    //Debug.WriteLine($"lat {R2D(y[i])} lon {R2D(x[i])}");
                    lat.Add(R2D(y[i]));
                    lon.Add(R2D(x[i]));
                }
                
            }
            
        }
        public static double R2D(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
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

            Chart chart1 = new Chart();
            chart1.ChartAreas.Add(new ChartArea());

            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = false;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.Title = "Longitude";
            chart1.ChartAreas[0].AxisY.Title = "Lattitude";
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.Titles.Add("GPS Track Visualizer");
            //chart1.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.AxisViewChanged);
            Series data = new Series();
            data.ChartType = SeriesChartType.Point;
            data.BorderWidth = 1;
            data.Points.DataBindXY(lon, lat);
            Debug.WriteLine($"Len DP {data.Points.Count}");
            for (int i = 0; i < data.Points.Count; i++)
            {
                if (i <= MT.Stops[12][0] && i >= MT.Starts[12][0])
                {
                    data.Points[i].Color = Color.Red;
                }
                else if (i > MT.Stops[12][0] || i < MT.Starts[12][0])
                {
                    data.Points[i].Color = Color.Blue;
                }
            }
            chart1.Series.Add(data);
            chart1.Location = new Point(2, 2);
            chart1.Size = new Size(850, 850);

            this.Controls.Add(chart1);

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

        private void AxisViewChanged(object sender, ViewEventArgs e)
        {
            foreach (Chart tempchart in this.Controls)
            {
                tempchart.ChartAreas[0].AxisX.ScaleView.Size = e.Axis.ScaleView.Size;
                tempchart.ChartAreas[0].AxisX.ScaleView.Position = e.Axis.ScaleView.Position;
                tempchart.ChartAreas[0].AxisY.ScaleView.Size = e.Axis.ScaleView.Size;
                tempchart.ChartAreas[0].AxisY.ScaleView.Position = e.Axis.ScaleView.Position;

            }
        }
    }
    
}
