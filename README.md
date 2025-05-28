# UNOMEGAPROJECT

# Ez a project asztali alkalmazások fejlesztésére készült, ahol egy canvason belul egy UNO programot kellett létrehozzak.
## Maga a játék lényege:
- Van egy felhasználó aki egy robot ellen játszik.
- a felhasználó kártyái és a kozépen lévő kártyák vannak csak megmutatva
- A játékos kártyái egymás mellé vannak kiiratva
- körök vannak minden egyes felhasználó által tett kattintás után a robot azzonnal reagálni fog
- 2 különböző helyre tudsz kattintan:
      - vagy a paklidban lévő kártyákra, ami ami megjelenik a középen lévő kártyán. Amennyiben a robotnak is van letehető kártyája azt azonnal látható lesz.
      - vagy a kártyahúzás gombra, a kártyáid között lesz +1, a robot pedig azonnal reagál a lehető legjobb lépéssel.
- a felhasználónak 5 a robotnak pedig 10kártyája van kezdéskor, ez megnöveli a játékélményt mivel valószínűleg a felhasználó fog nyerni.
- tanárúr kérésére angolul lesz írva minden de itt az md fileba minden magyarul lesz magyarázva.

# A kód magyarázata:

## XAML

      <Canvas Name="unoCanvas" Background="LightGray"/>

## CS

### privát változók létrehozása
-kell egy pakli string lista
-felhasználó kártyáinak string listája
-robot kártyáinak string listája
-közeépső kártya
-mivel pakli keverésnél random keverés van ezért egy kell a random

      private List<string> deck = new List<string>();
      private List<string> userHand = new List<string>();
      private List<string> robotHand = new List<string>();
      private string topCard;
      private Random rnd = new Random();

### DeckInicialization()
- Ez a pakli létrehozására lett létrehozva
- létrehozzuk az összes létező kártyát majd megkeverjük őket.


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


### GameStart()
- Ezzel kezdődik a játék amit a fő futásban meghívunk.
- Ebben meghívjuk a pakli létrehozás függvényt
- a playernek és a robotnak kiosztjuk a kártyákat, majd a középső kártyat is.
- a vizuális dolgokat frissítsuk vagyis a UserCardShow() > a felhasználó kártyáinak frissítése
                                          TopCardShow() > a középső kártya frissítése


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


### Shuffle()
- a shuffle a pakli megkeverésére szolgál minden kör legelején

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

### UserCardShow()
-ebben a függvényben a felhasználó kártyáit frissítsuk a felhasználónak, ezt sokszok megkell hívni mivel amikor rákattintunk egy kártyára és az eltűnik akkor ezt frissíteni kell vagy ha húzunk egy új lapot.
- minden egyes meghivásnál kitörli az egészet és mivel a privát változót mindig frissítsuk lerakásnál és húzásnál, azt kiíratni a felhasználónak.
- egy click függvény a kártyáknak
- Canvas segítségével betudjuk állítani a pozícióját, for segítségével ahányadik kártyát rakja bele annyiszer tolja el jobbra így nem fog egymás fellett lenni 2 kártya.
- mivel itt az egész canvast kitörlöm, ezért a laphúzás gombot is újrakell csinálni (adni kell egy click függvényt neki es elkell helyezni canvason belul)

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

### TopCardShow()
-ugyanaz mint a usercard csak ez a középső kártyát frissíti
-ezt azert kell kül mert amikor a robot mozog ezt akkor is kell frissiteni

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


### UserCard_Click(object sender, RoutedEventArgs e)
- ez a click esemény fut le a felhasználó kártyára nyomásának
- belekell rakni egy külön változóba, amennyiben az a kártya lerakható rakja le, ha nem értesítse a felhasználható, ezzel együtt meglehet nézni hogy az az utolsó kártya volt e es ha lerakható akkor nyert a felhasznaló
- ha nem akkor a robot következik

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

### PickACard_Click(object sender, RoutedEventArgs e)
- hogyha a pakli még nem üres akkor húzhatunk egy kártyát ami hozzaadodik a felhasznaló kártyaihoz, és mivel az már egy körnek számít, a robot jön.

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

### robotMove()
-alapbol a robot nem rakhat le kártyát mert előtte megkell nézni hogy a kártyáiból van e a feltételeknek megfelelő kártya
-ha rakhat akkor rakjon és menjen ki a függvényből
-amennyiben nincsen olyan kártyája de a pakliban van még kártya akkor huzzon egyet, frissítse a top kártyát ha véletlen rakott volna
-megkell nezni ha lerakás után nincs e több kártyája a robotnak ha nincs akkor o nyert

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

### cardCanBePlaced(string card)
-ezt a függvényt csak ugy lehet elérni ha megadunk neki egy kártyát amit letud ellenőrizni
-szétszedi a stringet, megnézi hogy a szin vagy a szám matchel- e ha igen akkor true-val tér vissza
-ha nem akkor false-al

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

### Winner()
- a winner függvény segítségével elengedhetjuk a messagebox.show -ot minden egyes kiírásnál és egy külön függvénybe adjuk meg a kódot.
- ezáltal szebb lesz a program egésze

## Az egész program

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
