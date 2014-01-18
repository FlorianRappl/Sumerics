using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAMP.Core;

namespace YAMP
{
    class Program
    {
        static void Main()
        {
            //Use:
            //http://www.codeproject.com/Articles/604301/Fluent-Method-and-Type-Builder

            //MatrixBenchmark();

            var ctx = new Runtime(
                useDefaultVariable: true
                );

            var a = new Dynamic();

            var repl = new Repl(ctx);
            repl.Run(new MyConsole());

            //var cos = MakeTenK("cos(3)");
            var little = MakeTenK("2+3");
            var standard = MakeTenK("2-3*5+7/2-8*2");
            var longer = MakeTenK("2-(3*5)^2+7/(2-8)*2");

            //warmup
            foreach (var query in longer)
                Runtime.Evaluate(query);

            //Benchmark("YAMP²", cos, query => YContext.Evaluate(query));
            Benchmark("YAMP²", little, query => Runtime.Evaluate(query));
            Benchmark("YAMP²", standard, query => Runtime.Evaluate(query));
            Benchmark("YAMP²", longer, query => Runtime.Evaluate(query));
        }

        #region Some Benchmarking

        static Complex[] MatrixTheory(int n, int m)
        {
            var c = new Complex[n * m];
            var r = new Random();

            for (int i = 0; i < c.Length; i++)
            {
                c[i] = r.NextDouble();
            }

            return c;
        }

