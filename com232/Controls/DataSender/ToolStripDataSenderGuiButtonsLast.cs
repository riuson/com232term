using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using com232term.Classes;

namespace com232term.Controls.DataSender
{
    public class ToolStripDataSenderGuiButtonsLast  : ToolStrip
    {
        private IDataSender mSender;

        public ToolStripDataSenderGuiButtonsLast()
        {
            this.Stretch = true;
        
            this.mSender = null;
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
                if (this.mSender != null)
                    this.mSender.Packets.ListChanged -= new ListChangedEventHandler(Packets_ListChanged);
                this.ClearButtons();

                this.mSender = value;

                if (this.mSender != null)
                {
                    this.mSender.Packets.ListChanged += new ListChangedEventHandler(Packets_ListChanged);

                    this.SuspendLayout();

                    this.BuildButtons();

                    this.PerformLayout();
                    this.ResumeLayout();
                }
            }
        }

        private void Packets_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SuspendLayout();

            this.ClearButtons();
            this.BuildButtons();

            this.PerformLayout();
            this.ResumeLayout();
        }

        private void ClearButtons()
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem item = this.Items[i];
                item.Click -= new EventHandler(item_Click);
                this.Items.Remove(item);
                item.Dispose();
            }
        }

        private void BuildButtons()
        {
            for (int i = 0; i < 10 && i < this.mSender.Packets.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(this.mSender.Packets[i]);
                item.Click += new EventHandler(item_Click);
                this.Items.Add(item);
            }
        }

        private void item_Click(object sender, EventArgs e)
        {
            if (this.mSender != null)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    this.mSender.Send(item.Text);
                }
            }
        }
    }
}
