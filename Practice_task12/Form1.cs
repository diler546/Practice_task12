using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practice_task12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitField();
            GetStateSudoku();
        }

        private TextBox[,] _textBoxes = new TextBox[9, 9];

        public void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 3);
            for (int i = 0; i < 4; i++)
            {
                e.Graphics.DrawLine(pen, 3, 94 * (i * 3) + 3, 851, 94 * (i * 3) + 3);
            }
            for (int i = 0; i < 4; i++)
            {
                e.Graphics.DrawLine(pen, 94 * (i * 3) + 3, 3, 94 * (i * 3) + 3, 850);
            }
        }

        private void InitField()
        {
            var font = new Font("Times New Roman", 60);
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    var tb = new TextBox()
                    {
                        Font = font,
                        MaxLength = 1,
                        Multiline = true,
                        Size = new Size(91, 91),
                        TextAlign = HorizontalAlignment.Center,
                        Location = new Point((91 + 3) * i + 5, (91 + 3) * j + 5),
                        BorderStyle = BorderStyle.None
                    };
                    tb.KeyPress += ValidateChar;
                    tb.TextChanged += ValidateField;
                    _textBoxes[i, j] = tb;
                    Controls.Add(_textBoxes[i, j]);
                }
            }
        }

        private void CaptureField(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_textBoxes[i, j].Text != "")
                    {
                        _textBoxes[i, j].Enabled = false;
                    }
                }
            }
        }

        public void SaveStateSudoku(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("save.txt");

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_textBoxes[i, j].Text == "")
                    {
                        sw.Write(' ');
                    }
                    else
                    {
                        sw.Write(_textBoxes[i, j].Text);
                    }
                }
            }

            sw.Close();
        }

        public void GetStateSudoku()
        {
            StreamReader sr = new StreamReader("save.txt");

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var sym = sr.Read();
                    if (sym != ' ')
                    {
                        _textBoxes[i, j].Text = (sym - '0').ToString();
                    }
                }
            }

            sr.Close();
        }
        public void ResetStateSudoku(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите польностью очистить судоку, а также очистить файл с сохранением?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        _textBoxes[i, j].Text = " ";
                    }
                }

                StreamWriter sw = new StreamWriter("save.txt");
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        sw.Write(" ");
                    }
                }
                sw.Close();
            }
        }

        private static void ValidateChar(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '0' || e.KeyChar > '9') && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void ValidateField(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (CheckHorizontal(i, j) &&
                        CheckVertical(i, j) &&
                        CheckSquare(i, j))
                    {
                        _textBoxes[i, j].ForeColor = Color.Black;
                        CheckVertFullness(i);
                        CheckHorzFullness(j);
                        CheckSquareFullness(i, j);
                    }
                    else
                    {
                        _textBoxes[i, j].ForeColor = Color.Red;
                    }
                }
            }
        }

        private bool CheckHorizontal(int x, int y)
        {
            string value = _textBoxes[x, y].Text;
            for (int i = 0; i < 9; i++)
            {
                if (i == x)
                    continue;

                if (_textBoxes[i, y].Text == value)
                    return false;
            }
            return true;
        }

        private bool CheckVertical(int x, int y)
        {
            string value = _textBoxes[x, y].Text;
            for (int i = 0; i < 9; i++)
            {
                if (i == y)
                    continue;

                if (_textBoxes[x, i].Text == value)
                    return false;
            }
            return true;
        }

        private bool CheckSquare(int x, int y)
        {
            int x1 = (int)Math.Floor((double)x / 3) * 3;
            int y1 = (int)Math.Floor((double)y / 3) * 3;
            string value = _textBoxes[x, y].Text;
            for (int i = x1; i < x1 + 3; i++)
            {
                for (int j = y1; j < y1 + 3; j++)
                {
                    if (i == x || j == y)
                        continue;

                    if (_textBoxes[i, j].Text == value)
                        return false;
                }
            }
            return true;
        }

        private void CheckHorzFullness(int y)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_textBoxes[i, y].Text == "" ||
                    _textBoxes[i, y].ForeColor == Color.Red)
                {
                    return;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                _textBoxes[i, y].Enabled = false;
            }
        }

        private void CheckVertFullness(int x)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_textBoxes[x, i].Text == "" ||
                    _textBoxes[x, i].ForeColor == Color.Red)
                {
                    return;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                _textBoxes[x, i].Enabled = false;
            }
        }

        private void CheckSquareFullness(int x, int y)
        {
            int x1 = (int)Math.Floor((double)x / 3) * 3;
            int y1 = (int)Math.Floor((double)y / 3) * 3;
            for (int i = x1; i < x1 + 3; i++)
            {
                for (int j = y1; j < y1 + 3; j++)
                {
                    if (_textBoxes[i, j].Text == "" ||
                        _textBoxes[i, j].ForeColor == Color.Red)
                        return;
                }
            }

            for (int i = x1; i < x1 + 3; i++)
            {
                for (int j = y1; j < y1 + 3; j++)
                {
                    _textBoxes[i, j].Enabled = false;
                }
            }
        }
    }
}
