﻿using System;
using Engine;

namespace C21_Ex02_Ofek_207336071_Elinoy_318532132
{
    class GameManger
    {
        private Board m_Board;
        private readonly Player m_FirstPlayer;
        private readonly Player m_SecondPlayer;
        private bool m_firstPlayerTurn = true; // true = player1, false = player2

        public GameManger(Board i_Board, Player i_First, Player i_Second)
        {
            m_Board = i_Board;
            m_FirstPlayer = i_First;
            m_SecondPlayer = i_Second;
        }

        public int getPlayerChoice(out int o_ColumnChoice)
        {
            bool choiceValid = false;
            int columnChoice = 0;
            int rowChoice = 0;
            string inputFromUser;
            bool goodInput = false;

            while (!choiceValid)
            {
                Console.WriteLine($"Please enter column input (a number between 1-{m_Board.Columns}): ");
                inputFromUser = Console.ReadLine();
                goodInput = int.TryParse(inputFromUser, out columnChoice);

                if (goodInput)
                {
                    // 1 <= columnChoice <= Columns 
                    if(1 <= columnChoice && m_Board.Columns >= columnChoice)
                    {
                        rowChoice = m_Board.GetRowByPlayerColumnChoice(columnChoice);
                        if(rowChoice != -1)
                        {
                            choiceValid = true;
                        }
                        else
                        {
                            Console.WriteLine($"Error! the column {columnChoice} is full! please select a different column");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error! you must enter a number between 1-{m_Board.Columns}! ");
                    }
                    // choiceValid = m_Board. TODO check if valid input (column is full) and the nuber is in the right range
                }
                else
                {
                    Console.WriteLine($"Error! you must enter an integer number! ");
                }
            }

            o_ColumnChoice = columnChoice;
            return rowChoice;
        }

        public bool CheckForTie()
        {
            bool isTie = m_Board.CheckIfBoardIsFull()  ? true  :  false;

            return isTie;
        }

        public bool CheckForWin(int i_Row, int i_Column, char i_Sign)
        {
            return m_Board.CheckWin(i_Row, i_Column, i_Sign);
        }

        public void GameLoop()
        {
            m_Board.InitBoard();
            bool playing = true;
            bool tie = false;
            int playerColumnChoice;
            int availableRow;
            char activeSign;
                
            //loop while the board isn't full
            while (playing && !tie)
            {
                //print board
                m_Board.PrintBoard();

                //get choice check if valid 
                availableRow = getPlayerChoice(out playerColumnChoice);

                //set cell to bard
                activeSign = GetActivePlayerSign();
                m_Board.SetCell(availableRow, playerColumnChoice, activeSign);

                //check if win
                playing = !(CheckForWin(availableRow, playerColumnChoice, activeSign));

                //check if tie
                tie = CheckForTie();

                //switch players
                changeActivePlayer();

                //clear screen
                Ex02.ConsoleUtils.Screen.Clear();
            }

            // print board
            m_Board.PrintBoard();


            // TODO tell who won and show the count 1-2
            PrintWins();

            // TODO new game will be at main



        }

        public char GetActivePlayerSign()
        {
            char activeSign = m_firstPlayerTurn  ? m_FirstPlayer.PlayerSign :  m_SecondPlayer.PlayerSign;

            return activeSign;
        }

        private void changeActivePlayer()
        {
            m_firstPlayerTurn = !m_firstPlayerTurn;    
        }

        public void PrintWins()
        {
            Player winner = (!m_firstPlayerTurn) ? m_SecondPlayer  :  m_FirstPlayer;
            winner.IncreaseWinsCounter();
            Console.WriteLine($"Congratulations! {winner.PlayerName} won this game !!!");
            Console.WriteLine($"{m_FirstPlayer.PlayerName} has won: {m_FirstPlayer.PlayerWinsCounter}. ");
            Console.WriteLine($"{m_SecondPlayer.PlayerName} has won: {m_SecondPlayer.PlayerWinsCounter}. ");
        }


    }
}
