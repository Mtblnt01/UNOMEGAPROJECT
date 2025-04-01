# UNOMEGAPROJECT

# Ez a project asztali alkalmazások fejlesztésére készült, ahol egy canvason belul egy UNO programot kellett létrehozzak.
## Maga a játék lényege:
- Van egy felhasználó aki egy robot ellen játszik.
- a felhasználó kártyái és a kozépen lévő kártyák vannak csak megmutatva
- A játékos kártyái egymás mellé vannak kiiratva
- körök vannak minden egyes felhasználó által tett kattintás után a robot azzonnal reagálni fog
- 2 különböző helyre tudsz kattintan:
      - vagy a paklidban lévő kártyákra, ami ami megjelenik a középen lévő kártyán. Amennyiben a robotnak is van letehető kártyája azt azonnal látható lesz.
      - vagy a kártyhúzás gombra, a kártyáid között lesz +1, a robot pedig azonnal reagál a lehető legjobb lépéssel.

- a felhasználónak 5 a robotnak pedig 10kártyája van kezdéskor, ez megnöveli a játékélményt mivel valószínűleg a felhasználó fog nyerni.
- tanárúr kérésére angolul lesz írva minden de itt az md fileba minden magyarul lesz magyarázva.

## A kód magyarázata:
### privát változók létrehozása
-kell egy pakli string lista
-felhasznalo kartyainak string listaja
-robot kartyainak string listája
-kozepso kartya
-mivel pakli keverésnél random keverés van ezért egy kell a random

private List<string> deck = new List<string>();
private List<string> userHand = new List<string>();
private List<string> robotHand = new List<string>();
private string topCard;
private Random rnd = new Random();

### DeckInicialization()
- Ez a pakli létrehozására lett létrehozva
- létrehozzuk az osszes létező kártyát majd megkeverjük őket.


        private void DeckInicialization()
        {
            string[] colors = { "Red", "Blue", "Green", "Yellow" };
            string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };

            foreach (string color in colors)
            {
                foreach (string number in numbers)
                {
                    deck.Add($"{color} {number}");
                }
            }

            deck = deck.OrderBy(x => rnd.Next()).ToList();
        }


### GameStart()
- Ezzel kezdődik a játék amit a fő futásban meghívunk.
- ebben meghuvjuk a pakli letrehozas fuggvenyt
- a playernek és a robotnak kiosztjuk a kártyákat, majd a középső kartyat is.
- a vizuális dolgokat frissítsuk vagyis a UserCardShow() > a felhasznalo kartyainak frissitese
                                          TopCardShow() > a kozepso kartya frissitise


        private void GameStart()
        {
            DeckInicialization();

            //5card for every player (robot and user)
            for (int i = 0; i < 5; i++)
            {
                userHand.Add(deck[0]);
                deck.RemoveAt(0);
            }

            for (int i = 0; i < 10; i++)
            {
                robotHand.Add(deck[0]);
                deck.RemoveAt(0);
            }

            //choosing the top card
            topCard = deck[0];
            deck.RemoveAt(0);

            //user interface update
            UserCardShow();
            TopCardShow();
        }

### UserCardShow()
-ebben a fuggvényben a felhasználó kártyáit frissítsuk a felhasználónak, ezt sokszok megkell hívni mivel amikor rákattintunk egy kártyára és az eltunik akkor ezt frissíteni kell vagy ha húzunk egy új lapot.
- minden egyes meghivásnál kitörli az egészet és mivel a privát változót mindig frissítsuk lerakásnál és húzásnál, azt kiíratni a felhasználónak.
- egy click fuggveny a kartyaknak
- Canvas segítségével betudjuk állítani a poziciojat, for segítségével ahányadik kártyát rakja bele annyiszer tolja el jobbra így nem fog egymás fellett lenni 2 kartya.


- mivel itt az egész canvast kitörlöm, ezért a laphuzas gombot is újrakell csinálni (adni kell egy click fuggvenyt neki es elkell helyezni canvason belul)

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
                        MessageBox.Show("Congratulations! YOU WON!");
                    }
                    else
                    {
                        robotMove();
                    }
                }
                else
                {
                    MessageBox.Show("You cant place THAT card");
                }
            }
        }

### TopCardShow()
-ugyanaz mint a usercard csak ez a kozepso kartyat frissiti
-ezt azert kell kulon mert amikor a robot mozog ezt akkor is kell frissiteni

        private void TopCardShow()
        {
            Button topButton = new Button
            {
                Content = topCard,
                Width = 50,
                Height = 70
            };
            Canvas.SetLeft(topButton, 100);
            Canvas.SetTop(topButton, 200);
            myCanvas.Children.Add(topButton);
        }


### UserCard_Click(object sender, RoutedEventArgs e)
- ez a click esemenye a felhasznalo kartyainak nyomasanak
- belekell rakni egy kulon valtozoba, amennyiben az a kartya lerakhato rakja le, ha nem értesítse a felhasznalot, ezel egyutt meglehet nezni hogy az az utolso kartya volt e es ha lerakhato akkor nyert a felhasznalo
- ha nem akkor a robot kovetkezik

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
                        MessageBox.Show("Congratulations! YOU WON!");
                    }
                    else
                    {
                        robotMove();
                    }
                }
                else
                {
                    MessageBox.Show("You cant place THAT card");
                }
            }
        }

