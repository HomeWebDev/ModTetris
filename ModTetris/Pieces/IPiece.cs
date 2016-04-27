// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
namespace ModTetris.Pieces
{
    /// <summary>
    /// Interface for Pieces
    /// </summary>
    public interface IPiece
    {
        /// <summary>
        /// How many bits the piece is made of
        /// </summary>
        int NumberOfBits { get; set; }
        /// <summary>
        /// The list with the Rectangle that make up the piece
        /// </summary>
        List<Rectangle> BitOfShape { get; set; }
    }
}
