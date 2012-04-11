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
            this.BeforeDisposing();
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
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.toolStripConsole = new com232term.Controls.DataSender.ToolStripDataSenderGuiConsole();
            this.toolStripDataSenderGuiButtonsLast = new com232term.Controls.DataSender.ToolStripDataSenderGuiButtonsLast();
            this.toolStripDataSenderGuiButtonsStatic = new com232term.Controls.DataSender.ToolStripDataSenderGuiButtonsStatic();
            this.toolStripViewGui = new com232term.Controls.View.ToolStripViewGui();
            this.toolStripConnectionGui = new com232term.Controls.DataSender.ToolStripConnectionGui();
            this.toolStripLogsGui = new com232term.Controls.DataSender.ToolStripLogsGui();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.RightToolStripPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStripConsole);
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(this.rtbLog);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(807, 378);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStripDataSenderGuiButtonsLast);
            toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStripDataSenderGuiButtonsStatic);
            toolStripContainer1.Size = new System.Drawing.Size(859, 429);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripViewGui);
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripConnectionGui);
            toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripLogsGui);
            // 
            // rtbLog
            // 
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(807, 378);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // toolStripConsole
            // 
            this.toolStripConsole.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripConsole.Location = new System.Drawing.Point(0, 0);
            this.toolStripConsole.Name = "toolStripConsole";
            this.toolStripConsole.Size = new System.Drawing.Size(859, 26);
            this.toolStripConsole.Stretch = true;
            this.toolStripConsole.TabIndex = 0;
            this.toolStripConsole.Text = "Console";
            // 
            // toolStripDataSenderGuiButtonsLast
            // 
            this.toolStripDataSenderGuiButtonsLast.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripDataSenderGuiButtonsLast.Location = new System.Drawing.Point(0, 0);
            this.toolStripDataSenderGuiButtonsLast.Name = "toolStripDataSenderGuiButtonsLast";
            this.toolStripDataSenderGuiButtonsLast.Size = new System.Drawing.Size(26, 378);
            this.toolStripDataSenderGuiButtonsLast.Stretch = true;
            this.toolStripDataSenderGuiButtonsLast.TabIndex = 1;
            this.toolStripDataSenderGuiButtonsLast.Text = "Last sended packets";
            // 
            // toolStripDataSenderGuiButtonsStatic
            // 
            this.toolStripDataSenderGuiButtonsStatic.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripDataSenderGuiButtonsStatic.Location = new System.Drawing.Point(26, 0);
            this.toolStripDataSenderGuiButtonsStatic.Name = "toolStripDataSenderGuiButtonsStatic";
            this.toolStripDataSenderGuiButtonsStatic.Size = new System.Drawing.Size(26, 378);
            this.toolStripDataSenderGuiButtonsStatic.Stretch = true;
            this.toolStripDataSenderGuiButtonsStatic.TabIndex = 2;
            this.toolStripDataSenderGuiButtonsStatic.Text = "Static packets";
            // 
            // toolStripViewGui
            // 
            this.toolStripViewGui.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripViewGui.Location = new System.Drawing.Point(304, 0);
            this.toolStripViewGui.Name = "toolStripViewGui";
            this.toolStripViewGui.Size = new System.Drawing.Size(245, 25);
            this.toolStripViewGui.TabIndex = 3;
            // 
            // toolStripConnectionGui
            // 
            this.toolStripConnectionGui.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripConnectionGui.Location = new System.Drawing.Point(3, 0);
            this.toolStripConnectionGui.Name = "toolStripConnectionGui";
            this.toolStripConnectionGui.Size = new System.Drawing.Size(80, 25);
            this.toolStripConnectionGui.TabIndex = 2;
            // 
            // toolStripLogsGui
            // 
            this.toolStripLogsGui.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripLogsGui.Location = new System.Drawing.Point(83, 0);
            this.toolStripLogsGui.Name = "toolStripLogsGui";
            this.toolStripLogsGui.Size = new System.Drawing.Size(162, 25);
            this.toolStripLogsGui.TabIndex = 1;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private Controls.DataSender.ToolStripDataSenderGuiConsole toolStripConsole;
        private Controls.DataSender.ToolStripDataSenderGuiButtonsLast toolStripDataSenderGuiButtonsLast;
        private Controls.DataSender.ToolStripDataSenderGuiButtonsStatic toolStripDataSenderGuiButtonsStatic;
        private Controls.DataSender.ToolStripLogsGui toolStripLogsGui;
        private Controls.DataSender.ToolStripConnectionGui toolStripConnectionGui;
        private Controls.View.ToolStripViewGui toolStripViewGui;
    }
}

