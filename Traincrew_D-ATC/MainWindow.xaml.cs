using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            // 三角形の回転角度
            double angle = 120 * (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) / 1000.0;
            double canvasLength = Math.Min(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            double triangleLength = 15; 

            // 三角形の位置を計算
            double radius = canvasLength / 2 - triangleLength; // 円の半径
            double radian = angle * Math.PI / 180;
            double triangleX = MainCanvas.ActualWidth / 2.0 + radius * Math.Cos(radian) - triangleLength / 2; // 三角形の幅を考慮
            double triangleY = MainCanvas.ActualHeight / 2.0 + radius * Math.Sin(radian) - triangleLength; // 三角形の高さを考慮

            // 三角形の位置と回転を設定
            Triangle.Points =
            [
                new Point(0, 0),
                new Point(triangleLength, 0),
                new Point(triangleLength / 2, triangleLength),
            ];
            Canvas.SetLeft(Triangle, triangleX);
            Canvas.SetTop(Triangle, triangleY);
            TriangleRotate.Angle = angle + 90; // 円を向くように調整
            
            
            // 円の大きさと位置を設定
            if (canvasLength <= 0)
            {
                return;
            }
            double circleDiameter = canvasLength - 2 * triangleLength; // 円の直径
            double circleX = (MainCanvas.ActualWidth - circleDiameter) / 2;
            double circleY = (MainCanvas.ActualHeight - circleDiameter) / 2;
            Circle.Width = Math.Abs(circleDiameter);
            Circle.Height = Math.Abs(circleDiameter);
            Canvas.SetLeft(Circle, circleX);
            Canvas.SetTop(Circle, circleY);
        }
    }
}