using CSDeskBand;
using NoteBar.Core;
using System;
using System.Runtime.InteropServices;

namespace NoteBar.Toolbar
{
    [ComVisible(true)]
    [Guid(Constants.NoteBarGuid)]
    [CSDeskBandRegistration(Name = "NoteBar")]
    public partial class DeskBand
    {
        private DeskBandViewModel ViewModel { get; }

        public DeskBand()
        {
            InitializeComponent();
            TransparencyEnabled = false;

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
