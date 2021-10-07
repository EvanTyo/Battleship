using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Ships;
using System.Text.RegularExpressions;
using BattleShip.UI;

namespace BattleShip.UI
{
    class UserInput
    {
        // Ask a user for their name
        public string AskForName(string prompt)
        {
            // Initialize fields
            string name;

            // Read input
            name = ReadInputString(prompt);

            // Return name
            return name;
        }

        // Ask for a ship type
        public ShipType AskForShipType(string prompt)
        {
            // Initialize fields
            string shipIntInput;
            int shipIndex;
            int MinIndexRange = 1;
            int MaxIndexRange = 5;

            // Create ShipType instance
            ShipType shipType = new ShipType();

            // Create loop
            while (true)
            {
                // Read input
                shipIntInput = ReadInputString(prompt);

                // Check if ship type choice is an int
                if (int.TryParse(shipIntInput, out shipIndex))
                {

                    // Check if ship type choice int is within range
                    if (withinRange(shipIndex, MinIndexRange, MaxIndexRange))
                    {
                        shipType = IntToShipType(shipIndex);
                        break;
                    }
                }

                // Display invalid entry message
                UserOutput.InvalidEntry($"Invalid ship choice input. Please try again.");
            }

            // Return ship type
            return shipType;
        }

        // Ask for a ship placement direction
        public ShipDirection AskForShipDirection(string prompt)
        {
            // Initilize fields
            string directionIntInput;
            int directionIndex;
            int MinIndexRange = 1;
            int MaxIndexRange = 4;

            // Create ShipDirection instance
            ShipDirection shipDirection = new ShipDirection();

            // Create loop
            while (true)
            {
                // Read input
                directionIntInput = ReadInputString(prompt);

                // Check if direction choice is an int
                if (int.TryParse(directionIntInput, out directionIndex))
                {

                    // Check if direction choice is within range
                    if (withinRange(directionIndex, MinIndexRange, MaxIndexRange))
                    {
                        shipDirection = IntToShipDirection(directionIndex);
                        break;
                    }
                }

                // Display invalid entry message
                UserOutput.InvalidEntry($"Invalid directional choice input. Please try again.");
            }

            // Return ship direction
            return shipDirection;
        }

        // Ask user for coordinate input, then returns a coordinate
        public Coordinate AskForCoordinates(string prompt)
        {
            // Initialize fields
            string input;
            int intOut;
            string xCoordinateString = null;
            string yCoordinateString = null;
            int xCoordinate;
            int yCoordinate;
            int coordinateLengthTwo = 2;
            int coordinateLengthThree = 3;
            int MinCoordinateRange = 1;
            int MaxCoordinateRange = 10;

            // Loop to perform checks on input and convert to x and y coordinates
            while (true)
            {
                // Read input
                input = ReadInputString(prompt);

                // Check if input is null or empty
                if ((input != null) && (input != ""))
                {
                    // Check if input is lenght of 2
                    if (input.Length == coordinateLengthTwo)
                    {
                        // Seperate the string into string coordinates
                        yCoordinateString = input[0].ToString();
                        xCoordinateString = input[1].ToString();
                    }

                    // Check if input length is 3
                    else if (input.Length == coordinateLengthThree)
                    {
                        // Seperate the string into string coordinates
                        yCoordinateString = input[0].ToString();
                        xCoordinateString = input[1].ToString() + input[2].ToString();
                    }

                    // Check if xCoordinate is a letter within range
                    if (characterWithinRange(yCoordinateString))
                    {

                        // Check if yCoordinate is an int
                        if (int.TryParse(xCoordinateString, out intOut))
                        {

                            // Check if yCoordinate is within range
                            if (withinRange(intOut, MinCoordinateRange, MaxCoordinateRange))
                            {

                                // Set x and y coordinates
                                yCoordinate = YCoordinateStringToInt(yCoordinateString);
                                xCoordinate = intOut;
                                break;
                            }
                        }
                    }
                }

                // Display invalid entry message
                UserOutput.InvalidEntry($"Invalid coordinate input. Please try again.");
            }

            // Create a coordiante with the x and y coordinates
            Coordinate coordinate = new Coordinate(xCoordinate, yCoordinate);

            // Return coordinate
            return coordinate;
        }

