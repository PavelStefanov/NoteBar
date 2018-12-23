using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Notebar.Toolbar
{
    /// <summary>
    /// Interaction logic for Icon.xaml
    /// </summary>
    public partial class Icon : UserControl, INotifyPropertyChanged
    {
        private int _port;

        public int Port
        {
            get => _port;
            set
            {
                if (value == _port)
                    return;

                _port = value;
                OnPropertyChanged();
            }
        }

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value == _imagePath)
                    return;

                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public Icon()
        {
            InitializeComponent();
        }

        private void Quit(object sender, System.Windows.RoutedEventArgs e)
        {
           // CloseDeskBand();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
