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
using System.Threading;
using System.IO;
using System.Media;

namespace Final_Project_Pac_Man
{
    public partial class Form1 : Form
    {
        //global variables
        Rectangle pacMan = new Rectangle();
        string pacManDirection;
        int pacManStartAngle;
        int pacManSpeed;
        int pacManPreviousX; //for reseting position after wall collision
        int pacManPreviousY;
        Rectangle pacManTop = new Rectangle(); //for stopping at walls
        Rectangle pacManLeft = new Rectangle();
        Rectangle pacManBottom = new Rectangle();
        Rectangle pacManRight = new Rectangle();
        Rectangle pacManCentre = new Rectangle(); //for turn points

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

        int score;
        int time;
        int highScore = 0;

        SolidBrush pacManBrush = new SolidBrush(Color.Yellow);
        SolidBrush pelletsBrush = new SolidBrush(Color.PapayaWhip);
        SolidBrush wallBrush = new SolidBrush(Color.DodgerBlue);

        SoundPlayer bgMusic = new SoundPlayer(Properties.Resources.background_music);

        public Form1()
        {
            InitializeComponent();
            bgMusic.Play();
        }

        /// <summary>
        /// Sets variables and screen objects and starts game
        /// </summary>
        public void GameInit()
        {
            bgMusic.Stop();
            bgMusic.Play();

            titleLabel.Text = "";
            instructionLabel.Text = "";

            score = 0;
            time = 700;

            SetWalls();
            SetTurnPoints();
            SetPellets();
            pellets = pelletsOrigins;

            pacMan = new Rectangle(205, 335, 20, 20);
            pacManDirection = "left";
            pacManStartAngle = 225;
            pacManSpeed = 10;

            gameState = "running";

            StartCountdown();

            gameTimer.Enabled = true;
        }

