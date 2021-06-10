namespace FlightPlotter
{
    partial class Plots
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plots));
            this.pnlPlots = new System.Windows.Forms.FlowLayoutPanel();
            this.gbPlots = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveChartsImage = new System.Windows.Forms.Button();
            this.gbPlots.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPlots
            // 
            this.pnlPlots.AutoScroll = true;
            this.pnlPlots.AutoSize = true;
            this.pnlPlots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlots.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlPlots.Location = new System.Drawing.Point(3, 16);
            this.pnlPlots.MaximumSize = new System.Drawing.Size(1750, 880);
            this.pnlPlots.Name = "pnlPlots";
            this.pnlPlots.Size = new System.Drawing.Size(717, 122);
            this.pnlPlots.TabIndex = 0;
            this.pnlPlots.WrapContents = false;
            // 
            // gbPlots
            // 
            this.gbPlots.AutoSize = true;
            this.gbPlots.Controls.Add(this.pnlPlots);
            this.gbPlots.Location = new System.Drawing.Point(4, 37);
            this.gbPlots.MaximumSize = new System.Drawing.Size(1750, 880);
            this.gbPlots.Name = "gbPlots";
            this.gbPlots.Size = new System.Drawing.Size(723, 141);
            this.gbPlots.TabIndex = 6;
            this.gbPlots.TabStop = false;
            this.gbPlots.Text = "Flight Plots";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.btnSaveChartsImage);
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 29);
            this.panel1.TabIndex = 7;
            // 
            // btnSaveChartsImage
            // 
            this.btnSaveChartsImage.Location = new System.Drawing.Point(3, 3);
            this.btnSaveChartsImage.Name = "btnSaveChartsImage";
            this.btnSaveChartsImage.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChartsImage.TabIndex = 0;
            this.btnSaveChartsImage.Text = "Save";
            this.btnSaveChartsImage.UseVisualStyleBackColor = true;
            this.btnSaveChartsImage.Click += new System.EventHandler(this.btnSaveChartsImage_Click);
            // 
            // Plots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(739, 182);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbPlots);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1755, 930);
            this.Name = "Plots";
            this.Text = "Plots";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Plots_FormClosed);
            this.Load += new System.EventHandler(this.Plots_Load);
            this.gbPlots.ResumeLayout(false);
            this.gbPlots.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlPlots;
        private System.Windows.Forms.GroupBox gbPlots;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveChartsImage;
    }
}