﻿namespace com232term
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            this.mWorker.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripContainer toolStripContainer1;
            System.Windows.Forms.ToolStrip toolStripSender;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.toolStripDataSender = new com232term.Controls.DataSender.ToolStripDataSenderGuiTable();
            this.tsConnectionMenu = new System.Windows.Forms.ToolStrip();
            this.tssbConnect = new System.Windows.Forms.ToolStripSplitButton();
            this.tscbPortName = new System.Windows.Forms.ToolStripComboBox();
            this.tscbBaudrates = new System.Windows.Forms.ToolStripComboBox();
            this.tscbParity = new System.Windows.Forms.ToolStripComboBox();
            this.tscbStopBits = new System.Windows.Forms.ToolStripComboBox();
            this.tsLogMenu = new System.Windows.Forms.ToolStrip();
            this.tsddbFormat = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsddbColors = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiColorTransmitted = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiColorReceived = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiColorSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiColorTime = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripConsole1 = new com232term.Controls.DataSender.ToolStripGuiConsole();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            toolStripSender = new System.Windows.Forms.ToolStrip();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.RightToolStripPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            toolStripSender.SuspendLayout();
            this.tsConnectionMenu.SuspendLayout();
            this.tsLogMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStripConsole1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(this.rtbLog);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(777, 379);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            toolStripContainer1.RightToolStripPanel.Controls.Add(toolStripSender);
            toolStripContainer1.Size = new System.Drawing.Size(859, 429);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsConnectionMenu);
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsLogMenu);
            // 
            // rtbLog
            // 
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(777, 379);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // toolStripSender
            // 
            toolStripSender.Dock = System.Windows.Forms.DockStyle.None;
            toolStripSender.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDataSender});
            toolStripSender.Location = new System.Drawing.Point(0, 0);
            toolStripSender.Name = "toolStripSender";
            toolStripSender.Size = new System.Drawing.Size(82, 379);
            toolStripSender.Stretch = true;
            toolStripSender.TabIndex = 1;
            toolStripSender.Text = "Data sending";
            // 
            // toolStripDataSender
            // 
            this.toolStripDataSender.Name = "toolStripDataSender";
            this.toolStripDataSender.Size = new System.Drawing.Size(80, 29);
            this.toolStripDataSender.Text = "toolStripDataSender1";
            // 
            // tsConnectionMenu
            // 
            this.tsConnectionMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.tsConnectionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbConnect});
            this.tsConnectionMenu.Location = new System.Drawing.Point(3, 0);
            this.tsConnectionMenu.Name = "tsConnectionMenu";
            this.tsConnectionMenu.Size = new System.Drawing.Size(80, 25);
            this.tsConnectionMenu.TabIndex = 0;
            this.tsConnectionMenu.Text = "Port";
            // 
            // tssbConnect
            // 
            this.tssbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbConnect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbPortName,
            this.tscbBaudrates,
            this.tscbParity,
            this.tscbStopBits});
            this.tssbConnect.Image = ((System.Drawing.Image)(resources.GetObject("tssbConnect.Image")));
            this.tssbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbConnect.Name = "tssbConnect";
            this.tssbConnect.Size = new System.Drawing.Size(68, 22);
            this.tssbConnect.Text = "Connect";
            this.tssbConnect.ButtonClick += new System.EventHandler(this.OnConnectionClick);
            // 
            // tscbPortName
            // 
            this.tscbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbPortName.Name = "tscbPortName";
            this.tscbPortName.Size = new System.Drawing.Size(300, 23);
            this.tscbPortName.ToolTipText = "Port Name";
            this.tscbPortName.SelectedIndexChanged += new System.EventHandler(this.OnPortSettingsChanged);
            // 
            // tscbBaudrates
            // 
            this.tscbBaudrates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbBaudrates.Name = "tscbBaudrates";
            this.tscbBaudrates.Size = new System.Drawing.Size(121, 23);
            this.tscbBaudrates.ToolTipText = "Baudrate";
            this.tscbBaudrates.SelectedIndexChanged += new System.EventHandler(this.OnPortSettingsChanged);
            // 
            // tscbParity
            // 
            this.tscbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbParity.Name = "tscbParity";
            this.tscbParity.Size = new System.Drawing.Size(121, 23);
            this.tscbParity.ToolTipText = "Parity";
            this.tscbParity.SelectedIndexChanged += new System.EventHandler(this.OnPortSettingsChanged);
            // 
            // tscbStopBits
            // 
            this.tscbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbStopBits.Name = "tscbStopBits";
            this.tscbStopBits.Size = new System.Drawing.Size(121, 23);
            this.tscbStopBits.ToolTipText = "Stop Bits";
            this.tscbStopBits.SelectedIndexChanged += new System.EventHandler(this.OnPortSettingsChanged);
            // 
            // tsLogMenu
            // 
            this.tsLogMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.tsLogMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbFormat,
            this.tsddbColors});
            this.tsLogMenu.Location = new System.Drawing.Point(83, 0);
            this.tsLogMenu.Name = "tsLogMenu";
            this.tsLogMenu.Size = new System.Drawing.Size(150, 25);
            this.tsLogMenu.TabIndex = 1;
            this.tsLogMenu.Text = "Log format";
            // 
            // tsddbFormat
            // 
            this.tsddbFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbFormat.Image = ((System.Drawing.Image)(resources.GetObject("tsddbFormat.Image")));
            this.tsddbFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbFormat.Name = "tsddbFormat";
            this.tsddbFormat.Size = new System.Drawing.Size(84, 22);
            this.tsddbFormat.Text = "Format: Hex";
            // 
            // tsddbColors
            // 
            this.tsddbColors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbColors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiColorTransmitted,
            this.tsmiColorReceived,
            this.tsmiColorSystem,
            this.tsmiColorTime});
            this.tsddbColors.Image = ((System.Drawing.Image)(resources.GetObject("tsddbColors.Image")));
            this.tsddbColors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbColors.Name = "tsddbColors";
            this.tsddbColors.Size = new System.Drawing.Size(54, 22);
            this.tsddbColors.Text = "Colors";
            // 
            // tsmiColorTransmitted
            // 
            this.tsmiColorTransmitted.Name = "tsmiColorTransmitted";
            this.tsmiColorTransmitted.Size = new System.Drawing.Size(138, 22);
            this.tsmiColorTransmitted.Text = "Transmitted";
            this.tsmiColorTransmitted.Click += new System.EventHandler(this.OnColorsClick);
            // 
            // tsmiColorReceived
            // 
            this.tsmiColorReceived.Name = "tsmiColorReceived";
            this.tsmiColorReceived.Size = new System.Drawing.Size(138, 22);
            this.tsmiColorReceived.Text = "Received";
            this.tsmiColorReceived.Click += new System.EventHandler(this.OnColorsClick);
            // 
            // tsmiColorSystem
            // 
            this.tsmiColorSystem.Name = "tsmiColorSystem";
            this.tsmiColorSystem.Size = new System.Drawing.Size(138, 22);
            this.tsmiColorSystem.Text = "System";
            this.tsmiColorSystem.Click += new System.EventHandler(this.OnColorsClick);
            // 
            // tsmiColorTime
            // 
            this.tsmiColorTime.Name = "tsmiColorTime";
            this.tsmiColorTime.Size = new System.Drawing.Size(138, 22);
            this.tsmiColorTime.Text = "Time";
            this.tsmiColorTime.Click += new System.EventHandler(this.OnColorsClick);
            // 
            // toolStripConsole1
            // 
            this.toolStripConsole1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripConsole1.Location = new System.Drawing.Point(3, 0);
            this.toolStripConsole1.Name = "toolStripConsole1";
            this.toolStripConsole1.Size = new System.Drawing.Size(192, 25);
            this.toolStripConsole1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 429);
            this.Controls.Add(toolStripContainer1);
            this.Name = "FormMain";
            this.Text = "Com232Term";
            toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            toolStripContainer1.BottomToolStripPanel.PerformLayout();
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.RightToolStripPanel.ResumeLayout(false);
            toolStripContainer1.RightToolStripPanel.PerformLayout();
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            toolStripSender.ResumeLayout(false);
            toolStripSender.PerformLayout();
            this.tsConnectionMenu.ResumeLayout(false);
            this.tsConnectionMenu.PerformLayout();
            this.tsLogMenu.ResumeLayout(false);
            this.tsLogMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ToolStrip tsConnectionMenu;
        private System.Windows.Forms.ToolStripSplitButton tssbConnect;
        private System.Windows.Forms.ToolStripComboBox tscbPortName;
        private System.Windows.Forms.ToolStripComboBox tscbBaudrates;
        private System.Windows.Forms.ToolStripComboBox tscbParity;
        private System.Windows.Forms.ToolStripComboBox tscbStopBits;
        private System.Windows.Forms.ToolStrip tsLogMenu;
        private System.Windows.Forms.ToolStripDropDownButton tsddbFormat;
        private System.Windows.Forms.ToolStripDropDownButton tsddbColors;
        private System.Windows.Forms.ToolStripMenuItem tsmiColorTransmitted;
        private System.Windows.Forms.ToolStripMenuItem tsmiColorReceived;
        private System.Windows.Forms.ToolStripMenuItem tsmiColorSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiColorTime;
        private Controls.DataSender.ToolStripDataSenderGuiTable toolStripDataSender;
        private Controls.DataSender.ToolStripGuiConsole toolStripConsole1;
    }
}

