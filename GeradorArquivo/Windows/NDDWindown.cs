using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GeradorArquivo.Annotations;
using MahApps.Metro.Controls;

namespace GeradorArquivo.Windows
{
    public class NDDWindown:MetroWindow,INotifyPropertyChanged
    {
        public NDDWindown()
        {
            Helper.HelperExt.SetBack();
        }

        protected override void OnClosed(EventArgs e)
        {
            Helper.HelperExt.SetBack();
            base.OnClosed(e);
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
