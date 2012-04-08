using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using com232term.Classes.Options;

namespace com232term
{
    public partial class FormMain : Form
    {
        private Worker mWorker;
        private Logger mLogger;

        public FormMain()
        {
            InitializeComponent();

            this.mWorker = new Worker();
            this.mWorker.OnSettingsChanged += new EventHandler(mWorker_OnSettingsChanged);
            this.mWorker.OnOpened += new EventHandler(mWorker_OnOpened);
            this.mWorker.OnClosed += new EventHandler(mWorker_OnClosed);
            this.mWorker.OnDataLog += new EventHandler<DataLogEventArgs>(mWorker_OnDataLog);

            this.mLogger = new Logger(this.rtbLog);

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

            // load format settings
            LogSettings.DisplayFormat format = Options.Instance.LogOptions.Format;
            this.tsddbFormat.Text = "Format: " + format.ToString();
            if (format == LogSettings.DisplayFormat.Auto)
            {
                foreach (ToolStripItem item in this.tsddbFormat.DropDownItems)
                {
                    ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                    LogSettings.DisplayFormat formatItem = (LogSettings.DisplayFormat)menuItem.Tag;
                    menuItem.Checked = (formatItem == LogSettings.DisplayFormat.Auto);
                }
            }
            else
            {
                foreach (ToolStripItem item in this.tsddbFormat.DropDownItems)
                {
                    ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                    LogSettings.DisplayFormat formatItem = (LogSettings.DisplayFormat)menuItem.Tag;
                    if (formatItem == LogSettings.DisplayFormat.Auto)
                        continue;
                    menuItem.Checked = ((format & formatItem) == formatItem);
                }
            }

            this.mLogger.LogOptions = Options.Instance.LogOptions;
            this.tsmiColorReceived.ForeColor = this.mLogger.LogOptions.ReceivedColor;
            this.tsmiColorTransmitted.ForeColor = this.mLogger.LogOptions.TransmittedColor;
        }

        private void SetDefaultValues()
        {
            this.tscbPortName.Items.Clear();
            this.tscbPortName.Items.AddRange(Worker.PortsList);

            this.tscbBaudrates.Items.Clear();
            foreach (int a in Worker.BaudratesList)
                this.tscbBaudrates.Items.Add(a);

            this.tscbParity.Items.Clear();
            foreach (System.IO.Ports.Parity a in Worker.ParitiesList)
                this.tscbParity.Items.Add(a);

            this.tscbStopBits.Items.Clear();
            foreach (System.IO.Ports.StopBits a in Worker.StopBitsList)
                this.tscbStopBits.Items.Add(a);

            this.tsddbFormat.DropDownItems.Clear();
            foreach (LogSettings.DisplayFormat a in Logger.DisplayFormats)
            {
                ToolStripMenuItem item = new ToolStripMenuItem() { Tag = a, Text = a.ToString(), CheckOnClick = true };
                item.Click += new EventHandler(DisplayFormat_Click);
                this.tsddbFormat.DropDownItems.Add(item);
            }
        }

        private void DisplayFormat_Click(object sender, EventArgs e)
        {
            LogSettings.DisplayFormat format = LogSettings.DisplayFormat.Hex;
            foreach (ToolStripItem item in this.tsddbFormat.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                LogSettings.DisplayFormat formatItem = (LogSettings.DisplayFormat)menuItem.Tag;
                if (menuItem.Checked)
                    format |= formatItem;
                else
                    format &= ~formatItem;
            }
            this.mLogger.LogOptions.Format = format;
            this.tsddbFormat.Text = "Format: " + format.ToString();
            Options.Instance.LogOptions.Format = format;
            Options.Save();
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

        private void OnColorsClick(object sender, EventArgs e)
        {
            if (sender == this.tsmiColorReceived)
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.Color = this.mLogger.LogOptions.ReceivedColor;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.mLogger.LogOptions.ReceivedColor = dialog.Color;
                        Options.Instance.LogOptions.ReceivedColor = dialog.Color;
                        this.tsmiColorReceived.ForeColor = dialog.Color;
                        Options.Save();
                    }
                }
            }
            if (sender == this.tsmiColorTransmitted)
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.Color = this.mLogger.LogOptions.TransmittedColor;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.mLogger.LogOptions.TransmittedColor = dialog.Color;
                        Options.Instance.LogOptions.TransmittedColor = dialog.Color;
                        this.tsmiColorTransmitted.ForeColor = dialog.Color;
                        Options.Save();
                    }
                }
            }
        }
    }
}
