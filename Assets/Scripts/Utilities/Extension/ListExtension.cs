using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ListExtension
{
    public static bool IsEmpty<T>(this IList<T> list)
    {
        return list.Count == 0;
    }

    public static void Some()
    {
        List<int> intList = new List<int>();
        intList.IsEmpty();
    }
}
