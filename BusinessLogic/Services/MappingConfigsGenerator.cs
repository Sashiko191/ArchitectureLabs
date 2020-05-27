using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Models;
using DB_Layer.Models;

namespace BusinessLogic.Services
{
    public class MappingConfigsGenerator
    {
        public Mapper RoomToDTOMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDTO>()
           .ForMember("GetRoomOrders", opt => opt.Ignore())
           .ForMember("GetPosibleActivities", opt => opt.Ignore())
           .ForMember("GetRoomEquipment", opt => opt.Ignore()));
            return new Mapper(config);
        }

        public Mapper DTOToRoomMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RoomDTO, Room>()
          .ForMember("RoomOrders", opt => opt.Ignore())
          .ForMember("PosibleActivities", opt => opt.Ignore())
          .ForMember("RoomEquipment", opt => opt.Ignore()));
            return new Mapper(config);
        }

        public Mapper OrderToDTOMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()
                       .ForMember("GetOrderedRooms", opt => opt.Ignore()));
            return new Mapper(config);
        }

        public Mapper DTOToOrderMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()
            .ForMember("OrderedRooms", opt => opt.Ignore()));
            return new Mapper(config);
        }

        public Mapper ActivityToDTOMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Activity, ActivityDTO>().ForMember("GetPosibleRooms", opt => opt.Ignore()));
            return new Mapper(config);
        }

        public Mapper DTOToActivityMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ActivityDTO, Activity>()
                       .ForMember("PosibleRooms", opt => opt.Ignore()));
            return new Mapper(config);
        }
    }
}
