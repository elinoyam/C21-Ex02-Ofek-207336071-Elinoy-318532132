using System;
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

        public int getPlayerChoice(out int o_ColumnChoice, out bool o_ToQuit)
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
                    // 1 <= columnChoice <= Columns 
                    if(1 <= columnChoice && m_Board.Columns >= columnChoice)
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
            bool isTie = m_Board.CheckIfBoardIsFull()  ? true  :  false;
            return isTie;
        }

        public bool CheckForWin(int i_Row, int i_Column, char i_Sign)
        {
            return m_Board.CheckWin(i_Row, i_Column, i_Sign);
        }

        public void GameLoop()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            m_Board.InitBoard();
            bool playing = true;
            bool tie = false;
            bool isQuit = false;
            int playerColumnChoice;
            int availableRow;
            char activeSign = 'X';
                
            //loop while the board isn't full
            while (playing && !tie)
            {
                //print board
                m_Board.PrintBoard(); 

                if ((m_SecondPlayer.IsComputer) && (GetActivePlayerSign() == 'O')) //TODO active player?
                {
                    m_SecondPlayer.GetComputerChoice(ref m_Board ,out availableRow,out playerColumnChoice);
                    activeSign = 'O';
                }
                else
                {

                    //get choice check if valid 
                    availableRow = getPlayerChoice(out playerColumnChoice, out isQuit);
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


            // TODO tell who won and show the count 1-2
            if (tie && playing)
            {
                Console.WriteLine("It's a tie!!! Better luck next time....");
                Console.WriteLine($"{m_FirstPlayer.PlayerName} has won: {m_FirstPlayer.PlayerWinsCounter}. ");
                Console.WriteLine($"{m_SecondPlayer.PlayerName} has won: {m_SecondPlayer.PlayerWinsCounter}. ");
            }
            else
            {
                PrintWins();
            }

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
