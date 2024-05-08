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
        int tileUsed;
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

        public void GenerateCorrectMatrix(int size)
        {
            correctMatrix = new int[size, size];

            int correctMatrixValue = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    correctMatrix[i, j] = correctMatrixValue;
                    correctMatrixValue++;

                }
            }
            correctMatrix[size - 1, size - 1] = -1;

        }

        public void GenerateRandomMatrix(int size)
        {
            matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = correctMatrix[i, j];
                }
            } 
            Random r = new();
            int[] array = ConvertTo1dArray(matrix);
            r.Shuffle(array);
            ConvertTo2dArray(array);
        }

        public int[] ConvertTo1dArray(int[,] _matrix)
        {
            return _matrix.Cast<int>().Select(x => x).ToArray();
        }

        public void ConvertTo2dArray(int[] array)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = array[i * size + j];
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
                    else
                    {
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
            if (!IsOutofBorder(row - 1, column))
            {
                if (matrix[row - 1, column] == -1)
                {
                    matrix[row - 1, column] = matrix[row, column];
                    matrix[row, column] = -1;
                    TileUsed();
                }
            }
            if (!IsOutofBorder(row + 1, column))
            {
                if (matrix[row + 1, column] == -1)
                {
                    matrix[row + 1, column] = matrix[row, column];
                    matrix[row, column] = -1;
                    TileUsed();
                }
            }
            if (!IsOutofBorder(row, column - 1))
            {
                if (matrix[row, column - 1] == -1)
                {
                    matrix[row, column - 1] = matrix[row, column];
                    matrix[row, column] = -1;
                    TileUsed();
                }
            }
            if (!IsOutofBorder(row, column + 1))
            {
                if (matrix[row, column + 1] == -1)
                {
                    matrix[row, column + 1] = matrix[row, column];
                    matrix[row, column] = -1;
                    TileUsed();
                }
            }
            ShowMatrix();

            if (IsPuzzleCorrect())
            {
                MessageBoxResult message = MessageBox.Show($"You have completed the puzzle using {tileUsed} moves");
            }

        }
        private bool IsOutofBorder(int row, int column)
        {
            return row < 0 || column < 0 || row >= size || column >= size;
        }

        private bool IsPuzzleCorrect()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(matrix[i, j] != correctMatrix[i, j])
                        return false;
                }
            }
            return true;
        }

        public void TileUsed()
        {
            tileUsed++;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            size = Convert.ToInt32(comboBox.SelectedItem);
            GenerateCorrectMatrix(size);
            GenerateRandomMatrix(size);
            tileUsed = 0;
            ShowMatrix();
        }
    }
}


