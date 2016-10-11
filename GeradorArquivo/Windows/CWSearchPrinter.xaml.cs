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
    /// <summary>
    /// Interaction logic for CWSearchPrinter.xaml
    /// </summary>
    public partial class CWSearchPrinter :INotifyPropertyChanged
    {
        private readonly ObservableCollection<PrinterSupplyModelCounter> _collectionPrinterSupplyModel;
        private ObservableCollection<PrinterModel> _collectionPrinterModel;
        public ObservableCollection<PrinterModel> CollectionPrinterModel
        {
            get { return _collectionPrinterModel ?? (_collectionPrinterModel = new ObservableCollection<PrinterModel>()); }
            set
            {
                _collectionPrinterModel = value;
                OnPropertyChanged("CollectionPrinterModel");
            }
        }

        private PrinterModel _selectdPrinterModel;
        public PrinterModel SelectdPrinterModel
        {
            get { return _selectdPrinterModel; }
            set
            {
                _selectdPrinterModel = value;
                
                OnPropertyChanged("SelectdPrinterModel");
            }
        }

        public CWSearchPrinter(ObservableCollection<PrinterSupplyModelCounter> collectionPrinterSupplyModel)
        {
            _collectionPrinterSupplyModel = collectionPrinterSupplyModel;
            InitializeComponent();
            ReadAll();
        }

        public CWSearchPrinter()
        {
            InitializeComponent();
            ReadAll();
        }

        private void ReadAll()
        {
            var db = new PrinterModelDB();
            CollectionPrinterModel.Clear();
            CollectionPrinterModel.AddRange(db.BuscaTodos());
            if (_collectionPrinterSupplyModel != null && _collectionPrinterSupplyModel.Count>0)
            {
                foreach (var item in _collectionPrinterSupplyModel)
                {
                    var printerModel = CollectionPrinterModel.First(p => p.PrinteModelID == item.PrinteModelID);
                    CollectionPrinterModel.Remove(printerModel);
                }
            }
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
                    var obj = o as PrinterModel;
                    return (obj.Brand.BrandName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.ModelName.ToUpper().StartsWith(filter.ToUpper())||
                            obj.NameXML.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }

        private void OnKeyRemoveEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                TbSearch.Text = string.Empty;
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
