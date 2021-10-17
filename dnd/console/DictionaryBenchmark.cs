using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace console
{
    [RPlotExporter]
    [SimpleJob]
    public class DictionaryBenchmark
    {
        int numberOfItem = 100;

        //[Benchmark]
        public void NestedDictionaryCreation()
        {
            var nested = new Dictionary<int, Dictionary<int, int>>();
            for (int i = 0; i < numberOfItem; i++)
            {
                nested[i] = new Dictionary<int, int>();
                for (int j = 0; j < numberOfItem; j++)
                {
                    nested[i].Add(j, numberOfItem);
                }
            }
        }

        //[Benchmark]
        public void NestedDictionaryRead()
        {
            int value = 0;
            var nested = new Dictionary<int, Dictionary<int, int>>();
            for (int i = 0; i < numberOfItem; i++)
            {
                nested[i] = new Dictionary<int, int>();
                for (int j = 0; j < numberOfItem; j++)
                {
                    nested[i].Add(j, numberOfItem);
                }
            }
            for (int i = 0; i < numberOfItem; i++)
            {
                for (int j = 0; j < numberOfItem; j++)
                {
                    Dictionary<int,int> nest;
                    nested.TryGetValue(i, out nest);
                    nest?.TryGetValue(j, out value);
                }
            }
            value.ToString();
        }

        //[Benchmark]
        public void DictionaryWithMulCreation()
        {
            var mul = new Dictionary<int, int>();
            for (int i = 0; i < numberOfItem; i++)
            {
                for (int j = 0; j < numberOfItem; j++)
                {
                    mul.Add(numberOfItem * i + j, numberOfItem);
                }
            }
        }

        [Benchmark]
        public void DictionaryWithMulReadNoMemo()
        {
            
                int value = 0;
                var mul = new Dictionary<int, int>();
                for (int i = 0; i < numberOfItem; i++)
                {
                    for (int j = 0; j < numberOfItem; j++)
                    {
                        mul.Add(this.mul(numberOfItem,i, j), numberOfItem);
                    }
                }
            for (int k = 0; k < 25; k++)
            {
                for (int i = 0; i < numberOfItem; i++)
                {
                    for (int j = 0; j < numberOfItem; j++)
                    {
                        mul.TryGetValue(this.mul(numberOfItem, i, j), out value);
                    }
                }
                value.ToString();
            }
        }

        [Benchmark]
        public void DictionaryWithMulReadMemo()
        {
            
                int value = 0;
                var mul = new Dictionary<int, int>();
                for (int i = 0; i < numberOfItem; i++)
                {
                    for (int j = 0; j < numberOfItem; j++)
                    {
                        mul.Add(this.memomul(numberOfItem, i, j), numberOfItem);
                    }
                }
            for (int k = 0; k < 25; k++)
            {
                for (int i = 0; i < numberOfItem; i++)
                {
                    for (int j = 0; j < numberOfItem; j++)
                    {
                        mul.TryGetValue(this.memomul(numberOfItem, i, j), out value);
                    }
                }
                value.ToString();
            }
        }

        private int mul(int max, int i, int j)
        {
            return max * i + j;
        }

        private Func<int, int, int, int> functionMemo;

        public int memomul(int max, int i, int j)
        {
            if (functionMemo == null)
            {
                functionMemo = Memoizer.Memoize<int, int, int, int>((a, b, c) =>
                {
                    return mul(a,b,c);
                });
            }

            return functionMemo(max, i, j);
        }

    }
}
