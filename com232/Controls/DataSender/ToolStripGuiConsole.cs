using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace com232term.Controls.DataSender
{
    public class ToolStripGuiConsole : ToolStrip
    {
        private ToolStripComboBoxStretched mComboBoxConsole;
        private ToolStripDropDownButton mButtonLineEnd;
        private ToolStripButton mButtonSend;

        public ToolStripGuiConsole()
        {
            this.Stretch = true;

            this.mComboBoxConsole = new ToolStripComboBoxStretched();

            this.mButtonLineEnd = new ToolStripDropDownButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "\\n",
            };
            this.mButtonLineEnd.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem(@""),
                new ToolStripMenuItem(@"\r"),
                new ToolStripMenuItem(@"\n"),
                new ToolStripMenuItem(@"\r\n"),
                new ToolStripMenuItem(@"\n\r")
            });

            this.mButtonSend = new ToolStripButton()
            {
                Alignment = ToolStripItemAlignment.Right,
                Text = "Send"
            };

            this.Items.AddRange(new ToolStripItem[] { this.mComboBoxConsole, this.mButtonSend, this.mButtonLineEnd });
        }
    }
}
