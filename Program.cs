﻿namespace SudokuSolver
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            int[,] array = new int[9, 9]{
                {0,0,0 ,0,8,0 ,0,0,9},
                {0,6,0 ,9,0,0 ,1,8,0},
                {0,0,4 ,0,3,0 ,0,0,0},
                {1,0,0 ,5,0,4 ,0,0,6},
                {0,0,0 ,3,0,7 ,5,4,0},
                {0,0,0 ,0,0,0 ,0,0,0},
                {5,0,7 ,0,0,0 ,0,1,0},
                {8,4,0 ,0,0,3 ,0,0,0},
                {0,0,9 ,0,0,0 ,7,0,2},
            };
            Sudoku test = new Sudoku(array);

            test.Solve();
        }
    }
}