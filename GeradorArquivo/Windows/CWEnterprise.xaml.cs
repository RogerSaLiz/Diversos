using System;
using System.Collections.Generic;
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
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;
using MahApps.Metro.Controls.Dialogs;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWEnterprise.xaml
    /// </summary>
    public partial class CWEnterprise 
    {
        private Enterprise _oBEnterprise=new Enterprise();
        public Enterprise OBEnterprise
        {
            get { return _oBEnterprise; }
            set
            {
                _oBEnterprise = value;
                OnPropertyChanged("OBEnterprise");
            }
        }

        public CWEnterprise()
        {
            InitializeComponent();
            TbName.Focus();
        }

        public CWEnterprise(Enterprise enterprise)
        {
            InitializeComponent();
            TbName.Focus();
            OBEnterprise = (Enterprise)enterprise.Clone();
        }

        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text))
            {
                EnterpriseNameError();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbKey.Text))
            {
                EnterpriseKeyError();
                return;
            }

            if (OBEnterprise.EnterpriseID == 0)
            {
                var db = new EnterpriseDB();
                db.Adicionar(OBEnterprise, () =>
                {
                    DialogResult = true;
                });
            }
            else
            {
                var db = new EnterpriseDB();
                db.Editar(OBEnterprise, () =>
                {
                    DialogResult = true;
                });
            }


        }

        private async void EnterpriseNameError()
        {
            await this.ShowMessageAsync("Campo em branco", "O Campo nome da empresa está em branco!!!");

        }

        private async void EnterpriseKeyError()
        {
            await this.ShowMessageAsync("Campo em branco", "O Campo key da empresa está em branco!!!");

        }

    }
}
