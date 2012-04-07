using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace com232term.Classes.Options
{
    public class LogSettings
    {
        [Flags]
        public enum DisplayFormat
        {
            Hex,
            Ascii,
            Utf8
        }

        public LogSettings()
        {
            this.TransmittedColor = Color.Green;
            this.ReceivedColor = Color.Blue;
            this.SpecialSymbolsColor = Color.Red;
            this.Format = DisplayFormat.Hex;
        }

        public Color TransmittedColor { get; set; }
        public Color ReceivedColor { get; set; }
        public Color SpecialSymbolsColor { get; set; }
        public DisplayFormat Format { get; set; }
    }
}
