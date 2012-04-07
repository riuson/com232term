using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace com232term.Classes
{
    public class Worker
    {
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

        public Worker()
        {
            this.Settings = new PortSettings();
        }

        public void SetPortName(string portname, int baudrate, Parity parity, StopBits stopbits)
        {
            this.Settings.PortName = portname;
            this.Settings.Baudrate = baudrate;
            this.Settings.Parity = parity;
            this.Settings.StopBits = stopbits;

            if (this.OnSettingsChanged != null)
                this.OnSettingsChanged(this, EventArgs.Empty);
        }

        public PortSettings Settings { get; private set; }

        public event EventHandler OnSettingsChanged;
    }
}
