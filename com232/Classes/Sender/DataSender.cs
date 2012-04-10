using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using com232term.Classes.Options;

namespace com232term.Classes.Sender
{
    public class DataSender : IDataSender, IDisposable
    {
        private PacketsHolder mPackets;

        public BindingList<String> Packets { get { return this.mPackets.Packets; } }
        public BindingList<String> PacketsStatic { get { return this.mPackets.PacketsStatic; } }
        public event EventHandler<CallPacketsEditorEventArgs> OnStaticEditorCall;
        public event EventHandler<SendDataEventArgs> OnSendData;
        public SendSettings Settings { get; set; }

        public DataSender()
        {

            this.mPackets = PacketsHolder.Load();

            this.Settings = Options.Options.Instance.SendOptions;
        }

        public void Dispose()
        {
            Options.Options.Instance.SendOptions = this.Settings;
            this.mPackets.Save();
        }

        public void Send(string packet)
        {
            lock (this.Packets)
            {
                if (!this.Packets.Contains(packet))
                    this.Packets.Insert(0, packet);
                else
                {
                    this.Packets.Remove(packet);
                    this.Packets.Insert(0, packet);
                }

                if (this.OnSendData != null)
                {
                    this.OnSendData(this, new SendDataEventArgs(this.ConvertStringToBytes(packet)));
                }
            }
        }

        public void CallStatisPacketsEditor()
        {
            if (this.OnStaticEditorCall != null)
            {
                CallPacketsEditorEventArgs args = new CallPacketsEditorEventArgs(this.PacketsStatic);
                this.OnStaticEditorCall(this, args);
                if (args.Changed)
                {
                    //this.PacketsStatic.RaiseListChangedEvents = false;
                    this.PacketsStatic.Clear();
                    foreach (string packet in args.List)
                    {
                        this.PacketsStatic.Add(packet);
                    }
                    //this.PacketsStatic.RaiseListChangedEvents = true;
                }
            }
        }

        private byte[] ConvertStringToBytes(string value)
        {
            byte[] result = new byte[0];

            // check for hex
            // value must be: "aa bb cc dd 01 0a 50"...
            Regex regHex = new Regex(@"^([0-9a-f]{2}\s*)+$");

            switch (this.Settings.Format)
            {
                case SendSettings.ParseFormats.Auto:
                    {
                        if (regHex.IsMatch(value))
                        {
                            result = this.StringHexToBytes(value);
                        }
                        else
                        {
                            bool nonascii = false;
                            foreach (char c in value)
                            {
                                if ((int)c > 128)
                                {
                                    nonascii = true;
                                    break;
                                }
                            }
                            if (nonascii)
                                result = this.StringToBytes(Encoding.UTF8, value);
                            else
                                result = this.StringToBytes(Encoding.ASCII, value);
                        }
                        break;
                    }
                case SendSettings.ParseFormats.Hex:
                    {
                        if (regHex.IsMatch(value))
                        {
                            result = this.StringHexToBytes(value);
                        }
                    }
                    break;
                case SendSettings.ParseFormats.Ascii:
                    {
                        result = this.StringToBytes(Encoding.ASCII, value);
                        break;
                    }
                case SendSettings.ParseFormats.Utf8:
                    {
                        result = this.StringToBytes(Encoding.UTF8, value);
                        break;
                    }
                default:
                    break;
            }

            return result;
        }

        private byte[] StringHexToBytes(string value)
        {
            string[] strs = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] result = new byte[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                byte a;
                if (byte.TryParse(strs[i], System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a))
                {
                    result[i] = a;
                }
            }
            return result;
        }

        private byte[] StringToBytes(Encoding encoding, string value)
        {
            string str;
            switch (this.Settings.LineEnd)
            {
                case SendSettings.LineEnds.None:
                    str = value;
                    break;
                case SendSettings.LineEnds.N:
                    str = value + "\n";
                    break;
                case SendSettings.LineEnds.R:
                    str = value + "\r";
                    break;
                case SendSettings.LineEnds.NR:
                    str = value + "\n\r";
                    break;
                case SendSettings.LineEnds.RN:
                    str = value + "\r\n";
                    break;
                default:
                    str = value;
                    break;
            }
            byte[] result = encoding.GetBytes(str);
            return result;
        }
    }

    public class CallPacketsEditorEventArgs : EventArgs, IDisposable
    {
        public CallPacketsEditorEventArgs(BindingList<String> sourceList)
        {
            this.List = new BindingList<string>();
            foreach (string packet in sourceList)
            {
                this.List.Add(packet);
            }
            this.List.ListChanged += new ListChangedEventHandler(List_ListChanged);
            this.Changed = false;
        }

        private void List_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.Changed = true;
        }

        public BindingList<String> List { get; private set; }
        public bool Changed { get; private set; }

        public void Dispose()
        {
            this.List.ListChanged -= new ListChangedEventHandler(List_ListChanged);
        }
    }

    public class SendDataEventArgs : EventArgs
    {
        public SendDataEventArgs(byte[] data)
        {
            this.Data = data;
        }

        public byte[] Data { get; private set; }
    }
}
