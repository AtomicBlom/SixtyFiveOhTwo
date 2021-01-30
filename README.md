# SixtyFiveOhTwo
A 6502 emulator written in C#

# Design Goals
* Timing accurate
  * I'd like the CPU core to synchronize clock cycles like a real one would

* Implement a modular bus
  * I'd like to be able to be able to add in peripherals
  
* Support concurrent CPUs, with breakpoints and debugging
* Easy to integrate and use.
* Unit Tested

# Progress
* CPU runs a Fetch/Execute cycle
* 8 out of 56 instructions implemented
  * 18 out of 151 opcodes implemented
* No ALU operations
* No Stack operations
* Rough Program Builder, I'd like to make something similar to the C# Reflection.Emit framework.
* OpCodes are being tested as they are implemented.
  * Few exceptions that will be fixed really soon.
