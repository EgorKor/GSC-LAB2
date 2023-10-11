namespace GSC_LAB2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;      //Объект графики, в котором происходит отрисовка
        private List<Point> pointList;  //Список точек
        private Pen drawBorderPen;      //Ручка для отрисовки границ
        private Pen paintingPen;        //Ручка для закрашивания
        private int pointCounter;       //Счётчик точек
        private int algoritmCode;       //Код выбранного алгоритма закрашивания
        private const int ORIENTED_ALGORITHM = 0;     //константа - ориентированный алгоритм
        private const int NON_ORIENTED_ALGORITHM = 1; //константа - неориентированный алгоритм

        private int visualMode;                //Код выбранного режима визуализации
        private const int BORDER_MODE = 0;     //Константа режима с границами
        private const int NON_BORDER_MODE = 1; //Константа режима без границ


        public Form1()
        {
            InitializeComponent();
            this.Text = "ГСК Лабораторная #2";
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pointList = new List<Point>();
            drawBorderPen = new Pen(Color.Black, 1);
            paintingPen = new Pen(Color.Green, 1);
            visualModeComboBox.SelectedIndex = 0;
            algoritmChoseComboBox.SelectedIndex = 0;
            colorComboBox.SelectedIndex = 0;
        }

        private void paint_with__non_oriented_algoritm()
        {
            /*Вычисляем Ymin и Ymax*/
            int Ymin = pointList.Min(point => point.Y);
            int Ymax = pointList.Max(point => point.Y);
            Ymin = Math.Max(Ymin, 0);
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            List<int> Xb = new List<int>();//список X-сов - пересечений многоугольника
            /*Закрашивание линий на которых лежит многоугольник*/
            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                Xb.Clear();//очищаем список
                /*Последовально просматривая все прямые
                 образующие многоугольник, находим X для точек пересечения
                 с текущей прямой по Y 
                 */
                for (int i = 0; i < pointCounter; i++)
                {
                    int k;
                    if (i < pointCounter - 1)
                        k = i + 1;
                    else
                        k = 0;

                    if ((pointList[i].Y < Y && pointList[k].Y >= Y) ||
                        (pointList[i].Y >= Y && pointList[k].Y < Y))
                    //Если прямая пересекает многоугольник
                    {
                        //Вычисляем Х пересечения и добавляем в список
                        Xb.Add(calculateXonY(pointList[i], pointList[k], Y));
                    }
                }
                Xb.Sort();//Сортируем список Х-сов
                /*Последовательно беря пары Х-сов из списков закрашиваем */
                for (int j = 0; j < Xb.Count; j += 2)
                {
                    graphics.DrawLine(paintingPen, new Point(Xb[j], Y), new Point(Xb.ElementAt(j + 1), Y));
                }
            }
        }

        /*  Метод вычисляющий точку 
            Х пересечения прямой по друм точкам
            с прямой заданной Y       
         */
        private int calculateXonY(Point pt1, Point pt2, int y)
        {
            return (y - pt1.Y) * (pt2.X - pt1.X) / (pt2.Y - pt1.Y) + pt1.X;
        }

        private void paint_with_oriented_algoritm()
        {
            /*Находим Ymax, Ymin, индекс j для вершины Ymax*/
            int Ymin = pointList[0].Y;
            int Ymax = pointList[0].Y;
            int j = 0;
            for (int i = 0; i < pointList.Count; i++)
            {
                if (pointList[i].Y > Ymax)
                {
                    Ymax = pointList[i].Y;
                    j = i;
                }
                if (pointList[i].Y < Ymin)
                    Ymin = pointList[i].Y;
            }
            Ymin = Math.Max(0, Ymin);
            Ymax = Math.Min(Ymax, pictureBox1.Height - 1);
            int j_minus_one = j == 0 ? pointList.Count - 1 : j - 1;//задаем индекс j - 1
            int j_plus_one = j == pointList.Count - 1 ? 0 : j + 1; //задаем индекс j + 1
            int S = detThree(new int[][] {//вычисляем площадь через опредитель
                new int[]{pointList[j_minus_one].X, pointList[j_minus_one].Y, 1},
                new int[]{pointList[j].X, pointList[j].Y, 1},
                new int[]{pointList[j_plus_one].X, pointList[j_plus_one].Y, 1} }) / 2;
            bool CW = S < 0 ? true : false;//Флаг ориентированности
            if (CW)//Если ориентация против часовой
                paintArea(0, Ymin);//закрашиваем верхнюю область до многоугольника
            List<int> Xl = new List<int>();//список левых Х-сов
            List<int> Xr = new List<int>();//список правых Х-сов
            /*Закрашивание линий на которых лежит многоугольник*/
            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                Xl.Clear();
                Xr.Clear();
                /*Последовально просматривая все прямые
                  образующие многоугольник, находим X для точек пересечения
                  с текущей прямой по Y 
                 */
                for (int i = 0; i < pointCounter; i++)
                {
                    int k;
                    if (i < pointCounter - 1)
                        k = i + 1;
                    else
                        k = 0;
                    if ((pointList[i].Y < Y && pointList[k].Y >= Y)
                        ||
                        (pointList[i].Y >= Y && pointList[k].Y < Y))
                    {//Если прямая пересекает многоугольник
                        //Вычисляем Х пересечения 
                        int X = calculateXonY(pointList[i], pointList[k], Y);
                        if (pointList[k].Y - pointList[i].Y > 0)//Если Х левый
                            Xr.Add(X);
                        else //Если Х левый
                            Xl.Add(X);
                    }
                }
                if (CW)//Если ориентация против часовой
                {
                    Xl.Add(0);//Добавляем X - начало области рисования
                    Xr.Add(pictureBox1.Width - 1);//Добавляем X - конец области рисования
                }
                Xl.Sort();//Сортируем список левых Х
                Xr.Sort();//Сортируем список правых Х
                /*Последовательно беря пары Х-сов из списков закрашиваем*/
                for (int i = 0; i < Xl.Count; i++)
                {
                    if (Xl[i] < Xr[i])
                    {
                        graphics.DrawLine(paintingPen, new Point(Xl[i], Y), new Point(Xr[i], Y));
                    }
                }
            }
            if (CW)//Если ориентация - против часовой
                //Закрашиваем нижнюю область под многоугольником
                paintArea(Ymax + 1, pictureBox1.Height - 1);
        }

        /*Метод вычисления определителя 3-го порядка*/
        private int detThree(int[][] matrix)
        {
            return matrix[0][0] * matrix[1][1] * matrix[2][2] +
                 matrix[1][0] * matrix[2][1] * matrix[0][2] +
                 matrix[0][1] * matrix[1][2] * matrix[2][0] -
                 matrix[0][2] * matrix[1][1] * matrix[2][0] -
                 matrix[0][1] * matrix[2][2] * matrix[1][0] -
                 matrix[0][0] * matrix[1][2] * matrix[2][1];
        }
        /*Метод закраски области рисования по двум заданным Y-кам*/
        private void paintArea(int Ybegin, int Yend)
        {
            for (int Y = Ybegin; Y < Yend; Y++)
            {
                graphics.DrawLine(paintingPen, new Point(0, Y), new Point(pictureBox1.Width - 1, Y));
            }
        }

        /*Обработчик события - нажатие мыши на области рисования*/
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//Если нажата ЛКМ
            {
                pointList.Add(new Point(e.X, e.Y));//Добавляем точку в список вершин многоугольника
                if (pointCounter > 0)//Если точек больше нуля
                {
                    if (visualMode == BORDER_MODE)//Если режим с визуализацией - рисуем границы
                        graphics.DrawLine(drawBorderPen, pointList.ElementAt(pointCounter - 1), pointList[pointCounter]);
                }
                if (visualMode == BORDER_MODE)//Если режим с визуализацией - рисуем точки
                    drawPoint(e.X, e.Y);
                pointCounter++;//изменяем счётчик точек
            }
            else//Если нажата ПКМ
            {
                if (pointCounter > 2)//Если достаточно точек для закрашивания
                {
                    if (visualMode == BORDER_MODE)
                        graphics.DrawLine(drawBorderPen, pointList.ElementAt(0), pointList.ElementAt(pointCounter - 1));
                    switch (algoritmCode)//проверяем какой алгоритм закрашивания выбран
                    {
                        case ORIENTED_ALGORITHM:
                            /*рисуем методом для ориентированных многоугольников*/
                            paint_with_oriented_algoritm();
                            break;
                        case NON_ORIENTED_ALGORITHM:
                            /*рисуем методом для неориентированных многоугольников*/
                            paint_with__non_oriented_algoritm();
                            break;
                    }
                    /*После отрисовки*/
                    pointCounter = 0;//зануляем счётчик точек
                    pointList.Clear();//очищаем список точек
                }
                else//Если недостаточно точек для рисования
                {
                    MessageBox.Show("Вы не можете рисовать, т.к. точек меньше 2", "Ошибка");
                }
            }
        }

        /*Обработчик события - изменение размера формы*/
        private void formSizeChangedListener(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pointCounter = 0;
            pointList.Clear();
        }

        /*Обработчик событие - нажатие на кнопку "Очистить"*/
        private void btClear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pointCounter = 0;
            pointList.Clear();
        }

        /*Метод рисования точки*/
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