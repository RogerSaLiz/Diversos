using System.Collections.Generic;

namespace GeradorArquivo.Objects
{
    public class SupplyModel
    {
        public int SupplyModelId { get; set; }
        public string PartNumber { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public SupplySubFunctionTypes SupplySubFunctionType { get; set; }
        public SupplySlots SupplySlot { get; set; }
        public BehaviorSupply BehSupply { get; set; }
        public List<BehaviorSupply> ListBehSupply { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
