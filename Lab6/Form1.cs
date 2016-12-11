using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;


namespace Lab6
{
    public partial class Form1 : Form
    {
        public enum CellSelection { N, O, X };                     // enum
        //dimensions of board
        private const float CLIENTSIZE = 100;                      // coord system will always be 100x100 units bc transform
        private const float LINELENGTH = 80;
        private const float BLOCK = LINELENGTH / 3;
        private const float OFFSET = 10;
        private const float DELTA = 5;
        private float scale;                                        // current scale factor

        public GameEngine tttGame;                                // init game engine

        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true;
            this.Text = "Lab6 - Elizabeth Kenny";
            tttGame = new GameEngine();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ApplyTransform(g);                                    // rescale via transform
            //draw board
            g.DrawLine(Pens.Black, BLOCK, 0, BLOCK, LINELENGTH);
            g.DrawLine(Pens.Black, 2 * BLOCK, 0, 2 * BLOCK, LINELENGTH);
            g.DrawLine(Pens.Black, 0, BLOCK, LINELENGTH, BLOCK);
            g.DrawLine(Pens.Black, 0, 2 * BLOCK, LINELENGTH, 2 * BLOCK);
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    if (tttGame.grid[i, j] == GameEngine.CellSelection.O)            // draw o
                        tttGame.DrawO(i, j, g);
                    else if (tttGame.grid[i, j] == GameEngine.CellSelection.X)       // draw x
                        tttGame.DrawX(i, j, g);
        }

        private void ApplyTransform(Graphics g)                         // rescale drawing to preserve world coordinates
        {
            scale = Math.Min(ClientRectangle.Width / CLIENTSIZE,
                            ClientRectangle.Height / CLIENTSIZE);
            if (scale == 0f)
                return;
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(OFFSET, OFFSET);
        }

        private void Form1_MouseDown_1(object sender, MouseEventArgs e)                 // user move with mouse clicks
        {
            Graphics g = CreateGraphics();
            ApplyTransform(g);                                        // rescale via transform
            PointF[] p = { new Point(e.X, e.Y) };
            g.TransformPoints(CoordinateSpace.World,
                            CoordinateSpace.Device, p);
            if (!tttGame.compMove)
            {
                Invalidate();
                tttGame.userTurn(e, p, tttGame);                // if user move
            }

            if (tttGame.moveCount >= 0)                                 // if first turn has passed
            {
                computerStartsToolStripMenuItem.Enabled = false;
                tttGame.compMove_1 = false;
            }

            Invalidate();
        }

        private void computerStartsToolStripMenuItem_Click(object sender, EventArgs e)              // computer first move selected
        {
            computerStartsToolStripMenuItem.Enabled = false;                // disable  
            tttGame.compMove_1 = true;                                      // comp has first move
            tttGame.compMove = true;
            tttGame.compTurn(tttGame);
            Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)         // restart game
        {
            tttGame = new GameEngine();
            computerStartsToolStripMenuItem.Enabled = true;
            Invalidate();
        }
    }
}
