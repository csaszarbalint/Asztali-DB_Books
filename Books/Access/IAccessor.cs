using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Access
{
    internal interface IAccessor<T>
    {
        bool Create(T obj);
        List<T> Read();
    }
}
