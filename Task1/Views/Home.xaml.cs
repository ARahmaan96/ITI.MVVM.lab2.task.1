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
using Task1.ModleView;

namespace Task1.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            BackgroundVideo?.Play();
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Restart the video when it ends
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo?.Play();
        }


    }
}
