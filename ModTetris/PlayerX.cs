// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using ModTetris.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModTetris
{
    public class PlayerX
    {
        private IPiece currentPiece;

        /// <summary>
        /// Constructor used for testing
        /// </summary>
        public PlayerX() : this(1,Key.Left,Key.Right,Key.Up,Key.Down)
        {

        }
        /// <summary>
        /// Defualt contructor
        /// </summary>
        /// <param name="id">Player id</param>
        /// <param name="left">Key to move Piece left</param>
        /// <param name="right">Key to move Piece rght</param>
        /// <param name="rotate">Key to rotate Piece</param>
        /// <param name="down">Key to move Piece down</param>
        public PlayerX(int id,Key left,Key right, Key rotate, Key down)
        {
            Id = id;
            Left = left;
            Right = right;
            Rotate = rotate;
            Down = down;
        }

        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }
        public bool MoveDown { get; set; }
        public bool MoveUp { get; set; }

        public IPiece CurrentPiece { get; set; }
        /// <summary>
        /// Player id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Key to move Piece left
        /// </summary>
        public Key Left { get; set; }
        /// <summary>
        /// Key to move Piece right
        /// </summary>
        public Key Right { get; set; }
        /// <summary>
        /// Key to move Piece rotate
        /// </summary>
        public Key Rotate { get; set; }
        /// <summary>
        /// Key to move Piece down
        /// </summary>
        public Key Down { get; set; }
    }
}
