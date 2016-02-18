using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SudokuSolver.GUI
{
    public class SudokuGrid : Grid
    {
        TextBox[] inputs = new TextBox[81];
        bool[] inputIsUserSpecified = new bool[81];

        readonly SolidColorBrush errorColor = new SolidColorBrush(Colors.Red);

        public SudokuGrid() : base()
        {
            for (int row = 0; row < 9; row++)
            {
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int col = 0; col < 9; col++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    const double outerBorderWidth = 5;
                    const double boxBorderWidth = 2;
                    const double defaultBorderWidth = 1;

                    Thickness borderThickness = new Thickness(defaultBorderWidth);

                    switch(row)
                    {
                        case 0:
                            borderThickness.Top = outerBorderWidth;
                            break;
                        case 3:
                        case 6:
                            borderThickness.Top = boxBorderWidth;
                            break;
                        case 8:
                            borderThickness.Bottom = outerBorderWidth;
                            break;
                        case 2:
                        case 5:
                            borderThickness.Bottom = boxBorderWidth;
                            break;
                    }
                    switch (col)
                    {
                        case 0:
                            borderThickness.Left = outerBorderWidth;
                            break;
                        case 3:
                        case 6:
                            borderThickness.Left = boxBorderWidth;
                            break;
                        case 8:
                            borderThickness.Right = outerBorderWidth;
                            break;
                        case 2:
                        case 5:
                            borderThickness.Right = boxBorderWidth;
                            break;
                    }

                    var border = new Border {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = borderThickness
                    };

                    Grid.SetColumn(border, col);
                    Grid.SetRow(border, row);
                    this.Children.Add(border);

                    var box = new TextBox();
                    box.HorizontalAlignment = HorizontalAlignment.Stretch;
                    box.VerticalAlignment = VerticalAlignment.Stretch;

                    box.Margin = new Thickness(6);

                    Grid.SetColumn(box, col);
                    Grid.SetRow(box, row);

                    this.Children.Add(box);

                    inputs[9 * row + col] = box;
                }
            }
        }

        public bool ValidateInput()
        {
            bool result = true;
            foreach(var input in inputs)
            {
                int junk;
                if(!(string.IsNullOrWhiteSpace(input.Text) || int.TryParse(input.Text, out junk)))
                {
                    input.BorderBrush = errorColor;
                    result = false;
                } else
                {
                    input.ClearValue(TextBox.BorderBrushProperty);
                }
            }

            return result;
        }

        public int[] ToIntArray()
        {
            int[] result = new int[81];
            for(int i=0;i<81;i++)
            {
                var input = inputs[i];

                int val;
                if (int.TryParse(input.Text, out val))
                {
                    result[i] = val;
                    inputIsUserSpecified[i] = true;
                }
                else
                {
                    result[i] = 0;
                    inputIsUserSpecified[i] = false;
                }
            }

            return result;
        }

        public void LoadIntArray(int[] puzzle)
        {
            for(int i=0;i<81;i++)
            {
                int val = puzzle[i];
                inputs[i].Text = (val == 0 ? "" : val.ToString());
            }
        }
    }
}
