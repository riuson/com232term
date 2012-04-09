using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using com232term.Classes;
using System.Drawing;

namespace com232term.Controls.DataSender
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripDataSenderGuiTable : ToolStripControlHost
    {
        public ToolStripDataSenderGuiTable() :
            base(new DataSenderGuiTable())
        {
        }

        public IDataSender Sender { get; set; }
    }
}
