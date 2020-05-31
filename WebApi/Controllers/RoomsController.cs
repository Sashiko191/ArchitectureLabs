using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Services;
using BusinessLogic.BL_Repositories;
using WebApi.Models;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Ninject;
using WebApi.Utils;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class RoomsController : ApiController
    {
        private IViewModelMappingConfigsGenerator configsGenerator;
        private IBusinessLogicRepository<RoomDTO> RoomsBLRepository;

        public RoomsController()
        {
            IKernel webApiDIResolver = WebApiDIResolver.GetDIResolver();
            RoomsBLRepository = webApiDIResolver.Get<IBusinessLogicRepository<RoomDTO>>();
            configsGenerator = webApiDIResolver.Get<IViewModelMappingConfigsGenerator>();
        }

        [HttpGet]
        public List<RoomViewModel> Get(bool WithActivities, bool WithEquipment, bool WithOrders)
        {
            List<RoomDTO> roomDTOs;
            if (WithActivities || WithEquipment || WithOrders)
                roomDTOs = RoomsBLRepository.GetWithEverything();
            else
                roomDTOs = RoomsBLRepository.Get();
            List<RoomViewModel> roomViewModels = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(roomDTOs);
            for (int i = 0; i < roomViewModels.Count; i++)
            {
                if (WithActivities)
                    roomViewModels[i].PosibleActivities = configsGenerator.DTOToActivityViewModel().Map<List<ActivityViewModel>>(roomDTOs[i].IncludedActivities);
                if (WithEquipment)
                    roomViewModels[i].RoomEquipment = configsGenerator.DTOToEquipmentViewModel().Map<List<EquipmentViewModel>>(roomDTOs[i].IncludedEquipment);
                if (WithOrders)
                    roomViewModels[i].RoomOrders = configsGenerator.DTOToOrderViewModel().Map<List<OrderViewModel>>(roomDTOs[i].IncludedOrders);
            }
            return roomViewModels;
        }
  
        [HttpGet]
        public RoomViewModel Get(int id, bool WithEverything)
        {
            RoomDTO roomDTO;
            RoomViewModel roomViewModel;
            if (WithEverything)
            {
                roomDTO = RoomsBLRepository.FindByIdWithEverything(id);
                roomViewModel = configsGenerator.DTOToRoomViewModel().Map<RoomViewModel>(roomDTO);
                roomViewModel.PosibleActivities = configsGenerator.DTOToActivityViewModel().Map<List<ActivityViewModel>>(roomDTO.IncludedActivities);
                roomViewModel.RoomEquipment = configsGenerator.DTOToEquipmentViewModel().Map<List<EquipmentViewModel>>(roomDTO.IncludedEquipment);
                roomViewModel.RoomOrders = configsGenerator.DTOToOrderViewModel().Map<List<OrderViewModel>>(roomDTO.IncludedOrders);
            }
            else
            {
                roomDTO = RoomsBLRepository.FindById(id);
                roomViewModel = configsGenerator.DTOToRoomViewModel().Map<RoomViewModel>(roomDTO);
            }
            return roomViewModel;
        }

        [HttpPost]
        public RoomViewModel Post([FromBody]RoomViewModel roomViewModel)
        {
            RoomDTO roomDTO = configsGenerator.ViewModelToRoomDTO().Map<RoomDTO>(roomViewModel);
            roomDTO.IncludedActivities = configsGenerator.ViewModelToActivityDTO().Map<List<ActivityDTO>>(roomViewModel.PosibleActivities);
            roomDTO.IncludedEquipment = configsGenerator.ViewModelToEquipmentDTO().Map<List<EquipmentDTO>>(roomViewModel.RoomEquipment);
            roomDTO.IncludedOrders = configsGenerator.ViewModelToOrderDTO().Map<List<OrderDTO>>(roomViewModel.RoomOrders);
            RoomDTO CreatedRoomDTO = RoomsBLRepository.Create(roomDTO);
            RoomViewModel CreatedRoomViewModel = configsGenerator.DTOToRoomViewModel().Map<RoomViewModel>(CreatedRoomDTO);
            CreatedRoomViewModel.PosibleActivities = configsGenerator.DTOToActivityViewModel().Map<List<ActivityViewModel>>(CreatedRoomDTO.IncludedActivities);
            CreatedRoomViewModel.RoomEquipment = configsGenerator.DTOToEquipmentViewModel().Map<List<EquipmentViewModel>>(CreatedRoomDTO.IncludedEquipment);
            CreatedRoomViewModel.RoomOrders = configsGenerator.DTOToOrderViewModel().Map<List<OrderViewModel>>(CreatedRoomDTO.IncludedOrders);
            return CreatedRoomViewModel;
        }
        
        [HttpPost]
        public List<RoomViewModel> FillRooms([FromBody]OrderSearchViewModel orderSearchViewModel)
        {
            IOrderService orderService = new OrderService();
            List<RoomViewModel> ResultViewModels = new List<RoomViewModel>(orderSearchViewModel.RoomViewModels);

            for (int i = 0; i < orderSearchViewModel.RoomViewModels.Count; i++)
            {
                RoomDTO roomDTO = configsGenerator.ViewModelToRoomDTO().Map<RoomDTO>(orderSearchViewModel.RoomViewModels[i]);
                List<OrderDTO> foundedOrders = orderService.GetActiveOrders(orderSearchViewModel.DesiredDateTime, roomDTO);

                for (int j = 0; j < foundedOrders.Count; j++)
                {
                    OrderViewModel viewModel = configsGenerator.DTOToOrderViewModel().Map<OrderViewModel>(foundedOrders[j]);
                    if (ResultViewModels[i].RoomOrders == null)
                        ResultViewModels[i].RoomOrders = new List<OrderViewModel>();
                    ResultViewModels[i].RoomOrders.Add(viewModel);
                }
            }

            return ResultViewModels;
        }
        
        [HttpPut]
        public void Update([FromBody]RoomViewModel roomViewModel)
        {
            RoomDTO roomDTO = configsGenerator.ViewModelToRoomDTO().Map<RoomDTO>(roomViewModel);
            roomDTO.IncludedActivities = configsGenerator.ViewModelToActivityDTO().Map<List<ActivityDTO>>(roomViewModel.PosibleActivities);
            roomDTO.IncludedEquipment = configsGenerator.ViewModelToEquipmentDTO().Map<List<EquipmentDTO>>(roomViewModel.RoomEquipment);
            roomDTO.IncludedOrders = configsGenerator.ViewModelToOrderDTO().Map<List<OrderDTO>>(roomViewModel.RoomOrders);
            RoomsBLRepository.Update(roomDTO);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            RoomsBLRepository.Delete(RoomsBLRepository.FindById(id));
        }       
    }
}
