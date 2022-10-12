using BenchmarkDotNet.Running;

namespace PrimitivSinchonizationTest
{

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CollectionTest>();
        }


    }
}