        static void MatrixBenchmark()
        {
            Console.WriteLine("Running matrix benchmarks ...");
            //var n = 1000;
            //var m = 1000;

            for (var n = 20; n <= 500; n += 20)
            {
                var m = n;
                var A = new Matrix(n, m);
                var B = new Matrix(m, n);
                A.Randomize();
                B.Randomize();
                //var A = MatrixTheory(n, m);
                //var B = MatrixTheory(m, n);
                //var C = new double[n * n];

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var C = A * B;
                //YAMP.Numerics.BlasL3.dGemm(A, 0, m, 1, B, 0, n, 1, C, 0, n, 1, n, n, m);
                sw.Stop();

                #region Outputs

                //---
                // YAMPv1
                //---
                //Time for n = 20, m = 20 : 7 ms
                //Time for n = 40, m = 40 : 8 ms
                //Time for n = 60, m = 60 : 28 ms
                //Time for n = 80, m = 80 : 51 ms
                //Time for n = 100, m = 100 : 135 ms
                //Time for n = 120, m = 120 : 273 ms
                //Time for n = 140, m = 140 : 281 ms
                //Time for n = 160, m = 160 : 387 ms
                //Time for n = 180, m = 180 : 585 ms
                //Time for n = 200, m = 200 : 845 ms
                //Time for n = 220, m = 220 : 1196 ms
                //Time for n = 240, m = 240 : 1709 ms
                //Time for n = 260, m = 260 : 2318 ms
                //Time for n = 280, m = 280 : 2451 ms
                //Time for n = 300, m = 300 : 2771 ms
                // and so on !

                //---
                // Theory
                //---
                //Time for n = 20, m = 20 : 1 ms
                //Time for n = 40, m = 40 : 0 ms
                //Time for n = 60, m = 60 : 0 ms
                //Time for n = 80, m = 80 : 2 ms
                //Time for n = 100, m = 100 : 4 ms
                //Time for n = 120, m = 120 : 7 ms
                //Time for n = 140, m = 140 : 12 ms
                //Time for n = 160, m = 160 : 19 ms
                //Time for n = 180, m = 180 : 29 ms
                //Time for n = 200, m = 200 : 36 ms
                //Time for n = 220, m = 220 : 49 ms
                //Time for n = 240, m = 240 : 65 ms
                //Time for n = 260, m = 260 : 88 ms
                //Time for n = 280, m = 280 : 99 ms
                //Time for n = 300, m = 300 : 125 ms
                //Time for n = 320, m = 320 : 196 ms
                //Time for n = 340, m = 340 : 224 ms
                //Time for n = 360, m = 360 : 249 ms
                //Time for n = 380, m = 380 : 355 ms
                //Time for n = 400, m = 400 : 382 ms
                //Time for n = 420, m = 420 : 609 ms
                //Time for n = 440, m = 440 : 479 ms
                //Time for n = 460, m = 460 : 752 ms
                //Time for n = 480, m = 480 : 820 ms
                //Time for n = 500, m = 500 : 917 ms

                //---
                // YAMPv2 BLOCK = 5
                //---
                //Time for n = 20, m = 20 : 7 ms
                //Time for n = 40, m = 40 : 0 ms
                //Time for n = 60, m = 60 : 1 ms
                //Time for n = 80, m = 80 : 2 ms
                //Time for n = 100, m = 100 : 5 ms
                //Time for n = 120, m = 120 : 9 ms
                //Time for n = 140, m = 140 : 14 ms
                //Time for n = 160, m = 160 : 26 ms
                //Time for n = 180, m = 180 : 28 ms
                //Time for n = 200, m = 200 : 39 ms
                //Time for n = 220, m = 220 : 51 ms
                //Time for n = 240, m = 240 : 65 ms
                //Time for n = 260, m = 260 : 82 ms
                //Time for n = 280, m = 280 : 114 ms
                //Time for n = 300, m = 300 : 125 ms
                //Time for n = 320, m = 320 : 176 ms
                //Time for n = 340, m = 340 : 188 ms
                //Time for n = 360, m = 360 : 219 ms
                //Time for n = 380, m = 380 : 259 ms
                //Time for n = 400, m = 400 : 361 ms
                //Time for n = 420, m = 420 : 543 ms
                //Time for n = 440, m = 440 : 685 ms
                //Time for n = 460, m = 460 : 640 ms
                //Time for n = 480, m = 480 : 939 ms
                //Time for n = 500, m = 500 : 1065 ms

                //---
                // YAMPv2 BLOCK = 15
                //---
                //Time for n = 20, m = 20 : 7 ms
                //Time for n = 40, m = 40 : 0 ms
                //Time for n = 60, m = 60 : 1 ms
                //Time for n = 80, m = 80 : 2 ms
                //Time for n = 100, m = 100 : 5 ms
                //Time for n = 120, m = 120 : 8 ms
                //Time for n = 140, m = 140 : 13 ms
                //Time for n = 160, m = 160 : 24 ms
                //Time for n = 180, m = 180 : 27 ms
                //Time for n = 200, m = 200 : 40 ms
                //Time for n = 220, m = 220 : 50 ms
                //Time for n = 240, m = 240 : 66 ms
                //Time for n = 260, m = 260 : 82 ms
                //Time for n = 280, m = 280 : 104 ms
                //Time for n = 300, m = 300 : 126 ms
                //Time for n = 320, m = 320 : 172 ms
                //Time for n = 340, m = 340 : 185 ms
                //Time for n = 360, m = 360 : 223 ms
                //Time for n = 380, m = 380 : 254 ms
                //Time for n = 400, m = 400 : 329 ms
                //Time for n = 420, m = 420 : 572 ms
                //Time for n = 440, m = 440 : 467 ms
                //Time for n = 460, m = 460 : 665 ms
                //Time for n = 480, m = 480 : 679 ms
                //Time for n = 500, m = 500 : 1063 ms

                //---
                // YAMPv2 BLOCK = 50
                //---
                //Time for n = 20, m = 20 : 11 ms
                //Time for n = 40, m = 40 : 0 ms
                //Time for n = 60, m = 60 : 1 ms
                //Time for n = 80, m = 80 : 2 ms
                //Time for n = 100, m = 100 : 5 ms
                //Time for n = 120, m = 120 : 9 ms
                //Time for n = 140, m = 140 : 14 ms
                //Time for n = 160, m = 160 : 26 ms
                //Time for n = 180, m = 180 : 37 ms
                //Time for n = 200, m = 200 : 43 ms
                //Time for n = 220, m = 220 : 63 ms
                //Time for n = 240, m = 240 : 70 ms
                //Time for n = 260, m = 260 : 100 ms
                //Time for n = 280, m = 280 : 137 ms
                //Time for n = 300, m = 300 : 145 ms
                //Time for n = 320, m = 320 : 176 ms
                //Time for n = 340, m = 340 : 181 ms
                //Time for n = 360, m = 360 : 237 ms
                //Time for n = 380, m = 380 : 356 ms
                //Time for n = 400, m = 400 : 345 ms
                //Time for n = 420, m = 420 : 522 ms
                //Time for n = 440, m = 440 : 469 ms
                //Time for n = 460, m = 460 : 703 ms
                //Time for n = 480, m = 480 : 757 ms
                //Time for n = 500, m = 500 : 909 ms

                #endregion

                Console.WriteLine("Time for n = {0}, m = {1} : {2} ms", n, m, sw.ElapsedMilliseconds);
            }

            Console.WriteLine("Finished !");
        }

