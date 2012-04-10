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
            this.mWorker.OnSettingsChanged += new EventHandler(mWorker_OnSettingsChanged);
            this.mWorker.OnOpened += new EventHandler(mWorker_OnOpened);
            this.mWorker.OnClosed += new EventHandler(mWorker_OnClosed);
            this.mWorker.OnDataLog += new EventHandler<DataLogEventArgs>(mWorker_OnDataLog);
            this.mWorker.OnMessageLog += new EventHandler<MessageLogEventArgs>(mWorker_OnMessageLog);

            this.mLogger = new Logger(this.rtbLog);
            this.toolStripLogsGui.Logger = this.mLogger;

            this.mSender = new DataSender();
            this.mSender.OnStaticEditorCall += new EventHandler<CallPacketsEditorEventArgs>(mSender_OnStaticEditorCall);
            this.mSender.OnSendData += new EventHandler<SendDataEventArgs>(mSender_OnSendData);
            this.toolStripConsole.Sender = this.mSender;
            this.toolStripDataSenderGuiButtonsLast.Sender = this.mSender;
            this.toolStripDataSenderGuiButtonsStatic.Sender = this.mSender;

            this.SetDefaultValues();

            this.LoadSettings();
        }

        private void LoadSettings()
        {
            // load port settings
            this.tscbPortName.SelectedItem = Options.Instance.PortOptions.PortName;
            this.tscbBaudrates.SelectedItem = Options.Instance.PortOptions.Baudrate;
            this.tscbParity.SelectedItem = Options.Instance.PortOptions.Parity;
            this.tscbStopBits.SelectedItem = Options.Instance.PortOptions.StopBits;

            this.mSender.Settings = Options.Instance.SendOptions;
        }

        private void SetDefaultValues()
        {
            this.tscbPortName.Items.Clear();
            this.tscbPortName.Items.AddRange(PortSettings.PortsList);

            this.tscbBaudrates.Items.Clear();
            foreach (int a in PortSettings.BaudratesList)
                this.tscbBaudrates.Items.Add(a);

            this.tscbParity.Items.Clear();
            foreach (System.IO.Ports.Parity a in PortSettings.ParitiesList)
                this.tscbParity.Items.Add(a);

            this.tscbStopBits.Items.Clear();
            foreach (System.IO.Ports.StopBits a in PortSettings.StopBitsList)
                this.tscbStopBits.Items.Add(a);
        }

        private void mWorker_OnSettingsChanged(object sender, EventArgs e)
        {
            this.UpdateConnectButtonText();
        }

        private void UpdateConnectButtonText()
        {
            string state;
            if (this.mWorker.IsOpen)
                state = "Close";
            else
                state = "Open";

            this.tssbConnect.Text = String.Format(
                "{4}: {0}, {1}, {2}, {3}",
                this.mWorker.PortOptions.PortName.ExtractPortName(),
                this.mWorker.PortOptions.Baudrate,
                this.mWorker.PortOptions.Parity,
                this.mWorker.PortOptions.StopBits,
                state);

            this.tssbConnect.ToolTipText = String.Format(
                "{4}: {0}, {1}, {2}, {3}",
                this.mWorker.PortOptions.PortName,
                this.mWorker.PortOptions.Baudrate,
                this.mWorker.PortOptions.Parity,
                this.mWorker.PortOptions.StopBits,
                state);
        }

        private void mWorker_OnClosed(object sender, EventArgs e)
        {
            this.UpdateConnectButtonText();
        }

        private void mWorker_OnOpened(object sender, EventArgs e)
        {
            this.UpdateConnectButtonText();

            // save port settings
            Options.Instance.PortOptions = this.mWorker.PortOptions;
            Options.Save();
        }

        private void mWorker_OnDataLog(object sender, DataLogEventArgs e)
        {
            this.mLogger.LogData(e.Time, e.DataDirection, e.Value);
        }

        private void mWorker_OnMessageLog(object sender, MessageLogEventArgs e)
        {
            this.mLogger.LogMessage(e.Time, e.Message);
        }

        private void OnPortSettingsChanged(object sender, EventArgs e)
        {
            // portname
            string portname;
            if (this.tscbPortName.SelectedIndex >= 0)
                portname = this.tscbPortName.SelectedItem.ToString();
            else
                portname = String.Empty;

            // parity
            System.IO.Ports.Parity parity;
            if (this.tscbParity.SelectedIndex >= 0)
                parity = (System.IO.Ports.Parity)this.tscbParity.SelectedItem;
            else
                parity = System.IO.Ports.Parity.None;

            // baudrate
            int baudrate = 9600;
            if (this.tscbBaudrates.SelectedIndex >= 0)
                baudrate = Convert.ToInt32(this.tscbBaudrates.SelectedItem);

            // stop bits
            System.IO.Ports.StopBits stopbits;
            if (this.tscbStopBits.SelectedIndex >= 0)
                stopbits = (System.IO.Ports.StopBits)this.tscbStopBits.SelectedItem;
            else
                stopbits = System.IO.Ports.StopBits.One;

            // setup
            this.mWorker.SetPortName(portname, baudrate, parity, stopbits);
        }

        private void OnConnectionClick(object sender, EventArgs e)
        {
            if (this.mWorker.IsOpen)
            {
                this.mWorker.Close();
            }
            else
            {
                this.mWorker.Open();
            }
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
