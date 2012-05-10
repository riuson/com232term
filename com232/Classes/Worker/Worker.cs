using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using com232term.Classes.Options;

namespace com232term.Classes.Worker
{
    public class Worker : IWorker, IDisposable
    {
        private SerialPortFixed mPort;
        private bool mDataReceived;
        private bool mPortOpenedLastState;
        private WorkerThread mThread;

        public PortSettings Settings { get; private set; }
        public event EventHandler<DataLogEventArgs> OnDataLog;
        public event EventHandler<MessageLogEventArgs> OnMessageLog;
        public event EventHandler OnConnectionChanged;

        public Worker()
        {
            this.Settings = Options.Options.Instance.PortOptions;
            this.mPort = new SerialPortFixed(this.Settings.PortName, this.Settings.Baudrate, this.Settings.Parity, 8, this.Settings.StopBits);
            this.mThread = new WorkerThread();
            this.mDataReceived = false;
            this.mPortOpenedLastState = false;

            this.mThread.Idle = this.IdleMethod;
        }

        public void Dispose()
        {
            this.mThread.Dispose();
            Options.Options.Instance.PortOptions = this.Settings;
        }

        public void IdleMethod()
        {
            if (this.CheckPortSettings())
            {
                if (this.mDataReceived)
                {
                    this.mDataReceived = false;

                    byte[] readedBytes = null;
                    lock (this.mPort)
                    {
                        if (this.mPort.IsOpen)
                        {
                            if (this.mPort.BytesToRead > 0)
                            {
                                Thread.Sleep(100);
                                readedBytes = new byte[this.mPort.BytesToRead];
                                this.mPort.Read(readedBytes, 0, readedBytes.Length);
                            }
                        }
                    }
                    if (readedBytes != null && readedBytes.Length > 0)
                    {
                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnDataLog != null)
                                this.OnDataLog(this, new DataLogEventArgs(Direction.Received, readedBytes));
                        });
                    }
                }
            }
        }

        private bool CheckPortSettings()
        {
            lock (this.mPort)
            {
                if (this.Settings.PortName == String.Empty)
                    return false;

                if (this.mPort.PortName != this.Settings.PortName.ExtractPortName() ||
                    this.mPort.BaudRate != this.Settings.Baudrate ||
                    this.mPort.Parity != this.Settings.Parity ||
                    this.mPort.StopBits != this.Settings.StopBits ||
                    this.mPort.RtsEnable != this.Settings.RtsEnable ||
                    this.mPort.DtrEnable != this.Settings.DtrEnable)
                {
                    bool opened = this.mPort.IsOpen;
                    if (opened)
                        this.mPort.Close();

                    this.mPort.PortName = this.Settings.PortName.ExtractPortName();
                    this.mPort.BaudRate = this.Settings.Baudrate;
                    this.mPort.Parity = this.Settings.Parity;
                    this.mPort.StopBits = this.Settings.StopBits;
                    this.mPort.RtsEnable = this.Settings.RtsEnable;
                    this.mPort.DtrEnable = this.Settings.DtrEnable;

                    if (opened)
                        this.mPort.Open();

                    this.mThread.EnqueueOutgoingTask(delegate()
                    {
                        this.LogMessage(String.Format("Port settins changed to: {0}, {1}, {2}, {3}{4}{5}",
                            this.Settings.PortName,
                            this.Settings.Baudrate,
                            this.Settings.Parity,
                            this.Settings.StopBits,
                            this.Settings.RtsEnable ? ", RTS" : "",
                            this.Settings.DtrEnable ? ", DTR" : ""));
                    });
                }

                if (this.mPortOpenedLastState != this.mPort.IsOpen)
                {
                    this.mPortOpenedLastState = this.mPort.IsOpen;

                    if (this.mPortOpenedLastState)
                    {
                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnConnectionChanged != null)
                                this.OnConnectionChanged(this, EventArgs.Empty);

                            this.LogMessage(String.Format("Port opened: {0}, {1}, {2}, {3}{4}{5}",
                                this.Settings.PortName,
                                this.Settings.Baudrate,
                                this.Settings.Parity,
                                this.Settings.StopBits,
                                this.Settings.RtsEnable ? ", RTS" : "",
                                this.Settings.DtrEnable ? ", DTR" : ""));
                        });
                    }
                    else
                    {
                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnConnectionChanged != null)
                                this.OnConnectionChanged(this, EventArgs.Empty);

                            this.LogMessage(String.Format("Port closed: {0}",
                                this.Settings.PortName));
                        });
                    }
                }

            }
            return true;
        }

        private void SendInternal(string msg)
        {
            this.mPort.Write(msg);
        }

        private void mPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.mDataReceived = true;
        }

        public void Open()
        {
            this.mThread.EnqueueIncomingTask(delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.Open();
                    if (this.mPort.IsOpen)
                    {
                        this.mPort.DataReceived += new SerialDataReceivedEventHandler(mPort_DataReceived);
                    }
                    else
                    {
                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnConnectionChanged != null)
                                this.OnConnectionChanged(this, EventArgs.Empty);

                            this.LogMessage(String.Format("Unable to open port: {0}, {1}, {2}, {3}",
                                this.Settings.PortName,
                                this.Settings.Baudrate,
                                this.Settings.Parity,
                                this.Settings.StopBits));
                        });

                    }
                }
            });
        }

        public void Close()
        {
            this.mThread.EnqueueIncomingTask(delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.DataReceived -= new SerialDataReceivedEventHandler(mPort_DataReceived);
                    this.mPort.Close();
                }
            });
        }

        public void Send(byte []value)
        {
            this.mThread.EnqueueIncomingTask(delegate()
            {
                lock (this.mPort)
                {
                    if (this.mPort.IsOpen)
                    {
                        this.mPort.Write(value, 0, value.Length);

                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnDataLog != null)
                                this.OnDataLog(this, new DataLogEventArgs(Direction.Transmitted, value));
                        });
                    }
                    else
                    {
                        this.mThread.EnqueueOutgoingTask(delegate()
                        {
                            this.LogMessage("Unable to send data: port closed!");
                        });
                    }
                }
            });
        }

        public bool IsOpen
        {
            get
            {
                bool result = false;
                lock (this.mPort)
                {
                    result = this.mPort.IsOpen;
                }
                return result;
            }
        }

        private void LogMessage(string message)
        {
            if (this.OnMessageLog != null)
                this.OnMessageLog(this, new MessageLogEventArgs(message));
        }
    }

    public class DataLogEventArgs : EventArgs
    {
        public DataLogEventArgs(Direction direction, byte []value)
        {
            this.DataDirection = direction;
            this.Time = DateTime.Now;
            this.Value = value;
        }

        public Direction DataDirection { get; private set; }
        public DateTime Time { get; private set; }
        public byte[] Value { get; private set; }
    }
    public enum Direction
    {
        Transmitted,
        Received
    }

    public class MessageLogEventArgs : EventArgs
    {
        public MessageLogEventArgs(string message)
        {
            this.Message = message;
            this.Time = DateTime.Now;
        }

        public string Message { get; private set; }
        public DateTime Time { get; private set; }
    }
}
