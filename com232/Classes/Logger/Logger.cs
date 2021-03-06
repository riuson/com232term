﻿using System;
using System.Collections.Generic;
using System.Text;
using com232term.Classes.Options;
using System.Windows.Forms;
using System.Drawing;
using com232term.Classes.Worker;

namespace com232term.Classes.Logger
{
    public class Logger : ILogger, IDisposable
    {
        private object mSync;
        private RichTextBox mBox;
        public LogSettings Settings { get; private set; }

        public Logger(RichTextBox box)
        {
            this.mSync = new object();
            this.Settings = Options.Options.Instance.LogOptions;
            this.mBox = box;
        }

        public void Dispose()
        {
            Options.Options.Instance.LogOptions = this.Settings;
        }

        public void LogData(DateTime time, Direction direction, byte[] array)
        {
            Color foreColor;
            if (direction == Direction.Received)
                foreColor = this.Settings.ReceivedColor;
            else
                foreColor = this.Settings.TransmittedColor;


            // time
            this.mBox.AppendText(String.Format("\n"));

            if (direction == Direction.Received)
                this.mBox.AppendText(">>> ", this.Settings.TimeColor);
            else
                this.mBox.AppendText("<<< ", this.Settings.TimeColor);

            this.mBox.AppendText(String.Format("{0:yyyy.MM.dd - HH:mm:ss.ffff}", time), this.Settings.TimeColor);
            this.mBox.AppendText(String.Format("\n"));

            LogSettings.DisplayFormat format = Options.Options.Instance.LogOptions.Format;

            if ((format & LogSettings.DisplayFormat.Auto) == LogSettings.DisplayFormat.Auto)
            {
                bool isHex = false;
                bool isAscii = true;
                foreach (byte b in array)
                {
                    if ((b < 32) && (b != 13) && (b != 10))
                    {
                        isHex = true;
                        isAscii = false;
                        break;
                    }
                    if (b > 128)
                        isAscii = false;
                }

                if (isHex)
                    format = LogSettings.DisplayFormat.Hex;
                else
                {
                    if (isAscii)
                        format = LogSettings.DisplayFormat.Ascii;
                    else
                        format = LogSettings.DisplayFormat.Utf8;
                }
            }

            if ((format & LogSettings.DisplayFormat.Hex) == LogSettings.DisplayFormat.Hex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in array)
                {
                    sb.AppendFormat("{0:X2} ", b);
                }
                sb.Append("\n");
                this.mBox.AppendText(sb.ToString(), foreColor);
            }

            if ((format & LogSettings.DisplayFormat.Ascii) == LogSettings.DisplayFormat.Ascii)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in (Encoding.ASCII.GetString(array)))
                {
                    if (!Char.IsControl(c) || (c == '\r') || (c == '\n'))
                        sb.Append(c);
                    else
                    {
                        int a = (int)c;
                        sb.AppendFormat("'\\x{0:x4}'", a);
                    }
                }

                sb.Append("\n");
                this.mBox.AppendText(sb.ToString(), foreColor);
            }

            if ((format & LogSettings.DisplayFormat.Utf8) == LogSettings.DisplayFormat.Utf8)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in (Encoding.UTF8.GetString(array)))
                {
                    if (!Char.IsControl(c) || (c == '\r') || (c == '\n'))
                        sb.Append(c);
                    else
                    {
                        int a = (int)c;
                        sb.AppendFormat("'\\x{0:x4}'", a);
                    }
                }

                sb.Append("\n");
                this.mBox.AppendText(sb.ToString(), foreColor);
            }
        }

        public void LogMessage(DateTime time, string message)
        {
            Color foreColor;
            foreColor = this.Settings.SystemColor;


            // time
            this.mBox.AppendText(String.Format("\n"));
            this.mBox.AppendText(String.Format("{0:yyyy.MM.dd - HH:mm:ss.ffff}", time), this.Settings.TimeColor);
            this.mBox.AppendText(String.Format("\n"));

            this.mBox.AppendText(message, foreColor);
            this.mBox.AppendText("\n");
        }

        public void Clear()
        {
            this.mBox.Clear();
        }
    }
}
