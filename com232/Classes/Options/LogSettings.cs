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
            Auto = 0x0,
            Hex = 0x1,
            Ascii = 0x2,
            Utf8 = 0x04
        }

        public LogSettings()
        {
            this.TransmittedColor = Color.Green;
            this.ReceivedColor = Color.Blue;
            this.Format = DisplayFormat.Hex;
        }
        public ColorSerialized TransmittedColor { get; set; }
        public ColorSerialized ReceivedColor { get; set; }
        public DisplayFormat Format { get; set; }
    }
}
