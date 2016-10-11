using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using GeradorArquivo.Windows;

namespace GeradorArquivo.Pages
{
    /// <summary>
    /// Interaction logic for PageCounter.xaml
    /// </summary>
    public partial class PageCounter : INotifyPropertyChanged
    {

        private ObservableCollection<CounterType> _collectionCounterType;
        public ObservableCollection<CounterType> CollectionCounterType
        {
            get { return _collectionCounterType ?? (_collectionCounterType = new ObservableCollection<CounterType>()); }
            set
            {
                _collectionCounterType = value;
                OnPropertyChanged("CollectionCounterType");
            }
        }

        private CounterType _selectedCounterType;
        public CounterType SelectedCounterType
        {
            get { return _selectedCounterType; }
            set
            {
                _selectedCounterType = value;
                RefleshButtons();
                OnPropertyChanged("SelectedCounterType");
            }
        }


        public PageCounter()
        {
            InitializeComponent();
            ControlButtons.OnClickAdicionarEvent += OnClickAdicionarEvent;
            ControlButtons.OnClickEditarEvent += OnClickEditarEvent;
            ReadAll();
        }
        private void RefleshButtons()
        {
            ControlButtons.EditarEnable = SelectedCounterType != null;
            ControlButtons.ExcluirEnable = SelectedCounterType != null;
        }


        private void OnClickEditarEvent(object sender, EventArgs e)
        {
            var cw = new CWCounter(SelectedCounterType);
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                    ReadAll();
                
            };
            cw.ShowDialog();
        }

        private void OnClickAdicionarEvent(object sender, EventArgs e)
        {
            var cw = new CWCounter();
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                    ReadAll();
            };
            cw.ShowDialog();
        }


        private void ReadAll()
        {
            var db = new CounterTypeDB();
            CollectionCounterType.Clear();
            CollectionCounterType.AddRange(db.BuscaTodos());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
