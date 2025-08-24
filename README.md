Microwave oven controller
Please supply a complete Visual studio solution including the controller code and unit tests.
It is a very simple microwave oven that has:
• a heater – can be turned on or off,
• a door – can be opened and closed by user,
• a start button – can be pressed by the user.
User stories:
• When I open door, Light is on.
• When I close door, Light turns off.
• When I open the door, the heater stops running.
• When I press the start button when the door is open nothing happens.
• When I press the start button when the door is closed, the heater runs for 1 minute.
• When I press the start button when the door is closed and already heating, increase remaining
time by 1 minute.
[Additional requirement]
• The microwave oven cannot run longer than the time specified in the configuration file.

Microwave.Console is the startup project.