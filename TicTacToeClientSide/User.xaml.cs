using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToeClientSide.Help;

namespace TicTacToeClientSide
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        string path = string.Empty;
        BitmapImage BI { get; set; }
        public User()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OPD = new OpenFileDialog();
            OPD.DefaultExt = ".png";
            OPD.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            Nullable<bool> result = OPD.ShowDialog();
            if (result == true)
            {
                // Open document 
                path = OPD.FileName;
                var ımageBrush= new ImageBrush();
                Image image = new Image();
                BI = new BitmapImage(new Uri(path));
                image.Source = BI;
                ımageBrush.ImageSource = image.Source;
                Img_back.Background = ımageBrush;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (path!=string.Empty&&Name_text_box.Text!=" "&& Name_text_box.Text != "")
            {
                UserLogin UL = new UserLogin(BI, Name_text_box.Text);
                MainWindow mainWindow= new MainWindow(UL);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
