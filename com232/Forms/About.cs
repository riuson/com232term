using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace com232term.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            this.linkLabel1.Links.Add(15, 35, "http://github.com/riuson/com232term");
            this.linkLabel1.Links.Add(52, 35, "http://code.google.com/p/com232term");
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        } 
    }
}
