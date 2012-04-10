using System;
using System.Collections.Generic;
using System.Text;
using com232term.Classes.Options;

namespace com232term.Classes
{
    public interface ILogger
    {
        LogSettings Settings { get; }
        void LogData(DateTime time, Direction direction, byte[] array);
        void LogMessage(DateTime time, string message);
        void Clear();
    }
}
