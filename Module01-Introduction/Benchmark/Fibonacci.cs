using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }
        
        private Dictionary<ulong, ulong> _fibCache = new Dictionary<ulong, ulong>();

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            if (!_fibCache.TryGetValue(n - 1, out var first))
            {
                first = RecursiveWithMemoization(n - 1);
                _fibCache.Add(n - 1, first);
            }

            if (!_fibCache.TryGetValue(n - 2, out var second))
            {
                second = RecursiveWithMemoization(n - 2);
                _fibCache.Add(n - 2, second);
            }

            return first + second;
        }

        // Terrible version of memoization
        // Never use this
        //private ConcurrentDictionary<ulong, ulong> _fibCache = new ConcurrentDictionary<ulong, ulong>();
        //[Benchmark]
        //[ArgumentsSource(nameof(Data))]
        //public ulong RecursiveWithMemoization(ulong n)
        //{
        //    if (n == 1 || n == 2) return 1;
        //
        //    return _fibCache.GetOrAdd(n - 1, RecursiveWithMemoization(n - 1)) +
        //           _fibCache.GetOrAdd(n - 2, RecursiveWithMemoization(n - 2));
        //}

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            ulong result = 1;
            ulong previous = 0;

            for (ulong i = 1; i < n; i++)
            {
                ulong toAdd = previous;
                previous = result;
                result += toAdd;
            }
            return result;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
