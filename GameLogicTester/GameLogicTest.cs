// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModTetris;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Shapes;
using ModTetris.Pieces;
using System.Windows;
using System.Windows.Media;

namespace GameLogicTester
{
    [TestClass]
    public class GameLogicTest
    {
        [TestMethod]
        public void IsPieceRotatedCorrectTest()
        {
            
            //Test setup
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            NameScope.SetNameScope(canvas,new NameScope());
            GameLogic g = new GameLogic(new PlayerX(),true);
            List<Rectangle> testList = new List<Rectangle>();
            g.CreateGrid(10, 10, canvas, 1);

            //Rotation should fail

            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Down);
            g.BeginMove(MoveDirectionEnum.Down);
            g.BeginMove(MoveDirectionEnum.Down);


        }
        [TestMethod]
        public void IsPieceMovedDownCorrectTest()
        {

            //Test setup
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            NameScope.SetNameScope(canvas, new NameScope());
            GameLogic g = new GameLogic(new PlayerX(), true);
            List<Rectangle> testList = new List<Rectangle>();
            g.CreateGrid(4, 10, canvas, 1);

            //Move should be ok
            Assert.IsTrue(g.CanMove(MoveDirectionEnum.Down));

            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Down);
            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Down);
            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Down);

            //Move should NOT be ok
            Assert.IsFalse(g.CanMove(MoveDirectionEnum.Down));

        }
        [TestMethod]
        public void IsPieceMovedRightCorrectTest()
        {

            //Test setup
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            NameScope.SetNameScope(canvas, new NameScope());
            GameLogic g = new GameLogic(new PlayerX(), true);
            List<Rectangle> testList = new List<Rectangle>();
            g.CreateGrid(10, 7, canvas, 1);

            //Generate a fix Piece
            g.GeneratePiece(1);

            //Move should be ok
            Assert.IsTrue(g.CanMove(MoveDirectionEnum.Right));

            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Right);

            //Move should NOT be ok
            Assert.IsFalse(g.CanMove(MoveDirectionEnum.Right));

        }
        [TestMethod]
        public void IsPieceMovedLeftCorrectTest()
        {

            //Test setup
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            NameScope.SetNameScope(canvas, new NameScope());
            GameLogic g = new GameLogic(new PlayerX(), true);
            List<Rectangle> testList = new List<Rectangle>();
            g.CreateGrid(10, 5, canvas, 1);

            //Generate a fix Piece
            g.GeneratePiece(1);

            //Move should be ok
            Assert.IsTrue(g.CanMove(MoveDirectionEnum.Left));

            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Left);
            //move piece to valid position
            g.BeginMove(MoveDirectionEnum.Left);

            //Move should NOT be ok
            Assert.IsFalse(g.CanMove(MoveDirectionEnum.Left));
        }
        [TestMethod]
        public void IsRowsRemovedCorrect()
        {

            //Test setup
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            NameScope.SetNameScope(canvas, new NameScope());
            GameLogic g = new GameLogic(new PlayerX(), true);
            List<Rectangle> testList = new List<Rectangle>();
            g.CreateGrid(10, 10, canvas, 1);

            //Create a row to delete
            for (int i = 0; i < 10; i++)
            {
                Rectangle newColor = (Rectangle)canvas.FindName("id1" + "X" + i + "Y6");
                newColor.Fill = Brushes.Azure;
            }
            //Expected result
            int expectedRowsRemoved = 1;
        }
    }
}
