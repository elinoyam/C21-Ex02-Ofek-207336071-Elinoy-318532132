﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Board
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


        }

        private bool isValidBoard(int i_RowsNumber, int i_ColumnsNumber)
        {
            bool isValid = false;
            if((4 <= i_RowsNumber && 8 >= i_RowsNumber) && (4 <= i_ColumnsNumber && 8 >= i_ColumnsNumber))
            {
                isValid = true;
            }

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
                    Console.Write($"{currentColumn}   "); //3 blank spaces
				}
            }

            Console.WriteLine("");
            for (int currentRow = 1; currentRow <= Rows; ++currentRow)
            {
                for (int currentColumn = 0; currentColumn < COLS + 1; currentColumn++)
                {
                    Console.Write("|   {getCell(currentRow currentColumn)}   "); //3  blank spaces from each side

				}
				Console.WriteLine("========================="); // needs to be as the length of the row
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
    }
}



/*
 *
 *#define _CRT_SECURE_NO_WARNINGS
#include "stdio.h"
#include "stdlib.h"
#include <stdbool.h>

//Ofek Shavit 207330671
//This define is for comparing sequence of the same sign to win against how much I have.
#define AMOUNT_TO_WIN 4

/// This function initializes the game board by assigning each cell
/// with ' ' (resulting with an empty game board).
void initBoard();

/// This function gets a row number and a column number (a cell),
/// and returns the character in that cell (could be 'X', 'O' or ' ').
/// For example:
/// char c = getCell(1, 1);
char getCell(int row, int col);

/// This function gets a row number, a column number and a sign,
/// and assigns the cell with the given sign.
/// For example:
/// setCell(1, 1, 'X');
void setCell(int row, int col, char sign);

/// This function clears the screen.
void clearScreen();

// This function prints the board frame.
void printBoard();

// This function switch the players.
void switchPlayer(char* activePlayer, char* sign);

// This function gets the player's choice.
int getPlayerChoice();

// This function checks the collom that the player chose.
// It checks that it is between 1 and 7 and checks that the collom is not full.
bool checkChoice(int col);

//This function gets the first avialable row in a spesfic collom.
int getAvilableRow(int col);

// This function checks for a sequence signs in a row.
bool checkRow(int row, int col, char sign);

// This function checks for a sequence signs in a collom.
bool checkCol(int row, int col, char sign);

// This function checks for a sequence signs in a negative slope diagonal (\).
bool checkNegSlopeDiagnol(int row, int col, char sign);

// This function checks for a sequence signs in a positive slope diagonal (/).
bool checkPosSlopeDiagnol(int row, int col, char sign);

// This function checks for a win. it checks the row, collom and diagonals sequence signs.
bool checkWin(int row, int col, char sign);

// checkTie
 //This function checks for a tie
 //it gets noting
 //it returns true/false

bool checkTie();

int main()
{
	int row = 0;
	int col = 0;
	char choice = 0;
	char sign = 'O';
	char activePlayer = '2';
	bool playing = true;
	bool tie = false;
	initBoard();

	while (playing && !tie)
	{
		switchPlayer(&activePlayer, &sign);
		printBoard();

		printf("\nPlayer number %c:\n", activePlayer);
		col = getPlayerChoice();
		row = getAvilableRow(col);

		setCell(row, col, sign);
		playing = !checkWin(row, col, sign);
		tie = checkTie();
		clearScreen();
	}//while

	printBoard();
	if (tie)
	{
		printf("\nIt's a tie, no one won! be better next time \n");
	}
	else
	{
		printf("\nPlayer %c won!\n", activePlayer);
	}
}//main


void initBoard()
{
	int i, j;

	for (i = 0; i < ROWS; i++)
	{
		for (j = 0; j < COLS; j++)
		{
			setCell(i + 1, j + 1, ' ');
		}
	}
}


void printBoard()
{
	int row;
	int col;

	//Print first row
	for (col = 0; col < COLS + 1; col++)
	{ //its better to do it opossite because it saves a check
		if (col == 0)
			printf(" ");
		else
			printf("   %d", col);
	}
	printf("\n");

	for (row = 1; row <= ROWS; row++)
	{
		for (col = 0; col < COLS + 1; col++)
		{
			if (0 == col)
				printf("%c", 'A' + row - 1);
			else
				printf("   %c", getCell(row, col));

		}
		printf("\n");
	}
}//printBoard

void switchPlayer(char* activePlayer, char* sign)
{
	if (*activePlayer == '1')
	{
		(*activePlayer)++;
		*sign = 'O';
	}
	else
	{
		(*activePlayer)--;
		*sign = 'X';
	}
}//switchPlayer

int getPlayerChoice()
{
	bool choiceValid = false;
	int choice = 0;

	while (!choiceValid)
	{
		printf("Please enter column input (a number between 1-%d): ", COLS);
		scanf("%d", &choice);
		choiceValid = checkChoice(choice);
	}

	return choice;
}//getPlayerChoice

bool checkChoice(int col)
{
	if (!((col >= 1) && (col <= COLS)))
	{
		printf("Choice need to be between 1 and %d\n", COLS);
		return false;
	}

	if (' ' != getCell(1, col))
	{
		printf("Col is full\n");
		return false;
	}

	return true;
}//checkChoice

int getAvilableRow(int col)
{
	int row = 0;
	for (row = ROWS + 1; row > 0; row--)
	{
		if (getCell(row, col) == ' ')
		{
			return row;
		}
	}
}//getAvilableRow

bool checkRow(int row, int col, char sign)
{
	int i = col - 1;
	int count = 1;

	while (sign == getCell(row, i) && i > 0)
	{
		count++;
		i--;
	}
	if (count == AMOUNT_TO_WIN)
		return true;
	i = col + 1;
	while (sign == getCell(row, i) && i <= COLS)
	{
		count++;
		i++;
	}
	return count >= AMOUNT_TO_WIN;
}//checkRow

bool checkCol(int row, int col, char sign)
{
	int i = row - 1;
	int count = 1;

	while (sign == getCell(i, col) && i > 0)
	{
		count++;
		i--;
	}
	if (count == AMOUNT_TO_WIN)
		return true;
	i = row + 1;
	while (sign == getCell(i, col) && i <= ROWS)
	{
		count++;
		i++;
	}
	return count >= AMOUNT_TO_WIN;
}//checkCol

bool checkNegSlopeDiagnol(int row, int col, char sign)
{
	int i = row - 1;
	int j = col - 1;
	int count = 1;

	while (sign == getCell(i, j) && i > 0 && j > 0)
	{
		count++;
		i--;
		j--;
	}

	if (count == AMOUNT_TO_WIN)
		return true;

	i = row + 1;
	j = col + 1;

	while (sign == getCell(i, j) && i <= ROWS && j <= COLS)
	{
		count++;
		i++;
		j++;
	}
	return count >= AMOUNT_TO_WIN;
}//checkNegSlopeDiagnol

bool checkPosSlopeDiagnol(int row, int col, char sign)
{
	int i = row + 1;
	int j = col - 1;
	int count = 1;

	while (sign == getCell(i, j) && i <= ROWS && j > 0)
	{
		count++;
		i++;
		j--;
	}

	if (count == AMOUNT_TO_WIN)
		return true;

	i = row - 1;
	j = col + 1;

	while (sign == getCell(i, j) && i > 0 && j <= COLS)
	{
		count++;
		i--;
		j++;
	}
	return count >= AMOUNT_TO_WIN;
}//checkPosSlopeDiagnol

bool checkWin(int row, int col, char sign)
{
	return checkRow(row, col, sign) ||
		   checkCol(row, col, sign) ||
			  checkNegSlopeDiagnol(row, col, sign) ||
		   checkPosSlopeDiagnol(row, col, sign);
}//checkWin

bool checkTie()
{
	int i;
	for (i = 1; i <= COLS; i++)
	{
		if (getCell(1, i) == ' ')
			return false;
	}
	return true;
}//checkTie
*
 *
 *
 */