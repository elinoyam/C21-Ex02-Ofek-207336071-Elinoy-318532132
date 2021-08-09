using System;
using Engine;

namespace C21_Ex02_Ofek_207336071_Elinoy_318532132
{
    class Program
    {
        public static void Main()
        {
            string inputFromUser;
            bool goodInput = false, validInput = false, playAgainstComputer=false,playNewGame = true;
            int rowsNumber=0, columnsNumber=0; //TODO y I need to initialize this?
            Board mainBoard = null;
            Player firstPlayer, secondPlayer;



            //create get input
            while (!validInput)
            {
                Console.WriteLine("Hello, what is the size of the board you want to play with? ");
                while(!goodInput)
                {
                    Console.WriteLine("Enter the number of wanted rows:");
                    inputFromUser = Console.ReadLine();
                    goodInput = int.TryParse(inputFromUser, out rowsNumber);
                }

                goodInput = false;
                while(!goodInput)
                {
                    Console.WriteLine("Enter the number of wanted columns:");
                    inputFromUser = Console.ReadLine();
                    goodInput = int.TryParse(inputFromUser, out columnsNumber);
                }

                try
                {
                    mainBoard = new Board(rowsNumber, columnsNumber);
                    validInput = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            goodInput = false;

            while(!goodInput)
            {
                Console.WriteLine("Do you want to play against the computer? (Y - for yes / N - for no)");
                inputFromUser = Console.ReadLine();
                inputFromUser = inputFromUser.ToUpper();

                goodInput = ((inputFromUser == "Y") || (inputFromUser == "N"));
                if(!goodInput)
                {
                    Console.WriteLine("You must type Y or N only! (Y - for yes / N - for no)");
                }
                else
                {
                    playAgainstComputer = inputFromUser == "Y" ? true : false;
                }
            }

            firstPlayer = new Player(false); // TODO false or with a bool const?
            secondPlayer = new Player(playAgainstComputer);
           
            GameManger gm = new GameManger(mainBoard, firstPlayer, secondPlayer);


            while(playNewGame)
            {
                gm.GameLoop();
                goodInput = false;
                while(!goodInput)
                {
                    Console.WriteLine("Would you like to play another game? (Y - for yes / N - for no)");
                    inputFromUser = Console.ReadLine();
                    inputFromUser = inputFromUser.ToUpper();
                    goodInput = ((inputFromUser == "Y") || (inputFromUser == "N"));
                    if(goodInput)
                    {
                        playNewGame = !(inputFromUser == "N");
                    }
                }
            }
        }  // exit Main


        
    }
}
