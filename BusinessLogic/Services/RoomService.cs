using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using DB_Layer.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mapConfigsGenerator;
        private IKernel DIResolver;

        public RoomService()
        {
            DIResolver = DI_Resolver.GetDIResolver();
            unitOfWork = DIResolver.Get<IUnitOfWork<AntiCafeDb>>();
            mapConfigsGenerator = DIResolver.Get<IMappingConfigsGenerator>();
        }

        public RoomService(IUnitOfWork<AntiCafeDb> _unitOfWork, IMappingConfigsGenerator _mappingConfigsGenerator)
        {
            unitOfWork = _unitOfWork;
            mapConfigsGenerator = _mappingConfigsGenerator;
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
