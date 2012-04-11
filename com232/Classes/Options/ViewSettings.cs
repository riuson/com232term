using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace com232term.Classes.Options
{
    public class ViewSettings
    {
        public ViewSettings()
        {
            this.ShowLastPackets = true;
            this.ShowStaticPackets = true;
        }

        public bool ShowLastPackets { get; set; }
        public bool ShowStaticPackets { get; set; }
    }
}
