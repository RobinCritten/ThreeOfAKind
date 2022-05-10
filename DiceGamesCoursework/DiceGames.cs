namespace DiceGames
{

    //interface Die so any game using dice must define the following attributed and method
    internal interface Die
    {
        int sides { get; } //how many sides a dice has
        int DieNumber { get; } //how may die there are

        //method to roll dice and return a list of dice rolled
        List<int> Rolls(int Dice);
    }

    class Game : Die
    {
        private int WinCondition = 50; //how many points are needed to win the game
        private Random rnd = new(); //instansiate Random class

        //initialise how many sides the dice have
        public int sides
        {
            get { return 6; }
        }


        //initialise how many dice there are
        public int DieNumber
        {
            get { return 5; }
        }

        public List<int> Rolls(int Dice)
        {
            int result; //stores randomly generated number before its added to results
            List<int> results = new(); //list to store all rolled die

            //loop
            for (int i = 0; i < (Dice); i++)
            {
                result = rnd.Next(1, (sides + 1));
                results.Add(result); //
            }

            return results;
        }

        public int GetWinCondition()
        {
            int temp = WinCondition;
            return temp;
        }

        public void SetWinCondition(int n)
        {
            WinCondition = n;
        }

        //method for a single players turn, takes into account player type and returns the players score
        public int PlayerTurn(int PlayerType)
        {
            List<int> InitialRoll = new List<int>(); //stores the players first roll
            List<int> SecondRoll = new List<int>(); //stores the players second roll if there is one
            int maxValue = 0; //stores the frequency of the most appearing die side
            int maxKey = 0; //stores the side of the most appearing die
            int tempValue; //temporarily stores a value so it can be compated to maxValue
            bool choice = false; //boolean value used to loop until true
            string temp; //used to store user inputs
            int TurnScore; //stores the number of point a player scorred in their turn



            InitialRoll = Rolls(DieNumber); //Roll dice and store values in a list

            InitialRoll = InitialRoll.OrderBy(x => x).ToList(); //oreder list in accending order
                                                                //Change the list into a dictionary where the key is the dice value
                                                                //and the value is the freqency the side of the dice is rolled
            var frequency = InitialRoll.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            //Allows for more interaction from human player as well as get the input that the player can use to quit the game
            if (PlayerType == 1)
            {
                Console.WriteLine("Press Enter to roll dice, or type 'quit' to exit the game:");
                temp = Console.ReadLine();
                if (temp == "quit")
                {
                    Console.WriteLine();
                    return 1;
                }
                Console.WriteLine();
            }

            Console.WriteLine("Player Rolls...\n");
            Console.WriteLine("Dice Rolled: {0},{1},{2},{3},{4}\n", InitialRoll[0], InitialRoll[1],
                InitialRoll[2], InitialRoll[3], InitialRoll[4]); //Print Dice rolls to the console
            Console.WriteLine("Number\tFrequency");
            //loop through the dictionary to find the most frequent number rolled
            foreach (KeyValuePair<int, int> kvp in frequency)
            {
                tempValue = kvp.Value;
                if (tempValue >= maxValue)
                {
                    maxValue = tempValue;
                    maxKey = kvp.Key;
                }
                Console.WriteLine("{0}\t{1}", kvp.Key, kvp.Value);
            }

            //if the highest frequency of a number is 2 and the player is a human allow for second roll
            if (maxValue == 2 && PlayerType == 1) //Reroll for human players
            {
                //loop until a correct input is given
                while (choice == false)
                {
                    Console.WriteLine("\nWould you like to roll the dice again? (y/n)");
                    Console.WriteLine("Player will be keeping all die containing the number {0}", maxKey);
                    temp = Console.ReadLine();

                    //if the player chooses to roll a second time
                    if (temp == "y")
                    {
                        choice = true; //end while loop
                        SecondRoll = Rolls(DieNumber - maxValue); //roll again with remaining die

                        //add die not rerolled inot the secondRoll list
                        for (int i = 0; i < maxValue; i++)
                        {
                            SecondRoll.Add(maxKey);
                        }
                        SecondRoll = SecondRoll.OrderBy(x => x).ToList();

                        //Make a second dictionary for the second roll
                        var frequencySecond = SecondRoll.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                        Console.WriteLine("\nPlayer Rolls...\n");
                        Console.WriteLine("Dice Rolled: {0},{1},{2},{3},{4}\n", SecondRoll[0], SecondRoll[1],
                        SecondRoll[2], SecondRoll[3], SecondRoll[4]); //Print Dice rolls to the console remember remove
                        Console.WriteLine("Number\tFrequency");

                        //Find max value for the second dictionary roll
                        foreach (KeyValuePair<int, int> kvp in frequencySecond)
                        {
                            tempValue = kvp.Value;
                            if (tempValue >= maxValue)
                            {
                                maxValue = tempValue;
                                maxKey = kvp.Key;
                            }
                            Console.WriteLine("{0}\t{1}", kvp.Key, kvp.Value);
                        }

                        TurnScore = GetScore(maxValue); //Run getscore method to calculate score for turn
                        return TurnScore; //return score
                    }
                    //if no second roll is chosen the turn ends as there is nothing left to do
                    else if (temp == "n")
                    {
                        choice = true; //end while 
                    }
                }
            }

            //if the highest frequency of a number is 2 and the player is a computer allow for second roll
            //works the same as the human player version but does not need human input to decide on second roll
            else if (maxValue == 2 && PlayerType == 2) //Reroll for computer players
            {
                SecondRoll = Rolls(DieNumber - maxValue);
                for (int i = 0; i < maxValue; i++)
                {
                    SecondRoll.Add(maxKey);
                }
                SecondRoll = SecondRoll.OrderBy(x => x).ToList();
                var frequencySecond = SecondRoll.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                Console.WriteLine("\nPlayer will be keeping all die containing the number {0}", maxKey);
                Console.WriteLine("\nPlayer Rolls...\n");
                Console.WriteLine("Number\tFrequency");
                foreach (KeyValuePair<int, int> kvp in frequencySecond)
                {
                    tempValue = kvp.Value;
                    if (tempValue >= maxValue)
                    {
                        maxValue = tempValue;
                        maxKey = kvp.Key;
                    }
                    Console.WriteLine("{0}\t{1}", kvp.Key, kvp.Value);
                }

                //runs if human player doesnt do second roll, or conditions for a second roll are not met
                TurnScore = GetScore(maxValue);
                return TurnScore;
            }

            TurnScore = GetScore(maxValue);
            return TurnScore;
        }

        //method that takes an input of the most frequent die value and decides on how much score to return
        private static int GetScore(int frequency)
        {
            if (frequency < 3)
            {
                Console.WriteLine("\nPlayer scored 0 points");
                return 0;
            }
            else if (frequency == 3)
            {
                Console.WriteLine("\nPlayer scored 3 points");
                return 3;
            }
            else if (frequency == 4)
            {
                Console.WriteLine("\nPlayer scored 6 points");
                return 6;
            }
            else if (frequency == 5)
            {
                Console.WriteLine("\nPlayer scored 12 points");
                return 12;
            }

            //error message incase an error occurs and there is more than 5 die in play
            else
            {
                Console.WriteLine("\nSomething went wrong, so no points will be given this turn");
                return 0;
            }
        }
    }
}



