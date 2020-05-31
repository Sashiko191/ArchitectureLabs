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
    public class EquipmentBLRepository : IBusinessLogicRepository<EquipmentDTO>
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mappingConfigs;
        public EquipmentBLRepository()
        {
            unitOfWork = DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>();
            mappingConfigs = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
        }

        public EquipmentDTO FindById(int id)
        {
            return mappingConfigs.EquipmentToDTOMapper().Map<EquipmentDTO>(unitOfWork.equipmentRepository.FindById(id));
        }

        public EquipmentDTO FindByIdWithEverything(int id)
        {
            EquipmentDTO equipmentDTO = FindById(id);
            equipmentDTO.IncludedRooms = equipmentDTO.GetRoomsThatHaveIt();
            return equipmentDTO;
        }

        public List<EquipmentDTO> Get()
        {
            return mappingConfigs.EquipmentToDTOMapper().Map<List<EquipmentDTO>>(unitOfWork.equipmentRepository.Get());
        }

        public List<EquipmentDTO> GetWithEverything()
        {
            List<EquipmentDTO> equipmentDTOs = Get();
            for (int i = 0;i < equipmentDTOs.Count;i++)
            {
                equipmentDTOs[i].IncludedRooms = equipmentDTOs[i].GetRoomsThatHaveIt();
            }
            return equipmentDTOs;
        }

        public EquipmentDTO Create(EquipmentDTO equipmentDTO)
        {
            Equipment equipment = mappingConfigs.DTOToEquipmentMapper().Map<Equipment>(equipmentDTO);
            if(equipmentDTO.IncludedRooms != null && equipmentDTO.IncludedRooms.Count != 0)
            {
                List<Room> rooms = new List<Room>();
                for(int i = 0;i < equipmentDTO.IncludedRooms.Count;i++)
                {
                    rooms.Add(unitOfWork.roomsRepository.FindById(equipmentDTO.IncludedRooms[i].Id));
                }

                equipment.RoomsThatHaveIt = rooms;
            }
            unitOfWork.equipmentRepository.Create(equipment);
            EquipmentDTO NewEquipmentDTO = mappingConfigs.EquipmentToDTOMapper().Map<EquipmentDTO>(equipment);
            NewEquipmentDTO.IncludedRooms = NewEquipmentDTO.GetRoomsThatHaveIt();
            return NewEquipmentDTO;
        }
    
        public void Update(EquipmentDTO equipmentDTO)
        {
            Equipment equipment = mappingConfigs.DTOToEquipmentMapper().Map<Equipment>(equipmentDTO);
            if(equipmentDTO.IncludedRooms != null)
            {
                List<Room> rooms = new List<Room>();
                for(int i = 0;i < equipmentDTO.IncludedRooms.Count;i++)
                {
                    rooms.Add(unitOfWork.roomsRepository.FindById(equipmentDTO.IncludedRooms[i].Id));
                }
                equipment.RoomsThatHaveIt = rooms;
            }

            int searchedId = equipmentDTO.Id;
            Equipment ExistingEquipment = unitOfWork.equipmentRepository.FindById(searchedId);
            ExistingEquipment.Name = equipment.Name;
            ExistingEquipment.Description = equipment.Description;
            ExistingEquipment.RoomsThatHaveIt = equipment.RoomsThatHaveIt;
            unitOfWork.equipmentRepository.Update(ExistingEquipment);
        }

        public void Delete(EquipmentDTO equipmentDTO)
        {
            int id = equipmentDTO.Id;
            unitOfWork.equipmentRepository.Remove(unitOfWork.equipmentRepository.FindById(id));
        }
    }
}
