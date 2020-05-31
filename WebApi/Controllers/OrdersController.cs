using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;
using DB_Layer;
using DB_Layer.Models;
using Newtonsoft.Json;
using BusinessLogic.BL_Repositories;
using WebApi.Services;
using WebApi.Models;
using Ninject;
using WebApi.Utils;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class OrdersController : ApiController
    {
        private IViewModelMappingConfigsGenerator configsGenerator;
        private IBusinessLogicRepository<OrderDTO> OrdersBLRepository;
        private IOrderService orderService;

        public OrdersController()
        {
            IKernel webApiDIResolver = WebApiDIResolver.GetDIResolver();
            OrdersBLRepository = webApiDIResolver.Get<IBusinessLogicRepository<OrderDTO>>();
            orderService = webApiDIResolver.Get<IOrderService>();
            configsGenerator = webApiDIResolver.Get<IViewModelMappingConfigsGenerator>();
        }

        [HttpGet]
        public List<OrderViewModel> Get(bool WithRooms)
        {
            List<OrderDTO> orderDTOs;
            if (WithRooms)
                orderDTOs = OrdersBLRepository.GetWithEverything();
            else
                orderDTOs = OrdersBLRepository.Get();
            List<OrderViewModel> orderViewModels = configsGenerator.DTOToOrderViewModel().Map<List<OrderViewModel>>(orderDTOs);
            for (int i = 0; i < orderViewModels.Count; i++)
            {
                orderViewModels[i].OrderedRooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(orderDTOs[i].IncludedRooms);
            }
            return orderViewModels;
        }

        [HttpGet]
        public OrderViewModel Get(int id, bool WithRooms)
        {
            OrderDTO orderDTO;
            OrderViewModel orderViewModel;
            if (WithRooms)
            {
                orderDTO = OrdersBLRepository.FindByIdWithEverything(id);
                orderViewModel = configsGenerator.DTOToOrderViewModel().Map<OrderViewModel>(orderDTO);
                orderViewModel.OrderedRooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(orderDTO.IncludedRooms);
            }
            else
            {
                orderDTO = OrdersBLRepository.FindById(id);
                orderViewModel = configsGenerator.DTOToOrderViewModel().Map<OrderViewModel>(orderDTO);

            }
            return orderViewModel;
        }

        [HttpPost]
        public OrderViewModel Post([FromBody]OrderViewModel orderViewModel)
        {
            OrderDTO orderDTO = configsGenerator.ViewModelToOrderDTO().Map<OrderDTO>(orderViewModel);
            orderDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(orderViewModel.OrderedRooms);
            if (!orderService.HasIntersection(orderDTO.StartDate, orderDTO.Hours, orderDTO.IncludedRooms))
            {
                OrderDTO CreatedOrderDTO = OrdersBLRepository.Create(orderDTO);
                OrderViewModel ReturnedOrderViewModel = configsGenerator.DTOToOrderViewModel().Map<OrderViewModel>(CreatedOrderDTO);
                ReturnedOrderViewModel.OrderedRooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(CreatedOrderDTO.IncludedRooms);
                return ReturnedOrderViewModel;
            }
            return null;
        }

        [HttpPut]
        public void Update([FromBody]OrderViewModel orderViewModel)
        {
            OrderDTO orderDTO = configsGenerator.ViewModelToOrderDTO().Map<OrderDTO>(orderViewModel);
            orderDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(orderViewModel.OrderedRooms);
            OrdersBLRepository.Update(orderDTO);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            OrdersBLRepository.Delete(OrdersBLRepository.FindById(id));
        }
    }
}
