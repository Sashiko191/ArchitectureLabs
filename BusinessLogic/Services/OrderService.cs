using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DB_Layer.Repositories;
using DB_Layer.Models;
using System.Data.Entity;
using Ninject;
using DB_Layer.Interfaces;

namespace BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mapConfigsGenerator;
        private IKernel DIResolver;
        public OrderService()
        {
            DIResolver = DI_Resolver.GetDIResolver();
            unitOfWork = DIResolver.Get<IUnitOfWork<AntiCafeDb>>();
            mapConfigsGenerator = DIResolver.Get<IMappingConfigsGenerator>();
        }
        public OrderService(IUnitOfWork<AntiCafeDb> _unitOfWork, IMappingConfigsGenerator _mappingConfigsGenerator)
        {
            unitOfWork = _unitOfWork;
            mapConfigsGenerator = _mappingConfigsGenerator;
        }
        public List<OrderDTO> GetActiveOrders(DateTime desiredDate, RoomDTO room)
        {
            var mapper = mapConfigsGenerator.OrderToDTOMapper();
            List<OrderDTO> AllOrders = mapper.Map<List<OrderDTO>>(unitOfWork.ordersRepository.Get(o => o.StartDate.Date == desiredDate.Date).ToList());
            //List<OrderDTO> AllOrders = mapper.Map<List<OrderDTO>>(unitOfWork.db.Orders.Where(o => DbFunctions.TruncateTime(o.StartDate) == desiredDate.Date).ToList());

            for (int i = 0; i < AllOrders.Count; i++)
            {
                List<RoomDTO> orderedRooms = AllOrders[i].GetOrderedRooms();
                for (int j = 0; j < orderedRooms.Count; j++)
                {
                    if (orderedRooms[j].Id == room.Id)
                    {
                        break;
                    }
                    else
                    {
                        if (j == orderedRooms.Count - 1)
                        {
                            AllOrders.RemoveAt(i);
                            i = i - 1;
                            if (i < 0)
                                i = 0;
                        }
                    }
                }
            }

            return AllOrders;
        }
        public void CreateNewOrder(OrderDTO orderDTO, List<RoomDTO> orderedRooms)
        {
            var mapper = mapConfigsGenerator.DTOToOrderMapper();
            Order order = mapper.Map<Order>(orderDTO);

            List<Room> rooms = new List<Room>();
            for (int i = 0; i < orderedRooms.Count; i++)
            {
                rooms.Add(unitOfWork.roomsRepository.FindById(orderedRooms[i].Id));
            }

            order.OrderedRooms = rooms;
            unitOfWork.ordersRepository.Create(order);
        }
        public bool HasIntersection(DateTime DesiredDate, int Hours, List<RoomDTO> rooms)
        {
            DateTime StartDateTime = DesiredDate;
            DateTime EndDateTime = DesiredDate.AddHours(Hours);

            if (StartDateTime.Hour > 8 && EndDateTime.Hour > StartDateTime.Hour && EndDateTime.Hour < 24)
            {

                for (int i = 0; i < rooms.Count; i++)
                {
                    List<OrderDTO> AllOrdersForRoom = GetActiveOrders(DesiredDate, rooms[i]);
                    for (int j = 0; j < AllOrdersForRoom.Count; j++)
                    {
                        DateTime OrderStartDateTime = AllOrdersForRoom[j].StartDate;
                        DateTime OrderEndDateTime = AllOrdersForRoom[j].StartDate.AddHours(AllOrdersForRoom[j].Hours);

                        if (StartDateTime < OrderStartDateTime)
                        {
                            if (EndDateTime <= OrderStartDateTime)
                            {

                            }
                            else
                                return true;
                        }
                        else if (StartDateTime >= OrderEndDateTime)
                        {
                            if (EndDateTime > OrderEndDateTime)
                            {

                            }
                            else
                                return true;
                        }
                        else
                            return true;

                    }
                }
            }
            else
                return true;

            return false;
        }
    }
}
