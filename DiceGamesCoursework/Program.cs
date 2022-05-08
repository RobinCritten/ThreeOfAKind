namespace DiceGames
{
    class Program
    {
        static void Main()
        {

            MainMenu Menu = new MainMenu(); //Create main menu object
            Player player = new Player(); //Create player object

            int StartMenuInput; //records input from StartMenu

            //loops until a break statement is run, when the user selects an option to quit the game
            while (true)
            {
                //run the start menu method and assign the user input to StartMenuInput
                StartMenuInput = Menu.StartMenu();

                //play game has been selected, run the gameselect method
                if (StartMenuInput == 1)
                {
                    Dictionary<string, List<int>> players = player.GameSetup(); //initialise game values
                    player.Turns(players); //run game
                }

                //quit has been selected
                else if (StartMenuInput == 2)
                {
                    Console.WriteLine("\nThankyou for playing");
                    break; //break main while loop, end application runtime
                }
            }

        }
    }
}

