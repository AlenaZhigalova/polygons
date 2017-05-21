using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Collections;

using System.Diagnostics;

namespace Mnogougol
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        public struct PointD
        {
            public double X;
            public double Y;

            public PointD(double x, double y)
            {
                X = x;
                Y = y;
            }

            public Point ToPoint()
            {
                return new Point((int)X, (int)Y);
            }

            public override bool Equals(object obj)
            {
                return obj is PointD && this == (PointD)obj;
            }
            public override int GetHashCode()
            {
                return X.GetHashCode() ^ Y.GetHashCode();
            }
            public static bool operator ==(PointD a, PointD b)
            {
                return a.X == b.X && a.Y == b.Y;
            }
            public static bool operator !=(PointD a, PointD b)
            {
                return !(a == b);
            }
        }
        public static T[] Shift<T>(T[] array, int shiftValue)
        {
            var newArray = new T[array.Length];
            shiftValue -= array.Length;
            if (shiftValue < 0)
            {
                shiftValue *= -1;
            }


            for (var i = 0; i < array.Length; i++)
            {
                var index = (i + shiftValue) % array.Length;

                newArray[i] = array[index];
            }
            return newArray;
        }
        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Minimum = 3; //выставляем минимум количества точек
            numericUpDown1.Maximum = 50; //выставляем максимум количества точек
            minimum.Minimum = 1; //минимальный размер ребра
          maximum.Minimum = 1; //минимальный размер ребра
        }
        public void star(int n)
        {
            double eps = 0.01; //отклонение от точек,чтобы не попадать на них при рандоме
             int length; //отдаление от центра
            int wid=pictureBox1.Width; //размер полотна(высота)
            int hei = pictureBox1.Height; //размер полотна(длина)
            bitmap = new Bitmap(wid, hei); //создаём битмап
            Graphics gr = Graphics.FromImage(bitmap); //привязываем битмак к объекту график
            gr.FillRectangle(Brushes.White, 0, 0, wid, hei); //заполняем белым
            Random random = new Random();
            PointF[] point = new PointF[n];
            int[] alpha = new int[n];
            alpha[0] = 0;
            for (int i = 1; i < n; i++)
            {
              if ((alpha[i - 1] + 180) >= 360)
                {
                   alpha[i] = random.Next(alpha[i - 1]+1, 359);
                  while  ( (360-alpha[i])/(n-i) <= 1)//остается меньше чем по градусу на угол
                      alpha[i] = random.Next(alpha[i - 1] + 1, 359);              
                }
                else
                {
                    alpha[i] = random.Next(alpha[i - 1]+1, alpha[i - 1] + 179);
                    while  ( (360-alpha[i]) / (n-i) <= 1)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                }
              if (i == n - 1)
              {
                  while ( 360 - alpha[i] >= 180 )
                      alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);

              }
            }
            length = 150;
            for (int i = 0; i < n; i++)
                point[i] = new PointF(Convert.ToSingle(wid / 2 + Math.Cos(-(alpha[i]) * Math.PI / 180) * length),
                                     Convert.ToSingle(hei / 2 + Math.Sin(-(alpha[i]) * Math.PI / 180) * length));  //пусть это S
            //пересортируем всё
            int index = random.Next(0, n);//теперь k тот луч с которог опроисходят изменения
            point =  Shift(point,n-index);       
            PointF[] polygon = new PointF[n];
            PointF[] help = new PointF[n+1];
            PointF[] help2 = new PointF[n + 1];
            PointF[] help3 = new PointF[n + 1];
            int k = -1;
            int gde = -1;
            double a1 ;
           PointF O = new PointF(wid / 2, hei / 2);
           label1:
           bool flag = false;
            
                for ( int i = 0 ; i<1 ; i++)
                {
                         if (O.X == point[i].X)
                                {
                                  a1 = 0;
                                }          
                              else a1 = (O.Y - point[i].Y) / (O.X - point[i].X); //наклон луча OSi
                         double b1 = point[i].Y - a1 * point[i].X;
                     //генерируем новую точку A1 или A0
                         double ax1;        
                         if (O.X < point[i].X)
                             ax1 = GetRandomNumber(Convert.ToDouble(O.X)+eps, Convert.ToDouble(point[i].X)-eps);
                         else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X)+eps, Convert.ToDouble(O.X) - eps);
                         double ay1 = a1 * ax1 + b1;
                         polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
                }
            //A1 выберем так чтобы A2 пыталось нарушить
                double helpax ;
                double helpa;
                if (point[2].X == O.X) { helpa = 0; }
                else helpa = (point[2].Y - O.Y) / (point[2].X - O.X); //наклон луча OS2
                double helpb = point[2].Y - helpa * point[2].X;
            if (O.X<point[2].X)
                helpax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[2].X) - eps);//выбрали нечто на OS2
            else helpax = GetRandomNumber(Convert.ToDouble(point[2].X) + eps, Convert.ToDouble(O.X) - eps);
            double helpay = helpa * helpax + helpb;
            //g - перечесение OS1 и A0hel
              PointF g = new PointF(Convert.ToSingle(((polygon[0].X * helpay - polygon[0].Y * helpax) * (wid / 2 - point[1].X) - (polygon[0].X - helpax) * (wid / 2 * point[1].Y - hei / 2 * point[1].X))
                       / ((polygon[0].X - helpax) * (-point[1].Y + hei / 2) - (polygon[0].Y - helpay) * (-point[1].X + wid / 2))), Convert.ToSingle
                       (((polygon[0].X * helpay- polygon[0].Y * helpax) * (hei / 2 - point[1].Y) - (polygon[0].Y -helpay) * (wid / 2 * point[1].Y - hei / 2 * point[1].X))
                       / ((polygon[0].X - helpax) * (-point[1].Y + hei / 2) - (polygon[0].Y - helpay) * (-point[1].X + wid / 2))));
              if (((g.X > point[1].X) & (g.X < O.X)) || ((g.X < point[1].X) & (g.X > O.X)))//  лежит на OS1
                  polygon[1] = g;
              else//не лежит, нет пересечений - любое
              {
                  if (O.X == point[1].X)
                  {
                      a1 = 0;
                  }
                  else a1 = (O.Y - point[1].Y) / (O.X - point[1].X); //наклон луча OS1
                  double b1 = point[1].Y - a1 * point[1].X;
                  //генерируем новую точку A1 или A0
                  double ax1;
                  if (O.X < point[1].X)
                      ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[1].X) - eps);
                  else ax1 = GetRandomNumber(Convert.ToDouble(point[1].X) + eps, Convert.ToDouble(O.X) - eps);
                  double ay1 = a1 * ax1 + b1;
                  polygon[1] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
              }              
            //проверим A2
            //f точка пересечения А0А1 и OS2
                PointF f = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[2].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[2].Y - hei / 2 * point[2].X))
                       / ((polygon[0].X - polygon[1].X) * (-point[2].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[2].X + wid / 2))), Convert.ToSingle
                       (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[2].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[2].Y - hei / 2 * point[2].X))
                       / ((polygon[0].X - polygon[1].X) * (-point[2].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[2].X + wid / 2))));
                if (((f.X > point[2].X) & (f.X < O.X)) || ((f.X < point[2].X) & (f.X > O.X)))//  лежит на OS2
                {                      //выберем A2 нарушая границу от f 
                    flag = true;
                    gde = 2;
                    double a;
                    if (point[2].X == f.X) { a = 0; }
                    else a = (point[2].Y - f.Y) / (point[2].X - f.X); //наклон луча OSi
                    double b = f.Y - a * f.X;
                    double ax;
                    if (point[2].X < f.X)
                        ax = GetRandomNumber(point[2].X + eps, Convert.ToDouble(f.X) - eps);
                    else ax = GetRandomNumber(Convert.ToDouble(f.X) + eps, point[2].X - eps);
                    double ay = a * ax + b;
                    k++;
                    help[k] = f;
                    // polygon[i] = d;
                    polygon[2] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                }
                else
                {
                    if (O.X == point[2].X)
                    {
                        a1 = 0;
                    }
                    else a1 = (O.Y - point[2].Y) / (O.X - point[2].X); //наклон луча OSi
                    double b1 = point[2].Y - a1 * point[2].X;
                    double ax1;
                    if (O.X < point[1].X)
                        ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[2].X) - eps);
                    else ax1 = GetRandomNumber(Convert.ToDouble(point[2].X) + eps, Convert.ToDouble(O.X) - eps);
                    double ay1 = a1 * ax1 + b1;
                    polygon[2] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
                }


                for (int i = 3; i < n; i++)
                {
                    if (!flag)// не было еще нарушений
                    //ищем любимые границы и нарушаем их
                    {
                        // d верхняя граница Ai-1Ai-2  и OSi
                        bool up = true;
                        bool down = true;
                        PointF d = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[i - 2].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                        / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                        (((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[i - 2].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                        / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))));

                        if (((d.X > point[i].X) & (d.X > O.X)) || ((d.X < point[i].X) & (d.X < O.X)))
                        {
                            d = point[i];
                            up = false;
                        }
                        // c точка пересчения A0Ai-1 и OSi
                        PointF c = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[0].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                            (((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[0].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))));

                        //S0 и Si лежат в разные стороны от OSi-1 => c - любое
                        if ((((point[i - 1].X - O.X) * (point[0].Y - O.Y) - ((point[0].X - O.X) * (point[i - 1].Y - O.Y))) * ((point[i - 1].X - O.X) * (point[i].Y - O.Y) - ((point[i].X - O.X) * (point[i - 1].Y - O.Y)))) < 0)
                        {
                            c = O;
                            down = false;
                        }

                          //пересечение A0A1 и OSi                      
                         PointF e = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[i].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                    (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[i].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))));
                         bool mid = true;

                             if (((e.X > point[i].X) & (e.X > O.X)) || ((e.X < point[i].X) & (e.X < O.X)))// не лежит на OSi
                             {
                                 mid = false;//e не важно 
                             }

                             if ((c.X < e.X)&(e.X<d.X) & (mid))
                                 d = e;
        


                        double a;
                        if (c.X == d.X) { a = 0;}
                        else a = (c.Y - d.Y) / (c.X - d.X); //наклон луча OSi
                        double b = c.Y - a * c.X;
                        //генерируем новую точку Ai
                        double ax;
                        //смотрим на up down если обоих нет то не нарушили, если один из них то нарушим его, если оба то любой
                        if ((up) & (down))
                        {
                            //нарушаем одну из них
                            int x = random.Next(0, 1);
                            if (x == 0)//нарушим низ с
                            {
                                if (O.X > c.X)
                                    ax = GetRandomNumber(Convert.ToDouble(c.X) + eps, Convert.ToDouble(O.X) - eps);
                                else ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(c.X) - eps);
                                double ay = a * ax + b;
                                polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                k++;
                                help[k] = c;
                            }
                            else//нарушим верх d
                            {
                                if (d.X > point[i].X)
                                    ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(d.X) - eps);
                                else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(point[i].X) - eps);
                                double ay = a * ax + b;
                                polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                k++;
                                help[k] = d;
                            }
                                flag = true;
                                gde = i;
                                
                        }
                        else
                            if ((!up) & (!down))
                            {
                                if (point[i].X > O.X)
                                    ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                                else ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                                double ay = a * ax + b;
                                polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                //ни один нельзя
                            }
                            else
                            {
                                     if (down)
                                {
                                    if (O.X > c.X)
                                        ax = GetRandomNumber(Convert.ToDouble(c.X) + eps, Convert.ToDouble(O.X) - eps);
                                    else ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(c.X) - eps);
                                    double ay = a * ax + b;
                                    polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                    flag = true;
                                    gde = i;
                                    k++;
                                    help[k] = c;
                                }
                                if (up)
                                {
                                    if (d.X > point[i].X)
                                        ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(d.X) - eps);
                                    else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(point[i].X) - eps);
                                    double ay = a * ax + b;
                                    polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                    flag = true;
                                    gde = i;
                                    k++;
                                    help[k] = d;
                                }
                             }
                    }


                    else//нарушена граница и идем дальше спокойно
                    {
                        if (O.X == point[i].X)
                        {
                            a1 = 0;
                        }
                        else a1 = (O.Y - point[i].Y) / (O.X - point[i].X);
                        double b1 = point[i].Y - a1 * point[i].X;
                        double ax1;
                        if (O.X < point[i].X)
                            ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                        else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                        double ay1 = a1 * ax1 + b1;
                        polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
                    }
                }
            if (!flag) goto label1;
            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {

                listBox1.Items.Add(Convert.ToString(i + 1) + ":" + Convert.ToString(point[i].X) + ";" + Convert.ToString(point[i].Y)); //вносим координаты в листбокс
            }
            if (beams.Checked)
            {
                for (int i = 0; i < n; i++)
                {
                    gr.DrawLine(new Pen(Color.Black, 2), new Point(wid / 2, hei / 2), point[i]);
                }
            }
         /*   for (int i = 0; i < help.Count(); i++)
            {
                gr.DrawRectangle(new Pen(Color.Orange, 2), help[i].X, help[i].Y, 5, 5);
            }*/

            gr.DrawPolygon(new Pen(Color.Red, 2), polygon);
            RectangleF rectf = new RectangleF(0, 0, 20, 20);
          //  string gdee = gde.ToString();
         //    gr.DrawString(gdee, new Font("Tahoma", 12), Brushes.Black, rectf);
            pictureBox1.BackgroundImage = bitmap; //ставим на нижний слой новый битмап
            pictureBox1.Image = new Bitmap(1, 1); //очищаем верхний слой с красными точками

        }
        public void gen(int n)
        {
            double eps = 0.01; //отклонение от точек,чтобы не попадать на них при рандоме
             int length; //отдаление от центра
            int wid=pictureBox1.Width; //размер полотна(высота)
            int hei = pictureBox1.Height; //размер полотна(длина)

            Random random = new Random();
            PointF[] point = new PointF[n];
            int[] alpha = new int[n];
            alpha[0] = 0;
            // Первый вариант генерации
            for (int i = 1; i < n; i++)
            {
              //  alpha[i] = random.Next(1, 179);
              if ((alpha[i - 1] + 180) >= 360)
                {
                   alpha[i] = random.Next(alpha[i - 1]+1, 359);
                  while  ( (360-alpha[i])/(n-i) <= 1)//остается меньше чем по градусу на угол
                      alpha[i] = random.Next(alpha[i - 1] + 1, 359);              
                }
                else
                {
                    alpha[i] = random.Next(alpha[i - 1]+1, alpha[i - 1] + 179);
                    while  ( (360-alpha[i]) / (n-i) <= 1)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                }
              if (i == n - 1)
              {
                  while ( 360 - alpha[i] >= 180 )
                      alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);

              }
            }

      //      alpha[n - 1] = 360 - alpha[n- 2];
            length = 150;
            for (int i = 0; i < n; i++)
                point[i] = new PointF(Convert.ToSingle(wid / 2 + Math.Cos(-(alpha[i]) * Math.PI / 180) * length),
                                     Convert.ToSingle(hei / 2 + Math.Sin(-(alpha[i]) * Math.PI / 180) * length));  //пусть это S
            PointF[] polygon = new PointF[n];
            PointF[] help = new PointF[n+1];
            PointF[] help2 = new PointF[n + 1];
            PointF[] help3 = new PointF[n + 1];
            int k = -1;
            int k2 = -1;
            int k3 = -1;
            double a1 ;
           PointF O = new PointF(wid / 2, hei / 2);
           // polygon[0] = point[0]; //легкий вариант
           
                for ( int i = 0 ; i<2 ; i++)
                {
            if (O.X == point[i].X)
            {
                a1 = 0;
            }          
            else a1 = (O.Y - point[i].Y) / (O.X - point[i].X); //наклон луча OSi
            double b1 = point[i].Y - a1 * point[i].X;
            //генерируем новую точку A1
            double ax1;
            
                if (O.X < point[i].X)
                    ax1 = GetRandomNumber(Convert.ToDouble(O.X)+eps, Convert.ToDouble(point[i].X)-eps);
                else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X)+eps, Convert.ToDouble(O.X) - eps);
           

            //   if (Math.Abs(ax) > point[i].X) ax = point[i].X;
            double ay1 = a1 * ax1 + b1;
            polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
                }
          //  polygon[1] = point[1];
           
            for (int i = 2; i < n; i++)
            {
                //проверка на параллельность
                //начнем не с первой 
                // d точка пересечения Ai-1Ai-2 и OSi
        
                {
                    PointF d = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[i - 2].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                    (((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[i - 2].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))));

                    if (((d.X > point[i].X) & (d.X > O.X)) || ((d.X < point[i].X) & (d.X < O.X)))  //( (((d.X-O.X)*(point[i].Y-O.Y)-(d.Y-O.Y)*(point[i].X-O.X)) != 0 ) ||  (((d.X > point[i].X)&(d.X > O.X)) || ((d.X<point[i].X)&(d.X<O.X))) )//(((( -wid/2 + point[i].X) *(-hei/2 + d.Y) - (- wid/2 + d.X) * ( -hei/2 + point[i].Y)) != 0) || ((-d.X+wid/2)*(-d.X+point[i].X)+((-d.Y+hei/2)*(-d.Y+point[i].Y))>0))
                    {
                        //d приналджеит OSi
                        //сли d не принадлежит OSi т.е перечение далекоооо, то берем d просто любую (S i )
                        //  1. [OSi, Od] = 0 – косое произведение (точка лежит на прямой)
                        //2. (OSi, Od) ≥ 0 – скалярное произведение (точка лежит на луче)  (MP1,MP2) ≤ 0

                       d = point[i];
                    }
                  //добавить для последней точки сравнение с A0A1 пересечение с OSi. Это будет верхней границей вместо  d если оно правее ( но не point[i]!)
                  
                    

                    k++;
                    help[k] = d;

                    // c точка пересчения A0Ai-1 и OSi
                    if (n > 3)
                    {
                        PointF c = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[0].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                            (((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[0].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))));
                       
                        //S0 и Si лежат в разные стороны от OSi-1 => c - любое
                     if ((((point[i-1].X-O.X)*(point[0].Y-O.Y)-((point[0].X-O.X)*(point[i-1].Y-O.Y))) *  ((point[i-1].X-O.X)*(point[i].Y-O.Y)-((point[i].X-O.X)*(point[i-1].Y-O.Y))))<0)
                      {
                          //они в разных поменять тут
                          //вывести c 
                          c = O;
                      }

                    //пересечение A0A1 и OSi
                      
                         PointF f = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[i].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                    (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[i].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))));
                         bool flag = false;
                         if (alpha[i] > 270)//четвертая четверть
                         {
                             if (((f.X > point[i].X) & (f.X > O.X)) || ((f.X < point[i].X) & (f.X < O.X)))// не лежит на OSi
                             {
                                 flag = true;//f не важно 
                             }

                             if ((c.X < f.X)&(f.X<d.X) & (!flag))
                                 d = f;
                             if (!flag)
                             {
                                 k3++;
                                 help3[k3] = f;
                             }  
                        /* else
                         {
                             if (((f.X > point[i].X) & (f.X > O.X)) || ((f.X < point[i].X) & (f.X < O.X)))
                             {
                                 flag = true;
                             }
                             if ((f.X > d.X) & (!flag))
                                 d = f;
                         }*/
                    } 
                      k2++;
                      help2[k2] = c;
                        double a ;
                        if (c.X == d.X) { a = 0; }
                        else a = (c.Y - d.Y) / (c.X - d.X); //наклон луча OSi
                        double b = c.Y - a * c.X;
                        //генерируем новую точку Ai
                        double ax;
                        if (d.X > c.X)
                            ax = GetRandomNumber(Convert.ToDouble(c.X)+eps, Convert.ToDouble(d.X)-eps);
                        else ax = GetRandomNumber(Convert.ToDouble(d.X)+eps, Convert.ToDouble(c.X)-eps);

                       if (i == 2)
                        {
                            if (O.X<d.X)
                            ax = GetRandomNumber(Convert.ToDouble(O.X)+eps, Convert.ToDouble(d.X)-eps);
                            else ax = GetRandomNumber(Convert.ToDouble(d.X)+eps, Convert.ToDouble(O.X)-eps);
                        }

                     //   if (Math.Abs(ax) > point[i].X) ax = point[i].X;
                        double ay = a * ax + b;
                        polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                    }


                    if (n == 3)
                    {
                        double a;
                        if (O.X == d.X) { a = 0; }
                        else a = (O.Y - d.Y) / (O.X - d.X); //наклон луча OSi
                        double b = d.Y - a * d.X;
                        double ax;
                        if (O.X < d.X)
                            ax = GetRandomNumber(O.X + eps, Convert.ToDouble(d.X)-eps);
                        else ax = GetRandomNumber(Convert.ToDouble(d.X)+eps, O.X - eps); 
                        double ay = a * ax + b;
                        k++;
                        help[k] = d;
                        // polygon[i] = d;
                        polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                    }
                }
              
            }


            listBox1.Items.Clear();
            bitmap = new Bitmap(wid, hei); //создаём битмап
            Graphics gr = Graphics.FromImage(bitmap); //привязываем битмак к объекту график
            for (int i = 0; i < n; i++)
            {
             
                listBox1.Items.Add(Convert.ToString(i + 1) + ":" + Convert.ToString(point[i].X) + ";" + Convert.ToString(point[i].Y)); //вносим координаты в листбокс
            }

    
           // gr.FillRectangle(Brushes.Green, 0, 0, 1, 1);
            gr.FillRectangle(Brushes.White, 0, 0, wid,hei); //заполняем белым
            if (beams.Checked)
            {
                for (int i = 0; i < n; i++)
                {
                    //   Point x = new Point(point[i].ToPoint());
                    gr.DrawLine(new Pen(Color.Black, 2), new Point(wid / 2, hei / 2), point[i]);
                }
            }            
            gr.DrawPolygon(new Pen(Color.Red, 2), polygon);

          pictureBox1.BackgroundImage = bitmap; //ставим на нижний слой новый битмап
          pictureBox1.Image = new Bitmap(1,1); //очищаем верхний слой с красными точками         
        }

        public void convfirst(int n)
        {
            double lgth; //отдаление от центра
            double angle; //угол первой точки
            int wid=pictureBox1.Width; 
            int hei = pictureBox1.Height; 

            Random random= new Random();
            int min = Convert.ToInt32(Convert.ToInt32(minimum.Value) / (2 * Math.Sin((180 / n) * (Math.PI / 180)))); //генерация отдаления от центра
            int max=Convert.ToInt32(Convert.ToInt32(maximum.Value) / (2 * Math.Sin((180 / n) * (Math.PI / 180)))); //генерация отдаления от центра
            lgth = random.Next(min, max + 1);//генерация отдаления от центра
            angle = random.Next(0, 361);//генерация угла первой точки

            int[] dob=new int[n];
            for (int i = 0; i < n; i++)
                dob[i] = (random.Next(0, (360 / n)/8) - (360 / n) / 16) * (Convert.ToInt32(maximum.Value-minimum.Value)/10); //откланение от стандартного угла

            Point[] point = new Point[n]; 
            for (int i = 0; i < n; i++)
                point[i] = new Point(Convert.ToInt32(wid / 2  +Math.Cos((angle+(360/n)*i+dob[i])*Math.PI/180)*lgth),
                                     Convert.ToInt32(hei / 2 + Math.Sin((angle+(360 / n) * i+dob[i]) * Math.PI / 180) * lgth)); //генерируем точки

            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(Convert.ToString(i + 1) + ":" + Convert.ToString(point[i].X) + ";" + Convert.ToString(point[i].Y)); 
            }

            bitmap = new Bitmap(wid, hei);
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, wid, hei);
            gr.DrawPolygon(new Pen(Color.Black, 2), point);
            pictureBox1.BackgroundImage = bitmap;
            pictureBox1.Image = new Bitmap(1, 1);
 

        }
        public void testconvfirst(int n)
        {
            double lgth; //отдаление от центра
            double angle; //угол первой точки
            int wid = pictureBox1.Width;
            int hei = pictureBox1.Height;

            Random random = new Random();
            int min = Convert.ToInt32(Convert.ToInt32(minimum.Value) / (2 * Math.Sin((180 / n) * (Math.PI / 180)))); //генерация отдаления от центра
            int max = Convert.ToInt32(Convert.ToInt32(maximum.Value) / (2 * Math.Sin((180 / n) * (Math.PI / 180)))); //генерация отдаления от центра
            lgth = random.Next(min, max + 1);//генерация отдаления от центра
            angle = random.Next(0, 361);//генерация угла первой точки

            int[] dob = new int[n];
            for (int i = 0; i < n; i++)
                dob[i] = (random.Next(0, (360 / n) / 8) - (360 / n) / 16) * (Convert.ToInt32(maximum.Value - minimum.Value) / 10); //откланение от стандартного угла

            Point[] point = new Point[n];
            for (int i = 0; i < n; i++)
                point[i] = new Point(Convert.ToInt32(wid / 2 + Math.Cos((angle + (360 / n) * i + dob[i]) * Math.PI / 180) * lgth),
                                     Convert.ToInt32(hei / 2 + Math.Sin((angle + (360 / n) * i + dob[i]) * Math.PI / 180) * lgth)); //генерируем точки
        }
        public void testgen(int n)
        {
            double eps = 0.01; //отклонение от точек,чтобы не попадать на них при рандоме
            int length; //отдаление от центра
            int wid = pictureBox1.Width; //размер полотна(высота)
            int hei = pictureBox1.Height; //размер полотна(длина)

            Random random = new Random();
            PointF[] point = new PointF[n];
            int[] alpha = new int[n];
            alpha[0] = 0;
            // Первый вариант генерации
            for (int i = 1; i < n; i++)
            {
                //  alpha[i] = random.Next(1, 179);
                if ((alpha[i - 1] + 180) >= 360)
                {
                    alpha[i] = random.Next(alpha[i - 1] + 1, 359);
                    while ((360 - alpha[i]) / (n - i) <= 1)//остается меньше чем по градусу на угол
                        alpha[i] = random.Next(alpha[i - 1] + 1, 359);
                }
                else
                {
                    alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                    while ((360 - alpha[i]) / (n - i) <= 1)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                }
                if (i == n - 1)
                {
                    while (360 - alpha[i] >= 180)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);

                }
            }
            length = 150;
            for (int i = 0; i < n; i++)
                point[i] = new PointF(Convert.ToSingle(wid / 2 + Math.Cos(-(alpha[i]) * Math.PI / 180) * length),
                                     Convert.ToSingle(hei / 2 + Math.Sin(-(alpha[i]) * Math.PI / 180) * length));  //пусть это S
            PointF[] polygon = new PointF[n];
            PointF[] help = new PointF[n + 1];
            PointF[] help2 = new PointF[n + 1];
            PointF[] help3 = new PointF[n + 1];
            int k = -1;
            int k2 = -1;
            int k3 = -1;
            double a1;
            PointF O = new PointF(wid / 2, hei / 2);
            for (int i = 0; i < 2; i++)
            {
                if (O.X == point[i].X)
                {
                    a1 = 0;
                }
                else a1 = (O.Y - point[i].Y) / (O.X - point[i].X); //наклон луча OSi
                double b1 = point[i].Y - a1 * point[i].X;
                //генерируем новую точку A1
                double ax1;

                if (O.X < point[i].X)
                    ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                double ay1 = a1 * ax1 + b1;
                polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
            }

            for (int i = 2; i < n; i++)
            {
                {
                    PointF d = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[i - 2].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                    (((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[i - 2].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))));

                    if (((d.X > point[i].X) & (d.X > O.X)) || ((d.X < point[i].X) & (d.X < O.X)))  //( (((d.X-O.X)*(point[i].Y-O.Y)-(d.Y-O.Y)*(point[i].X-O.X)) != 0 ) ||  (((d.X > point[i].X)&(d.X > O.X)) || ((d.X<point[i].X)&(d.X<O.X))) )//(((( -wid/2 + point[i].X) *(-hei/2 + d.Y) - (- wid/2 + d.X) * ( -hei/2 + point[i].Y)) != 0) || ((-d.X+wid/2)*(-d.X+point[i].X)+((-d.Y+hei/2)*(-d.Y+point[i].Y))>0))
                    {

                        d = point[i];
                    }
                    k++;
                    help[k] = d;
                    // c точка пересчения A0Ai-1 и OSi
                    if (n > 3)
                    {
                        PointF c = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[0].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                            (((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[0].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                            / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))));

                        //S0 и Si лежат в разные стороны от OSi-1 => c - любое
                        if ((((point[i - 1].X - O.X) * (point[0].Y - O.Y) - ((point[0].X - O.X) * (point[i - 1].Y - O.Y))) * ((point[i - 1].X - O.X) * (point[i].Y - O.Y) - ((point[i].X - O.X) * (point[i - 1].Y - O.Y)))) < 0)
                        {
                            //они в разных поменять тут
                            //вывести c 
                            c = O;
                        }

                        //пересечение A0A1 и OSi

                        PointF f = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[i].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                   / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                   (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[i].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                   / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))));
                        bool flag = false;
                        if (alpha[i] > 270)//четвертая четверть
                        {
                            if (((f.X > point[i].X) & (f.X > O.X)) || ((f.X < point[i].X) & (f.X < O.X)))// не лежит на OSi
                            {
                                flag = true;//f не важно 
                            }

                            if ((c.X < f.X) & (f.X < d.X) & (!flag))
                                d = f;
                            if (!flag)
                            {
                                k3++;
                                help3[k3] = f;
                            }

                        }
                        k2++;
                        help2[k2] = c;
                        double a;
                        if (c.X == d.X) { a = 0; }
                        else a = (c.Y - d.Y) / (c.X - d.X); //наклон луча OSi
                        double b = c.Y - a * c.X;
                        //генерируем новую точку Ai
                        double ax;
                        if (d.X > c.X)
                            ax = GetRandomNumber(Convert.ToDouble(c.X) + eps, Convert.ToDouble(d.X) - eps);
                        else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(c.X) - eps);

                        if (i == 2)
                        {
                            if (O.X < d.X)
                                ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(d.X) - eps);
                            else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(O.X) - eps);
                        }

                        //   if (Math.Abs(ax) > point[i].X) ax = point[i].X;
                        double ay = a * ax + b;
                        polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                    }


                    if (n == 3)
                    {
                        double a;
                        if (O.X == d.X) { a = 0; }
                        else a = (O.Y - d.Y) / (O.X - d.X); //наклон луча OSi
                        double b = d.Y - a * d.X;
                        double ax;
                        if (O.X < d.X)
                            ax = GetRandomNumber(O.X + eps, Convert.ToDouble(d.X) - eps);
                        else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, O.X - eps);
                        double ay = a * ax + b;
                        k++;
                        help[k] = d;
                        // polygon[i] = d;
                        polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                    }
                }

            }
        }
        public void teststar(int n)
        {
            double eps = 0.01; //отклонение от точек,чтобы не попадать на них при рандоме
            int length; //отдаление от центра
            int wid = pictureBox1.Width; //размер полотна(высота)
            int hei = pictureBox1.Height; //размер полотна(длина)
            bitmap = new Bitmap(wid, hei); //создаём битмап
            Graphics gr = Graphics.FromImage(bitmap); //привязываем битмак к объекту график
            gr.FillRectangle(Brushes.White, 0, 0, wid, hei); //заполняем белым
            Random random = new Random();
            PointF[] point = new PointF[n];
            int[] alpha = new int[n];
            alpha[0] = 0;
            for (int i = 1; i < n; i++)
            {
                if ((alpha[i - 1] + 180) >= 360)
                {
                    alpha[i] = random.Next(alpha[i - 1] + 1, 359);
                    while ((360 - alpha[i]) / (n - i) <= 1)//остается меньше чем по градусу на угол
                        alpha[i] = random.Next(alpha[i - 1] + 1, 359);
                }
                else
                {
                    alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                    while ((360 - alpha[i]) / (n - i) <= 1)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);
                }
                if (i == n - 1)
                {
                    while (360 - alpha[i] >= 180)
                        alpha[i] = random.Next(alpha[i - 1] + 1, alpha[i - 1] + 179);

                }
            }
            length = 150;
            for (int i = 0; i < n; i++)
                point[i] = new PointF(Convert.ToSingle(wid / 2 + Math.Cos(-(alpha[i]) * Math.PI / 180) * length),
                                     Convert.ToSingle(hei / 2 + Math.Sin(-(alpha[i]) * Math.PI / 180) * length));  //пусть это S
            //пересортируем всё
            int index = random.Next(0, n);//теперь k тот луч с которог опроисходят изменения
            point = Shift(point, n - index);
            PointF[] polygon = new PointF[n];
            PointF[] help = new PointF[n + 1];
            PointF[] help2 = new PointF[n + 1];
            PointF[] help3 = new PointF[n + 1];
            int k = -1;
            int gde = -1;
            double a1;
            PointF O = new PointF(wid / 2, hei / 2);
        label1:
            bool flag = false;

            for (int i = 0; i < 1; i++)
            {
                if (O.X == point[i].X)
                {
                    a1 = 0;
                }
                else a1 = (O.Y - point[i].Y) / (O.X - point[i].X); //наклон луча OSi
                double b1 = point[i].Y - a1 * point[i].X;
                //генерируем новую точку A1 или A0
                double ax1;
                if (O.X < point[i].X)
                    ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                double ay1 = a1 * ax1 + b1;
                polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
            }
            //A1 выберем так чтобы A2 пыталось нарушить
            double helpax;
            double helpa;
            if (point[2].X == O.X) { helpa = 0; }
            else helpa = (point[2].Y - O.Y) / (point[2].X - O.X); //наклон луча OS2
            double helpb = point[2].Y - helpa * point[2].X;
            if (O.X < point[2].X)
                helpax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[2].X) - eps);//выбрали нечто на OS2
            else helpax = GetRandomNumber(Convert.ToDouble(point[2].X) + eps, Convert.ToDouble(O.X) - eps);
            double helpay = helpa * helpax + helpb;
            //g - перечесение OS1 и A0hel
            PointF g = new PointF(Convert.ToSingle(((polygon[0].X * helpay - polygon[0].Y * helpax) * (wid / 2 - point[1].X) - (polygon[0].X - helpax) * (wid / 2 * point[1].Y - hei / 2 * point[1].X))
                     / ((polygon[0].X - helpax) * (-point[1].Y + hei / 2) - (polygon[0].Y - helpay) * (-point[1].X + wid / 2))), Convert.ToSingle
                     (((polygon[0].X * helpay - polygon[0].Y * helpax) * (hei / 2 - point[1].Y) - (polygon[0].Y - helpay) * (wid / 2 * point[1].Y - hei / 2 * point[1].X))
                     / ((polygon[0].X - helpax) * (-point[1].Y + hei / 2) - (polygon[0].Y - helpay) * (-point[1].X + wid / 2))));
            if (((g.X > point[1].X) & (g.X < O.X)) || ((g.X < point[1].X) & (g.X > O.X)))//  лежит на OS1
                polygon[1] = g;
            else//не лежит, нет пересечений - любое
            {
                if (O.X == point[1].X)
                {
                    a1 = 0;
                }
                else a1 = (O.Y - point[1].Y) / (O.X - point[1].X); //наклон луча OS1
                double b1 = point[1].Y - a1 * point[1].X;
                //генерируем новую точку A1 или A0
                double ax1;
                if (O.X < point[1].X)
                    ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[1].X) - eps);
                else ax1 = GetRandomNumber(Convert.ToDouble(point[1].X) + eps, Convert.ToDouble(O.X) - eps);
                double ay1 = a1 * ax1 + b1;
                polygon[1] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
            }
            //проверим A2
            //f точка пересечения А0А1 и OS2
            PointF f = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[2].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[2].Y - hei / 2 * point[2].X))
                   / ((polygon[0].X - polygon[1].X) * (-point[2].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[2].X + wid / 2))), Convert.ToSingle
                   (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[2].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[2].Y - hei / 2 * point[2].X))
                   / ((polygon[0].X - polygon[1].X) * (-point[2].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[2].X + wid / 2))));
            if (((f.X > point[2].X) & (f.X < O.X)) || ((f.X < point[2].X) & (f.X > O.X)))//  лежит на OS2
            {                      //выберем A2 нарушая границу от f 
                flag = true;
                gde = 2;
                double a;
                if (point[2].X == f.X) { a = 0; }
                else a = (point[2].Y - f.Y) / (point[2].X - f.X); //наклон луча OSi
                double b = f.Y - a * f.X;
                double ax;
                if (point[2].X < f.X)
                    ax = GetRandomNumber(point[2].X + eps, Convert.ToDouble(f.X) - eps);
                else ax = GetRandomNumber(Convert.ToDouble(f.X) + eps, point[2].X - eps);
                double ay = a * ax + b;
                k++;
                help[k] = f;
                // polygon[i] = d;
                polygon[2] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
            }
            else
            {
                if (O.X == point[2].X)
                {
                    a1 = 0;
                }
                else a1 = (O.Y - point[2].Y) / (O.X - point[2].X); //наклон луча OSi
                double b1 = point[2].Y - a1 * point[2].X;
                double ax1;
                if (O.X < point[1].X)
                    ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[2].X) - eps);
                else ax1 = GetRandomNumber(Convert.ToDouble(point[2].X) + eps, Convert.ToDouble(O.X) - eps);
                double ay1 = a1 * ax1 + b1;
                polygon[2] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
            }


            for (int i = 3; i < n; i++)
            {
                if (!flag)// не было еще нарушений
                //ищем любимые границы и нарушаем их
                {
                    // d верхняя граница Ai-1Ai-2  и OSi
                    bool up = true;
                    bool down = true;
                    PointF d = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[i - 2].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                    (((polygon[i - 1].X * polygon[i - 2].Y - polygon[i - 1].Y * polygon[i - 2].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[i - 2].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                    / ((polygon[i - 1].X - polygon[i - 2].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[i - 2].Y) * (-point[i].X + wid / 2))));

                    if (((d.X > point[i].X) & (d.X > O.X)) || ((d.X < point[i].X) & (d.X < O.X)))
                    {
                        d = point[i];
                        up = false;
                    }
                    // c точка пересчения A0Ai-1 и OSi
                    PointF c = new PointF(Convert.ToSingle(((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (wid / 2 - point[i].X) - (polygon[i - 1].X - polygon[0].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                        / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
                        (((polygon[i - 1].X * polygon[0].Y - polygon[i - 1].Y * polygon[0].X) * (hei / 2 - point[i].Y) - (polygon[i - 1].Y - polygon[0].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
                        / ((polygon[i - 1].X - polygon[0].X) * (-point[i].Y + hei / 2) - (polygon[i - 1].Y - polygon[0].Y) * (-point[i].X + wid / 2))));

                    //S0 и Si лежат в разные стороны от OSi-1 => c - любое
                    if ((((point[i - 1].X - O.X) * (point[0].Y - O.Y) - ((point[0].X - O.X) * (point[i - 1].Y - O.Y))) * ((point[i - 1].X - O.X) * (point[i].Y - O.Y) - ((point[i].X - O.X) * (point[i - 1].Y - O.Y)))) < 0)
                    {
                        c = O;
                        down = false;
                    }

                    //пересечение A0A1 и OSi                      
                    PointF e = new PointF(Convert.ToSingle(((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (wid / 2 - point[i].X) - (polygon[0].X - polygon[1].X) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
               / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))), Convert.ToSingle
               (((polygon[0].X * polygon[1].Y - polygon[0].Y * polygon[1].X) * (hei / 2 - point[i].Y) - (polygon[0].Y - polygon[1].Y) * (wid / 2 * point[i].Y - hei / 2 * point[i].X))
               / ((polygon[0].X - polygon[1].X) * (-point[i].Y + hei / 2) - (polygon[0].Y - polygon[1].Y) * (-point[i].X + wid / 2))));
                    bool mid = true;

                    if (((e.X > point[i].X) & (e.X > O.X)) || ((e.X < point[i].X) & (e.X < O.X)))// не лежит на OSi
                    {
                        mid = false;//e не важно 
                    }

                    if ((c.X < e.X) & (e.X < d.X) & (mid))
                        d = e;



                    double a;
                    if (c.X == d.X) { a = 0; }
                    else a = (c.Y - d.Y) / (c.X - d.X); //наклон луча OSi
                    double b = c.Y - a * c.X;
                    //генерируем новую точку Ai
                    double ax;
                    //смотрим на up down если обоих нет то не нарушили, если один из них то нарушим его, если оба то любой
                    if ((up) & (down))
                    {
                        //нарушаем одну из них
                        int x = random.Next(0, 1);
                        if (x == 0)//нарушим низ с
                        {
                            if (O.X > c.X)
                                ax = GetRandomNumber(Convert.ToDouble(c.X) + eps, Convert.ToDouble(O.X) - eps);
                            else ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(c.X) - eps);
                            double ay = a * ax + b;
                            polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                            k++;
                            help[k] = c;
                        }
                        else//нарушим верх d
                        {
                            if (d.X > point[i].X)
                                ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(d.X) - eps);
                            else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(point[i].X) - eps);
                            double ay = a * ax + b;
                            polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                            k++;
                            help[k] = d;
                        }
                        flag = true;
                        gde = i;

                    }
                    else
                        if ((!up) & (!down))
                        {
                            if (point[i].X > O.X)
                                ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                            else ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                            double ay = a * ax + b;
                            polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                            //ни один нельзя
                        }
                        else
                        {
                            if (down)
                            {
                                if (O.X > c.X)
                                    ax = GetRandomNumber(Convert.ToDouble(c.X) + eps, Convert.ToDouble(O.X) - eps);
                                else ax = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(c.X) - eps);
                                double ay = a * ax + b;
                                polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                flag = true;
                                gde = i;
                                k++;
                                help[k] = c;
                            }
                            if (up)
                            {
                                if (d.X > point[i].X)
                                    ax = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(d.X) - eps);
                                else ax = GetRandomNumber(Convert.ToDouble(d.X) + eps, Convert.ToDouble(point[i].X) - eps);
                                double ay = a * ax + b;
                                polygon[i] = new PointF(Convert.ToSingle(ax), Convert.ToSingle(ay));
                                flag = true;
                                gde = i;
                                k++;
                                help[k] = d;
                            }
                        }
                }


                else//нарушена граница и идем дальше спокойно
                {
                    if (O.X == point[i].X)
                    {
                        a1 = 0;
                    }
                    else a1 = (O.Y - point[i].Y) / (O.X - point[i].X);
                    double b1 = point[i].Y - a1 * point[i].X;
                    double ax1;
                    if (O.X < point[i].X)
                        ax1 = GetRandomNumber(Convert.ToDouble(O.X) + eps, Convert.ToDouble(point[i].X) - eps);
                    else ax1 = GetRandomNumber(Convert.ToDouble(point[i].X) + eps, Convert.ToDouble(O.X) - eps);
                    double ay1 = a1 * ax1 + b1;
                    polygon[i] = new PointF(Convert.ToSingle(ax1), Convert.ToSingle(ay1));
                }
            }
            if (!flag) goto label1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Typem.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип мноугогольника");
              
            }
            else
            {
                if (Typem.SelectedIndex == 0)
                    testconvfirst(Convert.ToInt32(numericUpDown1.Value));
                if (Typem.SelectedIndex == 1)
                    gen(Convert.ToInt32(numericUpDown1.Value));
                if (Typem.SelectedIndex == 2)
                {
                    if (Convert.ToInt32(numericUpDown1.Value) > 3)
                      star(Convert.ToInt32(numericUpDown1.Value));
                    else
                    {
                        MessageBox.Show("Для звёздного многоугольника должно быть n>3");
                       
                    }
                }
         
            }      
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap newbitmap = new Bitmap(pictureBox1.Width,pictureBox1.Height); //создание нового битмапа
            String[] str=((String)listBox1.Items[listBox1.SelectedIndex]).Split('.',';'); //разбиваем текст выделеного элемента листбокса
            Graphics gr = Graphics.FromImage(newbitmap); //новый объект графикс для прорисовки точки
            gr.FillRectangle(new SolidBrush(Color.Red), new Rectangle(Convert.ToInt32(str[1])-2, Convert.ToInt32(str[2])-2, 4,4)); //рисуем точку
            pictureBox1.Image = newbitmap; //заменяем верхний слой pictureBox
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }



        private void maximum_ValueChanged(object sender, EventArgs e)
        {
        
        }

        private void minimum_ValueChanged(object sender, EventArgs e)
        {
        
        }

        private void minimum_ValueChanged_1(object sender, EventArgs e)
        {
        
        }

        private void export_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int wid = pictureBox1.Width; //размер полотна(высота)
            int hei = pictureBox1.Height; //размер полотна(длина)
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save an Tex File";
            saveFileDialog.DefaultExt = "tex";
            PointF[] point = new PointF[listBox1.Items.Count];
            string[] spp = new string[listBox1.Items.Count];
            for (int i = 0; i < spp.Length; i++)
                spp[i] = listBox1.Items[i].ToString();

            for (int i = 0; i < spp.Length; i++)
            {
                int begx = spp[i].IndexOf(":");
                int endx = spp[i].IndexOf(";");

                string xx = (spp[i].Substring(begx + 1, endx - begx-1));
                float x = Convert.ToSingle(xx);
                string yy = (spp[i].Substring(endx + 1 ));
                float y = Convert.ToSingle(yy);
                point[i] = new PointF(x,y);
            }


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream fileStream = saveFileDialog.OpenFile();
                System.IO.StreamWriter file = new System.IO.StreamWriter(fileStream);
            
                        file.WriteLine(@" \documentclass[]{article}");
                        file.WriteLine(@"\usepackage{tikz}");
                        file.WriteLine(@"\usetikzlibrary{calc}");
                        file.WriteLine(@"\begin{document}");
                        file.WriteLine(@"\begin{figure}[h]");
                        file.WriteLine(@"\centering");
                        file.WriteLine(@"\begin{tikzpicture}[scale=0.07]");
                        double op = random.NextDouble();
                        string opacity = op.ToString();
                        opacity = "0." + opacity.Substring(3, 4);
                        file.WriteLine();
                        file.Write(@"\filldraw[draw = black,fill = blue, opacity=" + opacity + "] ");

                        for (int i = 0; i < point.Length; i++)
                        {
                             string xx = Convert.ToString((point[i].X - wid / 2), CultureInfo.GetCultureInfo("en-US"));
                         //   string yy = Convert.ToString(point[i].Y - hei / 2);
                            string yy = Convert.ToString((point[i].Y - hei / 2), CultureInfo.GetCultureInfo("en-US"));
                            if (i != point.Length - 1)
                                file.Write("(" + xx + "," + yy + ")--");
                            else file.Write("(" + xx + "," + yy + ")--cycle;");
                        }
                        file.WriteLine();
                        file.WriteLine(@"\end{tikzpicture}");
                        file.WriteLine(@"\end{figure}");
                        file.WriteLine(@"\end{document}");         
                file.Flush();
                file.Close();
            }
        }

        private void beams_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Test_Click(object sender, EventArgs e)
        {
            
            if (Typem.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип мноугогольника");
            }
            else
            {
                System.Diagnostics.Stopwatch sw = new Stopwatch();
                sw.Start();
                if (Typem.SelectedIndex == 0)
                {
                    for ( int i=0; i <m.Value ;i++)
                        testconvfirst(Convert.ToInt32(numericUpDown1.Value));
                }

                if (Typem.SelectedIndex == 1)
                {
                    for (int i = 0; i < m.Value; i++)
                    testgen(Convert.ToInt32(numericUpDown1.Value));
                }

                if (Typem.SelectedIndex == 2)
                {
                    
                        if (Convert.ToInt32(numericUpDown1.Value) > 3)
                            for (int i = 0; i < m.Value; i++)
                                teststar(Convert.ToInt32(numericUpDown1.Value));
                    
                    else
                    {
                        MessageBox.Show("Для звёздного многоугольника должно быть n>3");

                    }
                }
                sw.Stop();
                MessageBox.Show("Затраченное время в миллисекундах = "+(sw.ElapsedMilliseconds).ToString());
            }
         
        }



   


        }
    }

