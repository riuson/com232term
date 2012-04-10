using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace com232term.Classes.Options
{
    public class SendSettings
    {
        [TypeConverter(typeof(EnumConverter))]
        public enum SendLineEnd
        {
            [Description("None")]
            None,
            [Description(@"\n")]
            N,
            [Description(@"\r")]
            R,
            [Description(@"\n\r")]
            NR,
            [Description(@"\r\n")]
            RN
        }

        public enum ParseFormat
        {
            Auto,
            Hex,
            Ascii,
            Utf8
        }

        public SendSettings()
        {
            this.LineEnd = SendLineEnd.None;
            this.Format = ParseFormat.Auto;
        }

        public SendLineEnd LineEnd { get; set; }
        public ParseFormat Format { get; set; }
    }
}
