using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using System.ComponentModel;
using com232term.Classes.Options;
using System.Drawing;
using System.IO.Ports;
using com232term.Classes.Worker;

namespace com232term.Controls.DataSender
{
    public class ToolStripConnectionGui : ToolStrip
    {
        private ToolStripSplitButton mButtonConnect;
        private ToolStripComboBox mComboBoxPort;
        private ToolStripComboBox mComboBoxBaudate;
        private ToolStripComboBox mComboBoxParity;
        private ToolStripComboBox mComboBoxStopBits;
        private ToolStripMenuItem mMenuItemRtsEnable;
        private ToolStripMenuItem mMenuItemDtrEnable;
        private IWorker mWorker;

        public ToolStripConnectionGui()
        {
            this.mButtonConnect = new ToolStripSplitButton()
            {
                Text = "Connect"
            };
            this.mButtonConnect.ButtonClick += new EventHandler(mButtonConnect_Click);

            this.mComboBoxPort = new ToolStripComboBox()
            {
                ToolTipText = "Port name",
                Size = new Size(300, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.mComboBoxPort.SelectedIndexChanged += new EventHandler(mComboBoxPort_SelectedIndexChanged);

            this.mComboBoxBaudate = new ToolStripComboBox()
            {
                ToolTipText = "Baudrate",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.mComboBoxBaudate.SelectedIndexChanged += new EventHandler(mComboBoxBaudate_SelectedIndexChanged);

            this.mComboBoxParity = new ToolStripComboBox()
            {
                ToolTipText = "Parity",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.mComboBoxParity.SelectedIndexChanged += new EventHandler(mComboBoxParity_SelectedIndexChanged);

            this.mComboBoxStopBits = new ToolStripComboBox()
            {
                ToolTipText = "Stop bits",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.mComboBoxStopBits.SelectedIndexChanged += new EventHandler(mComboBoxStopBits_SelectedIndexChanged);

            this.mMenuItemRtsEnable = new ToolStripMenuItem("RTS");
            this.mMenuItemRtsEnable.Click += new EventHandler(mMenuItemRtsEnable_Click);

            this.mMenuItemDtrEnable = new ToolStripMenuItem("DTR");
            this.mMenuItemDtrEnable.Click += new EventHandler(mMenuItemDtrEnable_Click);

            this.Items.Add(this.mButtonConnect);
            this.mButtonConnect.DropDownItems.AddRange(new ToolStripItem[] { this.mComboBoxPort, this.mComboBoxBaudate, this.mComboBoxParity, this.mComboBoxStopBits, this.mMenuItemRtsEnable, this.mMenuItemDtrEnable });

            this.mWorker = null;
        }

        private void mButtonConnect_Click(object sender, EventArgs e)
        {
            if (this.mWorker != null)
            {
                if (this.mWorker.IsOpen)
                    this.mWorker.Close();
                else
                    this.mWorker.Open();
            }
        }

        private void mComboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portname = String.Empty;
            if (this.mComboBoxPort.SelectedIndex >= 0)
                portname = this.mComboBoxPort.SelectedItem.ToString();
            if (this.mWorker != null)
                this.mWorker.Settings.PortName = portname;
        }

        private void mComboBoxBaudate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int bauds = 57600;
            if (this.mComboBoxBaudate.SelectedIndex >= 0)
                bauds = Convert.ToInt32(this.mComboBoxBaudate.SelectedItem);
            if (this.mWorker != null)
                this.mWorker.Settings.Baudrate = bauds;
        }

        private void mComboBoxParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parity parity = Parity.None;
            if (this.mComboBoxParity.SelectedIndex >= 0)
                parity = (Parity)this.mComboBoxParity.SelectedItem;
            if (this.mWorker != null)
                this.mWorker.Settings.Parity = parity;
        }

        private void mComboBoxStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopBits bits = StopBits.One;
            if (this.mComboBoxStopBits.SelectedIndex >= 0)
                bits = (StopBits)this.mComboBoxStopBits.SelectedItem;
            if (this.mWorker != null)
                this.mWorker.Settings.StopBits = bits;
        }

        private void mMenuItemRtsEnable_Click(object sender, EventArgs e)
        {
            if (this.mWorker != null)
                this.mWorker.Settings.RtsEnable = !this.mWorker.Settings.RtsEnable;
            this.ReflectSettingsToGui();
        }

        private void mMenuItemDtrEnable_Click(object sender, EventArgs e)
        {
            if (this.mWorker != null)
                this.mWorker.Settings.DtrEnable = !this.mWorker.Settings.DtrEnable;
            this.ReflectSettingsToGui();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IWorker Worker
        {
            get
            {
                return this.mWorker;
            }
            set
            {
                this.mWorker = value;
                this.mWorker.OnConnectionChanged += new EventHandler(mWorker_OnConnectionChanged);
                this.SetDefaults();
                this.ReflectSettingsToGui();
            }
        }

        private void mWorker_OnConnectionChanged(object sender, EventArgs e)
        {
            this.ReflectSettingsToGui();
        }

        private void SetDefaults()
        {
            this.mComboBoxPort.Items.Clear();
            this.mComboBoxPort.Items.AddRange(PortSettings.PortsList);

            this.mComboBoxBaudate.Items.Clear();
            foreach (int a in PortSettings.BaudratesList)
                this.mComboBoxBaudate.Items.Add(a); 

            this.mComboBoxParity.Items.Clear();
            foreach (System.IO.Ports.Parity a in PortSettings.ParitiesList)
                this.mComboBoxParity.Items.Add(a);

            this.mComboBoxStopBits.Items.Clear();
            foreach (System.IO.Ports.StopBits a in PortSettings.StopBitsList)
                this.mComboBoxStopBits.Items.Add(a);
        }

        private void ReflectSettingsToGui()
        {
            this.mComboBoxPort.SelectedItem = this.mWorker.Settings.PortName;
            this.mComboBoxBaudate.SelectedItem = this.mWorker.Settings.Baudrate;
            this.mComboBoxParity.SelectedItem = this.mWorker.Settings.Parity;
            this.mComboBoxStopBits.SelectedItem = this.mWorker.Settings.StopBits;

            string state;
            if (this.mWorker.IsOpen)
                state = "Close";
            else
                state = "Open";

            this.mButtonConnect.Text = String.Format(
                "{0}: {1}, {2}, {3}, {4}{5}{6}",
                state,
                this.mWorker.Settings.PortName.ExtractPortName(),
                this.mWorker.Settings.Baudrate,
                this.mWorker.Settings.Parity,
                this.mWorker.Settings.StopBits,
                this.mWorker.Settings.RtsEnable ? ", RTS" : "",
                this.mWorker.Settings.DtrEnable ? ", DTR" : "");

            this.mButtonConnect.ToolTipText = String.Format(
                "{0}: {1}, {2}, {3}, {4}{5}{6}",
                state,
                this.mWorker.Settings.PortName,
                this.mWorker.Settings.Baudrate,
                this.mWorker.Settings.Parity,
                this.mWorker.Settings.StopBits,
                this.mWorker.Settings.RtsEnable ? ", RTS" : "",
                this.mWorker.Settings.DtrEnable ? ", DTR" : "");

            this.mMenuItemRtsEnable.Checked = this.mWorker.Settings.RtsEnable;
            this.mMenuItemDtrEnable.Checked = this.mWorker.Settings.DtrEnable;
        }
    }
}
