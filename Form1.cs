using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
//using v171;
//todo handle a-c-sess-meta logs
//todo parse telecho logs, align data
//build jarvis
//streamline parser
namespace FlightPlotter
{
    
    public partial class Form1 : Form
    {
        
        public ReportConfig RCFG = new ReportConfig();
        public ChartConfig chartConfugration = new ChartConfig();
        public SeriesConfig SeriesConfiguration;
        //classes for 1.62a logs
        public static ModeControlData M = new ModeControlData { };
        public static SensorProcData S = new SensorProcData { };
        public static GuidanceData G1 = new GuidanceData { };
        public static GCSData G2 = new GCSData { };
        public static NavigationData N = new NavigationData { };
        public static ControlData C = new ControlData { };
        // classes for 1.70 logs
        public static ModeControlData17 M17 = new ModeControlData17 { };
        public static SensorProcData17 S17 = new SensorProcData17 { };
        public static GuidanceData17 G117 = new GuidanceData17 { };
        public static GCSData17 G217 = new GCSData17 { };
        public static NavigationData17 N17 = new NavigationData17 { };
        public static ControlData17 C17 = new ControlData17 { };
        // classes for 1.7+ logs
        public static v171.AlogData Alog171 = new v171.AlogData { };
        public static v171.ClogData Clog171 = new v171.ClogData { };
        public static v171.MetaData Meta171 = new v171.MetaData { };
        public static v171.SessData Sess171 = new v171.SessData { };
        public static v171.ModeData Mode171 = new v171.ModeData { };
        public static v171.SnsrData Snsr171 = new v171.SnsrData { };
        public static v171.GuidData Guid171 = new v171.GuidData { };
        public static v171.GCSdData GCS171 = new v171.GCSdData { };
        public static v171.NavdData Nav171 = new v171.NavdData { };
        public static v171.CtrlData Ctrl171 = new v171.CtrlData { };

        public TimeSpan EngineSpan;
        public Tuple<List<double>,List<double>> EngSec;
        public TimeSpan FlightSpan;
        //public ModeTime FltSec = new ModeTime();
        public ModeTime MT = new ModeTime { };
        public Tuple<TimeSpan,string> InitSpan;
        public Tuple<double, double> InitSec;
        public double TotSec;
        public ParseTLV Log = new ParseTLV { };
        public v171.ParseTLV Log171 = new v171.ParseTLV { };
        public List<string> LogType = new List<string>() { "Choose Log Type", "1.62a", "1.70", "1.7+"};

        public List<double> XGPS = new List<double>();
        public List<double> YGPS = new List<double>();
        //public List<SeriesConfig> CT = new List<SeriesConfig>();
        //ChartchartConfugration ct = new ChartchartConfugration { };


        public Form1()
        {
            InitializeComponent();
            
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            txtChartTitle.Text = "Enter Chart Title Here";
            txtChartTitle.ForeColor = SystemColors.ButtonShadow;
            txtReportConfig.Text = "Enter Template Name Here";
            txtReportConfig.ForeColor = SystemColors.ButtonShadow;
            cbbLogType.DataSource = LogType;
            button1.Enabled = false;
           
            //this.DoubleBuffered = true;


        }
        private void txtChartTitle_Enter(object sender, EventArgs e)
        {
            RemoveText(txtChartTitle);
            

        }
        private void txtChartTitle_Leave(object sender, EventArgs e)
        {
            if (txtChartTitle.Text == "")
            {
                AddText("Enter Chart Title Here", txtChartTitle);
            }
            
        }

        private void txtReportConfig_Enter(object sender, EventArgs e)
        {
            RemoveText(txtReportConfig);
        }
        private void txtReportConfig_Leave(object sender, EventArgs e)
        {
            if (txtReportConfig.Text == "")
            {
                AddText("Enter Template Name Here", txtReportConfig);
            }
            
        }

        
        public void RemoveText(TextBox t)
        {
            if (t.Text == "Enter Chart Title Here" || t.Text == "Enter Template Name Here")
            {
                t.Text = "";
                t.ForeColor = Color.Black;
            }
        }

        public void AddText(string s, TextBox t)
        {
            if (string.IsNullOrWhiteSpace(t.Text))
                t.ForeColor = SystemColors.ButtonShadow;
                t.Text = s;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            lblPct.Text = "Getting Started";
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Select TLV for plotting",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "tlv",
                Filter = "tlv files (*.tlv)|*.tlv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                textBox1.Update();
                ForceDrop(textBox1.Text);
                
            }
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        public void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
            if (files != null && files.Any())
                textBox1.Text = files.First(); //select the first one 

            ForceDrop(textBox1.Text);
            
        }

        public void ForceDrop(string path)
        {
            if(cbbLogType.SelectedItem.ToString() == "1.62a")
            {
                //try
                //{
                    Log.DemuxToMemory(path, pbLoadTlv, lblPct, cbbLogType.SelectedItem.ToString());
                    pbLoadTlv.Value = 0;
                    lbModeControl.DataSource = Log.GetProperty<ModeControlData>();
                    lbModeControl.SelectedIndex = -1;
                    lbGcsData.DataSource = Log.GetProperty<GCSData>();
                    lbGcsData.SelectedIndex = -1;
                    lbControl.DataSource = Log.GetProperty<ControlData>();
                    lbControl.SelectedIndex = -1;
                    lbGuid.DataSource = Log.GetProperty<GuidanceData>();
                    lbGuid.SelectedIndex = -1;
                    lbNavd.DataSource = Log.GetProperty<NavigationData>();
                    lbNavd.SelectedIndex = -1;
                    lbSnsr.DataSource = Log.GetProperty<SensorProcData>();
                    lbSnsr.SelectedIndex = -1;

                XGPS = new List<double>(S.meas_lng_rad);
                YGPS = new List<double>(S.meas_lat_rad);
                int ba = GetBaseAlt(S.est_alt_ft, M.flight_state_tlm);
                //MT = new ModeTime();
                MT.GetPositions(
                    M.linux_time_s,
                    M.flight_state_tlm,
                    S.vectornav_tlm_Pitch_deg,
                    S.est_alt_ft,
                    ba,
                    S.vectornav_tlm_accel_Bx_m_s2,
                    S.vectornav_tlm_accel_By_m_s2,
                    S.vectornav_tlm_accel_Bz_m_s2,
                    -30,
                    8,
                    1500,
                    S.prop_rpm,
                    S.rc_control_en,
                    S.rc_input_rc_man_bckup_lsb
                    );

            }
            if (cbbLogType.SelectedItem.ToString() == "1.70")
            {
                try
                {
                    Log.DemuxToMemory(path, pbLoadTlv, lblPct, cbbLogType.SelectedItem.ToString());
                    pbLoadTlv.Value = 0;
                    lbModeControl.DataSource = Log.GetProperty<ModeControlData17>();
                    lbModeControl.SelectedIndex = -1;
                    lbGcsData.DataSource = Log.GetProperty<GCSData17>();
                    lbGcsData.SelectedIndex = -1;
                    lbControl.DataSource = Log.GetProperty<ControlData17>();
                    lbControl.SelectedIndex = -1;
                    lbGuid.DataSource = Log.GetProperty<GuidanceData17>();
                    lbGuid.SelectedIndex = -1;
                    lbNavd.DataSource = Log.GetProperty<NavigationData17>();
                    lbNavd.SelectedIndex = -1;
                    lbSnsr.DataSource = Log.GetProperty<SensorProcData17>();
                    lbSnsr.SelectedIndex = -1;
                }
                catch(System.ArgumentOutOfRangeException e)
                {
                    Debug.WriteLine(e.Message);
                }
                // init,wait,eng,hov,wing,land
                XGPS = new List<double>(S17.meas_lng_rad);
                YGPS = new List<double>(S17.meas_lat_rad);
                int ba = GetBaseAlt(S17.est_alt_ft, M17.flight_state_tlm);
                //MT = new ModeTime();
                MT.GetPositions(
                    M17.linux_time_s, 
                    M17.flight_state_tlm, 
                    S17.vectornav_tlm_Pitch_deg,
                    S17.est_alt_ft,
                    ba,  
                    S17.vectornav_tlm_accel_Bx_m_s2, 
                    S17.vectornav_tlm_accel_By_m_s2, 
                    S17.vectornav_tlm_accel_Bz_m_s2, 
                    -30,
                    8,
                    1500,
                    S17.prop_rpm,
                    S17.rc_control_en,
                    S17.rc_input_rc_man_bckup_lsb
                    );
            }
            if (cbbLogType.SelectedItem.ToString() == "1.7+")
            {
                try
                {
                    Log171.DemuxToMemory(path, pbLoadTlv, lblPct, cbbLogType.SelectedItem.ToString());
                    pbLoadTlv.Value = 0;
                    lbModeControl.DataSource = Log171.GetProperty<v171.ModeData>();
                    lbModeControl.SelectedIndex = -1;
                    lbGcsData.DataSource = Log171.GetProperty<v171.GCSdData>();
                    lbGcsData.SelectedIndex = -1;
                    lbControl.DataSource = Log171.GetProperty< v171.CtrlData> ();
                    lbControl.SelectedIndex = -1;
                    lbGuid.DataSource = Log171.GetProperty< v171.GuidData> ();
                    lbGuid.SelectedIndex = -1;
                    lbNavd.DataSource = Log171.GetProperty< v171.NavdData> ();
                    lbNavd.SelectedIndex = -1;
                    lbSnsr.DataSource = Log171.GetProperty< v171.SnsrData> ();
                    lbSnsr.SelectedIndex = -1;
                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    Debug.WriteLine(e.Message);
                }
                // init,wait,eng,hov,wing,land
                XGPS = new List<double>(Snsr171.meas_lng);
                YGPS = new List<double>(Snsr171.meas_lat);
                int ba = GetBaseAlt(Snsr171.est_alt, Mode171.flight_state_tlm);
                //MT = new ModeTime();
                List<bool> bandaid = new List<bool>();
                for (int i = 0; i < Snsr171.rc_control_en.Count; i++)
                {
                    bool vOut = Convert.ToBoolean(Snsr171.rc_control_en[i]);
                    bandaid.Add(vOut);
                }
                
                MT.GetPositions(
                    Mode171.linux_time,
                    Mode171.flight_state_tlm,
                    Snsr171.vectornav_tlm_Pitch,
                    Snsr171.est_alt,
                    ba,
                    Snsr171.vectornav_tlm_accel_Bx,
                    Snsr171.vectornav_tlm_accel_By,
                    Snsr171.vectornav_tlm_accel_Bz,
                    -30,
                    8,
                    1500,
                    Snsr171.prop_rpm,
                    bandaid,
                    Snsr171.rc_input_rc_man_bckup_lsb
                    );
            }


        }
        public void DeslectAllItems()
        {
            lbModeControl.SelectedIndex = -1;
            lbGcsData.SelectedIndex = -1;
            lbControl.SelectedIndex = -1;
            lbGuid.SelectedIndex = -1;
            lbNavd.SelectedIndex = -1;
            lbSnsr.SelectedIndex = -1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RCFG.ChartX = int.Parse(tbSizeX.Text);
            RCFG.ChartY = int.Parse(tbSizeY.Text);
            List<string> LB = new List<string> { lbControl.Name, lbGcsData.Name, lbGuid.Name, lbModeControl.Name, lbNavd.Name, lbSnsr.Name };
            List<decimal> Organizer = new List<decimal>();
            int row = 1;
            for (row = 1; row <= this.tlpSeriesConfiguration.RowCount-1; row++)
            {
                CheckBox tel = (CheckBox)this.tlpSeriesConfiguration.GetControlFromPosition(0, row);
                TextBox alias = (TextBox)this.tlpSeriesConfiguration.GetControlFromPosition(1, row);
                ComboBox trans = (ComboBox)this.tlpSeriesConfiguration.GetControlFromPosition(2, row);
                CheckBox addY = (CheckBox)this.tlpSeriesConfiguration.GetControlFromPosition(3, row);
                ComboBox ann = (ComboBox)this.tlpSeriesConfiguration.GetControlFromPosition(4, row);
                ComboBox op = (ComboBox)this.tlpSeriesConfiguration.GetControlFromPosition(5, row);
                TextBox thresh = (TextBox)this.tlpSeriesConfiguration.GetControlFromPosition(6, row);
                ComboBox freq = (ComboBox)this.tlpSeriesConfiguration.GetControlFromPosition(7, row);
                TextBox groupName = (TextBox)this.tlpSeriesConfiguration.GetControlFromPosition(8, row);
                NumericUpDown chrt = (NumericUpDown)this.tlpSeriesConfiguration.GetControlFromPosition(9, row);

                SeriesConfiguration = new SeriesConfig(tel.Text, alias.Text, trans.SelectedItem.ToString(), addY.Checked, ann.SelectedItem.ToString(), op.SelectedItem.ToString(), thresh.Text, freq.SelectedItem.ToString(), groupName.Text, chrt.Value, tel.Tag.ToString());
                if (tel.Checked)
                {
                    chartConfugration.SeriesList.Add(SeriesConfiguration);
                    // this list os to make organizing the series easier
                    //Debug.WriteLine($"Chart Val {chrt.Value}");
                    Organizer.Add(chrt.Value);
                }
            }

            List<decimal> distinct = Organizer.OrderByDescending(x => x).Distinct().ToList();
            //create a list in reportConfig for evey chartID type
            for (int i = 0; i < distinct.Count; i++)
            {
                //Debug.WriteLine($"Distinct {distinct[i]}");
                RCFG.ChartList.Add(new ChartConfig { ID = distinct[i] });
            }

            for (int i = 0; i < distinct.Count; i++)
            {
                
                for (int j = 0; j < chartConfugration.SeriesList.Count; j++)
                {
                    if((int)distinct[i] == (int)chartConfugration.SeriesList[j].ChartID)
                    {
                        RCFG.ChartList[i].SeriesList.Add(chartConfugration.SeriesList[j]);
                        //Debug.WriteLine($"Adding chart group {j}");
                    }
                }
            }
            if (RCFG.ChartList.Count > 0)
            {
                Plots form = new Plots(this, LB);

                if (txtChartTitle.Text == "Enter Chart Title Here")
                {
                    form.Text = "VBAT Log Exploitation Tool";
                }
                else
                {
                    form.Text = txtChartTitle.Text;
                }

                form.Show();
            }


            if (cbGPSTrack.Checked)
            {
                
                Form3 form3 = new Form3(XGPS, YGPS, MT);
                form3.Show();
            }

            if (cbSummary.Checked)
            {
                Form2 form2 = new Form2(MT);
                form2.Show();
            }
            if (txtReportConfig.Text != "Enter Template Name Here")
            {
                string path = Directory.GetCurrentDirectory();
                string newDir = "RCFG";
                string target = $@"{path}\{newDir}";
                Console.WriteLine("The current directory is {0}", path);
                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                    string output = JsonConvert.SerializeObject(RCFG);
                    Debug.WriteLine("Written");
                    Debug.WriteLine(output);
                    File.WriteAllText(@$"RCFG\{txtReportConfig.Text.Replace(" ", "_")}.json", output);
                }
                else
                {
                    string output = JsonConvert.SerializeObject(RCFG);
                    Debug.WriteLine("Written");
                    Debug.WriteLine(output);
                    File.WriteAllText(@$"RCFG\{txtReportConfig.Text.Replace(" ", "_")}.json", output);
                }
            }
            
