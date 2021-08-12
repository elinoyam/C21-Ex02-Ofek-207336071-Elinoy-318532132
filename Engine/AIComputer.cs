using System;
using System.Collections.Generic;

namespace Engine
{
    public class AIComputer
    {
        public const int k_UndefinedRowOrColumn = -1;
        private const int k_Connect4Move = 100;
        private const int k_Connect3Move = 5;
        private const int k_Connect2Move = 2;
        private const int k_OpponentConnect3Move = 10;

        // return the column and in the output the win percentage value for this column
        public int MinimaxAlgorithm(ref Board i_Board, int i_Row, int i_Column, char i_Sign, int i_Depth, double i_Alpha, double i_Beta, bool i_MaximizingPlayer, out double o_WinningPercentageValue)
        {
            Random random = new Random();
            int column, returnedColumn;
            List<int> possibleMoves = i_Board.GetPossibleMoves();
            bool isBoardFull = i_Board.CheckIfBoardIsFull();
            const bool v_MaxPlayer = true;
            double newScore;

            if(i_Depth == 0)
            {
                o_WinningPercentageValue = CheckWinAndGetScore(ref i_Board, i_Row, i_Column, Player.k_Player2Sign);
                column = k_UndefinedRowOrColumn;
            }
            else if((isBoardFull))
            {
                if(i_Board.CheckWin(i_Row, i_Column, Player.k_Player2Sign))
                {
                    o_WinningPercentageValue = double.PositiveInfinity;
                    column = k_UndefinedRowOrColumn;
                }
                else if(i_Board.CheckWin(i_Row, i_Column, Player.k_Player1Sign)) 
                {
                    o_WinningPercentageValue = double.NegativeInfinity;
                    column = k_UndefinedRowOrColumn;
                }
                else // Tie- the board is full
                {
                    o_WinningPercentageValue = 0f; 
                    column = k_UndefinedRowOrColumn;
                }
            }
            else
            {
                if(i_MaximizingPlayer)
                {
                    o_WinningPercentageValue = double.NegativeInfinity;
                    column = random.Next(1, i_Board.Columns);
                    foreach(int currentColumn in possibleMoves)
                    {
                        Board copiedBoard = i_Board.Copy();
                        int availableRowByCloumnChoice = copiedBoard.GetRowByPlayerColumnChoice(currentColumn);
                        copiedBoard.MakeMove(currentColumn, Player.k_Player2Sign);
                        returnedColumn = MinimaxAlgorithm(ref copiedBoard,availableRowByCloumnChoice,currentColumn, Player.k_Player2Sign, i_Depth - 1, i_Alpha, i_Beta, !v_MaxPlayer, out newScore);
                        if(newScore > o_WinningPercentageValue)
                        {
                            o_WinningPercentageValue = newScore;
                            column = currentColumn;
                        }

                        i_Alpha = Math.Max(i_Alpha, o_WinningPercentageValue);
                        if(i_Alpha >= i_Beta)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    o_WinningPercentageValue = double.PositiveInfinity;
                    column = random.Next(1, i_Board.Columns);
                    foreach(int currentColumn in possibleMoves)
                    {
                        Board copiedBoard = i_Board.Copy();
                        int availableRowByCloumnChoice = copiedBoard.GetRowByPlayerColumnChoice(currentColumn);
                        copiedBoard.MakeMove(currentColumn, Player.k_Player1Sign);
                        returnedColumn = MinimaxAlgorithm(ref copiedBoard,availableRowByCloumnChoice,currentColumn, Player.k_Player1Sign, i_Depth - 1, i_Alpha, i_Beta, v_MaxPlayer, out newScore);
                        if(newScore < o_WinningPercentageValue)
                        {
                            o_WinningPercentageValue =  newScore;
                            column = currentColumn;
                        }

                        i_Beta = Math.Min(i_Beta, o_WinningPercentageValue);
                        if(i_Alpha >= i_Beta)
                        {
                            break;
                        }
                    }
                }
            }

            return column;
        }

        public double CheckWinAndGetScore(ref Board i_Board, int i_Row, int i_Column, char i_Sign)
        {
            double scoreCountTotal = 0;
            int countCenter = 0;
            int centerColumn = (i_Board.Columns % 2 == 0) ? (i_Board.Columns / 2) : ((i_Board.Columns / 2) + 1);

            // Score center column
            for (int i = 1; i <= i_Board.Rows; ++i)
            {
                if (i_Board.GetCell(i, centerColumn) == i_Sign)
                {
                    ++countCenter;
                }
            }

            scoreCountTotal += countCenter * 2;
            // Score Horizontal = ROW
            for (int i = 1; i <= i_Board.Rows; ++i)
            {
                for (int j = 1; j <= i_Board.Columns - (Board.k_AmountToWin - 1); ++j)
                {
                    char[] window = new char[Board.k_AmountToWin];
                    for (int k = 0; k < Board.k_AmountToWin; ++k)
                    {
                        window[k] = i_Board.GetCell(i, j + k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            // Score Vertical = COL
            for (int i = 1; i <= i_Board.Columns; ++i)
            {
                for (int j = 1; j <= i_Board.Rows - (Board.k_AmountToWin - 1); ++j)
                {
                    char[] window = new char[Board.k_AmountToWin];
                    for (int k = 0; k < Board.k_AmountToWin; ++k)
                    {
                        window[k] = i_Board.GetCell(j + k, i);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            // Score negative sloped diagonal   \
            for (int i = 1; i <= i_Board.Rows - (Board.k_AmountToWin - 1); ++i)
            {
                for (int j = 1; j <= i_Board.Columns - (Board.k_AmountToWin - 1); ++j)
                {
                    char[] window = new char[Board.k_AmountToWin];
                    for (int k = 0; k < Board.k_AmountToWin; ++k)
                    {
                        window[k] = i_Board.GetCell(i + k, j + k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            // Score positive sloped diagonal = /
            for (int i = 1; i <= i_Board.Rows - (Board.k_AmountToWin - 1); ++i)
            {
                for (int j = 1; j <= i_Board.Columns - (Board.k_AmountToWin - 1); ++j)
                {
                    char[] window = new char[Board.k_AmountToWin];
                    for (int k = 0; k < Board.k_AmountToWin; ++k)
                    {
                        window[k] = i_Board.GetCell(i + (Board.k_AmountToWin - 1) - k, j + k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            return scoreCountTotal;
        }

        private int evaluate_window(char[] i_Window, char i_Sign)
        {
            int score = 0;
            int counterMySign = 0;
            int counterOpponentSign = 0;
            int counterEmptySign = 0;

            char opponent_piece = Player.k_Player1Sign;
            if (i_Sign == opponent_piece)
            {
                opponent_piece = Player.k_Player2Sign;
            }

            for (int i = 0; i < Board.k_AmountToWin; ++i)
            {
                if (i_Window[i] == i_Sign)
                {
                    ++counterMySign;
                }
                else if (i_Window[i] == opponent_piece)
                {
                    ++counterOpponentSign;
                }
                else
                {
                    ++counterEmptySign;
                }
            }

            switch (counterMySign)
            {
                case Board.k_AmountToWin:
                    score += k_Connect4Move;
                    break;
                case (Board.k_AmountToWin - 1):
                    score += k_Connect3Move;
                    break;
                case (Board.k_AmountToWin - 2):
                    score += k_Connect2Move;
                    break;
            }

            if ((counterOpponentSign == (Board.k_AmountToWin - 1)) && (counterEmptySign == 1))
            {
                score -= k_OpponentConnect3Move;
            }

            return score;
        }
    }
}
