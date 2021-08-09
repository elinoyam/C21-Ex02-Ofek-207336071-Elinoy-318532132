using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Engine
{
    public class AIComputer
    {
        //https://medium.com/analytics-vidhya/artificial-intelligence-at-play-connect-four-minimax-algorithm-explained-3b5fc32e4a4f
        // return column and in output the odd (value)
        public int minimax(Board i_Board, int i_Row, int i_Column, char i_Sign, int i_Depth, double i_Alpha, double i_Beta, bool i_MaximizingPlayer, out double o_WinningPercentageValue)
        {
            Random random = new Random();
            int column, returnedColumn;
            List<int> possibleMoves = i_Board.GetPossibleMoves(); //GetPossibleRowsMoves
            bool isBoardFull = i_Board.CheckIfBoardIsFull();
            double newScore;


            if(i_Depth == 0)
            {
                o_WinningPercentageValue = i_Board.CheckWinAndGetScore(i_Row, i_Column, 'O');
                column = -1;
            }
            else if((isBoardFull))
            {
                if (i_Board.CheckWin(i_Row, i_Column, i_Sign))
                {
                    o_WinningPercentageValue = double.PositiveInfinity;
                    column = -1;
                }

                else if(i_Board.CheckWin(i_Row, i_Column, 'O')) //TODO how to understand which is the X and which is the 0?
                {
                    o_WinningPercentageValue = double.NegativeInfinity;
                    column = -1;
                }
                else //Tie- the board is full
                {
                    o_WinningPercentageValue = 0f; // TODO tie = 0?
                    column = -1;
                }
            }
            else
            {
                if(i_MaximizingPlayer)
                {
                    o_WinningPercentageValue = double.NegativeInfinity;
                    column = random.Next(1, i_Board.Columns);
                    foreach(int col in possibleMoves)
                    {
                        Board copiedBoard = i_Board.Copy();
                        int row = copiedBoard.GetRowByPlayerColumnChoice(col);
                        copiedBoard.MakeMove(col, 'O');
                        returnedColumn = minimax(copiedBoard,row,col,'O', i_Depth - 1, i_Alpha, i_Beta, false, out newScore);
                        if(newScore > o_WinningPercentageValue)
                        {
                            o_WinningPercentageValue = newScore;
                            column = col;
                        }
                        i_Alpha = Math.Max(i_Alpha, o_WinningPercentageValue);
                        if  (i_Alpha >= i_Beta)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    o_WinningPercentageValue = double.PositiveInfinity;
                    column = random.Next(1, i_Board.Columns);
                    foreach(int col in possibleMoves)
                    {
                        Board copiedBoard = i_Board.Copy();
                        int row = copiedBoard.GetRowByPlayerColumnChoice(col);
                        copiedBoard.MakeMove(col, 'X');
                        returnedColumn = minimax(copiedBoard,row,col,'X', i_Depth - 1, i_Alpha, i_Beta, true, out newScore);
                        if(newScore < o_WinningPercentageValue)
                        {
                            o_WinningPercentageValue =  newScore;
                            column = col;
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


        /*
        // how deep the minmax alg looks
        private const int DEEPNESS = 4;
        int Strength = 30000;

        MinMaxAlgorithm alg = new MinMaxAlgorithm();

        public AIComputer()
        {
            Strength = 40000;
            alg.SetStrength(Strength);
        }

        // use minmax to find best move
        public int TakeTurn(Board GameBoard)
        {
            List<int> possibleMoves = GameBoard.GetPossibleMoves();

            // represents the best move value (start at obsurdly low)
            float best = float.MinValue;

            // the column that the AI will play
            int bestColumn = -1;

            // loop through each column and check the value of the move
            foreach (int col in possibleMoves)
            {
                float value = FindMoveValue(GameBoard, col, 0);

                // if the column has a better value for the AI than the current best move
                // use that column as the best
                if (value > best)
                {
                    best = value;
                    bestColumn = col;
                }
            }

            // return the best column fo the AI to play
            return bestColumn;
        }

        // finds the value of a specific move in a column
        // deep is how many moves ahead the alg is looking
        public float FindMoveValue(Board GameBoard, int col, int deep)
        {
            float result = 0;
            // create temporary Board, make the best move and check for next best
            //Board newBoard = GameBoard.Copy();

            // check if the move will win the game
            //WinState? win = newBoard.CheckWinState();
            bool ifWin = GameBoard.CheckWin();

            if(!ifWin && GameBoard.CheckIfBoardIsFull())
            {
                result = 0f;
            }
            // if the game is going to end with the move
            else if (ifWin)
            {
                // return 1 (best) for win, and -1 (worst) for lose
                //else if (win == WinState.BLACKWIN && Game1.AIColor == BoardState.BLACK)
                if (ifWin == WinState.BLACKWIN && GameBoard.CurrentPlayer == BoardState.BLACK)
                    return 1f;
                //else if (win == WinState.REDWIN && Game1.AIColor == BoardState.RED)
                else if (ifWin == WinState.REDWIN && GameBoard.CurrentPlayer == BoardState.RED)
                    return 1f;
                else
                    return -1f;
            }
            
           

            // if we have looked forward the maximum amount
            // return the value of the move
            if (deep == DEEPNESS)
            {
                // MCScore
                int newStrength = Convert.ToInt32(Strength / ((double)Math.Pow(7, DEEPNESS)));
                alg.SetStrength(newStrength);

                return alg.FindDeepValue(GameBoard, col);
            }

            //newBoard.MakeMove(col);
            GameBoard.MakeMove(col,'O');

            // Get the possible moves for the newBoard (the next move would be players)
            List<int> possibleMoves = GameBoard.GetPossibleMoves(); //newBoard.GetPossibleMoves();

            // start looking into deeper moves
            float value = float.MinValue;
            foreach (int col2 in possibleMoves)
                value = Math.Max(value, -1f * FindMoveValue(GameBoard, col2, deep + 1));

            // remove the last move made so it doesnt stay permanent
            GameBoard.Unmove(col);

            return value;
        }*/

}
}
