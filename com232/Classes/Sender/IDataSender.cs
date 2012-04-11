using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using com232term.Classes.Options;

namespace com232term.Classes.Sender
{
    public interface IDataSender
    {
        BindingList<String> Packets { get; }
        BindingList<String> PacketsStatic { get; }
        void Send(string packet);
        void CallStaticPacketsEditor();
        SendSettings Settings { get; }
    }
}
