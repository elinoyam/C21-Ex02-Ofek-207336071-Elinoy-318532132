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
                if (i_Board.CheckWin(i_Row, i_Column, 'O'))
                {
                    o_WinningPercentageValue = double.PositiveInfinity;
                    column = -1;
                }

                else if(i_Board.CheckWin(i_Row, i_Column, 'X')) //TODO how to understand which is the X and which is the 0?
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

}
}
