
namespace Final_Project_Pac_Man
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.scoreLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.highScoreLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.countDownLabel = new System.Windows.Forms.Label();
            this.p1Button = new System.Windows.Forms.Button();
            this.p2Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 50;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // scoreLabel
            // 
            this.scoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.scoreLabel.Font = new System.Drawing.Font("Consolas", 9.900001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(12, 9);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(109, 106);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "scoreLabel";
            // 
            // timeLabel
            // 
            this.timeLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLabel.Font = new System.Drawing.Font("Consolas", 9.900001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.Location = new System.Drawing.Point(145, 9);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(129, 106);
            this.timeLabel.TabIndex = 2;
            this.timeLabel.Text = "timeLabel";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // highScoreLabel
            // 
            this.highScoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.highScoreLabel.Font = new System.Drawing.Font("Consolas", 9.900001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highScoreLabel.Location = new System.Drawing.Point(235, 9);
            this.highScoreLabel.Name = "highScoreLabel";
            this.highScoreLabel.Size = new System.Drawing.Size(183, 106);
            this.highScoreLabel.TabIndex = 3;
            this.highScoreLabel.Text = "highScoreLabel";
            this.highScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.Yellow;
            this.titleLabel.Location = new System.Drawing.Point(64, 128);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(315, 64);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "titleLabel";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // instructionLabel
            // 
            this.instructionLabel.BackColor = System.Drawing.Color.Transparent;
            this.instructionLabel.Font = new System.Drawing.Font("Consolas", 14.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionLabel.Location = new System.Drawing.Point(-39, 243);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(516, 189);
            this.instructionLabel.TabIndex = 5;
            this.instructionLabel.Text = "instructionLabel";
            this.instructionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // countDownLabel
            // 
            this.countDownLabel.BackColor = System.Drawing.Color.Transparent;
            this.countDownLabel.Font = new System.Drawing.Font("Consolas", 14.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countDownLabel.Location = new System.Drawing.Point(-39, 205);
            this.countDownLabel.Name = "countDownLabel";
            this.countDownLabel.Size = new System.Drawing.Size(504, 117);
            this.countDownLabel.TabIndex = 6;
            this.countDownLabel.Text = "countDownLabel";
            this.countDownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.countDownLabel.Visible = false;
            // 
            // p1Button
            // 
            this.p1Button.BackColor = System.Drawing.Color.Transparent;
            this.p1Button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.p1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.p1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.p1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.p1Button.ForeColor = System.Drawing.Color.White;
            this.p1Button.Location = new System.Drawing.Point(152, 308);
            this.p1Button.Name = "p1Button";
            this.p1Button.Size = new System.Drawing.Size(58, 48);
            this.p1Button.TabIndex = 7;
            this.p1Button.Text = "1p";
            this.p1Button.UseVisualStyleBackColor = false;
            this.p1Button.Click += new System.EventHandler(this.p1Button_Click);
            // 
            // p2Button
            // 
            this.p2Button.BackColor = System.Drawing.Color.Transparent;
            this.p2Button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.p2Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.p2Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.p2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.p2Button.ForeColor = System.Drawing.Color.White;
            this.p2Button.Location = new System.Drawing.Point(231, 308);
            this.p2Button.Name = "p2Button";
            this.p2Button.Size = new System.Drawing.Size(58, 48);
            this.p2Button.TabIndex = 8;
            this.p2Button.Text = "2p";
            this.p2Button.UseVisualStyleBackColor = false;
            this.p2Button.Click += new System.EventHandler(this.p2Button_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(430, 470);
            this.Controls.Add(this.p2Button);
            this.Controls.Add(this.p1Button);
            this.Controls.Add(this.countDownLabel);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.highScoreLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 14.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label highScoreLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Label countDownLabel;
        private System.Windows.Forms.Button p1Button;
        private System.Windows.Forms.Button p2Button;
    }
}

