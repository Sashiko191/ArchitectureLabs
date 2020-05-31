using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BL_Repositories
{
    public class RoomsBLRepository : IBusinessLogicRepository<RoomDTO>
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mappingConfigs;
        public RoomsBLRepository()
        {
            unitOfWork = DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>();
            mappingConfigs = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
        }
        public RoomDTO FindById(int id)
        {
            return mappingConfigs.RoomToDTOMapper().Map<RoomDTO>(unitOfWork.roomsRepository.FindById(id));
        }
        public RoomDTO FindByIdWithEverything(int id)
        {
            RoomDTO roomDTO = FindById(id);
            roomDTO.IncludedActivities = roomDTO.GetPosibleActivities();
            roomDTO.IncludedEquipment = roomDTO.GetRoomEquipment();
            roomDTO.IncludedOrders = roomDTO.GetRoomOrders();
            return roomDTO;
        }
        public List<RoomDTO> Get()
        {
            return mappingConfigs.RoomToDTOMapper().Map<List<RoomDTO>>(unitOfWork.roomsRepository.Get());
        }
        public List<RoomDTO> GetWithEverything()
        {
            List<RoomDTO> roomDTOs = Get();
            for(int i = 0;i < roomDTOs.Count;i++)
            {
                roomDTOs[i].IncludedActivities = roomDTOs[i].GetPosibleActivities();
                roomDTOs[i].IncludedEquipment = roomDTOs[i].GetRoomEquipment();
                roomDTOs[i].IncludedOrders = roomDTOs[i].GetRoomOrders();
            }
            return roomDTOs;
        }
        public RoomDTO Create(RoomDTO roomDTO)
        {
            Room room = mappingConfigs.DTOToRoomMapper().Map<Room>(roomDTO);
            if(roomDTO.IncludedActivities != null && roomDTO.IncludedActivities.Count != 0)
            {
                List<Activity> activities = new List<Activity>();
                for(int i = 0;i < roomDTO.IncludedActivities.Count;i++)
                {
                    activities.Add(unitOfWork.activitiesRepository.FindById(roomDTO.IncludedActivities[i].Id));
                }
                room.PosibleActivities = activities;
            }
            if(roomDTO.IncludedOrders != null && roomDTO.IncludedOrders.Count != 0)
            {
                List<Order> orders = new List<Order>();
                for(int i = 0;i < roomDTO.IncludedOrders.Count;i++)
                {
                    orders.Add(unitOfWork.ordersRepository.FindById(roomDTO.IncludedOrders[i].Id));
                }
                room.RoomOrders = orders;
            }
            if(roomDTO.IncludedEquipment != null && roomDTO.IncludedEquipment.Count != 0)
            {
                List<Equipment> equipment = new List<Equipment>();
                for(int i = 0;i < roomDTO.IncludedEquipment.Count;i++)
                {
                    equipment.Add(unitOfWork.equipmentRepository.FindById(roomDTO.IncludedEquipment[i].Id));
                }
                room.RoomEquipment = equipment;
            }
            unitOfWork.roomsRepository.Create(room);
            RoomDTO CreatedRoomDTO = mappingConfigs.RoomToDTOMapper().Map<RoomDTO>(room);
            CreatedRoomDTO.IncludedActivities = CreatedRoomDTO.GetPosibleActivities();
            CreatedRoomDTO.IncludedEquipment = CreatedRoomDTO.GetRoomEquipment();
            CreatedRoomDTO.IncludedOrders = CreatedRoomDTO.GetRoomOrders();
            return CreatedRoomDTO;
        }
        public void Update(RoomDTO roomDTO)
        {
            Room room = mappingConfigs.DTOToRoomMapper().Map<Room>(roomDTO);
            if (roomDTO.IncludedActivities != null && roomDTO.IncludedActivities.Count != 0)
            {
                List<Activity> activities = new List<Activity>();
                for (int i = 0; i < roomDTO.IncludedActivities.Count; i++)
                {
                    activities.Add(unitOfWork.activitiesRepository.FindById(roomDTO.IncludedActivities[i].Id));
                }
                room.PosibleActivities = activities;
            }
            if (roomDTO.IncludedOrders != null && roomDTO.IncludedOrders.Count != 0)
            {
                List<Order> orders = new List<Order>();
                for (int i = 0; i < roomDTO.IncludedOrders.Count; i++)
                {
                    orders.Add(unitOfWork.ordersRepository.FindById(roomDTO.IncludedOrders[i].Id));
                }
                room.RoomOrders = orders;
            }
            if (roomDTO.IncludedEquipment != null && roomDTO.IncludedEquipment.Count != 0)
            {
                List<Equipment> equipment = new List<Equipment>();
                for (int i = 0; i < roomDTO.IncludedEquipment.Count; i++)
                {
                    equipment.Add(unitOfWork.equipmentRepository.FindById(roomDTO.IncludedEquipment[i].Id));
                }
                room.RoomEquipment = equipment;
            }

            int searchedId = roomDTO.Id;
            Room ExistingRoom = unitOfWork.roomsRepository.FindById(searchedId);
            ExistingRoom.Name = room.Name;
            ExistingRoom.PosibleActivities = room.PosibleActivities;
            ExistingRoom.RoomEquipment = room.RoomEquipment;
            ExistingRoom.RoomOrders = room.RoomOrders;
            unitOfWork.roomsRepository.Update(ExistingRoom);
        }
        public void Delete(RoomDTO roomDTO)
        {
            int roomId = roomDTO.Id;
            unitOfWork.roomsRepository.Remove(unitOfWork.roomsRepository.FindById(roomId));
        }
      
    }
}
