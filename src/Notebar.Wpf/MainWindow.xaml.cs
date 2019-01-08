using CSDeskBand;
using Notebar.Toolbar;
using System;
using System.Windows;

namespace Notebar.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeskBandViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.Height = 40;

            var options = new CSDeskBandOptions();

            ViewModel = new DeskBandViewModel(options, Dispatcher);
            ViewModel.Init(TaskbarOrientation.Horizontal);

            DataContext = ViewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ViewModel.OnClose();
        }
    }
}
