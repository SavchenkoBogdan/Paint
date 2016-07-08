using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private float fontSize = 1f;
        Point currentPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FullCommand.UndoAction();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private List<Command> tmpCommandsCollection = new List<Command>();
        private void PaintSurface_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(paintSurface);
                tmpCommandsCollection.Clear();
            }
        }

        private void PaintSurface_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            new FullCommand(tmpCommandsCollection);
            tmpCommandsCollection.Clear();
        }

        private void PaintSurface_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();
                //DrawLine(currentPoint, e.GetPosition(paintSurface));
                line.Stroke = SystemColors.WindowFrameBrush;
                line.StrokeThickness = fontSize;

                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(paintSurface).X;
                line.Y2 = e.GetPosition(paintSurface).Y;
                currentPoint = e.GetPosition(paintSurface);
                paintSurface.Children.Add(line);

                tmpCommandsCollection.Add(new Command(paintSurface, line, Command.DrawAction.ADD));
            }
        }

        private void DrawLine(Point currentPoint, Point point)
        {
            var difference = point - currentPoint;
            var step = difference * 0.1f;
            for (int i = 0; i < 10; i++)
            {
                Ellipse myEllipse = new Ellipse();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = fontSize;
                myEllipse.Stroke = Brushes.Black;
                myEllipse.Width = fontSize;
                myEllipse.Height = fontSize;
                paintSurface.Children.Add(myEllipse);
                Canvas.SetLeft(myEllipse, currentPoint.X + step.X * i);
                Canvas.SetLeft(myEllipse, currentPoint.Y + step.Y * i);
            }
        }
      
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
            FullCommand.Commands.Clear();
            paintSurface.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
        }

        private void textFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                fontSize = float.Parse((sender as TextBox).Text);
            }
            catch (Exception exception)
            {
                // ignored
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
            dl1.FileName = "MYFileSave";
            dl1.DefaultExt = ".png";
            dl1.Filter = "Image documents (.png)|*.png";
            Nullable<bool> result = dl1.ShowDialog();

            if (result == true)
            {
                string filename = dl1.FileName;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(@filename, UriKind.Relative));
                paintSurface.Background = brush;
            }
        }

   
    }
}
