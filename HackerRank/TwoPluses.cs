using System;
using System.Collections.Generic;

class Solution
{
    /// <summary>
    /// Challenge: https://www.hackerrank.com/challenges/two-pluses
    /// </summary>
    /// <param name="args"></param>
    static void Main2(String[] args)
    {
        // 1. Read input
        bool[,] board = ReadBoard();

        // 2. Get list of all possible pluses
        var pluses = GetPluses(board);

        // 3. find the two pluses that don't overlap and have maximum product
        int max = GetMaximumPlusesProduct(pluses);

        Console.WriteLine(max);
    }

    static bool[,] ReadBoard()
    {
        string[] tokens = Console.ReadLine().Split(' ');
        int n = int.Parse(tokens[0]);
        int m = int.Parse(tokens[1]);
        bool[,] board = new bool[n, m];
        for (int i = 0; i < n; i++)
        {
            var line = Console.ReadLine().ToCharArray();
            for (int j = 0; j < m; j++)
            {
                board[i, j] = line[j] == 'G';
            }
        }

        return board;
    }

    static List<Plus> GetPluses(bool[,] board)
    {
        int n = board.GetLength(0),
            m = board.GetLength(1);

        var pluses = new List<Plus>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (board[i, j])
                {
                    int length = 0;

                    do
                    {
                        pluses.Add(new Plus { X = j, Y = i, Length = length++ });
                    }
                    while ((i - length >= 0) && board[i - length, j] && (i + length < n) && board[i + length, j] &&
                           (j - length >= 0) && board[i, j - length] && (j + length < m) && board[i, j + length]);
                }
            }
        }

        return pluses;
    }

    static int GetMaximumPlusesProduct(List<Plus> pluses)
    {
        int max = 0;
        for (int i = 0; i < pluses.Count; i++)
        {
            for (int j = i + 1; j < pluses.Count; j++)
            {
                if (!pluses[i].Overlaps(pluses[j]))
                {
                    int product = pluses[i].Size * pluses[j].Size;
                    max = Math.Max(max, product);
                }
            }
        }

        return max;
    }

    public class Plus
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Length { get; set; }
        public int Size => (this.Length * 4) + 1;
        public bool Overlaps(Plus other)
        {
            int x1 = this.X,
                y1 = this.Y,
                l1 = this.Length,
                x2 = other.X,
                y2 = other.Y,
                l2 = other.Length;

            // If both have same center, return true;
            if (x1 == x2 && y1 == y2) return true;

            // If both are on the same x-axis or y-axis,
            // check if their length sum is bigger than or equal to
            // the distance between the two centers.
            if (x1 == x2 || y1 == y2) return Distance(x1, y1, x2, y2, l1 + l2);

            // By here we know that the two centers are on different x-axis and y-axis
            // So there are two possible points of intersection (x1, y2) and (x2, y1)
            // Check if each of the potential intersections is within the distance from both centers
            if (Distance(x1, y1, x2, y1, l1) && Distance(x2, y2, x2, y1, l2)) return true;
            if (Distance(x1, y1, x1, y2, l1) && Distance(x2, y2, x1, y2, l2)) return true;

            return false;
        }

        private bool Distance(int x1, int y1, int x2, int y2, int d)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) <= d * d;
        }
    }
}