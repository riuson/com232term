using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace com232term.Classes
{
    public class DataSender : IDataSender
    {
        public BindingList<String> Packets { get; private set; }
        public BindingList<String> PacketsStatic { get; private set; }
        public event EventHandler<CallPacketsEditorEventArgs> OnStaticEditorCall;

        public DataSender()
        {
            this.Packets = new BindingList<string>();
            this.Packets.Add("1");
            this.Packets.Add("2");

            this.PacketsStatic = new BindingList<string>();
            this.PacketsStatic.Add("one");
            this.PacketsStatic.Add("two");
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

        public void CallStatisPacketsEditor()
        {
            if (this.OnStaticEditorCall != null)
            {
                CallPacketsEditorEventArgs args = new CallPacketsEditorEventArgs(this.PacketsStatic);
                this.OnStaticEditorCall(this, args);
                if (args.Changed)
                {
                    //this.PacketsStatic.RaiseListChangedEvents = false;
                    this.PacketsStatic.Clear();
                    foreach (string packet in args.List)
	                {
                        this.PacketsStatic.Add(packet);
	                }
                    //this.PacketsStatic.RaiseListChangedEvents = true;
                }
            }
        }
    }

    public class CallPacketsEditorEventArgs : EventArgs, IDisposable
    {
        public CallPacketsEditorEventArgs(BindingList<String> sourceList)
        {
            this.List = new BindingList<string>();
            foreach (string packet in sourceList)
            {
                this.List.Add(packet);
            }
            this.List.ListChanged += new ListChangedEventHandler(List_ListChanged);
            this.Changed = false;
        }

        private void List_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.Changed = true;
        }

        public BindingList<String> List { get; private set; }
        public bool Changed { get; private set; }

        public void Dispose()
        {
            this.List.ListChanged -= new ListChangedEventHandler(List_ListChanged);
        }
    }
}
