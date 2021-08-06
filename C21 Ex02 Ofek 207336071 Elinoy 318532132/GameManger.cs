using System;
using System.IO;
using System.Runtime;
using System.Text;
using Engine;

namespace C21_Ex02_Ofek_207336071_Elinoy_318532132
{
    class GameManger
    {
        private Board m_Board;
        private readonly Player m_FirstPlayer;
        private readonly Player m_SecondPlayer;
        private bool m_firstPlayerTurn = true;

        public GameManger(Board i_Board, Player i_First, Player i_Second)
        {
            m_Board = i_Board;
            m_FirstPlayer = i_First;
            m_SecondPlayer = i_Second;
        }

        public void getPlayerChoice()
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
                   // choiceValid = m_Board. TODO check if valid input (column is full) and the nuber is in the right range
                    rowChoice = m_Board.EnterPlayerChoice(columnChoice);
                }

            }

           // return choice;
        }

        public bool CheckForTie()
        {
            bool isTie = m_Board.CheckIfBoardIsFull()  ? true  :  false;

            return isTie;
        }

        public bool CheckForWin()
        {
            return true;
        }

        public void GameLoop()
        {
            bool playing = true;
            bool tie = false;

            //loop while the board isn't full
            while (playing && !tie)
            {
                //print board
                m_Board.PrintBoard();

                //get choice
                getPlayerChoice();

                //check if valid

                //set cell to bard

                //check if win
                //playing = !CheckWin(row, col, sign);

                //check if tie
                tie = CheckForTie();

                //clear screen
                Ex02.ConsoleUtils.Screen.Clear();
            }

            // print board
            m_Board.PrintBoard();



            // tell who won and show the count 1-2

            // new game will be at main



        }
    }
}
