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
    /// Interaction logic for CWSupply.xaml
    /// </summary>
    public partial class CWSupply  
    {
        #region PROPRIEDADES
        private List<PrinterSupplyModelCounter> _listPrinterSupplyModelOriginal = new List<PrinterSupplyModelCounter>();

        private ObservableCollection<PrinterSupplyModelCounter> _collectionPrinterSupplyModel;
        public ObservableCollection<PrinterSupplyModelCounter> CollectionPrinterSupplyModel
        {
            get { return _collectionPrinterSupplyModel ?? (_collectionPrinterSupplyModel = new ObservableCollection<PrinterSupplyModelCounter>()); }
            set
            {
                _collectionPrinterSupplyModel = value;
                OnPropertyChanged("CollectionPrinterSupplyModel");
            }
        }

        private PrinterSupplyModelCounter _selectedPrinterSupplyModel;
        public PrinterSupplyModelCounter SelectedPrinterSupplyModel
        {
            get { return _selectedPrinterSupplyModel; }
            set
            {
                _selectedPrinterSupplyModel = value;
                BtnRemover.IsEnabled = _selectedPrinterSupplyModel != null;
                OnPropertyChanged("SelectedPrinterSupplyModel");
            }
        }

        private ObservableCollection<SupplyFunctions> _collectionSupplyFunctions;
        public ObservableCollection<SupplyFunctions> CollectionSupplyFunctions
        {
            get { return _collectionSupplyFunctions ?? (_collectionSupplyFunctions = new ObservableCollection<SupplyFunctions>()); }
            set
            {
                _collectionSupplyFunctions = value;
                OnPropertyChanged("CollectionSupplyFunctions");
            }
        }

        private SupplyFunctions _selectedSupplyFunctions;
        public SupplyFunctions SelectedSupplyFunctions
        {
            get { return _selectedSupplyFunctions; }
            set
            {
                _selectedSupplyFunctions = value;
                OnPropertyChanged("SelectedSupplyFunctions");
            }
        }


        private ObservableCollection<SupplyFunctionTypes> _collectionSupplyFunctionTypes;
        public ObservableCollection<SupplyFunctionTypes> CollectionSupplyFunctionTypes
        {
            get { return _collectionSupplyFunctionTypes ?? (_collectionSupplyFunctionTypes = new ObservableCollection<SupplyFunctionTypes>()); }
            set
            {
                _collectionSupplyFunctionTypes = value;
                OnPropertyChanged("CollectionSupplyFunctionTypes");
            }
        }

        private SupplyFunctionTypes _selectedSupplyFunctionTypes;
        public SupplyFunctionTypes SelectedSupplyFunctionTypes
        {
            get { return _selectedSupplyFunctionTypes; }
            set
            {
                _selectedSupplyFunctionTypes = value;
                OnPropertyChanged("SelectedSupplyFunctionTypes");
            }
        }

        private ObservableCollection<SupplyColors> _collectionSupplyColors;
        public ObservableCollection<SupplyColors> CollectionSupplyColors
        {
            get { return _collectionSupplyColors ?? (_collectionSupplyColors = new ObservableCollection<SupplyColors>()); }
            set
            {
                _collectionSupplyColors = value;
                OnPropertyChanged("CollectionSupplyColors");
            }
        }

        private SupplyColors _selectedSupplyColors;
        public SupplyColors SelectedSupplyColors
        {
            get { return _selectedSupplyColors; }
            set
            {
                _selectedSupplyColors = value;
                OnPropertyChanged("SelectedSupplyColors");
            }
        }

        private ObservableCollection<SupplySubFunctionTypes> _collectionSupplySubFunctionTypes;
        public ObservableCollection<SupplySubFunctionTypes> CollectionSupplySubFunctionTypes
        {
            get { return _collectionSupplySubFunctionTypes ?? (_collectionSupplySubFunctionTypes = new ObservableCollection<SupplySubFunctionTypes>()); }
            set
            {
                _collectionSupplySubFunctionTypes = value;
                OnPropertyChanged("CollectionSupplySubFunctionTypes");
            }
        }

        private SupplySubFunctionTypes _selectedSupplySubFunctionTypes;
        public SupplySubFunctionTypes SelectedSupplySubFunctionTypes
        {
            get { return _selectedSupplySubFunctionTypes; }
            set
            {
                _selectedSupplySubFunctionTypes = value;
                if (_selectedSupplySubFunctionTypes != null)
                    LoadSelected();
                OnPropertyChanged("SelectedSupplySubFunctionTypes");
            }
        }


        private SupplyModel _oBSupplyModel = new SupplyModel();
        public SupplyModel OBSupplyModel
        {
            get { return _oBSupplyModel; }
            set
            {
                _oBSupplyModel = value;
                OnPropertyChanged("OBSupplyModel");
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
        public Brand SelectdBrand
        {
            get { return _selectdBrand; }
            set
            {
                _selectdBrand = value;
                OnPropertyChanged("SelectdBrand");
            }
        }

        private ObservableCollection<BehaviorSupply> _collectionBehaviorSupply;
        public ObservableCollection<BehaviorSupply> CollectionBehaviorSupply
        {
            get { return _collectionBehaviorSupply ?? (_collectionBehaviorSupply = new ObservableCollection<BehaviorSupply>()); }
            set
            {
                _collectionBehaviorSupply = value;
                OnPropertyChanged("CollectionBehaviorSupply");
            }
        }

        private BehaviorSupply _selectedBehaviorSupply;
        public BehaviorSupply SelectedBehaviorSupply
        {
            get { return _selectedBehaviorSupply; }
            set
            {
                _selectedBehaviorSupply = value;
                OnPropertyChanged("SelectedBehaviorSupply");
            }
        }


        #endregion

        #region CONSTRUTOR

        public CWSupply()
        {
            InitializeComponent();
            LoadInital();
        }

        public CWSupply(SupplyModel supply)
        {
            InitializeComponent();
            OBSupplyModel = supply;
            LoadInital();

        }

        #endregion

        #region CARREGAR

        private void LoadBeharviors(Action ok)
        {
            var db = new BehaviorSupplyDB();
            CollectionBehaviorSupply.Clear();
            CollectionBehaviorSupply.AddRange(db.BuscaTodos());
            SelectedBehaviorSupply = OBSupplyModel.BehSupply != null ? CollectionBehaviorSupply.First(p => p.BehaviorSupplyID == OBSupplyModel.BehSupply.BehaviorSupplyID) : CollectionBehaviorSupply.First();
            ok.Invoke();
        }


        private void LoadPrinterSupplyModel()
        {
            var db = new PrinterSupplyModelDB();
            CollectionPrinterSupplyModel.Clear();
            CollectionPrinterSupplyModel.AddRange(db.BuscaTodos(0, OBSupplyModel.SupplyModelId));
            _listPrinterSupplyModelOriginal.AddRange(CollectionPrinterSupplyModel);
        }

        private void LoadBrand(Action completed)
        {
            var db = new BrandDB();
            CollectionBrand.Clear();
            CollectionBrand.AddRange(db.BuscaTodos());
            SelectdBrand = OBSupplyModel.Brand != null ? CollectionBrand.First(p => p.BrandID == OBSupplyModel.Brand.BrandID) : CollectionBrand.First();
            completed.Invoke();
        }

        private void LoadSupplyColors(Action completed)
        {
            var db = new SupplyColorsDB();
            CollectionSupplyColors.Clear();
            CollectionSupplyColors.AddRange(db.BuscaEspecifica(SelectedSupplyFunctionTypes.SupplyFunctionTypeID));
            SelectedSupplyColors = OBSupplyModel.SupplySlot != null
                ? CollectionSupplyColors.First(
                    p => p.SupplyColorID == OBSupplyModel.SupplySlot.SupplyColor.SupplyColorID)
                : CollectionSupplyColors.First();
            completed.Invoke();
        }

        private void LoadSupplySubFunctionTypes(Action completed)
        {
            var db = new SupplySubFunctionTypesDB();
            CollectionSupplySubFunctionTypes.Clear();
            CollectionSupplySubFunctionTypes.AddRange(db.BuscaTodos());
            SelectedSupplySubFunctionTypes = OBSupplyModel.SupplySubFunctionType != null ? CollectionSupplySubFunctionTypes.First(p => p.SupplySubFunctionTypeID == OBSupplyModel.SupplySubFunctionType.SupplySubFunctionTypeID) : CollectionSupplySubFunctionTypes.First();
            completed.Invoke();
        }

        private void LoadSupplyFunctionTypes(Action completed)
        {
            var db = new SupplyFunctionTypesDB();
            CollectionSupplyFunctionTypes.Clear();
            CollectionSupplyFunctionTypes.AddRange(db.BuscaEspecifica(SelectedSupplySubFunctionTypes.SupplyFunctionType.SupplyFunctionTypeID));
            SelectedSupplyFunctionTypes = OBSupplyModel.SupplySubFunctionType != null ? CollectionSupplyFunctionTypes.First(p => p.SupplyFunctionTypeID == OBSupplyModel.SupplySubFunctionType.SupplyFunctionType.SupplyFunctionTypeID) : CollectionSupplyFunctionTypes.First();
            completed.Invoke();
        }

        private void LoadSupplyFunction(Action completed)
        {
            var db = new SupplyFunctionsDB();
            CollectionSupplyFunctions.Clear();
            CollectionSupplyFunctions.AddRange(db.BuscaEspecifica(SelectedSupplyFunctionTypes.SupplyFunction.SupplyFunctionID));
            SelectedSupplyFunctions = OBSupplyModel.SupplySubFunctionType != null ? CollectionSupplyFunctions.First(p => p.SupplyFunctionID == OBSupplyModel.SupplySubFunctionType.SupplyFunctionType.SupplyFunction.SupplyFunctionID) : CollectionSupplyFunctions.First();
            completed.Invoke();
        }

        private void LoadInital()
        {

            LoadBrand(() =>
            {
                LoadSupplySubFunctionTypes(() =>
                {
                    LoadBeharviors(LoadPrinterSupplyModel);
                });
            });
        }

        private void LoadSelected()
        {
            LoadSupplyFunctionTypes(() =>
            {
                LoadSupplyFunction(() =>
                {
                    LoadSupplyColors(() => { });
                });
            });
        }

        private List<int> GetListPrintersNewAddsupply()
        {
            var ids = new List<int>();
            foreach (var item in CollectionPrinterSupplyModel)
            {
                var exist = _listPrinterSupplyModelOriginal.Any(p => p.PrinteModelID == item.PrinteModelID);
                if (!exist)
                    ids.Add(item.PrinteModelID);
            }
            return ids;
        }


        private List<int> GetListPrintersRemovedSupply()
        {
            var ids = new List<int>();
            foreach (var item in _listPrinterSupplyModelOriginal)
            {
                var exist = CollectionPrinterSupplyModel.Any(p => p.PrinteModelID == item.PrinteModelID);
                if (!exist)
                    ids.Add(item.PrinteModelID);
            }
            return ids;
        }

        #endregion

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OBSupplyModel.PartNumber))
            {
                this.ShowMessageAsync("Campo em Branco", "O Campo PartNumber está em branco!!!");
                return;
            }

            if (string.IsNullOrWhiteSpace(OBSupplyModel.Capacity.ToString()))
            {
                this.ShowMessageAsync("Campo em Branco", "O Campo capacidade está em branco!!!");
                return;
            }
            var regex = new Regex("^[0-9]+$");
            var onlyNumber = regex.Match(TbPartNumbers.Text);
            if (!onlyNumber.Success)
            {
                this.ShowMessageAsync("Somente Número", "O Campo Capacidade deve conter somente números!!!");
                return;
            }

            if (string.IsNullOrWhiteSpace(OBSupplyModel.Description))
            {
                this.ShowMessageAsync("Campo em Branco", "O Campo Descrição está em branco!!!");
                return;
            }

            OBSupplyModel.Brand = SelectdBrand;
            OBSupplyModel.SupplySubFunctionType = SelectedSupplySubFunctionTypes;
            OBSupplyModel.SupplySlot = new SupplySlots() { SupplySlotID = SelectedSupplyColors.SupplySlotID };
            OBSupplyModel.BehSupply = SelectedBehaviorSupply;
            var db = new SupplyDB();
            if (OBSupplyModel.SupplyModelId == 0)
            {
                db.Adicionar(OBSupplyModel, GetListPrintersNewAddsupply(), () =>
                {
                    if (DialogResult == null)
                        DialogResult = true;
                });
            }
            else
            {
                DbEditar(OBSupplyModel, () =>
                {
                    if (DialogResult == null)
                        DialogResult = true;
                });


            }

        }

        private void DbEditar(SupplyModel objeto, Action completed)
        {
            var db = new SupplyDB();
            db.Editar(objeto, GetListPrintersNewAddsupply(), GetListPrintersRemovedSupply(), () =>
            {
                completed.Invoke();
            });
        }

        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            var cw = new CWSearchPrinter(CollectionPrinterSupplyModel);
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    if (cw.Dg.SelectedItems.Count > 0)
                    {
                        foreach (PrinterModel item in cw.Dg.SelectedItems)
                        {
                            CollectionPrinterSupplyModel.Add(new PrinterSupplyModelCounter() { PrinteModelID = item.PrinteModelID, ModelName = item.ModelName, BrandName = item.Brand.BrandName, NameXML = item.NameXML});
                        }
                    }
                }
            };
            cw.ShowDialog();
        }

        private async void OnClickRemover(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("Remover", "Deseja realmente remover a impressora " + SelectedPrinterSupplyModel.ModelName + " ?", MessageDialogStyle.AffirmativeAndNegative)) == MessageDialogResult.Affirmative)
            {
                CollectionPrinterSupplyModel.Remove(SelectedPrinterSupplyModel);
            }
        }
    }


}
