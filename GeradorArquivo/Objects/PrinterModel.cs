using System.Collections.Generic;

namespace GeradorArquivo.Objects
{
    public class PrinterModel
    {
        public int PrinteModelID { get; set; }
        public string ModelName { get; set; }
        public Brand Brand { get; set; }
        public string NameXML { get; set; }
        public string Observation { get; set; }
        public int Index { get; set; }

        private bool _isNetWork = true;
        public bool IsNetwork
        {
            get { return _isNetWork; }
            set { _isNetWork = value; }
        }

        private bool _isCopier = true;
        public bool IsCopier
        {
            get { return _isCopier; }
            set { _isCopier = value; }
        }

        private bool _isColor = true;
        public bool IsColor
        {
            get { return _isColor; }
            set { _isColor = value; }
        }

        private bool _isDuplex = true;
        public bool IsDuplex
        {
            get { return _isDuplex; }
            set { _isDuplex = value; }
        }

        private bool _isScan = true;
        public bool IsScan
        {
            get { return _isScan; }
            set { _isScan = value; }
        }

        private bool _isLargeMedia = true;
        public bool IsLargeMedia
        {
            get { return _isLargeMedia; }
            set { _isLargeMedia = value; }
        }


        private string _location = string.Empty;
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }


        private string _serialNumber = string.Empty;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }


        private string _addressMac = string.Empty;
        public string AddressMac
        {
            get { return _addressMac; }
            set { _addressMac = value; }
        }

        private string _addressName = string.Empty;
        public string AddressName
        {
            get { return _addressName; }
            set { _addressName = value; }
        }

        private string _addressPort = string.Empty;
        public string AddressPort
        {
            get { return _addressPort; }
            set { _addressPort = value; }
        }


        private int _counterInitial = 5000;
        public int CounterInitial
        {
            get { return _counterInitial; }
            set { _counterInitial = value; }
        }

        /// <summary>
        /// Utilizado para definir quantas impressoras serão gerados
        /// </summary>
        private int _qtPrinters = 1;
        public int QtPrinters
        {
            get { return _qtPrinters; }
            set { _qtPrinters = value; }
        }

        private List<PrinterSupplyModelCounter> _listCounters = new List<PrinterSupplyModelCounter>();
        public List<PrinterSupplyModelCounter> ListCounters
        {
            get { return _listCounters; }
            set { _listCounters = value; }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
