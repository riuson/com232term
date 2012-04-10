using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using com232term.Classes;
using System.ComponentModel;
using com232term.Classes.Options;
using System.Drawing;

namespace com232term.Controls.DataSender
{
    public class ToolStripLogsGui : ToolStrip
    {
        private ToolStripDropDownButton mButtonFormat;
        private ToolStripDropDownButton mButtonColors;
        private ToolStripButton mButtonClear;
        private ILogger mLogger;
        public event EventHandler OnClear;

        public ToolStripLogsGui()
        {
            this.mButtonFormat = new ToolStripDropDownButton()
            {
                Text = "Format",
            };

            this.mButtonColors = new ToolStripDropDownButton()
            {
                Text = "Colors",
            };

            this.mButtonClear = new ToolStripButton()
            {
                Text = "Clear"
            };
            this.mButtonClear.Click += new EventHandler(mButtonClear_Click);

            this.Items.AddRange(new ToolStripItem[] { this.mButtonFormat, this.mButtonColors, this.mButtonClear });

            this.mLogger = null;
        }

        private void itemDisplayFormat_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                LogSettings.DisplayFormat formatItem = (LogSettings.DisplayFormat)menuItem.Tag;

                LogSettings.DisplayFormat format = this.mLogger.Settings.Format;

                // if selected any other, remove Auto
                if (formatItem != LogSettings.DisplayFormat.Auto)
                {
                    format &= ~LogSettings.DisplayFormat.Auto;

                    if ((format & formatItem) == formatItem)
                        format &= ~formatItem;
                    else
                        format |= formatItem;
                }
                else
                    format = LogSettings.DisplayFormat.Auto;

                this.mLogger.Settings.Format = format;

                this.ReflectSettingsToGui();
            }
        }

        private void itemColors_Click(object sender, EventArgs e)
        {
            Color value = Color.Black;
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                string parameter = menuItem.Tag.ToString();

                if (parameter == "TransmittedColor")
                    value = this.mLogger.Settings.TransmittedColor;
                if (parameter == "ReceivedColor")
                    value = this.mLogger.Settings.ReceivedColor;
                if (parameter == "SystemColor")
                    value = this.mLogger.Settings.SystemColor;
                if (parameter == "TimeColor")
                    value = this.mLogger.Settings.TimeColor;


                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.Color = value;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        value = dialog.Color;
                        if (parameter == "TransmittedColor")
                            this.mLogger.Settings.TransmittedColor = value;
                        if (parameter == "ReceivedColor")
                            this.mLogger.Settings.ReceivedColor = value;
                        if (parameter == "SystemColor")
                            this.mLogger.Settings.SystemColor = value;
                        if (parameter == "TimeColor")
                            this.mLogger.Settings.TimeColor = value;
                    }
                }
                this.ReflectSettingsToGui();
            }
            
        }

        private void mButtonClear_Click(object sender, EventArgs e)
        {
            if (this.OnClear != null)
                this.OnClear(this, EventArgs.Empty);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILogger Logger
        {
            get
            {
                return this.mLogger;
            }
            set
            {
                this.mLogger = value;
                this.SetDefaults();
                this.ReflectSettingsToGui();
            }
        }

        private void SetDefaults()
        {
            foreach (LogSettings.DisplayFormat a in LogSettings.DisplayFormatsList)
            {
                ToolStripMenuItem item = new ToolStripMenuItem() { Tag = a, Text = a.ToString(), CheckOnClick = true };
                item.Click += new EventHandler(itemDisplayFormat_Click);
                this.mButtonFormat.DropDownItems.Add(item);
            }

            ToolStripMenuItem colorsItem = new ToolStripMenuItem("Transmitted");
            colorsItem.Tag = "TransmittedColor";
            colorsItem.Click += new EventHandler(itemColors_Click);
            this.mButtonColors.DropDownItems.Add(colorsItem);

            colorsItem = new ToolStripMenuItem("Received");
            colorsItem.Tag = "ReceivedColor";
            colorsItem.Click += new EventHandler(itemColors_Click);
            this.mButtonColors.DropDownItems.Add(colorsItem);

            colorsItem = new ToolStripMenuItem("System");
            colorsItem.Tag = "SystemColor";
            colorsItem.Click += new EventHandler(itemColors_Click);
            this.mButtonColors.DropDownItems.Add(colorsItem);
            
            colorsItem = new ToolStripMenuItem("Time");
            colorsItem.Tag = "TimeColor";
            colorsItem.Click += new EventHandler(itemColors_Click);
            this.mButtonColors.DropDownItems.Add(colorsItem);
        }

        private void ReflectSettingsToGui()
        {
            foreach (ToolStripItem item in this.mButtonFormat.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    LogSettings.DisplayFormat format = (LogSettings.DisplayFormat)menuItem.Tag;
                    menuItem.Checked = (this.mLogger.Settings.Format & format) == format;
                }
            }
            this.mButtonFormat.Text = "Format: " + this.mLogger.Settings.Format.ToString();

            foreach (ToolStripItem item in this.mButtonColors.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    string parameter = menuItem.Tag.ToString();

                    if (parameter == "TransmittedColor")
                        menuItem.ForeColor = this.mLogger.Settings.TransmittedColor;
                    if (parameter == "ReceivedColor")
                        menuItem.ForeColor = this.mLogger.Settings.ReceivedColor;
                    if (parameter == "SystemColor")
                        menuItem.ForeColor = this.mLogger.Settings.SystemColor;
                    if (parameter == "TimeColor")
                        menuItem.ForeColor = this.mLogger.Settings.TimeColor;
                }
            }
        }
    }
}
