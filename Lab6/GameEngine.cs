using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public class GameEngine
    {
        public const float LINELENGTH = 80;                        // total size of square board
        public const float BLOCK = LINELENGTH / 3;                 // block size
        private const float DELTA = 5;
        public enum CellSelection { N, O, X };                     // enum
        public CellSelection[,] grid = new CellSelection[3, 3];    // 2D array to store state of cell (what to draw)

        public int[] compGuide = new int[8];                      // array to track logic for possible routes to win
                                                                  /*
                                                                   * row 0 :: i=0
                                                                   * row 1 ::   1
                                                                   * row 2 ::   2
                                                                   * col 0 ::   3
                                                                   * col 1 ::   4
                                                                   * col 2 ::   5
                                                                   * diag (top left to bottom right) ::   6
                                                                   * diag (bottom left to top right) ::   7
                                                                   */
        public int[] userGuide = new int[8];                       // array to track possible wins for user

        public bool endGame;                                       // game over
        public bool userW;                                         // user won game
        public bool compW;                                         // computer won game
        public bool tieGame;                                       // tie 
        public bool compMove;                                      // computer's turn
        public bool compMove_1;                                    // move comp first
        public int moveCount;                                      // turn counter

        public GameEngine()         // default constructor to start new game/ reset board
        {
            moveCount = 0;
            endGame = false;
            compMove = false;
            for (int k = 0; k < 8; k++)
            {
                compGuide[k] = 0;
                userGuide[k] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.grid[i, j] = CellSelection.N;
                }
            }
        }

        public void userTurn(MouseEventArgs e, PointF[] p, GameEngine hawtGame)
        {
            if (p[0].X < 0 || p[0].Y < 0)
                return;
            int i = (int)(p[0].X / BLOCK);
            int j = (int)(p[0].Y / BLOCK);
            if (i > 2 || j > 2)
                return;
            if (endGame)
                return;
            else if (!endGame && !compMove)
            {
                if ((hawtGame.grid[i, j] == CellSelection.N))                  // 
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        hawtGame.grid[i, j] = CellSelection.X;
                        if (i == 0)
                            userGuide[0]++;
                        if (i == 1)
                            userGuide[1]++;
                        if (i == 2)
                            userGuide[2]++;
                        if (j == 0)
                            userGuide[3]++;
                        if (j == 1)
                            userGuide[4]++;
                        if (j == 2)
                            userGuide[5]++;
                        if ((i == 0 && j == 0) || (i == 2 && j == 2))
                            userGuide[6]++;
                        if ((i == 0 && j == 2) && (i == 2 && j == 0))
                            userGuide[7]++;
                        if (i == 1 && j == 1)
                        {
                            userGuide[6]++;
                            userGuide[7]++;
                        }
                        this.moveCount++;
                        this.compMove = true;
                        this.isWinner(hawtGame);
                        this.compTurn(hawtGame);
                        
                    }
                }
                else
                {
                    MessageBox.Show("Invalid! Cell Occupied!");
                }
            }
        }

        public void compTurn(GameEngine hawtGame)
        {
            
            bool userMove = false;
            if (endGame)
            {
                compMove = false;
                isWinner(hawtGame);
                return;
            }
            else if (!compMove_1 && !endGame)
            {
                // check for diag wins first
                if (compGuide[6] == 2)  // diag 1
                {
                    if (hawtGame.grid[0, 0] == hawtGame.grid[1, 1] && hawtGame.grid[0, 0] != CellSelection.N)              // try for diag one (top left to bottom right)
                    {
                        if (hawtGame.grid[2, 2] == CellSelection.N)
                        {
                            hawtGame.grid[2, 2] = CellSelection.O;
                            compGuide[6]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if (hawtGame.grid[0, 0] == hawtGame.grid[2, 2] && hawtGame.grid[0, 0] != CellSelection.N)              // try for diag one
                    {
                        if (hawtGame.grid[1, 1] == CellSelection.N)
                        {
                            hawtGame.grid[1, 1] = CellSelection.O;
                            compGuide[6]++;        //diag 1 possible
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }

                    }
                    if (hawtGame.grid[2, 2] == hawtGame.grid[1, 1] && hawtGame.grid[2, 2] != CellSelection.N)              // try for diag one
                    {
                        if (hawtGame.grid[0, 0] == CellSelection.N)
                        {
                            hawtGame.grid[0, 0] = CellSelection.O;
                            compGuide[6]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                // diag 2
                if (compGuide[7] == 2)
                {
                    if (hawtGame.grid[2, 0] == hawtGame.grid[1, 1] && hawtGame.grid[2, 0] != CellSelection.N)              // try for diag two (bottom left to top right)
                    {
                        if (hawtGame.grid[0, 2] == CellSelection.N)
                        {
                            hawtGame.grid[0, 2] = CellSelection.O;
                            compGuide[7]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }

                    }
                    if (hawtGame.grid[2, 0] == hawtGame.grid[0, 2] && hawtGame.grid[2, 0] != CellSelection.N)              // try for diag two
                    {
                        if (hawtGame.grid[1, 1] == CellSelection.N)
                        {
                            hawtGame.grid[1, 1] = CellSelection.O;
                            compGuide[7]++;        //diag 2 possible
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }

                    }
                    if (hawtGame.grid[1, 1] == hawtGame.grid[0, 2] && hawtGame.grid[1, 1] != CellSelection.N)              // try for diag two
                    {
                        if (hawtGame.grid[2, 0] == CellSelection.N)
                        {
                            hawtGame.grid[2, 0] = CellSelection.O;
                            compGuide[7]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }

                    }
                }
                // row zero
                if (compGuide[0] == 2)
                {
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[0, 1]) && hawtGame.grid[0, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 2] == CellSelection.N)
                        {
                            hawtGame.grid[0, 2] = CellSelection.O;                          // finish row
                            compGuide[0]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[0, 2]) && hawtGame.grid[0, 2] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 1] == CellSelection.N)
                        {
                            hawtGame.grid[0, 1] = CellSelection.O;                          // finish row
                            compGuide[0]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[0, 1] == hawtGame.grid[0, 2]) && hawtGame.grid[0, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 0] == CellSelection.N)
                        {
                            hawtGame.grid[0, 0] = CellSelection.O;                          // finish row
                            compGuide[0]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (compGuide[1] == 2)                  //  check row 1
                {
                    // row1
                    if ((hawtGame.grid[1, 0] == hawtGame.grid[1, 1]) && hawtGame.grid[1, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 2] == CellSelection.N)
                        {
                            hawtGame.grid[1, 2] = CellSelection.O;                          // finish row
                            compGuide[1]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[1, 0] == hawtGame.grid[1, 2]) && hawtGame.grid[1, 2] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 1] == CellSelection.N)
                        {
                            hawtGame.grid[1, 1] = CellSelection.O;                          // finish row
                            compGuide[1]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    else if ((hawtGame.grid[1, 1] == hawtGame.grid[1, 2]) && hawtGame.grid[1, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 0] == CellSelection.N)
                        {
                            hawtGame.grid[1, 0] = CellSelection.O;                          // finish row
                            compGuide[1]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (compGuide[2] == 2)                                                  // row2
                {
                    if ((hawtGame.grid[2, 0] == hawtGame.grid[2, 1]) && hawtGame.grid[2, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 2] == CellSelection.N)
                        {
                            hawtGame.grid[2, 2] = CellSelection.O;                          // finish row
                            compGuide[2]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[2, 0] == hawtGame.grid[2, 2]) && hawtGame.grid[2, 2] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 1] == CellSelection.N)
                        {
                            hawtGame.grid[2, 1] = CellSelection.O;                          // finish row
                            compGuide[2]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[2, 1] == hawtGame.grid[2, 2]) && hawtGame.grid[2, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 0] == CellSelection.N)
                        {
                            hawtGame.grid[2, 0] = CellSelection.O;                          // finish row
                            compGuide[2]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (compGuide[3] == 2)
                {
                    // col0
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[1, 0]) && hawtGame.grid[1, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 0] == CellSelection.N)
                        {
                            hawtGame.grid[2, 0] = CellSelection.O;                          // finish col
                            compGuide[3]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[2, 0]) && hawtGame.grid[0, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 0] == CellSelection.N)
                        {
                            hawtGame.grid[1, 0] = CellSelection.O;                          // finish col
                            compGuide[3]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[1, 0] == hawtGame.grid[2, 0]) && hawtGame.grid[1, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 0] == CellSelection.N)
                        {
                            hawtGame.grid[0, 0] = CellSelection.O;                          // finish col
                            compGuide[3]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (compGuide[4] == 2)              // col1
                {
                    if ((hawtGame.grid[0, 1] == hawtGame.grid[1, 1]) && hawtGame.grid[1, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 0] == CellSelection.N)
                        {
                            hawtGame.grid[2, 0] = CellSelection.O;                          // finish col
                            compGuide[4]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            return;
                        }
                    }
                    if ((hawtGame.grid[0, 1] == hawtGame.grid[2, 1]) && hawtGame.grid[0, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 1] == CellSelection.N)
                        {
                            hawtGame.grid[1, 1] = CellSelection.O;                          // finish col
                            compGuide[4]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[1, 1] == hawtGame.grid[2, 1]) && hawtGame.grid[1, 1] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 1] == CellSelection.N)
                        {
                            hawtGame.grid[0, 1] = CellSelection.O;                          // finish col
                            compGuide[4]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (compGuide[5] == 2)
                {
                    // col2
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[1, 0]) && hawtGame.grid[1, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[2, 0] == CellSelection.N)
                        {
                            hawtGame.grid[2, 0] = CellSelection.O;                          // finish col
                            compGuide[5]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[0, 0] == hawtGame.grid[2, 0]) && hawtGame.grid[0, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[1, 0] == CellSelection.N)
                        {
                            hawtGame.grid[1, 0] = CellSelection.O;                          // finish col
                            compGuide[5]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                    if ((hawtGame.grid[1, 0] == hawtGame.grid[2, 0]) && hawtGame.grid[1, 0] != CellSelection.N)            // first two are equal
                    {
                        if (hawtGame.grid[0, 0] == CellSelection.N)
                        {
                            hawtGame.grid[0, 0] = CellSelection.O;                          // finish col
                            compGuide[5]++;
                            userMove = true;
                            compMove = false;
                            this.moveCount++;
                            isWinner(hawtGame);
                            return;
                        }
                    }
                }
                if (hawtGame.grid[1, 1] == GameEngine.CellSelection.N)  // if can't win, try to fill diagonals
                {           // center
                    hawtGame.grid[1, 1] = GameEngine.CellSelection.O;
                    compGuide[1]++;
                    compGuide[4]++;
                    compGuide[6]++;
                    compGuide[7]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[0, 0] == GameEngine.CellSelection.N)
                {            // top left
                    hawtGame.grid[0, 0] = GameEngine.CellSelection.O;
                    compGuide[0]++;
                    compGuide[3]++;
                    compGuide[6]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[2, 2] == GameEngine.CellSelection.N)
                {            // bottom right
                    hawtGame.grid[2, 2] = GameEngine.CellSelection.O;
                    compGuide[2]++;
                    compGuide[5]++;
                    compGuide[6]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[0, 2] == GameEngine.CellSelection.N)
                {           // top right
                    hawtGame.grid[0, 2] = GameEngine.CellSelection.O;
                    compGuide[0]++;
                    compGuide[5]++;
                    compGuide[7]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[2, 0] == GameEngine.CellSelection.N)
                {            // bottom left
                    hawtGame.grid[2, 0] = GameEngine.CellSelection.O;
                    compGuide[2]++;
                    compGuide[3]++;
                    compGuide[7]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                // fill the randos if the corners and center are filled
                if (hawtGame.grid[0, 1] == GameEngine.CellSelection.N)
                {            // top mid
                    hawtGame.grid[0, 1] = GameEngine.CellSelection.O;
                    compGuide[0]++;
                    compGuide[4]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[2, 1] == GameEngine.CellSelection.N)
                {            // bottom mid
                    hawtGame.grid[2, 1] = GameEngine.CellSelection.O;
                    compGuide[2]++;
                    compGuide[4]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[1, 0] == GameEngine.CellSelection.N)
                {            // mid left
                    hawtGame.grid[1, 0] = GameEngine.CellSelection.O;
                    compGuide[1]++;
                    compGuide[3]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (hawtGame.grid[1, 2] == GameEngine.CellSelection.N)
                {            // mid right
                    hawtGame.grid[1, 2] = GameEngine.CellSelection.O;
                    compGuide[1]++;
                    compGuide[5]++;
                    userMove = true;
                    compMove = false;
                    this.moveCount++;
                    isWinner(hawtGame);
                    return;
                }
                if (userMove)
                {
                    compMove = false;
                    return;
                }
            }
            else if (compMove_1 && !endGame)                                // comp first move set
            {
                hawtGame.grid[1, 1] = GameEngine.CellSelection.O;
                userMove = true;
                compMove = false;
                compMove_1 = false;
                hawtGame.moveCount++;
                isWinner(hawtGame);
                return;
            }
            else if (endGame)
                return;
            
        }

        public void isWinner(GameEngine hawtGame)
        {
            if (!endGame)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (compGuide[i] == 3)
                    {
                        compW = true;
                    }
                    if (userGuide[i] == 3)
                    {
                        userW = true;
                    }
                }
            }
            if (userW && !compW)
            {
                endGame = true;
                MessageBox.Show("You Win!");
            }
            if (!userW && compW)
            {
                endGame = true;
                MessageBox.Show("You Lose!");
            }
            if (hawtGame.moveCount == 9 && !endGame)
            {
                tieGame = true;
                MessageBox.Show("Tie!");
                endGame = true;
            }
        }
        public void DrawX(int i, int j, Graphics g)
        {
            g.DrawLine(Pens.Black, i * BLOCK + DELTA, j * BLOCK + DELTA,
                (i * BLOCK) + BLOCK - DELTA, (j * BLOCK) + BLOCK - DELTA);
            g.DrawLine(Pens.Black, (i * BLOCK) + BLOCK - DELTA, j * BLOCK + DELTA, (i * BLOCK) + DELTA, (j * BLOCK) + BLOCK - DELTA);
        }
        // fn to draw o's
        public void DrawO(int i, int j, Graphics g)
        {
            g.DrawEllipse(Pens.Black, i * BLOCK + DELTA, j * BLOCK + DELTA,
                BLOCK - 2 * DELTA, BLOCK - 2 * DELTA);
        }

    }
}
