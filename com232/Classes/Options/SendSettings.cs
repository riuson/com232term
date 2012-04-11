using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace com232term.Classes.Options
{
    public class SendSettings
    {
        [TypeConverter(typeof(EnumTypeConverter))]
        public enum LineEnds
        {
            [Description("none")]
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

        public enum ParseFormats
        {
            Auto,
            Hex,
            Ascii,
            Utf8
        }

        public SendSettings()
        {
            this.LineEnd = LineEnds.None;
            this.Format = ParseFormats.Auto;
        }

        public LineEnds LineEnd { get; set; }
        public ParseFormats Format { get; set; }

        public static LineEnds[] LineEndsList
        {
            get
            {
                List<LineEnds> result = new List<LineEnds>();
                foreach (LineEnds a in Enum.GetValues(typeof(LineEnds)))
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
        }

        public static ParseFormats[] ParseFormatsList
        {
            get
            {
                List<ParseFormats> result = new List<ParseFormats>();
                foreach (ParseFormats a in Enum.GetValues(typeof(ParseFormats)))
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
        }
    }
}
