// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModTetris.Pieces;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.Timers;
using System.Threading;

namespace ModTetris
{
    public class GameLogic
    {
        private IPiece currentPiece;
        private List<Rectangle> blockList = new List<Rectangle>();
        private List<Rectangle> blockListRight = new List<Rectangle>();
        private List<Rectangle> blockListLeft = new List<Rectangle>();
        private List<Rectangle> blockListTop = new List<Rectangle>();
        private int row,colum,playerID = 0;
        private System.Timers.Timer t = new System.Timers.Timer();
        private bool[] isPressingKey = new bool[4];
        private PlayerX _player;
        private bool testMode = false;
        private bool active = true;
        private int score = 0;
        private bool AILoop = false;
        private bool _ai = false;

        private List<Rectangle> blockPos = new List<Rectangle>();
        private Canvas GameBoard;

        public event EventHandler gameDone;
        public event EventHandler ScoreUpdate;

        public AutoResetEvent waitForCancel = new AutoResetEvent(true);

        /// <summary>
        /// Constructor that takes one parameter.
        /// </summary>
        /// <param name="Player">The player affected by this board</param>
        public GameLogic(PlayerX Player, bool TestMode = false, bool AI = false)
        {
            if (TestMode == true)
            {
                _player = Player;
                playerID = _player.Id;
                testMode = TestMode;
            }
            else if (AI == true)
            {
                _player = Player;
                playerID = _player.Id;
                _ai = AI;
            }
            else
            {
                _player = Player;
                playerID = _player.Id;
                t.Interval = 1000;
                t.Elapsed += t_Elapsed;
                t.Start();
            }
        }
        
        public void StartAIGame(bool testMove = false)
        {
            AILoop = true;

            while(AILoop)
            {
                if (CanMove(MoveDirectionEnum.Down, currentPiece))
                    BeginMove(MoveDirectionEnum.Down, currentPiece);    

                if(_player.MoveLeft || testMove)
                {
                    if (CanMove(MoveDirectionEnum.Left, _player.CurrentPiece))
                        BeginMove(MoveDirectionEnum.Left, _player.CurrentPiece);
                    testMove = false;
                }
                else if(_player.MoveRight || testMove)
                {
                    if (CanMove(MoveDirectionEnum.Right, _player.CurrentPiece))
                        BeginMove(MoveDirectionEnum.Right, _player.CurrentPiece);
                }

            }
        }

