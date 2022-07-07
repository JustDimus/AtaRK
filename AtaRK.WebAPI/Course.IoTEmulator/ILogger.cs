using System;
using System.Collections.Generic;
using System.Text;

namespace Course.IoTEmulator
{
    public interface ILogger
    {
        void Log(string message);

        void SetColor(ConsoleColor color);
    }
}
