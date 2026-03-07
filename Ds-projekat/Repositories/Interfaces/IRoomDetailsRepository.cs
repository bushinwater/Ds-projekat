using System.Collections.Generic;

namespace Ds_projekat.Repositories.Interfaces
{
    internal interface IRoomDetailsRepository
    {
        int Add(RoomDetails r);
        bool Update(RoomDetails r);
        bool Delete(int resourceId);
        RoomDetails GetByResourceId(int resourceId);
    }
}