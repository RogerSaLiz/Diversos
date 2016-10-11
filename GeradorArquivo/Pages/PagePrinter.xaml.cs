using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for PagePrinter.xaml
    /// </summary>
    public partial class PagePrinter : Page,INotifyPropertyChanged
    {
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
                RefleshButtons();
                OnPropertyChanged("SelectdPrinterModel");
            }
        }


        public PagePrinter()
        {
            InitializeComponent();
            ControlButtons.OnClickAdicionarEvent += OnClickAdicionarEvent;
            ControlButtons.OnClickEditarEvent += OnClickEditarEvent;
            ReadAll();
        }

        private void RefleshButtons()
        {
            ControlButtons.EditarEnable = _selectdPrinterModel != null;
            ControlButtons.ExcluirEnable = _selectdPrinterModel != null;
        }

        private void OnClickEditarEvent(object sender, EventArgs e)
        {
            var cw = new CWPrinterModel(SelectdPrinterModel);
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
            var cw = new CWPrinterModel();
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
            var db = new PrinterModelDB();
            CollectionPrinterModel.Clear();
            CollectionPrinterModel.AddRange(db.BuscaTodos());
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


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));



        }
    }
}
