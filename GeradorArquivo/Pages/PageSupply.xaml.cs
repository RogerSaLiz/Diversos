using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using GeradorArquivo.Windows;

namespace GeradorArquivo.Pages
{
    /// <summary>
    /// Interaction logic for PageSupply.xaml
    /// </summary>
    public partial class PageSupply : Page, INotifyPropertyChanged
    {
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
                RefleshButtons();
                OnPropertyChanged("SelectedSupplyModel");
            }
        }

        public PageSupply()
        {
            InitializeComponent();
            ControlButtons.OnClickAdicionarEvent += OnClickAdicionarEvent;
            ControlButtons.OnClickEditarEvent += OnClickEditarEvent;
            ReadAll();
        }


        private void RefleshButtons()
        {
            ControlButtons.EditarEnable = _selectedSupplyModel != null;
            ControlButtons.ExcluirEnable = _selectedSupplyModel != null;
        }
        private void OnClickEditarEvent(object sender, EventArgs e)
        {
            var cw = new CWSupply(SelectedSupplyModel);
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    ReadAll();
                }
            };
            cw.ShowDialog();
        }

        private void OnClickAdicionarEvent(object sender, EventArgs e)
        {
            var cw = new CWSupply();
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    ReadAll();
                }
            };
            cw.ShowDialog();
        }

        private void ReadAll()
        {
            var db = new SupplyDB();
            CollectionSupplyModel.Clear();
            CollectionSupplyModel.AddRange(db.BuscaTodos());
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
                    var supply = o as SupplyModel;
                    return (supply.Description.ToUpper().StartsWith(filter.ToUpper()) ||
                            supply.Brand.BrandName.ToUpper().StartsWith(filter.ToUpper()) ||
                            supply.SupplySubFunctionType.SupplyFunctionType.SupplyFunction.SupplyFunctionName.ToUpper().StartsWith(filter.ToUpper()) ||
                            supply.SupplySubFunctionType.SupplyFunctionType.SupplyFunctionTypeName.ToUpper().StartsWith(filter.ToUpper()) ||
                            supply.SupplySubFunctionType.SupplySubFunctionTypeName.ToUpper().StartsWith(filter.ToUpper()) ||
                            supply.SupplySlot.SupplyColor.SupplyColorName.ToUpper().StartsWith(filter.ToUpper())||
                            supply.PartNumber.ToUpper().StartsWith(filter.ToUpper())||
                            supply.Capacity.ToString().ToUpper().StartsWith(filter.ToUpper()));
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



    }
}
