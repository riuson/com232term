namespace com232term
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.tsConnectionMenu = new System.Windows.Forms.ToolStrip();
            this.tssbConnect = new System.Windows.Forms.ToolStripSplitButton();
            this.tscbPortName = new System.Windows.Forms.ToolStripComboBox();
            this.tscbBaudrates = new System.Windows.Forms.ToolStripComboBox();
            this.tscbParity = new System.Windows.Forms.ToolStripComboBox();
            this.tscbStopBits = new System.Windows.Forms.ToolStripComboBox();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            this.tsConnectionMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(this.rtbLog);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(834, 404);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new System.Drawing.Size(859, 429);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsConnectionMenu);
            // 
            // rtbLog
            // 
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(834, 404);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
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
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 429);
            this.Controls.Add(toolStripContainer1);
            this.Name = "FormMain";
            this.Text = "Com232Term";
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            this.tsConnectionMenu.ResumeLayout(false);
            this.tsConnectionMenu.PerformLayout();
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
    }
}

