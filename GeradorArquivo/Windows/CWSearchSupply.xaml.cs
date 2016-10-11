using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWSearchSupply.xaml
    /// </summary>
    public partial class CWSearchSupply :INotifyPropertyChanged
    {
        private readonly ObservableCollection<PrinterSupplyModelCounter> _collectionPrinterSupplyModel;
        private ObservableCollection<SupplyModel> _collectionSupplyModel;
        public ObservableCollection<SupplyModel> CollectionSupplyModel
        {
            get { return _collectionSupplyModel ?? (_collectionSupplyModel = new ObservableCollection<SupplyModel>()); }
            set
            {
                _collectionSupplyModel = value;
                OnPropertyChanged("CollectionSupplyModel");
            }
        }

        private SupplyModel _selectedSupplyModel;
        public SupplyModel SelectedSupplyModel
        {
            get { return _selectedSupplyModel; }
            set
            {
                _selectedSupplyModel = value;
                OnPropertyChanged("SelectedSupplyModel");
            }
        }


        public CWSearchSupply(ObservableCollection<PrinterSupplyModelCounter> collectionPrinterSupplyModel)
        {
            _collectionPrinterSupplyModel = collectionPrinterSupplyModel;
            InitializeComponent();
            RetrieveAllBehaviorSupply(ReadAll);
        }

        public CWSearchSupply()
        {
            InitializeComponent();
           RetrieveAllBehaviorSupply(ReadAll);
        }

        private  List<BehaviorSupply> _listBehaviorSupplies=new List<BehaviorSupply>();
        private void RetrieveAllBehaviorSupply(Action ok)
        {
            var db = new BehaviorSupplyDB();
            _listBehaviorSupplies.AddRange(db.BuscaTodos());
            ok.Invoke();

        }
        private void ReadAll()
        {
            var db = new SupplyDB();
            CollectionSupplyModel.Clear();
            foreach (var item in db.BuscaTodos())
            {
                item.ListBehSupply= new List<BehaviorSupply>();
                item.ListBehSupply.AddRange(_listBehaviorSupplies);
                item.BehSupply = item.ListBehSupply.FirstOrDefault(p=>p.BehaviorSupplyID==item.BehSupply.BehaviorSupplyID);
                CollectionSupplyModel.Add(item);
            }
            if (_collectionPrinterSupplyModel != null && _collectionPrinterSupplyModel.Count > 0)
            {
                foreach (var item in _collectionPrinterSupplyModel)
                {
                    var supply = CollectionSupplyModel.First(p => p.SupplyModelId == item.SupplyModelId);
                    CollectionSupplyModel.Remove(supply);
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
                    var obj = o as SupplyModel;
                    return (obj.Description.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.Brand.BrandName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.SupplySubFunctionType.SupplyFunctionType.SupplyFunction.SupplyFunctionName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.SupplySubFunctionType.SupplyFunctionType.SupplyFunctionTypeName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.SupplySubFunctionType.SupplySubFunctionTypeName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.SupplySlot.SupplyColor.SupplyColorName.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.PartNumber.ToUpper().StartsWith(filter.ToUpper()) ||
                            obj.Capacity.ToString().ToUpper().StartsWith(filter.ToUpper()));
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
