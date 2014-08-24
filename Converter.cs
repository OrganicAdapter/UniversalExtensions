using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalExtensions
{
    public class Converter
    {
        public static ObservableCollection<T> ListToObservableCollection<T>(List<T> list)
        {
            var observable = new ObservableCollection<T>();

            if (list == null)
                return observable;

            foreach (var item in list)
            {
                observable.Add(item);
            }

            return observable;
        }
    }
}