        /// <summary>
        /// Timer that triggers a down movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            GameBoard.Dispatcher.Invoke(new Action(() =>
                {
                    if (CanMove(MoveDirectionEnum.Down,currentPiece))
                        BeginMove(MoveDirectionEnum.Down,currentPiece);
                }));
        }

        /// <summary>
        /// Checks if the current piece can be moved
        /// </summary>
        /// <param name="MoveDirection">The direction of the move</param>
        /// <returns></returns>
        public bool CanMove(MoveDirectionEnum MoveDirection, IPiece pieceToMove = null)
        {
            if(pieceToMove == null)
            {
                pieceToMove = currentPiece;
            }
            foreach (Rectangle r in pieceToMove.BitOfShape)
            {
                if (MoveDirection == MoveDirectionEnum.Down)
                {

                    if (blockList.Any(x => x == r))
                    {
                        pieceToMove.BitOfShape.ForEach(x => x.Fill = Brushes.WhiteSmoke);
                        //blockPos.AddRange(currentPiece.BitOfShape);
                        GeneratePiece();
                        score += 10;
                        OnScoreUpdate(score,new EventArgs());
                        return false;
                    }

                    if (currentPiece.BitOfShape.Any(rec => _player.CurrentPiece.BitOfShape.Exists(
                        x => ((x.Tag as TagObject).Y == (rec.Tag as TagObject).Y + 1) && (
                        (x.Tag as TagObject).X == (rec.Tag as TagObject).X))))
                    {
                        OnplayerGameOver(this, new EventArgs());
                        return false;
                    }

                    //if (blockPos.Any(x => (((TagObject)x.Tag).Y == (((TagObject)r.Tag).Y + 1))
                    //&& (((TagObject)x.Tag).X == (((TagObject)r.Tag).X))))
                    //{
                    //    blockPos.AddRange(currentPiece.BitOfShape);
                    //    if (blockPos.Any(x => blockListTop.Any(y => y == x)))
                    //    {
                    //        OnplayerGameOver(this, new EventArgs());
                    //    }
                    //    else
                    //    {
                    //        GeneratePiece();
                    //    }
                    //    return false;
                    //}
                }
                if (MoveDirection == MoveDirectionEnum.Right)                    
                {
                    score -= 1;
                    OnScoreUpdate(score, new EventArgs());
                    if (blockListRight.Any(x => x == r))
                    {
                        return false;
                    }
                    if (blockPos.Any(x => (((TagObject)x.Tag).Y == (((TagObject)r.Tag).Y))
                    && (((TagObject)x.Tag).X == (((TagObject)r.Tag).X + 1))))
                    {
                        return false;
                    }
                }
                if (MoveDirection == MoveDirectionEnum.Left)
                {
                    score -= 1;
                    OnScoreUpdate(score, new EventArgs());
                    if (blockListLeft.Any(x => x == r))
                    {
                        return false;
                    }
                    if (blockPos.Any(x => (((TagObject)x.Tag).Y == (((TagObject)r.Tag).Y))
                    && (((TagObject)x.Tag).X == (((TagObject)r.Tag).X - 1))))
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// Moves a Piece in a given direction
        /// </summary>
        /// <param name="MoveDirection">Direction to move the piece</param>
        /// <returns></returns>
        public void BeginMove(MoveDirectionEnum MoveDirection, IPiece pieceToMove = null)
        {
            if(pieceToMove == null)
            {
                pieceToMove = currentPiece;
            }
            if(MoveDirection == MoveDirectionEnum.Down)
            {
                movePiece(0, 1,pieceToMove);
            }
            if (MoveDirection == MoveDirectionEnum.Left)
            {
                movePiece(-1, 0, pieceToMove);
            }
            if (MoveDirection == MoveDirectionEnum.Right)
            {
                movePiece(1, 0, pieceToMove);
            }
        }

        /// <summary>
        /// Move a given piece in a direction by coordinates.
        /// </summary>
        /// <param name="xpos">X coordinate</param>
        /// <param name="ypos">Y coordinate</param>
        /// <param name="pToMove">Piece to move</param>
        private void movePiece(int xpos, int ypos, IPiece pToMove)
        {
            pToMove.BitOfShape.ForEach(x => x.Fill = Brushes.WhiteSmoke);
            List<TagObject> tl = new List<TagObject>();
            pToMove.BitOfShape.ForEach(x => tl.Add((TagObject)x.Tag));
            pToMove.BitOfShape.Clear();
            foreach (TagObject tb in tl)
                pToMove.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(
                    x => (((TagObject)x.Tag).Y == (tb.Y + ypos))
                    && (((TagObject)x.Tag).X == (tb.X + xpos))).ToList());

            pToMove.BitOfShape.ForEach(p => p.Fill = ((Piece1)pToMove).Color);
        }

        /// <summary>
        /// Collition check after rotation
        /// </summary>
        /// <param name="in1">Object to check</param>
        /// <param name="in2">Object to check against</param>
        /// <param name="m">Move to check</param>
        /// <returns>True if the same position</returns>
        private bool TagCheck(TagObject in1,TagObject in2,MoveDirectionEnum m)
        {
            if(m == MoveDirectionEnum.Down)
                if ((in1.X == in2.X) && (in1.Y == in2.Y - 1))
                    return true;
            if (m == MoveDirectionEnum.Left)
                if ((in1.X == in2.X + 1) && (in1.Y == in2.Y))
                    return true;
            if (m == MoveDirectionEnum.Right)
                if ((in1.X == in2.X - 1) && (in1.Y == in2.Y + 1))
                    return true;
            if (m == MoveDirectionEnum.Top)
                if ((in1.X == in2.X) && (in1.Y == in2.Y + 1))
                    return true;  
            return false;
        }
        
        /// <summary>
        /// Collition check after rotation
        /// </summary>
        /// <param name="in1">Object to check</param>
        /// <param name="in2">Objeckt to check against</param>
        /// <returns>True if same position</returns>
        private bool TagCheck(TagObject in1, TagObject in2)
        {
            if ((in1.X == in2.X) && (in1.Y == in2.Y))
                return true;
            return false;
        }
                
        /// <summary>
        /// Generate a random Piece and set it as the current piece
        /// </summary>
        public void GeneratePiece()
        {
            //r.ForEach(x => x.Fill = Brushes.Black);
            Random r = new Random();
            currentPiece = PieceShapeColor(0);

            //currentPiece = PieceShapeColor(r.Next(7));
        }

        /// <summary>
        /// Generate a random Piece and set it as the current piece
        /// </summary>
        public void GeneratePieceForPlayer()
        {
            _player.CurrentPiece = PieceShapeColor(1);
        }


        /// <summary>
        /// Generate a fix Piece and set it as the current piece. 
        /// Used for testing
        /// </summary>
        /// <param name="PieceType"></param>
        public void GeneratePiece(int PieceType)
        {
            currentPiece = PieceShapeColor(PieceType);
        }

        /// <summary>
        /// Create a piece by index
        /// </summary>
        /// <param name="pieceShape"></param>
        /// <returns>Returns the generated Piece</returns>
        private IPiece PieceShapeColor(int pieceShape)
        {
            Piece1 p1 = new Piece1();
            p1.NumberOfBits = 4;
            if (pieceShape == 0)
            {
                p1.Color = Brushes.CornflowerBlue;
                p1.BitOfShape = new List<Rectangle>();
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X3Y0").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X3Y1").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X4Y1").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X4Y0").ToList());
                p1.NoRotate = true;
                p1.BitOfShape.ForEach(p => p.Fill = p1.Color);
            }
            if (pieceShape == 1)
            {
                p1.Color = Brushes.Black;
                p1.BitOfShape = new List<Rectangle>();
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X3Y18").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X3Y19").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X4Y19").ToList());
                p1.BitOfShape.AddRange(GameBoard.Children.OfType<Rectangle>().Where(x => x.Name == "id" + playerID + "X4Y18").ToList());
                p1.NoRotate = true;
                p1.BitOfShape.ForEach(p => p.Fill = p1.Color);
            }
            return p1;
        }



        /// <summary>
        /// Reset the GameBoard
        /// </summary>
        public void ResetGame()
        {
            score = 0;
            AILoop = false;
            blockPos.Clear();
            //GameBoard.Children.OfType<Rectangle>().ToList().ForEach((x => x.Fill = Brushes.WhiteSmoke ));
            //GeneratePiece();
            //GeneratePieceForPlayer();
        }

        /// <summary>
        /// Used for test only
        /// </summary>
        public IPiece CurrentPiece
        {
            get
            {
                return currentPiece;
            }
        }

        /// <summary>
        /// Creates the gameboard(Grid)
        /// </summary>
        /// <param name="Row">Number of rows in Grid</param>
        /// <param name="Colum">Number of colums in Grid</param>
        /// <param name="GridGUI">Canvas to use as Game Grid</param>
        /// <param name="PlayerGridID">Player id this Game grid belongs to.</param>
        public void CreateGrid(int Row, int Colum, Canvas GridGUI,int PlayerGridID)
        {
            row = Row;
            colum = Colum;
            GameBoard = GridGUI;
            playerID = PlayerGridID;
            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < colum; i++)
                {
                    Rectangle r = new Rectangle();
                    r.Name = string.Format("id" + playerID + "X{0}Y{1}", i, j);
                    GridGUI.RegisterName(r.Name, r);
                    r.Width = 20;
                    r.Height = 20;
                    r.Tag = new TagObject() { X = i, Y = j };
                    r.Fill = Brushes.WhiteSmoke;
                    GridGUI.Children.Add(r);
                    Canvas.SetLeft(r, (21 * i));
                    Canvas.SetTop(r, (21 * j));

                    if (j == 0)
                        blockListTop.Add(r);
                    if (j == row - 1)
                        blockList.Add(r);
                    if (i== 0)
                        blockListLeft.Add(r);
                    if(i == colum -1)
                        blockListRight.Add(r);
                }
            }
            //blockList.ForEach(x => x.Fill = Brushes.SkyBlue);
            GeneratePiece();
            GeneratePieceForPlayer();
            //Task.Run(new Action(() =>
            //{
            //    KeyMasterRight();
            //}));
            if (!testMode && !_ai)
            {
                Task.Run(new Action(() =>
                {
                    KeyMaster();
                }));
            }
            OnScoreUpdate(score,new EventArgs());

        }

        /// <summary>
        /// Checks if player controls are used.
        /// </summary>
        private void KeyMaster()
        {
            while (active)
            {
                GameBoard.Dispatcher.Invoke(new Action(() =>
                    {
                        //if (isPressingKey[0])
                        //{
                        //    if (CanMove(MoveDirectionEnum.Down,_player.CurrentPiece))
                        //    {
                        //        BeginMove(MoveDirectionEnum.Down, _player.CurrentPiece);
                        //    }
                        //}
                        if (isPressingKey[1])
                        {
                            if (CanMove(MoveDirectionEnum.Left, _player.CurrentPiece))
                            {
                                BeginMove(MoveDirectionEnum.Left, _player.CurrentPiece);
                            }
                        }
                        if (isPressingKey[2])
                        {
                            if (CanMove(MoveDirectionEnum.Right, _player.CurrentPiece))
                            {
                                BeginMove(MoveDirectionEnum.Right,_player.CurrentPiece);
                            }
                        }
                    }));
                Thread.Sleep(50);
            } 
            waitForCancel.Set();
        }

        /// <summary>
        /// Event triggerd when the player press down a button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GameBoard_KeyDownPlayer(object sender, KeyEventArgs e)
        {
            if (isPressingKey.Any(x => x != true))
            {
                //if (_player.Down == e.Key)
                //    if (!isPressingKey[0])
                //    {
                //        isPressingKey[0] = true;
                //    }
                if (_player.Left == e.Key)
                    if (!isPressingKey[1])
                    {
                        isPressingKey[1] = true;
                    }
                if (_player.Right == e.Key)
                    if (!isPressingKey[2])
                    {
                        isPressingKey[2] = true;
                    }
            }
        }
 
        public void GameBoard_KeyUpPlayer(object sender, KeyEventArgs e)
        {
            //if (_player.Down == e.Key)
            //    isPressingKey[0] = false;
            if (_player.Left == e.Key)
                isPressingKey[1] = false;
            if (_player.Right == e.Key)
                isPressingKey[2] = false;
        }


        /// <summary>
        /// Notify listners that player result as changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnplayerGameOver(object sender, EventArgs e)
        {
            EventHandler handler = gameDone;

            //If handler is not null run handler
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnScoreUpdate(int sender, EventArgs e)
        {
            EventHandler handler = ScoreUpdate;

            //If handler is not null run handler
            if (handler != null)
            {
                handler(score, e);
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Stop time thread and wait for KeyMaster thread if in sleep.
                    t.Stop();
                    t.Dispose();
                    active = false;
                    waitForCancel.WaitOne();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
    public class TagObject : IComparable<TagObject>
    {
        public TagObject()
        {

        }
        public double X { get; set; }
        public double Y { get; set; }

        int IComparable<TagObject>.CompareTo(TagObject other)
        {
            if (other.Y > this.Y)
                return -1;
            else if (other.Y == this.Y)
                return 0;
            else
                return 1;
        }

    }
}
