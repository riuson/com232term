using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace com232term.Classes
{
    public static class RichTextBoxExt
    {
        public static void AppendText(this RichTextBox box, string text, Color clr)
        {
            if (box != null && text != null && text != String.Empty)
            {
                int len = box.Text.Length;
                box.AppendText(text);
                box.Select(len, text.Length);
                box.SelectionColor = clr;
                box.DeselectAll();
            }
        }
    }
}
