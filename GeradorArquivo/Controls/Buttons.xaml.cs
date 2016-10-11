using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GeradorArquivo.Annotations;

namespace GeradorArquivo.Controls
{
    /// <summary>
    /// Interaction logic for Buttons.xaml
    /// </summary>
    public partial class Buttons : UserControl, INotifyPropertyChanged
    {
        public EventHandler OnClickAdicionarEvent;
        public EventHandler OnClickEditarEvent;
        public EventHandler OnClickExcluirEvent;

        private bool _editarEnable = false;
        public bool EditarEnable
        {
            get { return _editarEnable; }
            set
            {
                _editarEnable = value;
                OnPropertyChanged("EditarEnable");
            }
        }

        private bool _excluirEnable;
        public bool ExcluirEnable
        {
            get { return _excluirEnable; }
            set
            {
                _excluirEnable = value;
                OnPropertyChanged("ExcluirEnable");
            }
        }

        public Buttons()
        {
            InitializeComponent();
        }

        private void OnClickAdicionar(object sender, RoutedEventArgs e)
        {
            if (OnClickAdicionarEvent != null)
                OnClickAdicionarEvent.Invoke(sender, e);
        }

        private void OnClickEditar(object sender, RoutedEventArgs e)
        {
            if (OnClickEditarEvent != null)
                OnClickEditarEvent.Invoke(sender, e);
        }

        private void OnClickExcluir(object sender, RoutedEventArgs e)
        {
            if (OnClickExcluirEvent != null)
                OnClickExcluirEvent.Invoke(sender, e);
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