            chartConfugration.SeriesList.Clear();
            Organizer.Clear();
            distinct.Clear();
            DeslectAllItems();
        }

        public static int GetBaseAlt(List<double> alt, List<byte> state)
        {
            for (int i = 0; i < state.Count; i++)
            {
                if (state[i] >= 2)
                {
                    return (int)alt[i];
                }
            }
            return -1;
        }

        public static int GetBaseAlt171(List<double> alt, List<sbyte> state)
        {
            for (int i = 0; i < state.Count; i++)
            {
                if (state[i] >= 2)
                {
                    return (int)alt[i];
                }
            }
            return -1;
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListBox lb = sender as ListBox;
            //Debug.WriteLine($"{lb.Name}.{lb.SelectedItem}");
        }

        private new void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox lb = sender as ListBox;
            //Debug.WriteLine($"{lb.Name}.{lb.SelectedItem} RowCount {tlpSeriesConfiguration.RowCount}");
            PopulatechartConfugration PopTemp = new PopulatechartConfugration();
            //tlpSeriesConfiguration.Visible = false;
            PopTemp.PopulateTelemtryTable(panel1,tlpSeriesConfiguration, lb.Name, lb.SelectedItem.ToString());
            tlpSeriesConfiguration.Visible = true;
            //SCL.Add(PopTemp);
        }

        private void btnSaveReportConfig_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string newDir = "RCFG";
            string target = $@"{path}\{newDir}";
            Console.WriteLine("The current directory is {0}", path);
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
                string output = JsonConvert.SerializeObject(RCFG);
                Debug.WriteLine(output);
            }
            else
            {
                string output = JsonConvert.SerializeObject(RCFG);
                Debug.WriteLine(output);
            }
        }

        private void btnLoadReportConfig_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string newDir = "RCFG";
            string target = $@"{path}\{newDir}";
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {

                InitialDirectory = target,
                Title = "Select Report Configuration File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "json files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clearTable(tlpSeriesConfiguration);
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                //var lines = File.ReadLines(@"RCFG\REPCFG.json");
                Debug.WriteLine("Read");
                Debug.WriteLine(lines[0]);
                ReportConfig RCFGin = JsonConvert.DeserializeObject<ReportConfig>(lines[0]);
                //todo populate telemetry table with file data
                PopulatechartConfugration pcfg = new PopulatechartConfugration();
                foreach (var sl in RCFGin.ChartList)
                {
                    foreach (var item in sl.SeriesList)
                    {
                        pcfg.LoadTelemtryTable(tlpSeriesConfiguration, item);
                    }
                }
            }
        }
        public void clearTable(TableLayoutPanel tbl)
        {
            tbl.SuspendLayout();
            int rc = tbl.RowCount;
            for (int j = rc-1; j > 0; j--)
            {
                Debug.WriteLine($"Jis {j}, Row Count is {tbl.RowCount}");
                for (int i = 0; i < tbl.ColumnCount; i++)
                {
                    Control Control = tbl.GetControlFromPosition(i, j);
                    tbl.Controls.Remove(Control);
                }
                tbl.RowStyles.RemoveAt(j);
                tbl.RowCount--;
            }
            tbl.ResumeLayout();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tbSizeY_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Application.Restart();
        }

        private void cbbLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbLogType.SelectedItem.ToString() == "Choose Log Type")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            MT.Clear();
            if(cbbLogType.SelectedItem.ToString() == "1.62a")
            {
                M.Clear();
                S.Clear();
                G1.Clear();
                G2.Clear();
                N.Clear();
                C.Clear();
            }
            else if (cbbLogType.SelectedItem.ToString() == "1.70")
            {
                M17.Clear();
                S17.Clear();
                G117.Clear();
                G217.Clear();
                N17.Clear();
                C17.Clear();
            }
        }
    }

    public class ModeTime
    {
        public Dictionary<byte, List<double>> Starts { get; set; }

        public Dictionary<byte, List<double>> Stops { get; set; }

        public Dictionary<byte, List<double>> Spans { get; set; }

        public Dictionary<byte, bool> Started { get; set; }
        public Dictionary<byte, string> Response { get; set; }
        public List<byte> ModeList { get; set; }
        public double TotalTime = 0;

        public ModeTime()
        {
            ModeList = new List<byte>()
            {
                1 ,2 ,3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
            };

            Response = new Dictionary<byte, string>()
            {
                {1, "Init" },
                {2, "Wait" },
                {3, "Manual" },
                {4, "Launching" },
                {5, "Climbing" },
                {6, "MB Land" },
                {7, "Landing" },
                {8, "Atti Hover" },
                {9, "A GPS" },
                {10, "Trans Out" },
                {11, "trans In" },
                {12, "Fixed Wing" },
                {13, "MFW" },
                {14, "M GPS" },
                {15, "Air Time" },
                {16, "Tach" },
                {17, "EP Control" },
                {18, "Full RC" }
            };

            Started = new Dictionary<byte, bool>()
            {
                {1, false },
                {2, false },
                {3, false },
                {4, false },
                {5, false },
                {6, false },
                {7, false },
                {8, false },
                {9, false },
                {10, false },
                {11, false },
                {12, false },
                {13, false },
                {14, false },
                {15, false },
                {16, false },
                {17, false },
                {18, false }

            };

            Spans = new Dictionary<byte, List<double>>()
            {
                {1, new List<double>() },
                {2, new List<double>() },
                {3, new List<double>() },
                {4, new List<double>() },
                {5, new List<double>() },
                {6, new List<double>() },
                {7, new List<double>() },
                {8, new List<double>() },
                {9, new List<double>() },
                {10, new List<double>() },
                {11, new List<double>() },
                {12, new List<double>() },
                {13, new List<double>() },
                {14, new List<double>() },
                {15, new List<double>() },
                {16, new List<double>() },
                {17, new List<double>() },
                {18, new List<double>() }
            };

            Stops = new Dictionary<byte, List<double>>()
            {
                {1, new List<double>() },
                {2, new List<double>() },
                {3, new List<double>() },
                {4, new List<double>() },
                {5, new List<double>() },
                {6, new List<double>() },
                {7, new List<double>() },
                {8, new List<double>() },
                {9, new List<double>() },
                {10, new List<double>() },
                {11, new List<double>() },
                {12, new List<double>() },
                {13, new List<double>() },
                {14, new List<double>() },
                {15, new List<double>() },
                {16, new List<double>() },
                {17, new List<double>() },
                {18, new List<double>() }
            };

            Starts = new Dictionary<byte, List<double>>()
            {
                {1, new List<double>() },
                {2, new List<double>() },
                {3, new List<double>() },
                {4, new List<double>() },
                {5, new List<double>() },
                {6, new List<double>() },
                {7, new List<double>() },
                {8, new List<double>() },
                {9, new List<double>() },
                {10, new List<double>() },
                {11, new List<double>() },
                {12, new List<double>() },
                {13, new List<double>() },
                {14, new List<double>() },
                {15, new List<double>() },
                {16, new List<double>() },
                {17, new List<double>() },
                {18, new List<double>() }

            };
        }
        public void Clear()
        {
            foreach (KeyValuePair<byte, List<double>> entry in Starts)
            {
                entry.Value.Clear();
            }
            foreach (KeyValuePair<byte, List<double>> entry in Stops)
            {
                entry.Value.Clear();
            }
            foreach (KeyValuePair<byte, List<double>> entry in Spans)
            {
                entry.Value.Clear();
            }
            Started = new Dictionary<byte, bool>()
            {
                {1, false },
                {2, false },
                {3, false },
                {4, false },
                {5, false },
                {6, false },
                {7, false },
                {8, false },
                {9, false },
                {10, false },
                {11, false },
                {12, false },
                {13, false },
                {14, false },
                {15, false },
                {16, false },
                {17, false },
                {18, false }

            };
        }
        public void GetPositions(
            List<double> X, 
            List<byte> Y, 
            List<float>  Pitch, 
            List<double> Alt, 
            int BaseAlt, 
            List<float> xAcc, 
            List<float> yAcc, 
            List<float> zAcc, 
            int Force, 
            int PadAlt, 
            int RPM, 
            List<float> PropSpeed, 
            List<bool> Control, 
            List<short> FullRC)
        {
            TotalTime = X.Max() - X.Min();
            for (int i = 0; i < Y.Count; i++)
            {
                ///////// airborn
                if (Y[i] >= 4 && (int)Pitch[i] > 60 && Alt[i] >= BaseAlt + PadAlt && Started[15] == false)
                {
                    Starts[15].Add((int)X[i]);
                    Started[15] = true;
                    Debug.WriteLine("Started Flying");
                    //Debug.WriteLine($"BAse alt {baseAlt}, PadAlt {AltPAd} Both {baseAlt + AltPAd}");
                }
                if (Y[i] < 4 && Started[15] == true)
                {
                    Stops[15].Add((int)X[i]);
                    Started[15] = false;
                    Debug.WriteLine("Stopped Flying");
                }
                else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[15] == true)
                {
                    Stops[15].Add((int)X[i]);
                    Started[15] = false;
                    Debug.WriteLine("Stopped Flying for crash");
                }
                else if (i == Y.Count - 1 && Started[15] == true)
                {
                    Stops[15].Add((int)X[i]);
                    Started[15] = false;
                    Debug.WriteLine("Stopped Flying for no ending");
                }
                ///////// RPM/Tach
                if (PropSpeed[i] >= RPM && Started[16] == false)
                {
                    Starts[16].Add((int)X[i]);
                    Started[16] = true;
                    Debug.WriteLine("St Engine");
                }
                if (PropSpeed[i] < RPM && Started[16] == true)
                {
                    Stops[16].Add((int)X[i]);
                    Started[16] = false;
                    Debug.WriteLine("Stopped Engine");
                }
                else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[16] == true)
                {
                    Stops[16].Add((int)X[i]);
                    Started[16] = false;
                    Debug.WriteLine("Stopped Engine for crash");
                }
                else if (i == Y.Count - 1 && Started[16] == true)
                {
                    Stops[16].Add((int)X[i]);
                    Started[16] = false;
                    Debug.WriteLine("Stopped Engine for no ending");
                }

                ///////// CGS Control
                if (Control[i] == false && Started[17] == false)
                {
                    Starts[17].Add((int)X[i]);
                    Started[17] = true;
                    Debug.WriteLine("Started CGS Control");
                }
                if (Control[i] == true && Started[17] == true)
                {
                    Stops[17].Add((int)X[i]);
                    Started[17] = false;
                    Debug.WriteLine("Stopped CGS Control");
                }
                else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[17] == true)
                {
                    Stops[17].Add((int)X[i]);
                    Started[17] = false;
                    Debug.WriteLine("Stopped CGS Control for crash");
                }
                else if (i == Y.Count - 1 && Started[17] == true)
                {
                    Stops[17].Add((int)X[i]);
                    Started[17] = false;
                    Debug.WriteLine("Stopped CGS Control for no ending");
                }

                /////// full RC
                if (FullRC[i] > 400 || FullRC[i]  <= 0 && Started[18] == false)
                {
                    Starts[18].Add((int)X[i]);
                    Started[18] = true;
                    //Debug.WriteLine("Stopped full RC Control");
                }
                if (FullRC[i] <= 400 && FullRC[i] > 0 && Started[18] == true)
                {
                    Stops[18].Add((int)X[i]);
                    Started[18] = false;
                    Debug.WriteLine("Started full RC Control");
                }
                else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[18] == true)
                {
                    Stops[18].Add((int)X[i]);
                    Started[18] = false;
                    Debug.WriteLine("Stopped full RC Control for crash");
                }
                else if (i == Y.Count - 1 && Started[18] == true)
                {
                    Stops[18].Add((int)X[i]);
                    Started[18] = false;
                    Debug.WriteLine("Stopped full RC Control for no ending");
                }

                /////// MGPS
                if (Y[i] == 14 && (int)Pitch[i] > 60 && Alt[i] >= BaseAlt + PadAlt && Started[14] == false)
                {
                    Starts[14].Add((int)X[i]);
                    Started[14] = true;
                    Debug.WriteLine($"Started {Response[14]}");
                    //Debug.WriteLine($"BAse alt {baseAlt}, PadAlt {AltPAd} Both {baseAlt + AltPAd}");
                }
                if (Y[i] != 14 && Started[14] == true)
                {
                    Stops[14].Add((int)X[i]);
                    Started[14] = false;
                    Debug.WriteLine($"Stopped {Response[14]}");
                }
                else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[14] == true)
                {
                    Stops[14].Add((int)X[i]);
                    Started[14] = false;
                    Debug.WriteLine($"Stopped {Response[14]} for crash");
                }
                else if (i == Y.Count - 1 && Started[4] == true)
                {
                    Stops[14].Add((int)X[i]);
                    Started[14] = false;
                    Debug.WriteLine($"Stopped {Response[14]} for no ending");
                }
                //Debug.WriteLine("In I");
                for (byte j = 1; j < ModeList.Count; j++)
                {
                   // Debug.WriteLine("In J");
                    if (Y[i] == 4 || Y[i] == 5 || Y[i] == 8)
                    {
                        
                        //Debug.WriteLine($"I {i} J {j}");
                        if (Y[i] == j && (int)Pitch[i] > 60 && Alt[i] >= BaseAlt + PadAlt && Started[j] == false)
                        {
                            Starts[j].Add((int)X[i]);
                            Started[j] = true;
                            Debug.WriteLine($"Started {Response[j]}");
                            //Debug.WriteLine($"BAse alt {baseAlt}, PadAlt {AltPAd} Both {baseAlt + AltPAd}");
                        }
                        if (Y[i] != j && Started[j] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]}");
                        }
                        else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[j] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]} for crash");
                        }
                        else if (i == Y.Count - 1 && Started[4] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]} for no ending");
                        }
                    }
                    else
                    {
                        //Debug.WriteLine($"I {i} J {j}");
                        if (Y[i] == j && Started[j] == false)
                        {
                            Starts[j].Add((int)X[i]);
                            Started[j] = true;
                            Debug.WriteLine($"Started {Response[j]}");
                        }
                        if (Y[i] != j && Started[j] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]}");
                        }
                        else if (xAcc[i] <= Force || yAcc[i] <= Force || zAcc[i] <= Force && Started[j] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]} for crash");
                        }
                        else if (i == Y.Count - 1 && Started[j] == true)
                        {
                            Stops[j].Add((int)X[i]);
                            Started[j] = false;
                            Debug.WriteLine($"Stopped {Response[j]} for no ending");
                        }
                    }
                }
            }
        }
    }
    public class ReportConfig
    {
        public string Name { get; set; }
        public int FromLine { get; set; } = -1;
        public int ToLine { get; set; } = -1;
        public int ChartX { get; set; }
        public int ChartY { get; set; }
        public List<ChartConfig> ChartList { get; set; } = new List<ChartConfig>();
    }

    public class ChartConfig
    {
        public string Name { get; set; }
        public decimal ID { get; set; }
        public List<SeriesConfig> SeriesList { get; set; } = new List<SeriesConfig>();

        public void Save()
        {

        }
        public void Load()
        {

        }
    }


    public class SeriesConfig
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool AnnotationShowLabel { get; set; } = false;
        public bool AuxYAxis { get; set; } = false;
        public string GroupName { get; set; }
        public decimal ChartID  = 0;
        public string AnnotationType { get; set; }
        public string AnnotationOperator { get; set; }
        public string AnnotationFrequency { get; set; }
        public string AnnotationTransform { get; set; }
        public string AnnotationThreshold { get; set; }
        public string Parent { get; set; }
        public string Source { get; set; }
        public bool Hidden { get; set; } = false;

        public SeriesConfig()
        {

        }
        public SeriesConfig(
            string name,
            string alias,
            string annotationTransform,
            bool auxYAxis,
            string annotationType,
            string annotationOperator,
            string annotationThreshold,
            string annotationFrequency,
            string groupName,
            decimal chartid,
            //bool annotationShowLabel,
            string parent
            //string source
            )
        {
            Name = name;
            Alias = alias;
            AnnotationTransform = annotationTransform;
            AuxYAxis = auxYAxis;
            AnnotationType = annotationType;
            AnnotationOperator = annotationOperator;
            AnnotationThreshold = annotationThreshold;
            AnnotationFrequency = annotationFrequency;
            GroupName = groupName;
            ChartID = chartid;
            //AnnotationShowLabel = annotationShowLabel;
            Parent = parent;
            //Source = source;
        }
    }

    public class ParseTLV
    {
        public int snsrLen = 650; // 658 length+tag+len// length of payload in bytes 
        public int guidLen = 142; // 150
        public int modeLen = 18;  // 026
        public int gcsdLen = 118; // 126
        public int ctrlLen = 160; // 168
        public int navdLen = 186; // 194

        public int snsrLen17 = 650; // 658 length+tag+len// length of payload in bytes 
        public int guidLen17 = 146; // 154
        public int modeLen17 = 19;  // 027
        public int gcsdLen17 = 118; // 126
        public int ctrlLen17 = 161; // 169
        public int navdLen17 = 186; // 194

        public int LogLinelength = 1322; //total len bytes per composite packet
        public int LogLinelength17 = 1328; //total len bytes per composite packet
        byte[] payload = new byte[1];
        const int TagLen = 4;
        const int LengthLen = 4;
        List<int> LengthList { get; set; }
        List<int> LengthList17 { get; set; }

        public Dictionary<string, int> TLVDict { get; set; }
        public Dictionary<string, int> TLVDict17 { get; set; }

        public ParseTLV()
        {
            // carried over from python
            // d = double 8  double
            // f = Float  4  float
            // ? = bool   1  bool
            // h = short  2  short
            // H = ushort 2  ushort
            // B = uChar  1  byte
            // b = char   1  sbyte
            // i = int    4  int
            // I = uInt   4  uint

            LengthList = new List<int>() { snsrLen, guidLen, modeLen, gcsdLen, ctrlLen, navdLen };
            LengthList17 = new List<int>() { snsrLen17, guidLen17, modeLen17, gcsdLen17, ctrlLen17, navdLen17 };
            TLVDict = new Dictionary<string, int>
            {
                {"Snsr",snsrLen },
                {"Guid",guidLen },
                {"Mode",modeLen },
                {"Gcsd",gcsdLen },
                {"Ctrl",ctrlLen },
                {"Navd",navdLen }
            };

            TLVDict17 = new Dictionary<string, int>
            {
                {"Snsr",snsrLen17 },
                {"Guid",guidLen17 },
                {"Mode",modeLen17 },
                {"Gcsd",gcsdLen17 },
                {"Ctrl",ctrlLen17 },
                {"Navd",navdLen17 }
            };
        }

        public void DemuxToMemory(string path, ProgressBar p, Label l, string version)
        {
            Debug.WriteLine($"Version : {version}");
            if (version == "1.62a")
            {
                Queue<Tuple<int, byte[]>> q = new Queue<Tuple<int, byte[]>>();
                //Queue<byte[]> q = new Queue<byte[]>();
                int completePct = 0;
                int LineCount = 0;
                using (var stream = File.OpenRead(path))
                {
                    Debug.WriteLine($"Number of Lines: {stream.Length / LogLinelength} Clean: {stream.Length % LogLinelength}");
                    Debug.WriteLine(stream.Length);
                    byte[] LogLines = new byte[stream.Length / LogLinelength];
                    p.Maximum = 100;
                    int pctNow = 0;
                    int oldPct = 0;
                    while (stream.Length != stream.Position)
                    {

                        float pctComp = ((float)stream.Position / (float)stream.Length) * 100;
                        pctNow = (int)pctComp;
                        if (pctNow % 2 == 0 && pctNow != oldPct)
                        {
                            p.Value += 2;
                            completePct += 2;
                            l.Text = $"{pctNow} %";
                            l.Update();
                            //Debug.WriteLine(pctNow);
                            oldPct = pctNow;
                        }

                        //FillClasses(stream, S, G1, M, G2, C, N);
                        byte[] rawBytes = new byte[LogLinelength];
                        stream.Read(rawBytes, 0, LogLinelength);

                        //byte[] SuperLine = new byte[LogLinelength];
                        //stream.Read(SuperLine, 0, LogLinelength);
                        Tuple<int, byte[]> SuperLine = Tuple.Create(LineCount, rawBytes);
                        q.Enqueue(SuperLine);
                        ++LineCount;
                        FillSuperLine(q.Dequeue());
                        
                    }
                }
                l.Text = string.Empty;
                LineCount = 0;
                //List<Tuple<TimeSpan, string, string>> AFS = Form1.GetFlightSpan(Form1.M.linux_time_s, Form1.M.flight_state_tlm, "Auto Flight");
                //Debug.WriteLine($"{AFS.Last().Item3} Span: {AFS.Last().Item1.ToString()}, Time: {AFS.Last().Item2}");

                //List<Tuple<TimeSpan, string, string>> EFS = Form1.GetEngineSpan(Form1.S.linux_time_s, Form1.S.prop_rpm, "Engine On");
                //Debug.WriteLine($"{EFS.Last().Item3} Span: {EFS.Last().Item1.ToString()}, Time: {EFS.Last().Item2}");
            }
            else
            {
                //Queue<Tuple<int, byte[]>> q = new Queue<Tuple<int, byte[]>>();
                ////Queue<byte[]> q = new Queue<byte[]>();
                //int completePct = 0;
                //int LineCount = 0;
                //byte[] LogLines;
                //using (var stream = File.OpenRead(path))
                //{
                //    Debug.WriteLine($"Number of Lines 17: {stream.Length / LogLinelength17} Clean: {stream.Length % LogLinelength17}");
                //    Debug.WriteLine(stream.Length);
                //    if(stream.Length % LogLinelength17 == 0)
                //    {
                //        LogLines = new byte[stream.Length / LogLinelength17];
                //    }
                //    else
                //    {
                //        LogLines = new byte[stream.Length / LogLinelength17 - 1];
                //    }
                //    //byte[] LogLines = new byte[stream.Length / LogLinelength17];
                //    p.Maximum = 100;
                //    int pctNow = 0;
                //    int oldPct = 0;
                //    while (stream.Length != stream.Position)
                //    {

                //        float pctComp = ((float)stream.Position / (float)stream.Length) * 100;
                //        pctNow = (int)pctComp;
                //        if (pctNow % 2 == 0 && pctNow != oldPct)
                //        {
                //            p.Value += 2;
                //            completePct += 2;
                //            l.Text = $"{pctNow} %";
                //            l.Update();
                //            //Debug.WriteLine(pctNow);
                //            oldPct = pctNow;
                //        }

                //        //FillClasses(stream, S, G1, M, G2, C, N);
                //        byte[] rawBytes = new byte[LogLinelength17];
                //        stream.Read(rawBytes, 0, LogLinelength17);

                //        //byte[] SuperLine = new byte[LogLinelength];
                //        //stream.Read(SuperLine, 0, LogLinelength);
                //        Tuple<int, byte[]> SuperLine = Tuple.Create(LineCount, rawBytes);
                //        q.Enqueue(SuperLine);
                //        ++LineCount;
                //        FillSuperLine17(q.Dequeue());

                //    }
                //}
                //l.Text = string.Empty;
                //LineCount = 0;

                int completePct = 0;
                //int LineCount = 0;
                using (var stream = File.OpenRead(path))
                {
                    Debug.WriteLine($"Number of Lines 17: {stream.Length / LogLinelength17} Clean: {stream.Length % LogLinelength17}");
                    Debug.WriteLine(stream.Length);
                    byte[] LogLines = new byte[stream.Length / LogLinelength17]; 
                    p.Maximum = 100;
                    int pctNow = 0;
                    int oldPct = 0;
                    while (stream.Length != stream.Position)
                    {

                        float pctComp = ((float)stream.Position / (float)stream.Length) * 100;
                        pctNow = (int)pctComp;
                        if (pctNow % 2 == 0 && pctNow != oldPct)
                        {

                            p.Value += 2;
                            completePct += 2;
                            l.Text = $"{pctNow} %";
                            l.Update();
                            //Debug.WriteLine(pctNow);
                            oldPct = pctNow;
                        }

                        //FillClasses(stream, S17, G117, M17, G217, C17, N17);
                        FillClasses(stream);
                    }
                }
                l.Text = string.Empty;
                //LineCount = 0;
                //List<Tuple<TimeSpan, string, string>> AFS = Form1.GetFlightSpan(Form1.M17.linux_time_s, Form1.M17.flight_state_tlm, "Auto Flight");
                //Debug.WriteLine($"{AFS.Last().Item3} Span: {AFS.Last().Item1.ToString()}, Time: {AFS.Last().Item2}");

                //List<Tuple<TimeSpan, string, string>> EFS = Form1.GetEngineSpan(Form1.S17.linux_time_s, Form1.S17.prop_rpm, "Engine On");
                //Debug.WriteLine($"{EFS.Last().Item3} Span: {EFS.Last().Item1.ToString()}, Time: {EFS.Last().Item2}");
            }

        }

        public void FillSuperLine17(Tuple<int, byte[]> stream)
        {
            bool exit = false;
            int start = 0;
            int finish = 4;
            for (int i = 0; i < LengthList17.Count; i++)
            {
                byte[] TagBytes = stream.Item2.Slice(start, finish);
                start += TagLen;
                finish += LengthLen;
                byte[] len = stream.Item2.Slice(start, finish);
                start += LengthLen;
                finish += LengthList17[i];
                byte[] val = stream.Item2.Slice(start, finish);
                start += LengthList17[i];
                finish += TagLen;
                string TagText = Encoding.ASCII.GetString(TagBytes, 0, 4);
                Tuple<int, byte[]> ClassLine = Tuple.Create(stream.Item1, val);
                switch (TagText)
                {
                    case "Ctrl":
                        Form1.C17.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Snsr":
                        Form1.S17.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Mode":
                        Form1.M17.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Navd":
                        Form1.N17.ReadLogLine(ClassLine.Item2);
                        break;
                    case "GCSd":
                        Form1.G217.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Guid":
                        Form1.G117.ReadLogLine(ClassLine.Item2);
                        break;

                }

                if (exit) break;
                //Debug.WriteLine($"count has {Form1.G2.lost_comm_fault_HS.Count}");
                //Debug.WriteLine($"Tag: {rb}, Index: {stream.Item1}, Len: {stream.Item2.Length}");
            }
            //Debug.WriteLine("Done2");
        }

        public void FillSuperLine(Tuple<int,byte[]> stream)
        {
            bool exit = false;
            int start = 0;
            int finish = 4;
            for (int i = 0; i < LengthList.Count; i++)
            {
                byte[] TagBytes = stream.Item2.Slice(start, finish);
                start += TagLen;
                finish += LengthLen;
                byte[] len = stream.Item2.Slice(start, finish);
                start += LengthLen;
                finish += LengthList[i];
                byte[] val = stream.Item2.Slice(start, finish);
                start += LengthList[i];
                finish += TagLen;
                string TagText = Encoding.ASCII.GetString(TagBytes, 0, 4);
                Tuple<int, byte[]> ClassLine = Tuple.Create(stream.Item1, val);
                switch (TagText)
                {
                    case "Ctrl":
                        Form1.C.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Snsr":
                        Form1.S.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Mode":
                        Form1.M.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Navd":
                        Form1.N.ReadLogLine(ClassLine.Item2);
                        break;
                    case "GCSd":
                        Form1.G2.ReadLogLine(ClassLine.Item2);
                        break;
                    case "Guid":
                        Form1.G1.ReadLogLine(ClassLine.Item2);
                        break;

                }
                
                if (exit) break;
                //Debug.WriteLine($"count has {Form1.G2.lost_comm_fault_HS.Count}");
                //Debug.WriteLine($"Tag: {rb}, Index: {stream.Item1}, Len: {stream.Item2.Length}");
            }
            //Debug.WriteLine("Done2");
        }

        public void FillClasses( Stream stream)
        {

            string Tag = ReadTag(stream);
            Int32 l = ReadLength(stream);
            //Debug.WriteLine($"Tag: {Tag} Len: {l}");
            //ReadValue(stream, l);
            switch (Tag)
            {
                case "Snsr":
                    Form1.S17.ReadBytesData(stream);
                    break;
                case "Guid":
                    Form1.G117.ReadBytesData(stream);
                    break;
                case "Mode":
                    Form1.M17.ReadBytesData(stream);
                    break;
                case "GCSd":
                    Form1.G217.ReadBytesData(stream);
                    break;
                case "Ctrl":
                    Form1.C17.ReadBytesData(stream);
                    break;
                case "Navd":
                    Form1.N17.ReadBytesData(stream);
                    break;
            }
        }
        public string ReadTag(Stream stream)
        {
            var rawBytes = new byte[4];
            stream.Read(rawBytes, 0, 4);
            string rb = Encoding.ASCII.GetString(rawBytes, 0, 4);
            //Debug.WriteLine(rb);
            return rb;
        }
        public Int32 ReadLength(Stream stream)
        {
            var rawBytes = new byte[4];
            stream.Read(rawBytes, 0, 4);
            Int32 num = BitConverter.ToInt32(rawBytes, 0);
            //Debug.WriteLine(num);
            return num;
        }
        public void ReadValue(Stream stream, Int32 len)
        {
            var rawBytes = new byte[len];
            stream.Read(rawBytes, 0, len);
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
    }

    public class PopulatechartConfugration
    {
        public List<string> Type { get; set; } = new List<string>() {
            string.Empty,
            "Horizontal",
            "Vertical",
            "Box",
            "Comment"
        };
        public List<string> Operator { get; set; } = new List<string>() {
            string.Empty,
            "==",
            "<=",
            ">="
        };
        public List<string> Occurence { get; set; } = new List<string>() {
            string.Empty,
            "First",
            "Last",
            "First+Last",
            "All"
        };
        public List<string> Filter { get; set; } = new List<string>() {
            string.Empty,
            "M -> F",
            "KM -> NM",
            "Deg -> Rad",
            "Rad -> Deg"
        };
        //public void PopulateTelemtryTable(TableLayoutPanel tbl, string parent, string telemetry)
        public void PopulateTelemtryTable(Panel P, TableLayoutPanel tbl, string parent, string telemetry)
        {
            //tbl.Visible = false;
            
            tbl.SuspendLayout();
            //tbl.Update();
            RowStyle temp = tbl.RowStyles[tbl.RowCount - 1];
            //increase panel rows count by one
            tbl.RowCount++;
            //add a new RowStyle as a copy of the previous one
            tbl.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //add your three controls column row
            
            tbl.Controls.Add(new CheckBox() { Text = telemetry, Checked = true, Tag = parent, Size = new Size(244, 23), Dock = DockStyle.Fill, Name = $"lblTelemetry_{telemetry}_{tbl.RowCount - 1}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 10f)}, 0, tbl.RowCount - 1); // loop tpget tel in here
            tbl.Controls.Add(new TextBox() { Size = new Size(59, 23), Dock = DockStyle.Fill, Name = $"txtAlias_{telemetry}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 1, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Filter), Dock = DockStyle.Fill, Name = $"cbbFilter_{telemetry}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 2, tbl.RowCount - 1);
            tbl.Controls.Add(new CheckBox() { Text = string.Empty, Dock = DockStyle.Fill, Name = $"cbAddY_{telemetry}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 3, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Type), Dock = DockStyle.Fill, Name = $"cbbType_{telemetry}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 4, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Operator), Dock = DockStyle.Fill, Name = $"cbbOperator_{telemetry}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 5, tbl.RowCount - 1);
            tbl.Controls.Add(new TextBox() { Size = new Size(59, 23), Dock = DockStyle.Fill, Name = $"txtThresh_{telemetry}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 6, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Occurence), Dock = DockStyle.Fill, Name = $"cbbOccurence_{telemetry}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 7, tbl.RowCount - 1);
            tbl.Controls.Add(new TextBox() { Size = new Size(59, 23), Dock = DockStyle.Fill, Name = $"txtGroupName_{telemetry}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 8, tbl.RowCount - 1);
            tbl.Controls.Add(new NumericUpDown() { Name = $"nudChartID_{telemetry}_{tbl.RowCount - 1}",TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 9, tbl.RowCount - 1);
            //tbl.Controls.Add(new NumericUpDown() { Dock = DockStyle.Fill, Name = $"nudChartID_{telemetry}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 9, tbl.RowCount - 1);
            tbl.ResumeLayout();
            //tbl.Visible = true;
        }
        public void LoadTelemtryTable(TableLayoutPanel tbl, SeriesConfig sc)
        {
            //tbl.Visible = false;
            //string name,
            //string alias,
            //string annotationTransform,
            //bool auxYAxis,
            //string annotationType,
            //string annotationOperator,
            //string annotationThreshold,
            //string annotationFrequency,
            //string groupName,
            //decimal chartid,
            tbl.SuspendLayout();
            //tbl.Update();
            RowStyle temp = tbl.RowStyles[tbl.RowCount - 1];
            //increase panel rows count by one
            tbl.RowCount++;
            //add a new RowStyle as a copy of the previous one
            tbl.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //add your three controls column row

            tbl.Controls.Add(new CheckBox() { Text = sc.Name, Checked = true, Tag = sc.Parent, Size = new Size(244, 23), Dock = DockStyle.Fill, Name = $"lblTelemetry_{sc.Name}_{tbl.RowCount - 1}", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 10f) }, 0, tbl.RowCount - 1); // loop tpget tel in here
            tbl.Controls.Add(new TextBox() { Text = sc.Alias, Size = new Size(59, 23), Dock = DockStyle.Fill, Name = $"txtAlias_{sc.Name}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 1, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Filter), SelectedItem = sc.AnnotationTransform,Dock = DockStyle.Fill, Name = $"cbbFilter_{sc.Name}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 2, tbl.RowCount - 1);
            tbl.Controls.Add(new CheckBox() { Text = string.Empty, Checked = sc.AuxYAxis, Dock = DockStyle.Fill, Name = $"cbAddY_{sc.Name}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 3, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Type), SelectedItem = sc.AnnotationType, Dock = DockStyle.Fill, Name = $"cbbType_{sc.Name}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 4, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Operator), SelectedItem = sc.AnnotationOperator, Dock = DockStyle.Fill, Name = $"cbbOperator_{sc.Name}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 5, tbl.RowCount - 1);
            tbl.Controls.Add(new TextBox() { Size = new Size(59, 23), Text = sc.AnnotationThreshold, Dock = DockStyle.Fill, Name = $"txtThresh_{sc.Name}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 6, tbl.RowCount - 1);
            tbl.Controls.Add(new ComboBox() { DataSource = new List<string>(Occurence), SelectedItem = sc.AnnotationFrequency, Dock = DockStyle.Fill, Name = $"cbbOccurence_{sc.Name}_{tbl.RowCount - 1}", Font = new Font("Microsoft Sans Serif", 10f) }, 7, tbl.RowCount - 1);
            tbl.Controls.Add(new TextBox() { Size = new Size(59, 23), Dock = DockStyle.Fill, Text = sc.GroupName, Name = $"txtGroupName_{sc.Name}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 8, tbl.RowCount - 1);
            tbl.Controls.Add(new NumericUpDown() { Dock = DockStyle.Fill, Value = sc.ChartID, Name = $"nudChartID_{sc.Name}_{tbl.RowCount - 1}", TextAlign = HorizontalAlignment.Center, Font = new Font("Microsoft Sans Serif", 10f) }, 9, tbl.RowCount - 1);
            tbl.ResumeLayout();
            //tbl.Visible = true;
        }

    }

    public static class Reader
    {
        public static double ReadDouble(Stream stream)
        {
            var rawBytes = new byte[8];
            stream.Read(rawBytes, 0, 8);
            //Debug.WriteLine(BitConverter.ToDouble(rawBytes, 0));
            return BitConverter.ToDouble(rawBytes, 0);
        }
        public static float ReadFloat(Stream stream)
        {
            var rawBytes = new byte[4];
            stream.Read(rawBytes, 0, 4);
            //Debug.WriteLine(BitConverter.ToDouble(rawBytes, 0));
            return BitConverter.ToSingle(rawBytes, 0);
        }
        public static byte ReadByte(Stream stream)
        {
            // uint8
            byte[] rawBytes = new byte[1];
            stream.Read(rawBytes, 0, 1);
            return rawBytes[0];
        }
        public static sbyte ReadSByte(Stream stream)
        {
            // int8
            byte[] rawBytes = new byte[1];
            stream.Read(rawBytes, 0, 1);
            return (sbyte)rawBytes[0];
        }
        public static Int16 ReadShort(Stream stream)
        {
            var rawBytes = new byte[2];
            stream.Read(rawBytes, 0, 2);
            return BitConverter.ToInt16(rawBytes, 0);
        }
        public static Int32 ReadInt(Stream stream)
        {
            var rawBytes = new byte[4];
            stream.Read(rawBytes, 0, 4);
            return BitConverter.ToInt32(rawBytes, 0);
        }
        public static UInt32 ReadUInt(Stream stream)
        {
            var rawBytes = new byte[4];
            stream.Read(rawBytes, 0, 4);
            return BitConverter.ToUInt32(rawBytes, 0);
        }
        public static UInt16 ReadUShort(Stream stream)
        {
            var rawBytes = new byte[2];
            stream.Read(rawBytes, 0, 2);
            return BitConverter.ToUInt16(rawBytes, 0);
        }
        public static bool ReadBool(Stream stream)
        {
            byte[] rawBytes = new byte[1];
            stream.Read(rawBytes, 0, 1);
            return Convert.ToBoolean((sbyte)rawBytes[0]);
        }
    }

    public class ControlData
    {

        public int totalLength = 160;
        public int dataLength = 22;

        public List<double> linux_time_s { get; set; }
        public List<double> elevator_degrees { get; set; }
        public List<double> throttle_degrees { get; set; }
        public List<double> rudder_degrees { get; set; }
        public List<double> aileron_degrees { get; set; }
        public List<double> roll_degrees { get; set; }
        public List<double> alt_error { get; set; }
        public List<double> vel_err_Prime_ftps1 { get; set; }
        public List<double> vel_err_Prime_ftps2 { get; set; }
        public List<double> pos_err_Prime_ft1 { get; set; }

        public List<double> pos_err_Prime_ft2 { get; set; }
        public List<double> integ_alt_err_deg { get; set; }
        public List<double> theta_cmd_deg { get; set; }
        public List<double> phi_cmd_deg { get; set; }
        public List<double> hv_att_error_deg1 { get; set; }
        public List<double> hv_att_error_deg2 { get; set; }
        public List<double> hv_att_error_deg3 { get; set; }
        public List<double> hv_int_psi_error_deg { get; set; }
        public List<float> speed_err_int_kt { get; set; }
        public List<float> attHv_integ1 { get; set; }

        public List<float> attHv_integ2 { get; set; }
        public List<float> attHv_integ3 { get; set; }
        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public ControlData()
        {
            //linux_time_s = new List<int>();
            linux_time_s = new List<double>();
            elevator_degrees = new List<double>();
            throttle_degrees = new List<double>();
            rudder_degrees = new List<double>();
            aileron_degrees = new List<double>();
            roll_degrees = new List<double>();
            alt_error = new List<double>();
            vel_err_Prime_ftps1 = new List<double>();
            vel_err_Prime_ftps2 = new List<double>();
            pos_err_Prime_ft1 = new List<double>();

            pos_err_Prime_ft2 = new List<double>();
            integ_alt_err_deg = new List<double>();
            theta_cmd_deg = new List<double>();
            phi_cmd_deg = new List<double>();
            hv_att_error_deg1 = new List<double>();
            hv_att_error_deg2 = new List<double>();
            hv_att_error_deg3 = new List<double>();
            hv_int_psi_error_deg = new List<double>();
            speed_err_int_kt = new List<float>();
            attHv_integ1 = new List<float>();

            attHv_integ2 = new List<float>();
            attHv_integ3 = new List<float>();

        }

        public void Clear()
        {
            linux_time_s.Clear();
            elevator_degrees.Clear();
            throttle_degrees.Clear();
            rudder_degrees.Clear();
            aileron_degrees.Clear();
            roll_degrees.Clear();
            alt_error.Clear();
            vel_err_Prime_ftps1.Clear();
            vel_err_Prime_ftps2.Clear();
            pos_err_Prime_ft1.Clear();

            pos_err_Prime_ft2.Clear();
            integ_alt_err_deg.Clear();
            theta_cmd_deg.Clear();
            phi_cmd_deg.Clear();
            hv_att_error_deg1.Clear();
            hv_att_error_deg2.Clear();
            hv_att_error_deg3.Clear();
            hv_int_psi_error_deg.Clear();
            speed_err_int_kt.Clear();
            attHv_integ1.Clear();

            attHv_integ2.Clear();
            attHv_integ3.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            //Debug.WriteLine(input.Length.ToString());
            linux_time_s.Add(BitConverter.ToDouble(input, start));
            elevator_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            throttle_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            rudder_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            aileron_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            roll_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_error.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_err_Prime_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_err_Prime_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_err_Prime_ft1.Add(BitConverter.ToDouble(input, start += doubleVal));

            pos_err_Prime_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            integ_alt_err_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            theta_cmd_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            phi_cmd_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg1.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg2.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg3.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_int_psi_error_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            speed_err_int_kt.Add(BitConverter.ToSingle(input, start += doubleVal));
            attHv_integ1.Add(BitConverter.ToSingle(input, start += floatVal));

            attHv_integ2.Add(BitConverter.ToSingle(input, start += floatVal));
            attHv_integ3.Add(BitConverter.ToSingle(input, start += floatVal));
            //Debug.WriteLine("Filled Class");
        }
        public void ReadBytesData(Stream stream)
        {
            //linux_time_s.Add(Reader.ReadInt(stream));
            linux_time_s.Add(Reader.ReadDouble(stream));
            elevator_degrees.Add(Reader.ReadDouble(stream));
            throttle_degrees.Add(Reader.ReadDouble(stream));
            rudder_degrees.Add(Reader.ReadDouble(stream));
            aileron_degrees.Add(Reader.ReadDouble(stream));
            roll_degrees.Add(Reader.ReadDouble(stream));
            alt_error.Add(Reader.ReadDouble(stream));
            vel_err_Prime_ftps1.Add(Reader.ReadDouble(stream));
            vel_err_Prime_ftps2.Add(Reader.ReadDouble(stream));
            pos_err_Prime_ft1.Add(Reader.ReadDouble(stream));

            pos_err_Prime_ft2.Add(Reader.ReadDouble(stream));
            integ_alt_err_deg.Add(Reader.ReadDouble(stream));
            theta_cmd_deg.Add(Reader.ReadDouble(stream));
            phi_cmd_deg.Add(Reader.ReadDouble(stream));
            hv_att_error_deg1.Add(Reader.ReadDouble(stream));
            hv_att_error_deg2.Add(Reader.ReadDouble(stream));
            hv_att_error_deg3.Add(Reader.ReadDouble(stream));
            hv_int_psi_error_deg.Add(Reader.ReadDouble(stream));
            speed_err_int_kt.Add(Reader.ReadFloat(stream));
            attHv_integ1.Add(Reader.ReadFloat(stream));

            attHv_integ2.Add(Reader.ReadFloat(stream));
            attHv_integ3.Add(Reader.ReadFloat(stream));
        }

    }


    public class NavigationData
    {

        public int totalLength = 186;
        public int dataLength = 41;

        public List<double> linux_time_s { get; set; }
        public List<float> est_V_wind_N_ftps { get; set; }
        public List<float> est_V_wind_E_ftps { get; set; }
        public List<float> P11_V_wind_N { get; set; }
        public List<float> P22_V_wind_E { get; set; }
        public List<float> P33_IAS_bias { get; set; }
        public List<float> TAS_meas_residual_ftps { get; set; }
        public List<float> rho_kg_m3 { get; set; }
        public List<float> est_IAS_bias_ftps { get; set; }
        public List<float> est_wind_dir_deg { get; set; }

        public List<float> est_wind_speed_kts { get; set; }
        public List<float> density_alt_ft { get; set; }
        public List<uint> tag_age_body_ms { get; set; }
        public List<float> tag_pos_B_T_B_m1 { get; set; }
        public List<float> tag_pos_B_T_B_m2 { get; set; }
        public List<float> tag_pos_B_T_B_m3 { get; set; }
        public List<uint> tag_age_ned_ms { get; set; }
        public List<float> tag_pos_B_T_NED_m1 { get; set; }
        public List<float> tag_pos_B_T_NED_m2 { get; set; }
        public List<float> tag_pos_B_T_NED_m3 { get; set; }

        public List<float> tag_vel_T_B_NED_m_s1 { get; set; }
        public List<float> tag_vel_T_B_NED_m_s2 { get; set; }
        public List<float> tag_vel_T_B_NED_m_s3 { get; set; }
        public List<int> accum_body_upd { get; set; }
        public List<int> accum_ned_upd { get; set; }
        public List<sbyte> tracking { get; set; }
        public List<double> pos_B_T_NED_ft1 { get; set; }
        public List<double> pos_B_T_NED_ft2 { get; set; }
        public List<double> pos_B_T_NED_ft3 { get; set; }
        public List<double> vel_T_B_NED_ftps1 { get; set; }

        public List<double> vel_T_B_NED_ftps2 { get; set; }
        public List<double> vel_T_B_NED_ftps3 { get; set; }
        public List<float> est_pos_B_T_NED_ft1 { get; set; }
        public List<float> est_pos_B_T_NED_ft2 { get; set; }
        public List<float> est_pos_B_T_NED_ft3 { get; set; }
        public List<float> est_vel_T_B_NED_ftps1 { get; set; }
        public List<float> est_vel_T_B_NED_ftps2 { get; set; }
        public List<float> est_vel_T_B_NED_ftps3 { get; set; }
        public List<sbyte> altitude_low { get; set; }
        public List<float> V_min_kts { get; set; }

        public List<float> est_mass_lbm { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public NavigationData()
        {
            linux_time_s = new List<double>();
            est_V_wind_N_ftps = new List<float>();
            est_V_wind_E_ftps = new List<float>();
            P11_V_wind_N = new List<float>();
            P22_V_wind_E = new List<float>();
            P33_IAS_bias = new List<float>();
            TAS_meas_residual_ftps = new List<float>();
            rho_kg_m3 = new List<float>();
            est_IAS_bias_ftps = new List<float>();
            est_wind_dir_deg = new List<float>();

            est_wind_speed_kts = new List<float>();
            density_alt_ft = new List<float>();
            tag_age_body_ms = new List<uint>();
            tag_pos_B_T_B_m1 = new List<float>();
            tag_pos_B_T_B_m2 = new List<float>();
            tag_pos_B_T_B_m3 = new List<float>();
            tag_age_ned_ms = new List<uint>();
            tag_pos_B_T_NED_m1 = new List<float>();
            tag_pos_B_T_NED_m2 = new List<float>();
            tag_pos_B_T_NED_m3 = new List<float>();

            tag_vel_T_B_NED_m_s1 = new List<float>();
            tag_vel_T_B_NED_m_s2 = new List<float>();
            tag_vel_T_B_NED_m_s3 = new List<float>();
            accum_body_upd = new List<int>();
            accum_ned_upd = new List<int>();
            tracking = new List<sbyte>();
            pos_B_T_NED_ft1 = new List<double>();
            pos_B_T_NED_ft2 = new List<double>();
            pos_B_T_NED_ft3 = new List<double>();
            vel_T_B_NED_ftps1 = new List<double>();

            vel_T_B_NED_ftps2 = new List<double>();
            vel_T_B_NED_ftps3 = new List<double>();
            est_pos_B_T_NED_ft1 = new List<float>();
            est_pos_B_T_NED_ft2 = new List<float>();
            est_pos_B_T_NED_ft3 = new List<float>();
            est_vel_T_B_NED_ftps1 = new List<float>();
            est_vel_T_B_NED_ftps2 = new List<float>();
            est_vel_T_B_NED_ftps3 = new List<float>();
            altitude_low = new List<sbyte>();
            V_min_kts = new List<float>();

            est_mass_lbm = new List<float>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            est_V_wind_N_ftps.Clear();
            est_V_wind_E_ftps.Clear();
            P11_V_wind_N.Clear();
            P22_V_wind_E.Clear();
            P33_IAS_bias.Clear();
            TAS_meas_residual_ftps.Clear();
            rho_kg_m3.Clear();
            est_IAS_bias_ftps.Clear();
            est_wind_dir_deg.Clear();

            est_wind_speed_kts.Clear();
            density_alt_ft.Clear();
            tag_age_body_ms.Clear();
            tag_pos_B_T_B_m1.Clear();
            tag_pos_B_T_B_m2.Clear();
            tag_pos_B_T_B_m3.Clear();
            tag_age_ned_ms.Clear();
            tag_pos_B_T_NED_m1.Clear();
            tag_pos_B_T_NED_m2.Clear();
            tag_pos_B_T_NED_m3.Clear();

            tag_vel_T_B_NED_m_s1.Clear();
            tag_vel_T_B_NED_m_s2.Clear();
            tag_vel_T_B_NED_m_s3.Clear();
            accum_body_upd.Clear();
            accum_ned_upd.Clear();
            tracking.Clear();
            pos_B_T_NED_ft1.Clear();
            pos_B_T_NED_ft2.Clear();
            pos_B_T_NED_ft3.Clear();
            vel_T_B_NED_ftps1.Clear();

            vel_T_B_NED_ftps2.Clear();
            vel_T_B_NED_ftps3.Clear();
            est_pos_B_T_NED_ft1.Clear();
            est_pos_B_T_NED_ft2.Clear();
            est_pos_B_T_NED_ft3.Clear();
            est_vel_T_B_NED_ftps1.Clear();
            est_vel_T_B_NED_ftps2.Clear();
            est_vel_T_B_NED_ftps3.Clear();
            altitude_low.Clear();
            V_min_kts.Clear();

            est_mass_lbm.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int byteVal = 1;


            linux_time_s.Add(BitConverter.ToDouble(input, start));
            est_V_wind_N_ftps.Add(BitConverter.ToSingle(input, start += doubleVal));
            est_V_wind_E_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            P11_V_wind_N.Add(BitConverter.ToSingle(input, start += floatVal));
            P22_V_wind_E.Add(BitConverter.ToSingle(input, start += floatVal));
            P33_IAS_bias.Add(BitConverter.ToSingle(input, start += floatVal));
            TAS_meas_residual_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            rho_kg_m3.Add(BitConverter.ToSingle(input, start += floatVal));
            est_IAS_bias_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            est_wind_dir_deg.Add(BitConverter.ToSingle(input, start += floatVal));

            est_wind_speed_kts.Add(BitConverter.ToSingle(input, start += floatVal));
            density_alt_ft.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_age_body_ms.Add(BitConverter.ToUInt32(input, start += floatVal));
            tag_pos_B_T_B_m1.Add(BitConverter.ToSingle(input, start += intVal));
            tag_pos_B_T_B_m2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_pos_B_T_B_m3.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_age_ned_ms.Add(BitConverter.ToUInt32(input, start += floatVal));
            tag_pos_B_T_NED_m1.Add(BitConverter.ToSingle(input, start += intVal));
            tag_pos_B_T_NED_m2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_pos_B_T_NED_m3.Add(BitConverter.ToSingle(input, start += floatVal));

            tag_vel_T_B_NED_m_s1.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_vel_T_B_NED_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_vel_T_B_NED_m_s3.Add(BitConverter.ToSingle(input, start += floatVal));
            accum_body_upd.Add(BitConverter.ToInt32(input, start += floatVal));
            accum_ned_upd.Add(BitConverter.ToInt32(input, start += intVal));
            tracking.Add((sbyte)BitConverter.ToChar(input, start += intVal));
            pos_B_T_NED_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            pos_B_T_NED_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_B_T_NED_ft3.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_T_B_NED_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));

            vel_T_B_NED_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_T_B_NED_ftps3.Add(BitConverter.ToDouble(input, start += doubleVal));
            est_pos_B_T_NED_ft1.Add(BitConverter.ToSingle(input, start += doubleVal));
            est_pos_B_T_NED_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_pos_B_T_NED_ft3.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps3.Add(BitConverter.ToSingle(input, start += floatVal));
            altitude_low.Add((sbyte)BitConverter.ToChar(input, start += floatVal));
            V_min_kts.Add(BitConverter.ToSingle(input, start += byteVal));

            est_mass_lbm.Add(BitConverter.ToSingle(input, start += floatVal));
            //Debug.WriteLine($"should be {input.Length} but is {start}");

        }

        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            est_V_wind_N_ftps.Add(Reader.ReadFloat(stream));
            est_V_wind_E_ftps.Add(Reader.ReadFloat(stream));
            P11_V_wind_N.Add(Reader.ReadFloat(stream));
            P22_V_wind_E.Add(Reader.ReadFloat(stream));
            P33_IAS_bias.Add(Reader.ReadFloat(stream));
            TAS_meas_residual_ftps.Add(Reader.ReadFloat(stream));
            rho_kg_m3.Add(Reader.ReadFloat(stream));
            est_IAS_bias_ftps.Add(Reader.ReadFloat(stream));
            est_wind_dir_deg.Add(Reader.ReadFloat(stream));

            est_wind_speed_kts.Add(Reader.ReadFloat(stream));
            density_alt_ft.Add(Reader.ReadFloat(stream));
            tag_age_body_ms.Add(Reader.ReadUInt(stream));
            tag_pos_B_T_B_m1.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_B_m2.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_B_m3.Add(Reader.ReadFloat(stream));
            tag_age_ned_ms.Add(Reader.ReadUInt(stream));
            tag_pos_B_T_NED_m1.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_NED_m2.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_NED_m3.Add(Reader.ReadFloat(stream));

            tag_vel_T_B_NED_m_s1.Add(Reader.ReadFloat(stream));
            tag_vel_T_B_NED_m_s2.Add(Reader.ReadFloat(stream));
            tag_vel_T_B_NED_m_s3.Add(Reader.ReadFloat(stream));
            accum_body_upd.Add(Reader.ReadInt(stream));
            accum_ned_upd.Add(Reader.ReadInt(stream));
            tracking.Add(Reader.ReadSByte(stream));
            pos_B_T_NED_ft1.Add(Reader.ReadDouble(stream));
            pos_B_T_NED_ft2.Add(Reader.ReadDouble(stream));
            pos_B_T_NED_ft3.Add(Reader.ReadDouble(stream));
            vel_T_B_NED_ftps1.Add(Reader.ReadDouble(stream));

            vel_T_B_NED_ftps2.Add(Reader.ReadDouble(stream));
            vel_T_B_NED_ftps3.Add(Reader.ReadDouble(stream));
            est_pos_B_T_NED_ft1.Add(Reader.ReadFloat(stream));
            est_pos_B_T_NED_ft2.Add(Reader.ReadFloat(stream));
            est_pos_B_T_NED_ft3.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps1.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps2.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps3.Add(Reader.ReadFloat(stream));
            altitude_low.Add(Reader.ReadSByte(stream));
            V_min_kts.Add(Reader.ReadFloat(stream));

            est_mass_lbm.Add(Reader.ReadFloat(stream));
        }

    }

    public class GCSData
    {

        public int totalLength = 118;
        public int dataLength = 43;

        public List<double> linux_time_s { get; set; }
        public List<sbyte> shield_command { get; set; }
        public List<float> c2_ack_ratio { get; set; }
        public List<byte> mvg_base_HDOP { get; set; }
        public List<double> est_pos_mvb_NE_ft1 { get; set; }
        public List<double> est_pos_mvb_NE_ft2 { get; set; }
        public List<double> delta_X_ft { get; set; }
        public List<double> delta_Y_ft { get; set; }
        public List<int> accum_mvb_upd { get; set; }
        public List<double> est_vel_mvb_NE_ftps1 { get; set; }

        public List<double> est_vel_mvb_NE_ftps2 { get; set; }
        public List<sbyte> mvb_en { get; set; }
        public List<float> meas_pos_mvb_NE_ft1 { get; set; }
        public List<float> meas_pos_mvb_NE_ft2 { get; set; }
        public List<float> meas_vel_mvb_NE_mps1 { get; set; }
        public List<float> meas_vel_mvb_NE_mps2 { get; set; }
        public List<byte> fp_upload_cnt { get; set; }
        public List<short> fltplan_num_wpt1 { get; set; }
        public List<short> fltplan_num_wpt2 { get; set; }
        public List<short> fltplan_num_wpt3 { get; set; }

        public List<short> fltplan_num_wpt4 { get; set; }
        public List<short> fltplan_num_wpt5 { get; set; }
        public List<short> fltplan_num_wpt6 { get; set; }
        public List<short> fltplan_num_wpt7 { get; set; }
        public List<sbyte> current_fp_changed { get; set; }
        public List<short> next_waypoint { get; set; }
        public List<sbyte> next_wpt_updated { get; set; }
        public List<byte> packet20_freq_hz { get; set; }
        public List<byte> xbus_freq_hz { get; set; }
        public List<sbyte> lost_comm_fault_HV { get; set; }

        public List<sbyte> lost_comm_fault_HS { get; set; }
        public List<sbyte> stick_input_throttle_stick_lsb { get; set; }
        public List<sbyte> stick_input_aileron_stick_lsb { get; set; }
        public List<sbyte> stick_input_elevator_stick_lsb { get; set; }
        public List<sbyte> stick_input_rudder_stick_lsb { get; set; }
        public List<sbyte> stick_input_aux_A_stick_lsb { get; set; }
        public List<sbyte> stick_input_aux_B_stick_lsb { get; set; }
        public List<sbyte> stick_input_engine_disable { get; set; }
        public List<sbyte> stick_input_control_mode { get; set; }
        public List<sbyte> control_mode_rcvd { get; set; }

        public List<sbyte> auto_manvr_reset { get; set; }
        public List<sbyte> loss_of_xbus_fault { get; set; }
        public List<sbyte> constant_bank_en { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public GCSData()
        {
            linux_time_s = new List<double>();
            shield_command = new List<sbyte>();
            c2_ack_ratio = new List<float>();
            mvg_base_HDOP = new List<byte>();
            est_pos_mvb_NE_ft1 = new List<double>();
            est_pos_mvb_NE_ft2 = new List<double>();
            delta_X_ft = new List<double>();
            delta_Y_ft = new List<double>();
            accum_mvb_upd = new List<int>();
            est_vel_mvb_NE_ftps1 = new List<double>();

            est_vel_mvb_NE_ftps2 = new List<double>();
            mvb_en = new List<sbyte>();
            meas_pos_mvb_NE_ft1 = new List<float>();
            meas_pos_mvb_NE_ft2 = new List<float>();
            meas_vel_mvb_NE_mps1 = new List<float>();
            meas_vel_mvb_NE_mps2 = new List<float>();
            fp_upload_cnt = new List<byte>();
            fltplan_num_wpt1 = new List<short>();
            fltplan_num_wpt2 = new List<short>();
            fltplan_num_wpt3 = new List<short>();

            fltplan_num_wpt4 = new List<short>();
            fltplan_num_wpt5 = new List<short>();
            fltplan_num_wpt6 = new List<short>();
            fltplan_num_wpt7 = new List<short>();
            current_fp_changed = new List<sbyte>();
            next_waypoint = new List<short>();
            next_wpt_updated = new List<sbyte>();
            packet20_freq_hz = new List<byte>();
            xbus_freq_hz = new List<byte>();
            lost_comm_fault_HV = new List<sbyte>();

            lost_comm_fault_HS = new List<sbyte>();
            stick_input_throttle_stick_lsb = new List<sbyte>();
            stick_input_aileron_stick_lsb = new List<sbyte>();
            stick_input_elevator_stick_lsb = new List<sbyte>();
            stick_input_rudder_stick_lsb = new List<sbyte>();
            stick_input_aux_A_stick_lsb = new List<sbyte>();
            stick_input_aux_B_stick_lsb = new List<sbyte>();
            stick_input_engine_disable = new List<sbyte>();
            stick_input_control_mode = new List<sbyte>();
            control_mode_rcvd = new List<sbyte>();

            auto_manvr_reset = new List<sbyte>();
            loss_of_xbus_fault = new List<sbyte>();
            constant_bank_en = new List<sbyte>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            shield_command.Clear();
            c2_ack_ratio.Clear();
            mvg_base_HDOP.Clear();
            est_pos_mvb_NE_ft1.Clear();
            est_pos_mvb_NE_ft2.Clear();
            delta_X_ft.Clear();
            delta_Y_ft.Clear();
            accum_mvb_upd.Clear();
            est_vel_mvb_NE_ftps1.Clear();

            est_vel_mvb_NE_ftps2.Clear();
            mvb_en.Clear();
            meas_pos_mvb_NE_ft1.Clear();
            meas_pos_mvb_NE_ft2.Clear();
            meas_vel_mvb_NE_mps1.Clear();
            meas_vel_mvb_NE_mps2.Clear();
            fp_upload_cnt.Clear();
            fltplan_num_wpt1.Clear();
            fltplan_num_wpt2.Clear();
            fltplan_num_wpt3.Clear();

            fltplan_num_wpt4.Clear();
            fltplan_num_wpt5.Clear();
            fltplan_num_wpt6.Clear();
            fltplan_num_wpt7.Clear();
            current_fp_changed.Clear();
            next_waypoint.Clear();
            next_wpt_updated.Clear();
            packet20_freq_hz.Clear();
            xbus_freq_hz.Clear();
            lost_comm_fault_HV.Clear();

            lost_comm_fault_HS.Clear();
            stick_input_throttle_stick_lsb.Clear();
            stick_input_aileron_stick_lsb.Clear();
            stick_input_elevator_stick_lsb.Clear();
            stick_input_rudder_stick_lsb.Clear();
            stick_input_aux_A_stick_lsb.Clear();
            stick_input_aux_B_stick_lsb.Clear();
            stick_input_engine_disable.Clear();
            stick_input_control_mode.Clear();
            control_mode_rcvd.Clear();

            auto_manvr_reset.Clear();
            loss_of_xbus_fault.Clear();
            constant_bank_en.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int shortVal = 2;
            int byteVal = 1;

            linux_time_s.Add(BitConverter.ToDouble(input, start));
            shield_command.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            c2_ack_ratio.Add(BitConverter.ToSingle(input, start += byteVal));
            mvg_base_HDOP.Add((byte)BitConverter.ToChar(input, start += floatVal));
            est_pos_mvb_NE_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            est_pos_mvb_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            delta_X_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            delta_Y_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            accum_mvb_upd.Add(BitConverter.ToInt32(input, start += doubleVal));
            est_vel_mvb_NE_ftps1.Add(BitConverter.ToDouble(input, start += intVal));

            est_vel_mvb_NE_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            mvb_en.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            meas_pos_mvb_NE_ft1.Add(BitConverter.ToSingle(input, start += byteVal));
            meas_pos_mvb_NE_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_vel_mvb_NE_mps1.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_vel_mvb_NE_mps2.Add(BitConverter.ToSingle(input, start += floatVal));
            fp_upload_cnt.Add((byte)BitConverter.ToChar(input, start += floatVal));
            fltplan_num_wpt1.Add(BitConverter.ToInt16(input, start += byteVal));
            fltplan_num_wpt2.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt3.Add(BitConverter.ToInt16(input, start += shortVal));

            fltplan_num_wpt4.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt5.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt6.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt7.Add(BitConverter.ToInt16(input, start += shortVal));
            current_fp_changed.Add((sbyte)BitConverter.ToChar(input, start += shortVal));
            next_waypoint.Add(BitConverter.ToInt16(input, start += byteVal));
            next_wpt_updated.Add((sbyte)BitConverter.ToChar(input, start += shortVal));
            packet20_freq_hz.Add((byte)BitConverter.ToChar(input, start += byteVal));
            xbus_freq_hz.Add((byte)BitConverter.ToChar(input, start += byteVal));
            lost_comm_fault_HV.Add((sbyte)BitConverter.ToChar(input, start += byteVal));

            lost_comm_fault_HS.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_throttle_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aileron_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_elevator_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_rudder_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aux_A_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aux_B_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_engine_disable.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_control_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            control_mode_rcvd.Add((sbyte)BitConverter.ToChar(input, start += byteVal));

            auto_manvr_reset.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            loss_of_xbus_fault.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            constant_bank_en.Add((sbyte)BitConverter.ToChar(input, start));
            //Debug.WriteLine($"should be {input.Length} but is {start}");
        }


        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            shield_command.Add(Reader.ReadSByte(stream));
            c2_ack_ratio.Add(Reader.ReadFloat(stream));
            mvg_base_HDOP.Add(Reader.ReadByte(stream));
            est_pos_mvb_NE_ft1.Add(Reader.ReadDouble(stream));
            est_pos_mvb_NE_ft2.Add(Reader.ReadDouble(stream));
            delta_X_ft.Add(Reader.ReadDouble(stream));
            delta_Y_ft.Add(Reader.ReadDouble(stream));
            accum_mvb_upd.Add(Reader.ReadInt(stream));
            est_vel_mvb_NE_ftps1.Add(Reader.ReadDouble(stream));

            est_vel_mvb_NE_ftps2.Add(Reader.ReadDouble(stream));
            mvb_en.Add(Reader.ReadSByte(stream));
            meas_pos_mvb_NE_ft1.Add(Reader.ReadFloat(stream));
            meas_pos_mvb_NE_ft2.Add(Reader.ReadFloat(stream));
            meas_vel_mvb_NE_mps1.Add(Reader.ReadFloat(stream));
            meas_vel_mvb_NE_mps2.Add(Reader.ReadFloat(stream));
            fp_upload_cnt.Add(Reader.ReadByte(stream));
            fltplan_num_wpt1.Add(Reader.ReadShort(stream));
            fltplan_num_wpt2.Add(Reader.ReadShort(stream));
            fltplan_num_wpt3.Add(Reader.ReadShort(stream));

            fltplan_num_wpt4.Add(Reader.ReadShort(stream));
            fltplan_num_wpt5.Add(Reader.ReadShort(stream));
            fltplan_num_wpt6.Add(Reader.ReadShort(stream));
            fltplan_num_wpt7.Add(Reader.ReadShort(stream));
            current_fp_changed.Add(Reader.ReadSByte(stream));
            next_waypoint.Add(Reader.ReadShort(stream));
            next_wpt_updated.Add(Reader.ReadSByte(stream));
            packet20_freq_hz.Add(Reader.ReadByte(stream));
            xbus_freq_hz.Add(Reader.ReadByte(stream));
            lost_comm_fault_HV.Add(Reader.ReadSByte(stream));

            lost_comm_fault_HS.Add(Reader.ReadSByte(stream));
            stick_input_throttle_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aileron_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_elevator_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_rudder_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aux_A_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aux_B_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_engine_disable.Add(Reader.ReadSByte(stream));
            stick_input_control_mode.Add(Reader.ReadSByte(stream));
            control_mode_rcvd.Add(Reader.ReadSByte(stream));

            auto_manvr_reset.Add(Reader.ReadSByte(stream));
            loss_of_xbus_fault.Add(Reader.ReadSByte(stream));
            constant_bank_en.Add(Reader.ReadSByte(stream));
        }

    }
    public class GuidanceData
    {


        public int totalLength = 142;
        public int dataLength = 25;

        public List<double> linux_time_s { get; set; }
        public List<double> alt_rate_cmd_hs_ftps { get; set; }
        public List<double> phi_cmd_hs_deg { get; set; }
        public List<double> speed_cmd_hs_knots { get; set; }
        public List<double> alt_cmd_hs_ft { get; set; }
        public List<double> cog_cmd_hs_deg { get; set; }
        public List<sbyte> increment_wpt { get; set; }
        public List<double> psi_cmd_deg { get; set; }
        public List<double> urate_ft_s { get; set; }
        public List<double> pos_cmd_NE_ft1 { get; set; }

        public List<double> pos_cmd_NE_ft2 { get; set; }
        public List<double> alt_cmd_ft { get; set; }
        public List<double> hv_wpt_err_ft { get; set; }
        public List<double> vel_cmd_NED_ftps1 { get; set; }
        public List<double> vel_cmd_NED_ftps2 { get; set; }
        public List<double> vel_cmd_NED_ftps3 { get; set; }
        public List<float> cl_hv_deg { get; set; }
        public List<byte> stick_mode { get; set; }
        public List<sbyte> flip_state { get; set; }
        public List<float> wind_est_NE1 { get; set; }

        public List<float> wind_est_NE2 { get; set; }
        public List<float> hdg_err { get; set; }
        public List<byte> mvb_land_state { get; set; }
        public List<sbyte> hover_done { get; set; }
        public List<sbyte> launch_done { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public GuidanceData()
        {
            linux_time_s = new List<double>();
            alt_rate_cmd_hs_ftps = new List<double>();
            phi_cmd_hs_deg = new List<double>();
            speed_cmd_hs_knots = new List<double>();
            alt_cmd_hs_ft = new List<double>();
            cog_cmd_hs_deg = new List<double>();
            increment_wpt = new List<sbyte>();
            psi_cmd_deg = new List<double>();
            urate_ft_s = new List<double>();
            pos_cmd_NE_ft1 = new List<double>();

            pos_cmd_NE_ft2 = new List<double>();
            alt_cmd_ft = new List<double>();
            hv_wpt_err_ft = new List<double>();
            vel_cmd_NED_ftps1 = new List<double>();
            vel_cmd_NED_ftps2 = new List<double>();
            vel_cmd_NED_ftps3 = new List<double>();
            cl_hv_deg = new List<float>();
            stick_mode = new List<byte>();
            flip_state = new List<sbyte>();
            wind_est_NE1 = new List<float>();

            wind_est_NE2 = new List<float>();
            hdg_err = new List<float>();
            mvb_land_state = new List<byte>();
            hover_done = new List<sbyte>();
            launch_done = new List<sbyte>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            alt_rate_cmd_hs_ftps.Clear();
            phi_cmd_hs_deg.Clear();
            speed_cmd_hs_knots.Clear();
            alt_cmd_hs_ft.Clear();
            cog_cmd_hs_deg.Clear();
            increment_wpt.Clear();
            psi_cmd_deg.Clear();
            urate_ft_s.Clear();
            pos_cmd_NE_ft1.Clear();

            pos_cmd_NE_ft2.Clear();
            alt_cmd_ft.Clear();
            hv_wpt_err_ft.Clear();
            vel_cmd_NED_ftps1.Clear();
            vel_cmd_NED_ftps2.Clear();
            vel_cmd_NED_ftps3.Clear();
            cl_hv_deg.Clear();
            stick_mode.Clear();
            flip_state.Clear();
            wind_est_NE1.Clear();

            wind_est_NE2.Clear();
            hdg_err.Clear();
            mvb_land_state.Clear();
            hover_done.Clear();
            launch_done.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int byteVal = 1;

            linux_time_s.Add(BitConverter.ToDouble(input, start));
            alt_rate_cmd_hs_ftps.Add(BitConverter.ToDouble(input, start += doubleVal));
            phi_cmd_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            speed_cmd_hs_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_cmd_hs_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            cog_cmd_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            increment_wpt.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            psi_cmd_deg.Add(BitConverter.ToDouble(input, start += byteVal));
            urate_ft_s.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_cmd_NE_ft1.Add(BitConverter.ToDouble(input, start += doubleVal));

            pos_cmd_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_cmd_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_wpt_err_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps3.Add(BitConverter.ToDouble(input, start += doubleVal));
            cl_hv_deg.Add(BitConverter.ToSingle(input, start += doubleVal));
            stick_mode.Add((byte)BitConverter.ToChar(input, start += floatVal));
            flip_state.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            wind_est_NE1.Add(BitConverter.ToSingle(input, start += byteVal));

            wind_est_NE2.Add(BitConverter.ToSingle(input, start += floatVal));
            hdg_err.Add(BitConverter.ToSingle(input, start += floatVal));
            mvb_land_state.Add((byte)BitConverter.ToChar(input, start += floatVal));
            hover_done.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            launch_done.Add((sbyte)BitConverter.ToChar(input, start));
        }


        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            alt_rate_cmd_hs_ftps.Add(Reader.ReadDouble(stream));
            phi_cmd_hs_deg.Add(Reader.ReadDouble(stream));
            speed_cmd_hs_knots.Add(Reader.ReadDouble(stream));
            alt_cmd_hs_ft.Add(Reader.ReadDouble(stream));
            cog_cmd_hs_deg.Add(Reader.ReadDouble(stream));
            increment_wpt.Add(Reader.ReadSByte(stream));
            psi_cmd_deg.Add(Reader.ReadDouble(stream));
            urate_ft_s.Add(Reader.ReadDouble(stream));
            pos_cmd_NE_ft1.Add(Reader.ReadDouble(stream));

            pos_cmd_NE_ft2.Add(Reader.ReadDouble(stream));
            alt_cmd_ft.Add(Reader.ReadDouble(stream));
            hv_wpt_err_ft.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps1.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps2.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps3.Add(Reader.ReadDouble(stream));
            cl_hv_deg.Add(Reader.ReadFloat(stream));
            stick_mode.Add(Reader.ReadByte(stream));
            flip_state.Add(Reader.ReadSByte(stream));
            wind_est_NE1.Add(Reader.ReadFloat(stream));

            wind_est_NE2.Add(Reader.ReadFloat(stream));
            hdg_err.Add(Reader.ReadFloat(stream));
            mvb_land_state.Add(Reader.ReadByte(stream));
            hover_done.Add(Reader.ReadSByte(stream));
            launch_done.Add(Reader.ReadSByte(stream));
        }
    }

    public class ModeControlData
    {
        public int totalLength = 18;
        public int dataLength = 11;

        public List<double> linux_time_s { get; set; }
        public List<byte> flight_state_tlm { get; set; }
        public List<sbyte> control_mode { get; set; }
        public List<sbyte> start_waypoint_mode { get; set; }
        public List<short> next_wpt { get; set; }
        public List<byte> flightplan { get; set; }
        public List<sbyte> wpt_changed { get; set; }
        public List<sbyte> prevent_increment { get; set; }
        public List<sbyte> auto_landing { get; set; }
        public List<sbyte> rpm_low { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public ModeControlData()
        {
            linux_time_s = new List<double>();
            flight_state_tlm = new List<byte>();
            control_mode = new List<sbyte>();
            start_waypoint_mode = new List<sbyte>();
            next_wpt = new List<short>();
            flightplan = new List<byte>();
            wpt_changed = new List<sbyte>();
            prevent_increment = new List<sbyte>();
            auto_landing = new List<sbyte>();
            rpm_low = new List<sbyte>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            flight_state_tlm.Clear();
            control_mode.Clear();
            start_waypoint_mode.Clear();
            next_wpt.Clear();
            flightplan.Clear();
            wpt_changed.Clear();
            prevent_increment.Clear();
            auto_landing.Clear();
            rpm_low.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int shortVal = 2;
            int byteVal = 1;
            //Debug.WriteLine(input.Length);
            linux_time_s.Add(BitConverter.ToDouble(input, start));
            flight_state_tlm.Add((byte)BitConverter.ToChar(input, start += doubleVal));
            control_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            start_waypoint_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            next_wpt.Add(BitConverter.ToInt16(input, start += byteVal));
            flightplan.Add((byte)BitConverter.ToChar(input, start += shortVal));
            wpt_changed.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            prevent_increment.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            auto_landing.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            rpm_low.Add((sbyte)BitConverter.ToChar(input, start));

        }

        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            flight_state_tlm.Add(Reader.ReadByte(stream));
            control_mode.Add(Reader.ReadSByte(stream));
            start_waypoint_mode.Add(Reader.ReadSByte(stream));
            next_wpt.Add(Reader.ReadShort(stream));
            flightplan.Add(Reader.ReadByte(stream));
            wpt_changed.Add(Reader.ReadSByte(stream));
            prevent_increment.Add(Reader.ReadSByte(stream));
            auto_landing.Add(Reader.ReadSByte(stream));
            rpm_low.Add(Reader.ReadSByte(stream));
        }
    }

    public class SensorProcData
    {



        public int totalLength = 650;
        public int dataLength = 131;

        public List<double> meas_w_B_ECI_B_deg_s1 { get; set; }
        public List<double> meas_w_B_ECI_B_deg_s2 { get; set; }
        public List<double> meas_w_B_ECI_B_deg_s3 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s1 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s2 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s3 { get; set; }
        public List<double> q_B_NED1 { get; set; }
        public List<double> q_B_NED2 { get; set; }
        public List<double> q_B_NED3 { get; set; }
        public List<double> q_B_NED4 { get; set; }

        public List<double> Euler321_B_NED_hs_deg_psi_hs_deg { get; set; }
        public List<double> Euler321_B_NED_hs_deg_theta_hs_deg { get; set; }
        public List<double> Euler321_B_NED_hs_deg_phi_hs_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_psi_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_phi_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_theta_deg { get; set; }
        public List<double> filt_airspeed_knots { get; set; }
        public List<bool> airspeed_invalid { get; set; }
        public List<double> altimeter_ft { get; set; }
        public List<double> est_alt_ft { get; set; }

        public List<double> est_hdot_ft_s { get; set; }
        public List<float> AGL_bias_ft { get; set; }
        public List<double> time_unix_sec { get; set; }
        public List<float> H_MSL_ft { get; set; }
        public List<float> QNH_Pa { get; set; }
        public List<double> Vel_B_ECF_NED_knots1 { get; set; }
        public List<double> Vel_B_ECF_NED_knots2 { get; set; }
        public List<double> Vel_B_ECF_NED_knots3 { get; set; }
        public List<double> sog_knots { get; set; }
        public List<double> cog_deg { get; set; }

        public List<double> rc_input_rc_aileron { get; set; }
        public List<double> rc_input_rc_elevator { get; set; }
        public List<double> rc_input_rc_flap { get; set; }
        public List<double> rc_input_rc_gear { get; set; }
        public List<double> rc_input_rc_rudder { get; set; }
        public List<double> rc_input_rc_throttle { get; set; }
        public List<short> rc_input_rc_man_bckup_lsb { get; set; }
        public List<bool> rc_control_en { get; set; }
        public List<double> pos_NE_ft1 { get; set; }
        public List<double> pos_NE_ft2 { get; set; }

        public List<double> alt_INS_ft { get; set; }
        public List<double> meas_lat_rad { get; set; }
        public List<double> meas_lng_rad { get; set; }
        public List<bool> gps_ready { get; set; }
        public List<double> init_lat_lon_rad1 { get; set; }
        public List<double> init_lat_lon_rad2 { get; set; }
        public List<bool> pos_initialized { get; set; }
        public List<float> Accel_NED_ft_s_s1 { get; set; }
        public List<float> Accel_NED_ft_s_s2 { get; set; }
        public List<float> Accel_NED_ft_s_s3 { get; set; }

        public List<float> est_pos_NE_ft1 { get; set; }
        public List<float> est_pos_NE_ft2 { get; set; }
        public List<float> est_vel_NE_kts1 { get; set; }
        public List<float> est_vel_NE_kts2 { get; set; }
        public List<double> linux_time_s { get; set; }
        public List<bool> INS_solution_valid { get; set; }
        public List<double> est_alt_bias_ft { get; set; }
        public List<double> est_accel_bias_ft_s2 { get; set; }
        public List<double> vectornav_tlm_rawPositionLat_deg { get; set; }
        public List<double> vectornav_tlm_rawPositionLon_deg { get; set; }

        public List<double> vectornav_tlm_rawPositionAlt_m { get; set; }
        public List<float> vectornav_tlm_rawVel_N_m_s { get; set; }
        public List<float> vectornav_tlm_rawVel_E_m_s { get; set; }
        public List<float> vectornav_tlm_rawVel_D_m_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_Bx_rad_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_By_rad_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_Bz_rad_s { get; set; }
        public List<float> vectornav_tlm_accel_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_accel_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_accel_Bz_m_s2 { get; set; }

        public List<float> vectornav_tlm_uncmpAccel_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_uncmpAccel_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_uncmpAccel_Bz_m_s2 { get; set; }
        public List<float> vectornav_tlm_mag_Bx_G { get; set; }
        public List<float> vectornav_tlm_mag_By_G { get; set; }
        public List<float> vectornav_tlm_mag_Bz_G { get; set; }
        public List<ushort> vectornav_tlm_sensSat { get; set; }
        public List<float> vectornav_tlm_yawU_deg { get; set; }
        public List<float> vectornav_tlm_pitchU_deg { get; set; }
        public List<float> vectornav_tlm_rollU_deg { get; set; }

        public List<float> vectornav_tlm_posU_m { get; set; }
        public List<float> vectornav_tlm_velU_m_s { get; set; }
        public List<float> vectornav_tlm_GPS1GDOP { get; set; }
        public List<float> vectornav_tlm_GPS1TDOP { get; set; }
        public List<float> vectornav_tlm_GPS2GDOP { get; set; }
        public List<byte> vectornav_tlm_numGPS1Sats { get; set; }
        public List<byte> vectornav_tlm_numGPS2Sats { get; set; }
        public List<byte> vectornav_tlm_GPS1Fix { get; set; }
        public List<byte> vectornav_tlm_GPS2Fix { get; set; }
        public List<ushort> vectornav_tlm_INSStatus { get; set; }

        public List<ushort> vectornav_tlm_AHRSStatus { get; set; }
        public List<float> vectornav_tlm_temp_C { get; set; }
        public List<float> vectornav_tlm_press_kPa { get; set; }
        public List<float> vectornav_tlm_Yaw_deg { get; set; }
        public List<float> vectornav_tlm_Pitch_deg { get; set; }
        public List<float> vectornav_tlm_Roll_deg { get; set; }
        public List<float> vectornav_tlm_linAcc_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_linAcc_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_linAcc_Bz_m_s2 { get; set; }
        public List<float> prop_rpm { get; set; }

        public List<float> battery_V { get; set; }
        public List<float> fuel_percent { get; set; }
        public List<float> meas_AGL_m { get; set; }
        public List<int> slow_status_pkt_errors { get; set; }
        public List<int> slow_status_pkt_warnings { get; set; }
        public List<float> slow_status_pkt_battery_V { get; set; }
        public List<float> slow_status_pkt_fuel_percent { get; set; }
        public List<byte> slow_status_pkt_flagsA { get; set; }
        public List<byte> slow_status_pkt_power_status { get; set; }
        public List<byte> slow_status_pkt_flagsB { get; set; }

        public List<float> slow_status_pkt_battery_A { get; set; }
        public List<float> slow_status_pkt_generator_V { get; set; }
        public List<float> slow_status_pkt_generator_A { get; set; }
        public List<float> env_pkt_nose_airtemp_C { get; set; }
        public List<float> env_pkt_fuel_flow_lb_p_h { get; set; }
        public List<float> env_pkt_est_fuel_wght_lb { get; set; }
        public List<float> env_pkt_eng_cyl1_temp_C { get; set; }
        public List<float> env_pkt_eng_cyl2_temp_C { get; set; }
        public List<ushort> env_pkt_valid { get; set; }
        public List<float> pss8_pkt_impact_press_Pa { get; set; }

        public List<float> pss8_pkt_static_press_Pa { get; set; }
        public List<float> pss8_pkt_cal_airspeed_m_s { get; set; }
        public List<float> pss8_pkt_true_airspeed_m_s { get; set; }
        public List<float> pss8_pkt_press_altitude_m { get; set; }
        public List<ushort> pss8_pkt_quality { get; set; }
        public List<ushort> pss8_pkt_flags { get; set; }
        public List<float> pss8_pkt_static_temp_C { get; set; }
        public List<float> pss8_pkt_total_temp_C { get; set; }
        public List<ushort> pss8_pkt_slow_quality { get; set; }
        public List<ushort> pss8_pkt_slow_flags { get; set; }

        public List<uint> running_time_msec { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public SensorProcData()
        {
            meas_w_B_ECI_B_deg_s1 = new List<double>();
            meas_w_B_ECI_B_deg_s2 = new List<double>();
            meas_w_B_ECI_B_deg_s3 = new List<double>();
            filt_w_B_ECI_B_deg_s1 = new List<double>();
            filt_w_B_ECI_B_deg_s2 = new List<double>();
            filt_w_B_ECI_B_deg_s3 = new List<double>();
            q_B_NED1 = new List<double>();
            q_B_NED2 = new List<double>();
            q_B_NED3 = new List<double>();
            q_B_NED4 = new List<double>();

            Euler321_B_NED_hs_deg_psi_hs_deg = new List<double>();
            Euler321_B_NED_hs_deg_theta_hs_deg = new List<double>();
            Euler321_B_NED_hs_deg_phi_hs_deg = new List<double>();
            Euler312_B_NED_hv_deg_psi_deg = new List<double>();
            Euler312_B_NED_hv_deg_phi_deg = new List<double>();
            Euler312_B_NED_hv_deg_theta_deg = new List<double>();
            filt_airspeed_knots = new List<double>();
            airspeed_invalid = new List<bool>();
            altimeter_ft = new List<double>();
            est_alt_ft = new List<double>();

            est_hdot_ft_s = new List<double>();
            AGL_bias_ft = new List<float>();
            time_unix_sec = new List<double>();
            H_MSL_ft = new List<float>();
            QNH_Pa = new List<float>();
            Vel_B_ECF_NED_knots1 = new List<double>();
            Vel_B_ECF_NED_knots2 = new List<double>();
            Vel_B_ECF_NED_knots3 = new List<double>();
            sog_knots = new List<double>();
            cog_deg = new List<double>();

            rc_input_rc_aileron = new List<double>();
            rc_input_rc_elevator = new List<double>();
            rc_input_rc_flap = new List<double>();
            rc_input_rc_gear = new List<double>();
            rc_input_rc_rudder = new List<double>();
            rc_input_rc_throttle = new List<double>();
            rc_input_rc_man_bckup_lsb = new List<short>();
            rc_control_en = new List<bool>();
            pos_NE_ft1 = new List<double>();
            pos_NE_ft2 = new List<double>();

            alt_INS_ft = new List<double>();
            meas_lat_rad = new List<double>();
            meas_lng_rad = new List<double>();
            gps_ready = new List<bool>();
            init_lat_lon_rad1 = new List<double>();
            init_lat_lon_rad2 = new List<double>();
            pos_initialized = new List<bool>();
            Accel_NED_ft_s_s1 = new List<float>();
            Accel_NED_ft_s_s2 = new List<float>();
            Accel_NED_ft_s_s3 = new List<float>();

            est_pos_NE_ft1 = new List<float>();
            est_pos_NE_ft2 = new List<float>();
            est_vel_NE_kts1 = new List<float>();
            est_vel_NE_kts2 = new List<float>();
            linux_time_s = new List<double>();
            INS_solution_valid = new List<bool>();
            est_alt_bias_ft = new List<double>();
            est_accel_bias_ft_s2 = new List<double>();
            vectornav_tlm_rawPositionLat_deg = new List<double>();
            vectornav_tlm_rawPositionLon_deg = new List<double>();

            vectornav_tlm_rawPositionAlt_m = new List<double>();
            vectornav_tlm_rawVel_N_m_s = new List<float>();
            vectornav_tlm_rawVel_E_m_s = new List<float>();
            vectornav_tlm_rawVel_D_m_s = new List<float>();
            vectornav_tlm_uncmpAngRate_Bx_rad_s = new List<float>();
            vectornav_tlm_uncmpAngRate_By_rad_s = new List<float>();
            vectornav_tlm_uncmpAngRate_Bz_rad_s = new List<float>();
            vectornav_tlm_accel_Bx_m_s2 = new List<float>();
            vectornav_tlm_accel_By_m_s2 = new List<float>();
            vectornav_tlm_accel_Bz_m_s2 = new List<float>();

            vectornav_tlm_uncmpAccel_Bx_m_s2 = new List<float>();
            vectornav_tlm_uncmpAccel_By_m_s2 = new List<float>();
            vectornav_tlm_uncmpAccel_Bz_m_s2 = new List<float>();
            vectornav_tlm_mag_Bx_G = new List<float>();
            vectornav_tlm_mag_By_G = new List<float>();
            vectornav_tlm_mag_Bz_G = new List<float>();
            vectornav_tlm_sensSat = new List<ushort>();
            vectornav_tlm_yawU_deg = new List<float>();
            vectornav_tlm_pitchU_deg = new List<float>();
            vectornav_tlm_rollU_deg = new List<float>();

            vectornav_tlm_posU_m = new List<float>();
            vectornav_tlm_velU_m_s = new List<float>();
            vectornav_tlm_GPS1GDOP = new List<float>();
            vectornav_tlm_GPS1TDOP = new List<float>();
            vectornav_tlm_GPS2GDOP = new List<float>();
            vectornav_tlm_numGPS1Sats = new List<byte>();
            vectornav_tlm_numGPS2Sats = new List<byte>();
            vectornav_tlm_GPS1Fix = new List<byte>();
            vectornav_tlm_GPS2Fix = new List<byte>();
            vectornav_tlm_INSStatus = new List<ushort>();

            vectornav_tlm_AHRSStatus = new List<ushort>();
            vectornav_tlm_temp_C = new List<float>();
            vectornav_tlm_press_kPa = new List<float>();
            vectornav_tlm_Yaw_deg = new List<float>();
            vectornav_tlm_Pitch_deg = new List<float>();
            vectornav_tlm_Roll_deg = new List<float>();
            vectornav_tlm_linAcc_Bx_m_s2 = new List<float>();
            vectornav_tlm_linAcc_By_m_s2 = new List<float>();
            vectornav_tlm_linAcc_Bz_m_s2 = new List<float>();
            prop_rpm = new List<float>();

            battery_V = new List<float>();
            fuel_percent = new List<float>();
            meas_AGL_m = new List<float>();
            slow_status_pkt_errors = new List<int>();
            slow_status_pkt_warnings = new List<int>();
            slow_status_pkt_battery_V = new List<float>();
            slow_status_pkt_fuel_percent = new List<float>();
            slow_status_pkt_flagsA = new List<byte>();
            slow_status_pkt_power_status = new List<byte>();
            slow_status_pkt_flagsB = new List<byte>();

            slow_status_pkt_battery_A = new List<float>();
            slow_status_pkt_generator_V = new List<float>();
            slow_status_pkt_generator_A = new List<float>();
            env_pkt_nose_airtemp_C = new List<float>();
            env_pkt_fuel_flow_lb_p_h = new List<float>();
            env_pkt_est_fuel_wght_lb = new List<float>();
            env_pkt_eng_cyl1_temp_C = new List<float>();
            env_pkt_eng_cyl2_temp_C = new List<float>();
            env_pkt_valid = new List<ushort>();
            pss8_pkt_impact_press_Pa = new List<float>();

            pss8_pkt_static_press_Pa = new List<float>();
            pss8_pkt_cal_airspeed_m_s = new List<float>();
            pss8_pkt_true_airspeed_m_s = new List<float>();
            pss8_pkt_press_altitude_m = new List<float>();
            pss8_pkt_quality = new List<ushort>();
            pss8_pkt_flags = new List<ushort>();
            pss8_pkt_static_temp_C = new List<float>();
            pss8_pkt_total_temp_C = new List<float>();
            pss8_pkt_slow_quality = new List<ushort>();
            pss8_pkt_slow_flags = new List<ushort>();

            running_time_msec = new List<uint>();
        }

        public void Clear()
        {
            meas_w_B_ECI_B_deg_s1.Clear();
            meas_w_B_ECI_B_deg_s2.Clear();
            meas_w_B_ECI_B_deg_s3.Clear();
            filt_w_B_ECI_B_deg_s1.Clear();
            filt_w_B_ECI_B_deg_s2.Clear();
            filt_w_B_ECI_B_deg_s3.Clear();
            q_B_NED1.Clear();
            q_B_NED2.Clear();
            q_B_NED3.Clear();
            q_B_NED4.Clear();

            Euler321_B_NED_hs_deg_psi_hs_deg.Clear();
            Euler321_B_NED_hs_deg_theta_hs_deg.Clear();
            Euler321_B_NED_hs_deg_phi_hs_deg.Clear();
            Euler312_B_NED_hv_deg_psi_deg.Clear();
            Euler312_B_NED_hv_deg_phi_deg.Clear();
            Euler312_B_NED_hv_deg_theta_deg.Clear();
            filt_airspeed_knots.Clear();
            airspeed_invalid.Clear();
            altimeter_ft.Clear();
            est_alt_ft.Clear();

            est_hdot_ft_s.Clear();
            AGL_bias_ft.Clear();
            time_unix_sec.Clear();
            H_MSL_ft.Clear();
            QNH_Pa.Clear();
            Vel_B_ECF_NED_knots1.Clear();
            Vel_B_ECF_NED_knots2.Clear();
            Vel_B_ECF_NED_knots3.Clear();
            sog_knots.Clear();
            cog_deg.Clear();

            rc_input_rc_aileron.Clear();
            rc_input_rc_elevator.Clear();
            rc_input_rc_flap.Clear();
            rc_input_rc_gear.Clear();
            rc_input_rc_rudder.Clear();
            rc_input_rc_throttle.Clear();
            rc_input_rc_man_bckup_lsb.Clear();
            rc_control_en.Clear();
            pos_NE_ft1.Clear();
            pos_NE_ft2.Clear();

            alt_INS_ft.Clear();
            meas_lat_rad.Clear();
            meas_lng_rad.Clear();
            gps_ready.Clear();
            init_lat_lon_rad1.Clear();
            init_lat_lon_rad2.Clear();
            pos_initialized.Clear();
            Accel_NED_ft_s_s1.Clear();
            Accel_NED_ft_s_s2.Clear();
            Accel_NED_ft_s_s3.Clear();

            est_pos_NE_ft1.Clear();
            est_pos_NE_ft2.Clear();
            est_vel_NE_kts1.Clear();
            est_vel_NE_kts2.Clear();
            linux_time_s.Clear();
            INS_solution_valid.Clear();
            est_alt_bias_ft.Clear();
            est_accel_bias_ft_s2.Clear();
            vectornav_tlm_rawPositionLat_deg.Clear();
            vectornav_tlm_rawPositionLon_deg.Clear();

            vectornav_tlm_rawPositionAlt_m.Clear();
            vectornav_tlm_rawVel_N_m_s.Clear();
            vectornav_tlm_rawVel_E_m_s.Clear();
            vectornav_tlm_rawVel_D_m_s.Clear();
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Clear();
            vectornav_tlm_uncmpAngRate_By_rad_s.Clear();
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Clear();
            vectornav_tlm_accel_Bx_m_s2.Clear();
            vectornav_tlm_accel_By_m_s2.Clear();
            vectornav_tlm_accel_Bz_m_s2.Clear();

            vectornav_tlm_uncmpAccel_Bx_m_s2.Clear();
            vectornav_tlm_uncmpAccel_By_m_s2.Clear();
            vectornav_tlm_uncmpAccel_Bz_m_s2.Clear();
            vectornav_tlm_mag_Bx_G.Clear();
            vectornav_tlm_mag_By_G.Clear();
            vectornav_tlm_mag_Bz_G.Clear();
            vectornav_tlm_sensSat.Clear();
            vectornav_tlm_yawU_deg.Clear();
            vectornav_tlm_pitchU_deg.Clear();
            vectornav_tlm_rollU_deg.Clear();

            vectornav_tlm_posU_m.Clear();
            vectornav_tlm_velU_m_s.Clear();
            vectornav_tlm_GPS1GDOP.Clear();
            vectornav_tlm_GPS1TDOP.Clear();
            vectornav_tlm_GPS2GDOP.Clear();
            vectornav_tlm_numGPS1Sats.Clear();
            vectornav_tlm_numGPS2Sats.Clear();
            vectornav_tlm_GPS1Fix.Clear();
            vectornav_tlm_GPS2Fix.Clear();
            vectornav_tlm_INSStatus.Clear();

            vectornav_tlm_AHRSStatus.Clear();
            vectornav_tlm_temp_C.Clear();
            vectornav_tlm_press_kPa.Clear();
            vectornav_tlm_Yaw_deg.Clear();
            vectornav_tlm_Pitch_deg.Clear();
            vectornav_tlm_Roll_deg.Clear();
            vectornav_tlm_linAcc_Bx_m_s2.Clear();
            vectornav_tlm_linAcc_By_m_s2.Clear();
            vectornav_tlm_linAcc_Bz_m_s2.Clear();
            prop_rpm.Clear();

            battery_V.Clear();
            fuel_percent.Clear();
            meas_AGL_m.Clear();
            slow_status_pkt_errors.Clear();
            slow_status_pkt_warnings.Clear();
            slow_status_pkt_battery_V.Clear();
            slow_status_pkt_fuel_percent.Clear();
            slow_status_pkt_flagsA.Clear();
            slow_status_pkt_power_status.Clear();
            slow_status_pkt_flagsB.Clear();

            slow_status_pkt_battery_A.Clear();
            slow_status_pkt_generator_V.Clear();
            slow_status_pkt_generator_A.Clear();
            env_pkt_nose_airtemp_C.Clear();
            env_pkt_fuel_flow_lb_p_h.Clear();
            env_pkt_est_fuel_wght_lb.Clear();
            env_pkt_eng_cyl1_temp_C.Clear();
            env_pkt_eng_cyl2_temp_C.Clear();
            env_pkt_valid.Clear();
            pss8_pkt_impact_press_Pa.Clear();

            pss8_pkt_static_press_Pa.Clear();
            pss8_pkt_cal_airspeed_m_s.Clear();
            pss8_pkt_true_airspeed_m_s.Clear();
            pss8_pkt_press_altitude_m.Clear();
            pss8_pkt_quality.Clear();
            pss8_pkt_flags.Clear();
            pss8_pkt_static_temp_C.Clear();
            pss8_pkt_total_temp_C.Clear();
            pss8_pkt_slow_quality.Clear();
            pss8_pkt_slow_flags.Clear();

            running_time_msec.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int shortVal = 2;
            int byteVal = 1;
            //Debug.WriteLine(input.Length.ToString());
            meas_w_B_ECI_B_deg_s1.Add(BitConverter.ToDouble(input, start));
            meas_w_B_ECI_B_deg_s2.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_w_B_ECI_B_deg_s3.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s1.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s2.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s3.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED1.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED2.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED3.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED4.Add(BitConverter.ToDouble(input, start += doubleVal));

            Euler321_B_NED_hs_deg_psi_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler321_B_NED_hs_deg_theta_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler321_B_NED_hs_deg_phi_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_psi_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_phi_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_theta_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_airspeed_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            airspeed_invalid.Add(BitConverter.ToBoolean(input, start += doubleVal));
            altimeter_ft.Add(BitConverter.ToDouble(input, start += byteVal));
            est_alt_ft.Add(BitConverter.ToDouble(input, start += doubleVal));

            est_hdot_ft_s.Add(BitConverter.ToDouble(input, start += doubleVal));
            AGL_bias_ft.Add(BitConverter.ToSingle(input, start += doubleVal));
            time_unix_sec.Add(BitConverter.ToDouble(input, start += floatVal));
            H_MSL_ft.Add(BitConverter.ToSingle(input, start += doubleVal));
            QNH_Pa.Add(BitConverter.ToSingle(input, start += floatVal));
            Vel_B_ECF_NED_knots1.Add(BitConverter.ToDouble(input, start += floatVal));
            Vel_B_ECF_NED_knots2.Add(BitConverter.ToDouble(input, start += doubleVal));
            Vel_B_ECF_NED_knots3.Add(BitConverter.ToDouble(input, start += doubleVal));
            sog_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            cog_deg.Add(BitConverter.ToDouble(input, start += doubleVal));

            rc_input_rc_aileron.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_elevator.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_flap.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_gear.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_rudder.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_throttle.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_man_bckup_lsb.Add(BitConverter.ToInt16(input, start += doubleVal));
            rc_control_en.Add(BitConverter.ToBoolean(input, start += shortVal));
            pos_NE_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            pos_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));

            alt_INS_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_lat_rad.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_lng_rad.Add(BitConverter.ToDouble(input, start += doubleVal));
            gps_ready.Add(BitConverter.ToBoolean(input, start += doubleVal));
            init_lat_lon_rad1.Add(BitConverter.ToDouble(input, start += byteVal));
            init_lat_lon_rad2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_initialized.Add(BitConverter.ToBoolean(input, start += doubleVal));
            Accel_NED_ft_s_s1.Add(BitConverter.ToSingle(input, start += byteVal));
            Accel_NED_ft_s_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            Accel_NED_ft_s_s3.Add(BitConverter.ToSingle(input, start += floatVal));

            est_pos_NE_ft1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_pos_NE_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_NE_kts1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_NE_kts2.Add(BitConverter.ToSingle(input, start += floatVal));
            linux_time_s.Add(BitConverter.ToDouble(input, start += floatVal));
            INS_solution_valid.Add(BitConverter.ToBoolean(input, start += doubleVal));
            est_alt_bias_ft.Add(BitConverter.ToDouble(input, start += byteVal));
            est_accel_bias_ft_s2.Add(BitConverter.ToDouble(input, start += doubleVal));
            vectornav_tlm_rawPositionLat_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            vectornav_tlm_rawPositionLon_deg.Add(BitConverter.ToDouble(input, start += doubleVal));

            vectornav_tlm_rawPositionAlt_m.Add(BitConverter.ToDouble(input, start += doubleVal));
            vectornav_tlm_rawVel_N_m_s.Add(BitConverter.ToSingle(input, start += doubleVal));
            vectornav_tlm_rawVel_E_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_rawVel_D_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_By_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));

            vectornav_tlm_uncmpAccel_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAccel_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAccel_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_Bx_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_By_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_Bz_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_sensSat.Add(BitConverter.ToUInt16(input, start += floatVal));
            vectornav_tlm_yawU_deg.Add(BitConverter.ToSingle(input, start += shortVal));
            vectornav_tlm_pitchU_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_rollU_deg.Add(BitConverter.ToSingle(input, start += floatVal));

            vectornav_tlm_posU_m.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_velU_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS1GDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS1TDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS2GDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_numGPS1Sats.Add((byte)BitConverter.ToChar(input, start += floatVal));
            vectornav_tlm_numGPS2Sats.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_GPS1Fix.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_GPS2Fix.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_INSStatus.Add(BitConverter.ToUInt16(input, start += byteVal));

            vectornav_tlm_AHRSStatus.Add(BitConverter.ToUInt16(input, start += shortVal));
            vectornav_tlm_temp_C.Add(BitConverter.ToSingle(input, start += shortVal));
            vectornav_tlm_press_kPa.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Yaw_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Pitch_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Roll_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            prop_rpm.Add(BitConverter.ToSingle(input, start += floatVal));

            battery_V.Add(BitConverter.ToSingle(input, start += floatVal));
            fuel_percent.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_AGL_m.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_errors.Add(BitConverter.ToInt32(input, start += floatVal));
            slow_status_pkt_warnings.Add(BitConverter.ToInt32(input, start += intVal));
            slow_status_pkt_battery_V.Add(BitConverter.ToSingle(input, start += intVal));
            slow_status_pkt_fuel_percent.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_flagsA.Add((byte)BitConverter.ToChar(input, start += floatVal));
            slow_status_pkt_power_status.Add((byte)BitConverter.ToChar(input, start += byteVal));
            slow_status_pkt_flagsB.Add((byte)BitConverter.ToChar(input, start += byteVal));

            slow_status_pkt_battery_A.Add(BitConverter.ToSingle(input, start += byteVal));
            slow_status_pkt_generator_V.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_generator_A.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_nose_airtemp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_fuel_flow_lb_p_h.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_est_fuel_wght_lb.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_eng_cyl1_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_eng_cyl2_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_valid.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_impact_press_Pa.Add(BitConverter.ToSingle(input, start += shortVal));

            pss8_pkt_static_press_Pa.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_cal_airspeed_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_true_airspeed_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_press_altitude_m.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_quality.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_flags.Add(BitConverter.ToUInt16(input, start += shortVal));
            pss8_pkt_static_temp_C.Add(BitConverter.ToSingle(input, start += shortVal));
            pss8_pkt_total_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_slow_quality.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_slow_flags.Add(BitConverter.ToUInt16(input, start += shortVal));
            //Debug.WriteLine(start);
            //running_time_msec.Add(BitConverter.ToUInt32(input, start += shortVal));
            running_time_msec.Add(BitConverter.ToUInt32(input, start += shortVal));
            //Debug.WriteLine("Filled  Class");
        }
        public void ReadBytesData(Stream stream)
        {
            meas_w_B_ECI_B_deg_s1.Add(Reader.ReadDouble(stream));
            meas_w_B_ECI_B_deg_s2.Add(Reader.ReadDouble(stream));
            meas_w_B_ECI_B_deg_s3.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s1.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s2.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s3.Add(Reader.ReadDouble(stream));
            q_B_NED1.Add(Reader.ReadDouble(stream));
            q_B_NED2.Add(Reader.ReadDouble(stream));
            q_B_NED3.Add(Reader.ReadDouble(stream));
            q_B_NED4.Add(Reader.ReadDouble(stream));

            Euler321_B_NED_hs_deg_psi_hs_deg.Add(Reader.ReadDouble(stream));
            Euler321_B_NED_hs_deg_theta_hs_deg.Add(Reader.ReadDouble(stream));
            Euler321_B_NED_hs_deg_phi_hs_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_psi_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_phi_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_theta_deg.Add(Reader.ReadDouble(stream));
            filt_airspeed_knots.Add(Reader.ReadDouble(stream));
            airspeed_invalid.Add(Reader.ReadBool(stream));
            altimeter_ft.Add(Reader.ReadDouble(stream));
            est_alt_ft.Add(Reader.ReadDouble(stream));

            est_hdot_ft_s.Add(Reader.ReadDouble(stream));
            AGL_bias_ft.Add(Reader.ReadFloat(stream));
            time_unix_sec.Add(Reader.ReadDouble(stream));
            H_MSL_ft.Add(Reader.ReadFloat(stream));
            QNH_Pa.Add(Reader.ReadFloat(stream));
            Vel_B_ECF_NED_knots1.Add(Reader.ReadDouble(stream));
            Vel_B_ECF_NED_knots2.Add(Reader.ReadDouble(stream));
            Vel_B_ECF_NED_knots3.Add(Reader.ReadDouble(stream));
            sog_knots.Add(Reader.ReadDouble(stream));
            cog_deg.Add(Reader.ReadDouble(stream));

            rc_input_rc_aileron.Add(Reader.ReadDouble(stream));
            rc_input_rc_elevator.Add(Reader.ReadDouble(stream));
            rc_input_rc_flap.Add(Reader.ReadDouble(stream));
            rc_input_rc_gear.Add(Reader.ReadDouble(stream));
            rc_input_rc_rudder.Add(Reader.ReadDouble(stream));
            rc_input_rc_throttle.Add(Reader.ReadDouble(stream));
            rc_input_rc_man_bckup_lsb.Add(Reader.ReadShort(stream));
            rc_control_en.Add(Reader.ReadBool(stream));
            pos_NE_ft1.Add(Reader.ReadDouble(stream));
            pos_NE_ft2.Add(Reader.ReadDouble(stream));

            alt_INS_ft.Add(Reader.ReadDouble(stream));
            meas_lat_rad.Add(Reader.ReadDouble(stream));
            meas_lng_rad.Add(Reader.ReadDouble(stream));
            gps_ready.Add(Reader.ReadBool(stream));
            init_lat_lon_rad1.Add(Reader.ReadDouble(stream));
            init_lat_lon_rad2.Add(Reader.ReadDouble(stream));
            pos_initialized.Add(Reader.ReadBool(stream));
            Accel_NED_ft_s_s1.Add(Reader.ReadFloat(stream));
            Accel_NED_ft_s_s2.Add(Reader.ReadFloat(stream));
            Accel_NED_ft_s_s3.Add(Reader.ReadFloat(stream));

            est_pos_NE_ft1.Add(Reader.ReadFloat(stream));
            est_pos_NE_ft2.Add(Reader.ReadFloat(stream));
            est_vel_NE_kts1.Add(Reader.ReadFloat(stream));
            est_vel_NE_kts2.Add(Reader.ReadFloat(stream));
            linux_time_s.Add(Reader.ReadDouble(stream));
            INS_solution_valid.Add(Reader.ReadBool(stream));
            est_alt_bias_ft.Add(Reader.ReadDouble(stream));
            est_accel_bias_ft_s2.Add(Reader.ReadDouble(stream));
            vectornav_tlm_rawPositionLat_deg.Add(Reader.ReadDouble(stream));
            vectornav_tlm_rawPositionLon_deg.Add(Reader.ReadDouble(stream));

            vectornav_tlm_rawPositionAlt_m.Add(Reader.ReadDouble(stream));
            vectornav_tlm_rawVel_N_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rawVel_E_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rawVel_D_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_By_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_Bz_m_s2.Add(Reader.ReadFloat(stream));

            vectornav_tlm_uncmpAccel_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAccel_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAccel_Bz_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_Bx_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_By_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_Bz_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_sensSat.Add(Reader.ReadUShort(stream));
            vectornav_tlm_yawU_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_pitchU_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rollU_deg.Add(Reader.ReadFloat(stream));

            vectornav_tlm_posU_m.Add(Reader.ReadFloat(stream));
            vectornav_tlm_velU_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS1GDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS1TDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS2GDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_numGPS1Sats.Add(Reader.ReadByte(stream));
            vectornav_tlm_numGPS2Sats.Add(Reader.ReadByte(stream));
            vectornav_tlm_GPS1Fix.Add(Reader.ReadByte(stream));
            vectornav_tlm_GPS2Fix.Add(Reader.ReadByte(stream));
            vectornav_tlm_INSStatus.Add(Reader.ReadUShort(stream));

            vectornav_tlm_AHRSStatus.Add(Reader.ReadUShort(stream));
            vectornav_tlm_temp_C.Add(Reader.ReadFloat(stream));
            vectornav_tlm_press_kPa.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Yaw_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Pitch_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Roll_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_Bz_m_s2.Add(Reader.ReadFloat(stream));
            prop_rpm.Add(Reader.ReadFloat(stream));

            battery_V.Add(Reader.ReadFloat(stream));
            fuel_percent.Add(Reader.ReadFloat(stream));
            meas_AGL_m.Add(Reader.ReadFloat(stream));
            slow_status_pkt_errors.Add(Reader.ReadInt(stream));
            slow_status_pkt_warnings.Add(Reader.ReadInt(stream));
            slow_status_pkt_battery_V.Add(Reader.ReadFloat(stream));
            slow_status_pkt_fuel_percent.Add(Reader.ReadFloat(stream));
            slow_status_pkt_flagsA.Add(Reader.ReadByte(stream));
            slow_status_pkt_power_status.Add(Reader.ReadByte(stream));
            slow_status_pkt_flagsB.Add(Reader.ReadByte(stream));

            slow_status_pkt_battery_A.Add(Reader.ReadFloat(stream));
            slow_status_pkt_generator_V.Add(Reader.ReadFloat(stream));
            slow_status_pkt_generator_A.Add(Reader.ReadFloat(stream));
            env_pkt_nose_airtemp_C.Add(Reader.ReadFloat(stream));
            env_pkt_fuel_flow_lb_p_h.Add(Reader.ReadFloat(stream));
            env_pkt_est_fuel_wght_lb.Add(Reader.ReadFloat(stream));
            env_pkt_eng_cyl1_temp_C.Add(Reader.ReadFloat(stream));
            env_pkt_eng_cyl2_temp_C.Add(Reader.ReadFloat(stream));
            env_pkt_valid.Add(Reader.ReadUShort(stream));
            pss8_pkt_impact_press_Pa.Add(Reader.ReadFloat(stream));

            pss8_pkt_static_press_Pa.Add(Reader.ReadFloat(stream));
            pss8_pkt_cal_airspeed_m_s.Add(Reader.ReadFloat(stream));
            pss8_pkt_true_airspeed_m_s.Add(Reader.ReadFloat(stream));
            pss8_pkt_press_altitude_m.Add(Reader.ReadFloat(stream));
            pss8_pkt_quality.Add(Reader.ReadUShort(stream));
            pss8_pkt_flags.Add(Reader.ReadUShort(stream));
            pss8_pkt_static_temp_C.Add(Reader.ReadFloat(stream));
            pss8_pkt_total_temp_C.Add(Reader.ReadFloat(stream));
            pss8_pkt_slow_quality.Add(Reader.ReadUShort(stream));
            pss8_pkt_slow_flags.Add(Reader.ReadUShort(stream));

            running_time_msec.Add(Reader.ReadUInt(stream));
        }
    }
    public class ControlData17
    {

        public int totalLength = 161;
        public int dataLength = 22;

        public List<double> linux_time_s { get; set; }
        public List<double> elevator_degrees { get; set; }
        public List<double> throttle_degrees { get; set; }
        public List<double> rudder_degrees { get; set; }
        public List<double> aileron_degrees { get; set; }
        public List<double> roll_degrees { get; set; }
        public List<double> alt_error { get; set; }
        public List<double> vel_err_Prime_ftps1 { get; set; }
        public List<double> vel_err_Prime_ftps2 { get; set; }
        public List<double> pos_err_Prime_ft1 { get; set; }

        public List<double> pos_err_Prime_ft2 { get; set; }
        public List<double> integ_alt_err_deg { get; set; }
        public List<double> theta_cmd_deg { get; set; }
        public List<double> phi_cmd_deg { get; set; }
        public List<double> hv_att_error_deg1 { get; set; }
        public List<double> hv_att_error_deg2 { get; set; }
        public List<double> hv_att_error_deg3 { get; set; }
        public List<double> hv_int_psi_error_deg { get; set; }
        public List<float> speed_err_int_kt { get; set; }
        public List<float> attHv_integ1 { get; set; }

        public List<float> attHv_integ2 { get; set; }
        public List<float> attHv_integ3 { get; set; }
        public List<byte> saturate_ctr { get; set; }
        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public ControlData17()
        {
            //linux_time_s = new List<int>();
            linux_time_s = new List<double>();
            elevator_degrees = new List<double>();
            throttle_degrees = new List<double>();
            rudder_degrees = new List<double>();
            aileron_degrees = new List<double>();
            roll_degrees = new List<double>();
            alt_error = new List<double>();
            vel_err_Prime_ftps1 = new List<double>();
            vel_err_Prime_ftps2 = new List<double>();
            pos_err_Prime_ft1 = new List<double>();

            pos_err_Prime_ft2 = new List<double>();
            integ_alt_err_deg = new List<double>();
            theta_cmd_deg = new List<double>();
            phi_cmd_deg = new List<double>();
            hv_att_error_deg1 = new List<double>();
            hv_att_error_deg2 = new List<double>();
            hv_att_error_deg3 = new List<double>();
            hv_int_psi_error_deg = new List<double>();
            speed_err_int_kt = new List<float>();
            attHv_integ1 = new List<float>();

            attHv_integ2 = new List<float>();
            attHv_integ3 = new List<float>();
            saturate_ctr = new List<byte>();


        }

        public void Clear()
        {
            linux_time_s.Clear();
            elevator_degrees.Clear();
            throttle_degrees.Clear();
            rudder_degrees.Clear();
            aileron_degrees.Clear();
            roll_degrees.Clear();
            alt_error.Clear();
            vel_err_Prime_ftps1.Clear();
            vel_err_Prime_ftps2.Clear();
            pos_err_Prime_ft1.Clear();

            pos_err_Prime_ft2.Clear();
            integ_alt_err_deg.Clear();
            theta_cmd_deg.Clear();
            phi_cmd_deg.Clear();
            hv_att_error_deg1.Clear();
            hv_att_error_deg2.Clear();
            hv_att_error_deg3.Clear();
            hv_int_psi_error_deg.Clear();
            speed_err_int_kt.Clear();
            attHv_integ1.Clear();

            attHv_integ2.Clear();
            attHv_integ3.Clear();
            saturate_ctr.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            //int byteVal = 1;
            //Debug.WriteLine(input.Length.ToString());
            linux_time_s.Add(BitConverter.ToDouble(input, start));
            elevator_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            throttle_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            rudder_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            aileron_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            roll_degrees.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_error.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_err_Prime_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_err_Prime_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_err_Prime_ft1.Add(BitConverter.ToDouble(input, start += doubleVal));

            pos_err_Prime_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            integ_alt_err_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            theta_cmd_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            phi_cmd_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg1.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg2.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_att_error_deg3.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_int_psi_error_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            speed_err_int_kt.Add(BitConverter.ToSingle(input, start += doubleVal));
            attHv_integ1.Add(BitConverter.ToSingle(input, start += floatVal));

            attHv_integ2.Add(BitConverter.ToSingle(input, start += floatVal));
            attHv_integ3.Add(BitConverter.ToSingle(input, start += floatVal));
            saturate_ctr.Add((byte)BitConverter.ToChar(input, start += floatVal));
            //saturate_ctr.Add((byte)BitConverter.ToChar(input, start));
            //Debug.WriteLine("Filled Class");
        }
        public void ReadBytesData(Stream stream)
        {
            //linux_time_s.Add(Reader.ReadInt(stream));
            linux_time_s.Add(Reader.ReadDouble(stream));
            elevator_degrees.Add(Reader.ReadDouble(stream));
            throttle_degrees.Add(Reader.ReadDouble(stream));
            rudder_degrees.Add(Reader.ReadDouble(stream));
            aileron_degrees.Add(Reader.ReadDouble(stream));
            roll_degrees.Add(Reader.ReadDouble(stream));
            alt_error.Add(Reader.ReadDouble(stream));
            vel_err_Prime_ftps1.Add(Reader.ReadDouble(stream));
            vel_err_Prime_ftps2.Add(Reader.ReadDouble(stream));
            pos_err_Prime_ft1.Add(Reader.ReadDouble(stream));

            pos_err_Prime_ft2.Add(Reader.ReadDouble(stream));
            integ_alt_err_deg.Add(Reader.ReadDouble(stream));
            theta_cmd_deg.Add(Reader.ReadDouble(stream));
            phi_cmd_deg.Add(Reader.ReadDouble(stream));
            hv_att_error_deg1.Add(Reader.ReadDouble(stream));
            hv_att_error_deg2.Add(Reader.ReadDouble(stream));
            hv_att_error_deg3.Add(Reader.ReadDouble(stream));
            hv_int_psi_error_deg.Add(Reader.ReadDouble(stream));
            speed_err_int_kt.Add(Reader.ReadFloat(stream));
            attHv_integ1.Add(Reader.ReadFloat(stream));

            attHv_integ2.Add(Reader.ReadFloat(stream));
            attHv_integ3.Add(Reader.ReadFloat(stream));
            saturate_ctr.Add(Reader.ReadByte(stream));
        }

    }


    public class NavigationData17
    {

        public int totalLength = 186;
        public int dataLength = 41;

        public List<double> linux_time_s { get; set; }
        public List<float> est_V_wind_N_ftps { get; set; }
        public List<float> est_V_wind_E_ftps { get; set; }
        public List<float> P11_V_wind_N { get; set; }
        public List<float> P22_V_wind_E { get; set; }
        public List<float> P33_IAS_bias { get; set; }
        public List<float> TAS_meas_residual_ftps { get; set; }
        public List<float> rho_kg_m3 { get; set; }
        public List<float> est_IAS_bias_ftps { get; set; }
        public List<float> est_wind_dir_deg { get; set; }

        public List<float> est_wind_speed_kts { get; set; }
        public List<float> density_alt_ft { get; set; }
        public List<uint> tag_age_body_ms { get; set; }
        public List<float> tag_pos_B_T_B_m1 { get; set; }
        public List<float> tag_pos_B_T_B_m2 { get; set; }
        public List<float> tag_pos_B_T_B_m3 { get; set; }
        public List<uint> tag_age_ned_ms { get; set; }
        public List<float> tag_pos_B_T_NED_m1 { get; set; }
        public List<float> tag_pos_B_T_NED_m2 { get; set; }
        public List<float> tag_pos_B_T_NED_m3 { get; set; }

        public List<float> tag_vel_T_B_NED_m_s1 { get; set; }
        public List<float> tag_vel_T_B_NED_m_s2 { get; set; }
        public List<float> tag_vel_T_B_NED_m_s3 { get; set; }
        public List<int> accum_body_upd { get; set; }
        public List<int> accum_ned_upd { get; set; }
        public List<sbyte> tracking { get; set; }
        public List<double> pos_B_T_NED_ft1 { get; set; }
        public List<double> pos_B_T_NED_ft2 { get; set; }
        public List<double> pos_B_T_NED_ft3 { get; set; }
        public List<double> vel_T_B_NED_ftps1 { get; set; }

        public List<double> vel_T_B_NED_ftps2 { get; set; }
        public List<double> vel_T_B_NED_ftps3 { get; set; }
        public List<float> est_pos_B_T_NED_ft1 { get; set; }
        public List<float> est_pos_B_T_NED_ft2 { get; set; }
        public List<float> est_pos_B_T_NED_ft3 { get; set; }
        public List<float> est_vel_T_B_NED_ftps1 { get; set; }
        public List<float> est_vel_T_B_NED_ftps2 { get; set; }
        public List<float> est_vel_T_B_NED_ftps3 { get; set; }
        public List<sbyte> altitude_low { get; set; }
        public List<float> V_min_kts { get; set; }

        public List<float> est_mass_lbm { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public NavigationData17()
        {
            linux_time_s = new List<double>();
            est_V_wind_N_ftps = new List<float>();
            est_V_wind_E_ftps = new List<float>();
            P11_V_wind_N = new List<float>();
            P22_V_wind_E = new List<float>();
            P33_IAS_bias = new List<float>();
            TAS_meas_residual_ftps = new List<float>();
            rho_kg_m3 = new List<float>();
            est_IAS_bias_ftps = new List<float>();
            est_wind_dir_deg = new List<float>();

            est_wind_speed_kts = new List<float>();
            density_alt_ft = new List<float>();
            tag_age_body_ms = new List<uint>();
            tag_pos_B_T_B_m1 = new List<float>();
            tag_pos_B_T_B_m2 = new List<float>();
            tag_pos_B_T_B_m3 = new List<float>();
            tag_age_ned_ms = new List<uint>();
            tag_pos_B_T_NED_m1 = new List<float>();
            tag_pos_B_T_NED_m2 = new List<float>();
            tag_pos_B_T_NED_m3 = new List<float>();

            tag_vel_T_B_NED_m_s1 = new List<float>();
            tag_vel_T_B_NED_m_s2 = new List<float>();
            tag_vel_T_B_NED_m_s3 = new List<float>();
            accum_body_upd = new List<int>();
            accum_ned_upd = new List<int>();
            tracking = new List<sbyte>();
            pos_B_T_NED_ft1 = new List<double>();
            pos_B_T_NED_ft2 = new List<double>();
            pos_B_T_NED_ft3 = new List<double>();
            vel_T_B_NED_ftps1 = new List<double>();

            vel_T_B_NED_ftps2 = new List<double>();
            vel_T_B_NED_ftps3 = new List<double>();
            est_pos_B_T_NED_ft1 = new List<float>();
            est_pos_B_T_NED_ft2 = new List<float>();
            est_pos_B_T_NED_ft3 = new List<float>();
            est_vel_T_B_NED_ftps1 = new List<float>();
            est_vel_T_B_NED_ftps2 = new List<float>();
            est_vel_T_B_NED_ftps3 = new List<float>();
            altitude_low = new List<sbyte>();
            V_min_kts = new List<float>();

            est_mass_lbm = new List<float>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            est_V_wind_N_ftps.Clear();
            est_V_wind_E_ftps.Clear();
            P11_V_wind_N.Clear();
            P22_V_wind_E.Clear();
            P33_IAS_bias.Clear();
            TAS_meas_residual_ftps.Clear();
            rho_kg_m3.Clear();
            est_IAS_bias_ftps.Clear();
            est_wind_dir_deg.Clear();

            est_wind_speed_kts.Clear();
            density_alt_ft.Clear();
            tag_age_body_ms.Clear();
            tag_pos_B_T_B_m1.Clear();
            tag_pos_B_T_B_m2.Clear();
            tag_pos_B_T_B_m3.Clear();
            tag_age_ned_ms.Clear();
            tag_pos_B_T_NED_m1.Clear();
            tag_pos_B_T_NED_m2.Clear();
            tag_pos_B_T_NED_m3.Clear();

            tag_vel_T_B_NED_m_s1.Clear();
            tag_vel_T_B_NED_m_s2.Clear();
            tag_vel_T_B_NED_m_s3.Clear();
            accum_body_upd.Clear();
            accum_ned_upd.Clear();
            tracking.Clear();
            pos_B_T_NED_ft1.Clear();
            pos_B_T_NED_ft2.Clear();
            pos_B_T_NED_ft3.Clear();
            vel_T_B_NED_ftps1.Clear();

            vel_T_B_NED_ftps2.Clear();
            vel_T_B_NED_ftps3.Clear();
            est_pos_B_T_NED_ft1.Clear();
            est_pos_B_T_NED_ft2.Clear();
            est_pos_B_T_NED_ft3.Clear();
            est_vel_T_B_NED_ftps1.Clear();
            est_vel_T_B_NED_ftps2.Clear();
            est_vel_T_B_NED_ftps3.Clear();
            altitude_low.Clear();
            V_min_kts.Clear();

            est_mass_lbm.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int byteVal = 1;


            linux_time_s.Add(BitConverter.ToDouble(input, start));
            est_V_wind_N_ftps.Add(BitConverter.ToSingle(input, start += doubleVal));
            est_V_wind_E_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            P11_V_wind_N.Add(BitConverter.ToSingle(input, start += floatVal));
            P22_V_wind_E.Add(BitConverter.ToSingle(input, start += floatVal));
            P33_IAS_bias.Add(BitConverter.ToSingle(input, start += floatVal));
            TAS_meas_residual_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            rho_kg_m3.Add(BitConverter.ToSingle(input, start += floatVal));
            est_IAS_bias_ftps.Add(BitConverter.ToSingle(input, start += floatVal));
            est_wind_dir_deg.Add(BitConverter.ToSingle(input, start += floatVal));

            est_wind_speed_kts.Add(BitConverter.ToSingle(input, start += floatVal));
            density_alt_ft.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_age_body_ms.Add(BitConverter.ToUInt32(input, start += floatVal));
            tag_pos_B_T_B_m1.Add(BitConverter.ToSingle(input, start += intVal));
            tag_pos_B_T_B_m2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_pos_B_T_B_m3.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_age_ned_ms.Add(BitConverter.ToUInt32(input, start += floatVal));
            tag_pos_B_T_NED_m1.Add(BitConverter.ToSingle(input, start += intVal));
            tag_pos_B_T_NED_m2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_pos_B_T_NED_m3.Add(BitConverter.ToSingle(input, start += floatVal));

            tag_vel_T_B_NED_m_s1.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_vel_T_B_NED_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            tag_vel_T_B_NED_m_s3.Add(BitConverter.ToSingle(input, start += floatVal));
            accum_body_upd.Add(BitConverter.ToInt32(input, start += floatVal));
            accum_ned_upd.Add(BitConverter.ToInt32(input, start += intVal));
            tracking.Add((sbyte)BitConverter.ToChar(input, start += intVal));
            pos_B_T_NED_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            pos_B_T_NED_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_B_T_NED_ft3.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_T_B_NED_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));

            vel_T_B_NED_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_T_B_NED_ftps3.Add(BitConverter.ToDouble(input, start += doubleVal));
            est_pos_B_T_NED_ft1.Add(BitConverter.ToSingle(input, start += doubleVal));
            est_pos_B_T_NED_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_pos_B_T_NED_ft3.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_T_B_NED_ftps3.Add(BitConverter.ToSingle(input, start += floatVal));
            altitude_low.Add((sbyte)BitConverter.ToChar(input, start += floatVal));
            V_min_kts.Add(BitConverter.ToSingle(input, start += byteVal));

            est_mass_lbm.Add(BitConverter.ToSingle(input, start += floatVal));
            //est_mass_lbm.Add(BitConverter.ToSingle(input, start));
            //Debug.WriteLine($"should be {input.Length} but is {start}");
            //Debug.WriteLine($"should be {input.Length} but is {start}");

        }

        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            est_V_wind_N_ftps.Add(Reader.ReadFloat(stream));
            est_V_wind_E_ftps.Add(Reader.ReadFloat(stream));
            P11_V_wind_N.Add(Reader.ReadFloat(stream));
            P22_V_wind_E.Add(Reader.ReadFloat(stream));
            P33_IAS_bias.Add(Reader.ReadFloat(stream));
            TAS_meas_residual_ftps.Add(Reader.ReadFloat(stream));
            rho_kg_m3.Add(Reader.ReadFloat(stream));
            est_IAS_bias_ftps.Add(Reader.ReadFloat(stream));
            est_wind_dir_deg.Add(Reader.ReadFloat(stream));

            est_wind_speed_kts.Add(Reader.ReadFloat(stream));
            density_alt_ft.Add(Reader.ReadFloat(stream));
            tag_age_body_ms.Add(Reader.ReadUInt(stream));
            tag_pos_B_T_B_m1.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_B_m2.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_B_m3.Add(Reader.ReadFloat(stream));
            tag_age_ned_ms.Add(Reader.ReadUInt(stream));
            tag_pos_B_T_NED_m1.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_NED_m2.Add(Reader.ReadFloat(stream));
            tag_pos_B_T_NED_m3.Add(Reader.ReadFloat(stream));

            tag_vel_T_B_NED_m_s1.Add(Reader.ReadFloat(stream));
            tag_vel_T_B_NED_m_s2.Add(Reader.ReadFloat(stream));
            tag_vel_T_B_NED_m_s3.Add(Reader.ReadFloat(stream));
            accum_body_upd.Add(Reader.ReadInt(stream));
            accum_ned_upd.Add(Reader.ReadInt(stream));
            tracking.Add(Reader.ReadSByte(stream));
            pos_B_T_NED_ft1.Add(Reader.ReadDouble(stream));
            pos_B_T_NED_ft2.Add(Reader.ReadDouble(stream));
            pos_B_T_NED_ft3.Add(Reader.ReadDouble(stream));
            vel_T_B_NED_ftps1.Add(Reader.ReadDouble(stream));

            vel_T_B_NED_ftps2.Add(Reader.ReadDouble(stream));
            vel_T_B_NED_ftps3.Add(Reader.ReadDouble(stream));
            est_pos_B_T_NED_ft1.Add(Reader.ReadFloat(stream));
            est_pos_B_T_NED_ft2.Add(Reader.ReadFloat(stream));
            est_pos_B_T_NED_ft3.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps1.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps2.Add(Reader.ReadFloat(stream));
            est_vel_T_B_NED_ftps3.Add(Reader.ReadFloat(stream));
            altitude_low.Add(Reader.ReadSByte(stream));
            V_min_kts.Add(Reader.ReadFloat(stream));

            est_mass_lbm.Add(Reader.ReadFloat(stream));
        }

    }

    public class GCSData17
    {

        public int totalLength = 118;
        public int dataLength = 43;

        public List<double> linux_time_s { get; set; }
        public List<sbyte> shield_command { get; set; }
        public List<float> c2_ack_ratio { get; set; }
        public List<byte> mvg_base_HDOP { get; set; }
        public List<double> est_pos_mvb_NE_ft1 { get; set; }
        public List<double> est_pos_mvb_NE_ft2 { get; set; }
        public List<double> delta_X_ft { get; set; }
        public List<double> delta_Y_ft { get; set; }
        public List<int> accum_mvb_upd { get; set; }
        public List<double> est_vel_mvb_NE_ftps1 { get; set; }

        public List<double> est_vel_mvb_NE_ftps2 { get; set; }
        public List<sbyte> mvb_en { get; set; }
        public List<float> meas_pos_mvb_NE_ft1 { get; set; }
        public List<float> meas_pos_mvb_NE_ft2 { get; set; }
        public List<float> meas_vel_mvb_NE_mps1 { get; set; }
        public List<float> meas_vel_mvb_NE_mps2 { get; set; }
        public List<byte> fp_upload_cnt { get; set; }
        public List<short> fltplan_num_wpt1 { get; set; }
        public List<short> fltplan_num_wpt2 { get; set; }
        public List<short> fltplan_num_wpt3 { get; set; }

        public List<short> fltplan_num_wpt4 { get; set; }
        public List<short> fltplan_num_wpt5 { get; set; }
        public List<short> fltplan_num_wpt6 { get; set; }
        public List<short> fltplan_num_wpt7 { get; set; }
        public List<sbyte> current_fp_changed { get; set; }
        public List<short> next_waypoint { get; set; }
        public List<sbyte> next_wpt_updated { get; set; }
        public List<byte> packet20_freq_hz { get; set; }
        public List<byte> xbus_freq_hz { get; set; }
        public List<sbyte> lost_comm_fault_HV { get; set; }

        public List<sbyte> lost_comm_fault_HS { get; set; }
        public List<sbyte> stick_input_throttle_stick_lsb { get; set; }
        public List<sbyte> stick_input_aileron_stick_lsb { get; set; }
        public List<sbyte> stick_input_elevator_stick_lsb { get; set; }
        public List<sbyte> stick_input_rudder_stick_lsb { get; set; }
        public List<sbyte> stick_input_aux_A_stick_lsb { get; set; }
        public List<sbyte> stick_input_aux_B_stick_lsb { get; set; }
        public List<sbyte> stick_input_engine_disable { get; set; }
        public List<sbyte> stick_input_control_mode { get; set; }
        public List<sbyte> control_mode_rcvd { get; set; }

        public List<sbyte> auto_manvr_reset { get; set; }
        public List<sbyte> loss_of_xbus_fault { get; set; }
        public List<sbyte> constant_bank_en { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public GCSData17()
        {
            linux_time_s = new List<double>();
            shield_command = new List<sbyte>();
            c2_ack_ratio = new List<float>();
            mvg_base_HDOP = new List<byte>();
            est_pos_mvb_NE_ft1 = new List<double>();
            est_pos_mvb_NE_ft2 = new List<double>();
            delta_X_ft = new List<double>();
            delta_Y_ft = new List<double>();
            accum_mvb_upd = new List<int>();
            est_vel_mvb_NE_ftps1 = new List<double>();

            est_vel_mvb_NE_ftps2 = new List<double>();
            mvb_en = new List<sbyte>();
            meas_pos_mvb_NE_ft1 = new List<float>();
            meas_pos_mvb_NE_ft2 = new List<float>();
            meas_vel_mvb_NE_mps1 = new List<float>();
            meas_vel_mvb_NE_mps2 = new List<float>();
            fp_upload_cnt = new List<byte>();
            fltplan_num_wpt1 = new List<short>();
            fltplan_num_wpt2 = new List<short>();
            fltplan_num_wpt3 = new List<short>();

            fltplan_num_wpt4 = new List<short>();
            fltplan_num_wpt5 = new List<short>();
            fltplan_num_wpt6 = new List<short>();
            fltplan_num_wpt7 = new List<short>();
            current_fp_changed = new List<sbyte>();
            next_waypoint = new List<short>();
            next_wpt_updated = new List<sbyte>();
            packet20_freq_hz = new List<byte>();
            xbus_freq_hz = new List<byte>();
            lost_comm_fault_HV = new List<sbyte>();

            lost_comm_fault_HS = new List<sbyte>();
            stick_input_throttle_stick_lsb = new List<sbyte>();
            stick_input_aileron_stick_lsb = new List<sbyte>();
            stick_input_elevator_stick_lsb = new List<sbyte>();
            stick_input_rudder_stick_lsb = new List<sbyte>();
            stick_input_aux_A_stick_lsb = new List<sbyte>();
            stick_input_aux_B_stick_lsb = new List<sbyte>();
            stick_input_engine_disable = new List<sbyte>();
            stick_input_control_mode = new List<sbyte>();
            control_mode_rcvd = new List<sbyte>();

            auto_manvr_reset = new List<sbyte>();
            loss_of_xbus_fault = new List<sbyte>();
            constant_bank_en = new List<sbyte>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            shield_command.Clear();
            c2_ack_ratio.Clear();
            mvg_base_HDOP.Clear();
            est_pos_mvb_NE_ft1.Clear();
            est_pos_mvb_NE_ft2.Clear();
            delta_X_ft.Clear();
            delta_Y_ft.Clear();
            accum_mvb_upd = new List<int>();
            est_vel_mvb_NE_ftps1.Clear();

            est_vel_mvb_NE_ftps2.Clear();
            mvb_en.Clear();
            meas_pos_mvb_NE_ft1.Clear();
            meas_pos_mvb_NE_ft2.Clear();
            meas_vel_mvb_NE_mps1.Clear();
            meas_vel_mvb_NE_mps2.Clear();
            fp_upload_cnt.Clear();
            fltplan_num_wpt1.Clear();
            fltplan_num_wpt2.Clear();
            fltplan_num_wpt3.Clear();

            fltplan_num_wpt4.Clear();
            fltplan_num_wpt5.Clear();
            fltplan_num_wpt6.Clear();
            fltplan_num_wpt7.Clear();
            current_fp_changed.Clear();
            next_waypoint.Clear();
            next_wpt_updated.Clear();
            packet20_freq_hz.Clear();
            xbus_freq_hz.Clear();
            lost_comm_fault_HV.Clear();

            lost_comm_fault_HS.Clear();
            stick_input_throttle_stick_lsb.Clear();
            stick_input_aileron_stick_lsb.Clear();
            stick_input_elevator_stick_lsb.Clear();
            stick_input_rudder_stick_lsb.Clear();
            stick_input_aux_A_stick_lsb.Clear();
            stick_input_aux_B_stick_lsb.Clear();
            stick_input_engine_disable.Clear();
            stick_input_control_mode.Clear();
            control_mode_rcvd.Clear();

            auto_manvr_reset.Clear();
            loss_of_xbus_fault.Clear();
            constant_bank_en.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int shortVal = 2;
            int byteVal = 1;

            linux_time_s.Add(BitConverter.ToDouble(input, start));
            shield_command.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            c2_ack_ratio.Add(BitConverter.ToSingle(input, start += byteVal));
            mvg_base_HDOP.Add((byte)BitConverter.ToChar(input, start += floatVal));
            est_pos_mvb_NE_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            est_pos_mvb_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            delta_X_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            delta_Y_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            accum_mvb_upd.Add(BitConverter.ToInt32(input, start += doubleVal));
            est_vel_mvb_NE_ftps1.Add(BitConverter.ToDouble(input, start += intVal));

            est_vel_mvb_NE_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            mvb_en.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            meas_pos_mvb_NE_ft1.Add(BitConverter.ToSingle(input, start += byteVal));
            meas_pos_mvb_NE_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_vel_mvb_NE_mps1.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_vel_mvb_NE_mps2.Add(BitConverter.ToSingle(input, start += floatVal));
            fp_upload_cnt.Add((byte)BitConverter.ToChar(input, start += floatVal));
            fltplan_num_wpt1.Add(BitConverter.ToInt16(input, start += byteVal));
            fltplan_num_wpt2.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt3.Add(BitConverter.ToInt16(input, start += shortVal));

            fltplan_num_wpt4.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt5.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt6.Add(BitConverter.ToInt16(input, start += shortVal));
            fltplan_num_wpt7.Add(BitConverter.ToInt16(input, start += shortVal));
            current_fp_changed.Add((sbyte)BitConverter.ToChar(input, start += shortVal));
            next_waypoint.Add(BitConverter.ToInt16(input, start += byteVal));
            next_wpt_updated.Add((sbyte)BitConverter.ToChar(input, start += shortVal));
            packet20_freq_hz.Add((byte)BitConverter.ToChar(input, start += byteVal));
            xbus_freq_hz.Add((byte)BitConverter.ToChar(input, start += byteVal));
            lost_comm_fault_HV.Add((sbyte)BitConverter.ToChar(input, start += byteVal));

            lost_comm_fault_HS.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_throttle_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aileron_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_elevator_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_rudder_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aux_A_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_aux_B_stick_lsb.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_engine_disable.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            stick_input_control_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            control_mode_rcvd.Add((sbyte)BitConverter.ToChar(input, start += byteVal));

            auto_manvr_reset.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            loss_of_xbus_fault.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            constant_bank_en.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            //Debug.WriteLine($"should be {input.Length} but is {start}");
        }


        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            shield_command.Add(Reader.ReadSByte(stream));
            c2_ack_ratio.Add(Reader.ReadFloat(stream));
            mvg_base_HDOP.Add(Reader.ReadByte(stream));
            est_pos_mvb_NE_ft1.Add(Reader.ReadDouble(stream));
            est_pos_mvb_NE_ft2.Add(Reader.ReadDouble(stream));
            delta_X_ft.Add(Reader.ReadDouble(stream));
            delta_Y_ft.Add(Reader.ReadDouble(stream));
            accum_mvb_upd.Add(Reader.ReadInt(stream));
            est_vel_mvb_NE_ftps1.Add(Reader.ReadDouble(stream));

            est_vel_mvb_NE_ftps2.Add(Reader.ReadDouble(stream));
            mvb_en.Add(Reader.ReadSByte(stream));
            meas_pos_mvb_NE_ft1.Add(Reader.ReadFloat(stream));
            meas_pos_mvb_NE_ft2.Add(Reader.ReadFloat(stream));
            meas_vel_mvb_NE_mps1.Add(Reader.ReadFloat(stream));
            meas_vel_mvb_NE_mps2.Add(Reader.ReadFloat(stream));
            fp_upload_cnt.Add(Reader.ReadByte(stream));
            fltplan_num_wpt1.Add(Reader.ReadShort(stream));
            fltplan_num_wpt2.Add(Reader.ReadShort(stream));
            fltplan_num_wpt3.Add(Reader.ReadShort(stream));

            fltplan_num_wpt4.Add(Reader.ReadShort(stream));
            fltplan_num_wpt5.Add(Reader.ReadShort(stream));
            fltplan_num_wpt6.Add(Reader.ReadShort(stream));
            fltplan_num_wpt7.Add(Reader.ReadShort(stream));
            current_fp_changed.Add(Reader.ReadSByte(stream));
            next_waypoint.Add(Reader.ReadShort(stream));
            next_wpt_updated.Add(Reader.ReadSByte(stream));
            packet20_freq_hz.Add(Reader.ReadByte(stream));
            xbus_freq_hz.Add(Reader.ReadByte(stream));
            lost_comm_fault_HV.Add(Reader.ReadSByte(stream));

            lost_comm_fault_HS.Add(Reader.ReadSByte(stream));
            stick_input_throttle_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aileron_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_elevator_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_rudder_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aux_A_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_aux_B_stick_lsb.Add(Reader.ReadSByte(stream));
            stick_input_engine_disable.Add(Reader.ReadSByte(stream));
            stick_input_control_mode.Add(Reader.ReadSByte(stream));
            control_mode_rcvd.Add(Reader.ReadSByte(stream));

            auto_manvr_reset.Add(Reader.ReadSByte(stream));
            loss_of_xbus_fault.Add(Reader.ReadSByte(stream));
            constant_bank_en.Add(Reader.ReadSByte(stream));
        }

    }
    public class GuidanceData17
    {


        public int totalLength = 146;
        public int dataLength = 25;

        public List<double> linux_time_s { get; set; }
        public List<double> alt_rate_cmd_hs_ftps { get; set; }
        public List<double> phi_cmd_hs_deg { get; set; }
        public List<double> speed_cmd_hs_knots { get; set; }
        public List<double> alt_cmd_hs_ft { get; set; }
        public List<double> cog_cmd_hs_deg { get; set; }
        public List<sbyte> increment_wpt { get; set; }
        public List<double> psi_cmd_deg { get; set; }
        public List<double> urate_ft_s { get; set; }
        public List<double> pos_cmd_NE_ft1 { get; set; }

        public List<double> pos_cmd_NE_ft2 { get; set; }
        public List<double> alt_cmd_ft { get; set; }
        public List<double> hv_wpt_err_ft { get; set; }
        public List<double> vel_cmd_NED_ftps1 { get; set; }
        public List<double> vel_cmd_NED_ftps2 { get; set; }
        public List<double> vel_cmd_NED_ftps3 { get; set; }
        public List<float> cl_hv_deg { get; set; }
        public List<byte> stick_mode { get; set; }
        public List<sbyte> flip_state { get; set; }
        public List<float> wind_est_NE1 { get; set; }

        public List<float> wind_est_NE2 { get; set; }
        public List<float> hdg_err { get; set; }
        public List<byte> mvb_land_state { get; set; }
        public List<sbyte> hover_done { get; set; }
        public List<sbyte> launch_done { get; set; }
        public List<ushort> loiter_countdown { get; set; } // added for 1.7
        public List<ushort> waypoint_eta { get; set; } // added for 1.7

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public GuidanceData17()
        {
            linux_time_s = new List<double>();
            alt_rate_cmd_hs_ftps = new List<double>();
            phi_cmd_hs_deg = new List<double>();
            speed_cmd_hs_knots = new List<double>();
            alt_cmd_hs_ft = new List<double>();
            cog_cmd_hs_deg = new List<double>();
            increment_wpt = new List<sbyte>();
            psi_cmd_deg = new List<double>();
            urate_ft_s = new List<double>();
            pos_cmd_NE_ft1 = new List<double>();

            pos_cmd_NE_ft2 = new List<double>();
            alt_cmd_ft = new List<double>();
            hv_wpt_err_ft = new List<double>();
            vel_cmd_NED_ftps1 = new List<double>();
            vel_cmd_NED_ftps2 = new List<double>();
            vel_cmd_NED_ftps3 = new List<double>();
            cl_hv_deg = new List<float>();
            stick_mode = new List<byte>();
            flip_state = new List<sbyte>();
            wind_est_NE1 = new List<float>();

            wind_est_NE2 = new List<float>();
            hdg_err = new List<float>();
            mvb_land_state = new List<byte>();
            hover_done = new List<sbyte>();
            launch_done = new List<sbyte>();
            loiter_countdown = new List<ushort>(); // added for 1.7
            waypoint_eta = new List<ushort>(); // added for 1.7
        }

        public void Clear()
        {
            linux_time_s.Clear();
            alt_rate_cmd_hs_ftps.Clear();
            phi_cmd_hs_deg.Clear();
            speed_cmd_hs_knots.Clear();
            alt_cmd_hs_ft.Clear();
            cog_cmd_hs_deg.Clear();
            increment_wpt.Clear();
            psi_cmd_deg.Clear();
            urate_ft_s.Clear();
            pos_cmd_NE_ft1.Clear();

            pos_cmd_NE_ft2.Clear();
            alt_cmd_ft.Clear();
            hv_wpt_err_ft.Clear();
            vel_cmd_NED_ftps1.Clear();
            vel_cmd_NED_ftps2.Clear();
            vel_cmd_NED_ftps3.Clear();
            cl_hv_deg.Clear();
            stick_mode.Clear();
            flip_state.Clear();
            wind_est_NE1.Clear();

            wind_est_NE2.Clear();
            hdg_err.Clear();
            mvb_land_state.Clear();
            hover_done.Clear();
            launch_done.Clear();
            loiter_countdown.Clear(); // added for 1.7
            waypoint_eta.Clear(); // added for 1.7
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int byteVal = 1;
            int shortVal = 2;

            linux_time_s.Add(BitConverter.ToDouble(input, start));
            alt_rate_cmd_hs_ftps.Add(BitConverter.ToDouble(input, start += doubleVal));
            phi_cmd_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            speed_cmd_hs_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_cmd_hs_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            cog_cmd_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            increment_wpt.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            psi_cmd_deg.Add(BitConverter.ToDouble(input, start += byteVal));
            urate_ft_s.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_cmd_NE_ft1.Add(BitConverter.ToDouble(input, start += doubleVal));

            pos_cmd_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));
            alt_cmd_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            hv_wpt_err_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps1.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps2.Add(BitConverter.ToDouble(input, start += doubleVal));
            vel_cmd_NED_ftps3.Add(BitConverter.ToDouble(input, start += doubleVal));
            cl_hv_deg.Add(BitConverter.ToSingle(input, start += doubleVal));
            stick_mode.Add((byte)BitConverter.ToChar(input, start += floatVal));
            flip_state.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            wind_est_NE1.Add(BitConverter.ToSingle(input, start += byteVal));

            wind_est_NE2.Add(BitConverter.ToSingle(input, start += floatVal));
            hdg_err.Add(BitConverter.ToSingle(input, start += floatVal));
            mvb_land_state.Add((byte)BitConverter.ToChar(input, start += floatVal));
            hover_done.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            launch_done.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            loiter_countdown.Add(BitConverter.ToUInt16(input, start += byteVal)); // added for 1.7
            waypoint_eta.Add(BitConverter.ToUInt16(input, start += shortVal)); // added for 1.7
            //waypoint_eta.Add(BitConverter.ToUInt16(input, start)); // added for 1.7
        }


        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            alt_rate_cmd_hs_ftps.Add(Reader.ReadDouble(stream));
            phi_cmd_hs_deg.Add(Reader.ReadDouble(stream));
            speed_cmd_hs_knots.Add(Reader.ReadDouble(stream));
            alt_cmd_hs_ft.Add(Reader.ReadDouble(stream));
            cog_cmd_hs_deg.Add(Reader.ReadDouble(stream));
            increment_wpt.Add(Reader.ReadSByte(stream));
            psi_cmd_deg.Add(Reader.ReadDouble(stream));
            urate_ft_s.Add(Reader.ReadDouble(stream));
            pos_cmd_NE_ft1.Add(Reader.ReadDouble(stream));

            pos_cmd_NE_ft2.Add(Reader.ReadDouble(stream));
            alt_cmd_ft.Add(Reader.ReadDouble(stream));
            hv_wpt_err_ft.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps1.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps2.Add(Reader.ReadDouble(stream));
            vel_cmd_NED_ftps3.Add(Reader.ReadDouble(stream));
            cl_hv_deg.Add(Reader.ReadFloat(stream));
            stick_mode.Add(Reader.ReadByte(stream));
            flip_state.Add(Reader.ReadSByte(stream));
            wind_est_NE1.Add(Reader.ReadFloat(stream));

            wind_est_NE2.Add(Reader.ReadFloat(stream));
            hdg_err.Add(Reader.ReadFloat(stream));
            mvb_land_state.Add(Reader.ReadByte(stream));
            hover_done.Add(Reader.ReadSByte(stream));
            launch_done.Add(Reader.ReadSByte(stream));
            loiter_countdown.Add(Reader.ReadUShort(stream)); // added for 1.7
            waypoint_eta.Add(Reader.ReadUShort(stream)); // added for 1.7
        }
    }

    public class ModeControlData17
    {
        public int totalLength = 19;
        public int dataLength = 11;

        public List<double> linux_time_s { get; set; }
        public List<byte> flight_state_tlm { get; set; }
        public List<sbyte> control_mode { get; set; }
        public List<sbyte> start_waypoint_mode { get; set; }
        public List<short> next_wpt { get; set; }
        public List<byte> flightplan { get; set; }
        public List<sbyte> wpt_changed { get; set; }
        public List<sbyte> prevent_increment { get; set; }
        public List<sbyte> auto_landing { get; set; }
        public List<byte> nav_light_power { get; set; } // added for 1.7
        public List<sbyte> rpm_low { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public ModeControlData17()
        {
            linux_time_s = new List<double>();
            flight_state_tlm = new List<byte>();
            control_mode = new List<sbyte>();
            start_waypoint_mode = new List<sbyte>();
            next_wpt = new List<short>();
            flightplan = new List<byte>();
            wpt_changed = new List<sbyte>();
            prevent_increment = new List<sbyte>();
            auto_landing = new List<sbyte>();
            nav_light_power = new List<byte>(); // added for 1.7
            rpm_low = new List<sbyte>();
        }

        public void Clear()
        {
            linux_time_s.Clear();
            flight_state_tlm.Clear();
            control_mode.Clear();
            start_waypoint_mode.Clear();
            next_wpt.Clear();
            flightplan.Clear();
            wpt_changed.Clear();
            prevent_increment.Clear();
            auto_landing.Clear();
            nav_light_power.Clear(); // added for 1.7
            rpm_low.Clear();
        }

        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int shortVal = 2;
            int byteVal = 1;
            //Debug.WriteLine(input.Length);
            linux_time_s.Add(BitConverter.ToDouble(input, start));
            flight_state_tlm.Add((byte)BitConverter.ToChar(input, start += doubleVal));
            control_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            start_waypoint_mode.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            next_wpt.Add(BitConverter.ToInt16(input, start += byteVal));
            flightplan.Add((byte)BitConverter.ToChar(input, start += shortVal));
            wpt_changed.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            prevent_increment.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            auto_landing.Add((sbyte)BitConverter.ToChar(input, start += byteVal));
            nav_light_power.Add((byte)BitConverter.ToChar(input, start += byteVal)); // added for 1.7
            rpm_low.Add((sbyte)BitConverter.ToChar(input, start += byteVal));

        }

        public void ReadBytesData(Stream stream)
        {
            linux_time_s.Add(Reader.ReadDouble(stream));
            flight_state_tlm.Add(Reader.ReadByte(stream));
            control_mode.Add(Reader.ReadSByte(stream));
            start_waypoint_mode.Add(Reader.ReadSByte(stream));
            next_wpt.Add(Reader.ReadShort(stream));
            flightplan.Add(Reader.ReadByte(stream));
            wpt_changed.Add(Reader.ReadSByte(stream));
            prevent_increment.Add(Reader.ReadSByte(stream));
            auto_landing.Add(Reader.ReadSByte(stream));
            nav_light_power.Add(Reader.ReadByte(stream));
            rpm_low.Add(Reader.ReadSByte(stream));
        }
    }

    public class SensorProcData17
    {



        public int totalLength = 650;
        public int dataLength = 131;

        public List<double> meas_w_B_ECI_B_deg_s1 { get; set; }
        public List<double> meas_w_B_ECI_B_deg_s2 { get; set; }
        public List<double> meas_w_B_ECI_B_deg_s3 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s1 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s2 { get; set; }
        public List<double> filt_w_B_ECI_B_deg_s3 { get; set; }
        public List<double> q_B_NED1 { get; set; }
        public List<double> q_B_NED2 { get; set; }
        public List<double> q_B_NED3 { get; set; }
        public List<double> q_B_NED4 { get; set; }

        public List<double> Euler321_B_NED_hs_deg_psi_hs_deg { get; set; }
        public List<double> Euler321_B_NED_hs_deg_theta_hs_deg { get; set; }
        public List<double> Euler321_B_NED_hs_deg_phi_hs_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_psi_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_phi_deg { get; set; }
        public List<double> Euler312_B_NED_hv_deg_theta_deg { get; set; }
        public List<double> filt_airspeed_knots { get; set; }
        public List<bool> airspeed_invalid { get; set; }
        public List<double> altimeter_ft { get; set; }
        public List<double> est_alt_ft { get; set; }

        public List<double> est_hdot_ft_s { get; set; }
        public List<float> AGL_bias_ft { get; set; }
        public List<double> time_unix_sec { get; set; }
        public List<sbyte> pss8_alt_invalid { get; set; } // added for 1.7
        public List<float> H_MSL_ft { get; set; }
        public List<float> QNH_Pa { get; set; }
        public List<double> Vel_B_ECF_NED_knots1 { get; set; }
        public List<double> Vel_B_ECF_NED_knots2 { get; set; }
        public List<double> Vel_B_ECF_NED_knots3 { get; set; }
        public List<double> sog_knots { get; set; }
        public List<double> cog_deg { get; set; }

        public List<double> rc_input_rc_aileron { get; set; }
        public List<double> rc_input_rc_elevator { get; set; }
        public List<double> rc_input_rc_flap { get; set; }
        public List<double> rc_input_rc_gear { get; set; }
        public List<double> rc_input_rc_rudder { get; set; }
        public List<double> rc_input_rc_throttle { get; set; }
        public List<short> rc_input_rc_man_bckup_lsb { get; set; }
        public List<bool> rc_control_en { get; set; }
        public List<double> pos_NE_ft1 { get; set; }
        public List<double> pos_NE_ft2 { get; set; }

        public List<double> alt_INS_ft { get; set; }
        public List<double> meas_lat_rad { get; set; }
        public List<double> meas_lng_rad { get; set; }
        public List<bool> gps_ready { get; set; }
        public List<double> init_lat_lon_rad1 { get; set; }
        public List<double> init_lat_lon_rad2 { get; set; }
        public List<bool> pos_initialized { get; set; }
        public List<float> Accel_NED_ft_s_s1 { get; set; }
        public List<float> Accel_NED_ft_s_s2 { get; set; }
        public List<float> Accel_NED_ft_s_s3 { get; set; }

        public List<float> est_pos_NE_ft1 { get; set; }
        public List<float> est_pos_NE_ft2 { get; set; }
        public List<float> est_vel_NE_kts1 { get; set; }
        public List<float> est_vel_NE_kts2 { get; set; }
        public List<double> linux_time_s { get; set; }
        public List<bool> INS_solution_valid { get; set; }
        public List<double> est_alt_bias_ft { get; set; }
        //public List<double> est_accel_bias_ft_s2 { get; set; } // removed for 1.7
        public List<byte> alt_limit_ctr { get; set; } // added for 1.7
        public List<float> alt_meas_residual { get; set; } // added for 1.7
        public List<double> vectornav_tlm_rawPositionLat_deg { get; set; }
        public List<double> vectornav_tlm_rawPositionLon_deg { get; set; }

        public List<double> vectornav_tlm_rawPositionAlt_m { get; set; }
        public List<float> vectornav_tlm_rawVel_N_m_s { get; set; }
        public List<float> vectornav_tlm_rawVel_E_m_s { get; set; }
        public List<float> vectornav_tlm_rawVel_D_m_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_Bx_rad_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_By_rad_s { get; set; }
        public List<float> vectornav_tlm_uncmpAngRate_Bz_rad_s { get; set; }
        public List<float> vectornav_tlm_accel_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_accel_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_accel_Bz_m_s2 { get; set; }

        public List<float> vectornav_tlm_uncmpAccel_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_uncmpAccel_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_uncmpAccel_Bz_m_s2 { get; set; }
        public List<float> vectornav_tlm_mag_Bx_G { get; set; }
        public List<float> vectornav_tlm_mag_By_G { get; set; }
        public List<float> vectornav_tlm_mag_Bz_G { get; set; }
        public List<ushort> vectornav_tlm_sensSat { get; set; }
        public List<float> vectornav_tlm_yawU_deg { get; set; }
        public List<float> vectornav_tlm_pitchU_deg { get; set; }
        public List<float> vectornav_tlm_rollU_deg { get; set; }

        public List<float> vectornav_tlm_posU_m { get; set; }
        public List<float> vectornav_tlm_velU_m_s { get; set; }
        public List<float> vectornav_tlm_GPS1GDOP { get; set; }
        public List<float> vectornav_tlm_GPS1TDOP { get; set; }
        public List<float> vectornav_tlm_GPS2GDOP { get; set; }
        public List<byte> vectornav_tlm_numGPS1Sats { get; set; }
        public List<byte> vectornav_tlm_numGPS2Sats { get; set; }
        public List<byte> vectornav_tlm_GPS1Fix { get; set; }
        public List<byte> vectornav_tlm_GPS2Fix { get; set; }
        public List<ushort> vectornav_tlm_INSStatus { get; set; }

        public List<ushort> vectornav_tlm_AHRSStatus { get; set; }
        public List<float> vectornav_tlm_temp_C { get; set; }
        public List<float> vectornav_tlm_press_kPa { get; set; }
        public List<float> vectornav_tlm_Yaw_deg { get; set; }
        public List<float> vectornav_tlm_Pitch_deg { get; set; }
        public List<float> vectornav_tlm_Roll_deg { get; set; }
        public List<float> vectornav_tlm_linAcc_Bx_m_s2 { get; set; }
        public List<float> vectornav_tlm_linAcc_By_m_s2 { get; set; }
        public List<float> vectornav_tlm_linAcc_Bz_m_s2 { get; set; }
        public List<float> prop_rpm { get; set; }

        public List<float> battery_V { get; set; }
        public List<float> fuel_percent { get; set; }
        public List<float> meas_AGL_m { get; set; }
        public List<int> slow_status_pkt_errors { get; set; }
        public List<int> slow_status_pkt_warnings { get; set; }
        public List<float> slow_status_pkt_battery_V { get; set; }
        public List<float> slow_status_pkt_fuel_percent { get; set; }
        public List<byte> slow_status_pkt_flagsA { get; set; }
        public List<byte> slow_status_pkt_power_status { get; set; }
        public List<byte> slow_status_pkt_flagsB { get; set; }

        public List<float> slow_status_pkt_battery_A { get; set; }
        public List<float> slow_status_pkt_generator_V { get; set; }
        public List<float> slow_status_pkt_generator_A { get; set; }
        public List<uint> slow_status_pkt_slow_pkt_ctr { get; set; } // added for 1.7
        public List<float> env_pkt_nose_airtemp_C { get; set; }
        public List<float> env_pkt_fuel_flow_lb_p_h { get; set; }
        public List<float> env_pkt_est_fuel_wght_lb { get; set; }
        public List<float> env_pkt_eng_cyl1_temp_C { get; set; }
        public List<float> env_pkt_eng_cyl2_temp_C { get; set; }
        public List<ushort> env_pkt_valid { get; set; }
        public List<float> pss8_pkt_impact_press_Pa { get; set; }

        public List<float> pss8_pkt_static_press_Pa { get; set; }
        public List<float> pss8_pkt_cal_airspeed_m_s { get; set; }
        public List<float> pss8_pkt_true_airspeed_m_s { get; set; }
        public List<float> pss8_pkt_press_altitude_m { get; set; }
        public List<ushort> pss8_pkt_quality { get; set; }
        public List<ushort> pss8_pkt_flags { get; set; }
        public List<float> pss8_pkt_static_temp_C { get; set; }
        public List<float> pss8_pkt_total_temp_C { get; set; }
        public List<ushort> pss8_pkt_slow_quality { get; set; }
        public List<ushort> pss8_pkt_slow_flags { get; set; }
        public List<uint> pss8_pkt_pss8_pkt_ctr { get; set; } // added for 1.7

        public List<uint> running_time_msec { get; set; }

        public Tuple<int, byte[]>[] PreProcessed { get; set; }

        public SensorProcData17()
        {
            meas_w_B_ECI_B_deg_s1 = new List<double>();
            meas_w_B_ECI_B_deg_s2 = new List<double>();
            meas_w_B_ECI_B_deg_s3 = new List<double>();
            filt_w_B_ECI_B_deg_s1 = new List<double>();
            filt_w_B_ECI_B_deg_s2 = new List<double>();
            filt_w_B_ECI_B_deg_s3 = new List<double>();
            q_B_NED1 = new List<double>();
            q_B_NED2 = new List<double>();
            q_B_NED3 = new List<double>();
            q_B_NED4 = new List<double>();

            Euler321_B_NED_hs_deg_psi_hs_deg = new List<double>();
            Euler321_B_NED_hs_deg_theta_hs_deg = new List<double>();
            Euler321_B_NED_hs_deg_phi_hs_deg = new List<double>();
            Euler312_B_NED_hv_deg_psi_deg = new List<double>();
            Euler312_B_NED_hv_deg_phi_deg = new List<double>();
            Euler312_B_NED_hv_deg_theta_deg = new List<double>();
            filt_airspeed_knots = new List<double>();
            airspeed_invalid = new List<bool>();
            altimeter_ft = new List<double>();
            est_alt_ft = new List<double>();

            est_hdot_ft_s = new List<double>();
            AGL_bias_ft = new List<float>();
            time_unix_sec = new List<double>();
            pss8_alt_invalid = new List<sbyte>(); // added for 1.7
            //H_MSL_ft = new List<float>(); // removed for 1.7
            //QNH_Pa = new List<float>(); // removed for 1.7
            Vel_B_ECF_NED_knots1 = new List<double>();
            Vel_B_ECF_NED_knots2 = new List<double>();
            Vel_B_ECF_NED_knots3 = new List<double>();
            sog_knots = new List<double>();
            cog_deg = new List<double>();

            rc_input_rc_aileron = new List<double>();
            rc_input_rc_elevator = new List<double>();
            rc_input_rc_flap = new List<double>();
            rc_input_rc_gear = new List<double>();
            rc_input_rc_rudder = new List<double>();
            rc_input_rc_throttle = new List<double>();
            rc_input_rc_man_bckup_lsb = new List<short>();
            rc_control_en = new List<bool>();
            pos_NE_ft1 = new List<double>();
            pos_NE_ft2 = new List<double>();

            alt_INS_ft = new List<double>();
            meas_lat_rad = new List<double>();
            meas_lng_rad = new List<double>();
            gps_ready = new List<bool>();
            init_lat_lon_rad1 = new List<double>();
            init_lat_lon_rad2 = new List<double>();
            pos_initialized = new List<bool>();
            Accel_NED_ft_s_s1 = new List<float>();
            Accel_NED_ft_s_s2 = new List<float>();
            Accel_NED_ft_s_s3 = new List<float>();

            est_pos_NE_ft1 = new List<float>();
            est_pos_NE_ft2 = new List<float>();
            est_vel_NE_kts1 = new List<float>();
            est_vel_NE_kts2 = new List<float>();
            linux_time_s = new List<double>();
            INS_solution_valid = new List<bool>();
            est_alt_bias_ft = new List<double>();
            //est_accel_bias_ft_s2 = new List<double>(); // remove for 1.7
            alt_limit_ctr = new List<byte>(); // added for 1.7
            alt_meas_residual = new List<float>(); // added for 1.7
            vectornav_tlm_rawPositionLat_deg = new List<double>();
            vectornav_tlm_rawPositionLon_deg = new List<double>();

            vectornav_tlm_rawPositionAlt_m = new List<double>();
            vectornav_tlm_rawVel_N_m_s = new List<float>();
            vectornav_tlm_rawVel_E_m_s = new List<float>();
            vectornav_tlm_rawVel_D_m_s = new List<float>();
            vectornav_tlm_uncmpAngRate_Bx_rad_s = new List<float>();
            vectornav_tlm_uncmpAngRate_By_rad_s = new List<float>();
            vectornav_tlm_uncmpAngRate_Bz_rad_s = new List<float>();
            vectornav_tlm_accel_Bx_m_s2 = new List<float>();
            vectornav_tlm_accel_By_m_s2 = new List<float>();
            vectornav_tlm_accel_Bz_m_s2 = new List<float>();

            vectornav_tlm_uncmpAccel_Bx_m_s2 = new List<float>();
            vectornav_tlm_uncmpAccel_By_m_s2 = new List<float>();
            vectornav_tlm_uncmpAccel_Bz_m_s2 = new List<float>();
            vectornav_tlm_mag_Bx_G = new List<float>();
            vectornav_tlm_mag_By_G = new List<float>();
            vectornav_tlm_mag_Bz_G = new List<float>();
            vectornav_tlm_sensSat = new List<ushort>();
            vectornav_tlm_yawU_deg = new List<float>();
            vectornav_tlm_pitchU_deg = new List<float>();
            vectornav_tlm_rollU_deg = new List<float>();

            vectornav_tlm_posU_m = new List<float>();
            vectornav_tlm_velU_m_s = new List<float>();
            vectornav_tlm_GPS1GDOP = new List<float>();
            vectornav_tlm_GPS1TDOP = new List<float>();
            vectornav_tlm_GPS2GDOP = new List<float>();
            vectornav_tlm_numGPS1Sats = new List<byte>();
            vectornav_tlm_numGPS2Sats = new List<byte>();
            vectornav_tlm_GPS1Fix = new List<byte>();
            vectornav_tlm_GPS2Fix = new List<byte>();
            vectornav_tlm_INSStatus = new List<ushort>();

            vectornav_tlm_AHRSStatus = new List<ushort>();
            vectornav_tlm_temp_C = new List<float>();
            vectornav_tlm_press_kPa = new List<float>();
            vectornav_tlm_Yaw_deg = new List<float>();
            vectornav_tlm_Pitch_deg = new List<float>();
            vectornav_tlm_Roll_deg = new List<float>();
            vectornav_tlm_linAcc_Bx_m_s2 = new List<float>();
            vectornav_tlm_linAcc_By_m_s2 = new List<float>();
            vectornav_tlm_linAcc_Bz_m_s2 = new List<float>();
            prop_rpm = new List<float>();

            battery_V = new List<float>();
            fuel_percent = new List<float>();
            meas_AGL_m = new List<float>();
            slow_status_pkt_errors = new List<int>();
            slow_status_pkt_warnings = new List<int>();
            slow_status_pkt_battery_V = new List<float>();
            slow_status_pkt_fuel_percent = new List<float>();
            slow_status_pkt_flagsA = new List<byte>();
            slow_status_pkt_power_status = new List<byte>();
            slow_status_pkt_flagsB = new List<byte>();

            slow_status_pkt_battery_A = new List<float>();
            slow_status_pkt_generator_V = new List<float>();
            slow_status_pkt_generator_A = new List<float>();
            slow_status_pkt_slow_pkt_ctr = new List<uint>();
            env_pkt_nose_airtemp_C = new List<float>();
            env_pkt_fuel_flow_lb_p_h = new List<float>();
            env_pkt_est_fuel_wght_lb = new List<float>();
            env_pkt_eng_cyl1_temp_C = new List<float>();
            env_pkt_eng_cyl2_temp_C = new List<float>();
            env_pkt_valid = new List<ushort>();
            pss8_pkt_impact_press_Pa = new List<float>();

            pss8_pkt_static_press_Pa = new List<float>();
            pss8_pkt_cal_airspeed_m_s = new List<float>();
            pss8_pkt_true_airspeed_m_s = new List<float>();
            pss8_pkt_press_altitude_m = new List<float>();
            pss8_pkt_quality = new List<ushort>();
            pss8_pkt_flags = new List<ushort>();
            pss8_pkt_static_temp_C = new List<float>();
            pss8_pkt_total_temp_C = new List<float>();
            pss8_pkt_slow_quality = new List<ushort>();
            pss8_pkt_slow_flags = new List<ushort>();
            pss8_pkt_pss8_pkt_ctr = new List<uint>(); // added for 1.7

            running_time_msec = new List<uint>();
        }

        public void Clear()
        {
            meas_w_B_ECI_B_deg_s1.Clear();
            meas_w_B_ECI_B_deg_s2.Clear();
            meas_w_B_ECI_B_deg_s3.Clear();
            filt_w_B_ECI_B_deg_s1.Clear();
            filt_w_B_ECI_B_deg_s2.Clear();
            filt_w_B_ECI_B_deg_s3.Clear();
            q_B_NED1.Clear();
            q_B_NED2.Clear();
            q_B_NED3.Clear();
            q_B_NED4.Clear();

            Euler321_B_NED_hs_deg_psi_hs_deg.Clear();
            Euler321_B_NED_hs_deg_theta_hs_deg.Clear();
            Euler321_B_NED_hs_deg_phi_hs_deg.Clear();
            Euler312_B_NED_hv_deg_psi_deg.Clear();
            Euler312_B_NED_hv_deg_phi_deg.Clear();
            Euler312_B_NED_hv_deg_theta_deg.Clear();
            filt_airspeed_knots.Clear();
            airspeed_invalid.Clear();
            altimeter_ft.Clear();
            est_alt_ft.Clear();

            est_hdot_ft_s.Clear();
            AGL_bias_ft.Clear();
            time_unix_sec.Clear();
            pss8_alt_invalid.Clear(); // added for 1.7
            //H_MSL_ft.Clear(); // removed for 1.7
            //QNH_Pa.Clear(); // removed for 1.7
            Vel_B_ECF_NED_knots1.Clear();
            Vel_B_ECF_NED_knots2.Clear();
            Vel_B_ECF_NED_knots3.Clear();
            sog_knots.Clear();
            cog_deg.Clear();

            rc_input_rc_aileron.Clear();
            rc_input_rc_elevator.Clear();
            rc_input_rc_flap.Clear();
            rc_input_rc_gear.Clear();
            rc_input_rc_rudder.Clear();
            rc_input_rc_throttle.Clear();
            rc_input_rc_man_bckup_lsb.Clear();
            rc_control_en.Clear();
            pos_NE_ft1.Clear();
            pos_NE_ft2.Clear();

            alt_INS_ft.Clear();
            meas_lat_rad.Clear();
            meas_lng_rad.Clear();
            gps_ready.Clear();
            init_lat_lon_rad1.Clear();
            init_lat_lon_rad2.Clear();
            pos_initialized.Clear();
            Accel_NED_ft_s_s1.Clear();
            Accel_NED_ft_s_s2.Clear();
            Accel_NED_ft_s_s3.Clear();

            est_pos_NE_ft1.Clear();
            est_pos_NE_ft2.Clear();
            est_vel_NE_kts1.Clear();
            est_vel_NE_kts2.Clear();
            linux_time_s.Clear();
            INS_solution_valid.Clear();
            est_alt_bias_ft.Clear();
            //est_accel_bias_ft_s2.Clear(); // remove for 1.7
            alt_limit_ctr.Clear(); // added for 1.7
            alt_meas_residual.Clear(); // added for 1.7
            vectornav_tlm_rawPositionLat_deg.Clear();
            vectornav_tlm_rawPositionLon_deg.Clear();

            vectornav_tlm_rawPositionAlt_m.Clear();
            vectornav_tlm_rawVel_N_m_s.Clear();
            vectornav_tlm_rawVel_E_m_s.Clear();
            vectornav_tlm_rawVel_D_m_s.Clear();
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Clear();
            vectornav_tlm_uncmpAngRate_By_rad_s.Clear();
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Clear();
            vectornav_tlm_accel_Bx_m_s2.Clear();
            vectornav_tlm_accel_By_m_s2.Clear();
            vectornav_tlm_accel_Bz_m_s2.Clear();

            vectornav_tlm_uncmpAccel_Bx_m_s2.Clear();
            vectornav_tlm_uncmpAccel_By_m_s2.Clear();
            vectornav_tlm_uncmpAccel_Bz_m_s2.Clear();
            vectornav_tlm_mag_Bx_G.Clear();
            vectornav_tlm_mag_By_G.Clear();
            vectornav_tlm_mag_Bz_G.Clear();
            vectornav_tlm_sensSat.Clear();
            vectornav_tlm_yawU_deg.Clear();
            vectornav_tlm_pitchU_deg.Clear();
            vectornav_tlm_rollU_deg.Clear();

            vectornav_tlm_posU_m.Clear();
            vectornav_tlm_velU_m_s.Clear();
            vectornav_tlm_GPS1GDOP.Clear();
            vectornav_tlm_GPS1TDOP.Clear();
            vectornav_tlm_GPS2GDOP.Clear();
            vectornav_tlm_numGPS1Sats.Clear();
            vectornav_tlm_numGPS2Sats.Clear();
            vectornav_tlm_GPS1Fix.Clear();
            vectornav_tlm_GPS2Fix.Clear();
            vectornav_tlm_INSStatus.Clear();

            vectornav_tlm_AHRSStatus.Clear();
            vectornav_tlm_temp_C.Clear();
            vectornav_tlm_press_kPa.Clear();
            vectornav_tlm_Yaw_deg.Clear();
            vectornav_tlm_Pitch_deg.Clear();
            vectornav_tlm_Roll_deg.Clear();
            vectornav_tlm_linAcc_Bx_m_s2.Clear();
            vectornav_tlm_linAcc_By_m_s2.Clear();
            vectornav_tlm_linAcc_Bz_m_s2.Clear();
            prop_rpm.Clear();

            battery_V.Clear();
            fuel_percent.Clear();
            meas_AGL_m.Clear();
            slow_status_pkt_errors.Clear();
            slow_status_pkt_warnings.Clear();
            slow_status_pkt_battery_V.Clear();
            slow_status_pkt_fuel_percent.Clear();
            slow_status_pkt_flagsA.Clear();
            slow_status_pkt_power_status.Clear();
            slow_status_pkt_flagsB.Clear();

            slow_status_pkt_battery_A.Clear();
            slow_status_pkt_generator_V.Clear();
            slow_status_pkt_generator_A.Clear();
            slow_status_pkt_slow_pkt_ctr.Clear();
            env_pkt_nose_airtemp_C.Clear();
            env_pkt_fuel_flow_lb_p_h.Clear();
            env_pkt_est_fuel_wght_lb.Clear();
            env_pkt_eng_cyl1_temp_C.Clear();
            env_pkt_eng_cyl2_temp_C.Clear();
            env_pkt_valid.Clear();
            pss8_pkt_impact_press_Pa.Clear();

            pss8_pkt_static_press_Pa.Clear();
            pss8_pkt_cal_airspeed_m_s.Clear();
            pss8_pkt_true_airspeed_m_s.Clear();
            pss8_pkt_press_altitude_m.Clear();
            pss8_pkt_quality.Clear();
            pss8_pkt_flags.Clear();
            pss8_pkt_static_temp_C.Clear();
            pss8_pkt_total_temp_C.Clear();
            pss8_pkt_slow_quality.Clear();
            pss8_pkt_slow_flags.Clear();
            pss8_pkt_pss8_pkt_ctr.Clear(); // added for 1.7

            running_time_msec.Clear();
        }
        public void ReadLogLine(byte[] input)
        {
            int start = 0;
            int doubleVal = 8;
            int floatVal = 4;
            int intVal = 4;
            int shortVal = 2;
            int byteVal = 1;
            //Debug.WriteLine(input.Length.ToString());
            meas_w_B_ECI_B_deg_s1.Add(BitConverter.ToDouble(input, start));
            meas_w_B_ECI_B_deg_s2.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_w_B_ECI_B_deg_s3.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s1.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s2.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_w_B_ECI_B_deg_s3.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED1.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED2.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED3.Add(BitConverter.ToDouble(input, start += doubleVal));
            q_B_NED4.Add(BitConverter.ToDouble(input, start += doubleVal));

            Euler321_B_NED_hs_deg_psi_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler321_B_NED_hs_deg_theta_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler321_B_NED_hs_deg_phi_hs_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_psi_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_phi_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            Euler312_B_NED_hv_deg_theta_deg.Add(BitConverter.ToDouble(input, start += doubleVal));
            filt_airspeed_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            airspeed_invalid.Add(BitConverter.ToBoolean(input, start += doubleVal));
            altimeter_ft.Add(BitConverter.ToDouble(input, start += byteVal));
            est_alt_ft.Add(BitConverter.ToDouble(input, start += doubleVal));

            est_hdot_ft_s.Add(BitConverter.ToDouble(input, start += doubleVal));
            AGL_bias_ft.Add(BitConverter.ToSingle(input, start += doubleVal));
            time_unix_sec.Add(BitConverter.ToDouble(input, start += floatVal));
            pss8_alt_invalid.Add((sbyte)BitConverter.ToChar(input, start += doubleVal));
            //H_MSL_ft.Add(BitConverter.ToSingle(input, start += byteVal)); // remove for 1.7
            //QNH_Pa.Add(BitConverter.ToSingle(input, start += floatVal)); // remove for 1.7
            Vel_B_ECF_NED_knots1.Add(BitConverter.ToDouble(input, start += byteVal));
            Vel_B_ECF_NED_knots2.Add(BitConverter.ToDouble(input, start += doubleVal));
            Vel_B_ECF_NED_knots3.Add(BitConverter.ToDouble(input, start += doubleVal));
            sog_knots.Add(BitConverter.ToDouble(input, start += doubleVal));
            cog_deg.Add(BitConverter.ToDouble(input, start += doubleVal));

            rc_input_rc_aileron.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_elevator.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_flap.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_gear.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_rudder.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_throttle.Add(BitConverter.ToDouble(input, start += doubleVal));
            rc_input_rc_man_bckup_lsb.Add(BitConverter.ToInt16(input, start += doubleVal));
            rc_control_en.Add(BitConverter.ToBoolean(input, start += shortVal));
            pos_NE_ft1.Add(BitConverter.ToDouble(input, start += byteVal));
            pos_NE_ft2.Add(BitConverter.ToDouble(input, start += doubleVal));

            alt_INS_ft.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_lat_rad.Add(BitConverter.ToDouble(input, start += doubleVal));
            meas_lng_rad.Add(BitConverter.ToDouble(input, start += doubleVal));
            gps_ready.Add(BitConverter.ToBoolean(input, start += doubleVal));
            init_lat_lon_rad1.Add(BitConverter.ToDouble(input, start += byteVal));
            init_lat_lon_rad2.Add(BitConverter.ToDouble(input, start += doubleVal));
            pos_initialized.Add(BitConverter.ToBoolean(input, start += doubleVal));
            Accel_NED_ft_s_s1.Add(BitConverter.ToSingle(input, start += byteVal));
            Accel_NED_ft_s_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            Accel_NED_ft_s_s3.Add(BitConverter.ToSingle(input, start += floatVal));

            est_pos_NE_ft1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_pos_NE_ft2.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_NE_kts1.Add(BitConverter.ToSingle(input, start += floatVal));
            est_vel_NE_kts2.Add(BitConverter.ToSingle(input, start += floatVal));
            linux_time_s.Add(BitConverter.ToDouble(input, start += floatVal));
            INS_solution_valid.Add(BitConverter.ToBoolean(input, start += doubleVal));
            est_alt_bias_ft.Add(BitConverter.ToDouble(input, start += byteVal));
            //est_accel_bias_ft_s2.Add(BitConverter.ToDouble(input, start += doubleVal)); remove for 1.7
            alt_limit_ctr.Add((byte)BitConverter.ToChar(input, start += doubleVal)); // added for 1.7
            alt_meas_residual.Add(BitConverter.ToSingle(input, start += byteVal)); // added for 1.7
            vectornav_tlm_rawPositionLat_deg.Add(BitConverter.ToDouble(input, start += floatVal));
            vectornav_tlm_rawPositionLon_deg.Add(BitConverter.ToDouble(input, start += doubleVal));

            vectornav_tlm_rawPositionAlt_m.Add(BitConverter.ToDouble(input, start += doubleVal));
            vectornav_tlm_rawVel_N_m_s.Add(BitConverter.ToSingle(input, start += doubleVal));
            vectornav_tlm_rawVel_E_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_rawVel_D_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_By_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_accel_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));

            vectornav_tlm_uncmpAccel_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAccel_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_uncmpAccel_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_Bx_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_By_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_mag_Bz_G.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_sensSat.Add(BitConverter.ToUInt16(input, start += floatVal));
            vectornav_tlm_yawU_deg.Add(BitConverter.ToSingle(input, start += shortVal));
            vectornav_tlm_pitchU_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_rollU_deg.Add(BitConverter.ToSingle(input, start += floatVal));

            vectornav_tlm_posU_m.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_velU_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS1GDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS1TDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_GPS2GDOP.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_numGPS1Sats.Add((byte)BitConverter.ToChar(input, start += floatVal));
            vectornav_tlm_numGPS2Sats.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_GPS1Fix.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_GPS2Fix.Add((byte)BitConverter.ToChar(input, start += byteVal));
            vectornav_tlm_INSStatus.Add(BitConverter.ToUInt16(input, start += byteVal));

            vectornav_tlm_AHRSStatus.Add(BitConverter.ToUInt16(input, start += shortVal));
            vectornav_tlm_temp_C.Add(BitConverter.ToSingle(input, start += shortVal));
            vectornav_tlm_press_kPa.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Yaw_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Pitch_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_Roll_deg.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_Bx_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_By_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            vectornav_tlm_linAcc_Bz_m_s2.Add(BitConverter.ToSingle(input, start += floatVal));
            prop_rpm.Add(BitConverter.ToSingle(input, start += floatVal));

            battery_V.Add(BitConverter.ToSingle(input, start += floatVal));
            fuel_percent.Add(BitConverter.ToSingle(input, start += floatVal));
            meas_AGL_m.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_errors.Add(BitConverter.ToInt32(input, start += floatVal));
            slow_status_pkt_warnings.Add(BitConverter.ToInt32(input, start += intVal));
            slow_status_pkt_battery_V.Add(BitConverter.ToSingle(input, start += intVal));
            slow_status_pkt_fuel_percent.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_flagsA.Add((byte)BitConverter.ToChar(input, start += floatVal));
            slow_status_pkt_power_status.Add((byte)BitConverter.ToChar(input, start += byteVal));
            slow_status_pkt_flagsB.Add((byte)BitConverter.ToChar(input, start += byteVal));

            slow_status_pkt_battery_A.Add(BitConverter.ToSingle(input, start += byteVal));
            slow_status_pkt_generator_V.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_generator_A.Add(BitConverter.ToSingle(input, start += floatVal));
            slow_status_pkt_slow_pkt_ctr.Add(BitConverter.ToUInt32(input, start += floatVal)); // added for 1.7
            env_pkt_nose_airtemp_C.Add(BitConverter.ToSingle(input, start += intVal));
            env_pkt_fuel_flow_lb_p_h.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_est_fuel_wght_lb.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_eng_cyl1_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_eng_cyl2_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            env_pkt_valid.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_impact_press_Pa.Add(BitConverter.ToSingle(input, start += shortVal));

            pss8_pkt_static_press_Pa.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_cal_airspeed_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_true_airspeed_m_s.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_press_altitude_m.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_quality.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_flags.Add(BitConverter.ToUInt16(input, start += shortVal));
            pss8_pkt_static_temp_C.Add(BitConverter.ToSingle(input, start += shortVal));
            pss8_pkt_total_temp_C.Add(BitConverter.ToSingle(input, start += floatVal));
            pss8_pkt_slow_quality.Add(BitConverter.ToUInt16(input, start += floatVal));
            pss8_pkt_slow_flags.Add(BitConverter.ToUInt16(input, start += shortVal));
            pss8_pkt_pss8_pkt_ctr.Add(BitConverter.ToUInt32(input, start += shortVal)); //  added for 1.7
            //Debug.WriteLine(start);
            running_time_msec.Add(BitConverter.ToUInt32(input, start += intVal));
            //Debug.WriteLine("Filled  Class");
        }
        public void ReadBytesData(Stream stream)
        {
            meas_w_B_ECI_B_deg_s1.Add(Reader.ReadDouble(stream));
            meas_w_B_ECI_B_deg_s2.Add(Reader.ReadDouble(stream));
            meas_w_B_ECI_B_deg_s3.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s1.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s2.Add(Reader.ReadDouble(stream));
            filt_w_B_ECI_B_deg_s3.Add(Reader.ReadDouble(stream));
            q_B_NED1.Add(Reader.ReadDouble(stream));
            q_B_NED2.Add(Reader.ReadDouble(stream));
            q_B_NED3.Add(Reader.ReadDouble(stream));
            q_B_NED4.Add(Reader.ReadDouble(stream));

            Euler321_B_NED_hs_deg_psi_hs_deg.Add(Reader.ReadDouble(stream));
            Euler321_B_NED_hs_deg_theta_hs_deg.Add(Reader.ReadDouble(stream));
            Euler321_B_NED_hs_deg_phi_hs_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_psi_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_phi_deg.Add(Reader.ReadDouble(stream));
            Euler312_B_NED_hv_deg_theta_deg.Add(Reader.ReadDouble(stream));
            filt_airspeed_knots.Add(Reader.ReadDouble(stream));
            airspeed_invalid.Add(Reader.ReadBool(stream));
            altimeter_ft.Add(Reader.ReadDouble(stream));
            est_alt_ft.Add(Reader.ReadDouble(stream));

            est_hdot_ft_s.Add(Reader.ReadDouble(stream));
            AGL_bias_ft.Add(Reader.ReadFloat(stream));
            time_unix_sec.Add(Reader.ReadDouble(stream));
            pss8_alt_invalid.Add(Reader.ReadSByte(stream));
            // H_MSL_ft.Add(Reader.ReadFloat(stream));  // removed for 1.7
            //QNH_Pa.Add(Reader.ReadFloat(stream)); // removed for 1.7
            Vel_B_ECF_NED_knots1.Add(Reader.ReadDouble(stream));
            Vel_B_ECF_NED_knots2.Add(Reader.ReadDouble(stream));
            Vel_B_ECF_NED_knots3.Add(Reader.ReadDouble(stream));
            sog_knots.Add(Reader.ReadDouble(stream));
            cog_deg.Add(Reader.ReadDouble(stream));

            rc_input_rc_aileron.Add(Reader.ReadDouble(stream));
            rc_input_rc_elevator.Add(Reader.ReadDouble(stream));
            rc_input_rc_flap.Add(Reader.ReadDouble(stream));
            rc_input_rc_gear.Add(Reader.ReadDouble(stream));
            rc_input_rc_rudder.Add(Reader.ReadDouble(stream));
            rc_input_rc_throttle.Add(Reader.ReadDouble(stream));
            rc_input_rc_man_bckup_lsb.Add(Reader.ReadShort(stream));
            rc_control_en.Add(Reader.ReadBool(stream));
            pos_NE_ft1.Add(Reader.ReadDouble(stream));
            pos_NE_ft2.Add(Reader.ReadDouble(stream));

            alt_INS_ft.Add(Reader.ReadDouble(stream));
            meas_lat_rad.Add(Reader.ReadDouble(stream));
            meas_lng_rad.Add(Reader.ReadDouble(stream));
            gps_ready.Add(Reader.ReadBool(stream));
            init_lat_lon_rad1.Add(Reader.ReadDouble(stream));
            init_lat_lon_rad2.Add(Reader.ReadDouble(stream));
            pos_initialized.Add(Reader.ReadBool(stream));
            Accel_NED_ft_s_s1.Add(Reader.ReadFloat(stream));
            Accel_NED_ft_s_s2.Add(Reader.ReadFloat(stream));
            Accel_NED_ft_s_s3.Add(Reader.ReadFloat(stream));

            est_pos_NE_ft1.Add(Reader.ReadFloat(stream));
            est_pos_NE_ft2.Add(Reader.ReadFloat(stream));
            est_vel_NE_kts1.Add(Reader.ReadFloat(stream));
            est_vel_NE_kts2.Add(Reader.ReadFloat(stream));
            linux_time_s.Add(Reader.ReadDouble(stream));
            INS_solution_valid.Add(Reader.ReadBool(stream));
            est_alt_bias_ft.Add(Reader.ReadDouble(stream));
            //est_accel_bias_ft_s2.Add(Reader.ReadDouble(stream));  // removed for 1.7
            alt_limit_ctr.Add(Reader.ReadByte(stream)); ; // added for 1.7
            alt_meas_residual.Add(Reader.ReadFloat(stream)); ; // added for 1.7
            vectornav_tlm_rawPositionLat_deg.Add(Reader.ReadDouble(stream));
            vectornav_tlm_rawPositionLon_deg.Add(Reader.ReadDouble(stream));

            vectornav_tlm_rawPositionAlt_m.Add(Reader.ReadDouble(stream));
            vectornav_tlm_rawVel_N_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rawVel_E_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rawVel_D_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_Bx_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_By_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAngRate_Bz_rad_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_accel_Bz_m_s2.Add(Reader.ReadFloat(stream));

            vectornav_tlm_uncmpAccel_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAccel_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_uncmpAccel_Bz_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_Bx_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_By_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_mag_Bz_G.Add(Reader.ReadFloat(stream));
            vectornav_tlm_sensSat.Add(Reader.ReadUShort(stream));
            vectornav_tlm_yawU_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_pitchU_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_rollU_deg.Add(Reader.ReadFloat(stream));

            vectornav_tlm_posU_m.Add(Reader.ReadFloat(stream));
            vectornav_tlm_velU_m_s.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS1GDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS1TDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_GPS2GDOP.Add(Reader.ReadFloat(stream));
            vectornav_tlm_numGPS1Sats.Add(Reader.ReadByte(stream));
            vectornav_tlm_numGPS2Sats.Add(Reader.ReadByte(stream));
            vectornav_tlm_GPS1Fix.Add(Reader.ReadByte(stream));
            vectornav_tlm_GPS2Fix.Add(Reader.ReadByte(stream));
            vectornav_tlm_INSStatus.Add(Reader.ReadUShort(stream));

            vectornav_tlm_AHRSStatus.Add(Reader.ReadUShort(stream));
            vectornav_tlm_temp_C.Add(Reader.ReadFloat(stream));
            vectornav_tlm_press_kPa.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Yaw_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Pitch_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_Roll_deg.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_Bx_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_By_m_s2.Add(Reader.ReadFloat(stream));
            vectornav_tlm_linAcc_Bz_m_s2.Add(Reader.ReadFloat(stream));
            prop_rpm.Add(Reader.ReadFloat(stream));

            battery_V.Add(Reader.ReadFloat(stream));
            fuel_percent.Add(Reader.ReadFloat(stream));
            meas_AGL_m.Add(Reader.ReadFloat(stream));
            slow_status_pkt_errors.Add(Reader.ReadInt(stream));
            slow_status_pkt_warnings.Add(Reader.ReadInt(stream));
            slow_status_pkt_battery_V.Add(Reader.ReadFloat(stream));
            slow_status_pkt_fuel_percent.Add(Reader.ReadFloat(stream));
            slow_status_pkt_flagsA.Add(Reader.ReadByte(stream));
            slow_status_pkt_power_status.Add(Reader.ReadByte(stream));
            slow_status_pkt_flagsB.Add(Reader.ReadByte(stream));

            slow_status_pkt_battery_A.Add(Reader.ReadFloat(stream));
            slow_status_pkt_generator_V.Add(Reader.ReadFloat(stream));
            slow_status_pkt_generator_A.Add(Reader.ReadFloat(stream));
            slow_status_pkt_slow_pkt_ctr.Add(Reader.ReadUInt(stream)); // added for 1.7
            env_pkt_nose_airtemp_C.Add(Reader.ReadFloat(stream));
            env_pkt_fuel_flow_lb_p_h.Add(Reader.ReadFloat(stream));
            env_pkt_est_fuel_wght_lb.Add(Reader.ReadFloat(stream));
            env_pkt_eng_cyl1_temp_C.Add(Reader.ReadFloat(stream));
            env_pkt_eng_cyl2_temp_C.Add(Reader.ReadFloat(stream));
            env_pkt_valid.Add(Reader.ReadUShort(stream));
            pss8_pkt_impact_press_Pa.Add(Reader.ReadFloat(stream));

            pss8_pkt_static_press_Pa.Add(Reader.ReadFloat(stream));
            pss8_pkt_cal_airspeed_m_s.Add(Reader.ReadFloat(stream));
            pss8_pkt_true_airspeed_m_s.Add(Reader.ReadFloat(stream));
            pss8_pkt_press_altitude_m.Add(Reader.ReadFloat(stream));
            pss8_pkt_quality.Add(Reader.ReadUShort(stream));
            pss8_pkt_flags.Add(Reader.ReadUShort(stream));
            pss8_pkt_static_temp_C.Add(Reader.ReadFloat(stream));
            pss8_pkt_total_temp_C.Add(Reader.ReadFloat(stream));
            pss8_pkt_slow_quality.Add(Reader.ReadUShort(stream));
            pss8_pkt_slow_flags.Add(Reader.ReadUShort(stream));
            pss8_pkt_pss8_pkt_ctr.Add(Reader.ReadUInt(stream)); // added for 1.7

            running_time_msec.Add(Reader.ReadUInt(stream));
        }
    }
}


