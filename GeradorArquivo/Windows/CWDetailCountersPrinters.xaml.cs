using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.Windows
{
    /// <summary>
    /// Interaction logic for CWDetailCountersPrinters.xaml
    /// </summary>
    public partial class CWDetailCountersPrinters
    {
        private readonly IEnumerable<PrinterSupplyModelCounter> _listCountersDeteils;
        private readonly int _intervalCounters;

        private ObservableCollection<PrinterSupplyModelCounter> _listCounters2;
        public ObservableCollection<PrinterSupplyModelCounter> ListCounters2
        {
            get { return _listCounters2 ?? (_listCounters2 = new ObservableCollection<PrinterSupplyModelCounter>()); }
            set
            {
                _listCounters2 = value;
                OnPropertyChanged("ListCounters2");
            }
        }

        private CounterType _selectedCounters;
        public CounterType SelectedCounters
        {
            get { return _selectedCounters; }
            set
            {
                _selectedCounters = value;
                OnPropertyChanged("SelectedCounters");
            }
        }

        private ObservableCollection<CounterType> _listCountersCombobox;
        public ObservableCollection<CounterType> ListCountersCombobox
        {
            get { return _listCountersCombobox ?? (_listCountersCombobox = new ObservableCollection<CounterType>()); }
            set
            {
                _listCountersCombobox = value;
                OnPropertyChanged("ListCountersCombobox");
            }
        }

        private CounterType _selectedCountersCombobox;
        public CounterType SelectedCountersCombobox
        {
            get { return _selectedCountersCombobox; }
            set
            {
                _selectedCountersCombobox = value;

                FilterItemGridSelected();
                OnPropertyChanged("SelectedCountersCombobox");
            }
        }

        public CollectionView ListCounters { get; set; }

        public CWDetailCountersPrinters(IEnumerable<PrinterSupplyModelCounter> listCountersDeteils, int intervalCounters)
        {
            _listCountersDeteils = listCountersDeteils;
            _intervalCounters = intervalCounters;
            InitializeComponent();
            Loaded += CWDetailCountersPrinters_Loaded;
            //ListCounters2 = new ObservableCollection<PrinterSupplyModelCounter>(listCountersDeteils);
            //DgCounters.ItemsSource = ListCounters2;
            //ListCounters = (CollectionView)CollectionViewSource.GetDefaultView(DgCounters.ItemsSource);
            //ListCounters.Filter = FilterItemGrid;
            //ListCounters2.CollectionChanged += ListCounters2_CollectionChanged;
            foreach (var item in _listCountersDeteils)
            {
                item.HasChangeItem += HasChangeItem;
            }
            foreach (var item in _listCountersDeteils)
                ListCounters2.Add((PrinterSupplyModelCounter)item.Clone());


            ListCounters = (CollectionView)CollectionViewSource.GetDefaultView(ListCounters2);
            ListCounters.Filter = FilterItemGrid;



        }

        private readonly int COUNTERGERAL = 1;
        private void HasChangeItem(PrinterSupplyModelCounter counter)
        {
            PrinterSupplyModelCounter.ProcessInStateChange = true;
            //  ChangePropertiesCounters(counter, _intervalCounters,true);
            var counterInitial = counter.Total;
            var original = counter.Total;
            var counterTypeID = counter.CounterTypeID;
            if (counterTypeID == COUNTERGERAL)
            {
                foreach (var detail in ListCounters2.Where(p => p.DateIntervalReaders >= counter.DateIntervalReaders))
                {
                    if (counterTypeID != detail.CounterTypeID)
                    {
                        counterInitial = original;
                        counterTypeID = detail.CounterTypeID;
                    }
                    ChangePropertiesCounters(detail, counterInitial);
                    counterInitial = counterInitial + _intervalCounters;
                }
            }
            else
            {
                foreach (var detail in ListCounters2.Where(p => p.DateIntervalReaders >= counter.DateIntervalReaders && p.CounterTypeID != 1))
                {
                    if (counterTypeID != detail.CounterTypeID)
                    {
                        counterInitial = original;
                        counterTypeID = detail.CounterTypeID;
                    }
                    ChangePropertiesCounters(detail, counterInitial);
                    counterInitial = counterInitial + _intervalCounters;
                }
            }
            PrinterSupplyModelCounter.ProcessInStateChange = false;
            CreateResume(false);
        }

        private static void ChangePropertiesCounters(PrinterSupplyModelCounter detail, double counterInitial, bool isInitial = false)
        {
            // if (!isInitial)
            detail.Total = Math.Ceiling((counterInitial * detail.TotalPerc));
            detail.Color = Math.Ceiling((counterInitial * detail.TotalPerc) * detail.ColorPerc);
            detail.Mono = Math.Floor((counterInitial * detail.TotalPerc) * detail.MonoPerc);
            detail.SumMonoColor = detail.Color + detail.Mono;
            detail.DifTotalMonoColor = detail.Total - detail.SumMonoColor;
        }




        private bool FilterItemGrid(object obj)
        {
            if (SelectedCountersCombobox == null) return false;
            var printerSupplyModelCounter = obj as PrinterSupplyModelCounter;
            return printerSupplyModelCounter.CounterTypeID == SelectedCountersCombobox.CounterTypeID;
        }

        void CWDetailCountersPrinters_Loaded(object sender, RoutedEventArgs e)
        {
            CreateResume();
        }

        private void CreateResume(bool insertCounterCombobox = true)
        {
            var wrap = new WrapPanel() { Orientation = Orientation.Horizontal };
            if (insertCounterCombobox)
                ListCountersCombobox.Add(new CounterType() { CounterTypeID = 0, CounterTypeName = "Selecione um contador..." });
            foreach (var item in ListCounters2.GroupBy(p => p.CounterTypeID).Select(p => p.Last()))
            {
                var groupBox = new GroupBox() {Header = item.CounterTypeName, Margin = new Thickness(0,0,30,0)};
                var firstCounter = ListCounters2.First(p => p.CounterTypeID == item.CounterTypeID);
                var stackPanel = new StackPanel() { Margin = new Thickness(0, 0, 10, 0) };
                    stackPanel.Children.Add(new TextBlock() { Text = string.Concat("Color:  ", item.Color.ToIntNumeric()), FontSize = 16 });
                    stackPanel.Children.Add(new TextBlock() { Text = string.Concat("Mono:  ", item.Mono.ToIntNumeric()), FontSize = 16 });
                    stackPanel.Children.Add(new TextBlock() { Text = string.Concat("Total:   ", item.Total.ToIntNumeric()), FontSize = 16 });
                    stackPanel.Children.Add(new TextBlock() { Text = string.Concat("Final-Inícial= ", (item.Total - firstCounter.Total).ToIntNumeric()), FontSize = 16, Margin = new Thickness(0, 20, 0, 0) });
                groupBox.Content = stackPanel;
                wrap.Children.Add(groupBox);
                if (insertCounterCombobox)
                    ListCountersCombobox.Add(new CounterType() { CounterTypeID = item.CounterTypeID, CounterTypeName = item.CounterTypeName });
            }
            if (insertCounterCombobox)
                SelectedCountersCombobox = ListCountersCombobox.First();

            GbResumo.Children.Clear();
            GbResumo.Children.Add(wrap);
        }
        private void FilterItemGridSelected()
        {
            CollectionViewSource.GetDefaultView(ListCounters2).Refresh();
        }


        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnClickClosed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
