using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PuzzleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] correctMatrix, matrix;
        int[] ints;
        int size;
        public MainWindow()
        {
            InitializeComponent();
            AddItemsToComboBox();
        }

        public void AddItemsToComboBox()
        {
            for (int i = 3; i < 5; i++)
            {
                comboBox.Items.Add(i);
            }

            comboBox.SelectedIndex = 0;
        }

        public void GenerateMatrixes()
        {
            size = Convert.ToInt32(comboBox.SelectedItem);
            correctMatrix = new int[size, size];
            matrix = new int[size, size];

            int correctMatrixValue = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    correctMatrix[i, j] = correctMatrixValue;
                    correctMatrixValue++;

                }
            }
            correctMatrix[size-1, size-1] = -1;
            matrix = correctMatrix;
            int[] result = matrix.Cast<int>().Select(x => x).ToArray();
            Random r = new();
            r.Shuffle(result);
            ConvertTo2dArray(result);
            ShowMatrix();
        }

        public void ConvertTo2dArray(int[] result)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = result[i * size + j];
                }
            }
        }

        public void ShowMatrix()
        {
            matrixGrid.Children.Clear();
            matrixGrid.RowDefinitions.Clear();
            matrixGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < size; i++)
            {
                matrixGrid.RowDefinitions.Add(new());
            }
            for (int j = 0; j < size; j++)
            {
                matrixGrid.ColumnDefinitions.Add(new());
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == -1)
                    {
                        continue;
                    }
                    else {
                        Button b = new();

                        Grid.SetRow(b, i);
                        Grid.SetColumn(b, j);
                        b.Content = matrix[i, j];
                        b.Click += PuzzlePieceClick;
                        matrixGrid.Children.Add(b);
                    }
                }
            }
        }

        private void PuzzlePieceClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int row = Grid.GetRow(b);
            int column = Grid.GetColumn(b);
            if (!IsOutofBorder(row - 1, column) )
            {
                if (matrix[row - 1, column] == -1)
                {
                    matrix[row - 1, column] = matrix[row, column];
                    matrix[row, column] = -1;
                }
            }
            if (!IsOutofBorder(row + 1, column))
            {
                if (matrix[row + 1, column] == -1)
                {
                    matrix[row + 1, column] = matrix[row, column];
                    matrix[row, column] = -1;
                }
            }
            if (!IsOutofBorder(row, column - 1))
            {
                if (matrix[row, column - 1] == -1)
                { 
                    matrix[row, column - 1] = matrix[row, column];
                    matrix[row, column] = -1;
                }
            }
            if (!IsOutofBorder(row, column + 1))
            {
                if (matrix[row, column + 1] == -1)
                {
                    matrix[row, column + 1] = matrix[row, column];
                    matrix[row, column] = -1;
                }
            }
            ShowMatrix();
            CheckIfPuzzleFinished();

        }
        private bool IsOutofBorder(int row, int column)
        {
            return row < 0 || column < 0 || row >= size || column >= size;
        }

        private void CheckIfPuzzleFinished()
        {
            //TODO
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            GenerateMatrixes();
        }
    }
}
