using System;
using System.Collections.Generic;

namespace GeradorArquivo.Objects
{
    public class PrinterSupplyModelCounter
    {
        public int PrinteModelID { get; set; }
        public string ModelName { get; set; }
        public string NameXML { get; set; }
        public int SupplyModelId { get; set; }
        public string PartNumber { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string SupplyColorName { get; set; }
        public string BrandName { get; set; }
        public int CounterTypeID { get; set; }
        public string CounterTypeName { get; set; }
        public int Mono { get; set; }
        public int Color { get; set; }
        public int Total { get; set; }
        public BehaviorSupply BehSupply { get; set; }
        public List<BehaviorSupply> ListBehSupply { get; set; }

      

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
