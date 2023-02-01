using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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

namespace TicTacToeClientSide
{
    public partial class MainWindow : Window
    {
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        private bool IsOkay { get; set; } = true;
        private string strarr = "";
        private const int port = 27001;
       
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
            RequestLoop();
        }

        private void RequestLoop()
        {
            var receiver = Task.Run(() =>
            {
                while (true)
                {
                    ReceiveResponse();
                }
            });
        }

        private void ReceiveResponse()
        {

                var buffer = new byte[2048];
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0) return;
                var data = new byte[received];
                Array.Copy(buffer, data, received);
                string text = Encoding.ASCII.GetString(data);
                //  MessageBox.Show(text);
                IntegrateToView(text);
            
        }
        public bool HasSecondPlayerStart { get; set; } = false;
        private void IntegrateToView(string text)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var data = text.Split('\n');

                var row1 = data[0].Split('\t');
                var row2 = data[1].Split('\t');
                var row3 = data[2].Split('\t');
                var row4 = data[3];
                //MessageBox.Show(row4);
                if (data.Length >= 5)
                {
                    strarr = data[4];
                    //MessageBox.Show(strarr);
                }
                b1.Content = row1[0];
                b2.Content = row1[1];
                b3.Content = row1[2];

                b4.Content = row2[0];
                b5.Content = row2[1];
                b6.Content = row2[2];

                b7.Content = row3[0];
                b8.Content = row3[1];
                b9.Content = row3[2];
                bool bollen = Convert.ToBoolean(row4);
                EnabledAllButtons(!bollen);
                if (strarr != "")
                {
                    bool bl1 = Convert.ToBoolean(strarr);
                    Lose_Win ls = new Lose_Win(bl1, this.Title);
                    ClientSocket.Close();
                    ls.Show();
                    this.Close();
                }
            });
        }

        private void ConnectToServer()
        {
            //  int attempts = 0;
            this.Title = "LOADING...";
            while (!ClientSocket.Connected)
            {
                try
                {
                    //        ++attempts;
                    ClientSocket.Connect(IPAddress.Loopback, port);
                }
                catch { }

            }

            MessageBox.Show("Connected");

            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);

            string text1 = Encoding.ASCII.GetString(data);
            var row1 = text1.Split('\t');
            this.Title = "Player : " + row1[0];
            this.player.Text = this.Title;
            EnabledAllButtons(Convert.ToBoolean(row1[1]));

        }
        private void b1_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var bt = sender as Button;
                    var str = bt.Content.ToString();
                    // MessageBox.Show(str);
                    if (str != "O" && str != "X")
                    {
                        string request = str + player.Text.Split(' ')[2];
                        SendString(request);
                    }

                    // EnabledAllButtons(false);
                });
            });
        }

        public void EnabledAllButtons(bool enabled)
        {
            foreach (var item in myWrap.Children)
            {
                if (item is Button bt)
                    bt.IsEnabled = enabled;
            }
        }
        private void SendString(string request)
        {
            byte[]buffer=Encoding.ASCII.GetBytes(request);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
    }
}
