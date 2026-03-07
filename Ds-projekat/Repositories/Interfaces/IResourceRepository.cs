using System.Collections.Generic;

namespace Ds_projekat.Repositories.Interfaces
{
    internal interface IResourceRepository
    {
        int Add(Resource r);
        bool Update(Resource r);
        bool Delete(int id);
        Resource GetById(int id);
        List<Resource> GetAll();
        List<Resource> GetByLocation(int locationId);
        List<Resource> GetByType(string resourceType);
    }
}