using Pente.Classes;

namespace Pente
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            CreateButtonGrid(19, 19);
            game = new Game("1", "2");
        }

        private void CreateButtonGrid(int rows, int columns)
        {
            // Define rows and columns
            for (int i = 0; i < rows; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            for (int j = 0; j < columns; j++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Add buttons to the grid
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int currentRow = row;
                    int currentCol = col;

                    var button = new ImageButton
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Colors.Transparent,
                        WidthRequest = 75,
                        HeightRequest = 75,
                        Opacity = 1,
                        Margin = 1
                    };

                    button.Scale = 0.5;

                    // Add click event handler (optional)
                    button.Clicked += (s, e) =>
                    {
                        if (IsButtonEmpty(button))
                        {
                            game.PlaceStone(currentCol, currentRow);

                            UpdateBoard();
                            currentplayer = currentplayer == 0 ? 1 : 0;
                        }
                    };

                    // Place the button in the grid
                    GameGrid.Children.Add(button);

                    Grid.SetColumn(button, col);
                    Grid.SetRow(button, row);
                }
            }
        }

        private Game game;
        private int currentplayer = 0;

        //Check if button has an image

        private bool IsButtonEmpty(ImageButton button)
        {
            if (button.Source == null)
            {
                return true;
            }
            return false;
        }

        private void UpdateBoard()
        {
            for (int row = 0; row < 19; row++)
            {
                for (int col = 0; col < 19; col++)
                {
                    var button = (ImageButton)GameGrid.Children.Single(b => Grid.GetRow((BindableObject)b) == row && Grid.GetColumn((BindableObject)b) == col);
                    if (game.GameBoard.board[col, row] == true)
                    {
                        button.Source = "white.png";
                    }
                    else if (game.GameBoard.board[col, row] == false)
                    {
                        button.Source = "black.png";
                    }
                    else
                    {
                        button.Source = null;
                    }
                }
            }
        }
    }
}