### PickACard_Click(object sender, RoutedEventArgs e)
- hogyha a pakli még nem üres akkor húzhatunk egy kartyat ami hozzaadodik a felhasznalo kartyaihoz, és mivel az már egy körnek számít, a robot jön.

        private void PickACard_Click(object sender, RoutedEventArgs e)
        {
            if (deck.Count > 0)
            {
                string newCard = deck[0];
                deck.RemoveAt(0);
                userHand.Add(newCard);
                UserCardShow();
                robotMove();
            }
            else
            {
                MessageBox.Show("No more card in the deck!!!");
            }
        }

### robotMove()
-alapbol a robot nem rakhat le kartyat mert elotte megkell nezni hogy a kartyaibol van e a felteteleknek megfelelo kartya
-ha rakhat akkor rakjon és menjen ki a függvenybol
-amennyiben nincsen olyan kartyaja de a pakliban van még kartya akkor huzzon egyet, frissitse a top kartyat ha veletlen rakott volna
-megkell nezni ha lerakas utan nincs e tobb kartyaja a robotnak ha nincs akkor o nyert

        private void robotMove()
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

            if (canPlace == false && deck.Count > 0)
            {
                string newCard = deck[0];
                deck.RemoveAt(0);
                robotHand.Add(newCard);
            }
            TopCardShow();

            if (robotHand.Count == 0)
            {
                MessageBox.Show("The Robot WON!!");
            }
        }

###cardCanBePlaced(string card)
-ezt a fuggvenyt csak ugy lehet elerni ha megadunk neki egy kartyat amit letud ellenorizni
-szétszedi a stringet, megnézi hogy a szin vagy a szám matchel- e ha igen akkor true-val tér vissza
-ha nem akkor false-al


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


namespace UNO
{

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

        private void DeckInicialization()
        {
            string[] colors = { "Red", "Blue", "Green", "Yellow" };
            string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };

            foreach (string color in colors)
            {
                foreach (string number in numbers)
                {
                    deck.Add($"{color} {number}");
                }
            }

            deck = deck.OrderBy(x => rnd.Next()).ToList();
        }

        private void GameStart()
        {
            DeckInicialization();

            //5card for every player (robot and user)
            for (int i = 0; i < 5; i++)
            {
                userHand.Add(deck[0]);
                deck.RemoveAt(0);
            }

            for (int i = 0; i < 10; i++)
            {
                robotHand.Add(deck[0]);
                deck.RemoveAt(0);
            }

            //choosing the top card
            topCard = deck[0];
            deck.RemoveAt(0);

            //user interface update
            UserCardShow();
            TopCardShow();
        }

        private void UserCardShow()
        {
            myCanvas.Children.Clear();

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
                myCanvas.Children.Add(btn);
            }

            //HUZZUNK KARTYAT
            Button pickCardButton = new Button
            {
                Content = "Húzzunk egy kártyát",
                Width = 150,
                Height = 30
            };
            Canvas.SetLeft(pickCardButton, 100);
            Canvas.SetTop(pickCardButton, 300);
            pickCardButton.Click += PickACard_Click;
            myCanvas.Children.Add(pickCardButton);
        }

        private void TopCardShow()
        {
            Button topButton = new Button
            {
                Content = topCard,
                Width = 50,
                Height = 70
            };
            Canvas.SetLeft(topButton, 100);
            Canvas.SetTop(topButton, 200);
            myCanvas.Children.Add(topButton);
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
                        MessageBox.Show("Congratulations! YOU WON!");
                    }
                    else
                    {
                        robotMove();
                    }
                }
                else
                {
                    MessageBox.Show("You cant place THAT card");
                }
            }
        }

        private void PickACard_Click(object sender, RoutedEventArgs e)
        {
            if (deck.Count > 0)
            {
                string newCard = deck[0];
                deck.RemoveAt(0);
                userHand.Add(newCard);
                UserCardShow();
                robotMove();
            }
            else
            {
                MessageBox.Show("No more card in the deck!!!");
            }
        }

        private void robotMove()
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

            if (canPlace == false && deck.Count > 0)
            {
                string newCard = deck[0];
                deck.RemoveAt(0);
                robotHand.Add(newCard);
            }
            TopCardShow();

            if (robotHand.Count == 0)
            {
                MessageBox.Show("The Robot WON!!");
            }
        }


        private bool cardCanBePlaced(string card)
        {
            string[] topCardParts = topCard.Split(' ');
            string topCardColor = topCardParts[0];
            string topCardNumber = topCardParts[1];

            string[] cardParts = card.Split(' ');
            string cardColor = cardParts[0];
            string cardNumber = cardParts[1];

            return cardColor == topCardColor || cardNumber == topCardNumber;
        }


    }
}