        static string[] MakeTenK(string s)
        {
            var l = new string[10000];

            for (var i = 0; i < l.Length; i++)
                l[i] = s;

            return l;
        }

        static void Benchmark(string name, string[] lines, Action<string> parser)
        {
            var sw = new System.Diagnostics.Stopwatch();
            Console.Write(name);
            Console.Write(" : Running benchmark ... ");
            sw.Start();

            foreach (var query in lines)
            {
                parser(query);
            }

            sw.Stop();
            Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);
        }

        #endregion

        class MyConsole : IConsole
        {
            #region Members

            static Dictionary<ConsoleColor, Color> colors;
            bool cancelRequested;

            #endregion

            #region ctor

            static MyConsole()
            {
                colors = new Dictionary<ConsoleColor, Color>();
                colors[ConsoleColor.Black] = Color.Black;
                colors[ConsoleColor.Blue] = Color.Blue;
                colors[ConsoleColor.Yellow] = Color.Yellow;
                colors[ConsoleColor.White] = Color.White;
                colors[ConsoleColor.Red] = Color.Red;
                colors[ConsoleColor.Green] = Color.Green;
                colors[ConsoleColor.Magenta] = Color.Magenta;
                colors[ConsoleColor.Black] = Color.Black;
            }

            public MyConsole()
            {
                cancelRequested = false;
            }

            #endregion

            #region Properties

            public bool CancelRequested
            {
                get { return cancelRequested; }
            }

            public Color FontColor
            {
                get
                {
                    return colors.ContainsKey(Console.ForegroundColor) ? colors[Console.ForegroundColor] : Color.White;
                }
                set
                {
                    Console.ForegroundColor = colors.Where(m => m.Value == value).Select(m => m.Key).SingleOrDefault();
                }
            }

            #endregion

            #region Methods

            public void WriteLine(string message)
            {
                Console.WriteLine(message);
            }

            public void Write(string message)
            {
                Console.Write(message);
            }

            public string ReadCommand()
            {
                return Console.ReadLine();
            }

            public void Startup()
            {
                Console.WriteLine("===============================================================================");
                Console.WriteLine("===============================================================================");
                Console.WriteLine("================================ YAMP TEST-REPL ===============================");
                Console.WriteLine("===============================================================================");
                Console.WriteLine("============================= (exit with CTRL + C) ============================");
                Console.WriteLine("===============================================================================");
                Console.WriteLine("===============================================================================");
                Console.CancelKeyPress += CancelKeyPress;
            }

            public void Exit()
            {
                Console.WriteLine("(Terminated)");
                Console.CancelKeyPress -= CancelKeyPress;
            }

            public void ResetColor()
            {
                Console.ResetColor();
            }

            #endregion

            #region Event Handlers

            void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
            {
                cancelRequested = true;
                e.Cancel = true;
            }

            #endregion
        }
    }
}
