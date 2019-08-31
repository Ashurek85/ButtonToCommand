using SharpDX.DirectInput;
using System;
using System.Threading;

namespace ButtonToCommand
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                bool showInfo = false;
                if (args.Length == 1)
                {
                    if (args[0] == "-info")
                        showInfo = true;
                    else
                    {
                        ShowUsage();
                        return;
                    }
                }
                else if (args.Length > 1)
                {
                    ShowUsage();                    
                    return;
                }

                // Get configuration
                Configuration config = new Configuration(showInfo);

                // Initialize DirectInput
                var directInput = new DirectInput();

                // Find gamepad/joystick
                bool found = false;
                int devicesFoundCount = 0;
                foreach (DeviceInstance device in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                {
                    devicesFoundCount++;

                    if (config.ShowInfo)
                        Console.WriteLine("Gamepad: " + device.InstanceName + " " + device.InstanceGuid);
                    if (!found && device.InstanceGuid == config.DeviceGuid)
                        found = true;
                }

                foreach (DeviceInstance device in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    devicesFoundCount++;

                    if (config.ShowInfo)
                        Console.WriteLine("Joystick: " + device.InstanceName + " " + device.InstanceGuid);
                    if (!found && device.InstanceGuid == config.DeviceGuid)
                        found = true;
                }

                if (devicesFoundCount == 0)
                    Console.WriteLine("No Gamepads/Joysticks (DirectInput) found");

                if (!found)
                {
                    Console.WriteLine($"Gamepad/Joystick {config.DeviceGuid.ToString()} not found!");
                    Console.ReadKey();
                }
                else
                {
                    // Reader loop
                    ReadInputDevice(directInput, config);
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ReadInputDevice(DirectInput directInput, Configuration config)
        {
            Joystick joystick = new Joystick(directInput, config.DeviceGuid);
            joystick.Acquire();
            while (true)
            {
                // Delay to save CPU usage
                Thread.Sleep(config.ReaderDelayMs);
                // Get joystick state
                JoystickState state = joystick.GetCurrentState();

                bool anyActive = false;
                for (int i = 0; i < state.Buttons.Length; i++)
                {
                    if (state.Buttons[i])
                    {
                        Console.Write(i + " ");
                        anyActive = true;
                    }
                }
                if (anyActive)
                    Console.WriteLine();

                foreach (int button in config.ButtonCommands.Keys)
                {
                    if (state.Buttons[button])
                    {
                        ExecuteCommand(config.ButtonCommands[button]);
                        if (config.ShowInfo)
                            Console.WriteLine("Execute: " + config.ButtonCommands[button]);
                    }
                }
            }
        }

        private static void ExecuteCommand(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage: ButtonToCommand [-info]");
            Console.ReadKey();
        }
    }
}
