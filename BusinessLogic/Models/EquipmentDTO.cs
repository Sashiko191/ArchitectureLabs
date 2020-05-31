using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using DB_Layer.Repositories;
using Ninject;

namespace BusinessLogic.Models
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RoomDTO> IncludedRooms { get; set; }
        public List<RoomDTO> GetRoomsThatHaveIt()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDTO>()
                           .ForMember("GetRoomOrders", opt => opt.Ignore())
                           .ForMember("GetPosibleActivities", opt => opt.Ignore())
                           .ForMember("GetRoomEquipment", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            return mapper.Map<List<RoomDTO>>(DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>().equipmentRepository.FindById(Id).RoomsThatHaveIt.ToList());
        }
    }
}
