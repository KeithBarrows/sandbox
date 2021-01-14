using BigO.Core;
using BigO.Interfaces;
using BigO.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BigO
{
    class Program
    {
        private static IReader _reader;
        private static ILogger _logger;
        private static List<Route> _testPatterns;
        private static List<Route> _listPatternsToTestWith;
        private static Dictionary<string, Route> _dictionaryPatternsToTestWith;
        private static Dictionary<string, Route2> _triePatternsToTestWith;
        private static Timing _timings;
        private static Dictionary<string, Route2> _routeTable;

        static void Main(string[] args)
        {
            _logger = new ConsoleLogger();

            // Normally handled via startup, comment two of these, or last instantiation wins...
            //// ---==< use 2 files, one each for config and paths >==--- //
            //if (args.Length == 2)
            //    _reader = new FileReader(args, _logger);
            //// ---==< input configs and paths one line at a time >==--- //
            //_reader = new LineReader(_logger);
            // ---==< test samples given >==--- //
            _reader = new TestReader(_logger);

            _listPatternsToTestWith = _reader.ReadConfig();
            _routeTable = _reader.CreateRouteTable(_listPatternsToTestWith);
            var paths = _reader.ReadTargets();


            _logger.Log("Start...");
            BuildSearchData(1000001);
            _timings = new Timing();

            //BuildThenTestRoutes(paths, 100, 1000, 100);
            //BuildThenTestRoutes(paths, 1000, 10000, 1000);
            //BuildThenTestRoutes(paths, 10000, 100000, 5000);
            BuildThenTestRoutes(paths, 100000, 1000001, 25000);
            _logger.Log("Complete...");

            var results = _timings.DisplayResults();

            _logger.Log("");
            results.ToList().ForEach(a => _logger.Log(a));
            File.WriteAllLines(@"D:\Projects\Interview\results.csv", results);

            _logger.Log("");
            _logger.Log("Hit any key to end.");
            Console.ReadKey();
        }


        private static void BuildSearchData(int iterations)
        {
            // build 676 (26*26) unique 2 letter codes...
            List<string> pathParts = new List<string>();
            for (char c1 = 'A'; c1 <= 'Z'; c1++)
            {
                for (char c2 = 'A'; c2 <= 'Z'; c2++)
                {
                    pathParts.Add($"{c1}{c2}");
                }
            }

            // build a whole bunch of non-duplicate test patterns...
            Random rnd = new Random();
            //var tmp = _listPatternsToTestWith;
            var tmp = new List<Route>();
            var counter = 0;
            var max = pathParts.Count;

            for (int i = 0; i < max; i++)           // 676 iterations
            {
                for (int j = 0; j < max; j++)       // 456,976 (676*676) iterations
                {
                    for (int k = 0; k < max; k++)   // 308,915,776 (676*676*676) iterations
                    {
                        tmp.Add(new Route { Path = $"/{pathParts[i]}/{pathParts[j]}/{pathParts[k]}", Endpoint = $"{pathParts[i]}_{pathParts[j]}_{pathParts[k]}" });
                        if (counter++ >= iterations)
                            break;
                    }
                    if (counter >= iterations)
                        break;
                }
                if (counter >= iterations)
                    break;
            }
            _testPatterns = tmp;
        }
        private static void GetTestPatterns(int iterations)
        {
            _logger.Log("");
            _logger.Log($"  [LOADING {iterations:00,000} records]");
            Stopwatch sw = new Stopwatch();

            // append to the list collection...
            sw.Start();
            _listPatternsToTestWith = new List<Route>();
            for (int i = 0; i < iterations; i++)
                _listPatternsToTestWith.Add(_testPatterns[i]);
            sw.Stop();
            _timings.AddUpdateDataStore("Load", "ListLinq", _listPatternsToTestWith.Count, sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds);
            _logger.Log($"[{sw.Elapsed.TotalMilliseconds / 1000:0.000000}s] -- ListLinq");
            sw.Reset();

            // append to the dictionary collection...
            sw.Start();
            _dictionaryPatternsToTestWith = new Dictionary<string, Route>();
            for (int i = 0; i < iterations; i++)
                _dictionaryPatternsToTestWith.Add(_testPatterns[i].Path, _testPatterns[i]);
            sw.Stop();
            _timings.AddUpdateDataStore("Load", "DictionaryLinq", _dictionaryPatternsToTestWith.Count, sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds);
            _logger.Log($"[{sw.Elapsed.TotalMilliseconds / 1000:0.000000}s] -- DictionaryLinq");
            sw.Reset();

            // append to the trie collection...
            sw.Start();
            var reader = new TestReader(_logger);
            var test = _testPatterns.GetRange(0, iterations);
            _triePatternsToTestWith = reader.CreateRouteTable(test);
            sw.Stop();
            _timings.AddUpdateDataStore("Load", "Trie", _dictionaryPatternsToTestWith.Count, sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds);
            _logger.Log($"[{sw.Elapsed.TotalMilliseconds / 1000:0.000000}s] -- DictionaryLinq");
            sw.Reset();

            //// split the wildcards from the full routes for each collection type...
            //_dictRegular = _dictionaryPatternsToTestWith.Where(a => !a.Key.Contains("/X")).ToDictionary(x => x.Key, x => x.Value);
            //_dictWildcard = _dictionaryPatternsToTestWith.Where(a => a.Key.Contains("/X")).ToDictionary(x => x.Key, x => x.Value);
            //_listRegularPatternsToTestWith = _listPatternsToTestWith.Where(a => !a.Path.Contains("/X")).ToList();
            //_listWildcardPatternsToTestWith = _listPatternsToTestWith.Where(a => a.Path.Contains("/X")).ToList();
        }
        private static void RunTest(ISearch searcher, List<string> paths, string name)
        {
            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                sw.Start();
                searcher.Execute(paths);
                sw.Stop();
                _timings.AddUpdateDataStore("Read", name, _listPatternsToTestWith.Count, sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds);
                _logger.Log($"[{sw.Elapsed.TotalMilliseconds / 1000:0.000000}s] -- {name}");
                sw.Reset();
            }
        }
        private static void BuildThenTestRoutes(List<string> paths, int startLoop, int endLoop, int stepLoop)
        {
            for (int iterations = startLoop; iterations < endLoop; iterations += stepLoop)
            {
                GetTestPatterns(iterations);

                _logger.Log("");
                _logger.Log($"  [READING {_listPatternsToTestWith.Count:000,000} records]");
                RunTest(new SearchList(_listPatternsToTestWith), paths, "List");
                RunTest(new SearchListLinq(_listPatternsToTestWith), paths, "ListLinq");
                RunTest(new SearchDictionary(_dictionaryPatternsToTestWith), paths, "Dictionary");
                RunTest(new SearchDictionaryLinq(_dictionaryPatternsToTestWith), paths, "DictionaryLinq");
                RunTest(new SearchTrie(_triePatternsToTestWith), paths, "Trie");
            }
        }
    }
}
