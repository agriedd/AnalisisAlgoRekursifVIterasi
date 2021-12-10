using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AnalisisAlgoRekursifVIterasi
{
    class Program{

        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            sw.Start();
            Console.WriteLine("Mulai..... (2006080087. Norman W. A. M. Peni)");

            var t1 = new Task(() => KerjakanSesuatu(1, 5000));
            var t2 = new Task(() => KerjakanSesuatu(2, 5000));
            var t3 = new Task(() => KerjakanSesuatu(3, 5000));

            t1.Start();
            t2.Start();
            t3.Start();

            Task.WaitAll(new Task[] { t1, t2, t3 });


            sw.Stop();

            Console.WriteLine("Selesai Semua. Total Waktu : {0} ms", sw.Elapsed.TotalMilliseconds);

            sw.Start();
            Console.WriteLine("Mulai For...");

            Parallel.For(0, 10, i =>
            {
                KerjakanSesuatu(i + 1, 1000);
            });

            sw.Stop();

            Console.WriteLine("Selesai For. Total Waktu : {0} ms", sw.Elapsed.TotalMilliseconds);

            Console.ReadKey();
        }
        private static void KerjakanSesuatu(int nomorTaks, int sleep)
        {
            Console.WriteLine("Mulai {0}", nomorTaks);
            var sw = new Stopwatch();
            sw.Start();
            Thread.Sleep(sleep);
            sw.Stop();

            Console.WriteLine("Selesai {0}. Total Waktu : {1:N0} ms", nomorTaks, sw.Elapsed.TotalMilliseconds);
        }
    }
}
