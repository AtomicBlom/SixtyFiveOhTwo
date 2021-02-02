using System.Threading;
using System.Threading.Tasks;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Runner;

var cpuHaltSource = new CancellationTokenSource();

var clock = new Clock();
var bus = new Bus(clock);
var logger = new Logger();
var cpu = new CPU(bus, cpuHaltSource, logger);
var rom = new ROM(cpu.InstructionSet, logger);
var memory = new MemoryChip(bus, cpuHaltSource.Token, rom.Bytes);

var programWait = new TaskCompletionSource();
cpuHaltSource.Token.Register(() => programWait.SetResult());

var clockThread = new Thread(clock.Run) {Name = "Clock"};
var cpuThread = new Thread(cpu.Run) {Name = "CPU"};
var memoryThread = new Thread(memory.Run) { Name = "Memory" };

clockThread.Start();
memoryThread.Start();
cpuThread.Start();

await programWait.Task.ConfigureAwait(false);
await Task.Yield();

cpuThread.Join();
memoryThread.Join();

clock.Stop();
clockThread.Join();