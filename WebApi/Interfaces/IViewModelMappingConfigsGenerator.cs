using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Interfaces
{
    public interface IViewModelMappingConfigsGenerator
    {
        Mapper DTOToActivityViewModel();
        Mapper ViewModelToActivityDTO();
        Mapper DTOToRoomViewModel();
        Mapper ViewModelToRoomDTO();
        Mapper DTOToOrderViewModel();
        Mapper ViewModelToOrderDTO();
        Mapper DTOToEquipmentViewModel();
        Mapper ViewModelToEquipmentDTO();
    }
}