        /// <summary>
        /// Displays a countdown from 3 to GO
        /// </summary>
        public void StartCountdown()
        {
            countDownLabel.Visible = true;
            countDownLabel.Text = "3";
            Refresh();
            Thread.Sleep(1000);

            countDownLabel.Text = "2";
            Refresh();
            Thread.Sleep(1000);

            countDownLabel.Text = "1";
            Refresh();
            Thread.Sleep(1000);

            countDownLabel.Text = "GO!";
            Refresh();
            Thread.Sleep(1000);
            countDownLabel.Visible = false;
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
            pacManPreviousX = pacMan.X;
            pacManPreviousY = pacMan.Y;

            //move in current direction
            MovePacMan();

            //check for collision with wall in current direction
            PacManWallCollision();

            //check for change in direction
            ChangePacManDirection();

            //check if pacman touches the end of a tunnel
            TunnelTeleport();

            //chack if pacman collides with a pellet
            PacManPelletCollision();

            //decrease time, check for end game
            time--;
            CheckEndConditions();

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

        /// <summary>
        /// Checks current direction and moves PacMan
        /// </summary>
        public void MovePacMan()
        {
            switch (pacManDirection)
            {
                case "up":
                    pacMan.Y -= pacManSpeed;
                    break;
                case "left":
                    pacMan.X -= pacManSpeed;
                    break;
                case "down":
                    pacMan.Y += pacManSpeed;
                    break;
                case "right":
                    pacMan.X += pacManSpeed;
                    break;
            }
        }

        /// <summary>
        /// Checks if PacMan collides with a wall in the current direction
        /// </summary>
        public void PacManWallCollision()
        {
            switch (pacManDirection)
            {
                case "up":
                    pacManTop = new Rectangle(pacMan.X, pacMan.Y, 20, 1);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManTop.IntersectsWith(walls[i]))
                        {
                            WallCollisionReset();
                        }
                    }
                    break;
                case "left":
                    pacManLeft = new Rectangle(pacMan.X, pacMan.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManLeft.IntersectsWith(walls[i]))
                        {
                            WallCollisionReset();
                        }
                    }
                    break;
                case "down":
                    pacManBottom = new Rectangle(pacMan.X, pacMan.Y + 15, 20, 1);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManBottom.IntersectsWith(walls[i]))
                        {
                            WallCollisionReset();
                        }
                    }
                    break;
                case "right":
                    pacManRight = new Rectangle(pacMan.X + 15, pacMan.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManRight.IntersectsWith(walls[i]))
                        {
                            WallCollisionReset();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Resets pacman back to position before movement so it doesn't go into the wall and stops
        /// </summary>
        public void WallCollisionReset()
        {
            pacMan.X = pacManPreviousX;
            pacMan.Y = pacManPreviousY;
            pacManSpeed = 0;
        }

        /// <summary>
        /// Checks current direction and changes direction if key is pressed while at a turn point, can always reverse, changes PacMan pie start angle to face direction of movement
        /// </summary>
        public void ChangePacManDirection()
        {
            pacManCentre = new Rectangle(pacMan.X + 5, pacMan.Y + 5, 10, 10);

            switch (pacManDirection)
            {
                case "up":
                    if (downArrowDown == true)
                    {
                        pacManDirection = "down";
                        pacManStartAngle = 135;
                        pacManSpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (leftArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "left";
                            pacManStartAngle = 225;
                            pacManSpeed = 10;
                        }
                        if (rightArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "right";
                            pacManStartAngle = 45;
                            pacManSpeed = 10;
                        }
                    }
                    break;
                case "left":
                    if (rightArrowDown == true)
                    {
                        pacManDirection = "right";
                        pacManStartAngle = 45;
                        pacManSpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (upArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "up";
                            pacManStartAngle = 315;
                            pacManSpeed = 10;
                        }
                        if (downArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "down";
                            pacManStartAngle = 135;
                            pacManSpeed = 10;
                        }
                    }
                    break;
                case "down":
                    if (upArrowDown == true)
                    {
                        pacManDirection = "up";
                        pacManStartAngle = 315;
                        pacManSpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (leftArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "left";
                            pacManStartAngle = 225;
                            pacManSpeed = 10;
                        }
                        if (rightArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "right";
                            pacManStartAngle = 45;
                            pacManSpeed = 10;
                        }
                    }
                    break;
                case "right":
                    if (leftArrowDown == true)
                    {
                        pacManDirection = "left";
                        pacManStartAngle = 225;
                        pacManSpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (upArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "up";
                            pacManStartAngle = 315;
                            pacManSpeed = 10;
                        }
                        if (downArrowDown == true && pacManCentre.IntersectsWith(turnPoints[i]))
                        {
                            pacManDirection = "down";
                            pacManStartAngle = 135;
                            pacManSpeed = 10;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Moves pacman to the opposite side if touches the end of either tunnel
        /// </summary>
        public void TunnelTeleport()
        {
            if (pacManLeft.X == -5)
            {
                pacMan.X = this.Width - pacMan.Width - 5;
            }
            else if (pacManRight.X == this.Width + 7)
            {
                pacMan.X = 5;
            }
        }

        /// <summary>
        /// Checks for collision beteen PacMan and a pellet, adds points and removes pellet if there is a collision
        /// </summary>
        public void PacManPelletCollision()
        {
            for (int i = 0; i < pellets.Count(); i++)
            {
                if (pacMan.IntersectsWith(pellets[i]))
                {
                    score += 10;
                    pellets.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Ends game if time is up or all the pellet are gone
        /// </summary>
        public void CheckEndConditions()
        {
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
        }

        /// <summary>
        /// Adds all the wall rectangles to the walls list
        /// </summary>
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
            walls.Add(new Rectangle(150, 200, 130, 48));
        }

        /// <summary>
        /// Adds all the turn point rectangles to the turnPoints list
        /// </summary>
        public void SetTurnPoints()
        {
            //intersection points from top to bottom, left to right
            int turnPointSize = 1;
            //row 1
            int turnPointY = 55;
            turnPoints.Add(new Rectangle(25, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(95, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(195, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(235, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(335, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(405, turnPointY, turnPointSize, turnPointSize));

            //row 2
            turnPointY = 105;
            TurnsRow2_X_Pattern(turnPointY, turnPointSize);

            //row 3
            turnPointY = 145;
            TurnsRow2_X_Pattern(turnPointY, turnPointSize);

            //row 4
            turnPointY = 185;
            turnPoints.Add(new Rectangle(139, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(195, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(235, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(291, turnPointY, turnPointSize, turnPointSize));

            //row 5
            turnPointY = 225;
            turnPoints.Add(new Rectangle(95, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(137, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(291, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(335, turnPointY, turnPointSize, turnPointSize));

            //row 6
            turnPointY = 260;
            turnPoints.Add(new Rectangle(139, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(291, turnPointY, turnPointSize, turnPointSize));

            //row 7
            turnPointY = 305;
            TurnsRow2_X_Pattern(turnPointY, turnPointSize);

            //row 8
            turnPointY = 345;
            TurnsRow8_X_Pattern(turnPointY, turnPointSize);

            //row 9
            turnPointY = 385;
            TurnsRow8_X_Pattern(turnPointY, turnPointSize);

            //row 10
            turnPointY = 425;
            turnPoints.Add(new Rectangle(25, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(195, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(235, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(405, turnPointY, turnPointSize, turnPointSize));
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the turn points list, pattern in multiple rows
        /// </summary>
        /// <param name="turnPointY"></param>row y value
        /// <param name="turnPointSize"></param>size of square point
        public void TurnsRow2_X_Pattern(int turnPointY, int turnPointSize)
        {
            turnPoints.Add(new Rectangle(25, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(95, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(139, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(195, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(235, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(291, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(335, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(405, turnPointY, turnPointSize, turnPointSize));
        }

        /// <summary>
        /// Adds a series of rectangles, same as row 2 + two more, in the same row to the turn points list, pattern in multiple rows
        /// </summary>
        /// <param name="turnPointY"></param>row y value
        /// <param name="turnPointSize"></param>size of square point
        public void TurnsRow8_X_Pattern(int turnPointY, int turnPointSize)
        {
            TurnsRow2_X_Pattern(turnPointY, turnPointSize);
            turnPoints.Add(new Rectangle(55, turnPointY, turnPointSize, turnPointSize));
            turnPoints.Add(new Rectangle(375, turnPointY, turnPointSize, turnPointSize));
        }

        /// <summary>
        /// Adds all the pellet rectangles to the pelletsOrigins list
        /// </summary>
        public void SetPellets()
        {
            int pelletSize = 4;
            //pellets top to bottom, left to right
            //row 1
            Row1_X_Pattern(53, pelletSize);

            //row 2
            Row2_X_Pattern(70, pelletSize);

            //row 3
            Row2_X_Pattern(86, pelletSize);

            //row 4
            Row4_X_Pattern(103, pelletSize);

            //row 5
            Row5_X_Pattern(123, pelletSize);

            //row 6
            Row6_X_Pattern(143, pelletSize);

            // rows 7 - 15
            Row7to15(pelletSize);

            //row 16
            Row1_X_Pattern(303, pelletSize);

            //row 17
            Row17_X_Pattern(323, pelletSize);

            //row 18
            Row18_X_Pattern(343, pelletSize);

            //row 19
            Row19_X_Pattern(363, pelletSize);

            //row 20
            Row6_X_Pattern(383, pelletSize);

            //row 21
            Row21_X_Pattern(403, pelletSize);

            //row 22
            Row4_X_Pattern(423, pelletSize);
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
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

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
        public void Row2_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
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

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
        public void Row5_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(135, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(289, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
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

        /// <summary>
        /// Adds multiple rows with the same series of rectangles to the pelletsOrigins list
        /// </summary>
        /// <param name="pelletSize"></param>square pellet value
        public void Row7to15(int pelletSize)
        {
            for (int i = 159; i <= 287; i += 16)
            {
                int pelletY = i;
                pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
                pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            }
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
        public void Row17_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
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

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
        public void Row19_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(51, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(93, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(135, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(289, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(333, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(375, pelletY, pelletSize, pelletSize));
        }

        /// <summary>
        /// Adds a series of rectangles in the same row to the pelletsOrigins list, pattern in multiple rows
        /// </summary>
        /// <param name="pelletY"></param>row y value
        /// <param name="pelletSize"></param>square pellet value
        public void Row21_X_Pattern(int pelletY, int pelletSize)
        {
            pelletsOrigins.Add(new Rectangle(23, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(193, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(233, pelletY, pelletSize, pelletSize));
            pelletsOrigins.Add(new Rectangle(403, pelletY, pelletSize, pelletSize));
        }
    } 
}
