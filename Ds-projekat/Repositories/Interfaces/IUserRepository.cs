using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.Repositories.Interfaces
{
    internal interface IUserRepository
    {
        int Add(User u);
        bool Update(User u);
        bool Delete(int id);
        User GetById(int id);
        List<User> GetAll();
        List<User> GetByMembershipType(int membershipTypeId);
        List<User> GetByStatus(string status);
    }
}
