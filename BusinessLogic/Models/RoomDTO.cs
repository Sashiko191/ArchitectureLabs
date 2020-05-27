using AutoMapper;
using BusinessLogic.UnitOfWorkRealization;
using DB_Layer.Models;
using DB_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderDTO> GetRoomOrders()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()
             .ForMember("GetOrderedRooms", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            return mapper.Map<List<OrderDTO>>(UnitOfWork.GetUnitOfWork().roomsRepository.FindById(Id).RoomOrders.ToList());
        }
        public List<ActivityDTO> GetPosibleActivities()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Activity, ActivityDTO>()
                       .ForMember("GetPosibleRooms", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            return mapper.Map<List<ActivityDTO>>(UnitOfWork.GetUnitOfWork().roomsRepository.FindById(Id).PosibleActivities.ToList());
        }     
        public List<EquipmentDTO> GetRoomEquipment()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>()
                       .ForMember("GetRoomsThatHaveIt", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            return mapper.Map<List<EquipmentDTO>>(UnitOfWork.GetUnitOfWork().roomsRepository.FindById(Id).RoomEquipment.ToList());
        }
    }
}
