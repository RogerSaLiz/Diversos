using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GeradorArquivo.Annotations;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using GeradorArquivo.Windows;

namespace GeradorArquivo.Pages
{
    /// <summary>
    /// Interaction logic for PageEnterprises.xaml
    /// </summary>
    public partial class PageEnterprises : INotifyPropertyChanged
    {
        private Enterprise _selectedEnterprise;
        public Enterprise SelectedEnterprise
        {
            get { return _selectedEnterprise; }
            set
            {
                _selectedEnterprise = value;
                ControlButtons.ExcluirEnable = _selectedEnterprise != null;
                ControlButtons.EditarEnable = _selectedEnterprise != null;
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


        public PageEnterprises()
        {
            InitializeComponent(); 
            ControlButtons.OnClickAdicionarEvent += OnClickAdicionarEvent;
            ControlButtons.OnClickEditarEvent += OnClickEditarEvent;
            ControlButtons.OnClickExcluirEvent += OnClickExcluirEvent;
            ReadAll();

        }

        private void ReadAll()
        {
            var db = new EnterpriseDB();
            CollectionEnterprises.Clear();
            CollectionEnterprises.AddRange(db.BuscaTodos());
        }

        private void OnClickExcluirEvent(object sender, EventArgs e)
        {
           
        }

        private void OnClickEditarEvent(object sender, EventArgs e)
        {
            var cw = new CWEnterprise(SelectedEnterprise);
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
            var cw = new CWEnterprise();
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
