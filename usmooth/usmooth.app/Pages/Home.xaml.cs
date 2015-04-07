﻿using System;
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

namespace usmooth.app.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private QotdClient _client;
        private ushort _clientPort = 5555;
        //private string _clientHost = "127.0.0.1";
        private string _clientHost = "10.20.98.91";


        public Home()
        {
            InitializeComponent();
        }

        private void bt_connect_Click(object sender, RoutedEventArgs e)
        {
            _client = new QotdClient(_clientHost, _clientPort, AddToLog);
        }
        private void AddToLog(string text)
        {
            Console.WriteLine(text);
        }
    }
}
