using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    interface IDAO<T>
    {
        List<T> findAll();
        T find(int id);
        bool Delete(int id);
    }
}
