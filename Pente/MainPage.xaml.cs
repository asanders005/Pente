namespace Pente
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void CreateButtonGrid(int rows, int columns)
        {
            // Define rows and columns
            for (int i = 0; i < rows; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            for (int j = 0; j < columns; j++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Add buttons to the grid
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var button = new Button
                    {
                        Text = $"Button {row},{col}",
                        BackgroundColor = Colors.LightBlue,
                        Margin = 5
                    };

                    // Add click event handler (optional)
                    button.Clicked += (s, e) =>
                    {
                        DisplayAlert("Button Clicked", $"You clicked {((Button)s).Text}", "OK");
                    };

                    // Place the button in the grid
                    gameGrid.Children.Add(button, col, row);
                }
            }
        }

        Grid gameGrid;
    }

}
