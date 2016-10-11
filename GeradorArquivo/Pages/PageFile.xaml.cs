using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using GeradorArquivo.Helper;

namespace GeradorArquivo.Pages
{
    /// <summary>
    /// Interaction logic for PageFile.xaml
    /// </summary>
    public partial class PageFile : INotifyPropertyChanged
    {
        #region PROPRIEDADES


        private ObservableCollection<OperationSystem> _collectionOperationSystem;
        public ObservableCollection<OperationSystem> CollectionOperationSystem
        {
            get { return _collectionOperationSystem ?? (_collectionOperationSystem = new ObservableCollection<OperationSystem>()); }
            set
            {
                _collectionOperationSystem = value;
                OnPropertyChanged("CollectionOperationSystem");
            }
        }

        private OperationSystem _selectedOperationSystem;
        public OperationSystem SelectedOperationSystem
        {
            get { return _selectedOperationSystem; }
            set
            {
                _selectedOperationSystem = value;
                OnPropertyChanged("SelectedOperationSystem");
            }
        }


        private ObservableCollection<Product> _collectionProducts;
        public ObservableCollection<Product> CollectionProducts
        {
            get { return _collectionProducts ?? (_collectionProducts = new ObservableCollection<Product>()); }
            set
            {
                _collectionProducts = value;
                OnPropertyChanged("CollectionProducts");
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private Enterprise _selectedEnterprise;
        public Enterprise SelectedEnterprise
        {
            get { return _selectedEnterprise; }
            set
            {
                _selectedEnterprise = value;
                OnPropertyChanged("SelectedEnterprise");
            }
        }

        private ObservableCollection<Enterprise> _collectionEnterprises;
        public ObservableCollection<Enterprise> CollectionEnterprises
        {
            get { return _collectionEnterprises ?? (_collectionEnterprises = new ObservableCollection<Enterprise>()); }
            set
            {
                _collectionEnterprises = value;
                OnPropertyChanged("CollectionEnterprises");
            }
        }

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

        private ObservableCollection<PrinterModel> _collectionPrinterModelSelecteds;
        public ObservableCollection<PrinterModel> CollectionPrinterModelSelecteds
        {
            get { return _collectionPrinterModelSelecteds ?? (_collectionPrinterModelSelecteds = new ObservableCollection<PrinterModel>()); }
            set
            {
                _collectionPrinterModelSelecteds = value;
                OnPropertyChanged("CollectionPrinterModelSelecteds");
            }
        }

        private DateTime _dateStart = DateTime.Now;
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                OnPropertyChanged("DateStart");
            }
        }

        private DateTime _dateEnd = DateTime.Now.AddDays(30);
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                _dateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }

        private int _intervalCounters = 10;
        public int IntervalCounters
        {
            get { return _intervalCounters; }
            set
            {
                _intervalCounters = value;
                OnPropertyChanged("IntervalCounters");
            }
        }

        #endregion

        #region CONSTRUTOR

        public PageFile()
        {
            InitializeComponent();
            RetrieveAll();
           
        }

        #endregion

        private void IPAddress()
        {
            TbNameComputer.Text = Environment.MachineName;
            TbIP.Text= Dns.GetHostByName(Environment.MachineName).AddressList[1].ToString();

        }

        private void RetrieveAll()
        {
            LoadAllPrinterModels();
            LoadAllEnterprises();
            LoadAllProducts();
            LoadAllOperationSystem();
            IPAddress();
        }

        private void LoadAllPrinterModels()
        {
            var db = new PrinterModelDB();
            CollectionPrinterModel.Clear();
            CollectionPrinterModel.AddRange(db.BuscaTodos());
        }
        
        private void LoadAllProducts()
        {
            var db = new ProductDB();
            CollectionProducts.Clear();
            CollectionProducts.AddRange(db.BuscaTodos());
            SelectedProduct = CollectionProducts.FirstOrDefault();
        }

        private void LoadAllEnterprises()
        {
            var db = new EnterpriseDB();
            CollectionEnterprises.Clear();
            CollectionEnterprises.AddRange(db.BuscaTodos());
            SelectedEnterprise = CollectionEnterprises.FirstOrDefault();
        }

