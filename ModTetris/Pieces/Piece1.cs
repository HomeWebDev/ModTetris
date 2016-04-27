// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModTetris.Pieces
{
    public class Piece1 :Pieces.PieceBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Piece1()
        {

        }
        /// <summary>
        /// If piece can rotate or not
        /// </summary>
        public bool NoRotate { get; set; }
    }
}
