using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using MahApps.Metro.Controls.Dialogs;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWPrinterModel.xaml
    /// </summary>
    public partial class CWPrinterModel : INotifyPropertyChanged
    {
        #region PROPRIEDADES

        private List<PrinterSupplyModelCounter> _listPrinterSupplyModelOriginal = new List<PrinterSupplyModelCounter>();

        private ObservableCollection<PrinterSupplyModelCounter> _collectionSupplyModel;
        public ObservableCollection<PrinterSupplyModelCounter> CollectionSupplyModel
        {
            get { return _collectionSupplyModel ?? (_collectionSupplyModel = new ObservableCollection<PrinterSupplyModelCounter>()); }
            set
            {
                _collectionSupplyModel = value;
                OnPropertyChanged("CollectionSupplyModel");
            }
        }

        private PrinterSupplyModelCounter _selectedSupplyModel;
        public PrinterSupplyModelCounter SelectedSupplyModel
        {
            get { return _selectedSupplyModel; }
            set
            {
                _selectedSupplyModel = value;
                BtnRemover.IsEnabled = _selectedSupplyModel != null;
                OnPropertyChanged("SelectedSupplyModel");
            }
        }

        private List<PrinterSupplyModelCounter> _listCounterPrinterOriginal = new List<PrinterSupplyModelCounter>();
        private ObservableCollection<PrinterSupplyModelCounter> _collectionCounterPrinter;
        public ObservableCollection<PrinterSupplyModelCounter> CollectionCounterPrinter
        {
            get { return _collectionCounterPrinter ?? (_collectionCounterPrinter = new ObservableCollection<PrinterSupplyModelCounter>()); }
            set
            {
                _collectionCounterPrinter = value;
                OnPropertyChanged("CollectionCounterPrinter");
            }
        }

        private PrinterSupplyModelCounter _selectedCounterPrinter;
        public PrinterSupplyModelCounter SelectedCounterPrinter
        {
            get { return _selectedCounterPrinter; }
            set
            {
                _selectedCounterPrinter = value;
                if (_selectedCounterPrinter != null && _selectedCounterPrinter.CounterTypeID == 1)
                    BtnRemoverCounter.IsEnabled = false;
                else
                    BtnRemoverCounter.IsEnabled = _selectedCounterPrinter != null;
                OnPropertyChanged("SelectedCounterPrinter");
            }
        }

        private PrinterModel _oBPrinterModel = new PrinterModel();
        public PrinterModel OBPrinterModel
        {
            get { return _oBPrinterModel; }
            set
            {
                _oBPrinterModel = value;
                OnPropertyChanged("OBPrinterModel");
            }
        }

        private ObservableCollection<Brand> _collectionBrand;
        public ObservableCollection<Brand> CollectionBrand
        {
            get { return _collectionBrand ?? (_collectionBrand = new ObservableCollection<Brand>()); }
            set
            {
                _collectionBrand = value;
                OnPropertyChanged("CollectionBrand");
            }
        }

        private Brand _selectdBrand;
        private List<BehaviorSupply> _listBehaviorSupplies = new List<BehaviorSupply>();

        public Brand SelectdBrand
        {
            get { return _selectdBrand; }
            set
            {
                _selectdBrand = value;
                OnPropertyChanged("SelectdBrand");
            }
        }

        #endregion

        #region CONSTRUTOR

        public CWPrinterModel()
        {
            InitializeComponent();
            LoadAllBrands();
            TbModelName.Focus();
        }

        public CWPrinterModel(PrinterModel printer)
        {
            InitializeComponent();
            OBPrinterModel = (PrinterModel)printer.Clone();
            LoadAllBrands();
            //   LoadSupplyModels();
            RetrieveAllBehaviorSupply(LoadSupplyModels);
            LoadCounterPrinters();
            TbModelName.Focus();
        }

        #endregion

        private void RetrieveAllBehaviorSupply(Action ok)
        {
            var db = new BehaviorSupplyDB();
            _listBehaviorSupplies.AddRange(db.BuscaTodos());
            ok.Invoke();
        }

        private void LoadCounterPrinters()
        {
            var db = new PrinterModelDB();
            CollectionCounterPrinter.Clear();
            var list = db.BuscaTodosContadores(OBPrinterModel.PrinteModelID);
            CollectionCounterPrinter.AddRange(list);
            _listCounterPrinterOriginal = list.Select(objEntity => (PrinterSupplyModelCounter)objEntity.Clone()).ToList();
        }

        private void LoadAllBrands()
        {
            var db = new BrandDB();
            CollectionBrand.Clear();
            CollectionBrand.AddRange(db.BuscaTodos());
            if (OBPrinterModel == null || OBPrinterModel.PrinteModelID == 0)
                SelectdBrand = CollectionBrand.FirstOrDefault();
            else
                SelectdBrand = CollectionBrand.First(p => p.BrandID == OBPrinterModel.Brand.BrandID);

        }

        private void LoadSupplyModels()
        {
            var db = new PrinterSupplyModelDB();
            CollectionSupplyModel.Clear();
            _listPrinterSupplyModelOriginal.Clear();
            foreach (var item in db.BuscaTodos(OBPrinterModel.PrinteModelID, 0))
            {
                item.ListBehSupply = _listBehaviorSupplies;
                item.BehSupply = item.ListBehSupply.First(p => p.BehaviorSupplyID == item.BehSupply.BehaviorSupplyID);
                CollectionSupplyModel.Add(item);
                _listPrinterSupplyModelOriginal.Add(item);
            }

        }

        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OBPrinterModel.ModelName))
            {
                MessageBox.Show("Digite um nome para a Impressora!!!", "Impressora sem Nome", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (SelectdBrand == null || SelectdBrand.BrandID == 0)
            {
                MessageBox.Show("Não existe fabricante cadastrado!!!", "Sem fabricante", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var dbCliente = new PrinterModelDB();
            OBPrinterModel.Brand = new Brand() { BrandID = SelectdBrand.BrandID };
            if (OBPrinterModel.PrinteModelID > 0)
                dbCliente.Editar(OBPrinterModel, GetListPrintersNewAddsupply(), GetListPrintersRemovedSupply(),
                    GetListPrinterCounterAdd(), GetListPrinterCounterRemove(), GetListPrinterCounterChange(),
                    delegate
                    {
                        if (DialogResult == null)
                            DialogResult = true;
                    });
            else
                dbCliente.Adicionar(OBPrinterModel, GetListPrintersNewAddsupply(), GetListPrinterCounterAdd(), delegate
                {
                    if (DialogResult == null)
                        DialogResult = true;
                });
        }

        private List<PrinterSupplyModelCounter> GetListPrintersNewAddsupply()
        {
            var ids = new List<PrinterSupplyModelCounter>();
            foreach (var item in CollectionSupplyModel)
            {
                var exist = _listPrinterSupplyModelOriginal.Any(p => p.SupplyModelId == item.SupplyModelId);
                if (!exist)
                    ids.Add(item);
            }
            return ids;
        }

        private List<int> GetListPrintersRemovedSupply()
        {
            var ids = new List<int>();
            foreach (var item in _listPrinterSupplyModelOriginal)
            {
                var exist = CollectionSupplyModel.Any(p => p.SupplyModelId == item.SupplyModelId);
                if (!exist)
                    ids.Add(item.SupplyModelId);
            }
            return ids;
        }

        private List<PrinterSupplyModelCounter> GetListPrinterCounterChange()
        {
            var ids = new List<PrinterSupplyModelCounter>();
            foreach (var item in CollectionCounterPrinter)
            {
                var ob = _listCounterPrinterOriginal.FirstOrDefault(p => p.CounterTypeID == item.CounterTypeID && (p.Mono != item.Mono || p.Color != item.Color || p.Total != item.Total));
                if (ob != null)
                    ids.Add(item);
            }
            return ids;
        }

        private List<PrinterSupplyModelCounter> GetListPrinterCounterAdd()
        {
            var ids = new List<PrinterSupplyModelCounter>();
            foreach (var item in CollectionCounterPrinter)
            {
                var exist = _listCounterPrinterOriginal.Any(p => p.CounterTypeID == item.CounterTypeID);
                if (!exist)
                    ids.Add(item);
            }
            return ids;
        }

        private List<int> GetListPrinterCounterRemove()
        {
            var ids = new List<int>();
            foreach (var item in _listCounterPrinterOriginal)
            {
                var exist = CollectionCounterPrinter.Any(p => p.CounterTypeID == item.CounterTypeID);
                if (!exist)
                    ids.Add(item.CounterTypeID);
            }
            return ids;
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            var cw = new CWSearchSupply(CollectionSupplyModel);
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    if (cw.Dg.SelectedItems.Count > 0)
                    {
                        foreach (SupplyModel item in cw.Dg.SelectedItems)
                        {
                            var itemList = new PrinterSupplyModelCounter();
                            itemList.PartNumber = item.PartNumber;
                            itemList.Capacity = item.Capacity;
                            itemList.BrandName = item.Brand.BrandName;
                            itemList.Description = item.Description;
                            itemList.SupplyColorName = item.SupplySlot.SupplyColor.SupplyColorName;
                            itemList.SupplyModelId = item.SupplyModelId;
                            itemList.ListBehSupply = item.ListBehSupply;
                            itemList.BehSupply = item.ListBehSupply.First(p => p.BehaviorSupplyID == item.BehSupply.BehaviorSupplyID);
                            CollectionSupplyModel.Add(itemList);
                        }

                    }
                }
            };
            cw.ShowDialog();
        }

        private async void OnClickRemover(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("Remover", "Deseja realmente remover o suprimento " + SelectedSupplyModel.Description + " ?", MessageDialogStyle.AffirmativeAndNegative)) == MessageDialogResult.Affirmative)
            {
                CollectionSupplyModel.Remove(SelectedSupplyModel);
            }
        }

        private async void OnClickRemoverCounter(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("Remover", "Deseja realmente remover o contador " + SelectedCounterPrinter.CounterTypeName + " ?", MessageDialogStyle.AffirmativeAndNegative)) == MessageDialogResult.Affirmative)
            {
                CollectionCounterPrinter.Remove(SelectedCounterPrinter);
            }
        }

        private void OnClickAddCounter(object sender, RoutedEventArgs e)
        {
            var cw = new CWSearchCounter(CollectionCounterPrinter);
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    if (cw.Dg.SelectedItems.Count > 0)
                    {
                        foreach (CounterType item in cw.Dg.SelectedItems)
                        {
                            CollectionCounterPrinter.Add(new PrinterSupplyModelCounter() { CounterTypeID = item.CounterTypeID, CounterTypeName = item.CounterTypeName });
                        }
                    }
                }
            };
            cw.ShowDialog();
        }
    }
}
