using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Ships;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;

namespace BattleShip.UI
{
    class UserOutput
    {
        // Display title message
        public static void DisplayTitle()
        {
            Console.WriteLine("\n\nWelcome to the game of Battle Ship!!");
            PressKeyToContinue("Press any key to start the application.");
        }

        // Display start menu / splash screen
        public static void SplashScreen()
        {
            Console.Clear();
            Console.WriteLine($"          _____                    _____                _____                _____                    _____            _____                    _____                    _____                    _____                    _____          " +
                $"\n" + @"         /\    \                  /\    \              /\    \              /\    \                  /\    \          /\    \                  /\    \                  /\    \                  /\    \                  /\    \         " +
                $"\n" + @"        /::\    \                /::\    \            /::\    \            /::\    \                /::\____\        /::\    \                /::\    \                /::\____\                /::\    \                /::\    \        " +
                $"\n" + @"       /::::\    \              /::::\    \           \:::\    \           \:::\    \              /:::/    /       /::::\    \              /::::\    \              /:::/    /                \:::\    \              /::::\    \       " +
                $"\n" + @"      /::::::\    \            /::::::\    \           \:::\    \           \:::\    \            /:::/    /       /::::::\    \            /::::::\    \            /:::/    /                  \:::\    \            /::::::\    \      " +
                $"\n" + @"     /:::/\:::\    \          /:::/\:::\    \           \:::\    \           \:::\    \          /:::/    /       /:::/\:::\    \          /:::/\:::\    \          /:::/    /                    \:::\    \          /:::/\:::\    \     " +
                $"\n" + @"    /:::/__\:::\    \        /:::/__\:::\    \           \:::\    \           \:::\    \        /:::/    /       /:::/__\:::\    \        /:::/__\:::\    \        /:::/____/                      \:::\    \        /:::/__\:::\    \    " +
                $"\n" + @"   /::::\   \:::\    \      /::::\   \:::\    \          /::::\    \          /::::\    \      /:::/    /       /::::\   \:::\    \       \:::\   \:::\    \      /::::\    \                      /::::\    \      /::::\   \:::\    \   " +
                $"\n" + @"  /::::::\   \:::\    \    /::::::\   \:::\    \        /::::::\    \        /::::::\    \    /:::/    /       /::::::\   \:::\    \    ___\:::\   \:::\    \    /::::::\    \   _____    ____    /::::::\    \    /::::::\   \:::\    \  " +
                $"\n" + @" /:::/\:::\   \:::\ ___\  /:::/\:::\   \:::\    \      /:::/\:::\    \      /:::/\:::\    \  /:::/    /       /:::/\:::\   \:::\    \  /\   \:::\   \:::\    \  /:::/\:::\    \ /\    \  /\   \  /:::/\:::\    \  /:::/\:::\   \:::\____\ " +
                $"\n" + @"/:::/__\:::\   \:::|    |/:::/  \:::\   \:::\____\    /:::/  \:::\____\    /:::/  \:::\____\/:::/____/       /:::/__\:::\   \:::\____\/::\   \:::\   \:::\____\/:::/  \:::\    /::\____\/::\   \/:::/  \:::\____\/:::/  \:::\   \:::|    |" +
                $"\n" + @"\:::\   \:::\  /:::|____|\::/    \:::\  /:::/    /   /:::/    \::/    /   /:::/    \::/    /\:::\    \       \:::\   \:::\   \::/    /\:::\   \:::\   \::/    /\::/    \:::\  /:::/    /\:::\  /:::/    \::/    /\::/    \:::\  /:::|____|" +
                $"\n" + @" \:::\   \:::\/:::/    /  \/____/ \:::\/:::/    /   /:::/    / \/____/   /:::/    / \/____/  \:::\    \       \:::\   \:::\   \/____/  \:::\   \:::\   \/____/  \/____/ \:::\/:::/    /  \:::\/:::/    / \/____/  \/_____/\:::\/:::/    / " +
                $"\n" + @"  \:::\   \::::::/    /            \::::::/    /   /:::/    /           /:::/    /            \:::\    \       \:::\   \:::\    \       \:::\   \:::\    \               \::::::/    /    \::::::/    /                    \::::::/    /  " +
                $"\n" + @"   \:::\   \::::/    /              \::::/    /   /:::/    /           /:::/    /              \:::\    \       \:::\   \:::\____\       \:::\   \:::\____\               \::::/    /      \::::/____/                      \::::/    /   " +
                $"\n" + @"    \:::\  /:::/    /               /:::/    /    \::/    /            \::/    /                \:::\    \       \:::\   \::/    /        \:::\  /:::/    /               /:::/    /        \:::\    \                       \::/____/    " +
                $"\n" + @"     \:::\/:::/    /               /:::/    /      \/____/              \/____/                  \:::\    \       \:::\   \/____/          \:::\/:::/    /               /:::/    /          \:::\    \                       ~~          " +
                $"\n" + @"      \::::::/    /               /:::/    /                                                      \:::\    \       \:::\    \               \::::::/    /               /:::/    /            \:::\    \                                  " +
                $"\n" + @"       \::::/    /               /:::/    /                                                        \:::\____\       \:::\____\               \::::/    /               /:::/    /              \:::\____\                                 " +
                $"\n" + @"        \::/____/                \::/    /                                                          \::/    /        \::/    /                \::/    /                \::/    /                \::/    /                                 " +
                $"\n" + @"         ~~                       \/____/                                                            \/____/          \/____/                  \/____/                  \/____/                  \/____/                                  " + "\n");
        }

