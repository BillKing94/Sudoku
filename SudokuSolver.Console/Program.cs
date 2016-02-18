using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Console
{
    class Program
    {

        public static void Main()
        {
            //int[] puzzle = new int[] {
            //    3,1,0, 0,0,0, 0,0,0,
            //    0,2,0, 0,8,9, 0,3,0,
            //    0,0,9, 0,6,0, 0,0,1,

            //    9,0,7, 0,0,0, 5,0,0,
            //    0,0,0, 2,7,6, 0,0,0,
            //    0,0,6, 0,0,0, 7,0,0,

            //    5,0,0, 0,9,0, 1,0,0,
            //    0,6,0, 8,4,0, 0,2,0,
            //    0,0,0, 0,0,0, 0,7,8
            //};

            int[] puzzle = new int[]
            {
                0,0,2, 5,0,0, 0,0,4,
                0,4,0, 0,2,0, 0,5,0,
                8,0,0, 0,0,6, 7,0,0,

                6,0,0, 0,0,1, 8,0,0,
                0,1,0, 0,7,0, 0,6,0,
                0,0,4, 2,0,0, 0,0,3,

                0,0,8, 9,0,0, 0,0,7,
                0,3,0, 0,1,0, 0,9,0,
                9,0,0, 0,0,5, 3,0,0
            };

            System.Console.Write(SudokuSolver.ToString(puzzle));

            var solved = SudokuSolver.Solve(puzzle);

            System.Console.Write(SudokuSolver.ToString(solved));
        }
    }
}
