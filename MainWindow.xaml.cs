using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_1_UNO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        private List<string> deck = new List<string>();
        private List<string> userHand = new List<string>();
        private List<string> robotHand = new List<string>();
        private string topCard;
        private Random rnd = new Random();


        public MainWindow()
        {
            InitializeComponent();
            GameStart();
        }

        private void DeckInit()
        {
            string[] colors = { "Piros", "Kék", "Zöld", "Sárga" };
            string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            foreach (string color in colors)
            {
                foreach (string number in numbers)
                {
                    deck.Add($"{color} {number}");
                    //System.Diagnostics.Debug.Write($"{color} {number};");
                }
            }

            //deck = deck.OrderBy(x => rnd.Next()).ToList();
            Shuffle(deck);
            //foreach (string item in deck)
            //{
            //    System.Diagnostics.Debug.Write($"{item.ToString()}; ");
            //}
        }

        public void Shuffle(List<string> ts)
        {
            int count = ts.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int csereIndex = rnd.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[csereIndex];
                ts[csereIndex] = tmp;
            }
        }

        private void GameStart()
        {
            DeckInit();
            //a játékosok kártyái
            //a robot 2x annyi kártyával játszik.

            for (int i = 0; i < 5; i++)
            {
                robotHand.Add(CardUp());
                robotHand.Add(CardUp());
                userHand.Add(CardUp());
            }
            //elso top kartya kivalasztasa
            topCard = CardUp();

            //foreach (string hand in robotHand)
            //{
            //    System.Diagnostics.Debug.Write(hand);
            //}

            UserCardShow();
            TopCardShow();
        }

        private string CardUp()
        {
            string tmp = deck[0].ToString();
            deck.RemoveAt(0);
            return tmp;
        }

        private void UserCardShow()
        {
            unoCanvas.Children.Clear();
            for (int i = 0; i < userHand.Count; i++)
            {
                Button btn = new Button
                {
                    Content = userHand[i],
                    Width = 50,
                    Height = 70
                };
                Canvas.SetLeft(btn, 100 + i * 60);
                Canvas.SetTop(btn, 100);
                btn.Click += UserCard_Click;
                unoCanvas.Children.Add(btn);
            }

            //kartyahuzas
            Button pickCardButton = new Button
            {
                Content = "Húzás",
                Width = 50,
                Height = 70
            };
            Canvas.SetLeft(pickCardButton, 160);
            Canvas.SetTop(pickCardButton, 200);
            pickCardButton.Click += PickCardButton_Click;
            unoCanvas.Children.Add(pickCardButton);
        }

        private void TopCardShow()
        {
            Button topBtn = new Button
            {
                Content = topCard,
                Width = 50,
                Height = 70,
                IsEnabled = false
            };
            Canvas.SetLeft(topBtn, 100);
            Canvas.SetTop(topBtn, 200);
            unoCanvas.Children.Add(topBtn);
        }

        private void UserCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string chosenUserCard = button.Content.ToString();

                if (cardCanBePlaced(chosenUserCard))
                {
                    userHand.Remove(chosenUserCard);
                    topCard = chosenUserCard;

                    UserCardShow();
                    TopCardShow();

                    if (userHand.Count == 0)
                    {
                        Winner("Gratulalok nyertel");
                    }
                    else
                    {
                        RobotPut();
                    }
                }
                else
                {
                    MessageBox.Show("nem rakhatod le ezt a kártyát");
                }
            }
        }

        private void PickCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (deck.Count > 0)
            {
                userHand.Add(CardUp());
                UserCardShow();
                TopCardShow();
                RobotPut();
            }
            else
            {
                Winner("Elfogyott a pakli, döntetlen.");
            }
        }

        private void RobotPut()
        {
            bool canPlace = false;
            for (int i = 0; i < robotHand.Count; i++)
            {
                if (cardCanBePlaced(robotHand[i]))
                {
                    topCard = robotHand[i];
                    robotHand.RemoveAt(i);
                    canPlace = true;
                    break;
                }
            }

            foreach (string hand in robotHand)
            {
                System.Diagnostics.Debug.Write(hand + "\n");
            }

            if (!canPlace && deck.Count > 0)
            {
                robotHand.Add(CardUp());
            }
            TopCardShow();

            if (robotHand.Count == 0)
            {
                Winner("A robot nyert");
            }

        }

        private bool cardCanBePlaced(string card)
        {
            string[] topCardParts = topCard.Split(' ');
            string[] cardParts = card.Split(' ');

            return topCardParts[0] == cardParts[0] || topCardParts[1] == cardParts[1];
        }

        private void Winner(string message)
        {
            MessageBox.Show(message);
            unoCanvas.Children.Clear();
        }
    }
}