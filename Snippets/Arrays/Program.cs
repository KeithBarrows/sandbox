using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp1.EncoderTask;

namespace ConsoleApp1 {
    class Program {
        static void Main (string[] args) {
            string[] nd = "5 4".Split(' ');
            int n = Convert.ToInt32(nd[0]);
            int d = Convert.ToInt32(nd[1]);
            int[] a = Array.ConvertAll("1 2 3 4 5".Split(' '), aTemp => Convert.ToInt32(aTemp));
            int[] result = rotLeft(a, d);
        }

        static int[] rotLeft (int[] a, int d) {
            while (d > a.Length)
                d = d - a.Length;
            var a2 = a.Take (d).ToArray ();
            var a1 = a.Skip (d).Take (a.Length - d).ToArray ();
            var result = new int[a.Length];
            Array.Copy (a1, result, a1.Length);
            Array.Copy (a2, 0, result, a1.Length, a2.Length);
            return result;
        }

        static string FindNumber(int size, int[] source, int target) => source.Contains(target) ? "YES" : "NO";

        static List<int> OddNumbers(int l, int r)
        {
           List<int> arr = Enumerable.Range(l, r-l+1).ToList();
           var ret = arr.Where(a => a % 2 != 0).ToList();
           return ret;
        }

    }
}