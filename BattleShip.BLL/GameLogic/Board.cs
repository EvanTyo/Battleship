using System;
using System.Collections.Generic;
using System.Linq;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.BLL.GameLogic
{
    public class Board
    {
        public const int MAX_SHIPS = 5;
        public const int MIN_COORDINATE = 1;
        public const int MAX_COORDINATE = 10;

        private Dictionary<Coordinate, ShotHistory> _shotHistory;
        private int _currentShipIndex;

        public Ship[] Ships { get; private set; }

        public Board()
        {
            _shotHistory = new Dictionary<Coordinate, ShotHistory>();
            _currentShipIndex = 0;

            Ships = new Ship[MAX_SHIPS];
        }

        public FireShotResponse FireShot(Coordinate coordinate)
        {
            FireShotResponse response = new FireShotResponse();

            // is this coordinate on the board?
            if (!IsValidCoordinate(coordinate))
            {
                response.ShotStatus = ShotStatus.Invalid;
            }
            // did they already try this position?
            else if (_shotHistory.ContainsKey(coordinate))
            {
                response.ShotStatus = ShotStatus.Duplicate;
            }
            else
            {
                CheckShipsForHit(coordinate, response);
                CheckForVictory(response);
            }

            return response;
        }

        public ShotHistory CheckCoordinate(Coordinate coordinate)
        {
            return (_shotHistory.ContainsKey(coordinate) ? _shotHistory[coordinate] : Responses.ShotHistory.Unknown);
        }

        public ShipPlacement PlaceShip(PlaceShipRequest request)
        {
            if (_currentShipIndex > MAX_SHIPS - 1)
            {
                throw new Exception($"You can not add another ship, {MAX_SHIPS} is the limit!");
            }

            if (!IsValidCoordinate(request.Coordinate))
            {
                return ShipPlacement.NotEnoughSpace;
            }

            Ship newShip = ShipCreator.CreateShip(request.ShipType);
            int xIncrement = 0;
            int yIncrement = 0;

            switch (request.Direction)
            {
                case ShipDirection.Down:
                    yIncrement = 1;
                    break;

                case ShipDirection.Up:
                    yIncrement = -1;
                    break;

                case ShipDirection.Left:
                    xIncrement = -1;
                    break;

                default:
                    xIncrement = 1;
                    break;
            }

            return PlaceShip(request.Coordinate, newShip, xIncrement, yIncrement);
        }

        private void CheckForVictory(FireShotResponse response)
        {
            if (response.ShotStatus == ShotStatus.HitAndSunk)
            {
                // did they win?
                if (Ships.All(s => s.IsSunk))
                {
                    response.ShotStatus = ShotStatus.Victory;
                }
            }
        }

        private void CheckShipsForHit(Coordinate coordinate, FireShotResponse response)
        {
            ShotHistory shotHistory = Responses.ShotHistory.Miss;
            response.ShotStatus = ShotStatus.Miss;

            // check only un-sunk ships
            foreach (var ship in Ships.Where(s => !s.IsSunk))
            {
                response.ShotStatus = ship.FireAtShip(coordinate);

                // if a hit, set response, history type
                if (response.ShotStatus != ShotStatus.Miss)
                {
                    // Only return ship if sunk
                    if (response.ShotStatus == ShotStatus.HitAndSunk)
                    {
                        response.ShipImpacted = ship.ShipName;
                    }
                    shotHistory = Responses.ShotHistory.Hit;
                    // hit can only be 1 ship, all done
                    break;
                }
            }

            // Save history
            _shotHistory.Add(coordinate, shotHistory);
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.XCoordinate >= MIN_COORDINATE && coordinate.XCoordinate <= MAX_COORDINATE
                && coordinate.YCoordinate >= MIN_COORDINATE && coordinate.YCoordinate <= MAX_COORDINATE;
        }

        private ShipPlacement PlaceShip(Coordinate coordinate, Ship newShip, int xIncrement, int yIncrement)
        {
            if (xIncrement != 0 && yIncrement != 0)
            {
                throw new Exception("Diagonal positioning not allowed");
            }

            if (Math.Abs(xIncrement) > 1 || Math.Abs(yIncrement) > 1)
            {
                throw new Exception("Invalid increment. Must be zero or 1");
            }

            if (Ships.Any(s => s != null && s.ShipType == newShip.ShipType))
            {
                throw new Exception("Ship has already been added");
            }
            

            int positionIndex = 0;
            int x = coordinate.XCoordinate;
            int y = coordinate.YCoordinate;

            for (int i = newShip.BoardPositions.Length; i > 0; i--, x += xIncrement, y += yIncrement)
            {
                Coordinate currentCoordinate = new Coordinate(x, y);

                if (!IsValidCoordinate(currentCoordinate))
                {
                    return ShipPlacement.NotEnoughSpace;
                }

                if (OverlapsAnotherShip(currentCoordinate))
                {
                    return ShipPlacement.Overlap;
                }

                newShip.BoardPositions[positionIndex++] = currentCoordinate;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private void AddShipToBoard(Ship newShip)
        {
            Ships[_currentShipIndex++] = newShip;
        }

        private bool OverlapsAnotherShip(Coordinate coordinate)
        {
            return (Ships.Any(s => s != null && s.BoardPositions.Contains(coordinate)));
        }
    }
}