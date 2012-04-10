using System;
using System.Collections.Generic;
using System.Text;
using com232term.Classes.Options;

namespace com232term.Classes
{
    public interface IWorker
    {
        bool IsOpen { get; }
        void Send(byte[] value);
        void Open();
        void Close();

        PortSettings Settings { get; }
        event EventHandler<DataLogEventArgs> OnDataLog;
        event EventHandler<MessageLogEventArgs> OnMessageLog;
        event EventHandler OnConnectionChanged;
    }
}