        // Display basic prompt
        public static void DisplayPrompt(string prompt)
        {
            // Display prompt
            Console.Write(prompt);
        }

        // Display player's turn
        public static void DisplayPlayersTurn(Player player)
        {
            // Initialize fields
            string name;

            // Set fields
            name = player.Name;

            // Display player's turn
            colorForegroundCyan($"\n{name}, it is your turn. ");
        }

        // Display pick your placement
        public static void DisplayPickPlacement()
        {
            // Display player's turn
            colorForegroundCyan($"Pick your placement.");
        }

        // Display pick your shot coordinate
        public static void DisplayPickShotCoordinate()
        {
            // Display player's turn
            colorForegroundCyan($"Pick your shot coordinate.");
        }

        // Display response to the shot fired
        public static void DisplayShotFiredResponse(FireShotResponse response)
        {
            // Instantiate class
            ShotStatus shotStatus = new ShotStatus();

            // Initialize fields
            String prompt = null;
            shotStatus = response.ShotStatus;

            // Switch case for response
            switch (shotStatus)
            {
                case ShotStatus.Invalid:
                    prompt = $"\n{ShotStatus.Invalid} entry, please try again.";
                    break;
                case ShotStatus.Duplicate:
                    prompt = $"\n{ShotStatus.Duplicate} entry, please try again.";
                    break;
                case ShotStatus.Miss:
                    prompt = $"\nIt was a {ShotStatus.Miss}.";
                    break;
                case ShotStatus.Hit:
                    prompt = $"\nIt was a {ShotStatus.Hit}!";
                    break;
                case ShotStatus.HitAndSunk:
                    prompt = $"\nYou've sunken their {response.ShipImpacted}!";
                    break;
                case ShotStatus.Victory:
                    prompt = $"\nYou've sunken all of their ships, you are victorious!";
                    break;
                default:
                    DisplayError("display shot fired response.");
                    break;
            }

            // Display output based on response
            Console.WriteLine(prompt);
        }

        // Display each player's board
        public static void DisplayBoard(Board board)
        {
            // Initialize fields
            int MaxRowAndColumnAmount = 10;

            // Create array of characters
            string[] rowCharacters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            // Display top Column numbers
            Console.Write("\n\n* 1 2 3 4 5 6 7 8 9 10");

            // Display Columns
            for (int i = 0; i < MaxRowAndColumnAmount; i++)
            {

                // Display each Row character
                Console.Write($"\n{rowCharacters[i]}");

                // Display Rows
                for (int j = 0; j < MaxRowAndColumnAmount; j++)
                {

                    // Display shot history board
                    DisplayShotHistory(board, i, j);
                }
            }

            // Create line space
            Console.WriteLine("");
        }

