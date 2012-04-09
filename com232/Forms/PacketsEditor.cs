using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace com232term.Forms
{
    public partial class PacketsEditor : Form
    {
        private BindingList<String> mSourceList;
        private BindingList<StringValue> mList;

        public PacketsEditor(BindingList<String> list)
        {
            InitializeComponent();

            this.mSourceList = list;

            this.mList = new BindingList<StringValue>();
            this.mList.AllowNew = true;
            foreach (string packet in list)
            {
                this.mList.Add(packet);
            }
            this.dgvPackets.DataSource = this.mList;
        }

        private void PacketsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this.mSourceList.Clear();
                foreach (string packet in this.mList)
                {
                    this.mSourceList.Add(packet);
                }
            }
        }
    }

    /// <summary>
    /// Workaround to bind BindingList strings to DataGridView
    /// o_O
    /// </summary>
    public class StringValue
    {
        private string mValue;

        public StringValue()
        {
            this.mValue = String.Empty;
        }

        public StringValue(string value)
        {
            this.Value = value;
        }
        public string Value
        {
            get { return this.mValue; }
            set { this.mValue = value; }
        }

        public static implicit operator string(StringValue value)
        {
            return value.Value;
        }
        public static implicit operator StringValue(string value)
        {
            return new StringValue(value);
        }
    }

}
