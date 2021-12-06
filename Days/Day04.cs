using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day04 : Day
    {
        class Board
        {
            const int boardSize = 5;
            bool[,] marked = new bool[boardSize, boardSize];

            // key: number, value: (row, col)
            Dictionary<int, (int, int)> numbers = new Dictionary<int, (int, int)>();

            public static Board ParseBoard(StreamReader sr)
            {
                Board board = new Board();
                for (int row = 0; row < boardSize; ++row)
                {
                    string[] numbers = sr.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int col = 0; col < boardSize; ++col)
                        board.numbers.Add(int.Parse(numbers[col]), (row, col));
                }
                return board;
            }

            // Check for win in row 'row' or in collumn 'col'.
            bool CheckForWin(int row, int col)
            {
                // check row
                bool win = true;
                for (int i = 0; i < boardSize && win; ++i)
                    if (!marked[row, i])
                        win = false;
                if (win)
                    return true;

                // check col
                win = true;
                for (int i = 0; i < boardSize && win; ++i)
                    if (!marked[i, col])
                        win = false;
                return win;
            }

            // return true, if marking the value results in win
            public bool Mark(int n)
            {
                if (numbers.ContainsKey(n))
                {
                    var (row, col) = numbers[n];
                    marked[row, col] = true;
                    return CheckForWin(row, col);
                }
                return false;
            }

            public int Score()
            {
                int score = 0;
                foreach (var (n, (row, col)) in numbers)
                {
                    if (!marked[row, col])
                        score += n;
                }
                return score;
            }
        }

        IEnumerable<int> drawn;
        List<Board> boards;

        public override void SetInput(string inputPath)
        {
            boards = new List<Board>();
            StreamReader sr = new StreamReader(inputPath);
            drawn = sr.ReadLine()
                        .Split(',')
                        .Select<string, int>(s => int.Parse(s));
            sr.ReadLine();
            while (sr.Peek() != -1)
            {
                boards.Add(Board.ParseBoard(sr));
                sr.ReadLine();
            }
        }

        public override string Solve1()
        {
            foreach (int drawnNumber in drawn)
            {
                foreach (Board b in boards)
                {
                    if (b.Mark(drawnNumber))
                    {
                        int score = b.Score();
                        return $"{score} * {drawnNumber} = {score * drawnNumber}";
                    }
                }
            }
            return "No win";
        }

        public override string Solve2()
        {
            HashSet<Board> boardSet = new HashSet<Board>(boards);
            IEnumerator<int> drawnNumber = drawn.GetEnumerator();
            if (!drawnNumber.MoveNext())
                return "Not every border won";
            while (boardSet.Count > 1)
            {
                HashSet<Board> wonBoards = new HashSet<Board>();
                foreach (Board b in boardSet)
                {
                    if (b.Mark(drawnNumber.Current))
                        wonBoards.Add(b);
                }
                boardSet.ExceptWith(wonBoards);
                if (!drawnNumber.MoveNext())
                    return "Not every border won";
            }

            // last board
            Board board = boardSet.ToArray<Board>()[0];
            do
            {
                if (board.Mark(drawnNumber.Current))
                {
                    int score = board.Score();
                    return $"{score} * {drawnNumber.Current} = {score * drawnNumber.Current}";
                }
            } while (drawnNumber.MoveNext());
            return "Not every border won";
        }
    }
}
