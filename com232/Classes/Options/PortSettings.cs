using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace com232term.Classes.Options
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

        public override string ToString()
        {
            return String.Format(
                "{0}, {1}, {2}, {3}",
                this.PortName,
                this.Baudrate,
                this.Parity,
                this.StopBits);
        }

        public static string[] PortsList
        {
            get
            {
                return SerialPortFixed.Ports;
            }
        }

        public static int[] BaudratesList
        {
            get
            {
                return new int[]{
                    1200,
                    2400,
                    4800,
                    9600,
                    19200,
                    38400,
                    57600,
                    115200};
            }
        }

        public static Parity[] ParitiesList
        {
            get
            {
                List<Parity> result = new List<Parity>();
                foreach (Parity a in Enum.GetValues(typeof(Parity)))
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
        }

        public static StopBits[] StopBitsList
        {
            get
            {
                List<StopBits> result = new List<StopBits>();
                foreach (StopBits a in Enum.GetValues(typeof(StopBits)))
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
        }
    }
}
