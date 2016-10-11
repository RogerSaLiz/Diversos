namespace GeradorArquivo.Objects
{
    public class CounterType
    {
        public int CounterTypeID { get; set; }
        public string CounterTypeName { get; set; }
        public int Mono { get; set; }
        public int Color { get; set; }

        public int Total
        {
            get
            {
                return Mono + Color;
            }
            set { }
        }

        public string Observation { get; set; }



        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
