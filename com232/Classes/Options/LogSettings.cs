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
            Auto = 0x1,
            Hex = 0x2,
            Ascii = 0x4,
            Utf8 = 0x08
        }

        private DisplayFormat mFormat;

        public LogSettings()
        {
            this.TransmittedColor = Color.Green;
            this.ReceivedColor = Color.Blue;
            this.SystemColor = Color.Silver;
            this.TimeColor = Color.Navy;
            this.mFormat = DisplayFormat.Hex;
        }
        public ColorSerialized TransmittedColor { get; set; }
        public ColorSerialized ReceivedColor { get; set; }
        public ColorSerialized SystemColor { get; set; }
        public ColorSerialized TimeColor { get; set; }
        public DisplayFormat Format
        {
            get
            {
                return this.mFormat;
            }
            set
            {
                // remove Auto if any of others selected
                this.mFormat = value;
            }
        }

        public static LogSettings.DisplayFormat[] DisplayFormatsList
        {
            get
            {
                List<LogSettings.DisplayFormat> result = new List<LogSettings.DisplayFormat>();
                foreach (LogSettings.DisplayFormat a in Enum.GetValues(typeof(LogSettings.DisplayFormat)))
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
        }
    }
}
