using System.Collections.Generic;

namespace Ds_projekat
{
    internal interface ILocationRepository
    {
        int Add(Location l);
        bool Update(Location l);
        bool Delete(int id);
        Location GetById(int id);
        List<Location> GetAll();
    }
}
