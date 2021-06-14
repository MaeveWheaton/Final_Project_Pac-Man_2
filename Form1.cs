using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project_Pac_Man
{
    public partial class Form1 : Form
    {
        //global variables

        bool upArrowDown;
        bool leftArrowDown;
        bool downArrowDown;
        bool rightArrowDown;

        string gameState = "waiting";
        string outcome;

        int score = 0;
        int time = 2000;
        int highScore = 0;

        SolidBrush pacManBrush = new SolidBrush(Color.Yellow);
        SolidBrush pelletsBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
        }

        public void GameInit()
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInit();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                //write labels
                timeLabel.Text = "";
                scoreLabel.Text = "";
                highScoreLabel.Text = $"HIGHSCORE: {highScore}";
                instructionLabel.Text = "PRESS SPACE TO START\n\nPRESS ESCAPE TO EXIT\n\nUSE ARROWS TO MOVE";
                titleLabel.Text = "PAC-MAN";

                //draw pacman and dots
                e.Graphics.FillPie(pacManBrush, 220, 300, 40, 40, 45, 270);
                e.Graphics.FillEllipse(pelletsBrush, 280, 315, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 310, 315, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 340, 315, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 370, 315, 10, 10);
            }
            /*else if (gameState == "running")
            {
                //update labels
                timeLabel.Text = $"Time Left: {time}";
                scoreLabel.Text = $"Score: {score}";

                //draw ground
                e.Graphics.FillRectangle(greenBrush, 0, this.Height - groundHeight,
                    this.Width, groundHeight);

                //draw hero
                e.Graphics.FillRectangle(whiteBrush, hero);

                //draw balls
                for (int i = 0; i < balls.Count(); i++)
                {
                    if (ballColours[i] == "red")
                    {
                        e.Graphics.FillEllipse(redBrush, balls[i]);
                    }
                    else if (ballColours[i] == "green")
                    {
                        e.Graphics.FillEllipse(greenBrush, balls[i]);
                    }
                    else if (ballColours[i] == "gold")
                    {
                        e.Graphics.FillEllipse(goldBrush, balls[i]);
                    }
                }
            }
            else if (gameState == "over")
            {
                timeLabel.Text = "";
                scoreLabel.Text = "";

                if (outcome == "win")
                {
                    titleLabel.Text = "YOU MADE IT!";
                    subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                }
                else if (outcome == "lose")
                {
                    titleLabel.Text = "THEY GOT YOU";
                    subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                }
            }*/
        }
    }
}
