using System;
using System.Collections.Generic;
using System.Linq;

namespace HackerRank
{
    class BearAndSteadyGene
    {
        /// <summary>
        /// Challenge: https://www.hackerrank.com/challenges/bear-and-steady-gene
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            string gene = Console.ReadLine();
            int count = n / 4;

            var extras = new Dictionary<char, int>
            {
                { 'A', 0 },
                { 'G', 0 },
                { 'C', 0 },
                { 'T', 0 }
            };

            var tracker = new Dictionary<char, int>(extras);

            foreach (var l in gene) extras[l]++;

            extras['A'] -= count;
            extras['G'] -= count;
            extras['C'] -= count;
            extras['T'] -= count;

            // If we have balanced gene, exit
            if (extras.All(i => i.Value == 0))
            {
                Console.WriteLine(0);
                return;
            }

            int length = int.MaxValue,
                start = 0;

            for (int end = 0; end < n; end++)
            {
                tracker[gene[end]]++;

                // Keep incrementing end until we find a substring that has the extra letters
                if (!ExtraLettersFound(extras, tracker)) continue;

                // Keep shrinking the substring from the start and see if the extra letters still found.
                // Keep track of the minimum substring found.
                while (start < end && ExtraLettersFound(extras, tracker))
                {
                    length = Math.Min(length, end - start + 1);
                    tracker[gene[start++]]--;
                }
            }

            Console.WriteLine(length);
        }

        static bool ExtraLettersFound(Dictionary<char, int> extras, Dictionary<char, int> tracker)
        {
            return extras.All(i => i.Value <= 0 || i.Value <= tracker[i.Key]);
        }
    }
}
