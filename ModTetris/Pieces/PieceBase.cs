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
using System.Windows.Media;

namespace ModTetris.Pieces
{
    /// <summary>
    /// Base for piece
    /// </summary>
    public abstract class PieceBase : IPiece
    {
        public int NumberOfBits { get; set; }
        public List<Rectangle> BitOfShape { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
