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
        static void Main(string[] args){


            //var loader = new Loader();
            //loader.start();

            /**
             * memuat data sampel sebagai array string
             * 
             */
            String[] data = loadFile("../../Assets/words_dictionary.json");
            /**
             * periksa string hasil
             * 
             */
            //Console.WriteLine("{0}", String.Join(",", data));

            /**
             * program dibawah
             * 
             */
            //Console.WriteLine("{0}", data[138128]);
            orderData(data);

            //loader.loading = false;
            //loader = null;
            Console.Title = "Rekursif v Iterasi";

            var watch = new Stopwatch();

            watch.Start();
            int result = binarySearchRekusif(data, "money", data.Length);
            watch.Stop();

            if (result == -1)
                Console.WriteLine("Element not present");
            else
                Console.WriteLine("Element found at "
                                + "index " + result + ": {0}", data[result]);
            Console.WriteLine("Lama: "+watch.ElapsedMilliseconds + "." + watch.ElapsedTicks + "ms");
            Console.Read();

        }

        private static string[] orderData(string[] data){
            Console.WriteLine("Sedang mengurutkan data...");
            Array.Sort(data, (x, y) => String.Compare(x, y));
            Console.Clear();
            return data;
        }

        /**
         * rekursif
         * 
         */
        public static int linearSearchRekursif(String[] data, String x, int i = 0){
            if (i >= data.Length || i <= 1000) return -1;
            else if (data[i] == x) return i;
            i++;
            return linearSearchRekursif(data, x, i);
        }

        static int binarySearchRekusif(String[] arr, String x, int r, int m = 0, int l = 0)
        {
            if (l > r) return -1;
            m = l + ((r - l) / 2);
            int res = x.CompareTo(arr[m]);
            if (res == 0)
                return m;

            else
                if (res > 0)
                    l = m + 1;
                else
                    r = m - 1;
            return binarySearchRekusif(arr, x, r, m, l);
        }

        public static int linearSearch(String[] data, String x){
            int n = data.Length;
            for (int i = 0; i < n; i++)
                if (data[i] == x)
                    return i;
            return -1;
        }

        static int binarySearch(String[] arr, String x)
        {
            int l = 0, r = arr.Length - 1;
            while (l <= r)
            {
                int m = l + ((r - l) / 2);

                int res = x.CompareTo(arr[m]);

                // Check if x is present at mid
                if (res == 0) return m;
                else
                    // If x greater, ignore left half
                    if (res > 0) l = m + 1;
                    // If x is smaller, ignore right half
                    else r = m - 1;
            }
            return -1;
        }

        static String[] loadFile(String path){
            String[] values;
            Console.WriteLine("Sedang memuat file...");

            try
            {
                String filedir = System.AppContext.BaseDirectory;
                String json_string = File.ReadAllText(filedir + path);

                /**
                 * 
                 * sampel
                 * 
                 */
                //String json_sample = "{\n" +
                //    "\"edd\": 1, \n" +
                //    "\"hello\": 1 \n" +
                //    "}";

                Regex regex = new Regex("(\")(\\w+)(\")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection result = regex.Matches(json_string);

                Console.Clear();

                if (result.Count > 0)
                {
                    values = new String[result.Count];
                    int i = 0;
                    foreach (Match match in result)
                    {
                        values[i] = match.Groups[2].Value;
                        i++;
                    }
                    Console.WriteLine("s: {0}", result.Count);

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

        class Loader
        {
            public bool loading = false;
            int counter = 0;
            public Loader(){
                
            }
            public void start(){
                this.loading = true;
                this.loadRuning();
            }
            public void loadRuning(){
                Task.Delay(50).ContinueWith((task) => {
                    if (loading)
                        this.loadRuning();
                    else
                    {
                        Console.Title = "Rekursif v Iterasi";
                        return;
                    }
                        
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.SetCursorPosition(0, 0);
                    if (counter >= 4) counter = 0;
                    String bar;
                    if (counter == 1) bar = "/";
                    else if (counter == 2) bar = "|";
                    else if (counter == 3) bar = "\\";
                    else bar = "-";
                    Console.Title = "Memuat... " + bar;
                    Console.ResetColor();
                    counter++;
                });
            }
        }
    }
}
