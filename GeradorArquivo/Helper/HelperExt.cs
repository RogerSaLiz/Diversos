﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GeradorArquivo.Helper
{
    public static class HelperExt
    {

        public static int Remove<T>(
        this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
                coll.Remove(itemToRemove);
            

            return itemsToRemove.Count;
        }


        public static void AddRange<T>(this IList<T> owner, IEnumerable<T> items)
        {
            foreach (var item in items)
                owner.Add(item);
        }

        
        public static void SetBack()
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.SetBackgroundRet();
        }

        public static string ToStringReader(this object ob )
        {
            if (ob == null || ob == DBNull.Value)
                return string.Empty;
            return ob.ToString();
        }

        public static bool ToBoolReader(this object ob)
        {
            if (ob == null || ob==DBNull.Value)
                return false;
            return Convert.ToBoolean(ob);
        }

        public static int ToIntReader(this object ob)
        {
            if (ob == null || ob == DBNull.Value)
                return 0;
            return Convert.ToInt32(ob);
        }


        public static string ToIntNumeric(this object ob)
        {
            var num = Convert.ToInt32(ob);
            return num.ToString("N0");
        }

        public static void AddSorted<T>(this IList<T> list, T item,int index)
        {
            list.Insert(index, item);
        }
    }
}
