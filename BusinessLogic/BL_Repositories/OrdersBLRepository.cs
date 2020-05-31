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
    public class OrdersBLRepository : IBusinessLogicRepository<OrderDTO>
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mappingConfigs; 
        public OrdersBLRepository()
        {
            unitOfWork = DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>();
            mappingConfigs = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
        }
        public List<OrderDTO> Get()
        {
            return mappingConfigs.OrderToDTOMapper().Map<List<OrderDTO>>(unitOfWork.ordersRepository.Get());
        }
        public List<OrderDTO> GetWithEverything()
        {
            List<OrderDTO> orderDTOs = mappingConfigs.OrderToDTOMapper().Map<List<OrderDTO>>(unitOfWork.ordersRepository.Get());
            for(int i = 0;i < orderDTOs.Count;i++)
            {
                orderDTOs[i].IncludedRooms = orderDTOs[i].GetOrderedRooms();
            }
            return orderDTOs;
        }
        public OrderDTO FindById(int id)
        {
            return mappingConfigs.OrderToDTOMapper().Map<OrderDTO>(unitOfWork.ordersRepository.FindById(id));
        }
        public OrderDTO FindByIdWithEverything(int id)
        {
            OrderDTO orderDTO = FindById(id);
            orderDTO.IncludedRooms = orderDTO.GetOrderedRooms();
            return orderDTO;
        }
        public OrderDTO Create(OrderDTO orderDTO)
        {
            Order order = mappingConfigs.DTOToOrderMapper().Map<Order>(orderDTO);
            if(orderDTO.IncludedRooms != null && orderDTO.IncludedRooms.Count != 0)
            {
                List<Room> rooms = new List<Room>();
                for(int i = 0;i < orderDTO.IncludedRooms.Count;i++)
                {
                    rooms.Add(unitOfWork.roomsRepository.FindById(orderDTO.IncludedRooms[i].Id));
                }
                order.OrderedRooms = rooms;
            }
            unitOfWork.ordersRepository.Create(order);
            OrderDTO CreatedOrderDTO = mappingConfigs.OrderToDTOMapper().Map<OrderDTO>(order);
            CreatedOrderDTO.IncludedRooms = mappingConfigs.RoomToDTOMapper().Map<List<RoomDTO>>(order.OrderedRooms);
            return CreatedOrderDTO;
        }    
        public void Update(OrderDTO orderDTO)
        {
            Order order = mappingConfigs.DTOToOrderMapper().Map<Order>(orderDTO);
            List<Room> rooms = new List<Room>();
            for (int i = 0;i < orderDTO.IncludedRooms.Count;i++)
            {
                rooms.Add(unitOfWork.roomsRepository.FindById(orderDTO.IncludedRooms[i].Id));
            }
            order.OrderedRooms = rooms;

            int searchedId = orderDTO.Id;
            Order ExistingOrder = unitOfWork.ordersRepository.FindById(searchedId);

            ExistingOrder.Name = order.Name;
            ExistingOrder.Surname = order.Surname;
            ExistingOrder.StartDate = order.StartDate;
            ExistingOrder.Hours = order.Hours;
            ExistingOrder.OrderedRooms = order.OrderedRooms;
            unitOfWork.ordersRepository.Update(ExistingOrder);
        }
        public void Delete(OrderDTO orderDTO)
        {
            int id = orderDTO.Id;
            unitOfWork.ordersRepository.Remove(unitOfWork.ordersRepository.FindById(id));
        }
    }
}
