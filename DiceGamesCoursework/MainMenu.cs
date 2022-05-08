namespace DiceGames
{

	public class MainMenu
	{
		//method to display the start menu and allow the user to select an option in the menu
		public int StartMenu()
		{
			bool choice = false; //false until correct input entered
			int input = 0; //records user input
			string temp; //temporarily records user input while its in string form

			//print the start menu into the console
			Console.WriteLine(new String('-', 15));
			Console.WriteLine(" Three or More");
			Console.WriteLine(new String('-', 15));
			Console.WriteLine("\n1) Play Game");
			Console.WriteLine("2) Quit");

			//loop until correct input entered
			while (choice == false)
			{
				//enter input
				Console.WriteLine("\nPlease select an option by entering '1' or '2':");
				temp = Console.ReadLine();

				//error checking for integer type and integer range
				try
				{
					input = Convert.ToInt32(temp);

					if (input > 0 && input < 3)
					{
						choice = true;
					}

					else
					{
						Console.WriteLine("\nThat is not a valid input, please try again"); //error message
					}
				}
				catch (FormatException)

				{
					Console.WriteLine("\nThat is not a valid input, please try again"); //error message
				}
			}

			return input; //return correct user input
		}
	}
}