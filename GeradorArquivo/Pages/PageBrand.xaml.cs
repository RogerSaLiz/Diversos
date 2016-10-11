using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using GeradorArquivo.Windows;

namespace GeradorArquivo.Pages
{
    /// <summary>
    /// Interaction logic for PageBrand.xaml
    /// </summary>
    public partial class PageBrand : INotifyPropertyChanged
    {
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

        public PageBrand()
        {
            InitializeComponent();
            ControlButtons.OnClickAdicionarEvent += OnClickAdicionarEvent;
            ControlButtons.OnClickEditarEvent += OnClickEditarEvent;
            ControlButtons.OnClickExcluirEvent += OnClickExcluirEvent;
            ReadAll();
            
        }

        private void ReadAll()
        {
            var db = new BrandDB();
            CollectionBrand.Clear();
            CollectionBrand.AddRange(db.BuscaTodos());
        }

        private void OnClickExcluirEvent(object sender, EventArgs e)
        {
            
        }

        private void OnClickEditarEvent(object sender, EventArgs e)
        {
            var cw = new CWBrand(SelectdBrand);
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
            var cw = new CWBrand();
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    ReadAll();
                }
            };
            cw.ShowDialog();
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