        // Ask user to play again
        public int AskToPlayAgain(string prompt, int min, int max)
        {
            //Initialize fields
            string choiceString;
            int choiceInt;

            // Create loop
            while (true)
            {
                // Read input
                choiceString = ReadInputString(prompt);

                // Check if input is an integer
                if (int.TryParse(choiceString, out choiceInt))
                {

                    // Check if input is within range
                    if (withinRange(choiceInt, min, max))
                    {
                        return choiceInt;
                    }
                }
                // Display invalid entry message
                UserOutput.InvalidEntry($"Invalid choice input. Please try again.");
            }
        }

        // Check if int is within given range
        private bool withinRange(int input, int min, int max)
        {
            // Initialize fields
            bool isWithinRange = false;

            // Check if int is within range
            if (input >= min && input <= max)
            {
                isWithinRange = true;
            }

            // Return bool
            return isWithinRange;
        }

        // Handle ship index switch to ship type
        private ShipType IntToShipType(int shipIndex)
        {
            // Initialize fields
            ShipType shipType = new ShipType();

            // Switch case to pick ship direction
            switch (shipIndex)
            {
                case 1:
                    shipType = ShipType.Destroyer;
                    break;
                case 2:
                    shipType = ShipType.Submarine;
                    break;
                case 3:
                    shipType = ShipType.Cruiser;
                    break;
                case 4:
                    shipType = ShipType.Battleship;
                    break;
                case 5:
                    shipType = ShipType.Carrier;
                    break;
                default:
                    UserOutput.DisplayError("shipIndex to shipType.");
                    break;
            }

            // Return ship direction
            return shipType;
        }

        // Handle direction index switch to ship direction
        private ShipDirection IntToShipDirection(int directionIndex)
        {
            // Initialize fields
            ShipDirection shipDirection = new ShipDirection();

            // Switch case to pick ship direction
            switch (directionIndex)
            {
                case 1:
                    shipDirection = ShipDirection.Up;
                    break;
                case 2:
                    shipDirection = ShipDirection.Down;
                    break;
                case 3:
                    shipDirection = ShipDirection.Left;
                    break;
                case 4:
                    shipDirection = ShipDirection.Right;
                    break;
                default:
                    UserOutput.DisplayError("directionIndex to shipDirection.");
                    break;
            }

            // Return ship direction
            return shipDirection;
        }

        // Handle xCoordinateString switch to xCoordinate Int
        private int YCoordinateStringToInt(string yCoordinateString)
        {
            // Initialize fields
            int yCoordinate = 0;

            // Switch case to pick xCoordinate
            switch (yCoordinateString)
            {
                case "A":
                    yCoordinate = 1;
                    break;
                case "B":
                    yCoordinate = 2;
                    break;
                case "C":
                    yCoordinate = 3;
                    break;
                case "D":
                    yCoordinate = 4;
                    break;
                case "E":
                    yCoordinate = 5;
                    break;
                case "F":
                    yCoordinate = 6;
                    break;
                case "G":
                    yCoordinate = 7;
                    break;
                case "H":
                    yCoordinate = 8;
                    break;
                case "I":
                    yCoordinate = 9;
                    break;
                case "J":
                    yCoordinate = 10;
                    break;
                default:
                    UserOutput.DisplayError("y coordinate string to int.");
                    break;
            }

            // Return xCoordinate
            return yCoordinate;
        }

        // Check if character is within range
        private bool characterWithinRange(string yCoordinateString)
        {
            // Initialize fields
            bool isWithinRange = false;
            char yCoordinate;
            string prompt;

            // Convert string to character
            if (char.TryParse(yCoordinateString, out yCoordinate))
            {

                // Check if character is within range
                if ((yCoordinate >= 'A' && yCoordinate <= 'J'))
                {
                    isWithinRange = true;
                }
            }

            // Return bool
            return isWithinRange;
        }

        // Read input and display prompt
        public string ReadInputString(string prompt)
        {
            // Initialize fields
            string output;

            // Display prompt and read input
            UserOutput.colorForegroundGreen(prompt);
            output = Console.ReadLine();

            // Return output string
            return output;
        }
    }
}
