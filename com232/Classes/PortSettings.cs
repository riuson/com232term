using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace com232term.Classes
{
    public class PortSettings
    {
        public PortSettings()
        {
            this.PortName = "COM1";
            this.Parity = System.IO.Ports.Parity.None;
            this.Baudrate = 57600;
            this.StopBits = System.IO.Ports.StopBits.One;
        }

        public string PortName { get; set; }
        public Parity Parity { get; set; }
        public int Baudrate { get; set; }
        public StopBits StopBits { get; set; }
    }
}
