using BusinessLogic.BL_Repositories;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Services;
using BusinessLogic.Interfaces;
using Ninject;
using WebApi.Utils;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class EquipmentController : ApiController
    {
        private IViewModelMappingConfigsGenerator configsGenerator;
        private IBusinessLogicRepository<EquipmentDTO> EquipmentBLRepository;

        public EquipmentController()
        {
            IKernel webApiDIResolver = WebApiDIResolver.GetDIResolver();
            EquipmentBLRepository = webApiDIResolver.Get<IBusinessLogicRepository<EquipmentDTO>>();
            configsGenerator = webApiDIResolver.Get<IViewModelMappingConfigsGenerator>();
        }

        [HttpGet]
        public List<EquipmentViewModel> Get(bool WithRooms)
        {         
            List<EquipmentDTO> equipmentDTOs;
            if (WithRooms)
                equipmentDTOs = EquipmentBLRepository.GetWithEverything();
            else
                equipmentDTOs = EquipmentBLRepository.Get();
            List<EquipmentViewModel> equipmentViewModels = configsGenerator.DTOToEquipmentViewModel().Map<List<EquipmentViewModel>>(equipmentDTOs);
            for (int i = 0;i < equipmentDTOs.Count;i++)
            {
                equipmentViewModels[i].RoomsThatHaveIt = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(equipmentDTOs[i].IncludedRooms);
            }
            return equipmentViewModels;
        }

        [HttpGet]
        public EquipmentViewModel Get(int id, bool WithRooms)
        {
            EquipmentDTO equipmentDTO;
            EquipmentViewModel equipmentViewModel;
            if (WithRooms)
            {
                equipmentDTO = EquipmentBLRepository.FindByIdWithEverything(id);
                equipmentViewModel = configsGenerator.DTOToEquipmentViewModel().Map<EquipmentViewModel>(equipmentDTO);
                equipmentViewModel.RoomsThatHaveIt = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(equipmentDTO.IncludedRooms);
            }
            else
            {
                equipmentDTO = EquipmentBLRepository.FindById(id);
                equipmentViewModel = configsGenerator.DTOToEquipmentViewModel().Map<EquipmentViewModel>(equipmentDTO);
            }
            return equipmentViewModel;
        }

        [HttpPost]
        public EquipmentViewModel Post([FromBody]EquipmentViewModel equipmentViewModel)
        {
            EquipmentDTO equipmentDTO = configsGenerator.ViewModelToEquipmentDTO().Map<EquipmentDTO>(equipmentViewModel);
            equipmentDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(equipmentViewModel.RoomsThatHaveIt);
            EquipmentDTO CreatedEquipmentDTO = EquipmentBLRepository.Create(equipmentDTO);
            EquipmentViewModel CreatedEquipmentViewModel = configsGenerator.DTOToEquipmentViewModel().Map<EquipmentViewModel>(CreatedEquipmentDTO);
            CreatedEquipmentViewModel.RoomsThatHaveIt = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(CreatedEquipmentDTO.IncludedRooms);
            return CreatedEquipmentViewModel;
        }

        [HttpPut]
        public void Update([FromBody]EquipmentViewModel equipmentViewModel)
        {
            EquipmentDTO equipmentDTO = configsGenerator.ViewModelToEquipmentDTO().Map<EquipmentDTO>(equipmentViewModel);
            equipmentDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(equipmentViewModel.RoomsThatHaveIt);
            EquipmentBLRepository.Update(equipmentDTO);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            EquipmentBLRepository.Delete(EquipmentBLRepository.FindById(id));
        }
    }
}
