using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using MahApps.Metro.Controls.Dialogs;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWCounter.xaml
    /// </summary>
    public partial class CWCounter 
    {
        private CounterType _oBCounterType= new CounterType();
        public CounterType OBCounterType
        {
            get { return _oBCounterType; }
            set
            {
                _oBCounterType = value;
                OnPropertyChanged("OBCounterType");
            }
        }
       
        private List<PrinterSupplyModelCounter> _listCounterPrinterOriginal = new List<PrinterSupplyModelCounter>();

        private ObservableCollection<PrinterSupplyModelCounter> _collectionCounterPrinters;
        public ObservableCollection<PrinterSupplyModelCounter> CollectionCounterPrinters
        {
            get { return _collectionCounterPrinters ?? (_collectionCounterPrinters = new ObservableCollection<PrinterSupplyModelCounter>()); }
            set
            {
                _collectionCounterPrinters = value;
                OnPropertyChanged("CollectionCounterPrinters");
            }
        }

        private PrinterSupplyModelCounter _selectedCounterPrinter;
        public PrinterSupplyModelCounter SelectedCounterPrinter
        {
            get { return _selectedCounterPrinter; }
            set
            {
                _selectedCounterPrinter = value;
                BtnRemover.IsEnabled = _selectedCounterPrinter != null;
                OnPropertyChanged("SelectedCounterPrinter");
            }
        }



        public CWCounter()
        {
            InitializeComponent();
            TbName.Focus();
        }

        public CWCounter(CounterType counter)
        {
            InitializeComponent();
            OBCounterType = (CounterType)counter.Clone();
            LoadPrinters();
            TbName.Focus();
        }

        private void LoadPrinters()
        {
            var db = new CounterTypeDB();
            CollectionCounterPrinters.Clear();
            CollectionCounterPrinters.AddRange(db.BuscaTodasImpressorasDoContador(OBCounterType.CounterTypeID));
            _listCounterPrinterOriginal.AddRange(CollectionCounterPrinters);
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            var db = new CounterTypeDB();
            if (OBCounterType.CounterTypeID == 0)
            {
                db.Adicionar(OBCounterType, GetListPrintersNewAddsupply(), () =>
                {
                    if (DialogResult == null)
                        DialogResult = true;
                });
            }
            else
            {
                db.Editar(OBCounterType, GetListPrintersNewAddsupply(),GetListPrintersRemovedSupply(), () =>
                {
                    if (DialogResult == null)
                        DialogResult = true;
                });
            }
        }


        private List<int> GetListPrintersNewAddsupply()
        {
            var ids = new List<int>();
            foreach (var item in CollectionCounterPrinters)
            {
                var exist = _listCounterPrinterOriginal.Any(p => p.PrinteModelID == item.PrinteModelID);
                if (!exist)
                    ids.Add(item.PrinteModelID);
            }
            return ids;
        }


        private List<int> GetListPrintersRemovedSupply()
        {
            var ids = new List<int>();
            foreach (var item in _listCounterPrinterOriginal)
            {
                var exist = CollectionCounterPrinters.Any(p => p.PrinteModelID == item.PrinteModelID);
                if (!exist)
                    ids.Add(item.PrinteModelID);
            }
            return ids;
        }

        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            var cw = new CWSearchPrinter();
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    if (cw.Dg.SelectedItems.Count > 0)
                    {
                        foreach (PrinterModel item in cw.Dg.SelectedItems)
                        {
                            CollectionCounterPrinters.Add(new PrinterSupplyModelCounter() { PrinteModelID = item.PrinteModelID, ModelName = item.ModelName, BrandName = item.Brand.BrandName });
                        }
                    }
                }
            };
            cw.ShowDialog();
        }

        private async void OnClickRemover(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("Remover", "Deseja realmente remover a impressora " + SelectedCounterPrinter.ModelName + " ?", MessageDialogStyle.AffirmativeAndNegative)) == MessageDialogResult.Affirmative)
            {
                CollectionCounterPrinters.Remove(SelectedCounterPrinter);
            }
        }
    }
}
