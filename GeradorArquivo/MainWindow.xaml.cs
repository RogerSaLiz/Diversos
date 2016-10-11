using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeradorArquivo.Annotations;
using GeradorArquivo.Pages;
using GeradorArquivo.Windows;
using MahApps.Metro.Controls;

namespace GeradorArquivo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private string _pathFileStyle = string.Concat(Directory.GetCurrentDirectory(), "\\Styles.txt");
        private string _pathFileDataUser = string.Concat(Directory.GetCurrentDirectory(), "\\DataUser.txt");

        private Visibility _backgroundRet = Visibility.Collapsed;
        public Visibility BackgroundRet
        {
            get { return _backgroundRet; }
            set
            {
                _backgroundRet = value;
                OnPropertyChanged("BackgroundRet");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CreateAndAppplyStyle();
            RetrieveDataAvatar();
        }

        private void CreateAndAppplyStyle()
        {
            try
            {
                if (!File.Exists(_pathFileStyle))
                {
                    using (TextWriter tw = new StreamWriter(_pathFileStyle))
                    {
                        tw.WriteLine("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml");
                        tw.WriteLine("pack://application:,,,/MahApps.Metro;component/Styles/Accents/basedark.xaml");
                        tw.Close();
                    }
                }
                else
                {
                    string paths = "";
                    using (var sr = new StreamReader(_pathFileStyle))
                    {
                        paths = sr.ReadToEnd();
                    }
                    string[] styles = paths.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    if (styles.Count() < 3)
                    {
                        while (Application.Current.Resources.MergedDictionaries.Count() > 3)
                            Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[3]);
                    }
                    foreach (var style in styles)
                    {
                        if (!string.IsNullOrWhiteSpace(style))
                        {
                            var dic = new ResourceDictionary();
                            dic.Source = new Uri(style);
                            Application.Current.Resources.MergedDictionaries.Add(dic);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
            
        }

        public void SetBackgroundRet()
        {
            BackgroundRet = BackgroundRet == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RetrieveDataAvatar()
        {
            try
            {
                if (File.Exists(_pathFileDataUser))
                {
                    string paths = "";
                    using (var sr = new StreamReader(_pathFileDataUser))
                    {
                        paths = sr.ReadToEnd();
                    }
                    string[] lines = paths.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                    var isImage = true;
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && isImage)
                        {
                            ImageAvatar.Source = new BitmapImage(new Uri(line, UriKind.RelativeOrAbsolute));
                            isImage = false;
                        }
                        else if (!string.IsNullOrWhiteSpace(line))
                            LbNameUser.Content = line;
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
           
        }

        private void OnClickPrinter(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Impressoras";
            var page = new PagePrinter();
            FrameMaster.Content = page;
        }

        private void OnClickSupply(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Suprimentos";
            var page = new PageSupply();
            FrameMaster.Content = page;
        }

        private void OnClickBrand(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Fabricantes";
            var page = new PageBrand();
            FrameMaster.Content = page;
        }

        private void OnClickCounter(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Contadores";
            var page = new PageCounter();
            FrameMaster.Content = page;
        }

        private void OnClickGerarArquivo(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Gerar Arquivo";
            var page = new PageFile();
            FrameMaster.Content = page;
        }

        private void OnClickEnterprises(object sender, RoutedEventArgs e)
        {
            GbPages.Header = "Empresas";
            var page = new PageEnterprises();
            FrameMaster.Content = page;
        }

        private void OnClickChangeTema(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var dic = new ResourceDictionary();
            dic.Source = new Uri(menuItem.Tag.ToString());
            while (Application.Current.Resources.MergedDictionaries.Count() > 3)
                Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[3]);
            Application.Current.Resources.MergedDictionaries.Add(dic);

            using (TextWriter tw = new StreamWriter(_pathFileStyle))
            {
                tw.WriteLine(dic.Source.OriginalString);
                tw.Close();
            }

        }

        private void OnClickChangeTemaEspecial(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var dic = new ResourceDictionary();
            dic.Source = new Uri(menuItem.Tag.ToString());
            var dicDark = new ResourceDictionary();
            dicDark.Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/basedark.xaml");
            Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[3]);
            Application.Current.Resources.MergedDictionaries.Add(dicDark);
            Application.Current.Resources.MergedDictionaries.Add(dic);

            using (TextWriter tw = new StreamWriter(_pathFileStyle))
            {
                tw.WriteLine(dic.Source.OriginalString);
                tw.WriteLine(dicDark.Source.OriginalString);
                tw.Close();
            }
        }

        private void OnClickAlterarAvatar(object sender, RoutedEventArgs e)
        {
            var cw = new CWSetAvatar();
            cw.Closed += delegate
            {
                if (cw.DialogResult.Value)
                {
                    RetrieveDataAvatar();
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
