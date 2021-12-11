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

        private static string[] data;
        private static string[] Stopword;
        private static string[] BankKata;

        static void Main(string[] args){
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            
            var loadTeks = new Task(()=> data = loadFile("../../Assets/teks.txt"));
            var loadStopword = new Task(() => Stopword = loadFile("../../Assets/stopword.txt"));
            var loadBankKata = new Task(() => BankKata = loadFile("../../Assets/bank_kata.txt"));

            loadTeks.Start();
            loadStopword.Start();
            loadBankKata.Start();
            
            Task.WaitAll(new Task[] { loadTeks, loadStopword, loadBankKata });

            Console.WriteLine("Cetak hasil sampel kalimat: ");
            foreach (String s in data)
                Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine();

            Parallel.For(0, data.Length, i =>
            {
                TextProcessing(data[i]);
            });
            stopwatch.Stop();
            Console.WriteLine("Waktu {0}ms", stopwatch.ElapsedMilliseconds);


            Console.WriteLine();
            Console.ReadLine();

        }

        private static void TextProcessing(string v)
        {
            v = CaseFolding(v);
            Console.WriteLine();
            Console.WriteLine(v);
            String[] V = Tokenizing(v);
            V = Filtering(V);
            V = Stemming(V);

            foreach (String s in V)
                Console.Write("{0} ", s);
            Console.WriteLine();
        }

        private static string[] Filtering(string[] v){
            List<String> list = new List<String>();
            foreach (String s in v)
            {
                bool ketemu = false;
                foreach (String stopw in Stopword)
                    if (s.Equals(stopw.Trim().ToLower()))
                    {
                        ketemu = true;
                        break;
                    }
                if (!ketemu)
                    list.Add(s);
            }
            return list.ToArray();
        }

        private static String[] Tokenizing(string v)
        {
            v = Regex.Replace(v, @"[^a-zA-Z0-9\-\s]", "");
            v = Regex.Replace(v, @"\b[0-9]+\b", "");
            return v.Split();
        }

        private static string CaseFolding(string v){
            return v.ToLower();
        }

        private static String[] Stemming(String[] v)
        {
            List<String> V = new List<string>();
            foreach(string kata in v)
            {
                bool ketemu = false;
                foreach(String bankw in BankKata)
                {
                    if (kata.Equals(bankw.Trim().ToLower()))
                    {
                        ketemu = true;
                        V.Add(kata);
                        break;
                    } else {
                        var regex = new Regex(@"^(sub|re-?|me(-|n|m|ng|ny)|di|be(l|r)?|ter-?|ke-?|se-?|pen?)?(" 
                            + bankw.Trim().ToLower() 
                            + ")(lah|-?nya|an|kan|i|-?ku|-?mu|kah|tah)?$"
                        );
                        var res = regex.Match(kata);
                        if(res.Success)
                        {
                            ketemu = true;
                            V.Add(bankw.Trim().ToLower());
                            break;
                        }
                    }
                }
                if(!ketemu)
                    V.Add(kata);
            }
            return V.ToArray();
        }

        static String[] loadFile(String path){
            String[] values;
            Console.WriteLine("Sedang memuat file...");

            try
            {
                String filedir = System.AppContext.BaseDirectory;
                String file_string = File.ReadAllText(filedir + path);

                Regex regex = new Regex("(.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection result = regex.Matches(file_string);

                Console.Clear();

                if (result.Count > 0)
                {
                    values = new String[result.Count];
                    int i = 0;
                    foreach (Match match in result)
                    {
                        values[i] = match.Groups[1].Value;
                        i++;
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("File '{0}' dimuat: {1} baris data", path, result.Count);
                    Console.ResetColor();

                    return values;
                }
            } catch(FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File tidak ada :'(");
            } catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("error: {0}", ex.Message);
            }

            values = new String[0];
            return values;
        }

        static String[] loadFile()
        {
            String[] values =
            {
                "Halo apakabar",
                "Halo dunia",
                "selamat pagi :)",
                "Semoga harimu menyenangkan.",
            };
            return values;
        }

    }
}
