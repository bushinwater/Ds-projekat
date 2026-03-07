using System.Collections.Generic;

namespace Ds_projekat.Repositories.Interfaces
{
    internal interface IDeskDetailsRepository
    {
        int Add(DeskDetails d);
        bool Update(DeskDetails d);
        bool Delete(int resourceId);
        DeskDetails GetByResourceId(int resourceId);
    }
}