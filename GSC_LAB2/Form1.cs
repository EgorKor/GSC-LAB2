using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;

namespace GSC_LAB2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private List<Point> pointList1;//список точек - первого многоугольника
        private List<Point> pointList2;//второго
        private bool isFirstPolygon; //Флаг - рисуется первый многоугольник
        private bool isTSO;          //Флаг - следующий клик отрисует ТМО
        private Pen drawBorderPen;   
        private Pen paintingPen;
        private Pen cleaningPen;
        private int pointCounter1;//счётчик точек первого многоугольника
        private int pointCounter2;//второго

        private int tsoCode = 0;                //Код текущей ТМО
        private const int UNION_CODE = 0;       //Код объединения
        private const int INTERSECTION_CODE = 1;//Код пересечения
        private const int SYMMETRIC_DIFFERENCE_CODE = 2;//Код симм. разности
        private const int AB_DIFFERENCE_CODE = 3;//Код разности А к В
        private const int BA_DIFFERENCE_CODE = 4;//Код разности В к А


        public Form1()
        {
            InitializeComponent();
            this.Text = "ГСК Лабораторная #2";
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pointList1 = new List<Point>();
            pointList2 = new List<Point>();
            drawBorderPen = new Pen(Color.Black, 1);
            paintingPen = new Pen(Color.Green, 1);
            cleaningPen = new Pen(Color.White);
            colorComboBox.SelectedIndex = 0;
            tsoComboBox.SelectedIndex = 0;
            isFirstPolygon = true;
            isTSO = false;
        }


        /*Метод реализующий линейную интерполяцию
         применяется при закрашивании многоугольников*/
        private int calculateXonY(Point pt1, Point pt2, int y)
        {
            return (y - pt1.Y) * (pt2.X - pt1.X) / (pt2.Y - pt1.Y) + pt1.X;
        }


        /*Метод закрашивания многоугольников*/
        private void paintPolygon(List<Point> points)
        {

            int Ymin = points.Min(point => point.Y);
            int Ymax = points.Max(point => point.Y);
            Ymin = Math.Max(0, Ymin);
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            List<int> Xl = new List<int>();
            List<int> Xr = new List<int>();
            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                Xl.Clear();
                Xr.Clear();
                for (int i = 0; i < points.Count; i++)
                {
                    int k;
                    if (i < points.Count - 1)
                        k = i + 1;
                    else
                        k = 0;
                    if ((points.ElementAt(i).Y < Y && points.ElementAt(k).Y >= Y)
                        ||
                        (points.ElementAt(i).Y >= Y && points.ElementAt(k).Y < Y))
                    {
                        int X = calculateXonY(points.ElementAt(i), points.ElementAt(k), Y);
                        if (points.ElementAt(k).Y - points.ElementAt(i).Y > 0)
                            Xr.Add(X);
                        else
                            Xl.Add(X);
                    }
                }
                Xl.Sort();
                Xr.Sort();
                for (int i = 0; i < Xl.Count; i++)
                {
                    graphics.DrawLine(paintingPen, new Point(Xl.ElementAt(i), Y), new Point(Xr.ElementAt(i), Y));
                }
            }
        }

        /*Метод возвращающий пару списков - 
         списки левых и правых Х-сов в многоугольнике
         по заданной строке Y
         */
        private Tuple<List<int>, List<int>> getLists(int Y, List<Point> points)
        {
            List<int> Xb = new List<int>();
            for (int i = 0; i < points.Count; i++)
            {
                int k;
                if (i < points.Count - 1)
                    k = i + 1;
                else
                    k = 0;
                if ((points.ElementAt(i).Y < Y && points.ElementAt(k).Y >= Y)
                    ||
                    (points.ElementAt(i).Y >= Y && points.ElementAt(k).Y < Y))
                {
                    Xb.Add(calculateXonY(points.ElementAt(i), points.ElementAt(k), Y));
                }
            }
            Xb.Sort();
            List<int> Xr = new List<int>();
            List<int> Xl = new List<int>();
            for (int i = 0; i < Xb.Count; i++)
            {
                if (i % 2 == 0)
                    Xl.Add(Xb[i]);
                else
                    Xr.Add(Xb[i]);
            }
            return new Tuple<List<int>, List<int>>(Xl, Xr);
        }

        /*Алгоритм Теоретико-Множественных операций*/
        private void tsoAlgoritm()
        {
            int[] setQ = new int[2];
            List<M> mList = new List<M>();
            switch (tsoCode)
            {
                case UNION_CODE:
                    {
                        setQ[0] = 1;
                        setQ[1] = 3;
                        break;
                    }
                case INTERSECTION_CODE:
                    {
                        setQ[0] = 3;
                        setQ[1] = 3;
                        break;
                    }
                case SYMMETRIC_DIFFERENCE_CODE:
                    {
                        setQ[0] = 1;
                        setQ[1] = 2;
                        break;
                    }
                case AB_DIFFERENCE_CODE:
                    {
                        setQ[0] = 2;
                        setQ[1] = 2;
                        break;
                    }
                case BA_DIFFERENCE_CODE:
                    {
                        setQ[0] = 1;
                        setQ[1] = 1;
                        break;
                    }
            }
            int Ymax = Math.Max(pointList1.Max(point => point.Y), pointList2.Max(point => point.Y));
            int Ymin = Math.Min(pointList1.Min(point => point.Y), pointList2.Min(point => point.Y));
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            Ymin = Math.Max(Ymin, 0);
            /*Для каждой строки применяем алгоритм ТМО*/
            for (int Y = Ymin + 1; Y < Ymax; Y++)
            {
                mList.Clear();
                var listsA = getLists(Y, pointList1);
                var listsB = getLists(Y, pointList2);
                List<int> Xal = listsA.Item1;
                List<int> Xar = listsA.Item2;
                List<int> Xbl = listsB.Item1;
                List<int> Xbr = listsB.Item2;

                List<int> Xrr = new List<int>();
                List<int> Xrl = new List<int>();
                foreach (int x in Xal)
                {
                    mList.Add(new M(x, 2));
                }
                foreach (int x in Xar)
                {
                    mList.Add(new M(x, -2));
                }
                foreach (int x in Xbl)
                {
                    mList.Add(new M(x, 1));
                }
                foreach (int x in Xbr)
                {
                    mList.Add(new M(x, -1));
                }
                mList.Sort(Comparer<M>.Create((m1, m2) => m1.x - m2.x));
                int k = 0, m = 0, Q = 0;
                if (mList[0].x >= 0 && mList[0].dQ < 0)
                {
                    Xrl.Insert(0, 0);
                    Q = -mList[0].dQ;
                    k = 1;
                }
                foreach (M mElem in mList)
                {
                    int x = mElem.x;
                    int Qnew = Q + mElem.dQ;
                    if ((Q < setQ[0] || Q > setQ[1]) && (Qnew >= setQ[0] && Qnew <= setQ[1]))
                    {
                        Xrl.Insert(k, x);
                        k++;
                    }
                    if ((Q >= setQ[0] && Q <= setQ[1]) && (Qnew < setQ[0] || Qnew > setQ[1]))
                    {
                        Xrr.Insert(m, x);
                        m++;
                    }
                    Q = Qnew;
                }
                if (Q == setQ[0] || Q == setQ[1])
                    Xrr.Insert(m, pictureBox1.Width - 1);
                /*Блок отрисовки*/
                Xrr.Sort();
                Xrl.Sort();
                int Xmin = Math.Min(pointList1.Min(point => point.X), pointList2.Min(point => point.X));
                int Xmax = Math.Max(pointList1.Max(point => point.X), pointList2.Max(point => point.X));
                if (Xrr.Count == 0)
                {
                    graphics.DrawLine(cleaningPen, new Point(Xmin, Y), new Point(Xmax, Y));
                }
                if(Xrr.Count != 0)
                {
                    graphics.DrawLine(cleaningPen, new Point(Xmin, Y), new Point(Xrr[0],Y));
                }
                for (int i = 0; i < Xrr.Count; i++)
                {
                    graphics.DrawLine(paintingPen, new Point(Xrl[i], Y), new Point(Xrr[i], Y));
                    if(i != Xrr.Count - 1)
                    {
                        graphics.DrawLine(cleaningPen, new Point(Xrr[i], Y), new Point(Xrl[i + 1], Y));
                    }
                }
                if (Xrr.Count != 0)
                {
                    graphics.DrawLine(cleaningPen, new Point(Xrr[Xrr.Count - 1], Y), new Point(Xmax, Y));
                }
            }

        }



        /*Обработчик кликов мыши*/
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isFirstPolygon)
                {
                    pointList1.Add(new Point(e.X, e.Y));
                    pointCounter1++;
                }
                else
                {
                    pointList2.Add(new Point(e.X, e.Y));
                    pointCounter2++;
                }
            }
            else
            {
                if (isFirstPolygon && !isTSO)
                {
                    if (pointCounter1 > 2)
                    {
                        paintPolygon(pointList1);
                        isFirstPolygon = false;
                        return;
                    }
                }
                else
                {
                    if (isTSO)
                    {
                        isTSO = false;
                        tsoAlgoritm();
                        pointCounter1 = 0;
                        pointCounter2 = 0;
                        pointList1.Clear();
                        pointList2.Clear();
                        return;
                    }
                    if (pointCounter2 > 2)
                    {
                        paintPolygon(pointList2);
                        isFirstPolygon = true;
                        isTSO = true;
                        return;
                    }
                }
                MessageBox.Show("Вы не можете рисовать, т.к. точек меньше двух", "Ошибка");
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
            pointCounter1 = 0;
            pointCounter2 = 0;
            pointList1.Clear();
            pointList2.Clear();
            isFirstPolygon = true;
            isTSO = false;
        }


        private void drawPoint(int x, int y)
        {
            graphics.DrawEllipse(drawBorderPen, x - 2, y - 2, 5, 5);
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
        private void tsoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsoCode = tsoComboBox.SelectedIndex;
        }
    }
}

//Структура М - вспомогательная структура для алгоритма ТМО
struct M
{
    public int x;
    public int dQ;

    public M(int x, int dQ)
    {
        this.x = x;
        this.dQ = dQ;
    }
}