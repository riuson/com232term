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

            string version = com232term.Properties.Resources.version_included;
            // possible value: "git-commit-info c7f662b Wed Apr 11 19:22:24 2012 +0600"
            version = version.Trim();
            if (version.StartsWith("git-commit-info") && version.Length > 25)
            {
                string hash = version.Substring("git-commit-info ".Length, 7);
                string date = version.Substring(version.IndexOf(hash) + hash.Length + 1);
                this.lVersion.Text = String.Format("Version: {0}\n{1}", hash, date);
            }
            else
                this.lVersion.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
