using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UniversalExtensions.GroupedItems
{
    public class GroupedListItem<T> where T : IGroupItem
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<T> Items { get; set; }


        public GroupedListItem()
        {
            Items = new List<T>();
        }

        public static List<GroupedListItem<T>> GroupItems(List<T> items)
        {
            if (items == null) return new List<GroupedListItem<T>>();

            var q = from x in items
                    group x by x.Key into grp
                    orderby grp.Key
                    select grp;

            var grpItems = new List<GroupedListItem<T>>();

            foreach (var item in q)
            {
                GroupedListItem<T> akt = new GroupedListItem<T>();
                akt.GroupName = item.Key;

                grpItems.Add(akt);
            }

            foreach (var item in items)
            {
                foreach (var grpItem in grpItems)
                {
                    if (grpItem.GroupName.Equals(item.Key))
                    {
                        grpItem.Items.Add(item);
                    }
                }
            }

            return grpItems;
        }
    }
}
