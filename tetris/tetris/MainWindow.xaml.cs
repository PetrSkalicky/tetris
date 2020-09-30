using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Media;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Drawing dr = new Drawing(MainCanvas.Children, NextCanvas.Children); //instance Drawing bere v konstruktoru jako parametry odkaz na Děti Canvasů, v rámci Drawing se na Canvasy umístí objekty
            game = new Game(dr);                                                //instance Drawing se předá do třídy obsahující hlavní logiku hry
            DataContext = game;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)    //při stisku tlačítka Start
        {
            game.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)                                    //při kliknutí na tlačítko Pause
        {
            game.pauseGame();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("\r\n\nGame controls:\r\nA - Move left\r\nD - Move right\r\nW  - Rotate\r\nS - Drop\r\nSpacebar - Pause / Resume");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)                          //zde se po stisku příslušných kláves volají metody pro pohyby bloku
        {
            game.KeyDown(e.Key);
        }
    }
}