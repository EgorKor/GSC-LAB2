namespace GSC_LAB2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private List<Point> pointList;
        private Pen drawBorderPen;
        private Pen paintingPen;
        private int pointCounter;
        private int algoritmCode;
        private const int ORIENTED_ALGORITHM = 0;
        private const int NON_ORIENTED_ALGORITHM = 1;

        private int visualMode;
        private const int BORDER_MODE = 0;
        private const int NON_BORDER_MODE = 1;

        private const int UNION_CODE = 0;
        private const int INTERSECTION_CODE = 1;
        private const int SYMMETRIC_DIFFERENCE_CODE = 2;
        private const int AB_DIFFERENCE_CODE = 3;
        private const int BA_DIFFERENCE_CODE = 4;


        public Form1()
        {
            InitializeComponent();
            this.Text = "ГСК Лабораторная #2";
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pointList = new List<Point>();
            drawBorderPen = new Pen(Color.Black, 1);
            paintingPen = new Pen(Color.Green, 1);
            algoritmChoseComboBox.SelectedIndex = 0;
            colorComboBox.SelectedIndex = 0;
            visualModeComboBox.SelectedIndex = 0;
        }

        private void paint_with__non_oriented_algoritm()
        {
            int Ymin = pointList.Min(point => point.Y);
            int Ymax = pointList.Max(point => point.Y);
            Ymin = Math.Max(Ymin, 0);
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            List<int> Xb = new List<int>();
            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                Xb.Clear();
                for (int i = 0; i < pointCounter; i++)
                {
                    int k;
                    if (i < pointCounter - 1)
                        k = i + 1;
                    else
                        k = 0;

                    if ((pointList.ElementAt(i).Y < Y && pointList.ElementAt(k).Y >= Y)
                         ||
                         (pointList.ElementAt(i).Y >= Y && pointList.ElementAt(k).Y < Y))
                    {
                        Xb.Add(calculateXonY(pointList.ElementAt(i), pointList.ElementAt(k), Y));
                    }
                }
                Xb.Sort();
                for (int j = 0; j < Xb.Count; j += 2)
                {
                    graphics.DrawLine(paintingPen, new Point(Xb.ElementAt(j), Y), new Point(Xb.ElementAt(j + 1), Y));
                }
            }
        }

        private int calculateXonY(Point pt1, Point pt2, int y)
        {
            return (y - pt1.Y) * (pt2.X - pt1.X) / (pt2.Y - pt1.Y) + pt1.X;
        }

        private void paint_with_oriented_algoritm()
        {

            int Ymin = pointList.ElementAt(0).Y;
            int Ymax = pointList.ElementAt(0).Y;
            int j = 0;
            for (int i = 0; i < pointList.Count; i++)
            {
                if (pointList.ElementAt(i).Y > Ymax)
                {
                    Ymax = pointList.ElementAt(i).Y;
                    j = i;
                }
                if (pointList.ElementAt(i).Y < Ymin)
                    Ymin = pointList.ElementAt(i).Y;
            }
            Ymin = Math.Max(0, Ymin);
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            int j_minus_one = j == 0 ? pointList.Count - 1 : j - 1;
            int j_plus_one = j == pointList.Count - 1 ? 0 : j + 1;
            int S = detThree(new int[][] {
            new int[]{pointList.ElementAt(j_minus_one).X, pointList.ElementAt(j_minus_one).Y, 1},
            new int[]{pointList.ElementAt(j).X, pointList.ElementAt(j).Y, 1},
            new int[]{pointList.ElementAt(j_plus_one).X, pointList.ElementAt(j_plus_one).Y, 1} }) / 2;
            bool CW = S < 0 ? true : false;
            if (CW)
            {
                for (int Y = 0; Y < Ymin; Y++)
                {
                    graphics.DrawLine(paintingPen, new Point(0, Y), new Point(pictureBox1.Width - 1, Y));
                }
            }
            List<int> Xl = new List<int>();
            List<int> Xr = new List<int>();
            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                Xl.Clear();
                Xr.Clear();
                for (int i = 0; i < pointCounter; i++)
                {
                    int k;
                    if (i < pointCounter - 1)
                        k = i + 1;
                    else
                        k = 0;
                    if ((pointList.ElementAt(i).Y < Y && pointList.ElementAt(k).Y >= Y)
                        ||
                        (pointList.ElementAt(i).Y >= Y && pointList.ElementAt(k).Y < Y))
                    {
                        int X = calculateXonY(pointList.ElementAt(i), pointList.ElementAt(k), Y);
                        if (pointList.ElementAt(k).Y - pointList.ElementAt(i).Y > 0)
                            Xr.Add(X);
                        else
                            Xl.Add(X);
                    }
                }
                if (CW)
                {
                    Xl.Add(0);
                    Xr.Add(pictureBox1.Width - 1);
                }
                Xl.Sort();
                Xr.Sort();
                for (int i = 0; i < Xl.Count; i++)
                {
                    if (Xl.ElementAt(i) < Xr.ElementAt(i))
                    {
                        graphics.DrawLine(paintingPen, new Point(Xl.ElementAt(i), Y), new Point(Xr.ElementAt(i), Y));
                    }
                }
            }
            if (CW)
            {
                for (int Y = Ymax + 1; Y < pictureBox1.Height - 1; Y++)
                {
                    graphics.DrawLine(paintingPen, new Point(0, Y), new Point(pictureBox1.Width - 1, Y));
                }
            }
        }

        private int detThree(int[][] matrix)
        {
            return matrix[0][0] * matrix[1][1] * matrix[2][2]
                + matrix[1][0] * matrix[2][1] * matrix[0][2]
                + matrix[0][1] * matrix[1][2] * matrix[2][0]
                - matrix[0][2] * matrix[1][1] * matrix[2][0]
                - matrix[0][1] * matrix[2][2] * matrix[1][0]
                - matrix[0][0] * matrix[1][2] * matrix[2][1];
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pointList.Add(new Point(e.X, e.Y));
                if (pointCounter > 0)
                {
                    if (visualMode == BORDER_MODE)
                        graphics.DrawLine(drawBorderPen, pointList.ElementAt(pointCounter - 1), pointList.ElementAt(pointCounter));
                }
                if (visualMode == BORDER_MODE)
                    drawPoint(e.X, e.Y);
                pointCounter++;
            }
            else
            {
                if (pointCounter > 2)
                {
                    if (visualMode == BORDER_MODE)
                        graphics.DrawLine(drawBorderPen, pointList.ElementAt(0), pointList.ElementAt(pointCounter - 1));
                    switch (algoritmCode)
                    {
                        case ORIENTED_ALGORITHM:
                            paint_with_oriented_algoritm();
                            break;
                        case NON_ORIENTED_ALGORITHM:
                            paint_with__non_oriented_algoritm();
                            break;
                    }
                    pointCounter = 0;
                    pointList.Clear();
                }
                else
                {
                    MessageBox.Show("Вы не можете рисовать, т.к. точек меньше 2", "Ошибка");
                }
            }
        }

        private void formSizeChangedListener(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pointCounter = 0;
            pointList.Clear();
        }


        private void drawPoint(int x, int y)
        {
            graphics.DrawEllipse(drawBorderPen, x - 2, y - 2, 5, 5);
        }

        private void visualModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            visualMode = visualModeComboBox.SelectedIndex;
        }

        private void algoritmChoseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            algoritmCode = algoritmChoseComboBox.SelectedIndex;
        }

        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (colorComboBox.SelectedIndex)
            {
                case 0:
                    paintingPen.Color = Color.Green;
                    break;
                case 1:
                    paintingPen.Color = Color.Red;
                    break;
                case 2:
                    paintingPen.Color = Color.Yellow;
                    break;
                case 3:
                    paintingPen.Color = Color.Blue;
                    break;
            }
        }
    }
}