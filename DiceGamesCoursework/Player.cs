using System.Collections.Specialized;

namespace DiceGames
{


    //class encapsulated all attributes private and 2 of 4 methods private, only available through public interface
    public class Player
    {
        private int TurnCounter = 0; //counts number of turns in a game
        private Game game = new Game(); //creates game object

        //method for the creation of players, one parameter for how many human players in the game
        private static Dictionary<string, List<int>> CreatPlayers(int humans)
        {
            Dictionary<string, List<int>> PlayerDict = new Dictionary<string, List<int>>();
            List<int> human = new List<int>() { 0, 1 }; //list to store initial values for human players {score, player type}
            List<int> computer = new List<int>() { 0, 2 }; //list to store initial values for computer players                                                       

            //Add five players to the dictionary, and the specified number of human players
            for (int i = 1; i < 6; i++)
            {
                if (i < (humans + 1))
                {
                    PlayerDict.Add(String.Format("Player{0}", i.ToString()), human);
                }
                else
                {
                    PlayerDict.Add(String.Format("Computer{0}", i.ToString()), computer);
                }
            }
            return PlayerDict;
        }

        //Method to print the current scoreboard to the console
        private static void Scoreboard(Dictionary<string, List<int>> PlayerDict)
        {
            string playertype; //changes depending if the player is human or computer controlled
            Console.WriteLine("Player\t\tScore\t\tPlayer Type\n");
            foreach (KeyValuePair<string, List<int>> kvp in PlayerDict)
            {
                if (kvp.Value[1] == 1)
                {
                    playertype = "Human";
                }
                else
                {
                    playertype = "Computer";
                }
                //print player name, score and type in that order
                Console.WriteLine("{0}:\t  {1}\t\t   {2}", kvp.Key, kvp.Value[0], playertype);
            }
            Console.WriteLine(); //empty space for improved readability
        }

        //method to loop the Turn method until the win condition is met, also congradtulates the winner
        public void Turns(Dictionary<string,List<int>> PlayerDict)
        {
            int points; //holds the current players score for their turn
            bool play = true; //boolean value used to loop while loop until the game has been won
            TurnTotals turnScore = new();
            TurnTotals gameScore = new();
            TurnTotals PlayerTurnScore = new();
            turnScore.SetTotal(0); //reset turnScore counter at the start of every game
            gameScore.SetTotal(0); //reset gameScore at the start of every game
            PlayerTurnScore.SetTotal(0); //reset at the start of game
            TurnCounter = 0; //reset TurnCounter for each game

            while (play == true)
            {
                TurnCounter++; //itterate for each turn
                //Console.WriteLine("Testing turnScore {0}", turnScore.total);
                turnScore.SetTotal(0); //reset turnScore at the start of every turn
                //Console.WriteLine("Testing turnScore again {0}", turnScore.total);

                //Loop through each player for their turns
                foreach (KeyValuePair<string,List<int>> kvp in PlayerDict)
                {
                    //Console.WriteLine("Testing PlayerScore {0}", PlayerTurnScore.total);
                    PlayerTurnScore.SetTotal(0); //reset at the start of every turn
                    //Console.WriteLine("Testing PlayerScore {0}", PlayerTurnScore.total);
                    Console.WriteLine(new string('-', 30) + "\n");
                    Console.WriteLine("{0}'s Turn\n", kvp.Key);

                    //run a single players turn and return the points they scorred
                    points = game.PlayerTurn(kvp.Value[1]);

                    //runs when the player decides to quit the game
                    if (points == 1)
                    {
                        play = false;
                        break;
                    }

                    PlayerTurnScore.SetTotal(points);
                    turnScore += PlayerTurnScore;
                    points += kvp.Value[0];

                    //update dictionary to represent new points a player has
                    PlayerDict[kvp.Key] = new List<int>() { points, kvp.Value[1] };

                    //if the win condition has been met, print message to console and break while loop
                    if (points >= game.GetWinCondition())
                    {
                        Console.WriteLine("\nCongratulations to {0} for winning this game in {1} turns\n", kvp.Key, TurnCounter);
                        play = false;
                        break;
                    }
                    Console.WriteLine("Press 'Enter' to begin next players turn");
                    Console.ReadLine();
                    Console.WriteLine("\n" + new String('-', 30) + "\n");
                }
                //Print final scoreboard once the game has ended and at the end of every set of turns
                Scoreboard(PlayerDict);
                Console.WriteLine("\nPlayers scored {0} points this turn\n",turnScore.total);
                gameScore += turnScore;

            }
            Console.WriteLine("Players scored {0} points this game in total\n", gameScore.total);
        }

