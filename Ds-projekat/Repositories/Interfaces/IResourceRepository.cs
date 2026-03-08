using System.Collections.Generic;

namespace Ds_projekat
{
    internal interface IResourceRepository
    {
        int AddResource(Resource r, DeskDetails desk, RoomDetails room);
        bool DeleteResource(int resourceId);

        bool UpdateResource(Resource r, DeskDetails desk, RoomDetails room);
        Resource GetResource(int resourceId);
        DeskDetails GetDeskDetails(int resourceId);
        RoomDetails GetRoomDetails(int resourceId);

        List<Resource> GetAllResources();
        List<Resource> GetResourcesByLocation(int locationId);
    }
}
