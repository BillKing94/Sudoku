using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SudokuSolver.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int[] originalPuzzle = null;

        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
            this.btn_Run.Click += Btn_Run_Click;
            this.btn_Clear.Click += Btn_Clear_Click;
        }

        private void Btn_Run_Click(object sender, RoutedEventArgs e)
        {
            if(this.SudokuGrid.ValidateInput())
            {
                originalPuzzle = this.SudokuGrid.ToIntArray();
                int[] solved = SudokuSolver.Solve(originalPuzzle);
                this.SudokuGrid.LoadIntArray(solved);
            }
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            if(originalPuzzle != null)
            {
                this.SudokuGrid.LoadIntArray(originalPuzzle);
            }
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            const int padding = 80;
            var minDimension = Math.Min(e.NewSize.Height, e.NewSize.Width);

            double sideLength = SudokuGrid.Width = SudokuGrid.Height = minDimension - padding;
        }
    }
}
