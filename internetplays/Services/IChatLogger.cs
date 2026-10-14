using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace internetplays.Services
{
    public interface IChatLogger
    {
        void LogMessage(string message);
    }
}
