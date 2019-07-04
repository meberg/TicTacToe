using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        public static string player1;
        public static string player2;
        public int player1Score = 0;
        public int player2Score = 0;
        public bool player1Starts = true;

        public Form1()
        {
            InitializeComponent();
            SetStartingPlayer();

        }

        private void DoButtonClick(Button button)
        {
            LockNames(true);

            if (player1TBox.BackColor == Color.GreenYellow)
            {
                button.Text = "X";
                player1TBox.BackColor = Color.LightGray;
                player2TBox.BackColor = Color.GreenYellow;
            }
            else
            {
                button.Text = "O";
                player2TBox.BackColor = Color.LightGray;
                player1TBox.BackColor = Color.GreenYellow;
            }

            if (CheckIfThreeInRow(button))
            {
                string winner;
                if (player1TBox.BackColor == Color.LightGray)
                {
                    player1TBox.BackColor = Color.Yellow;
                    player2TBox.BackColor = Color.LightGray;
                    winner = player1TBox.Text;
                    player1Score++;
                    p1Score.Text = player1Score.ToString();
                }
                else
                {
                    player1TBox.BackColor = Color.LightGray;
                    player2TBox.BackColor = Color.Yellow;
                    winner = player2TBox.Text;
                    player2Score++;
                    p2Score.Text = player2Score.ToString();
                }

                string winText = $"{winner} is the winner!\n\n\nVill du fortsätta spela?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                string caption = "TicTacToe";

                var winnerWindow = MessageBox.Show(winText, caption, buttons,
                    MessageBoxIcon.None);

                if (winnerWindow == DialogResult.Yes)
                {
                    RestartGame();
                }
                else
                {
                    QuitGame();
                }

            }
            else
            {
                if (CheckIfBoardIsFull(button))
                {
                    string gameOverText = $"Spelet blev oavgjort!\n\n\nVill ni fortsätta spela?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    string caption = "TicTacToe";

                    var winnerWindow = MessageBox.Show(gameOverText, caption, buttons,
                        MessageBoxIcon.None);

                    if (winnerWindow == DialogResult.Yes)
                    {
                        RestartGame();
                    }
                    else
                    {
                        QuitGame();
                    }
                }
            }
        }

        private void SetStartingPlayer()
        {
            if (player1Starts)
            {
                player1TBox.BackColor = Color.GreenYellow;
                player2TBox.BackColor = Color.LightGray;
            }
            else
            {
                player1TBox.BackColor = Color.LightGray;
                player2TBox.BackColor = Color.GreenYellow;
            }
        }

        private void QuitGame()
        {
            this.Close();
        }



        // Check if board is full (all 9 buttons pressed)
        private bool CheckIfBoardIsFull(Button button)
        {
            List<Button> buttonList = new List<Button> { button1, button2, button3, button4, button5
            , button6, button7, button8, button9};
            bool done = true;

            foreach (var btn in buttonList)
            {
                if (btn.Text == "")
                {
                    done = false;
                }
            }
            return done;
        }

        // Check every direction from button to see if there are three in a row
        private bool CheckIfThreeInRow(Button button)
        {
            //                                            00     01       02          10        11       12          20       21        22
            Button[,] buttonArray = new Button[,] { { button1, button2, button3 }, { button4, button5, button6 }, { button7, button8, button9 } };

            //                                      "up",   "down",   "right"   "left",  "upright", "downleft", "downright", "upleft" 
            int[,] arrayDirection = new int[,] { { -1, 0 }, { 1, 0 }, { 0, 1 }, { 0, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 }, { -1, -1 } };
            //                                      00 01    10 11     20 21     30  31      40 41     50  51     60  61     70  71    

            Tuple<int, int> buttonCoordinates = GetCoordinatesOf(button, buttonArray);
            int yCoordinate = buttonCoordinates.Item1;
            int xCoordinate = buttonCoordinates.Item2;

            string correctLetter = button.Text;

            // For each up-down, right-left... pair
            for (int i = 0; i < 8;)
            {
                int lettersInARow = 1;

                // Check how many letters there are in a row counting in both directions
                for (int x = 0; x < 2; x++)
                {

                    int newYCoordinate = yCoordinate;
                    int newXCoordinate = xCoordinate;

                    // Change direction if index is out of bounds
                    while (true)
                    {
                        newYCoordinate = newYCoordinate + arrayDirection[i, 0];
                        newXCoordinate = newXCoordinate + arrayDirection[i, 1];

                        if (newYCoordinate < 0 || newXCoordinate < 0 || newYCoordinate > 2 || newXCoordinate > 2)
                        {
                            break;
                        }

                        Button buttonToCheck = buttonArray[newYCoordinate, newXCoordinate];
                        string nextLetter = buttonToCheck.Text;

                        if (nextLetter == correctLetter)
                        {
                            lettersInARow++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    i++;
                }

                if (lettersInARow == 3)
                {
                    return true;
                }
            }

            return false;
        }

        private Tuple<int, int> GetCoordinatesOf(Button button, Button[,] buttonArray)
        {
            int w = buttonArray.GetLength(0); // width
            int h = buttonArray.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (buttonArray[x, y].Equals(button))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);


        }

        private void LockNames(bool isLocked)
        {
            player1TBox.ReadOnly = isLocked;
            player2TBox.ReadOnly = isLocked;

            p1Label.Text = player1TBox.Text;
            p2Label.Text = player2TBox.Text;
        }

        public void RestartGame()
        {
            List<Button> buttonList = new List<Button> { button1, button2, button3, button4, button5
            , button6, button7, button8, button9};
            foreach (var button in buttonList)
            {
                button.Text = "";
            }
            player1Starts = !player1Starts;

            if (player1Starts)
            {
                player1TBox.BackColor = Color.GreenYellow;
                player2TBox.BackColor = Color.LightGray;
            }
            else
            {
                player1TBox.BackColor = Color.LightGray;
                player2TBox.BackColor = Color.GreenYellow;
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "")
            {
                DoButtonClick(button1);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "")
            {
                DoButtonClick(button2);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "")
            {
                DoButtonClick(button3);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "")
            {
                DoButtonClick(button4);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "")
            {
                DoButtonClick(button5);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "")
            {
                DoButtonClick(button6);
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "")
            {
                DoButtonClick(button7);
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "")
            {
                DoButtonClick(button8);
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (button9.Text == "")
            {
                DoButtonClick(button9);
            }
        }

    }
}
