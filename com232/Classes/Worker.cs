using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using com232term.Classes.Options;

namespace com232term.Classes
{
    public class Worker : IWorker, IDisposable
    {
        private Thread mThread;
        private bool mNeedStop;
        private AutoResetEvent mStopEvent;
        private Queue<ThreadedMethod> mIncomingTasksQueue;
        private Queue<ThreadedMethod> mOutgoingTasksQueue;
        private SerialPortFixed mPort;
        private bool mDataReceived;
        private System.Windows.Forms.Timer mTimerSync;
        private bool mPortOpenedLastState;

        public PortSettings Settings { get; private set; }
        public event EventHandler<DataLogEventArgs> OnDataLog;
        public event EventHandler<MessageLogEventArgs> OnMessageLog;
        public event EventHandler OnConnectionChanged;

        public Worker()
        {
            this.Settings = Options.Options.Instance.PortOptions;
            this.mStopEvent = new AutoResetEvent(false);
            this.mIncomingTasksQueue = new Queue<ThreadedMethod>();
            this.mOutgoingTasksQueue = new Queue<ThreadedMethod>();
            this.mNeedStop = false;
            this.mPort = new SerialPortFixed(this.Settings.PortName, this.Settings.Baudrate, this.Settings.Parity, 8, this.Settings.StopBits);
            this.mThread = new Thread(new ThreadStart(this.Work));
            this.mDataReceived = false;
            this.mPortOpenedLastState = false;

            this.mTimerSync = new System.Windows.Forms.Timer();
            this.mTimerSync.Interval = 500;
            this.mTimerSync.Tick += new EventHandler(mTimerSync_Tick);
            this.mTimerSync.Start();

            this.mThread.Start();
        }

        public void Dispose()
        {
            this.mTimerSync.Stop();
            this.mStopEvent.Reset();
            this.mNeedStop = true;
            this.mStopEvent.WaitOne();
            Options.Options.Instance.PortOptions = this.Settings;
        }

        private void Work()
        {
            while (!this.mNeedStop)
            {
                Thread.Sleep(100);

                if (!this.CheckPortSettings())
                    continue;

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
                        this.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnDataLog != null)
                                this.OnDataLog(this, new DataLogEventArgs(Direction.Received, readedBytes));
                        });
                    }
                }

                // execute new incoming tasks
                ThreadedMethod task = null;
                lock (this.mIncomingTasksQueue)
                {
                    if (this.mIncomingTasksQueue.Count > 0)
                        task = this.mIncomingTasksQueue.Dequeue();
                }
                if (task != null)
                {
                    task();
                }
            }
            this.mStopEvent.Set();
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
                    this.mPort.StopBits != this.Settings.StopBits)
                {
                    bool opened = this.mPort.IsOpen;
                    if (opened)
                        this.mPort.Close();

                    this.mPort.PortName = this.Settings.PortName.ExtractPortName();
                    this.mPort.BaudRate = this.Settings.Baudrate;
                    this.mPort.Parity = this.Settings.Parity;
                    this.mPort.StopBits = this.Settings.StopBits;
                    //this.mPort.

                    if (opened)
                        this.mPort.Open();

                    this.EnqueueOutgoingTask(delegate()
                    {
                        this.LogMessage(String.Format("Port settins changed to: {0}, {1}, {2}, {3}",
                            this.Settings.PortName,
                            this.Settings.Baudrate,
                            this.Settings.Parity,
                            this.Settings.StopBits));
                    });
                }

                if (this.mPortOpenedLastState != this.mPort.IsOpen)
                {
                    this.mPortOpenedLastState = this.mPort.IsOpen;

                    if (this.mPortOpenedLastState)
                    {
                        this.EnqueueOutgoingTask(delegate()
                        {
                            if (this.OnConnectionChanged != null)
                                this.OnConnectionChanged(this, EventArgs.Empty);

                            this.LogMessage(String.Format("Port opened: {0}, {1}, {2}, {3}",
                                this.Settings.PortName,
                                this.Settings.Baudrate,
                                this.Settings.Parity,
                                this.Settings.StopBits));
                        });
                    }
                    else
                    {
                        this.EnqueueOutgoingTask(delegate()
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
            this.EnqueueIncomingTask(delegate()
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
                        this.EnqueueOutgoingTask(delegate()
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
            this.EnqueueIncomingTask(delegate()
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
            this.EnqueueIncomingTask(delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.Write(value, 0, value.Length);
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

        private void mTimerSync_Tick(object sender, EventArgs e)
        {
            ThreadedMethod[] tasks = new ThreadedMethod[0];
            lock (this.mOutgoingTasksQueue)
            {
                tasks = this.mOutgoingTasksQueue.ToArray();
                this.mOutgoingTasksQueue.Clear();
            }
            foreach (ThreadedMethod task in tasks)
            {
                if (task.Method != null)
                    task();
            }
        }

        private void LogMessage(string message)
        {
            if (this.OnMessageLog != null)
                this.OnMessageLog(this, new MessageLogEventArgs(message));
        }

        private void EnqueueIncomingTask(ThreadedMethod task)
        {
            lock (this.mIncomingTasksQueue)
            {
                this.mIncomingTasksQueue.Enqueue(task);
            }
        }

        private void EnqueueOutgoingTask(ThreadedMethod task)
        {
            lock (this.mOutgoingTasksQueue)
            {
                this.mOutgoingTasksQueue.Enqueue(task);
            }
        }
    }

    public delegate void ThreadedMethod();

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
