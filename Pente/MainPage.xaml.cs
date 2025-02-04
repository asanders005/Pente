using Pente.Classes;
using System.Diagnostics;
using System.Timers;

namespace Pente
{
    public partial class MainPage : ContentPage
    {
        int timerCount = 0;

        IDispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();
            CreateButtonGrid(19, 19);

            
            whiteStoneCount = WhiteStoneCount;
            blackStoneCount = BlackStoneCount;
            currentPlayerName = CurrentPlayerName;
            notification = NotificationLabel;
            timerlabel = TimerLabel;

            notification.Text = "Pente Game";

        }

        public void OnPlay(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Player1NameEntry.Text))
            {
                Player1NameEntry.Text = "Player 1";
            }

            if (string.IsNullOrWhiteSpace(Player2NameEntry.Text))
            {
                Player2NameEntry.Text = "Player 2";
            }

            Player1NameEntry.IsEnabled = false;
            Player2NameEntry.IsEnabled = false;

            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                timerCount++;

                if (game.ResetTimer)
                {
                    timerCount = 0;
                    game.ResetTimer = false;
                }

                if (timerCount == 30)
                {
                    NotificationLabel.Text = "Your turn is about to be forfeit";
                }

                if (timerCount == 35)
                {
                    game.PassTurn();
                    currentPlayerName.Text = $"{game.Players[game.CurrentPlayer]}'s Turn";
                    NotificationLabel.Text = "";
                };

                timerlabel.Text = timerCount.ToString() + " secs";
            };
            timer.Start();
            game = new Game(Player1NameEntry.Text,Player2NameEntry.Text);
            currentplayer = game.CurrentPlayer;
            currentPlayerName.Text = $"{game.Players[currentplayer]}'s Turn";
            notification.Text = "";
            whiteStoneCount.Text = "x " + game.CapturedWhite.ToString();
            blackStoneCount.Text = "x " + game.CapturedBlack.ToString();
            UpdateBoard();
        }

        public void QuitGame(object sender, EventArgs e)
        {
            if(timer != null)
            {
                timer.Stop();
            }

            Application.Current.Quit();
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
                        WidthRequest = 80,
                        HeightRequest = 80,
                        Opacity = 1,
                        Margin = 1
                    };

                    button.Scale = 0.5;

                    // Add click event handler (optional)
                    button.Clicked += (s, e) =>
                    {
                        if (IsButtonEmpty(button) && game != null)
                        {
                            if (!game.GameOver)
                            {
                                game.PlaceStone(currentCol, currentRow);

                                UpdateBoard();


                                whiteStoneCount.Text = "x " + game.CapturedWhite.ToString();
                                blackStoneCount.Text = "x " + game.CapturedBlack.ToString();

                                switch (game.Notification)
                                {
                                    case NotificationType.NONE:
                                        NotificationLabel.Text = "";
                                        break;
                                    case NotificationType.TRIA:
                                        NotificationLabel.Text = "Tria";
                                        break;
                                    case NotificationType.TESSERA:
                                        NotificationLabel.Text = "Tessera";
                                        break;
                                    case NotificationType.WIN:
                                        NotificationLabel.Text = $"{game.Winner} Wins!";
                                        timerCount = 0;
                                        timer.Stop();
                                        Player1NameEntry.IsEnabled = true;
                                        Player2NameEntry.IsEnabled = true;
                                        break;
                                }

                                currentplayer = game.CurrentPlayer;

                                currentPlayerName.Text = $"{game.Players[currentplayer]}'s Turn";
                            }
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
        private int currentplayer;

        private Label whiteStoneCount;
        private Label blackStoneCount;
        private Label currentPlayerName;
        private Label notification;
        private Label timerlabel;

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
                    if (game.GameBoard.board[col, row] == false)
                    {
                        button.Source = "white.png";
                    }
                    else if (game.GameBoard.board[col, row] == true)
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
