using System.Collections.Generic;

namespace Ds_projekat
{
    internal interface IUserRepository
    {
        int Add(User u);
        bool Update(User u);
        bool Delete(int id);
        User? GetById(int id);
        User? GetByEmail(string email);
        List<User> GetAll();
    }
}