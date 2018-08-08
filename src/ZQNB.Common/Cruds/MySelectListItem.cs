using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZQNB.Common.Cruds
{
    public class MySelectListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        /// <summary>
        ///  当需要为多个属性设置参照集合时，通过property来区分不同的集合
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static List<MySelectListItem> TryGetMySelectListItems(string property = null)
        {
            Dictionary<string, List<MySelectListItem>> dict = GetDict();
            if (dict == null || dict.Count == 0)
            {
                return new List<MySelectListItem>();
            }
            var theKey = MySelectListItems + property;
            if (!dict.ContainsKey(theKey))
            {
                //只有一个，返回默认
                return dict.Count > 1 ? dict.Values.Single() : new List<MySelectListItem>();
            }
            return dict[theKey];
        }
        /// <summary>
        ///  当需要为多个属性设置参照集合时，通过property来区分不同的集合
        /// </summary>
        /// <param name="items"></param>
        /// <param name="property"></param>
        public static void TrySetMySelectListItems(List<MySelectListItem> items, string property = null)
        {
            var theKey = MySelectListItems + property;

            Dictionary<string, List<MySelectListItem>> dict = GetDict();
            var fixItems = new List<MySelectListItem>();
            if (items != null)
            {
                fixItems = items;
            }
            dict[theKey] = fixItems;
        }

        private static string MySelectListItems = "MySelectListItems_";
        public static Dictionary<string, List<MySelectListItem>> GetDict()
        {
            Dictionary<string, List<MySelectListItem>> dict = new Dictionary<string, List<MySelectListItem>>();
            if (HttpContext.Current.Items[MySelectListItems] == null)
            {
                HttpContext.Current.Items[MySelectListItems] = dict;
            }
            return HttpContext.Current.Items[MySelectListItems] as Dictionary<string, List<MySelectListItem>>;
        }
    }

}
