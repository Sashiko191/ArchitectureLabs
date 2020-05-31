using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMappingConfigsGenerator
    {
        Mapper RoomToDTOMapper();
        Mapper DTOToRoomMapper();
        Mapper OrderToDTOMapper();
        Mapper DTOToOrderMapper();
        Mapper ActivityToDTOMapper();
        Mapper DTOToActivityMapper();
        Mapper EquipmentToDTOMapper();
        Mapper DTOToEquipmentMapper();
    }
}
