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
        }

        private void mWorker_OnSettingsChanged(object sender, EventArgs e)
        {
            this.tssbConnect.Text = String.Format(
                "{0}, {1}, {2}, {3}",
                this.mWorker.Settings.PortName.ExtractPortName(),
                this.mWorker.Settings.Baudrate,
                this.mWorker.Settings.Parity,
                this.mWorker.Settings.StopBits);

            this.tssbConnect.ToolTipText = String.Format(
                "{0}, {1}, {2}, {3}",
                this.mWorker.Settings.PortName,
                this.mWorker.Settings.Baudrate,
                this.mWorker.Settings.Parity,
                this.mWorker.Settings.StopBits);
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
    }
}
