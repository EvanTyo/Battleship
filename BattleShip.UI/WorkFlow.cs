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
    class WorkFlow
    {
        // Method to start the battleship battle
        public void BattleStart()
        {
            // Instantiate classes
            UserInput userInput = new UserInput();

            // Initialize fields
            string prompt;
            ShipType[] usedTypes = new ShipType[5];

            // Display the spalsh screen
            UserOutput.SplashScreen();

            // Display title message
            UserOutput.DisplayTitle();

            // Create 2 players
            Player playerOne = new Player();
            Player playerTwo = new Player();

            // Create 2 player boards
            Board boardOne = new Board();
            Board boardTwo = new Board();

            // Ask for first player's name
            prompt = ($"\nPlease enter the first player's name.");
            UserOutput.DisplayPrompt(prompt);
            prompt = ($"\nPlayer One's Name: ");
            playerOne.Name = userInput.AskForName(prompt);

            // Ask for second player's name
            prompt = ($"\nPlease enter the second player's name.");
            UserOutput.DisplayPrompt(prompt);
            prompt = ($"\nPlayer Two's Name: ");
            playerTwo.Name = userInput.AskForName(prompt);

            // Randomly determine which player goes first.... or don't

            // Set each player with their board
            playerOne.PlayerBoard = boardOne;
            playerTwo.PlayerBoard = boardTwo;

            // Set up boards based on player ship placements
            PlayerShipPlacements(userInput, playerOne, playerTwo);

            // Each player takes their turns
            TakePlayerTurn(userInput, playerOne, playerTwo);

            // Ask players if they'd like to play again
            PlayAgain(userInput);

            // Display closing message
            UserOutput.DisplayClosing();
        }

        // Method to handle choosing ship type for placement
        public PlaceShipRequest RequestShipType(UserInput userInput, Player player, PlaceShipRequest placeShipRequest)
        {
            // Initialize fields
            string prompt;
            int genericShipSize = 2;
            bool shipNotUsed = false;

            // Instantiate classes
            Ship newShip;

            // Check if ship was already used
            while (!shipNotUsed)
            {
                // Request player for Ship type to place
                prompt = ($"\nChoose which ship you would like to place: " +
                    $"\n1) {ShipType.Destroyer}\t 2 slots" +
                    $"\n2) {ShipType.Submarine}\t 3 slots" +
                    $"\n3) {ShipType.Cruiser}\t 3 slots" +
                    $"\n4) {ShipType.Battleship}\t 4 slots" +
                    $"\n5) {ShipType.Carrier}\t 5 slots");
                UserOutput.DisplayPrompt(prompt);
                prompt = ($"\nSelection: ");
                placeShipRequest.ShipType = userInput.AskForShipType(prompt);

                // Create new ship to check if shiptype already exits
                newShip = new Ship(placeShipRequest.ShipType, genericShipSize);

                // Ask for ship again if shiptype already exits
                if (player.PlayerBoard.Ships.Any(s => s != null && s.ShipType == newShip.ShipType))
                {
                    UserOutput.InvalidEntry($"{newShip.ShipType} has already been placed. Please try again.");
                }

                // If shiptype does not exist, continue
                else
                {
                    shipNotUsed = true;
                }
            }

            // Return shiptype
            return placeShipRequest;
        }

        // Method to handle choosing the coordinate for placement
        public PlaceShipRequest RequestCoordinate(UserInput userInput, Player player, PlaceShipRequest placeShipRequest)
        {
            // Initialize fields
            string prompt;
            bool coordinateNotUsed = false;

            // Check if coordinate was already populated
            while (!coordinateNotUsed)
            {
                // Request player for Ship placement coordinate
                prompt = ($"\nEnter the coordinates for your ship placement: " +
                    $"\nCoordinates range from A1 - J10.");
                UserOutput.DisplayPrompt(prompt);
                prompt = ($"\nSelection: ");
                placeShipRequest.Coordinate = userInput.AskForCoordinates(prompt);

                // Ask for coordinate again if coordinate is already populated
                if (player.PlayerBoard.Ships.Any(c => c != null && c.BoardPositions.Contains(placeShipRequest.Coordinate)))
                {
                    UserOutput.InvalidEntry($"That coordinate is already populated. Please try again.");
                }

                // If coordinate not populated, continue
                else
                {
                    coordinateNotUsed = true;
                }
            }

            // Return coordiante
            return placeShipRequest;
        }

        // Method to handle choosing the direction for placement
        public PlaceShipRequest RequestDirection(UserInput userInput, Player player, PlaceShipRequest placeShipRequest)
        {
            //Instantiate classes
            ShipPlacement shipPlacement = new ShipPlacement();

            // Initialize fields
            string prompt;
            bool directionOk = false;

            // Loop for ship placement
            while (!directionOk)
            {

                // Request player for Ship direction
                prompt = ($"\nEnter the direction for your ship placement: " +
                    $"\n1) Up" +
                    $"\n2) Down" +
                    $"\n3) Left" +
                    $"\n4) Right");
                UserOutput.DisplayPrompt(prompt);
                prompt = ($"\nSelection: ");
                placeShipRequest.Direction = userInput.AskForShipDirection(prompt);

                // Try to place ship
                shipPlacement = player.PlayerBoard.PlaceShip(placeShipRequest);

                // Ask for direction again if invalid
                if (shipPlacement == ShipPlacement.NotEnoughSpace)
                {
                    UserOutput.InvalidEntry($"There is not enough space on board to place in this direction. Please try again.");
                }

                // Ask for direction again if invalid
                if (shipPlacement == ShipPlacement.Overlap)
                {
                    UserOutput.InvalidEntry($"This direction overlaps another ships placement. Please try again.");
                }

                // If valid direction, continue
                if (shipPlacement == ShipPlacement.Ok)
                {
                    directionOk = true;
                }
            }

            // Return direction
            return placeShipRequest;
        }

        // Method to handle ship placement requests
        public PlaceShipRequest RequestAndPlace(UserInput userInput, Player player)
        {
            // Instantiate classes
            PlaceShipRequest placeShipRequest = new PlaceShipRequest();

            // Check if ship was already used
            placeShipRequest = RequestShipType(userInput, player, placeShipRequest);

            // Check if coordinate was already populated
            placeShipRequest = RequestCoordinate(userInput, player, placeShipRequest);

            // Check if direcion is valid and place ship
            placeShipRequest = RequestDirection(userInput, player, placeShipRequest);

            // Return ship request
            return placeShipRequest;
        }

        // Method to handle the placement of ships
        public void PlayerShipPlacements(UserInput userInput, Player playerOne, Player playerTwo)
        {
            // Instantiate classes
            PlaceShipRequest request = new PlaceShipRequest();

            // Initialize variables
            int swapPlayers = 1;
            int firstPlayer = 1;
            int secondPlayer = 2;
            int firstPlayerCount = 0;
            int secondPlayerCount = 0;
            int totalShipPlacements = 10;

            // Set up boards with loop to request 5 ship placements per player
            for (int i = 0; i < totalShipPlacements; i++)
            {

                // Clear the console
                UserOutput.ClearConsole();

                // Display Splash Screen
                UserOutput.SplashScreen();

                // Swap between the two players
                // First player's choice
                if (swapPlayers == firstPlayer)
                {
                    // Display player one's turn
                    UserOutput.DisplayPlayersTurn(playerOne);
                    UserOutput.DisplayPickPlacement();

                    // Display ships placed
                    if (firstPlayerCount > 0)
                    {
                        UserOutput.DisplayPopulatedCoordinates(playerOne, firstPlayerCount);
                    }

                    // Increment first player placement count
                    firstPlayerCount++;

                    // Request and place
                    RequestAndPlace(userInput, playerOne);

                    // Swap players
                    swapPlayers++;
                }

                // Second player's choice
                else if (swapPlayers == secondPlayer)
                {
                    // Display player two's trun
                    UserOutput.DisplayPlayersTurn(playerTwo);
                    UserOutput.DisplayPickPlacement();

                    // Display ships placed
                    if (secondPlayerCount > 0)
                    {
                        UserOutput.DisplayPopulatedCoordinates(playerTwo, secondPlayerCount);
                    }

                    // Increment second player placement count
                    secondPlayerCount++;

                    // Request and place ship
                    RequestAndPlace(userInput, playerTwo);

                    // Swap players
                    swapPlayers--;
                }
            }
        }

        // Method to handle each player's turn
        public void TakePlayerTurn(UserInput userInput, Player playerOne, Player playerTwo)
        {
            // Instantiate classes
            Coordinate coordinate;
            PlaceShipRequest request = new PlaceShipRequest();
            FireShotResponse fireShotResponse = new FireShotResponse();

            // Initialize fields
            string prompt;
            bool victoryStatus = false;
            int swapPlayers = 1;
            int firstPlayer = 1;
            int secondPlayer = 2;
            int shipsPlaced = 5;

            // Create 2 player boards
            Board boardOne = new Board();
            Board boardTwo = new Board();

            // Pass board ship placement state
            boardOne = playerOne.PlayerBoard;
            boardTwo = playerTwo.PlayerBoard;

            // Loop for each player to take their turn, breaks upon vitory
            while (victoryStatus == false)
            {

                // Swap between the two players
                // First player's turn
                if (swapPlayers == firstPlayer)
                {
                    // Clear the console
                    UserOutput.ClearConsole();

                    // Display Splash Screen
                    UserOutput.SplashScreen();

                    // Display player's turn
                    UserOutput.DisplayPlayersTurn(playerOne);
                    UserOutput.DisplayPickShotCoordinate();

                    // Display grid for first player's shot history
                    Console.WriteLine($"\nBelow is your shot history.");
                    UserOutput.DisplayBoard(playerTwo.PlayerBoard);

                    // Display ships and coordinates
                    UserOutput.DisplayPopulatedCoordinates(playerOne, shipsPlaced);

                    // Loop for shot fired
                    while (true)
                    {
                        // Enter prompt and ask for coordinate
                        prompt = $"\n\nEnter the coordinates for your shot fired: " +
                            $"\nCoordinates range from A1 - J10.";
                        UserOutput.DisplayPrompt(prompt);

                        prompt = $"\nSelection: ";
                        coordinate = userInput.AskForCoordinates(prompt);
                        fireShotResponse = playerTwo.PlayerBoard.FireShot(coordinate);

                        // Display shot fired response
                        UserOutput.DisplayShotFiredResponse(fireShotResponse);

                        // Check to break out of loop
                        if ((fireShotResponse.ShotStatus != ShotStatus.Invalid) && (fireShotResponse.ShotStatus != ShotStatus.Duplicate))
                        {
                            break;
                        }
                    }

                    // If first player is not victorious, pass turn to second player
                    if (fireShotResponse.ShotStatus != ShotStatus.Victory)
                    {
                        // Prompt to switch players
                        prompt = $"\nPress any key to continue to the second player's turn.";
                        UserOutput.PressKeyToContinue(prompt);

                        // Swap player
                        swapPlayers++;
                    }

                    // If first player is victorious, game ends
                    else if (fireShotResponse.ShotStatus == ShotStatus.Victory)
                    {
                        victoryStatus = true;
                    }
                }

                // Second player's turn
                else if (swapPlayers == secondPlayer)
                {
                    // Clear the console
                    UserOutput.ClearConsole();

                    // Display player's turn
                    UserOutput.DisplayPlayersTurn(playerTwo);
                    UserOutput.DisplayPickShotCoordinate();

                    // Display grid for second player's shot history
                    Console.WriteLine($"\nBelow is your shot history.");
                    UserOutput.DisplayBoard(playerOne.PlayerBoard);

                    // Display ships and coordinates
                    UserOutput.DisplayPopulatedCoordinates(playerTwo, shipsPlaced);

                    // Loop for shot fired
                    while (true)
                    {
                        // Enter prompt and ask for coordinate
                        prompt = $"\n\nEnter the coordinates for your shot fired: " +
                            $"\nCoordinates range from A1 - J10.";
                        UserOutput.DisplayPrompt(prompt);

                        prompt = $"\nSelection: ";
                        coordinate = userInput.AskForCoordinates(prompt);
                        fireShotResponse = playerOne.PlayerBoard.FireShot(coordinate);

                        // Display shot fired response
                        UserOutput.DisplayShotFiredResponse(fireShotResponse);

                        // Check to break out of loop
                        if ((fireShotResponse.ShotStatus != ShotStatus.Invalid) && (fireShotResponse.ShotStatus != ShotStatus.Duplicate))
                        {
                            break;
                        }
                    }

                    // If second player is not victories, pass turn to first player
                    if (fireShotResponse.ShotStatus != ShotStatus.Victory)
                    {
                        // Prompt to switch players
                        prompt = $"\nPress any key to continue to the first player's turn.";
                        UserOutput.PressKeyToContinue(prompt);

                        // Swap player
                        swapPlayers--;
                    }

                    // If second player is victorious, game ends
                    else if (fireShotResponse.ShotStatus == ShotStatus.Victory)
                    {
                        victoryStatus = true;
                    }
                }
            }
        }

        // Ask user if they would like to play again
        public void PlayAgain(UserInput userInput)
        {
            // Initialize fields
            string prompt;
            int choice;
            int PlayAgain = 1;
            int StopPlaying = 2;

            // Create prompt
            prompt = $"\nWould you like to play again? " +
                "\n1: Yes" +
                "\n2: No";
            UserOutput.DisplayPrompt(prompt);

            // Ask players if they want to play again
            prompt = ($"\nSelection: ");
            choice = userInput.AskToPlayAgain(prompt, PlayAgain, StopPlaying);

            //Play again or exit based on answer
            if (choice == PlayAgain)
            {
                BattleStart();
            }

            else if (choice == StopPlaying)
            {
                // Do nothing, code continues to end
            }
        }

    }

    // Create Player Class for instantiation
    public class Player
    {
        // Create Name, PlayerBoard, and Ship Types used array
        public string Name { get; set; }
        public Board PlayerBoard { get; set; }

        public string[] enteredCoordinates = new string[5];
    }
}
