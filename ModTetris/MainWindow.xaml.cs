// Created:   Lennart Fraiman, 2015-11-10
// Revised:   
// Version: 1
// Purpose:   Tetris

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ModTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mp = new MediaPlayer();
        private PlayerX p1 = new PlayerX(1, Key.Left, Key.Right, Key.Up, Key.Down);
        private PlayerX p2 = new PlayerX(2, Key.A, Key.D, Key.W, Key.S);
        private GameLogic gl;
        public MainWindow()
        {
            InitializeComponent();
            //InitMusic();
            InitPlayers();

        }

        /// <summary>
        /// Create player, player Grid and event hook-ups
        /// </summary>
        private void InitPlayers()
        {
            gl = new GameLogic(p1,false,true);
            gl.CreateGrid(20, 10, RightCanvas, 1);
            this.KeyDown += gl.GameBoard_KeyDownPlayer;
            this.KeyUp += gl.GameBoard_KeyUpPlayer;
            gl.gameDone += gl_gameDone;
            gl.ScoreUpdate += Gl_ScoreUpdate;
            gl.StartAIGame(true);                 
        }

        private void Gl_ScoreUpdate(object sender, EventArgs e)
        {
            Score.Content = sender;
        }

        /// <summary>
        /// Start music
        /// </summary>
        private void InitMusic()
        {
            mp.Open(new Uri(@"pack://siteoforigin:,,,/Music/tetris-gameboy-02.mp3"));
            mp.MediaEnded += mp_MediaEnded;
            Task.Run(new Action(() =>
                {
                    mp.Dispatcher.Invoke(new Action(() =>
                        {
                            mp.Play();
                        }));
                })); ;
        }

        /// <summary>
        /// Triggerd when music is ended. used to restart the music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mp_MediaEnded(object sender, EventArgs e)
        {
            mp.Position = new TimeSpan();
            mp.Play();
        }

        /// <summary>
        /// Triggerd when the Player grid is full. Used to reset the Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl2_gameDone(object sender, EventArgs e)
        {
            ((GameLogic)sender).ResetGame();
        }

        /// <summary>
        /// Triggerd when the Player grid is full. Used to reset the Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_gameDone(object sender, EventArgs e)
        {
            //MessageBox.Show("GameOver");
            ((GameLogic)sender).ResetGame();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Un-hook events
            this.KeyDown -= gl.GameBoard_KeyDownPlayer;
            this.KeyUp -= gl.GameBoard_KeyUpPlayer;

            //Dispose of game logic
            gl.Dispose();
        }
        
    }
}
