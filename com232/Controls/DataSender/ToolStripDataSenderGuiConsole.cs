using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using System.ComponentModel;
using com232term.Classes.Options;
using com232term.Classes.Sender;

namespace com232term.Controls.DataSender
{
    public class ToolStripDataSenderGuiConsole : ToolStrip
    {
        private ToolStripComboBoxStretched mComboBoxConsole;
        private ToolStripDropDownButton mButtonLineEnd;
        private ToolStripDropDownButton mButtonFormat;
        private ToolStripButton mButtonSend;
        private IDataSender mSender;

        public ToolStripDataSenderGuiConsole()
        {
            this.Stretch = true;

            this.mComboBoxConsole = new ToolStripComboBoxStretched();
            this.mComboBoxConsole.ComboBox.KeyUp += new KeyEventHandler(ComboBox_KeyUp);

            this.mButtonLineEnd = new ToolStripDropDownButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "\\n",
            };

            this.mButtonFormat = new ToolStripDropDownButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "None",
            };

            this.mButtonSend = new ToolStripButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "Send"
            };
            this.mButtonSend.Click += new EventHandler(mButtonSend_Click);

            this.Items.AddRange(new ToolStripItem[] { this.mComboBoxConsole, this.mButtonSend, this.mButtonLineEnd, this.mButtonFormat });

            this.mSender = null;
        }

        private void itemLineEnd_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                com232term.Classes.Options.SendSettings.LineEnds end = (com232term.Classes.Options.SendSettings.LineEnds)menuItem.Tag;
                this.mSender.Settings.LineEnd = end;
                this.ReflectSettingsToGui();
            }
        }

        private void itemParseFormat_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                com232term.Classes.Options.SendSettings.ParseFormats format = (com232term.Classes.Options.SendSettings.ParseFormats)menuItem.Tag;
                this.mSender.Settings.Format = format;
                this.ReflectSettingsToGui();
            }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.CallSender();
        }

        private void mButtonSend_Click(object sender, EventArgs e)
        {
            this.CallSender();
        }

        private void CallSender()
        {
            if (this.mSender != null)
            {
                if (this.mComboBoxConsole.ComboBox.SelectedIndex >= 0)
                {
                    this.mSender.Send(this.mComboBoxConsole.ComboBox.SelectedItem.ToString());
                }
                else
                {
                    this.mSender.Send(this.mComboBoxConsole.ComboBox.Text);
                }
                if (this.mComboBoxConsole.ComboBox.Items.Count > 0)
                    this.mComboBoxConsole.ComboBox.SelectedIndex = 0;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataSender Sender
        {
            get
            {
                return this.mSender;
            }
            set
            {
                this.mSender = value;
                this.SetDefaults();
                this.ReflectSettingsToGui();
            }
        }

        private void SetDefaults()
        {
            foreach (com232term.Classes.Options.SendSettings.LineEnds a in SendSettings.LineEndsList)
            {
                ToolStripMenuItem item = new ToolStripMenuItem() { Tag = a, Text = a.ToString(), CheckOnClick = true };
                item.Click += new EventHandler(itemLineEnd_Click);
                this.mButtonLineEnd.DropDownItems.Add(item);
            }

            foreach (com232term.Classes.Options.SendSettings.ParseFormats a in SendSettings.ParseFormatsList)
            {
                ToolStripMenuItem item = new ToolStripMenuItem() { Tag = a, Text = a.ToString(), CheckOnClick = true };
                item.Click += new EventHandler(itemParseFormat_Click);
                this.mButtonFormat.DropDownItems.Add(item);
            }

            this.mComboBoxConsole.ComboBox.DataSource = this.mSender.Packets;
        }

        private void ReflectSettingsToGui()
        {
            foreach (ToolStripItem item in this.mButtonLineEnd.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    com232term.Classes.Options.SendSettings.LineEnds end = (com232term.Classes.Options.SendSettings.LineEnds)menuItem.Tag;
                    menuItem.Checked = (end == this.mSender.Settings.LineEnd);
                }
            }
            this.mButtonLineEnd.Text = this.mSender.Settings.LineEnd.ToString();

            foreach (ToolStripItem item in this.mButtonFormat.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    com232term.Classes.Options.SendSettings.ParseFormats end = (com232term.Classes.Options.SendSettings.ParseFormats)menuItem.Tag;
                    menuItem.Checked = (end == this.mSender.Settings.Format);
                }
            }
            this.mButtonFormat.Text = this.mSender.Settings.Format.ToString();
        }
    }
}
