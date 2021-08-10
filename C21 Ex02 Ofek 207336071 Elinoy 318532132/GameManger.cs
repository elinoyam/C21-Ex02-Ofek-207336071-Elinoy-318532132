using System;
using Engine;

namespace C21_Ex02_Ofek_207336071_Elinoy_318532132
{
    class GameManger
    {
        private const char k_Player1Sign = 'X';
        private const char k_Player2Sign = 'O';
        private Board m_Board;
        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer;
        private bool m_FirstPlayerTurn = true; // true = player1, false = player2

        public GameManger(Board i_Board, Player i_First, Player i_Second)
        {
            m_Board = i_Board;
            r_FirstPlayer = i_First;
            r_SecondPlayer = i_Second;
        }

        public int GetPlayerChoice(out int o_ColumnChoice, out bool o_ToQuit)
        {
            bool choiceValid = false;
            int columnChoice = 0;
            int rowChoice = 0;
            string inputFromUser;
            bool goodInput = false;

            o_ToQuit = false;
            o_ColumnChoice = -1;

            while (!choiceValid)
            {
                Console.WriteLine($"Please enter column input (a number between 1-{m_Board.Columns}) or Q to quit: ");
                inputFromUser = Console.ReadLine();
                goodInput = int.TryParse(inputFromUser, out columnChoice);

                if(goodInput)
                {
                    if(1 <= columnChoice && (m_Board.Columns >= columnChoice))
                    {
                        rowChoice = m_Board.GetRowByPlayerColumnChoice(columnChoice);
                        if(rowChoice != -1)
                        {
                            o_ColumnChoice = columnChoice;
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
                }
                else
                {
                    if (inputFromUser == "Q")
                    {
                        o_ToQuit = true;
                        o_ColumnChoice = -1;
                        choiceValid = true;
                    }
                    else
                    {
                        Console.WriteLine($"Error! you must enter an integer number! ");
                    }
                }
            }
            
            return rowChoice;
        }

        public bool CheckForTie()
        {
            return m_Board.CheckIfBoardIsFull();
        }

        public bool CheckForWin(int i_Row, int i_Column, char i_Sign)
        {
            return m_Board.CheckWin(i_Row, i_Column, i_Sign);
        }

        public void GameLoop()
        {
            bool playing = true;
            bool tie = false;
            bool isQuit = false;
            int playerColumnChoice;
            int availableRow;
            char activeSign = k_Player1Sign;

            Ex02.ConsoleUtils.Screen.Clear();
            m_Board.InitBoard();
            //loop while the board isn't full and the game isn't over
            while (playing && !tie)
            {
                m_Board.PrintBoard(); 
                if ((r_SecondPlayer.IsComputer) && (GetActivePlayerSign() == k_Player2Sign)) 
                {
                    r_SecondPlayer.GetComputerChoice(ref m_Board ,out availableRow,out playerColumnChoice);
                    activeSign = k_Player2Sign;
                }
                else
                {
                    availableRow = GetPlayerChoice(out playerColumnChoice, out isQuit);
                    if (isQuit)
                    {
                        playing = false;
                        Ex02.ConsoleUtils.Screen.Clear();
                        changeActivePlayer();
                        break;
                    }

                    activeSign = GetActivePlayerSign();
                    m_Board.SetCell(availableRow, playerColumnChoice, activeSign);
                }

                playing = !(CheckForWin(availableRow, playerColumnChoice, activeSign));
                tie = CheckForTie();
                changeActivePlayer();
                Ex02.ConsoleUtils.Screen.Clear();
            }

            if(!isQuit)
            {
                m_Board.PrintBoard();
                changeActivePlayer();
            }

            if (tie && playing)     
            {
                Console.WriteLine("It's a tie!!! Better luck next time....");
                Console.WriteLine($"{r_FirstPlayer.PlayerName} has won: {r_FirstPlayer.PlayerWinsCounter}. ");
                Console.WriteLine($"{r_SecondPlayer.PlayerName} has won: {r_SecondPlayer.PlayerWinsCounter}. ");
            }
            else
            {
                PrintWins();
            }
        }

        public char GetActivePlayerSign()
        {
            char activeSign = m_FirstPlayerTurn  ? r_FirstPlayer.PlayerSign :  r_SecondPlayer.PlayerSign;

            return activeSign;
        }

        private void changeActivePlayer()
        {
            m_FirstPlayerTurn = !m_FirstPlayerTurn;    
        }

        public void PrintWins()
        {
            Player winner = (!m_FirstPlayerTurn) ? r_SecondPlayer  :  r_FirstPlayer;

            winner.IncreaseWinsCounter();
            Console.WriteLine($"Congratulations! {winner.PlayerName} won this game !!!");
            Console.WriteLine($"{r_FirstPlayer.PlayerName} has won: {r_FirstPlayer.PlayerWinsCounter}. ");
            Console.WriteLine($"{r_SecondPlayer.PlayerName} has won: {r_SecondPlayer.PlayerWinsCounter}. ");
        }
    }
}
