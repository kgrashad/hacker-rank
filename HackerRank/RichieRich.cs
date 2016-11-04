using System;
using System.Linq;

namespace HackerRank
{
    class RichieRich
    {
        /// <summary>
        /// Challenge: https://www.hackerrank.com/challenges/richie-rich
        /// </summary>
        /// <param name="args"></param>
        static void Main(String[] args)
        {
            // 1. Read input.
            int k;
            var digits = ReadInput(out k);

            // 2. Check if possible to make it palindromes.
            if (digits == null || digits.Count(d => !d.Symmetric) > k)
            {
                Console.WriteLine(-1);
                return;
            }

            // 3. Make symmetric and maximize value.
            Fix(digits, k);

            // 4. Print.
            Print(digits);
        }

        static Digit[] ReadInput(out int k)
        {
            string[] tokens = Console.ReadLine().Split(' ');
            int n = int.Parse(tokens[0]);
            k = int.Parse(tokens[1]);

            if (n == 0) return null;

            string word = Console.ReadLine();
            int len = (int)Math.Ceiling(n / 2.0);

            var digits = new Digit[len];
            
            for (int i = 0; i < len; i++)
            {
                digits[i] = new Digit
                {
                    LeftValue = word[i],
                    RightValue =word[n-i-1],
                    Middle = (i == (n-i-1)),
                    Changed = false
                };
            }

            return digits;
        }

        static void Fix(Digit[] digits, int k)
        {
            int cost = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                cost += digits[i].Symmetrize();
            }

            int budget = k - cost;
            for (int i = 0; i < digits.Length; i++)
            {
                budget -= digits[i].Maximize(budget);
            }
        }

        static void Print(Digit[] digits)
        {
            var left = digits.Select(d => d.LeftValue).ToArray();
            var skip = digits.Last().Middle ? 1 : 0;
            var right = digits.Reverse().Skip(skip).Select(d => d.LeftValue).ToArray();
            Console.WriteLine(string.Join("", left.Concat(right)));
        }

        public class Digit
        {
            public char LeftValue { get; set; }

            public char RightValue { get; set; }

            public bool Changed { get; set; }

            public bool Middle { get; set; }

            public bool Symmetric => this.LeftValue == this.RightValue;

            public int Symmetrize()
            {
                if (this.Symmetric) return 0;

                if (this.LeftValue > this.RightValue) this.RightValue = this.LeftValue;
                else this.LeftValue = this.RightValue;

                this.Changed = true;

                return 1;
            }

            public int Maximize(int budget)
            {
                if (this.LeftValue == '9') return 0;

                // If we changed a character, we already paid 1 unit of cost
                // If this is the middle char, then we already need 1 unit of cost.
                int requiredCost = this.Changed || this.Middle ? 1 : 0;
                int cost =  2 - requiredCost;

                if (cost > budget) return 0;

                this.LeftValue = this.RightValue = '9';
                return cost;
            }
        }
    }
}
