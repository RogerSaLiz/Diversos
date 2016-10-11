using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using GeradorArquivo.Annotations;
using MahApps.Metro.Controls.Dialogs;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWSetAvatar.xaml
    /// </summary>
    public partial class CWSetAvatar :INotifyPropertyChanged
    {
        private ObservableCollection<AvatarsImage> _collectionAvatars;
        public ObservableCollection<AvatarsImage> CollectionAvatars
        {
            get { return _collectionAvatars ?? (_collectionAvatars = new ObservableCollection<AvatarsImage>()); }
            set
            {
                _collectionAvatars = value;
                OnPropertyChanged("CollectionAvatars");
            }
        }

        private AvatarsImage _selectedAvatar;
        public AvatarsImage SelectedAvatar
        {
            get { return _selectedAvatar; }
            set
            {
                _selectedAvatar = value;
                OnPropertyChanged("SelectedAvatar");
            }
        }

        private readonly bool _hasChange;
        private BitmapImage _bitmapImageAvatar;
        public BitmapImage BitmapImageAvatar
        {
            get { return _bitmapImageAvatar; }
            set
            {
                _bitmapImageAvatar = value;
                OnPropertyChanged("BitmapImageAvatar");
            }
        }

        private string _pathFileStyle = string.Concat(Directory.GetCurrentDirectory(), "\\Avatars");
        private string _pathFileDataUser = string.Concat(Directory.GetCurrentDirectory(), "\\DataUser.txt");

        public CWSetAvatar(bool hasChange=false)
        {
            _hasChange = hasChange;
            InitializeComponent();
            Loaded += CWSetAvatar_Loaded;
           
        }

        void CWSetAvatar_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] fileEntries = Directory.GetFiles(_pathFileStyle);
                foreach (string fileName in fileEntries)
                {
                    var avatarsImage = new AvatarsImage();
                    avatarsImage.AvatarImage = new BitmapImage(new Uri(fileName.Replace(" ",""), UriKind.RelativeOrAbsolute));
                    avatarsImage.PathAvatar = fileName;
                    CollectionAvatars.Add(avatarsImage);
                };
                DgAvatars.ItemsSource = CollectionAvatars;
                LoadTxtDataUser();
            }
            catch (Exception ex)
            {
                
            }
           
        }

        
        private void LoadTxtDataUser()
        {
            try
            {
                string paths = "";
                using (var sr = new StreamReader(_pathFileDataUser))
                {
                    paths = sr.ReadToEnd();
                }
                string[] lines = paths.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                var isImage = true;
                var source = "";
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && isImage)
                    {
                        Avatar.Source = new BitmapImage(new Uri(line, UriKind.RelativeOrAbsolute));
                        source = line;
                        isImage = false;
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                        TbUser.Text = line;
                }
                var firstOrDefault = CollectionAvatars.FirstOrDefault(p => p.PathAvatar == source);
                SelectedAvatar = firstOrDefault;
            }
            catch (Exception ex)
            {
               
            }
            
        }

       
        private void OnClickChangeAvatar(object sender, RoutedEventArgs e)
        {
            FlyoutAvatar.IsOpen = true;
        }

        private void OnClickSalvarImagem(object sender, RoutedEventArgs e)
        {
            Avatar.Source = new BitmapImage(new Uri(SelectedAvatar.PathAvatar, UriKind.RelativeOrAbsolute));
            FlyoutAvatar.IsOpen = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbUser.Text))
            {
                ErrorNameUser();
            }
            else
            {
                using (TextWriter tw = new StreamWriter(_pathFileDataUser))
                {
                    tw.WriteLine(SelectedAvatar.PathAvatar);
                    tw.WriteLine(TbUser.Text);
                    tw.Close();
                }
                DialogResult = true;
            }
        }

        private async void ErrorNameUser()
        {
            await
                this.ShowMessageAsync("Nome Usuário", "Digite um nome para o usuário!!!");
        }
    }

    public class AvatarsImage
    {
        public BitmapImage AvatarImage { get; set; }
        public string PathAvatar { get; set; }
    }


}
