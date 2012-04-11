using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;

namespace com232term
{
    /// <summary>
    /// Workaround for serial port crashes then device disconnected
    /// </summary>
    // http://connect.microsoft.com/VisualStudio/feedback/details/140018/serialport-crashes-after-disconnect-of-usb-com-port
    public class SerialPortFixed : SerialPort
    {
        public SerialPortFixed(System.ComponentModel.IContainer cont)
            : base(cont)
        {
        }

        public SerialPortFixed(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {

        }

        public new void Open()
        {
            try
            {
                base.Open();
                /*
                 * Because of the issue with the FTDI USB serial device,
                 * the call to the stream's finalize is suppressed
                 * it will be un-suppressed in Dispose if the stream
                 * is still good
                 */
                GC.SuppressFinalize(BaseStream);
            }
            catch
            {
            }
        }

        public new void Dispose()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (base.Container != null))
            {
                base.Container.Dispose();
            }

            try
            {
                /*
                 * Bbecause of the issue with the FTDI USB serial device,
                 * the call to the stream's finalize is suppressed
                 * an attempt to un-suppress the stream's finalize is made
                 * here, but if it fails, the exception is caught and
                 * ignored
                 */
                GC.ReRegisterForFinalize(BaseStream);
            }
            catch
            {
            }
            base.Dispose(disposing);
        }

        public static string[] Ports
        {
            get
            {
                List<string> result = new List<string>();
                try
                {
                    // select serial ports by guid
                    using (ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity where ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}'"))
                    {
                        using (ManagementObjectCollection ports = mos.Get())
                        {
                            // get ports list by standard SerialPort.GetPortNames() method
                            List<string> serialPorts = new List<string>();
                            Regex regex = new Regex(@"(?<=COM)\d*", RegexOptions.IgnoreCase);
                            foreach(string port in SerialPort.GetPortNames())
                            {
                                if (regex.IsMatch(port))
                                    serialPorts.Add(String.Format("COM{0}", regex.Match(port).Value));
                            }

                            // get serial ports from Management, what detected  in standard method
                            Regex regex2 = new Regex(@"(?<=\()COM\d*(?=\))", RegexOptions.IgnoreCase);
                            foreach (object obj in ports)
                            {
                                if (obj is ManagementObject)
                                {
                                    ManagementObject port = (ManagementObject)obj;
                                    {
                                        string portname = Convert.ToString(port["Name"]);
                                        if (regex2.IsMatch(portname))
                                        {
                                            string portid = regex2.Match(portname).Value;
                                            if (serialPorts.Contains(portid))
                                                result.Add(portname);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    result.Clear();
                    foreach(string port in SerialPort.GetPortNames())
                    {
                        result.Add(String.Format("Serial Port ({0})", port));
                    }
                }
                return result.ToArray();
            }
        }

    }
    /// <summary>
    /// Extension to extract port name from string
    /// </summary>
    public static class StringPortNameExtension
    {
        public static string ExtractPortName(this string value)
        {
            if (value == null)
                return String.Empty;

            Regex regex = new Regex(@"(?<=\()COM\d*(?=\))", RegexOptions.IgnoreCase);
            if (regex.IsMatch(value))
                return regex.Match(value).Value;

            regex = new Regex(@"COM\d*", RegexOptions.IgnoreCase);
            if (regex.IsMatch(value))
                return regex.Match(value).Value;

            return String.Empty;
        }
    }
}
