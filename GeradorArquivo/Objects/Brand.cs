namespace GeradorArquivo.Objects
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        
    }
}
