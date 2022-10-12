using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimitivSinchonizationTest
{
    public class CollectionTest
    {
        [Params(10, 1000, 100000)] public int Count;
       

        [GlobalSetup]
        public void Setup()
        {
            _queue = new Queue<int>(Count);
            _stack = new Stack<int>(Count);
            _set = new HashSet<int>(Count);
        }

        private Queue<int> _queue;
        private Stack<int> _stack;
        private HashSet<int> _set;

        private readonly ConcurrentBag<int> _bag = new();
        private readonly ConcurrentQueue<int> _cQueue = new();
        private readonly ConcurrentStack<int> _cStack = new();

        private readonly object _lock = new();

        [Benchmark(Description ="Queue")]

        public void QueueTest()
        {
            _queue.Clear();
            Parallel.For(0, Count, i =>
            {
                  Monitor.Enter(_lock);
                  _queue.Enqueue(i);
                  Monitor.Exit(_lock);
            });

            Parallel.For(0, Count, _ =>
            {
                  Monitor.Enter(_lock);
                  var x = _queue.Dequeue();
                  Monitor.Exit(_lock);
            });


        }

        [Benchmark(Description ="CStack")]
        public void CStackTest()
        {
            _cStack.Clear();
            Parallel.For(0, Count, i => _cStack.Push(i));
            Parallel.For(0, Count, _ => _cStack.TryPop(out int x));
        }




    }
}
