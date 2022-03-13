using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeBiteLadderGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle landingRec; // help in identifying the rectangle on the box
        Rectangle player; // player rectangle

        Rectangle opponent; // opponent rectangle

        List<Rectangle> Moves = new List<Rectangle>(); // stores the pieces of rectangle on board

        DispatcherTimer gametimer = new DispatcherTimer(); 

        ImageBrush playerimage = new ImageBrush(); //to import the player image and attach it to player rectangle
        ImageBrush opponentimage = new ImageBrush();// to import the opponent image and attach it to opponenent rectangle

        // i and j will help us to know the position of player and opponenent on board
        int i = -1; // at start both player and opponent is at same pos
        int j = -1;

        //position and currentposition of player
        int position;
        int currentposition;
        //position and currentposition of opponent
        int opponentposition;
        int opponentcurrentposition;

        int images = -1;//used to show the integer images when we created them

        Random rand = new Random();

        bool playeroneround, playertworound;//determine the turn weather it is player and opponent

        int temppos;// show the current position of player and opponent on board
        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
        }
        
        /// <summary>
        /// Event that will trigger when game start
        /// click event is linked to the canvas ,so player is able to click anywhere 
        /// on the canvas to play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickEvent(object sender, MouseButtonEventArgs e)
        {
            //checking player 1 and 2 boolean values set to false first
            if(playeroneround == false && playertworound == false)
            {
                position = rand.Next(1, 7);//genrate random num for player 
                txtplayer.Content = "You rolled a "+ position;// show it to player 
                currentposition = 0;

                // in this if statement we are checking that i integer which is current position of 
                //player is less than or equal to 99
                if((i+position) <= 99)
                {
                    playeroneround = true;
                    gametimer.Start();
                }
                else
                {
                    if(playertworound == false)
                    {
                        playertworound = true;
                        opponentposition = rand.Next(1, 7);
                        txtopponent.Content = "opponennt rolled a " + opponentposition;
                        opponentcurrentposition = 0;
                        gametimer.Start();
                    }
                    else
                    {
                        gametimer.Stop();
                        playeroneround = false;
                        playertworound = false;
                    }
                }

            }

        }
        /// <summary>
        /// initialise the game board and set up player and opponent on board
        /// </summary>
        private void SetUpGame()
        {

            int leftpos = 10; // left pos will help us position the boxes from right to left
            int toppos = 600;// top pos will help us position the boxes from bottom to top
            // a integer will help us to lay 10 boxes in a row
            int a = 0;
            //importing the player and opponent images and attaching them to the image brushes
            playerimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/player.gif"));
            opponentimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/opponent.gif"));

            //main loop that will create board 
            for(int i =0;i<100;i++)
            {
                images++;//increment the images
                ImageBrush titleimages = new ImageBrush();

                // import the board images inside the tile images
                //we are able to do it because images have name from 0.jpg to 99.jpg
                titleimages.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/" + images + ".jpg"));

                // create a rectangle box that will hold images each time
                Rectangle box = new Rectangle
                {
                    Height = 60,
                    Width = 60,
                    Fill = titleimages,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                //provide unique name to each box
                box.Name = "box" + i.ToString();
                //register the name inside of wpf app
                this.RegisterName(box.Name, box);

                // add the newly created box to moves list
                Moves.Add(box);

                // below we are making the algorithm we need to lay the boxes 10 in a row
                // we will make the boxes from left to right then move up and reverse that process
                // remember "a" integer is controlling how we position the boxes down so we need to keep in mind on it can be controlled inside of this loop

                // it will happen after we have positioned 10 boxes at their place
                if (a == 10)
                {
                    toppos -= 60;//reduce 60 from the top pos as we have a filled a row now
                    a = 30;// change the value of a to 30 so that boxes can move now from right to left
                }
                if(a == 20)
                {
                    toppos -= 60;
                    a = 0;
                }
                if(a > 20)
                {
                    //// this if statement will help us position the boxes from right to left

                    a--;
                    Canvas.SetLeft(box, leftpos);
                    // set the box inside the canvas by the value of the left pos integer
                    leftpos -= 60;
                }
                if(a < 10)
                {
                     // this will happen when we want to position the boxes from left to right

                    a++;
                    Canvas.SetLeft(box, leftpos);
                    leftpos += 60;
                    Canvas.SetLeft(box, leftpos);
                }

                // set the box top pos to the value of top pos integer each loop
                Canvas.SetTop(box, toppos);
                //add the box to canvas display
                Mycanvas.Children.Add(box);
            }

            gametimer.Tick += GameTimerEvent;
            gametimer.Interval = TimeSpan.FromSeconds(.2);

            // set up rectangle for player 
            player = new Rectangle
            {
                Height = 30,
                Width = 30,
                Fill = playerimage,
                StrokeThickness = 2
            };
            // set up rectangle for opponent
            opponent = new Rectangle
            {
                Height=30,
                Width=30,
                Fill=opponentimage,
                StrokeThickness = 2
            };

            // add both player and opponent to the canvas
            Mycanvas.Children.Add(player);
            Mycanvas.Children.Add(opponent);

            // run the move piece function and reference the player and opponent inside of it
            // also reference where we want the player and the opponent to be positioned beginning of the game
            MovePlace(player, "box" + 0);
            MovePlace(opponent, "box" + 0);

        }
        /// <summary>
        /// this is the game timer event this event will move the player and the opponent on the board

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameTimerEvent(object sender, EventArgs e)
        {
            //check if it is player turn
            if(playeroneround == true && playertworound == false)
            {
                //// check if i is less than the total number of board pieces inside of the moves list
                ///  i has current position of the player
                if (i<Moves.Count)
                {
                    if(currentposition < position)
                    {
                        currentposition++;
                        i++;
                        MovePlace(player, "box" + i);// update the player position 
                    }
                    // if the player one round is set to false then do the following

                    else
                    {
                        // set up the opponent round
                        playertworound = true;
                        // now run the i which is the player position through the check snakes and ladders function

                        i = CheckSnakeOrLadder(i);
                        //update the position of the player
                        MovePlace(player, "box" + i);

                        opponentposition = rand.Next(1, 7);
                        txtopponent.Content = "opponent Rolled a " + opponentposition;
                        opponentcurrentposition = 0;
                        temppos = i;
                        txtplayerposition.Content = "player is @" + (temppos + 1);
                    }
                }
                if(i == 99)
                {
                    //this if statement will check weather the player has made it to top or not
                    gametimer.Stop();
                    MessageBox.Show("game over ! you win " + Environment.NewLine + "click ok to play again");
                    RestartGame();
                }
            }
            // this section below is for the CPU, this will only run when the player two round is set to true

            if (playertworound == true)
            {
                // checking cpu position is less than board numbers
                if(j<Moves.Count)
                {
                    // if yes we are checking if the current position of the opponent is less than the generated position
                    // and we are checking is the CPU has more moves ahead of it, this way we can stop the cpu from making last minutes moves and allow the player to move after its turn

                    if (opponentcurrentposition < opponentposition && (j + opponentposition < 101))
                    {
                        opponentcurrentposition++;
                        j++;
                        MovePlace(opponent, "box" + j);
                    }
                    else
                    {
                        playeroneround = false;
                        playertworound = false;
                        j = CheckSnakeOrLadder(j);
                        MovePlace(opponent, "box" + j);
                        temppos = j;
                        txtopponentposition.Content = "opponenet is @ " + (temppos + 1);
                        gametimer.Stop();
                    }
                }
                if(j == 99 )
                {
                    // check if cpu on top of the board
                    gametimer.Stop();
                    MessageBox.Show("game over ! opponent win" + Environment.NewLine + "click ok to start again");
                    RestartGame();
                }
            }
        }

        /// <summary>
        /// this function will set everything back to default
        /// </summary>
        private void RestartGame()
        {
            // if I and J back to -1 and set the player and opponent the 0 position on the board

            i = -1;
            j = -1;
            MovePlace(player, "box" + 0);
            MovePlace(opponent, "box" + 0);

            // set player position and current position to 0

            position = 0;
            currentposition = 0;
            // set opponent position and current position to 0

            opponentposition = 0;
            opponentcurrentposition = 0;
            // set player one and player two rounds to false

            playeroneround = false;
            playertworound = false;
            // set the player and opponent labels back to their default content

            txtplayer.Content = "You rolled a" + position;
            txtplayerposition.Content = "Player is @ 1";

            txtopponent.Content = "opponenet Rolled a" + opponentposition;
            txtopponentposition.Content = "opponent is @ 1";

            gametimer.Stop();

        }

        /// <summary>
        // this is the check snakes or ladders function. The purpose of this function is to check if thep player has
        // landed on the bottom of a ladder or top of the snake

        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int CheckSnakeOrLadder(int num)
        {
            if(num == 1)
            {
                num = 37;
            }

            if(num == 6)
            {
                num = 13;
            }
            if(num == 7)
            {
                num = 30;
            }
            if(num == 14)
            {
                num = 25;
            }
            if(num == 15)
            {
                num = 5;
            }
            if(num == 20)
            {
                num = 41;
            }
            if(num == 27)
            {
                num = 83;
            }

            if(num == 35)
            {
                num = 43;
            }
            if(num == 45)
            {
                num = 24;
            }

            if(num == 48)
            {
                num = 10;
            }

            if(num == 50)
            {
                num = 66;
            }

            if(num == 61)
            {
                num = 18;
            }

            if(num == 63)
            {
                num = 59;
            }
            if(num == 70)
            {
                num = 90;
            }
            if(num == 73)
            {
                num= 52;
            }
            if(num == 77)
            {
                num = 97;
            }

            if(num == 86)
            {
                num = 93;
            }
            if(num == 88)
            {
                num = 67;
            }
            if(num == 91)
            {
                num = 87;
            }
            if(num == 94)
            {
                num = 74;
            }
            if(num == 98)
            {
                num = 79;
            }
            return num;
        }
        // this function will move the player and the opponent across the board
        // the way it does it is very simply, we have added of the board rectangles to the moves list 
        // from the for each loop below we can loop through all of the rectangles from that list
        // we are also checking if any of the rectangle has the posName, if they do then we will link the landing rect to that rectangle found inside of the for each loop
        // this way we can move the rectangle that is being passed inside of this function and run in the timer event to animate it when it starts

        private void MovePlace(Rectangle player,string posName)
        {
            foreach(Rectangle rectangle in Moves)
            {
                if(rectangle.Name == posName)
                {
                    landingRec = rectangle;
                }
            }
            // the two lines here will place the "player" object that is being passed in this function to the landingRec location

            Canvas.SetLeft(player, Canvas.GetLeft(landingRec) + player.Width / 2);
            Canvas.SetTop(player, Canvas.GetTop(landingRec) + player.Height / 2);
        }
    }
}
