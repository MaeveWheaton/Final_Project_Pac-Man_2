/*
 * Maeve Wheaton
 * Mr.T
 * June 17, 2021
 * Pac-Man inspired game where the player must collect all the dots in the map before the time runs out. 
*/

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
        Rectangle pacMan = new Rectangle();
        string pacManDirection;
        int pacManStartAngle;
        int pacManSpeed;

        List<Rectangle> walls = new List<Rectangle>();
        List<Rectangle> pelletsOrigins = new List<Rectangle>();
        List<Rectangle> pellets = new List<Rectangle>();
        List<Rectangle> turnPoints = new List<Rectangle>();


        bool upArrowDown;
        bool leftArrowDown;
        bool downArrowDown;
        bool rightArrowDown;

        string gameState = "waiting";
        string outcome;

        int score = 0;
        int time = 1000;
        int highScore = 0;

        SolidBrush pacManBrush = new SolidBrush(Color.Yellow);
        SolidBrush pelletsBrush = new SolidBrush(Color.PapayaWhip);
        SolidBrush wallBrush = new SolidBrush(Color.DodgerBlue);

        public Form1()
        {
            InitializeComponent();
        }

        public void GameInit()
        {
            titleLabel.Text = "";
            instructionLabel.Text = "";

            score = 0;
            time = 1000;

            SetWalls();
            SetTurnPoints();
            SetPellets();
            pellets = pelletsOrigins;

            pacMan = new Rectangle(205, 335, 20, 20);
            pacManDirection = "left";
            pacManStartAngle = 225;
            pacManSpeed = 10;

            gameState = "running";
            gameTimer.Enabled = true;
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
            int pacManPreviousX = pacMan.X;
            int pacManPreviousY = pacMan.Y;

            //check for change direction 


            //move pacMan based on direction


            //if (pacManSpeed == 0 &&
            if (upArrowDown == true)
            {
                pacManSpeed = 10;
                pacMan.Y -= pacManSpeed;
            }
            if (leftArrowDown == true)
            {
                pacManSpeed = 10;
                pacMan.X -= pacManSpeed;
            }
            if (downArrowDown == true)
            {
                pacManSpeed = 10;
                pacMan.Y += pacManSpeed;
            }
            if (rightArrowDown == true)
            {
                pacManSpeed = 10;
                pacMan.X += pacManSpeed;
            }

            //stop when collides with wall
            for (int i = 0; i < walls.Count(); i++)
            {
                if (pacMan.IntersectsWith(walls[i]))
                {
                    pacMan.X = pacManPreviousX;
                    pacMan.Y = pacManPreviousY;
                    pacManSpeed = 0;
                }
            }

            //pacman use tunnels to teleport to the other side
            if (pacMan.X <= 0)
            {
                pacMan.X = this.Width - pacMan.Width;
            }
            else if (pacMan.X >= this.Width - pacMan.Width)
            {
                pacMan.X = 0;
            }

            //pacman collide with pellet, gain points and remove pellet
           for (int i = 0; i < pellets.Count(); i++)
            {
                if (pacMan.IntersectsWith(pellets[i]))
                {
                    score += 10;
                    pellets.RemoveAt(i);
                }
            }

           //decrease time and end game if time is up or all the pellet are gone
            time--;

            if (time == 0)
            {
                outcome = "lose";
                gameTimer.Enabled = false;
                gameState = "over";
            }
            else if (pellets.Count() == 0)
            {
                outcome = "win";
                gameTimer.Enabled = false;
                gameState = "over";
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                //write labels
                timeLabel.Text = "";
                scoreLabel.Text = "";
                highScoreLabel.Text = $"HIGHSCORE: {highScore}";
                instructionLabel.Text = "PRESS SPACE TO START\n\nPRESS ESCAPE TO EXIT\n\nUSE ARROWS TO CHANGE DIRECTION";
                titleLabel.Text = "PAC-MAN";

                //draw pacman and dots
                e.Graphics.FillPie(pacManBrush, 135, 195, 40, 40, 45, 270);
                e.Graphics.FillEllipse(pelletsBrush, 195, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 225, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 255, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 285, 210, 10, 10);
            }
            else if (gameState == "running")
            {
                //update labels
                timeLabel.Text = $"TIME LEFT: {time}";
                scoreLabel.Text = $"SCORE: {score}";

                //draw walls
                for (int i = 0; i < walls.Count(); i++)
                {
                    e.Graphics.FillRectangle(wallBrush, walls[i]);
                }

                //draw pellets
                for (int i = 0; i < pellets.Count(); i++)
                {
                    e.Graphics.FillRectangle(pelletsBrush, pellets[i]);
                }

                //draw pacman
                e.Graphics.FillPie(pacManBrush, pacMan, pacManStartAngle, 270);
            }
            else if (gameState == "over")
            {
                //clear labels
                timeLabel.Text = "";
                scoreLabel.Text = "";

                //determine title message
                if (outcome == "win")
                {
                    titleLabel.Text = "YOU WIN";
                }
                else if (outcome == "lose")
                {
                    titleLabel.Text = "GAME OVER";
                }
                instructionLabel.Text = "PRESS SPACE TO PLAY AGAIN\n\nPRESS ESCAPE TO EXIT";

                //draw pacman and dots
                e.Graphics.FillPie(pacManBrush, 135, 195, 40, 40, 45, 270);
                e.Graphics.FillEllipse(pelletsBrush, 195, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 225, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 255, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 285, 210, 10, 10);

                //update highscore
                if (score > highScore)
                {
                    highScore = score;
                    highScoreLabel.Text = $"HIGHSCORE: {highScore}";
                }
            }
        }

        public void SetWalls()
        {
            int outsideWallWeight = 5;
            //outside walls from the top counterclockwise (around in the left direction)
            walls.Add(new Rectangle(5, 35, this.Width - 10, outsideWallWeight)); //top
            walls.Add(new Rectangle(5, 35, outsideWallWeight, 130));
            walls.Add(new Rectangle(5, 160, 75, outsideWallWeight));
            walls.Add(new Rectangle(75, 160, outsideWallWeight, 45));
            walls.Add(new Rectangle(0, 205, 80, outsideWallWeight)); //left top tunnel wall
            walls.Add(new Rectangle(0, 240, 80, outsideWallWeight)); //left bottom tunnel wall
            walls.Add(new Rectangle(75, 240, outsideWallWeight, 45));
            walls.Add(new Rectangle(5, 285, 75, outsideWallWeight));
            walls.Add(new Rectangle(5, 285, outsideWallWeight, 160));
            walls.Add(new Rectangle(5, this.Height - 30, this.Width - 10, outsideWallWeight)); //bottom
            walls.Add(new Rectangle(this.Width - 10, 285, outsideWallWeight, 160));
            walls.Add(new Rectangle(this.Width - 80, 285, 75, outsideWallWeight));
            walls.Add(new Rectangle(this.Width - 80, 240, outsideWallWeight, 45));
            walls.Add(new Rectangle(this.Width - 80, 240, 80, outsideWallWeight)); //right bottom tunnel wall
            walls.Add(new Rectangle(this.Width - 80, 205, 80, outsideWallWeight)); //right top tunnel wall
            walls.Add(new Rectangle(this.Width - 80, 160, outsideWallWeight, 45));
            walls.Add(new Rectangle(this.Width - 80, 160, 75, outsideWallWeight));
            walls.Add(new Rectangle(this.Width - 10, 35, outsideWallWeight, 130));

            //large rectangles at the top
            walls.Add(new Rectangle(40, 70, 40, 20)); //left
            walls.Add(new Rectangle(110, 70, 70, 20));
            walls.Add(new Rectangle(this.Width - 80, 70, 40, 20)); //right
            walls.Add(new Rectangle(this.Width - 180, 70, 70, 20));

            int insideWallWeight = 10;
            //inside walls from top to bottom, left to right
            walls.Add(new Rectangle((this.Width / 2) - 5, 35, insideWallWeight, 55)); //vertical at very top
            walls.Add(new Rectangle(40, 120, 40, insideWallWeight)); //left horizontal individual
            walls.Add(new Rectangle(110, 120, insideWallWeight, 88)); //left horizontal T
            walls.Add(new Rectangle(110, 160, 70, insideWallWeight));
            walls.Add(new Rectangle(150, 120, 130, insideWallWeight)); //top T
            walls.Add(new Rectangle(210, 120, insideWallWeight, 45));
            walls.Add(new Rectangle(310, 120, insideWallWeight, 88)); //right horizontal T
            walls.Add(new Rectangle(250, 160, 70, insideWallWeight));
            walls.Add(new Rectangle(this.Width - 80, 120, 40, insideWallWeight)); //right horizontal individual
            walls.Add(new Rectangle(110, 242, insideWallWeight, 48)); //lone vertical lines in the middle
            walls.Add(new Rectangle(310, 242, insideWallWeight, 48));
            walls.Add(new Rectangle(150, 280, 130, insideWallWeight)); //middle T
            walls.Add(new Rectangle(210, 280, insideWallWeight, 50));
            walls.Add(new Rectangle(40, 320, 40, insideWallWeight)); //left inverted L
            walls.Add(new Rectangle(70, 320, insideWallWeight, 50));
            walls.Add(new Rectangle(110, 320, 70, insideWallWeight)); //two horizontal individual
            walls.Add(new Rectangle(250, 320, 70, insideWallWeight));
            walls.Add(new Rectangle(350, 320, 40, insideWallWeight)); //right inverted L
            walls.Add(new Rectangle(350, 320, insideWallWeight, 50));
            walls.Add(new Rectangle(10, 360, 30, insideWallWeight)); //left short horizontal
            walls.Add(new Rectangle(40, 400, 140, insideWallWeight)); //left inverted T
            walls.Add(new Rectangle(110, 360, insideWallWeight, 50));
            walls.Add(new Rectangle(150, 360, 130, insideWallWeight)); //bottom T
            walls.Add(new Rectangle(210, 360, insideWallWeight, 50));
            walls.Add(new Rectangle(250, 400, 140, insideWallWeight)); //right inverted T
            walls.Add(new Rectangle(310, 360, insideWallWeight, 50));
            walls.Add(new Rectangle(390, 360, 30, insideWallWeight)); //right short horizontal

            //ghost house
            walls.Add(new Rectangle(150, 203, 130, 45));
        }

        public void SetTurnPoints()
        {

        }

        public void SetPellets()
        {
            int pelletSize = 4;
            //pellets top to bottom, left to right
            //row 1
            int pelletY = 53;
            Row1_X_Pattern(pelletY, pelletSize);

            //row 2
            pelletY = 70;
            Row2_X_Pattern(pelletY, pelletSize);

            //row 3
            pelletY = 86;
            Row2_X_Pattern(pelletY, pelletSize);

            //row 4
            pelletY = 103;
            Row4_X_Pattern(pelletY, pelletSize);

            //row 5
            pelletY = 123;
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(135, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(289, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));

            //row 6
            pelletY = 143;
            Row6_X_Pattern(pelletY, pelletSize);

            // rows 7 - 15
            Row7to15(pelletY, pelletSize);

            //row 16
            pelletY = 303;
            Row1_X_Pattern(pelletY, pelletSize);

            //row 17
            pelletY = 323;
            Row17_X_Pattern(pelletY, pelletSize);

            //row 18
            pelletY = 343;
            Row18_X_Pattern(pelletY, pelletSize);

            //row 19
            pelletY = 363;
            pelletsOrigins.Add(new Rectangle(51, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(135, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(289, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(375, pelletY, pelletSize, pelletSize));

            //row 20
            pelletY = 383;
            Row6_X_Pattern(pelletY, pelletSize);

            //row 21
            pelletY = 403;
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));

            //row 22
            pelletY = 423;
            Row4_X_Pattern(pelletY, pelletSize);
        }

        public void Row1_X_Pattern(int pelletY, int pelletSize)
        {
            for (int i = 23; i <= 163; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(178, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            for (int i = 233; i <= 303; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(318, pelletY, pelletSize, pelletSize));
            for (int i = 333; i <= 403; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
        }

        public void Row2_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }

        public void Row4_X_Pattern(int pelletY, int pelletSize)
        {
            for (int i = 23; i <= 163; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(178, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));

            pelletsOrigins.Add(new Rectangle(207, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(221, pelletY, pelletSize, pelletSize));

            for (int i = 233; i <= 303; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(318, pelletY, pelletSize, pelletSize));
            for (int i = 333; i <= 403; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
        }

        public void Row6_X_Pattern(int pelletY, int pelletSize)
        {
            for (int i = 23; i <= 93; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            for (int i = 135; i <= 163; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(178, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            for (int i = 233; i <= 289; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            for (int i = 333; i <= 403; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
        }

        public void Row7to15(int pelletY, int pelletSize)
        {
            for (int i = 159; i <= 287; i += 16)
            {
                pelletY = i;
                pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
                pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            }
        }

        public void Row17_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }

        public void Row18_X_Pattern(int pelletY, int pelletSize)
        {
            for (int i = 23; i <= 51; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            for (int i = 93; i <= 163; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(178, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            for (int i = 233; i <= 303; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
            pelletsOrigins.Add(new Rectangle(318, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            for (int i = 375; i <= 403; i += 14)
            {
                pelletsOrigins.Add(new Rectangle(i, pelletY, pelletSize, pelletSize));
            }
        }
    } 
}
