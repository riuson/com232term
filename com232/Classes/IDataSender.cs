using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace com232term.Classes
{
    public interface IDataSender
    {
        BindingList<String> Packets { get; }
        void Send(string packet);
    }
}
