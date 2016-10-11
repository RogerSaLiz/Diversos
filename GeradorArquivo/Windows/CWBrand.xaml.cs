using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GeradorArquivo.Annotations;
using GeradorArquivo.Objects;
using GeradorArquivo.ObjectsDB;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWBrand.xaml
    /// </summary>
    public partial class CWBrand 
    {
        private Brand _oBBrand= new Brand();
        public Brand OBBrand
        {
            get { return _oBBrand; }
            set
            {
                _oBBrand = value;
                OnPropertyChanged("OBBrand");
            }
        }


        public CWBrand()
        {
            InitializeComponent();
            TbName.Focus();
        }

        public CWBrand(Brand brand)
        {
            InitializeComponent();
            OBBrand = (Brand) brand.Clone();
            TbName.Focus();
        }



        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            var dbCliente = new BrandDB();

            if (OBBrand.BrandID > 0)
                dbCliente.Editar(OBBrand, delegate { DialogResult = true; });
            else
                dbCliente.Adicionar(OBBrand, delegate { DialogResult = true; });
        }

       
    }
}
