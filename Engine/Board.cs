using System;
using System.Collections.Generic;

namespace Engine
{
    public class Board
    {
		private const int k_AmountToWin = 4;
        private readonly int m_RowsNumber;
        private readonly int m_ColumnsNumber;
        private char[,] m_Board;

        public int Rows
        {
            get
            {
                return m_RowsNumber;
            }
        }
        public int Columns
        {
            get
            {
                return m_ColumnsNumber;
            }
        }

        public Board(int i_RowsNumber, int i_ColumnsNumber)
        {
            // check if legal? 4-8
            if(!(isValidBoard(i_RowsNumber, i_ColumnsNumber)))
            {
				throw new Exception("Error! the number of rows and columns should be between 4 to 8");
			}

			m_RowsNumber = i_RowsNumber;
			m_ColumnsNumber = i_ColumnsNumber;
			m_Board = new char[m_RowsNumber, m_ColumnsNumber];
			InitBoard();
        }

		public void InitBoard()
		{
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					SetCell(i+1, j+1,  ' ');
				}
			}	
		}

		private bool isValidBoard(int i_RowsNumber, int i_ColumnsNumber)
        {
            bool isValid = (4 <= i_RowsNumber && 8 >= i_RowsNumber) && (4 <= i_ColumnsNumber && 8 >= i_ColumnsNumber);

            return isValid;
        }

        public void PrintBoard()
        {
            //Print first row
            for (int currentColumn = 0; currentColumn < Columns + 1; ++currentColumn)
            { 
                if(currentColumn == 0)
                {
                    Console.Write("   "); //3 blank spaces
                }
                else
                {
                    Console.Write($"{currentColumn}     "); //5 blank spaces
				}
            }

            Console.WriteLine("");
            for (int currentRow = 0; currentRow < Rows; ++currentRow)
            {
                for (int currentColumn = 0; currentColumn < Columns; currentColumn++)
                {
                    Console.Write($"|  {GetCell(currentRow+1, currentColumn+1)}  "); //2  blank spaces from each side

				}

				Console.WriteLine("|"); // add closing to the last one
                Console.Write("=");
				for (int currentColumn = 0; currentColumn < Columns; currentColumn++)
                {
                    Console.Write("======"); //3  blank spaces from each side

                }
				Console.WriteLine(""); 
			}
        }

        public char GetCell(int i_ChosenRow, int i_ChosenColumn)
        {
            if (!checkIfValidIndex(i_ChosenRow, i_ChosenColumn))
            {
                throw new Exception("Error! The given indexs are not valid - outside of board boundries.");
            }

            return m_Board[i_ChosenRow-1, i_ChosenColumn-1];
        }

        public void SetCell(int i_ChosenRow, int i_ChosenColumn, char i_CellInput)
        {
            if (!checkIfValidIndex(i_ChosenRow, i_ChosenColumn))
            {
                throw new Exception("Error! The given indexs are not valid - outside of board boundries.");
            }

            m_Board[i_ChosenRow-1, i_ChosenColumn-1] = i_CellInput;
        }

        private bool checkIfValidIndex(int i_ChosenRow, int i_ChosenColumn)
        {
            bool isValid = true;
            if(i_ChosenRow > Rows || i_ChosenRow < 1)
            {
                isValid = false;
            }
            else if (i_ChosenColumn > Columns || i_ChosenColumn < 1)
            {
                isValid = false;
            }

            return isValid;
        }

		private bool checkIfColumnIsFull(int i_ColumnNumber)
        {
			bool columnIsFull = false;

            if (!checkIfValidIndex(1, i_ColumnNumber))
            {
				throw new Exception("Error! The given indexs are not valid - outside of board boundries.");
			}
			if(m_Board[0,i_ColumnNumber-1] != ' ')
            {
				columnIsFull = true;
            }

			return columnIsFull;
		}

		public bool CheckIfBoardIsFull()
        {
			bool boardIsFull = true;

			for(int i = 0; i < Columns && boardIsFull;  i++)
            {
                if (!checkIfColumnIsFull(i + 1))
                {
					boardIsFull = false;
				}
            }
				
			return boardIsFull;
        }

        private bool checkRowToWin(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign, out double o_ScoreCount)
        {
            int i = i_LastColumnInsert - 1;
            int count = 1;
            bool winExists = false;

			while ((i > 0) && i_UserSign == GetCell(i_LastRowInsert, i) )
            {
                ++count;
                --i;
            }

            if(count < k_AmountToWin)
            {
                i = i_LastColumnInsert + 1;
                while((i <= Columns) && i_UserSign == GetCell(i_LastRowInsert, i) )
                {
                    ++count;
                    ++i;
                }
                winExists = (count >= k_AmountToWin);
			}
            else
            {
                winExists = true;
            }

            o_ScoreCount = count;
            return winExists;
        } 

		private bool checkColumnToWin(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign, out double o_ScoreCount)
        {
			bool winExists = false;
			int countLastingSign = 0, rowIndex = i_LastRowInsert;

			while ((rowIndex <= Rows) && i_UserSign == GetCell(rowIndex, i_LastColumnInsert) )
			{
				countLastingSign++;
				rowIndex++;
			}

			if (countLastingSign >= k_AmountToWin)
            {
				winExists = true;
			}

            o_ScoreCount = countLastingSign;

            return winExists;
        }

        private bool checkNegSlopeDiagnol(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign, out double o_ScoreCount) // it's this \
		{
            bool winExists = false;
			int i = i_LastRowInsert - 1;
            int j = i_LastColumnInsert - 1;
            int count = 1;

            while (i > 0 && j > 0 && i_UserSign == GetCell(i, j))
            {
                count++;
                i--;
                j--;
            }

            if(count < k_AmountToWin)
            {
                i = i_LastRowInsert + 1;
                j = i_LastColumnInsert + 1;

                while(i <= Rows && j <= Columns && i_UserSign == GetCell(i, j))
                {
                    count++;
                    i++;
                    j++;
                }

                winExists = (count >= k_AmountToWin);
            }
			else
            {
                winExists = true;
            }

            o_ScoreCount = count;
            return winExists;
        } 

		private bool checkPosSlopeDiagnol(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign, out double o_ScoreCount) 
        {
			bool winExists = false;
			int countLastingSign = 1, rowIndex = i_LastRowInsert + 1, columnIndex = i_LastColumnInsert - 1;
			
			while ((rowIndex <= Rows && columnIndex > 0) && i_UserSign == GetCell(rowIndex, columnIndex))
			{
				countLastingSign++;
				rowIndex++;
				columnIndex--;
			}
			if (countLastingSign >= k_AmountToWin)
            {
				winExists = true;

			}

			rowIndex = i_LastRowInsert - 1;
			columnIndex = i_LastColumnInsert + 1;

			while (!winExists && (rowIndex > 0 && columnIndex <= Columns) && i_UserSign == GetCell(rowIndex, columnIndex))
			{
				countLastingSign++;
				rowIndex--;
				columnIndex++;
			}

			if (countLastingSign >= k_AmountToWin)
			{
				winExists = true;
			}

            o_ScoreCount = countLastingSign;
            return winExists;
		}


        public bool CheckWin(int row, int col, char sign)
        {
            double counter;
            return checkRowToWin(row, col, sign, out counter) ||
                   checkColumnToWin(row, col, sign, out counter) ||
                   checkNegSlopeDiagnol(row, col, sign, out counter) ||
                   checkPosSlopeDiagnol(row, col, sign, out counter);
        }

        public int GetRowByPlayerColumnChoice(int i_ColumnChoice)
        {
			int returnRowIndex = -1;

            for(int rowIndex = Rows - 1; rowIndex >= 0; --rowIndex)
            {
				if (GetCell(rowIndex+1, i_ColumnChoice) == ' ')
                {
                    returnRowIndex = rowIndex+1;
                    break;
                }
            }

            return returnRowIndex;
        }

        // remove the last move made and change back to the other player
        public void Unmove(int i_Col)
        {
			//find the place
            for(int i = 0; i < m_ColumnsNumber; ++i)
            {
                if (GetCell(i+1, i_Col) != ' ')
                {
                    SetCell(i + 1, i_Col, ' ');
                    //returnRowIndex = rowIndex + 1;
                    break;
                }
			}
            //tilesPerColumn[col]--;

            //long position = ((long)1 << (tilesPerColumn[col] + 7 * col));

            //if (CurrentOpponent == BoardState.BLACK)
            //    numBlack ^= position;
            //else
            //    numRed ^= position;

            //theBoard[tilesPerColumn[col], col] = BoardState.EMPTY;

        }

        // Gets all possible moves (7 or less) for the AI player
        public List<int> GetPossibleMoves()
        {
            List<int> possibleMoves = new List<int>();

            for (int i = 1; i <= m_ColumnsNumber; i++)
            {
                if (!checkIfColumnIsFull(i))
                    possibleMoves.Add(i);
            }
            return possibleMoves;
        }

        public Board Copy()
        {
            Board newBoard = new Board(Rows, Columns);
            //newBoard.CurrentPlayer = this.CurrentPlayer;
            //newBoard.CurrentOpponent = this.CurrentOpponent;
            //newBoard.numRed = this.numRed;
            //newBoard.numBlack = this.numBlack;
            //for (int i = 0; i < Width; i++)
            //    newBoard.tilesPerColumn[i] = this.tilesPerColumn[i];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    newBoard.m_Board[i, j] = this.m_Board[i, j];
            }
            //newBoard.isGameOver = this.isGameOver;
            //newBoard.Winner = this.Winner;

            return newBoard;
        }

        public void MakeMove(int i_ColumnIndex, char i_ActivePlayerSign)
        {
            // Check if the move is valid, then place the piece
            if (checkIfValidIndex(1,i_ColumnIndex))
            {
                // change the tile to be held by the current player
                m_Board[GetRowByPlayerColumnChoice(i_ColumnIndex) -1, i_ColumnIndex - 1 ] = i_ActivePlayerSign;
            }
        }

        public double CheckWinAndGetScore(int i_Row, int i_Column, char i_Sign)
        {
            double scoreCountTotal = 0;
            int countCenter = 0;
            int centerColumn = (Columns % 2 == 0) ? (Columns / 2) : ((Columns / 2) + 1);


            // Score center column
            for (int i = 1; i <= Rows; ++i)
            {
                if (GetCell(i, centerColumn) == i_Sign)
                {
                    ++countCenter;
                }
            }

            scoreCountTotal += countCenter * 3;


            // Score Horizontal - ROW
            for(int i = 1; i <= Rows; ++i)
            {
                for(int j = 1; j <= Columns-3; ++j)
                {
                    char[] window = new char[4];
                    for(int k = 0; k < 4; ++k)
                    {
                        window[k] = GetCell(i, j+k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            
            // Score Vertical = COL
            for (int i = 1; i <= Columns; ++i)
            {
                for (int j = 1; j <= Rows - 3; ++j)
                {
                    char[] window = new char[4];
                    for (int k = 0; k < 4; ++k)
                    {
                        window[k] = GetCell(j+k, i);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            // Score negative sloped diagonal   \
            for (int i = 1; i <= Rows - 3; ++i)
            {
                for (int j = 1; j <= Columns - 3; ++j)
                {
                    char[] window = new char[4];
                    for (int k = 0; k < 4; ++k)
                    {
                        window[k] = GetCell(i + k, j+k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }
            
            // Score positive sloped diagonal  /
            for (int i = 1; i <= Rows - 3; ++i)
            {
                for (int j = 1; j <= Columns - 3; ++j)
                {
                    char[] window = new char[4];
                    for (int k = 0; k < 4; ++k)
                    {
                        window[k] = GetCell(i + 3 - k, j + k);
                    }

                    scoreCountTotal += evaluate_window(window, i_Sign);
                }
            }

            return scoreCountTotal;
        }


        public int evaluate_window(char[] i_Window, char i_Sign)
        {
            int score = 0;
            int counterMySign = 0;
            int counterOpponentSign = 0;
            int counterEmptySign = 0;

            char opp_piece = 'X'; //TODO const (define) to the AI SIGN and Player sign
            if(i_Sign == opp_piece)
            {
                opp_piece = 'O';
            }

            for(int i = 0; i < 4; ++i)
            {
                if(i_Window[i] == i_Sign)
                {
                    ++counterMySign;
                }
                else if(i_Window[i] == opp_piece)
                {
                    ++counterOpponentSign;
                }
                else
                {
                    ++counterEmptySign;
                }
            }

            switch(counterMySign)
            {
                case 4:
                    score += 100;
                    break;
                case 3:
                    score += 5;
                    break;
                case 2:
                    score += 2;
                    break;
            }

            if ((counterOpponentSign == 3) && (counterEmptySign == 1))
            {
                score -= 8;
            }

            return score;
        }
    }
}

