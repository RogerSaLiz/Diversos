namespace GeradorArquivo.Objects
{
    public class Enterprise
    {
        public int EnterpriseID { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseKey { get; set; }
        public string Observation { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
