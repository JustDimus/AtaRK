using System;

namespace Course.IoTEmulator
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void SetColor(ConsoleColor color)
        {
            Console.BackgroundColor = color;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string deviceName = string.Empty;
            string devicePassword = string.Empty;
            string deviceId = string.Empty;

            Console.WriteLine("Loading Emulator...");
            Console.Write("Enter the device name: ");
            deviceName = Console.ReadLine();

            Console.Write("Enter the device password: ");
            devicePassword = Console.ReadLine();

            Console.Write("Enter the device id: ");
            deviceId = Console.ReadLine();

            Logger logger = new Logger();

            Emulator emulator = new Emulator(logger);
            emulator.Start(deviceName, devicePassword, deviceId);

            Console.ReadLine();
        }
    }
}
