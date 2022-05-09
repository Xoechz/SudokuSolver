﻿namespace SudokuSolver
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            int[,] array = new int[9, 9]{
                {2,3,5 ,0,0,0 ,0,7,0},
                {0,0,8 ,0,0,0 ,0,0,0},
                {0,0,0 ,0,2,3 ,0,4,0},

                {8,6,4 ,0,0,0 ,0,0,0},
                {0,0,7 ,0,0,6 ,0,8,5},
                {0,0,0 ,0,7,2 ,0,0,0},

                {0,5,0 ,0,6,7 ,0,1,8},
                {0,0,1 ,0,0,0 ,0,0,0},
                {9,0,0 ,1,0,0 ,0,2,3}
            };
            Sudoku test = new Sudoku(array);

            test.Solve();
        }
    }
}