namespace FlightPlotter
{

    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pbLoadTlv = new System.Windows.Forms.ProgressBar();
            this.lbModeControl = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lbSnsr = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbNavd = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbControl = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbGcsData = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbGuid = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtChartTitle = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tlpSeriesConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblPct = new System.Windows.Forms.Label();
            this.btnLoadReportConfig = new System.Windows.Forms.Button();
            this.btnSaveReportConfig = new System.Windows.Forms.Button();
            this.txtReportConfig = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFromLine = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtToLine = new System.Windows.Forms.TextBox();
            this.cbLogPct = new System.Windows.Forms.CheckBox();
            this.cbZoomableY = new System.Windows.Forms.CheckBox();
            this.cbLinkXZoom = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbSizeY = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbSizeX = new System.Windows.Forms.TextBox();
            this.cbbLogType = new System.Windows.Forms.ComboBox();
            this.cbSummary = new System.Windows.Forms.CheckBox();
            this.cbGPSTrack = new System.Windows.Forms.CheckBox();
            this.btnClearData = new System.Windows.Forms.Button();
            this.cbMouseDP = new System.Windows.Forms.CheckBox();
            this.cbWhiteBG = new System.Windows.Forms.CheckBox();
            this.cbSeriesLog = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tlpSeriesConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.Location = new System.Drawing.Point(248, 20);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "Get TLV";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(411, 24);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(934, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver);
            // 
            // pbLoadTlv
            // 
            this.pbLoadTlv.BackColor = System.Drawing.SystemColors.Control;
            this.pbLoadTlv.ForeColor = System.Drawing.Color.Lime;
            this.pbLoadTlv.Location = new System.Drawing.Point(411, 56);
            this.pbLoadTlv.Margin = new System.Windows.Forms.Padding(6);
            this.pbLoadTlv.Maximum = 90;
            this.pbLoadTlv.Name = "pbLoadTlv";
            this.pbLoadTlv.Size = new System.Drawing.Size(937, 6);
            this.pbLoadTlv.TabIndex = 2;
            // 
            // lbModeControl
            // 
            this.lbModeControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbModeControl.FormattingEnabled = true;
            this.lbModeControl.ItemHeight = 24;
            this.lbModeControl.Location = new System.Drawing.Point(11, 36);
            this.lbModeControl.Margin = new System.Windows.Forms.Padding(6);
            this.lbModeControl.Name = "lbModeControl";
            this.lbModeControl.Size = new System.Drawing.Size(296, 220);
            this.lbModeControl.TabIndex = 3;
            this.lbModeControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 74);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(2046, 332);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fligh Log Parameters";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lbSnsr);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(1624, 46);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox7.Size = new System.Drawing.Size(411, 262);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Sensor Data";
            // 
            // lbSnsr
            // 
            this.lbSnsr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSnsr.FormattingEnabled = true;
            this.lbSnsr.ItemHeight = 24;
            this.lbSnsr.Location = new System.Drawing.Point(0, 36);
            this.lbSnsr.Margin = new System.Windows.Forms.Padding(6);
            this.lbSnsr.Name = "lbSnsr";
            this.lbSnsr.Size = new System.Drawing.Size(396, 220);
            this.lbSnsr.TabIndex = 3;
            this.lbSnsr.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lbNavd);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(1302, 46);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox6.Size = new System.Drawing.Size(312, 262);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Navigation Data";
            // 
            // lbNavd
            // 
            this.lbNavd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNavd.FormattingEnabled = true;
            this.lbNavd.ItemHeight = 24;
            this.lbNavd.Location = new System.Drawing.Point(0, 36);
            this.lbNavd.Margin = new System.Windows.Forms.Padding(6);
            this.lbNavd.Name = "lbNavd";
            this.lbNavd.Size = new System.Drawing.Size(296, 220);
            this.lbNavd.TabIndex = 3;
            this.lbNavd.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbControl);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(979, 46);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox5.Size = new System.Drawing.Size(312, 262);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Control Data";
            // 
            // lbControl
            // 
            this.lbControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbControl.FormattingEnabled = true;
            this.lbControl.ItemHeight = 24;
            this.lbControl.Location = new System.Drawing.Point(0, 36);
            this.lbControl.Margin = new System.Windows.Forms.Padding(6);
            this.lbControl.Name = "lbControl";
            this.lbControl.Size = new System.Drawing.Size(296, 220);
            this.lbControl.TabIndex = 3;
            this.lbControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbGcsData);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(334, 46);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox3.Size = new System.Drawing.Size(312, 262);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GCS Data";
            // 
            // lbGcsData
            // 
            this.lbGcsData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGcsData.FormattingEnabled = true;
            this.lbGcsData.ItemHeight = 24;
            this.lbGcsData.Location = new System.Drawing.Point(0, 36);
            this.lbGcsData.Margin = new System.Windows.Forms.Padding(6);
            this.lbGcsData.Name = "lbGcsData";
            this.lbGcsData.Size = new System.Drawing.Size(296, 220);
            this.lbGcsData.TabIndex = 3;
            this.lbGcsData.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbGuid);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(656, 46);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox4.Size = new System.Drawing.Size(312, 262);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Guidance Data";
            // 
            // lbGuid
            // 
            this.lbGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGuid.FormattingEnabled = true;
            this.lbGuid.ItemHeight = 24;
            this.lbGuid.Location = new System.Drawing.Point(0, 36);
            this.lbGuid.Margin = new System.Windows.Forms.Padding(6);
            this.lbGuid.Name = "lbGuid";
            this.lbGuid.Size = new System.Drawing.Size(296, 220);
            this.lbGuid.TabIndex = 3;
            this.lbGuid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbModeControl);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(11, 46);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(312, 262);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode Control Data";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 544);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(152, 42);
            this.button2.TabIndex = 6;
            this.button2.Text = "Plot";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtChartTitle
            // 
            this.txtChartTitle.Location = new System.Drawing.Point(22, 418);
            this.txtChartTitle.Margin = new System.Windows.Forms.Padding(6);
            this.txtChartTitle.Name = "txtChartTitle";
            this.txtChartTitle.Size = new System.Drawing.Size(591, 29);
            this.txtChartTitle.TabIndex = 7;
            this.txtChartTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChartTitle.Enter += new System.EventHandler(this.txtChartTitle_Enter);
            this.txtChartTitle.Leave += new System.EventHandler(this.txtChartTitle_Leave);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tlpSeriesConfiguration);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(22, 598);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2044, 962);
            this.panel1.TabIndex = 9;
            // 
            // tlpSeriesConfiguration
            // 
            this.tlpSeriesConfiguration.AutoSize = true;
            this.tlpSeriesConfiguration.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlpSeriesConfiguration.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpSeriesConfiguration.ColumnCount = 10;
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 337F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 271F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 227F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 282F));
            this.tlpSeriesConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 177F));
            this.tlpSeriesConfiguration.Controls.Add(this.label12, 8, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label2, 0, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label9, 9, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label8, 7, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label7, 6, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label6, 5, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label4, 3, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label3, 2, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label5, 4, 0);
            this.tlpSeriesConfiguration.Controls.Add(this.label11, 1, 0);
            this.tlpSeriesConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpSeriesConfiguration.Location = new System.Drawing.Point(0, 0);
            this.tlpSeriesConfiguration.Margin = new System.Windows.Forms.Padding(6);
            this.tlpSeriesConfiguration.Name = "tlpSeriesConfiguration";
            this.tlpSeriesConfiguration.RowCount = 1;
            this.tlpSeriesConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tlpSeriesConfiguration.Size = new System.Drawing.Size(2042, 85);
            this.tlpSeriesConfiguration.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1594, 1);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(270, 83);
            this.label12.TabIndex = 9;
            this.label12.Text = "Group Name";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(325, 83);
            this.label2.TabIndex = 0;
            this.label2.Text = "Telemetry";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1879, 4);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 76);
            this.label9.TabIndex = 7;
            this.label9.Text = "Chart #";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1437, 1);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 83);
            this.label8.TabIndex = 6;
            this.label8.Text = "Freq";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1251, 1);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 83);
            this.label7.TabIndex = 5;
            this.label7.Text = "Thresh";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1111, 1);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 83);
            this.label6.TabIndex = 4;
            this.label6.Text = "Symbol";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(845, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 83);
            this.label4.TabIndex = 2;
            this.label4.Text = "Alt Y";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(617, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 83);
            this.label3.TabIndex = 1;
            this.label3.Text = "XForm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(938, 1);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 83);
            this.label5.TabIndex = 3;
            this.label5.Text = "Annotation ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(345, 1);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(259, 83);
            this.label11.TabIndex = 8;
            this.label11.Text = "Alias";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPct
            // 
            this.lblPct.AutoSize = true;
            this.lblPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct.Location = new System.Drawing.Point(1359, 26);
            this.lblPct.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPct.Name = "lblPct";
            this.lblPct.Size = new System.Drawing.Size(0, 32);
            this.lblPct.TabIndex = 10;
            // 
            // btnLoadReportConfig
            // 
            this.btnLoadReportConfig.BackColor = System.Drawing.Color.Gainsboro;
            this.btnLoadReportConfig.Location = new System.Drawing.Point(1916, 408);
            this.btnLoadReportConfig.Margin = new System.Windows.Forms.Padding(6);
            this.btnLoadReportConfig.Name = "btnLoadReportConfig";
            this.btnLoadReportConfig.Size = new System.Drawing.Size(152, 48);
            this.btnLoadReportConfig.TabIndex = 11;
            this.btnLoadReportConfig.Text = "Load";
            this.btnLoadReportConfig.UseVisualStyleBackColor = false;
            this.btnLoadReportConfig.Click += new System.EventHandler(this.btnLoadReportConfig_Click);
            // 
            // btnSaveReportConfig
            // 
            this.btnSaveReportConfig.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSaveReportConfig.Location = new System.Drawing.Point(1916, 492);
            this.btnSaveReportConfig.Margin = new System.Windows.Forms.Padding(6);
            this.btnSaveReportConfig.Name = "btnSaveReportConfig";
            this.btnSaveReportConfig.Size = new System.Drawing.Size(152, 48);
            this.btnSaveReportConfig.TabIndex = 12;
            this.btnSaveReportConfig.Text = "Save";
            this.btnSaveReportConfig.UseVisualStyleBackColor = false;
            this.btnSaveReportConfig.Visible = false;
            this.btnSaveReportConfig.Click += new System.EventHandler(this.btnSaveReportConfig_Click);
            // 
            // txtReportConfig
            // 
            this.txtReportConfig.Location = new System.Drawing.Point(1315, 418);
            this.txtReportConfig.Margin = new System.Windows.Forms.Padding(6);
            this.txtReportConfig.Name = "txtReportConfig";
            this.txtReportConfig.Size = new System.Drawing.Size(591, 29);
            this.txtReportConfig.TabIndex = 13;
            this.txtReportConfig.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtReportConfig.Enter += new System.EventHandler(this.txtReportConfig_Enter);
            this.txtReportConfig.Leave += new System.EventHandler(this.txtReportConfig_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 456);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 29);
            this.label1.TabIndex = 14;
            this.label1.Text = "X Slice:";
            // 
            // txtFromLine
            // 
            this.txtFromLine.Location = new System.Drawing.Point(128, 460);
            this.txtFromLine.Margin = new System.Windows.Forms.Padding(6);
            this.txtFromLine.Name = "txtFromLine";
            this.txtFromLine.Size = new System.Drawing.Size(72, 29);
            this.txtFromLine.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(205, 458);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 29);
            this.label10.TabIndex = 16;
            this.label10.Text = ":";
            // 
            // txtToLine
            // 
            this.txtToLine.Location = new System.Drawing.Point(227, 460);
            this.txtToLine.Margin = new System.Windows.Forms.Padding(6);
            this.txtToLine.Name = "txtToLine";
            this.txtToLine.Size = new System.Drawing.Size(72, 29);
            this.txtToLine.TabIndex = 17;
            // 
            // cbLogPct
            // 
            this.cbLogPct.AutoSize = true;
            this.cbLogPct.Location = new System.Drawing.Point(312, 462);
            this.cbLogPct.Margin = new System.Windows.Forms.Padding(6);
            this.cbLogPct.Name = "cbLogPct";
            this.cbLogPct.Size = new System.Drawing.Size(56, 29);
            this.cbLogPct.TabIndex = 18;
            this.cbLogPct.Text = "%";
            this.cbLogPct.UseVisualStyleBackColor = true;
            // 
            // cbZoomableY
            // 
            this.cbZoomableY.AutoSize = true;
            this.cbZoomableY.Location = new System.Drawing.Point(374, 462);
            this.cbZoomableY.Margin = new System.Windows.Forms.Padding(6);
            this.cbZoomableY.Name = "cbZoomableY";
            this.cbZoomableY.Size = new System.Drawing.Size(186, 29);
            this.cbZoomableY.TabIndex = 19;
            this.cbZoomableY.Text = "Zoomable Y Axis";
            this.cbZoomableY.UseVisualStyleBackColor = true;
            // 
            // cbLinkXZoom
            // 
            this.cbLinkXZoom.AutoSize = true;
            this.cbLinkXZoom.Checked = true;
            this.cbLinkXZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLinkXZoom.Location = new System.Drawing.Point(374, 502);
            this.cbLinkXZoom.Margin = new System.Windows.Forms.Padding(4);
            this.cbLinkXZoom.Name = "cbLinkXZoom";
            this.cbLinkXZoom.Size = new System.Drawing.Size(148, 29);
            this.cbLinkXZoom.TabIndex = 20;
            this.cbLinkXZoom.Text = "Link X Zoom";
            this.cbLinkXZoom.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(24, 498);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 29);
            this.label13.TabIndex = 22;
            this.label13.Text = "Size X:Y";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // tbSizeY
            // 
            this.tbSizeY.Location = new System.Drawing.Point(227, 498);
            this.tbSizeY.Margin = new System.Windows.Forms.Padding(6);
            this.tbSizeY.Name = "tbSizeY";
            this.tbSizeY.Size = new System.Drawing.Size(72, 29);
            this.tbSizeY.TabIndex = 25;
            this.tbSizeY.Text = "250";
            this.tbSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSizeY.TextChanged += new System.EventHandler(this.tbSizeY_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(205, 496);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 29);
            this.label14.TabIndex = 24;
            this.label14.Text = ":";
            // 
            // tbSizeX
            // 
            this.tbSizeX.Location = new System.Drawing.Point(128, 498);
            this.tbSizeX.Margin = new System.Windows.Forms.Padding(6);
            this.tbSizeX.Name = "tbSizeX";
            this.tbSizeX.Size = new System.Drawing.Size(72, 29);
            this.tbSizeX.TabIndex = 23;
            this.tbSizeX.Text = "750";
            this.tbSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbbLogType
            // 
            this.cbbLogType.FormattingEnabled = true;
            this.cbbLogType.Location = new System.Drawing.Point(18, 26);
            this.cbbLogType.Margin = new System.Windows.Forms.Padding(4);
            this.cbbLogType.Name = "cbbLogType";
            this.cbbLogType.Size = new System.Drawing.Size(206, 32);
            this.cbbLogType.TabIndex = 26;
            this.cbbLogType.SelectedIndexChanged += new System.EventHandler(this.cbbLogType_SelectedIndexChanged);
            // 
            // cbSummary
            // 
            this.cbSummary.AutoSize = true;
            this.cbSummary.Location = new System.Drawing.Point(578, 462);
            this.cbSummary.Margin = new System.Windows.Forms.Padding(6);
            this.cbSummary.Name = "cbSummary";
            this.cbSummary.Size = new System.Drawing.Size(122, 29);
            this.cbSummary.TabIndex = 28;
            this.cbSummary.Text = "Summary";
            this.cbSummary.UseVisualStyleBackColor = true;
            // 
            // cbGPSTrack
            // 
            this.cbGPSTrack.AutoSize = true;
            this.cbGPSTrack.Location = new System.Drawing.Point(578, 502);
            this.cbGPSTrack.Margin = new System.Windows.Forms.Padding(4);
            this.cbGPSTrack.Name = "cbGPSTrack";
            this.cbGPSTrack.Size = new System.Drawing.Size(135, 29);
            this.cbGPSTrack.TabIndex = 29;
            this.cbGPSTrack.Text = "GPS Track";
            this.cbGPSTrack.UseVisualStyleBackColor = true;
            // 
            // btnClearData
            // 
            this.btnClearData.Location = new System.Drawing.Point(193, 546);
            this.btnClearData.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(152, 42);
            this.btnClearData.TabIndex = 30;
            this.btnClearData.Text = "Clear All Data";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // cbMouseDP
            // 
            this.cbMouseDP.AutoSize = true;
            this.cbMouseDP.Location = new System.Drawing.Point(731, 462);
            this.cbMouseDP.Name = "cbMouseDP";
            this.cbMouseDP.Size = new System.Drawing.Size(193, 29);
            this.cbMouseDP.TabIndex = 31;
            this.cbMouseDP.Text = "Mouse Data Point";
            this.cbMouseDP.UseVisualStyleBackColor = true;
            // 
            // cbWhiteBG
            // 
            this.cbWhiteBG.AutoSize = true;
            this.cbWhiteBG.Location = new System.Drawing.Point(731, 502);
            this.cbWhiteBG.Name = "cbWhiteBG";
            this.cbWhiteBG.Size = new System.Drawing.Size(122, 29);
            this.cbWhiteBG.TabIndex = 32;
            this.cbWhiteBG.Text = "White BG";
            this.cbWhiteBG.UseVisualStyleBackColor = true;
            // 
            // cbSeriesLog
            // 
            this.cbSeriesLog.AutoSize = true;
            this.cbSeriesLog.Location = new System.Drawing.Point(951, 462);
            this.cbSeriesLog.Name = "cbSeriesLog";
            this.cbSeriesLog.Size = new System.Drawing.Size(132, 29);
            this.cbSeriesLog.TabIndex = 33;
            this.cbSeriesLog.Text = "Series Log";
            this.cbSeriesLog.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(951, 511);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(183, 29);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "Individual Charts";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2092, 1576);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cbSeriesLog);
            this.Controls.Add(this.cbWhiteBG);
            this.Controls.Add(this.cbMouseDP);
            this.Controls.Add(this.btnClearData);
            this.Controls.Add(this.cbGPSTrack);
            this.Controls.Add(this.cbSummary);
            this.Controls.Add(this.cbbLogType);
            this.Controls.Add(this.tbSizeY);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbSizeX);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbLinkXZoom);
            this.Controls.Add(this.cbZoomableY);
            this.Controls.Add(this.cbLogPct);
            this.Controls.Add(this.txtToLine);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtFromLine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReportConfig);
            this.Controls.Add(this.btnSaveReportConfig);
            this.Controls.Add(this.btnLoadReportConfig);
            this.Controls.Add(this.pbLoadTlv);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblPct);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtChartTitle);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "OneShot Plotter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver);
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tlpSeriesConfiguration.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ProgressBar pbLoadTlv;
        private System.Windows.Forms.ListBox lbModeControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbGcsData;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox lbControl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbGuid;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListBox lbSnsr;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox lbNavd;
        public System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtChartTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tlpSeriesConfiguration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblPct;
        private System.Windows.Forms.Button btnLoadReportConfig;
        private System.Windows.Forms.Button btnSaveReportConfig;
        private System.Windows.Forms.TextBox txtReportConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.CheckBox cbZoomableY;
        public System.Windows.Forms.CheckBox cbLogPct;
        public System.Windows.Forms.TextBox txtFromLine;
        public System.Windows.Forms.TextBox txtToLine;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox tbSizeY;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox tbSizeX;
        public System.Windows.Forms.CheckBox cbLinkXZoom;
        public System.Windows.Forms.ComboBox cbbLogType;
        private System.Windows.Forms.CheckBox cbSummary;
        private System.Windows.Forms.CheckBox cbGPSTrack;
        private System.Windows.Forms.Button btnClearData;
        public System.Windows.Forms.CheckBox cbMouseDP;
        public System.Windows.Forms.CheckBox cbWhiteBG;
        private System.Windows.Forms.CheckBox cbSeriesLog;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

