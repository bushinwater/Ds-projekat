using System.Collections.Generic;

namespace Ds_projekat
{
    internal interface IAdminRepository
    {
        bool Add(Admin a);
        bool Update(Admin a);
        bool Delete(int userId);
        Admin? GetByUserId(int userId);
        Admin? GetByUsername(string username);
        List<Admin> GetAll();
    }
}