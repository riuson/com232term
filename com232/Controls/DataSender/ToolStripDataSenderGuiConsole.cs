using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;

namespace com232term.Controls.DataSender
{
    public class ToolStripDataSenderGuiConsole : ToolStrip
    {
        private ToolStripComboBoxStretched mComboBoxConsole;
        private ToolStripDropDownButton mButtonLineEnd;
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
            this.mButtonLineEnd.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem(@""),
                new ToolStripMenuItem(@"\r"),
                new ToolStripMenuItem(@"\n"),
                new ToolStripMenuItem(@"\r\n"),
                new ToolStripMenuItem(@"\n\r")
            });

            this.mButtonSend = new ToolStripButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "Send"
            };
            this.mButtonSend.Click += new EventHandler(mButtonSend_Click);

            this.Items.AddRange(new ToolStripItem[] { this.mComboBoxConsole, this.mButtonSend, this.mButtonLineEnd });

            this.mSender = null;
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

        public IDataSender Sender
        {
            get
            {
                return this.mSender;
            }
            set
            {
                this.mSender = value;
                this.mComboBoxConsole.ComboBox.DataSource = this.mSender.Packets;
            }
        }
    }
}
