# ButtonToCommand
Execute commands with button presses

Only works with **DirectInput** compatible device.

Configuration:
- DeviceGuid: Guid of gamepad/joystick to use.
- ReaderDelayMs: Milleseconds of delay between state readings. 0 to maximun performance.
- Set relationship between the button number and the command. 


To extract gamepad guids and identify button number use -info parameter in command line.

Example: 

```
<appSettings>
    <add key="DeviceGuid" value="d2b973c0-b7ba-11e9-8001-444553540000"/> <!-- My gamepad Guid -->
    <add key="ReaderDelayMs" value="100"/> <!-- 100ms between state readings -->
    <add key="8" value="calc.exe"/> <!-- When i press button8 execute cmd /c calc.exe -->
  </appSettings>```
  
