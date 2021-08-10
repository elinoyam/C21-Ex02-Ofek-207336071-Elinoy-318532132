using System;
using System.Collections.Generic;

namespace Engine
{
    public class Board
    {
		public const int k_AmountToWin = 4;
        private readonly int r_RowsNumber;
        private readonly int r_ColumnsNumber;
        private char[,] m_Board;

        public int Rows
        {
            get
            {
                return r_RowsNumber;
            }
        }
        public int Columns
        {
            get
            {
                return r_ColumnsNumber;
            }
        }

        public Board(int i_RowsNumber, int i_ColumnsNumber)
        {
            if(!(isValidBoard(i_RowsNumber, i_ColumnsNumber))) 
            {
				throw new Exception("Error! the number of rows and columns should be between 4 to 8");
			}

			r_RowsNumber = i_RowsNumber;
			r_ColumnsNumber = i_ColumnsNumber;
			m_Board = new char[r_RowsNumber, r_ColumnsNumber];
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
                    Console.Write($"|  {GetCell(currentRow + 1, currentColumn + 1)}  "); //2  blank spaces from each side
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

            return m_Board[i_ChosenRow - 1, i_ChosenColumn - 1];
        }

        public void SetCell(int i_ChosenRow, int i_ChosenColumn, char i_CellInput)
        {
            if (!checkIfValidIndex(i_ChosenRow, i_ChosenColumn))
            {
                throw new Exception("Error! The given indexs are not valid - outside of board boundries.");
            }

            m_Board[i_ChosenRow - 1, i_ChosenColumn - 1] = i_CellInput;
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

			if(m_Board[0, i_ColumnNumber - 1] != ' ')
            {
				columnIsFull = true;
            }

			return columnIsFull;
		}

		public bool CheckIfBoardIsFull()
        {
			bool boardIsFull = true;

			for(int i = 0; i < Columns && boardIsFull; i++)
            {
                if (!checkIfColumnIsFull(i + 1))
                {
					boardIsFull = false;
				}
            }
				
			return boardIsFull;
        }

        private bool checkRowToWin(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign)
        {  
            int columnIndex = i_LastColumnInsert - 1;
            int countLastingSign = 1;
            bool winExists = false;

			while ((columnIndex > 0) && i_UserSign == GetCell(i_LastRowInsert, columnIndex))
            {
                ++countLastingSign;
                --columnIndex;
            }

            if(countLastingSign < k_AmountToWin)
            {
                columnIndex = i_LastColumnInsert + 1;
                while((columnIndex <= Columns) && i_UserSign == GetCell(i_LastRowInsert, columnIndex))
                {
                    ++countLastingSign;
                    ++columnIndex;
                }

                winExists = (countLastingSign >= k_AmountToWin);
			}
            else
            {
                winExists = true;
            }

            return winExists;
        } 

		private bool checkColumnToWin(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign)
        {
			bool winExists = false;
			int countLastingSign = 0, rowIndex = i_LastRowInsert;

			while ((rowIndex <= Rows) && i_UserSign == GetCell(rowIndex, i_LastColumnInsert))
			{
				countLastingSign++;
				rowIndex++;
			}

			if (countLastingSign >= k_AmountToWin)
            {
				winExists = true;
			}

            return winExists;
        }

        private bool checkNegativeSlopeDiagnol(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign) // Negative Slope = \
        {
            bool winExists = false;
			int rowIndex = i_LastRowInsert - 1;
            int columnIndex = i_LastColumnInsert - 1;
            int countLastingSign = 1;

            while (rowIndex > 0 && columnIndex > 0 && i_UserSign == GetCell(rowIndex, columnIndex))
            {
                countLastingSign++;
                rowIndex--;
                columnIndex--;
            }

            if(countLastingSign < k_AmountToWin)
            {
                rowIndex = i_LastRowInsert + 1;
                columnIndex = i_LastColumnInsert + 1;

                while(rowIndex <= Rows && columnIndex <= Columns && i_UserSign == GetCell(rowIndex, columnIndex))
                {
                    countLastingSign++;
                    rowIndex++;
                    columnIndex++;
                }

                winExists = (countLastingSign >= k_AmountToWin);
            }
			else
            {
                winExists = true;
            }

            return winExists;
        } 

		private bool checkPositiveSlopeDiagnol(int i_LastRowInsert, int i_LastColumnInsert, char i_UserSign) // Positive Slope = /
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

            return winExists;
		}

        public bool CheckWin(int i_RowIndex, int i_ColumnIndex, char i_UserSign)
        {
            return checkRowToWin(i_RowIndex, i_ColumnIndex, i_UserSign) ||
                   checkColumnToWin(i_RowIndex, i_ColumnIndex, i_UserSign) ||
                   checkNegativeSlopeDiagnol(i_RowIndex, i_ColumnIndex, i_UserSign) ||
                   checkPositiveSlopeDiagnol(i_RowIndex, i_ColumnIndex, i_UserSign);
        }

        public int GetRowByPlayerColumnChoice(int i_ColumnChoice)
        {
			int returnRowIndex = -1;

            for(int rowIndex = Rows - 1; rowIndex >= 0; --rowIndex)
            {
				if (GetCell(rowIndex + 1, i_ColumnChoice) == ' ')
                {
                    returnRowIndex = rowIndex + 1;
                    break;
                }
            }

            return returnRowIndex;
        }

        public Board Copy()
        {
            Board newBoard = new Board(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newBoard.m_Board[i, j] = this.m_Board[i, j];
                }
            }

            return newBoard;
        }

        public void MakeMove(int i_ColumnIndex, char i_ActivePlayerSign)
        {
            if (checkIfValidIndex(1, i_ColumnIndex))
            {
                m_Board[GetRowByPlayerColumnChoice(i_ColumnIndex) - 1, i_ColumnIndex - 1] = i_ActivePlayerSign;
            }
        }

        public List<int> GetPossibleMoves()     
        {
            List<int> possibleMovesList = new List<int>();

            for (int i = 1; i <= Columns; i++)
            {
                if (!checkIfColumnIsFull(i))
                {
                    possibleMovesList.Add(i);
                }
            }

            return possibleMovesList;
        }
    }
}