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
        Rectangle landingRec; // varible that will take care of landing of players after their turn.
        Rectangle player; // take care for player shape in windows

        Rectangle opponent; // take care for opponent shape in window

        List<Rectangle> Moves = new List<Rectangle>(); // take care of all the moves that are occuring during the play

        DispatcherTimer gametimer = new DispatcherTimer(); 

        ImageBrush playerimage = new ImageBrush();
        ImageBrush opponentimage = new ImageBrush();

        int i = -1; // at start both player and opponent is at same pos
        int j = -1;

        int position;
        int currentposition;

        int opponentposition;
        int opponentcurrentposition;

        int images = -1;

        Random rand = new Random();

        bool playeroneround, playertworound;

        int temppos;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Event that will trigger when game start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickEvent(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// initialise the game
        /// </summary>
        private void SetUpGame()
        {

        }

        private void RestartGame()
        {

        }

        private int CheckSnakeOrLadder(int num)
        {
            return num;
        }

        private void MovePlace(Rectangle player,string posName)
        {

        }
    }
}
