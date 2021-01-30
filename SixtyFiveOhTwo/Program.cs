using System.Threading;
using System.Threading.Tasks;
using SixtyFiveOhTwo;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;

var cpuHaltSource = new CancellationTokenSource();

var instructionSet = new InstructionSet();

var clock = new Clock();
var bus = new Bus(clock);
var logger = new Logger();
var cpu = new CPU(instructionSet.Instructions, bus, cpuHaltSource, logger);
var memory = new MemoryChip(bus, cpuHaltSource.Token, new ROM(logger).Bytes);

var programWait = new TaskCompletionSource();
cpuHaltSource.Token.Register(() => programWait.SetResult());

var clockThread = new Thread(clock.Run) {Name = "Clock"};
var cpuThread = new Thread(cpu.Run) {Name = "CPU"};
var memoryThread = new Thread(memory.Run) { Name = "Memory" };

cpuThread.Start();
clockThread.Start();
memoryThread.Start();

await programWait.Task.ConfigureAwait(false);
await Task.Yield();

cpuThread.Join();
memoryThread.Join();

clock.Stop();
clockThread.Join();