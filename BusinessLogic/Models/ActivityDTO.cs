using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DB_Layer.Repositories;
using BusinessLogic.Services;
using Ninject;
using BusinessLogic.Interfaces;
using DB_Layer.Interfaces;

namespace BusinessLogic.Models
{
    public class ActivityDTO
    {       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public bool IsSpecialActivity { get; set; }
        public List<RoomDTO> IncludedRooms { get; set; }
        public List<RoomDTO> GetPosibleRooms()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDTO>()
                           .ForMember("GetRoomOrders", opt => opt.Ignore())
                           .ForMember("GetPosibleActivities", opt => opt.Ignore())
                           .ForMember("GetRoomEquipment", opt => opt.Ignore()));
            var mapper = new Mapper(config);
            return mapper.Map<List<RoomDTO>>(DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>().activitiesRepository.FindById(Id).PosibleRooms.ToList());

        }
    }
}
