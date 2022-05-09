namespace SudokuSolver
{
    internal class Field
    {
        public IList<int> PossibleNumbers { get; private set; }

        public override string ToString()
        { return PossibleNumbers.Count == 1 ? PossibleNumbers[0].ToString() : " "; }

        public int Number
        {
            get => PossibleNumbers.Count == 1 ? PossibleNumbers[0] : 0; set => PossibleNumbers = new List<int> { value };
        }

        public bool IsSolved { get => PossibleNumbers.Count == 1; }

        public Field()
        {
            PossibleNumbers = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                PossibleNumbers.Add(i);
            }
        }

        public Field(int number)
        {
            PossibleNumbers = new List<int> { number };
        }

        public bool RemovePossibleNumber(int number)
        {
            return PossibleNumbers.Remove(number);
        }

        public bool RemovePossibleNumbers(IList<int> numbers)
        {
            bool changed = false;
            foreach (int number in numbers)
            {
                if (PossibleNumbers.Remove(number))
                {
                    changed = true;
                }
            }
            return changed;
        }

        public bool ContainsPossibleNumber(int number)
        {
            return PossibleNumbers.Contains(number);
        }
    }
}