using System;
using Engine;

namespace C21_Ex02_Ofek_207336071_Elinoy_318532132
{
    class Program
    {
        public static void Main()
        {
            RunProgram(); 
        } 

        public static void RunProgram()
        {
            string inputFromUser;
            bool goodInput = false, playNewGame = true;
            Board mainBoard = null;
            Player firstPlayer, secondPlayer;

            GetBoardSizeFromUser(out mainBoard);
            GetPlayersFromUser(out firstPlayer, out secondPlayer);
            GameManger gameManger = new GameManger(mainBoard, firstPlayer, secondPlayer);
            while (playNewGame)
            {
                gameManger.GameLoop();
                goodInput = false;
                while (!goodInput)
                {
                    Console.WriteLine("Would you like to play another game? (Y - for yes / N - for no)");
                    inputFromUser = Console.ReadLine();
                    inputFromUser = inputFromUser.ToUpper();
                    goodInput = ((inputFromUser == "Y") || (inputFromUser == "N"));
                    if (goodInput)
                    {
                        playNewGame = !(inputFromUser == "N");
                    }
                }
            }
        }   // exit RunProgram()

        public static void GetBoardSizeFromUser(out Board o_MainBoard)
        {
            bool validInput = false, goodInput = false;
            string inputFromUser;
            int rowsNumber = 0, columnsNumber = 0; 
            o_MainBoard = null;

            while (!validInput)
            {
                Console.WriteLine("Hello, what is the size of the board you want to play with? ");
                while (!goodInput)
                {
                    Console.WriteLine("Enter the number of wanted rows:");
                    inputFromUser = Console.ReadLine();
                    goodInput = int.TryParse(inputFromUser, out rowsNumber);
                }

                goodInput = false;
                while (!goodInput)
                {
                    Console.WriteLine("Enter the number of wanted columns:");
                    inputFromUser = Console.ReadLine();
                    goodInput = int.TryParse(inputFromUser, out columnsNumber);
                }

                try
                {
                    o_MainBoard = new Board(rowsNumber, columnsNumber);
                    validInput = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }  // exit GetBoardSizeFromUser()

        public static void GetPlayersFromUser(out Player o_FirstPlayer, out Player o_SecondPlayer)
        {
            const bool v_IsComputer = true;
            bool goodInput = false, playAgainstComputer = false;
            string inputFromUser;

            while (!goodInput)
            {
                Console.WriteLine("Do you want to play against the computer? (Y - for yes / N - for no)");
                inputFromUser = Console.ReadLine();
                inputFromUser = inputFromUser.ToUpper();

                goodInput = ((inputFromUser == "Y") || (inputFromUser == "N"));
                if (!goodInput)
                {
                    Console.WriteLine("You must type Y or N only! (Y - for yes / N - for no)");
                }
                else
                {
                    playAgainstComputer = inputFromUser == "Y" ? true : false;
                }
            }

            o_FirstPlayer = new Player(!v_IsComputer); 
            o_SecondPlayer = new Player(playAgainstComputer);
        }
    }
}
