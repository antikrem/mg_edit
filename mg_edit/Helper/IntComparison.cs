using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Helper
{
    class IntComparison
    {
        private static void Swap<T>(List<T> list, int a, int b) where T : IIntComparator
        {
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        private static int Partition<T>(List<T> list, int low, int high) where T : IIntComparator
        {
            IIntComparator pivot = list[high];

            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {
                if (list[j].Value() < pivot.Value())
                {
                    i++;
                    Swap(list, i, j);
                }
            }

            Swap(list, i+1, high);
            return i + 1;
        }

        private static void QSortInternal<T>(List<T> list, int l, int u) where T : IIntComparator
        {
            if (l < u)
            {
                int pi = Partition(list, l, u);

                QSortInternal(list, l, pi - 1);
                QSortInternal(list, pi + 1, u);
            }
        }

        public static void QSort<T>(List<T> list) where T : IIntComparator
        {
            QSortInternal(list, 0, list.Count - 1);
        }

    }
}
