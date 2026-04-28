using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observatory.Photographer.UI
{
    internal class PhotoViewComparer : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is ListViewItem itemX && y is ListViewItem itemY)
            {
                var dateX = (DateTime?)itemX.Tag;
                var dateY = (DateTime?)itemY.Tag;
                if (dateX.HasValue && dateY.HasValue)
                    return DateTime.Compare(dateX.Value, dateY.Value);
            }
            return 0;
        }
    }
}
