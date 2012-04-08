using System;
using System.Collections.Generic;
using System.Text;
using com232term.Classes.Options;
using System.Windows.Forms;

namespace com232term.Classes
{
    public class Logger
    {
        public static LogSettings.DisplayFormat[] DisplayFormats
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

        private object mSync;
        private RichTextBox mBox;
        private LogSettings mLogOptions;

        public Logger(RichTextBox box)
        {
            this.mSync = new object();
            this.mLogOptions = new  LogSettings();
            this.mBox = box;
        }

        public LogSettings LogOptions
        {
            get
            {
                lock (this.mSync)
                {
                    return this.mLogOptions;
                }
            }
            set
            {
                lock (this.mSync)
                {
                    this.mLogOptions = value;
                }
            }
        }

        public void Log(byte[] array)
        {
            this.mBox.AppendText(ArraysConverter.FromArray(array));
            this.mBox.AppendText("\n");
        }
    }
}
