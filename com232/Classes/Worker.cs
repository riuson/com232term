﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace com232term.Classes
{
    public class Worker : IDisposable
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

        private Thread mThread;
        private bool mNeedStop;
        private AutoResetEvent mStopEvent;
        private Queue<ThreadTask> mIncomingTasksQueue;
        private Queue<ThreadTask> mCompletedTasksQueue;
        private SerialPortFixed mPort;
        private ThreadTask mTask;
        private bool mDataReceived;
        private System.Windows.Forms.Timer mTimerSync;

        public event EventHandler OnSettingsChanged;
        public event EventHandler<DataReceivedEventArgs> OnDataReceived;
        public event EventHandler OnOpened;
        public event EventHandler OnClosed;
        public PortSettings Settings { get; private set; }

        public Worker()
        {
            this.Settings = new PortSettings();
            this.mStopEvent = new AutoResetEvent(false);
            this.mIncomingTasksQueue = new Queue<ThreadTask>();
            this.mCompletedTasksQueue = new Queue<ThreadTask>();
            this.mNeedStop = false;
            this.mPort = new SerialPortFixed(this.Settings.PortName, this.Settings.Baudrate, this.Settings.Parity, 8, this.Settings.StopBits);
            this.mThread = new Thread(new ThreadStart(this.Work));
            this.mDataReceived = false;

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

        private void Work()
        {
            while (!this.mNeedStop)
            {
                this.CheckPortSettings();

                ThreadTask task = null;

                if (this.mDataReceived)
                {
                    this.mDataReceived = false;

                    byte[] readedBytes = new byte[0];
                    lock (this.mPort)
                    {
                        if (this.mPort.IsOpen)
                        {
                            if (this.mPort.BytesToRead > 0)
                            {
                                readedBytes = new byte[this.mPort.BytesToRead];
                                this.mPort.Read(readedBytes, 0, readedBytes.Length);
                            }
                        }
                    }
                    if (readedBytes.Length > 0)
                    {
                        task = new ThreadTask();
                        task.Completed = delegate()
                        {
                            if (this.OnDataReceived != null)
                                this.OnDataReceived(this, new DataReceivedEventArgs(readedBytes));
                        };
                        lock (this.mCompletedTasksQueue)
                        {
                            this.mCompletedTasksQueue.Enqueue(task);
                        }
                        task = null;
                    }
                }

                // get new message to send
                lock (this.mIncomingTasksQueue)
                {
                    if (this.mIncomingTasksQueue.Count > 0)
                        task = this.mIncomingTasksQueue.Dequeue();
                }
                if (task != null)
                {
                    if (task.Work != null) task.Work();

                    if (task.Completed != null)
                    {
                        lock (this.mCompletedTasksQueue)
                        {
                            this.mCompletedTasksQueue.Enqueue(task);
                        }
                    }
                }

                Thread.Sleep(100);
            }
            this.mStopEvent.Set();
        }

        private void CheckPortSettings()
        {
            lock (this.mPort)
            {
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

                    if (opened)
                        this.mPort.Open();
                }
            }
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
            ThreadTask task = new ThreadTask();
            task.Work = delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.Open();
                    if (this.mPort.IsOpen)
                    {
                        this.mPort.DataReceived += new SerialDataReceivedEventHandler(mPort_DataReceived);
                    }
                }
            };
            task.Completed = delegate()
            {
                if (this.OnOpened != null)
                    this.OnOpened(this, EventArgs.Empty);
            };

            lock (this.mIncomingTasksQueue)
            {
                this.mIncomingTasksQueue.Enqueue(task);
            }
        }

        public void Close()
        {
            ThreadTask task = new ThreadTask();
            task.Work = delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.DataReceived -= new SerialDataReceivedEventHandler(mPort_DataReceived);
                    this.mPort.Close();
                }
            };
            task.Completed = delegate()
            {
                if (this.OnClosed != null)
                    this.OnClosed(this, EventArgs.Empty);
            };

            lock (this.mIncomingTasksQueue)
            {
                this.mIncomingTasksQueue.Enqueue(task);
            }
        }

        public void Send(byte []value)
        {
            ThreadTask task = new ThreadTask();
            task.Work = delegate()
            {
                lock (this.mPort)
                {
                    this.mPort.Write(value, 0, value.Length);
                }
            };

            lock (this.mIncomingTasksQueue)
            {
                this.mIncomingTasksQueue.Enqueue(task);
            }
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
            ThreadTask[] tasks = new ThreadTask[0];
            lock (this.mCompletedTasksQueue)
            {
                tasks = this.mCompletedTasksQueue.ToArray();
                this.mCompletedTasksQueue.Clear();
            }
            foreach (ThreadTask task in tasks)
            {
                if (task.Completed != null)
                    task.Completed();
            }
        }
    }

    public delegate void ThreadedMethod();

    public class ThreadTask
    {
        public ThreadTask()
        {
            this.Work = null;
            this.Completed = null;
        }
        public ThreadTask(ThreadedMethod work, ThreadedMethod completed)
        {
            this.Work = work;
            this.Completed = completed;
        }
        public ThreadedMethod Work { get; set; }
        public ThreadedMethod Completed { get; set; }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(byte []value)
        {
            this.Value = value;
        }

        public byte[] Value { get; private set; }
    }
}
