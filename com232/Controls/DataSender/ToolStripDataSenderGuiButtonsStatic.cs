using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using com232term.Classes;
using com232term.Classes.Sender;

namespace com232term.Controls.DataSender
{
    public class ToolStripDataSenderGuiButtonsStatic  : ToolStrip
    {
        private IDataSender mSender;

        public ToolStripDataSenderGuiButtonsStatic()
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
                    this.mSender.PacketsStatic.ListChanged -= new ListChangedEventHandler(Packets_ListChanged);
                this.ClearButtons();

                this.mSender = value;

                if (this.mSender != null)
                {
                    this.mSender.PacketsStatic.ListChanged += new ListChangedEventHandler(Packets_ListChanged);

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
            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit...");
            itemEdit.Click += new EventHandler(itemEdit_Click);
            this.Items.Add(itemEdit);

            for (int i = 0; i < this.mSender.PacketsStatic.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(this.mSender.PacketsStatic[i]);
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

        private void itemEdit_Click(object sender, EventArgs e)
        {
            if (this.mSender != null)
            {
                this.mSender.CallStaticPacketsEditor();
            }
        }
    }
}
