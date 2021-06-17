/*
 * Maeve Wheaton
 * Mr.T
 * June 17, 2021
 * Pac-Man inspired game with 1 player and 2 player options.
 * 1 player - the player, as Pac-Man, must collect all the "pellets" in the map before the time runs out without being caught by a ghost
 * 2 player - player 1, as Pac-man, must collect all the "pellets" in the map; player 2, as the ghost Blinky, must try to catch p1
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
        //player 1
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

        //blinky = red ghost, player 2/ enemy
        Rectangle blinky = new Rectangle();
        string blinkyDirection;
        int blinkySpeed;
        int blinkyPreviousX; //for reseting position after wall collision
        int blinkyPreviousY;
        Rectangle blinkyTop = new Rectangle(); //for stopping at walls
        Rectangle blinkyLeft = new Rectangle();
        Rectangle blinkyBottom = new Rectangle();
        Rectangle blinkyRight = new Rectangle();
        Rectangle blinkyCentre = new Rectangle(); //for turn points

        Random randGhostDirection = new Random();
        int newGhostDirection;

        List<Rectangle> walls = new List<Rectangle>();
        List<Rectangle> pelletsOrigins = new List<Rectangle>();
        List<Rectangle> pellets = new List<Rectangle>();
        List<Rectangle> turnPoints = new List<Rectangle>();

        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        bool upArrowDown;
        bool leftArrowDown;
        bool downArrowDown;
        bool rightArrowDown;

        string gameState = "waiting";
        string gameMode = "undefined";
        string outcome;

        int score;
        int time;
        int onePHighScore = 0;
        int twoPHighScore = 0;

        SolidBrush pacManBrush = new SolidBrush(Color.Yellow);
        SolidBrush blinkyBrush = new SolidBrush(Color.Red);
        SolidBrush pelletsBrush = new SolidBrush(Color.PapayaWhip);
        SolidBrush wallBrush = new SolidBrush(Color.DodgerBlue);

        SoundPlayer bgMusic = new SoundPlayer(Properties.Resources.background_music);

        public Form1()
        {
            InitializeComponent();
            bgMusic.Play();
        }

        private void p1Button_Click(object sender, EventArgs e)
        {
            gameMode = "1p";

            //remove buttons
            p1Button.Visible = false;
            p1Button.Enabled = false;
            p2Button.Visible = false;
            p2Button.Enabled = false;
        }

        private void p2Button_Click(object sender, EventArgs e)
        {
            gameMode = "2p";

            //remove buttons
            p1Button.Visible = false;
            p1Button.Enabled = false;
            p2Button.Visible = false;
            p2Button.Enabled = false;
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
            time = 850;

            SetWalls();
            SetTurnPoints();
            SetPellets();
            pellets = pelletsOrigins;

            pacMan = new Rectangle(205, 335, 20, 20);
            pacManDirection = "left";
            pacManStartAngle = 225;
            pacManSpeed = 10;

            blinky = new Rectangle(205, 175, 20, 20);
            blinkyDirection = "right";
            blinkySpeed = 10;

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
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
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
                    if (gameState == "waiting" && gameMode == "1p" ||
                        gameState == "waiting" && gameMode == "2p" || 
                        gameState == "over")
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
                case Keys.B:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        gameState = "waiting";
                        gameMode = "undefined";

                        //add buttons
                        p1Button.Visible = true;
                        p1Button.Enabled = true;
                        p2Button.Visible = true;
                        p2Button.Enabled = true;
                        Refresh();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
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

            if (gameMode == "2p")
            {
                //blinky controls and collisions
                blinkyPreviousX = blinky.X;
                blinkyPreviousY = blinky.Y;

                //move in current direction
                MoveBlinky();

                //check for collision with wall in current direction
                BlinkyWallCollision();

                //check for change in direction
                ChangeBlinkyDirection();
            }
            else
            {
                //blinky controls and collisions
                blinkyPreviousX = blinky.X;
                blinkyPreviousY = blinky.Y;

                //move in current direction
                MoveBlinky();

                //check for change in direction
                BlinkyAtonumousDirectionChange();

                //check for collision with wall in current direction
                BlinkyWallCollision();

                //decrease time for 1p
                time--;
            }

            //check for end game
            CheckEndConditions();

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                if (gameMode == "undefined")
                {
                    //write labels
                    timeLabel.Text = "";
                    scoreLabel.Text = "";
                    highScoreLabel.Text = "";
                    instructionLabel.Text = "CHOOSE GAME MODE\n\n\n\n\n\n";
                    titleLabel.Text = "PAC-MAN";

                    //draw pacman and dots
                    e.Graphics.FillPie(pacManBrush, 135, 195, 40, 40, 45, 270);
                    e.Graphics.FillEllipse(pelletsBrush, 195, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 225, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 255, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 285, 210, 10, 10);
                }
                else if (gameMode == "1p" || gameMode == "2p")
                {
                    //write labels
                    if (gameMode == "1p")
                    {
                        highScoreLabel.Text = $"HIGHSCORE: {onePHighScore}";
                        instructionLabel.Text = "PRESS SPACE TO START\n\nPRESS ESCAPE TO EXIT\n\nUSE ARROWS TO CHANGE DIRECTION\n\nPRESS B TO CHANGE GAME MODE";
                        titleLabel.Text = "1 PLAYER";
                    }
                    else
                    {
                        highScoreLabel.Text = $"HIGHSCORE: {twoPHighScore}";
                        instructionLabel.Text = "PRESS SPACE TO START\n\nPRESS ESCAPE TO EXIT\n\nPRESS B TO CHANGE GAME MODE\n\nPACMAN - ARROWS (collect all dots to win)\nBLINKY - WASD (catch Pac-Man to win)";
                        titleLabel.Text = "2 PLAYERS";
                    }

                    //draw pacman and dots
                    e.Graphics.FillPie(pacManBrush, 135, 195, 40, 40, 45, 270);
                    e.Graphics.FillEllipse(pelletsBrush, 195, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 225, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 255, 210, 10, 10);
                    e.Graphics.FillEllipse(pelletsBrush, 285, 210, 10, 10);
                }
            }
            else if (gameState == "running")
            {
                //update score
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

                //draw blinky
                e.Graphics.FillEllipse(blinkyBrush, blinky);

                //mode specific code
                if (gameMode == "1p")
                {
                    //update time
                    timeLabel.Text = $"TIME LEFT: {time}";
                }
            }
            else if (gameState == "over")
            {
                if (gameMode == "1p")
                {
                    //clear time
                    timeLabel.Text = "";

                    //determine title message
                    if (outcome == "p1win")
                    {
                        titleLabel.Text = "YOU WIN";
                    }
                    else
                    {
                        titleLabel.Text = "GAME OVER";
                    }

                    //update highscore
                    if (score > onePHighScore)
                    {
                        onePHighScore = score;
                        highScoreLabel.Text = $"HIGHSCORE: {onePHighScore}";
                    }
                }
                else if (gameMode == "2p")
                {
                    //determine title message
                    if (outcome == "p1win")
                    {
                        titleLabel.Text = "PACMAN WINS";
                    }
                    else
                    {
                        titleLabel.Text = "GHOST WINS";
                    }

                    //update highscore
                    if (score > twoPHighScore)
                    {
                        twoPHighScore = score;
                        highScoreLabel.Text = $"HIGHSCORE: {twoPHighScore}";
                    }
                }

                //write instructions
                scoreLabel.Text = $"SCORE: {score}";
                instructionLabel.Text = "PRESS SPACE TO PLAY AGAIN\n\nPRESS ESCAPE TO EXIT\n\nPRESS B TO CHANGE GAME MODE\n";

                //draw pacman and dots
                e.Graphics.FillPie(pacManBrush, 135, 195, 40, 40, 45, 270);
                e.Graphics.FillEllipse(pelletsBrush, 195, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 225, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 255, 210, 10, 10);
                e.Graphics.FillEllipse(pelletsBrush, 285, 210, 10, 10);
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
                            PacManWallCollisionReset();
                        }
                    }
                    break;
                case "left":
                    pacManLeft = new Rectangle(pacMan.X, pacMan.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManLeft.IntersectsWith(walls[i]))
                        {
                            PacManWallCollisionReset();
                        }
                    }
                    break;
                case "down":
                    pacManBottom = new Rectangle(pacMan.X, pacMan.Y + 15, 20, 1);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManBottom.IntersectsWith(walls[i]))
                        {
                            PacManWallCollisionReset();
                        }
                    }
                    break;
                case "right":
                    pacManRight = new Rectangle(pacMan.X + 15, pacMan.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (pacManRight.IntersectsWith(walls[i]))
                        {
                            PacManWallCollisionReset();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Resets pacman back to position before movement so it doesn't go into the wall and stops
        /// </summary>
        public void PacManWallCollisionReset()
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
        /// Checks current direction and moves Blinky
        /// </summary>
        public void MoveBlinky()
        {
            switch (blinkyDirection)
            {
                case "up":
                    blinky.Y -= blinkySpeed;
                    break;
                case "left":
                    blinky.X -= blinkySpeed;
                    break;
                case "down":
                    blinky.Y += blinkySpeed;
                    break;
                case "right":
                    blinky.X += blinkySpeed;
                    break;
            }
        }

        /// <summary>
        /// Checks if Blinky collides with a wall in the current direction
        /// </summary>
        public void BlinkyWallCollision()
        {
            switch (blinkyDirection)
            {
                case "up":
                    blinkyTop = new Rectangle(blinky.X, blinky.Y, 20, 1);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (blinkyTop.IntersectsWith(walls[i]))
                        {
                            BlinkyWallCollisionReset();
                        }
                    }
                    break;
                case "left":
                    blinkyLeft = new Rectangle(blinky.X, blinky.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (blinkyLeft.IntersectsWith(walls[i]))
                        {
                            BlinkyWallCollisionReset();
                        }
                    }
                    break;
                case "down":
                    blinkyBottom = new Rectangle(blinky.X, blinky.Y + 15, 20, 1);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (blinkyBottom.IntersectsWith(walls[i]))
                        {
                            BlinkyWallCollisionReset();
                        }
                    }
                    break;
                case "right":
                    blinkyRight = new Rectangle(blinky.X + 15, blinky.Y, 1, 20);
                    for (int i = 0; i < walls.Count(); i++)
                    {
                        if (blinkyRight.IntersectsWith(walls[i]))
                        {
                            BlinkyWallCollisionReset();
                        }
                    }
                    break;
            }

            if (blinky.X <= 0 || blinky.X >= this.Width - blinky.Width)
            {
                if (gameMode == "1p")
                {
                    if (blinkyDirection == "left")
                    {
                        blinkyDirection = "right";
                    }
                    else
                    {
                        blinkyDirection = "left";
                    }
                }
                else
                {
                    BlinkyWallCollisionReset();
                }
            }
        }

        /// <summary>
        /// Resets Blinky back to position before movement so it doesn't go into the wall and stops
        /// </summary>
        public void BlinkyWallCollisionReset()
        {
            blinky.X = blinkyPreviousX;
            blinky.Y = blinkyPreviousY;
            blinkySpeed = 0;
        }

        /// <summary>
        /// Checks current Blinky direction and changes direction if key is pressed while at a turn point, can always reverse
        /// </summary>
        public void ChangeBlinkyDirection()
        {
            blinkyCentre = new Rectangle(blinky.X + 5, blinky.Y + 5, 10, 10);

            switch (blinkyDirection)
            {
                case "up":
                    if (sDown == true)
                    {
                        blinkyDirection = "down";
                        blinkySpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (aDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "left";
                            blinkySpeed = 10;
                        }
                        if (dDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "right";
                            blinkySpeed = 10;
                        }
                    }
                    break;
                case "left":
                    if (dDown == true)
                    {
                        blinkyDirection = "right";
                        blinkySpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (wDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "up";
                            blinkySpeed = 10;
                        }
                        if (sDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "down";
                            blinkySpeed = 10;
                        }
                    }
                    break;
                case "down":
                    if (wDown == true)
                    {
                        blinkyDirection = "up";
                        blinkySpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (aDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "left";
                            blinkySpeed = 10;
                        }
                        if (dDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "right";
                            blinkySpeed = 10;
                        }
                    }
                    break;
                case "right":
                    if (aDown == true)
                    {
                        blinkyDirection = "left";
                        blinkySpeed = 10;
                    }
                    for (int i = 0; i < turnPoints.Count(); i++)
                    {
                        if (wDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "up";
                            blinkySpeed = 10;
                        }
                        if (sDown == true && blinkyCentre.IntersectsWith(turnPoints[i]))
                        {
                            blinkyDirection = "down";
                            blinkySpeed = 10;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Ramdomly decides new ghost direction, for 1p mode
        /// </summary>
        public void BlinkyAtonumousDirectionChange()
        {
            blinkyCentre = new Rectangle(blinky.X + 5, blinky.Y + 5, 10, 10);

            //1 = up, 2 = left, 3 = down, 4 = right
            for (int i = 0; i < turnPoints.Count(); i++)
            {
                if (blinkyCentre.IntersectsWith(turnPoints[i]))
                {
                    if (blinkyDirection == "up")
                    {
                        newGhostDirection = randGhostDirection.Next(2, 5);
                    }
                    else if (blinkyDirection == "left")
                    {
                        newGhostDirection = randGhostDirection.Next(1, 5);
                        while (newGhostDirection == 2)
                        {
                            newGhostDirection = randGhostDirection.Next(1, 5);
                        }
                    }
                    else if (blinkyDirection == "down")
                    {
                        newGhostDirection = randGhostDirection.Next(1, 4);
                    }
                    else if (blinkyDirection == "right")
                    {
                        newGhostDirection = randGhostDirection.Next(1, 5);
                        while (newGhostDirection == 4)
                        {
                            newGhostDirection = randGhostDirection.Next(1, 5);
                        }
                    }

                    SwitchGhostDirection();
                }
            }
        }

        /// <summary>
        /// Changes ghost direction
        /// </summary>
        public void SwitchGhostDirection()
        {
            switch (newGhostDirection)
            {
                case 1:
                    blinkyDirection = "up";
                    blinkySpeed = 10;
                    break;
                case 2:
                    blinkyDirection = "left";
                    blinkySpeed = 10;
                    break;
                case 3:
                    blinkyDirection = "down";
                    blinkySpeed = 10;
                    break;
                case 4:
                    blinkyDirection = "right";
                    blinkySpeed = 10;
                    break;
            }
        }

        /// <summary>
        /// Ends game if time is up or all the pellet are gone
        /// </summary>
        public void CheckEndConditions()
        {
            if (gameMode == "1p" && time == 0)
            {
                outcome = "lose";
                gameTimer.Enabled = false;
                gameState = "over";
            }
            else if (blinky.IntersectsWith(pacMan))
            {
                outcome = "p2win";
                gameTimer.Enabled = false;
                gameState = "over";
            }
            else if (pellets.Count() == 0)
            {
                outcome = "p1win";
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
