using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;
using NUnit.Framework;

namespace Battleship.Tests
{
    [TestFixture]
    public class FireShotTests
    {
        #region "Board Setup"
        /// <summary>
        /// Let's set up a board as follows:
        /// Destroyer: (1,8) (2,8)
        /// cRuiser: (3,1) (3,2) (3,3)
        /// Sub: (1,5) (2,5) (3,5)
        /// Battleship: (10,6) (10,7) (10,8) (10, 9)
        /// Carrier: (4,4) (5,4) (6,4) (7,4) (8,4)
        /// 
        ///    1 2 3 4 5 6 7 8 9 10
        ///  1     R
        ///  2     R
        ///  3     R
        ///  4       C C C C C
        ///  5 S S S
        ///  6                   B
        ///  7                   B
        ///  8 D D               B
        ///  9                   B
        /// 10
        /// </summary>
        /// <returns>A board that is ready to play</returns>
        private Board SetupBoard()
        {
            Board board = new Board();

            PlaceDestroyer(board);
            PlaceCruiser(board);
            PlaceSubmarine(board);
            PlaceBattleship(board);
            PlaceCarrier(board);

            return board;
        }

        private void PlaceCarrier(Board board)
        {
            var request = new PlaceShipRequest()
            {
                Coordinate = new Coordinate(4, 4),
                Direction = ShipDirection.Right,
                ShipType = ShipType.Carrier
            };

            board.PlaceShip(request);
        }

        private void PlaceBattleship(Board board)
        {
            var request = new PlaceShipRequest()
            {
                Coordinate = new Coordinate(10, 6),
                Direction = ShipDirection.Down,
                ShipType = ShipType.Battleship
            };

            board.PlaceShip(request);
        }

        private void PlaceSubmarine(Board board)
        {
            var request = new PlaceShipRequest()
            {
                Coordinate = new Coordinate(3, 5),
                Direction = ShipDirection.Left,
                ShipType = ShipType.Submarine
            };

            board.PlaceShip(request);
        }

        private void PlaceCruiser(Board board)
        {
            var request = new PlaceShipRequest()
            {
                Coordinate = new Coordinate(3, 3),
                Direction = ShipDirection.Up,
                ShipType = ShipType.Cruiser
            };

            board.PlaceShip(request);
        }

        private void PlaceDestroyer(Board board)
        {
            var request = new PlaceShipRequest()
            {
                Coordinate = new Coordinate(1, 8),
                Direction = ShipDirection.Right,
                ShipType = ShipType.Destroyer
            };

            board.PlaceShip(request);
        }
        #endregion

        [Test]
        public void CoordinateEquality()
        {
            var c1 = new Coordinate(5, 10);
            var c2 = new Coordinate(5, 10);

            Assert.AreEqual(c1, c2);
        }

        [Test]
        public void CanNotFireOffBoard()
        {
            var board = SetupBoard();

            var coordinate = new Coordinate(15, 20);

            var response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Invalid, response.ShotStatus);
        }

        [Test]
        public void CanNotFireDuplicateShot()
        {
            var board = SetupBoard();

            var coordinate = new Coordinate(5, 5);
            var response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Miss, response.ShotStatus);

            // fire same shot
            response = board.FireShot(coordinate);
            Assert.AreEqual(ShotStatus.Duplicate, response.ShotStatus);
        }

        [Test]
        public void CanMissShip()
        {
            var board = SetupBoard();

            var coordinate = new Coordinate(5, 5);
            var response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Miss, response.ShotStatus);
        }

        [Test]
        public void CanHitShip()
        {
            var board = SetupBoard();

            var coordinate = new Coordinate(3, 1);
            var response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Hit, response.ShotStatus);
        }

        [Test]
        public void CanSinkShip()
        {
            var board = SetupBoard();

            var coordinate = new Coordinate(10, 6);
            var response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Hit, response.ShotStatus);

            coordinate = new Coordinate(10, 7);
            response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Hit, response.ShotStatus);

            coordinate = new Coordinate(10, 8);
            response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.Hit, response.ShotStatus);

            coordinate = new Coordinate(10, 9);
            response = board.FireShot(coordinate);

            Assert.AreEqual(ShotStatus.HitAndSunk, response.ShotStatus);
            Assert.AreEqual("Battleship", response.ShipImpacted);
        }

        [Test]
        public void CanWinGame()
        {
            var board = SetupBoard();

            Assert.AreEqual(ShotStatus.HitAndSunk, SinkDestroyer(board).ShotStatus);
            Assert.AreEqual(ShotStatus.HitAndSunk, SinkCruiser(board).ShotStatus);
            Assert.AreEqual(ShotStatus.HitAndSunk, SinkSubmarine(board).ShotStatus);
            Assert.AreEqual(ShotStatus.HitAndSunk, SinkBattleship(board).ShotStatus);
            Assert.AreEqual(ShotStatus.Victory, SinkCarrier(board).ShotStatus);
        }

        private FireShotResponse SinkCarrier(Board board)
        {
            var coordinate = new Coordinate(4, 4);
            board.FireShot(coordinate);

            coordinate = new Coordinate(5, 4);
            board.FireShot(coordinate);

            coordinate = new Coordinate(6, 4);
            board.FireShot(coordinate);

            coordinate = new Coordinate(7, 4);
            board.FireShot(coordinate);

            coordinate = new Coordinate(8, 4);
            return board.FireShot(coordinate);
        }

        private FireShotResponse SinkBattleship(Board board)
        {
            var coordinate = new Coordinate(10, 6);
            board.FireShot(coordinate);

            coordinate = new Coordinate(10, 7);
            board.FireShot(coordinate);

            coordinate = new Coordinate(10, 8);
            board.FireShot(coordinate);

            coordinate = new Coordinate(10, 9);
            return board.FireShot(coordinate);
        }

        private FireShotResponse SinkSubmarine(Board board)
        {
            var coordinate = new Coordinate(1, 5);
            board.FireShot(coordinate);

            coordinate = new Coordinate(2, 5);
            board.FireShot(coordinate);

            coordinate = new Coordinate(3, 5);
            return board.FireShot(coordinate);
        }

        private FireShotResponse SinkCruiser(Board board)
        {
            var coordinate = new Coordinate(3, 1);
            board.FireShot(coordinate);

            coordinate = new Coordinate(3, 2);
            board.FireShot(coordinate);

            coordinate = new Coordinate(3, 3);
            return board.FireShot(coordinate);
        }

        private FireShotResponse SinkDestroyer(Board board)
        {
            var coordinate = new Coordinate(1, 8);
            board.FireShot(coordinate);

            coordinate = new Coordinate(2, 8);
            return board.FireShot(coordinate);
        }
    }
}