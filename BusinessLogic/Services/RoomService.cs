using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.UnitOfWorkRealization;
using DB_Layer.Models;
using DB_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class RoomService : IRoomService
    {
        private UnitOfWork unitOfWork;
        private MappingConfigsGenerator mapConfigsGenerator;

        public RoomService()
        {
            unitOfWork = UnitOfWork.GetUnitOfWork();
            mapConfigsGenerator = new MappingConfigsGenerator();
        }
        public List<RoomDTO> GetAllRooms()
        {
            var mapper = mapConfigsGenerator.RoomToDTOMapper();
            return mapper.Map<List<RoomDTO>>(unitOfWork.roomsRepository.Get());
        }

        public RoomDTO GetRoom(int id)
        {
            var mapper = mapConfigsGenerator.RoomToDTOMapper();
            return mapper.Map<RoomDTO>(unitOfWork.roomsRepository.FindById(id));
        }
    }
}
