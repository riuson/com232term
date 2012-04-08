using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using com232term.Classes.Options;

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
        private bool mDataReceived;
        private System.Windows.Forms.Timer mTimerSync;

        public event EventHandler OnSettingsChanged;
        public event EventHandler<DataLogEventArgs> OnDataLog;
        public event EventHandler OnOpened;
        public event EventHandler OnClosed;
        public PortSettings PortOptions { get; private set; }

        public Worker()
        {
            this.PortOptions = new PortSettings();
            this.mStopEvent = new AutoResetEvent(false);
            this.mIncomingTasksQueue = new Queue<ThreadTask>();
            this.mCompletedTasksQueue = new Queue<ThreadTask>();
            this.mNeedStop = false;
            this.mPort = new SerialPortFixed(this.PortOptions.PortName, this.PortOptions.Baudrate, this.PortOptions.Parity, 8, this.PortOptions.StopBits);
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
            this.PortOptions.PortName = portname;
            this.PortOptions.Baudrate = baudrate;
            this.PortOptions.Parity = parity;
            this.PortOptions.StopBits = stopbits;

            if (this.OnSettingsChanged != null)
                this.OnSettingsChanged(this, EventArgs.Empty);
        }

        private void Work()
        {
            while (!this.mNeedStop)
            {
                Thread.Sleep(100);

                if (!this.CheckPortSettings())
                    continue;

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
                            if (this.OnDataLog != null)
                                this.OnDataLog(this, new DataLogEventArgs(Direction.Received, readedBytes));
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
            }
            this.mStopEvent.Set();
        }

        private bool CheckPortSettings()
        {
            lock (this.mPort)
            {
                if (this.PortOptions.PortName == String.Empty)
                    return false;

                if (this.mPort.PortName != this.PortOptions.PortName.ExtractPortName() ||
                    this.mPort.BaudRate != this.PortOptions.Baudrate ||
                    this.mPort.Parity != this.PortOptions.Parity ||
                    this.mPort.StopBits != this.PortOptions.StopBits)
                {
                    bool opened = this.mPort.IsOpen;
                    if (opened)
                        this.mPort.Close();

                    this.mPort.PortName = this.PortOptions.PortName.ExtractPortName();
                    this.mPort.BaudRate = this.PortOptions.Baudrate;
                    this.mPort.Parity = this.PortOptions.Parity;
                    this.mPort.StopBits = this.PortOptions.StopBits;

                    if (opened)
                        this.mPort.Open();
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
}
