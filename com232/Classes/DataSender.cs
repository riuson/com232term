using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace com232term.Classes
{
    public class DataSender : IDataSender
    {
        public BindingList<String> Packets { get; private set; }

        public DataSender()
        {
            this.Packets = new BindingList<string>();
            this.Packets.Add("1");
            this.Packets.Add("2");
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
            }
        }
    }
}
