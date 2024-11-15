﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Reflection.Metadata.BlobBuilder;
using Color = System.Windows.Media.Color;

namespace MinesweeperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int gridSize;
        private int nbMine;
        private int nbCellOpen = 0;
        private int nbFlags;
        private int[,] matrix;
        private int[] tabX;
        private int[] tabY;
        private int nbBombesPlaces = 0;
        private int nbTemps;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            initialisation();
            CBXLevel.Items.Add("Facile");
            CBXLevel.Items.Add("Moyen");
            CBXLevel.Items.Add("Difficile");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); ;
            timer.Tick += timer_tick;
            timer.Start();
        }

        private int setGridSize()
        {
            int grdSize = 0;
            if(CBXLevel.SelectedItem == "Facile")
            {
                grdSize = 8;
            }
            if(CBXLevel.SelectedItem == "Moyen")
            {
                grdSize = 18;
            }
            if (CBXLevel.SelectedItem == "Difficile")
            {
                grdSize = 20;

            }
            return grdSize;
        }

        private int setNbMine()
        {
            int NbreMine = 0;
            if (CBXLevel.SelectedItem == "Facile")
            {
                NbreMine = 10;
            }
            if (CBXLevel.SelectedItem == "Moyen")
            {
                NbreMine = 35;
            }
            if (CBXLevel.SelectedItem == "Difficile")
            {
                NbreMine = 50;
            }
            return NbreMine;
        }


        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Button)sender).Content == null)
            {
                ((Button)sender).Content = "F";
                nbFlags--;
                LBLFlags.Content = "Flags : " + nbFlags;
            }
            else
            {
                ((Button)sender).Content = null;
                nbFlags++;
                LBLFlags.Content = "Flags : " + nbFlags;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            nbTemps++;
            LBLTemps.Content = nbTemps;
        }

        private void initialisation()
        {
            this.nbMine = setNbMine();
            this.gridSize = setGridSize();
            this.nbFlags = nbMine;
            matrix = new int[gridSize, gridSize];
            tabX = new int[nbMine];
            tabY = new int[nbMine];
            nbBombesPlaces = 0;
            nbTemps = 0;
            nbCellOpen = 0;
            GRDGame.Children.Clear();
            GRDGame.ColumnDefinitions.Clear();
            GRDGame.RowDefinitions.Clear();
            

            for (int i = 0; i < gridSize; i++)
            {
                GRDGame.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                GRDGame.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            }
            fillGrid();
            LBLTemps.Content = nbTemps;
            LBLFlags.Content = "Flags : " + nbFlags;

        }

        private void fillGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Border b = new Border();
                    b.BorderThickness = new Thickness(0);
                    b.SetValue(Grid.RowProperty, j);
                    b.SetValue(Grid.ColumnProperty, i);
                    GRDGame.Children.Add(b);

                    Grid grid = new Grid();

                    grid.Height = b.Height;
                    b.Child = grid;

                    Label LBLNombre = new Label();
                    LBLNombre.FontSize = 20;
                    LBLNombre.FontWeight = FontWeights.Bold;
                    LBLNombre.HorizontalContentAlignment = HorizontalAlignment.Center;
                    LBLNombre.VerticalContentAlignment = VerticalAlignment.Center;
                    grid.Children.Add(LBLNombre);



                    Button button = new Button();
                    button.Click += Button_Click;
                    button.FontSize = 20;
                    button.FontWeight = FontWeights.Bold;
                    button.BorderThickness = new Thickness(0);
                    button.HorizontalContentAlignment = HorizontalAlignment.Center;
                    button.VerticalContentAlignment = VerticalAlignment.Center;
                    button.Foreground = Brushes.Red;


                    grid.Children.Add(button);
                    int col = Grid.GetColumn(b);
                    int row = Grid.GetRow(b);


                    if ((row % 2 == 0 && col % 2 == 0) || (row % 2 != 0 && col % 2 != 0))
                    {
                        button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(167, 217, 72));
                        grid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(229, 194, 159));
                    }
                    else
                    {
                        button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(142, 204, 57));
                        grid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(215, 184, 153));
                    }
                }
            }
            addBombs();
            addNumbers();
            foreach (UIElement element in GRDGame.Children)
            {
                if (element is Border border && border.Child is Grid grid)
                {
                    foreach (UIElement gridElement in grid.Children)
                    {
                        if (gridElement is Label label)
                        {
                            int row = Grid.GetRow(border);
                            int col = Grid.GetColumn(border);
                            if (matrix[col, row] == -1)
                            {
                                label.Content = "B";
                                label.Foreground = Brushes.Red;

                            }
                            if (matrix[col, row] == 0)
                            {
                                label.Content = "";
                            }
                            if (matrix[col, row] > 0)
                            {
                                label.Content = matrix[col, row];
                                if (matrix[col, row] == 1)
                                {
                                    label.Foreground = Brushes.Blue;
                                }
                                if (matrix[col, row] == 2)
                                {
                                    label.Foreground = Brushes.Green;
                                }
                                if (matrix[col, row] == 3)
                                {
                                    label.Foreground = Brushes.Red;
                                }
                                if (matrix[col, row] == 4)
                                {
                                    label.Foreground = Brushes.OrangeRed;
                                }
                                if (matrix[col, row] == 5)
                                {
                                    label.Foreground = Brushes.DarkRed;
                                }
                            }
                        }
                    }
                }
            }
            
            foreach (UIElement element in GRDGame.Children)
            {
                if (element is Border border && border.Child is Grid grid)
                {
                    foreach (UIElement gridElement in grid.Children)
                    {
                        if (gridElement is Button button)
                        {
                            button.MouseRightButtonDown += Button_MouseRightButtonDown;
                        }
                    }
                }
            }

        }


        private void addBombs()
        {
            Random random = new Random();
            for (int i = 0; i < nbMine; i++)
            {
                int randomX = random.Next(gridSize);
                int randomY = random.Next(gridSize);
                while (!(isBombValid(randomX, randomY)))
                {
                    randomX = random.Next(gridSize);
                    randomY = random.Next(gridSize);
                }
                tabX[nbBombesPlaces] = randomX;
                tabY[nbBombesPlaces] = randomY;
                nbBombesPlaces++;
                matrix[randomX, randomY] = -1;
            }
        }


        private bool isBombValid(int xToInsert, int yToInsert)
        {
            for (int i = 0; i < nbBombesPlaces; i++)
            {
                if (xToInsert == tabX[i] && yToInsert == tabY[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void addNumbers()
        {
            for (int i = 0; i < gridSize; i++)
            {

                for (int j = 0; j < gridSize; j++)
                {
                    if (matrix[i, j] >= 0)
                    {
                        for (int k = Math.Max(0, i - 1); k <= Math.Min(i + 1, gridSize - 1); k++)
                        {
                            for (int m = Math.Max(0, j - 1); m <= Math.Min(j + 1, gridSize - 1); m++)
                            {
                                if (matrix[k, m] == -1)
                                {
                                    matrix[i, j]++;
                                }

                            }

                        }

                    }
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content == "F")
            {
                nbFlags++;
                LBLFlags.Content = "Flags : " + nbFlags;
            }
          
            Button button = (Button)sender;
            Border b = (Border)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(button));
            int col = Grid.GetColumn(b);
            int row = Grid.GetRow(b);
            verifieCellule(col, row);
        }

        private bool clickedOnBomb(int col, int row)
        {
            if (matrix[col, row] == -1)
            {
                return true;
            }
            return false;
        }

        private UIElement GetUIElementFromPosition(Grid g, int col, int row)
        {
            return g.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
        }

        private bool verifieCellule(int col, int row)
        {
            Border border = (Border)GetUIElementFromPosition(GRDGame, col, row);
            Grid g = (Grid)border.Child;
            Button button = (Button)g.Children[1];

            if (button.IsVisible)
            {
                ((Control)button).Visibility = Visibility.Collapsed;
                nbCellOpen++;
                if (clickedOnBomb(col, row))
                {
                    MessageBoxResult res = MessageBox.Show("Voulez vous recommencer?", "Perdu", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        initialisation();
                        return true;
                    }
                    else { Close(); }
                }
                else
                {
                    if (gridSize * gridSize - nbCellOpen == nbMine)
                    {
                        MessageBoxResult res = MessageBox.Show("Vous avez gagné au bout de " + nbTemps + " secondes \n Voulez vous recommencer", "Gagné", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                        {
                            initialisation();
                        }
                        else { Close(); }
                    }
                    else
                    {
                        if (matrix[col, row] == 0)
                        {
                            for (int i = Math.Max(0, col - 1); i <= Math.Min(col + 1, gridSize - 1); i++)
                            {
                                for (int j = Math.Max(0, row - 1); j <= Math.Min(row + 1, gridSize - 1); j++)
                                {
                                    bool resultat = verifieCellule(i, j);
                                    if (resultat)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void CBXLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initialisation();
        }

        private void CBXLevel_Loaded(object sender, RoutedEventArgs e)
        {
            CBXLevel.SelectedIndex = 1;
        }
    }
}
