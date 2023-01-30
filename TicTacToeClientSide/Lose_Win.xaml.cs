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
using System.Windows.Shapes;

namespace TicTacToeClientSide
{
    /// <summary>
    /// Interaction logic for Lose_Win.xaml
    /// </summary>
    public partial class Lose_Win : Window
    {
        public Lose_Win(bool isok,string txt)
        {
            InitializeComponent();
            if (isok)
                rxr.Content = $"Win: {txt}";
            else
                rxr.Content = $"Lose: {txt}";
        }
    }
}
