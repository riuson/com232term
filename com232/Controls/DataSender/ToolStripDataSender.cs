using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace com232term.Controls.DataSender
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripDataSender : ToolStripControlHost
    {
        public ToolStripDataSender() :
            base(new DataSender())
        {
        }
    }
}
