using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Traincrew_D_ATC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            CompositionTarget.Rendering += UpdateTrianglePosition;
        }

        private void UpdateTrianglePosition(object sender, EventArgs e)
        {
            MainCanvas.Children.Clear();
            double canvasLength = Math.Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            double triangleLength = 20;

            // 円の半径
            double radius = canvasLength / 2 - triangleLength;

            for (int i = 0; i < 1; i++)
            {
                double speed = i * 20;
                double currentAngle = GetAngleForSpeed(speed);
                double radian = currentAngle * Math.PI / 180;

                // 三角形の位置を計算
                double triangleX = MainCanvas.ActualWidth / 2.0 + radius * Math.Cos(-radian) - triangleLength / 2;
                double triangleY = MainCanvas.ActualHeight / 2.0 + radius * Math.Sin(-radian) - triangleLength;

                // 三角形を作成
                Polygon triangle = new Polygon
                {
                    Fill = Brushes.Green,
                    Points = new PointCollection
                    {
                        new Point(0, 0),
                        new Point(triangleLength, 0),
                        new Point(triangleLength / 2, triangleLength),
                    },
                    RenderTransformOrigin = new Point(0.5, 1),
                    RenderTransform = new RotateTransform(-currentAngle + 90)
                };

                // 三角形をCanvasに追加
                Canvas.SetLeft(triangle, triangleX);
                Canvas.SetTop(triangle, triangleY);
                MainCanvas.Children.Add(triangle);
            }


            // 円の大きさと位置を設定
            if (canvasLength <= 0)
            {
                return;
            }

            var circle = new Ellipse
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Transparent
            };

            double circleDiameter = canvasLength - 2 * triangleLength; // 円の直径
            double circleX = (MainCanvas.ActualWidth - circleDiameter) / 2;
            double circleY = (MainCanvas.ActualHeight - circleDiameter) / 2;
            circle.Width = Math.Abs(circleDiameter);
            circle.Height = Math.Abs(circleDiameter);
            Canvas.SetLeft(circle, circleX);
            Canvas.SetTop(circle, circleY);
            MainCanvas.Children.Add(circle);
        }

        private double GetAngleForSpeed(double speed)
        {
            // 速度と角度の対応を定義
            SortedDictionary<double, double> SpeedToAngle = new()
            {
                { 0, 210 }, // 0km/h時の角度
                { 20, 169 },
                { 40, 127 },
                { 60, 92.5 },
                { 80, 56 },
                { 100, 14 },
                { 120, -30 }
            };
            // 辞書のキーを取得(すでにソート済み)
            var keys = SpeedToAngle.Keys.ToList();

            // 速度が範囲外の場合、最小または最大の角度を返す
            if (speed <= keys.First()) return SpeedToAngle[keys.First()];
            if (speed >= keys.Last()) return SpeedToAngle[keys.Last()];

            // 隣接するキーを取得
            for (int i = 0; i < keys.Count - 1; i++)
            {
                if (speed >= keys[i] && speed <= keys[i + 1])
                {
                    // 線形補間
                    double t = (speed - keys[i]) / (keys[i + 1] - keys[i]);
                    return SpeedToAngle[keys[i]] + t * (SpeedToAngle[keys[i + 1]] - SpeedToAngle[keys[i]]);
                }
            }

            return 0; // デフォルト値（到達しないはず）
        }
    }
}