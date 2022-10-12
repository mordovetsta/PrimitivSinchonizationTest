using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace PrimitivSinchonizationTest
{
    [MemoryDiagnoser]
    [RankColumn(BenchmarkDotNet.Mathematics.NumeralSystem.Roman)]
    public class BenchMark
    {
        [Params(10, 1000, 100000)]
        public int test;
        object locker = new object();

        [Benchmark(Description = "monitor")]

        public void MonitorTest()
        {
            int counter = 0;
            for (int i = 0; i < test; i++)
            {
                Monitor.Enter(locker);


                var j = ++counter;
                Monitor.Exit(locker);
            }
        }
        [Benchmark(Description = "locker")]
        public void LockerTest()
        {
            int counter = 0;
            for (int i = 0; i < test; i++)
            {
                lock (locker)
                {
                    var j = ++counter;
                }

            }
        }
        AutoResetEvent auto = new AutoResetEvent(true);
        [Benchmark(Description = "autotest")]

        public void AutoResetTest()
        {
            int counter = 0;
            for (int i = 0; i < test; i++)
            {

                var j = ++counter;

            }
        }

        Mutex mtx = new Mutex(true);
        [Benchmark(Description = "mutex")]

        public void MutexTest()
        {
            int counter = 0;
            for (int i = 0; i < test; i++)
            {
                auto.WaitOne();
                var j = ++counter;
                auto.Set();
            }
        }

        EventWaitHandle eventWaitHandle = new EventWaitHandle(true, EventResetMode.AutoReset);
        [Benchmark(Description ="eventWaitHandle")]

        public void EventWaitHandleTester()
        {
            int counter = 0;
            for(int i=0;i<test;i++)
            {
                eventWaitHandle.WaitOne();
                var x = ++counter;
                eventWaitHandle.Set();
            }
        }
    }
}