        private void LoadAllOperationSystem()
        {
            var db = new OperationSystemDB();
            CollectionOperationSystem.Clear();
            CollectionOperationSystem.AddRange(db.BuscaTodos());
            SelectedOperationSystem = CollectionOperationSystem.FirstOrDefault();
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
                            obj.ModelName.StartsWith(filter.ToUpper()));
                };
            }
        }

        private void OnKeyRemoveEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                TbSearch.Text = string.Empty;
        }

        private void OnClickAddPrinter(object sender, RoutedEventArgs e)
        {
            if (CollectionPrinterModel.Count == 0 || Dg.SelectedItems.Count == 0)
                PrinterNotSelected();
            else
                AddPrinter(() => { });
        }

        private void AddPrinter(Action ok)
        {
            var listIDs = new List<int>();
            foreach (PrinterModel item in Dg.SelectedItems)
            {
                CollectionPrinterModelSelecteds.Add(item);
                listIDs.Add(item.PrinteModelID);
            }


            foreach (var item in listIDs)
            {
                var printerModel = CollectionPrinterModel.FirstOrDefault(p => p.PrinteModelID == item);
                if (printerModel != null)
                    CollectionPrinterModel.Remove(printerModel);
            }
            ok.Invoke();
        }

        private void OnClickRemovePrinter(object sender, RoutedEventArgs e)
        {
            if (CollectionPrinterModelSelecteds.Count == 0 || DgPrintersSelecteds.SelectedItems.Count == 0)
                PrinterNotSelected();
            else
                RemovePrinter(() => { });
        }

        private void RemovePrinter(Action ok)
        {
            var listIDs = new List<int>();
            foreach (PrinterModel item in DgPrintersSelecteds.SelectedItems)
            {
                CollectionPrinterModel.AddSorted(item, item.Index);
                listIDs.Add(item.PrinteModelID);
            }

            foreach (var item in listIDs)
            {
                var printerModel = CollectionPrinterModelSelecteds.FirstOrDefault(p => p.PrinteModelID == item);
                if (printerModel != null)
                    CollectionPrinterModelSelecteds.Remove(printerModel);
            }
            ok.Invoke();
        }

        public async Task<MessageDialogResult> PrinterNotSelected()
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            return await metroWindow.ShowMessageAsync("Impressora", "Selecione a(s) impressora(s)!!!");
        }

        private void OnClickNextWizzard1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbVersionProduct.Text))
            {
                this.TryFindParent<MetroWindow>().ShowMessageAsync("Campo em Branco", "Digite a versão do produto!!!");
                TbVersionProduct.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbNameComputer.Text))
            {
                this.TryFindParent<MetroWindow>().ShowMessageAsync("Campo em Branco", "Digite o nome do computador!!!");
                TbNameComputer.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbIP.Text))
            {
                this.TryFindParent<MetroWindow>().ShowMessageAsync("Campo em Branco", "Digite o IP do computador!!!");
                TbNameComputer.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbMac.Text))
            {
                this.TryFindParent<MetroWindow>().ShowMessageAsync("Campo em Branco", "Digite a máscara da Sub-rede!!!");
                TbMac.Focus();
                return;
            }
            
            TbWizzard1.Visibility = Visibility.Collapsed;
            TbWizzard2.Visibility = Visibility.Visible;
            TbWizzard2.IsSelected = true;
        }
        
       

        private void OnClickBeforeWizzard2(object sender, RoutedEventArgs e)
        {
            TbWizzard1.Visibility = Visibility.Visible;
            TbWizzard2.Visibility = Visibility.Collapsed;
            TbWizzard1.IsSelected = true;
        }

        private void OnClickNextWizzard2(object sender, RoutedEventArgs e)
        {
            if (CollectionPrinterModelSelecteds.Count == 0)
                PrinterNotSelected();
            else
            {
                TbWizzard3.Visibility = Visibility.Visible;
                TbWizzard2.Visibility = Visibility.Collapsed;
                TbWizzard3.IsSelected = true;
            }
        }

        private void OnClickBeforeWizzard3(object sender, RoutedEventArgs e)
        {
            TbWizzard3.Visibility = Visibility.Collapsed;
            TbWizzard2.Visibility = Visibility.Visible;
            TbWizzard2.IsSelected = true;
        }

        private void OnClickNextWizzard3(object sender, RoutedEventArgs e)
        {

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
