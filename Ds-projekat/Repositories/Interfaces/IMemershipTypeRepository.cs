using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.Repositories.Interfaces
{
    internal interface IMemershipTypeRepository
    {
        int Add(MembershipType mt);
        bool Update(MembershipType mt);
        bool Delete(int id);
        MembershipType GetById(int id);
        List<MembershipType> GetAll();
    }
}
