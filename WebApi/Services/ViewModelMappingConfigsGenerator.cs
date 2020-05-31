using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BusinessLogic.Models;
using WebApi.Models;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class ViewModelMappingConfigsGenerator : IViewModelMappingConfigsGenerator
    {
        public Mapper DTOToActivityViewModel()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ActivityDTO, ActivityViewModel>()
            .ForMember("PosibleRooms", a => a.Ignore()));
            return new Mapper(config);
        }

        public Mapper ViewModelToActivityDTO()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ActivityViewModel, ActivityDTO>()
            .ForMember("IncludedRooms", vm => vm.Ignore())
            .ForMember("GetPosibleRooms", vm => vm.Ignore()));
            return new Mapper(config);
        }

        public Mapper DTOToRoomViewModel()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RoomDTO, RoomViewModel>()
                .ForMember("RoomOrders", r => r.Ignore())
                .ForMember("PosibleActivities", r => r.Ignore())
                .ForMember("RoomEquipment", r => r.Ignore()));
            return new Mapper(config);
        }

        public Mapper ViewModelToRoomDTO()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RoomViewModel, RoomDTO>()
              .ForMember("GetRoomOrders", r => r.Ignore())
              .ForMember("GetPosibleActivities", r => r.Ignore())
              .ForMember("GetRoomEquipment", r => r.Ignore())
              .ForMember("IncludedOrders", r => r.Ignore())
              .ForMember("IncludedActivities", r => r.Ignore())
              .ForMember("IncludedEquipment", r => r.Ignore()));
            return new Mapper(config);
        } 

        public Mapper DTOToOrderViewModel()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, OrderViewModel>()
              .ForMember("OrderedRooms", r => r.Ignore()));
            return new Mapper(config);
        }

        public Mapper ViewModelToOrderDTO()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderViewModel, OrderDTO>()
              .ForMember("GetOrderedRooms", r => r.Ignore())
              .ForMember("IncludedRooms", r => r.Ignore()));
            return new Mapper(config);
        }

        public Mapper DTOToEquipmentViewModel()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO,EquipmentViewModel>()
                         .ForMember("RoomsThatHaveIt", r => r.Ignore()));
            return new Mapper(config);
        }

        public Mapper ViewModelToEquipmentDTO()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentViewModel,EquipmentDTO>()
                         .ForMember("GetRoomsThatHaveIt", r => r.Ignore())
                         .ForMember("IncludedRooms",r => r.Ignore()));
            return new Mapper(config);
        }
    }
}