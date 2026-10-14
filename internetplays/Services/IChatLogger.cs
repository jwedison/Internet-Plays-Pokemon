using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace internetplays.Services
{
    public interface IChatLogger : IDisposable
    {
        void LogMessage(string message);
    }
}
