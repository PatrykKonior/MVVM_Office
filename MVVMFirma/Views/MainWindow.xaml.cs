using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            StartClock();
            this.DataContext = new MainWindowViewModel();
        }

        private void StartClock()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) => 
            {
                ClockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            timer.Start();
        }
    }
}