namespace SudokuSolver
{
    internal class Sudoku
    {
        private Field[,] board;

        public Sudoku()
        {
            board = new Field[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = new Field();
                }
            }
        }
        public Sudoku(int[,] array)
        {
            board = new Field[9, 9];

            try
            {
                if (array.GetLength(0) != 9 || array.GetLength(1) != 9)
                {
                    throw new ArgumentException();
                }
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {

                        if (array[i, j] < 1 || array[i, j] > 9)
                        {
                            board[i, j] = new Field();
                        }
                        else
                        {
                            board[i, j] = new Field(array[i, j]);
                        }
                    }
                }

            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Needs a 9x9 array " + e.Message);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        board[i, j] = new Field();
                    }
                }
            }
        }

        public Field this[int row, int col]
        {
            get { return board[row, col]; }
            set { board[row, col] = value; }
        }

        public void Print()
        {
            Console.WriteLine("Sudoku:");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(board[i, j].ToString());
                    if (j < 8)
                    {
                        if (j % 3 == 2)
                        {
                            Console.Write(" | ");
                        }
                        else
                        {
                            Console.Write(" . ");
                        }
                    }
                }
                if (i < 8)
                {
                    Console.WriteLine();
                    if (i % 3 == 2)
                    {
                        Console.WriteLine("=================================");
                    }
                    else
                    {
                        Console.WriteLine("---------------------------------");
                    }
                }
            }
            Console.WriteLine();
        }

        public IList<int> CheckRow(int row)
        {
            List<int> NumbersInRow = new List<int>();
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    if (board[row, i].IsSolved)
                    {
                        if (NumbersInRow.Contains(board[row, i].Number))
                        {
                            throw new InvalidSudokuException("Row " + row);
                        }
                        NumbersInRow.Add(board[row, i].Number);
                    }
                }
            }
            catch (InvalidSudokuException e)
            {
                Console.WriteLine(e.Message);
            }
            return NumbersInRow;
        }

        public IList<int> CheckColumn(int col)
        {
            List<int> NumbersInColumn = new List<int>();
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    if (NumbersInColumn.Contains(board[i, col].Number))
                    {
                        throw new InvalidSudokuException("Column " + col);
                    }
                    if (board[i, col].IsSolved)
                    {
                        NumbersInColumn.Add(board[i, col].Number);
                    }
                }
            }
            catch (InvalidSudokuException e)
            {
                Console.WriteLine(e.Message);
            }
            return NumbersInColumn;
        }

        public IList<int> CheckBox(int row, int col)
        {
            List<int> NumbersInBox = new List<int>();
            try
            {
                int startRow = 0;
                int startColumn = 0;
                if (row / 3 == 0)
                {
                    startRow = 0;
                }
                else if (row / 3 == 1)
                {
                    startRow = 3;
                }
                else
                {
                    startRow = 6;
                }
                if (col / 3 == 0)
                {
                    startColumn = 0;
                }
                else if (col / 3 == 1)
                {
                    startColumn = 3;
                }
                else
                {
                    startColumn = 6;
                }
                for (int i = startRow; i < startRow + 3; i++)
                {
                    for (int j = startColumn; j < startColumn + 3; j++)
                    {
                        if (NumbersInBox.Contains(board[i, j].Number))
                        {
                            throw new InvalidSudokuException("Box at " + i + "|" + j);
                        }
                        if (board[i, j].IsSolved)
                        {
                            NumbersInBox.Add(board[i, j].Number);
                        }
                    }
                }
            }
            catch (InvalidSudokuException e)
            {
                Console.WriteLine(e.Message);
            }
            return NumbersInBox;
        }

        public void Solve()
        {
            Print();
            FillPossibilities();
            PrintLong();
            CheckOtherPossibilities();
            if (IsSolved)
            {
                Print();
                Console.WriteLine("Solved successfully");
            }
            else
            {
                PrintLong();
                Console.WriteLine("Needs more tactics ¯\\_(ツ)_/¯");
            }
        }


        private void FillPossibilities()
        {
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (!board[i, j].IsSolved)
                        {
                            if (GetPossibilities(i, j))
                            {
                                changed = true;
                            }
                        }
                    }
                }
            }
        }

        private bool GetPossibilities(int row, int col)
        {
            IList<int> NumbersToRemove = new List<int>();
            IList<int> NumbersInRow = CheckRow(row);
            IList<int> NumbersInColumn = CheckColumn(col);
            IList<int> NumbersInBox = CheckBox(row, col);
            foreach (int number in board[row, col].PossibleNumbers)
            {
                if (CheckOtherBoxesInColumn(row, col, number) || CheckOtherBoxesInRow(row, col, number))
                {
                    NumbersToRemove.Add(number);
                }
            }
            if (board[row, col].RemovePossibleNumbers(NumbersInBox) ||
            board[row, col].RemovePossibleNumbers(NumbersInRow) ||
            board[row, col].RemovePossibleNumbers(NumbersInColumn) ||
            board[row, col].RemovePossibleNumbers(NumbersToRemove))
            {
                return true;
            }

            return false;
        }

        public bool IsSolved
        {
            get
            {
                IList<int> FullList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int i = 0; i < 9; i++)

                {

                    IList<int> NumbersInRow = CheckRow(i).OrderBy(i => i).ToList();
                    IList<int> NumbersInColumn = CheckColumn(i).OrderBy(i => i).ToList();
                    if (!(NumbersInRow.SequenceEqual(FullList) && NumbersInColumn.SequenceEqual(FullList)))
                    {
                        return false;
                    }
                    for (int j = 0; j < 9; j++)
                    {
                        IList<int> NumbersInBox = CheckBox(i, j).OrderBy(i => i).ToList();
                        if (!(board[i, j].IsSolved && NumbersInBox.SequenceEqual(FullList)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        }


        [Serializable]
        class InvalidSudokuException : Exception
        {
            public InvalidSudokuException() { }

            public InvalidSudokuException(string place)
                : base(String.Format("Invalid Number in {0}", place))
            {
                //TODO EXCEPTION HANDLING and testing harder sudokus
            }
        }

        private void CheckOtherPossibilities()
        {
            bool changed = true;
            while (changed)
            {
                changed = false;

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if ((CheckOtherPossibilitiesInBox(i, j) ||
                        CheckOtherPossibilitiesInCollumn(i, j) ||
                        CheckOtherPossibilitiesInRow(i, j)))
                        {
                            FillPossibilities();
                            changed = true;
                        }
                    }
                }
            }
        }

        private bool CheckOtherPossibilitiesInRow(int row, int col)
        {
            bool isContained = false;
            if (!board[row, col].IsSolved)
            {
                foreach (int number in board[row, col].PossibleNumbers)
                {
                    isContained = false;
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != row && board[i, col].ContainsPossibleNumber(number))
                        {
                            isContained = true;
                            break;
                        }
                    }
                    if (!isContained)
                    {
                        board[row, col].Number = number;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckOtherPossibilitiesInCollumn(int row, int col)
        {
            bool isContained = false;
            if (!board[row, col].IsSolved)
            {
                foreach (int number in board[row, col].PossibleNumbers)
                {
                    isContained = false;
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != col && board[row, i].ContainsPossibleNumber(number))
                        {
                            isContained = true;
                            break;
                        }
                    }
                    if (!isContained)
                    {
                        board[row, col].Number = number;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckOtherPossibilitiesInBox(int row, int col)
        {
            int startRow = 0;
            int startColumn = 0;
            if (!board[row, col].IsSolved)
            {
                if (row / 3 == 0)
                {
                    startRow = 0;
                }
                else if (row / 3 == 1)
                {
                    startRow = 3;
                }
                else
                {
                    startRow = 6;
                }
                if (col / 3 == 0)
                {
                    startColumn = 0;
                }
                else if (col / 3 == 1)
                {
                    startColumn = 3;
                }
                else
                {
                    startColumn = 6;
                }
                foreach (int number in board[row, col].PossibleNumbers)
                {
                    bool isContained = false;
                    for (int i = startRow; i < startRow + 3; i++)
                    {
                        for (int j = startColumn; j < startColumn + 3; j++)
                        {
                            if (!(j == col && i == row) && board[i, j].ContainsPossibleNumber(number))
                            {
                                isContained = true;
                                break;
                            }
                        }
                    }
                    if (!isContained)
                    {
                        board[row, col].Number = number;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckOtherBoxesInRow(int row, int col, int number)
        {
            int firstColumn, secondColumn, startRow;
            bool inRow = false;
            bool outsideRow = false;
            if (row / 3 == 0)
            {
                startRow = 0;
            }
            else if (row / 3 == 1)
            {
                startRow = 3;
            }
            else
            {
                startRow = 6;
            }
            if (col / 3 == 0)
            {
                firstColumn = 3;
                secondColumn = 6;
            }
            else if (col / 3 == 1)
            {
                firstColumn = 0;
                secondColumn = 6;
            }
            else
            {
                firstColumn = 0;
                secondColumn = 3;
            }
            for (int i = startRow; i < startRow + 3; i++)
            {

                for (int j = firstColumn; j < firstColumn + 3; j++)
                {
                    if (i == row)
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            inRow = true;
                        }
                    }
                    else
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            outsideRow = true;
                        }
                    }
                }
            }
            if (inRow && !outsideRow)
            {
                return true;
            }
            inRow = false;
            outsideRow = false;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = secondColumn; j < secondColumn + 3; j++)
                {
                    if (i == row)
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            inRow = true;
                        }
                    }
                    else
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            outsideRow = true;
                        }
                    }
                }
            }
            if (inRow && !outsideRow)
            {
                return true;
            }

            return false;

        }
        private bool CheckOtherBoxesInColumn(int row, int col, int number)
        {
            //debuging function(can be ignored)
            /* if (row == 7 && col == 5 && number == 2 && board[0, 4].Number == 5)
            {
                Console.Write("test");
                PrintLong();
                Console.Write("test");

            } */
            int firstRow, secondRow, startColumn;
            bool inColumn = false;
            bool outsideColumn = false;
            if (col / 3 == 0)
            {
                startColumn = 0;
            }
            else if (col / 3 == 1)
            {
                startColumn = 3;
            }
            else
            {
                startColumn = 6;
            }
            if (row / 3 == 0)
            {
                firstRow = 3;
                secondRow = 6;
            }
            else if (row / 3 == 1)
            {
                firstRow = 0;
                secondRow = 6;
            }
            else
            {
                firstRow = 0;
                secondRow = 3;
            }
            for (int j = startColumn; j < startColumn + 3; j++)
            {
                for (int i = firstRow; i < firstRow + 3; i++)
                {
                    if (j == col)
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            inColumn = true;
                        }
                    }
                    else
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            outsideColumn = true;
                        }
                    }
                }
            }
            if (inColumn && !outsideColumn)
            {
                return true;
            }
            inColumn = false;
            outsideColumn = false;
            for (int j = startColumn; j < startColumn + 3; j++)
            {

                for (int i = secondRow; i < secondRow + 3; i++)
                {
                    if (j == col)
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            inColumn = true;
                        }
                    }
                    else
                    {
                        if (board[i, j].ContainsPossibleNumber(number))
                        {
                            outsideColumn = true;
                        }
                    }
                }
            }
            if (inColumn && !outsideColumn)
            {
                return true;
            }

            return false;

        }
        private void PrintLong()
        {
            Console.WriteLine("Sudoku Long:");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    foreach (int number in board[i, j].PossibleNumbers)
                    {
                        Console.Write(number);
                    }
                    if (j < 8)
                    {
                        if (j % 3 == 2)
                        {
                            Console.Write(" | ");
                        }
                        else
                        {
                            Console.Write(" . ");
                        }
                    }
                }
                if (i < 8)
                {
                    Console.WriteLine();
                    if (i % 3 == 2)
                    {
                        Console.WriteLine("=================================");
                    }
                    else
                    {
                        Console.WriteLine("---------------------------------");
                    }
                }
            }
            Console.WriteLine();
        }
    }

}