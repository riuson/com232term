using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using System.ComponentModel;
using com232term.Classes.Options;
using System.Drawing;
using com232term.Classes.Logger;

namespace com232term.Controls.View
{
    public class ToolStripViewGui : ToolStrip
    {
        private ToolStripButton mButtonShowLastPackets;
        private ToolStripButton mButtonShowStaticPackets;
        private ToolStripButton mButtonAbout;

        public ViewSettings Settings { get; private set; }
        public event EventHandler OnToolBarsVisibleChanging;

        public ToolStripViewGui()
        {
            this.Settings = Options.Instance.ViewOptions;

            this.mButtonShowLastPackets = new ToolStripButton()
            {
                Text = "Last packets",
                Checked = this.Settings.ShowLastPackets
            };
            this.mButtonShowLastPackets.Click += new EventHandler(mButtonShowLastPackets_Click);

            this.mButtonShowStaticPackets = new ToolStripButton()
            {
                Text = "Static packets",
                Checked = this.Settings.ShowStaticPackets
            };
            this.mButtonShowStaticPackets.Click += new EventHandler(mButtonShowStaticPackets_Click);

            this.mButtonAbout = new ToolStripButton()
            {
                Text = "About"
            };
            this.mButtonAbout.Click += new EventHandler(mButtonAbout_Click);

            this.Items.AddRange(new ToolStripItem[] { this.mButtonShowLastPackets, this.mButtonShowStaticPackets, this.mButtonAbout });
        }

        protected override void Dispose(bool disposing)
        {
            Options.Instance.ViewOptions = this.Settings;
            base.Dispose(disposing);
        }

        private void mButtonShowLastPackets_Click(object sender, EventArgs e)
        {
            this.Settings.ShowLastPackets = !this.Settings.ShowLastPackets;
            if (this.OnToolBarsVisibleChanging != null)
                this.OnToolBarsVisibleChanging(this, EventArgs.Empty);
            this.ReflectSettingsToGui();
        }

        private void mButtonShowStaticPackets_Click(object sender, EventArgs e)
        {
            this.Settings.ShowStaticPackets = !this.Settings.ShowStaticPackets;
            if (this.OnToolBarsVisibleChanging != null)
                this.OnToolBarsVisibleChanging(this, EventArgs.Empty);
            this.ReflectSettingsToGui();
        }

        private void mButtonAbout_Click(object sender, EventArgs e)
        {
            using (com232term.Forms.About dialog = new Forms.About())
            {
                dialog.ShowDialog();
            }
        }

        private void ReflectSettingsToGui()
        {
            this.mButtonShowLastPackets.Checked = this.Settings.ShowLastPackets;
            this.mButtonShowStaticPackets.Checked = this.Settings.ShowStaticPackets;
        }
    }
}
