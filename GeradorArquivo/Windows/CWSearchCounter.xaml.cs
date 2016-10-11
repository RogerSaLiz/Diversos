using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;

namespace GeradorArquivo.Windows
{
   
    public partial class CWSearchCounter : INotifyPropertyChanged
    {
        private ObservableCollection<PrinterSupplyModelCounter> _collectionCounterPrinters;
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
                OnPropertyChanged("SelectedCounterType");
            }
        }

        public CWSearchCounter()
        {
            InitializeComponent();
            ReadAll();
        }

        public CWSearchCounter(ObservableCollection<PrinterSupplyModelCounter> collectionCounterPrinters)
        {
            _collectionCounterPrinters = collectionCounterPrinters;
            InitializeComponent();
            ReadAll();
        }

        private void ReadAll()
        {
            var db = new CounterTypeDB();
            CollectionCounterType.Clear();
            //CollectionCounterType.AddRange(db.BuscaTodos());
            if (_collectionCounterPrinters != null && _collectionCounterPrinters.Count > 0)
            {
                foreach (var item in db.BuscaTodos())
                {
                    var ob = _collectionCounterPrinters.FirstOrDefault(p => p.CounterTypeID == item.CounterTypeID);
                    if (ob == null)
                        CollectionCounterType.Add(item);
                }
            }
            else
                CollectionCounterType.AddRange(db.BuscaTodos());
            

        }

        private void OnTextSearch(object sender, TextChangedEventArgs e)
        {
            var t = (TextBox)sender;
            string filter = t.Text;

            var cv = CollectionViewSource.GetDefaultView(Dg.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    cv.Filter = null;
                    return;
                }
                cv.Filter = o =>
                {
                    var obj = o as CounterType;
                    return (obj.CounterTypeID.ToString().ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.CounterTypeName.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }

        private void OnKeyRemoveEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                TbSearch.Text = string.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
