using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using com232term.Classes.Options;
using com232term.Controls.DataSender;
using com232term.Forms;
using com232term.Classes.Worker;
using com232term.Classes.Logger;
using com232term.Classes.Sender;

namespace com232term
{
    public partial class FormMain : Form
    {
        private Worker mWorker;
        private Logger mLogger;
        private DataSender mSender;

        public FormMain()
        {
            InitializeComponent();

            this.mWorker = new Worker();
            this.mWorker.OnDataLog += new EventHandler<DataLogEventArgs>(mWorker_OnDataLog);
            this.mWorker.OnMessageLog += new EventHandler<MessageLogEventArgs>(mWorker_OnMessageLog);
            this.toolStripConnectionGui.Worker = this.mWorker;

            this.mLogger = new Logger(this.rtbLog);
            this.toolStripLogsGui.Logger = this.mLogger;

            this.mSender = new DataSender();
            this.mSender.OnStaticEditorCall += new EventHandler<CallPacketsEditorEventArgs>(mSender_OnStaticEditorCall);
            this.mSender.OnSendData += new EventHandler<SendDataEventArgs>(mSender_OnSendData);
            this.toolStripConsole.Sender = this.mSender;
            this.toolStripDataSenderGuiButtonsLast.Sender = this.mSender;
            this.toolStripDataSenderGuiButtonsStatic.Sender = this.mSender;
        }

        private void BeforeDisposing()
        {
            com232term.Classes.Options.Options.Save();
            this.mWorker.Dispose();
            this.mSender.Dispose();
            this.mLogger.Dispose();
        }

        private void mWorker_OnDataLog(object sender, DataLogEventArgs e)
        {
            this.mLogger.LogData(e.Time, e.DataDirection, e.Value);
        }

        private void mWorker_OnMessageLog(object sender, MessageLogEventArgs e)
        {
            this.mLogger.LogMessage(e.Time, e.Message);
        }

        private void mSender_OnStaticEditorCall(object sender, CallPacketsEditorEventArgs e)
        {
            using (PacketsEditor dialog = new PacketsEditor(e.List))
            {
                dialog.ShowDialog();
            }
        }

        private void mSender_OnSendData(object sender, SendDataEventArgs e)
        {
            this.mWorker.Send(e.Data);
        }
    }
}
