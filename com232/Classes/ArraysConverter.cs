using System;
using System.Collections.Generic;
using System.Text;
using com232term.Classes.Options;

namespace com232term.Classes
{
    public static class ArraysConverter
    {
        /// <summary>
        /// Converts
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromArray(byte[] value)
        {
            StringBuilder result = new StringBuilder();

            LogSettings.DisplayFormat format = Options.Options.Instance.LogOptions.Format;

            if ((format & LogSettings.DisplayFormat.Hex) == LogSettings.DisplayFormat.Hex)
            {
                foreach (byte b in value)
                {
                    result.AppendFormat("{0:X2} ", b);
                }
                result.Append("\n");
            }

            if ((format & LogSettings.DisplayFormat.Ascii) == LogSettings.DisplayFormat.Ascii)
            {
                result.Append(Encoding.ASCII.GetString(value));
                result.Append("\n");
            }

            if ((format & LogSettings.DisplayFormat.Utf8) == LogSettings.DisplayFormat.Utf8)
            {
                result.Append(Encoding.UTF8.GetString(value));
                result.Append("\n");
            }

            return result.ToString();
        }

        //public static byte[] ToArray(string value)
        //{
        //}
    }
}