        // Display populated coordinates
        public static void DisplayPopulatedCoordinates(Player player, int playerCount)
        {
            // Instantiate classes
            Coordinate coordinate;

            // Initialize fields
            string yCoordinate;
            string xCoordinate;
            
            // Display header
            colorForegroundYellow("\n\nShips placed and their coordinates: ");

            // Display ships and coordinates
            for (int i = 0; i < playerCount; i++)
            {
                // Display ships
                Console.Write($"\n{player.PlayerBoard.Ships[i].ShipType.ToString()}: \t");

                // Display coordinates
                for (int j = 0; j < player.PlayerBoard.Ships[i].BoardPositions.Length; j++)
                {
                    // Set coordinate
                    coordinate = player.PlayerBoard.Ships[i].BoardPositions[j];

                    // Convert coordinates to string values
                    yCoordinate = YCoordinateIntToString(coordinate.YCoordinate);
                    xCoordinate = coordinate.XCoordinate.ToString();

                    // Display coordinates
                    Console.Write($"{yCoordinate}{xCoordinate} ");
                }
            }

            // Create blank line following display
            Console.WriteLine("");
        }

        // Display player's shot history
        public static void DisplayShotHistory(Board board, int i, int j)
        {
            // Instantiate classes
            Coordinate coordinate;
            ShotHistory shotHistory;

            // Initialize fields
            string prompt;

            // Create a space between each character output
            Console.Write(" ");

            // Display character based on coordinate shot history
            coordinate = new Coordinate(j + 1, i + 1);
            shotHistory = board.CheckCoordinate(coordinate);

            // If hit, display a red background H
            if (shotHistory == ShotHistory.Hit)
            {
                prompt = "H";
                colorBackgroundRed(prompt);
            }

            // If miss, display a yellow background M
            else if (shotHistory == ShotHistory.Miss)
            {
                prompt = "M";
                colorBackgroundYellow(prompt);
            }

            // If unknown, display a blue background -
            else if (shotHistory == ShotHistory.Unknown)
            {
                prompt = "-";
                colorBackgroundBlue(prompt);
            }
        }

        // Display closing message
        public static void DisplayClosing()
        {
            Console.WriteLine("\nThank you for playing Battle Ship!!" +
                "\nI hope you had a ~splashing~ time!");
            PressKeyToContinue("\nPress any key to close the application.");
        }

        // Display invalid entry
        public static void InvalidEntry(string prompt)
        {
            colorForegroundRed($"\n{prompt}\n");
        }

        // Display prompt to continue
        public static void PressKeyToContinue(string prompt = "Press any key to continue...")
        {
            Console.WriteLine($"{prompt}");
            Console.ReadKey();
        }

        // Display error message
        public static void DisplayError(string prompt)
        {
            Console.WriteLine($"\nError handling {prompt}");
        }

        // Clear console
        public static void ClearConsole()
        {
            Console.Clear();
        }

        // Convert YCoordinate from int to string
        private static string YCoordinateIntToString(int yCoordinate)
        {
            // Initialize fields
            string yCoordinateString = null;

            // Switch case to pick xCoordinate
            switch (yCoordinate)
            {
                case 1:
                    yCoordinateString = "A";
                    break;
                case 2:
                    yCoordinateString = "B";
                    break;
                case 3:
                    yCoordinateString = "C";
                    break;
                case 4:
                    yCoordinateString = "D";
                    break;
                case 5:
                    yCoordinateString = "E";
                    break;
                case 6:
                    yCoordinateString = "F";
                    break;
                case 7:
                    yCoordinateString = "G";
                    break;
                case 8:
                    yCoordinateString = "H";
                    break;
                case 9:
                    yCoordinateString = "I";
                    break;
                case 10:
                    yCoordinateString = "J";
                    break;
                default:
                    UserOutput.DisplayError("y coordinate string to int.");
                    break;
            }

            // Return xCoordinate
            return yCoordinateString;
        }

        //Change Console color to red
        public static void colorBackgroundRed(String prompt)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console color to yellow
        public static void colorBackgroundYellow(String prompt)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console color to blue
        public static void colorBackgroundBlue(String prompt)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console color to green
        public static void colorForegroundGreen(String prompt)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console foreground color to cyan
        public static void colorForegroundCyan(String prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console foreground color to yellow
        public static void colorForegroundYellow(String prompt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(prompt);
            Console.ResetColor();
        }

        //Change Console foreground color to red
        public static void colorForegroundRed(String prompt)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(prompt);
            Console.ResetColor();
        }
    }
}
