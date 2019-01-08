using CSDeskBand;
using Notebar.Core;
using System;
using System.Runtime.InteropServices;

namespace Notebar.Toolbar
{
    [ComVisible(true)]
    [Guid(Constants.NotebarGuid)]
    [CSDeskBandRegistration(Name = "Notebar", ShowDeskBand = true)]
    public partial class DeskBand
    {
        private DeskBandViewModel ViewModel { get; }

        public DeskBand()
        {
            InitializeComponent();

            ViewModel = new DeskBandViewModel(Options, Dispatcher);
            ViewModel.Init(TaskbarInfo.Orientation);

            DataContext = ViewModel;
        }

        protected override void OnClose()
        {
            base.OnClose();
            ViewModel.OnClose();
        }
    }
}
