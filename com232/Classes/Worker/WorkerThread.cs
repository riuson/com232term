using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using com232term.Classes.Options;

namespace com232term.Classes.Worker
{
    public class WorkerThread : IDisposable
    {
        private Thread mThread;
        private bool mNeedStop;
        private AutoResetEvent mStopEvent;
        private Queue<ThreadedMethod> mIncomingTasksQueue;
        private Queue<ThreadedMethod> mOutgoingTasksQueue;
        private System.Windows.Forms.Timer mTimerSync;

        public ThreadedMethod Idle { private get; set; }

        public WorkerThread()
        {
            this.mStopEvent = new AutoResetEvent(false);
            this.mIncomingTasksQueue = new Queue<ThreadedMethod>();
            this.mOutgoingTasksQueue = new Queue<ThreadedMethod>();
            this.mNeedStop = false;
            this.mThread = new Thread(new ThreadStart(this.Work));
            
            this.mTimerSync = new System.Windows.Forms.Timer();
            this.mTimerSync.Interval = 500;
            this.mTimerSync.Tick += new EventHandler(mTimerSync_Tick);
            this.mTimerSync.Start();

            this.Idle = null;

            this.mThread.Start();
        }

        public void Dispose()
        {
            this.mTimerSync.Stop();
            this.mStopEvent.Reset();
            this.mNeedStop = true;
            this.mStopEvent.WaitOne();
        }

        private void Work()
        {
            while (!this.mNeedStop)
            {
                Thread.Sleep(100);

                ThreadedMethod idle = this.Idle;
                if (idle != null)
                    idle();

                // execute new incoming tasks
                ThreadedMethod task = null;
                lock (this.mIncomingTasksQueue)
                {
                    if (this.mIncomingTasksQueue.Count > 0)
                        task = this.mIncomingTasksQueue.Dequeue();
                }
                if (task != null)
                    task();
            }
            this.mStopEvent.Set();
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
                if (task != null)
                    task();
            }
        }

        public void EnqueueIncomingTask(ThreadedMethod task)
        {
            lock (this.mIncomingTasksQueue)
            {
                this.mIncomingTasksQueue.Enqueue(task);
            }
        }

        public void EnqueueOutgoingTask(ThreadedMethod task)
        {
            lock (this.mOutgoingTasksQueue)
            {
                this.mOutgoingTasksQueue.Enqueue(task);
            }
        }
    }

    public delegate void ThreadedMethod();
}
