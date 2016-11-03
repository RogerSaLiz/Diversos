using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeradorArquivo.Helper
{
    public static class Extensions
    {
        //public static List<T> CopyList<T>(this List<T> lst)
        //{
        //    List<T> lstCopy = new List<T>();
        //    foreach (var item in lst)
        //    {
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            formatter.Serialize(stream, item);
        //            stream.Position = 0;
        //            lstCopy.Add((T)formatter.Deserialize(stream));
        //        }
        //    }
        //    return lstCopy;
        //}


        public class CloneableList<T> : List<T>, ICloneable where T : ICloneable
        {
            public object Clone()
            {
                var clone = new List<T>();
                ForEach(item => clone.Add((T)item.Clone()));
                return clone;
            }
        }
    }
}
