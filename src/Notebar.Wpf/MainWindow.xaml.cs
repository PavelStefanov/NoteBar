using Notebar.Core.Grpc;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Notebar.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
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

        private string _imageTestPath;
        public string ImageTestPath
        {
            get => _imageTestPath;
            set
            {
                if (value == _imageTestPath)
                    return;

                _imageTestPath = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Field> Fields { get; set; }
        public class Field
        {
            public string ImagePath { get; set; }
            public int Port { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();

            var iconsService = new IconsService();
            var indicatorsService = new IndicatorsService(iconsService);

            GrpcServer.Run(port => indicatorsService.Add(port));

            Fields = new ObservableCollection<Field>();
            Fields.Add(new Field() { ImagePath = "pack://application:,,,/Notebar.Core;component/Icons/Resources/red@2x.png", Port = 100 });
            Fields.Add(new Field() { ImagePath = "Password", Port = 80 });
            Fields.Add(new Field() { ImagePath = "City", Port = 100 });
            Fields.Add(new Field() { ImagePath = "State", Port = 40 });
            Fields.Add(new Field() { ImagePath = "Zipcode", Port = 60 });

            FieldsListBox.ItemsSource = Fields;

            ImageTestPath = "pack://application:,,,/Notebar.Test;component/yellow@2x.png";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
