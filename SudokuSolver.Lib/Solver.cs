using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    public class SudokuSolver
    {
        private struct Square : IEquatable<Square>
        {
            public readonly int x, y;
            public readonly List<int> PossibleValues;

            public Square(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }

            public Square(int x, int y, int val)
            {
                this.x = x;
                this.y = y;
                this.PossibleValues = new List<int> { val };
            }

            public Square(int x, int y, IEnumerable<int> vals)
            {
                this.x = x;
                this.y = y;
                this.PossibleValues = new List<int>(vals);
            }

            public Square Copy()
            {
                return new Square(x, y, PossibleValues);
            }

            public override bool Equals(object obj)
            {
                if (obj is Square)
                    return false;

                return this.Equals((Square)obj);
            }

            public override int GetHashCode()
            {
                return x.GetHashCode() * 6 + y.GetHashCode();
            }

            public bool Equals(Square other)
            {
                return other.x == this.x &&
                    other.y == this.y &&
                    other.PossibleValues.SequenceEqual(this.PossibleValues);
            }
        }

        public static string ToString(int[] puzzle)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("+=============+");
            for (int y = 0; y < 9; y++)
            {
                sb.Append("||");
                for (int x = 0; x < 9; x++)
                {
                    int val = puzzle[9 * y + x];
                    sb.Append(val != 0 ? val.ToString() : " ");
                    if (x == 2 || x == 5)
                    {
                        sb.Append("|");
                    }

                }
                sb.Append("||");

                sb.AppendLine();
                if (y == 2 || y == 5)
                {
                    sb.AppendLine("||---+---+---||");
                }
            }
            sb.AppendLine("+=============+");

            return sb.ToString();
        }

        private class Sudoku : IEquatable<Sudoku>
        {
            public List<Square> squares;

            public Sudoku()
            {
                squares = new List<Square>();
            }

            public Sudoku(IEnumerable<Square> squares)
            {
                this.squares = new List<Square>(squares);
            }

            public Sudoku Copy()
            {
                return new Sudoku(this.squares.Select(sq => sq.Copy()).ToList());
            }

            public bool Equals(Sudoku other)
            {
                return this.squares.SequenceEqual(other.squares);
            }

            internal int[] ToArray()
            {
                return squares.OrderBy(sq => sq.y).ThenBy(sq => sq.x).Select(sq => sq.PossibleValues.Count == 1 ? sq.PossibleValues.First() : 0).ToArray();
            }
        }

        public static int[] Solve(int[] puzzle)
        {

            List<Sudoku> stack = new List<Sudoku>();

            {
                Sudoku sudoku = new Sudoku();

                for (int y = 1; y <= 9; y++)
                {
                    for (int x = 1; x <= 9; x++)
                    {
                        int val = puzzle[(y - 1) * 9 + (x - 1)];
                        if (val == 0)
                        {
                            sudoku.squares.Add(new Square(x, y));
                        }
                        else {
                            sudoku.squares.Add(new Square(x, y, val));
                        }
                    }
                }
                stack.Add(sudoku);
            }


            while (true)
            {
                bool badPath = false;

                if (!stack.Any())
                {
                    //Console.WriteLine("Stumped.");
                    return null; //TODO: do we want to return our last calculation?
                }

                var original = stack[0];
                stack.RemoveAt(0);

                var copy = original.Copy();

                foreach (var s in copy.squares)
                {
                    if (s.PossibleValues.Count == 1)
                    {
                        continue;
                    }

                    var boxNeighbors = copy.squares.Where(sq =>
                        !(sq.x == s.x && sq.y == s.y) &&
                        Math.Ceiling(sq.x / 3f) == Math.Ceiling(s.x / 3f) &&
                        Math.Ceiling(sq.y / 3f) == Math.Ceiling(s.y / 3f)
                    );

                    var columnNeighbors = copy.squares.Where(sq =>
                        sq.x == s.x && sq.y != s.y
                    );

                    var rowNeighbors = copy.squares.Where(sq =>
                        sq.y == s.y && sq.x != s.x
                    );

                    var allNeighbors = boxNeighbors.Union(columnNeighbors).Union(rowNeighbors);

                    //Console.WriteLine($"Neighbors of ({s.x},{s.y}):");
                    //foreach(var n in neighbors)
                    //{
                    //    Console.WriteLine($"{n.x},{n.y}");
                    //}

                    var neighborValues = allNeighbors.Where(sq => sq.PossibleValues.Count == 1).SelectMany(sq => sq.PossibleValues).Distinct();

                    s.PossibleValues.RemoveAll(x => neighborValues.Contains(x));

                    if (s.PossibleValues.Count == 1)
                    {
                        continue;
                    }

                    var uniquePossibilities = s.PossibleValues.Where(x =>
                        !boxNeighbors.Any(n => n.PossibleValues.Count > 1 && n.PossibleValues.Contains(x)) ||
                        !columnNeighbors.Any(n => n.PossibleValues.Count > 1 && n.PossibleValues.Contains(x)) ||
                        !rowNeighbors.Any(n => n.PossibleValues.Count > 1 && n.PossibleValues.Contains(x))
                    ).ToList();

                    if (uniquePossibilities.Any())
                    {
                        if (uniquePossibilities.Count() > 1)
                        {
                            badPath = true;
                            break;
                        }

                        s.PossibleValues.Clear();
                        s.PossibleValues.Add(uniquePossibilities.Single());
                    }

                    if (!s.PossibleValues.Any())
                    {
                        badPath = true;
                        break;
                    }
                }

                if (badPath)
                {
                    continue;
                }


                //sudoku.Print();

                if (!copy.squares.Any(sq => sq.PossibleValues.Count > 1))
                {
                    return copy.ToArray();
                }
                else if (copy.Equals(original))
                {
                    //copy.Print();
                    // Split
                    var squareWithLeastEntropy = copy.squares.Where(sq => sq.PossibleValues.Count > 1).OrderBy(sq => sq.PossibleValues.Count).First();
                    foreach (int possibleValue in squareWithLeastEntropy.PossibleValues)
                    {
                        Sudoku branch = copy.Copy();
                        var mutatedSquare = branch.squares.Where(sq => sq.x == squareWithLeastEntropy.x && sq.y == squareWithLeastEntropy.y).Single();
                        branch.squares.Remove(mutatedSquare);
                        branch.squares.Add(new Square(mutatedSquare.x, mutatedSquare.y, possibleValue));

                        stack.Insert(0, branch);
                    }
                }
                else
                {
                    stack.Insert(0, copy);
                }
            }
        }
    }
}
