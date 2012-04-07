using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;

namespace com232term
{
    public partial class FormMain : Form
    {
        private Worker mWorker;

        public FormMain()
        {
            InitializeComponent();

            this.mWorker = new Worker();
            this.mWorker.OnSettingsChanged += new EventHandler(mWorker_OnSettingsChanged);
            this.mWorker.OnOpened += new EventHandler(mWorker_OnOpened);
            this.mWorker.OnClosed += new EventHandler(mWorker_OnClosed);
            this.mWorker.OnDataReceived += new EventHandler<DataReceivedEventArgs>(mWorker_OnDataReceived);

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

            this.tscbPortName.SelectedItem = Options.Instance.PortOptions.PortName;
            this.tscbBaudrates.SelectedItem = Options.Instance.PortOptions.Baudrate;
            this.tscbParity.SelectedItem = Options.Instance.PortOptions.Parity;
            this.tscbStopBits.SelectedItem = Options.Instance.PortOptions.StopBits;
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

        private void mWorker_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.rtbLog.AppendText(e.Value.Length.ToString() + "\n");
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
    }
}