        //Method that runs upon the player deciding to begin a game, allows the player to decide upon number of human players
        //also allows for the player to decide on what length of game to play
        public Dictionary<string,List<int>> GameSetup()
        {
            string choice; //stores players choice in string form
            int IntChoice; //stores players choice in integer form
            Dictionary<string,List<int>> PlayerDict = new Dictionary<string,List<int>>();

            //Prints rules to the console
            Console.WriteLine("\nGame Rules:");
            Console.WriteLine("-Player rolls five dice and tries to get three of a kind or more");
            Console.WriteLine("-If three of a kind or more are rolled the player will score 3, 6 or 12 points respectively");
            Console.WriteLine("-If two of a kind is rolled, the player can throw the remaining die if they wish to try and get three of a kind");
            Console.WriteLine("-If three or more of a kind has not been rolled after the second roll no points are scorred");
            Console.WriteLine("-The first player to reach the winning number of points wins the game");
            Console.WriteLine("-Players can exit the game by typing 'quit' at the start of a human players turn");

            //Asks user for input and explains on what input does
            Console.WriteLine("\nHow many people playing the game are there?:");
            Console.WriteLine("1) 1 human player");
            Console.WriteLine("2) 2 human players");
            Console.WriteLine("3) 3 human players");
            Console.WriteLine("4) 4 human players");
            Console.WriteLine("5) 5 human players");

            //Loop until a correct input is given
            while (true)
            {
                choice = Console.ReadLine();

                //catches any non integer type inputs from the user as to not crash the program
                try
                {
                    IntChoice = Convert.ToInt32(choice);
                    if (IntChoice > 0 && IntChoice < 6)
                    {
                        PlayerDict = CreatPlayers(IntChoice);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input, please try again"); //Error message for user
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("That is not a valid input, please try again"); //Error message for the user
                }

            }

            //Asks user for input and explains what input does
            Console.WriteLine("\nPlease enter what game duration you would like to play:");
            Console.WriteLine("1) Quick game for 20 points");
            Console.WriteLine("2) Medium length game for 50 points");
            Console.WriteLine("3) Long game for 80 points");

            //Loop until correct input is given
            while (true)
            {
                choice = Console.ReadLine();

                //catches any non integer inputs the user may give
                try
                {
                    IntChoice = Convert.ToInt32(choice);

                    //sets the score threshold for the win condition of the game
                    if (IntChoice == 1)
                    {
                        game.SetWinCondition(20);
                        break;
                    }
                    else if (IntChoice == 2)
                    {
                        game.SetWinCondition(50);
                        break;
                    }
                    else if (IntChoice == 3)
                    {
                        game.SetWinCondition(80);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input, please try again"); //Error message for user
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("That is not a valid input, please try again"); //Error message for user
                }
            }

            return PlayerDict;
        }
    }

    //class for calculating player turn totals, and game score totals
    public class TurnTotals
    {
        public int total; //holds a value that is a total score of some form
        
        //dircetly change the value of total
        public void SetTotal(int n)
        {
            total = n;
        }

        //Add two totals objects together to create a different total
        public static TurnTotals operator +(TurnTotals A, TurnTotals B)
        {
            TurnTotals tempTotal = new TurnTotals();
            tempTotal.total = A.total + B.total;
            return tempTotal;
        }
    }
}



