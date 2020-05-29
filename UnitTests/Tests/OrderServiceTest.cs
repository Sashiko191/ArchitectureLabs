using BusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BusinessLogic.Interfaces;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using BusinessLogic.Services;
using Ninject;
using System.Data.Entity;

namespace UnitTests.Tests
{
    [TestClass]
    public class OrderServiceTest
    {
        [TestMethod]
        public void GetActiveOrdersTest()
        {
            //Arrange
            Room room1 = new Room() { Id = 1, Name = "Room1" };
            Room room2 = new Room() { Id = 2, Name = "Room2" };
            Room room3 = new Room() { Id = 3, Name = "Room3" };
            Room room4 = new Room() { Id = 4, Name = "Room4" };
            Room room5 = new Room() { Id = 5, Name = "Room5" };

            List<Order> someOrders = new List<Order>()
            {
                new Order(){Id = 1, Name = "Customer1", Surname = "Surname1", StartDate = new DateTime(2020,4,3), Hours = 1, OrderedRooms = new List<Room>(){room1, room2 }},
                new Order(){Id = 2, Name = "Customer2", Surname = "Surname2", StartDate = new DateTime(2020,4,6), Hours = 2, OrderedRooms = new List<Room>(){room2, room3}},
                new Order(){Id = 3, Name = "Customer3", Surname = "Surname3", StartDate = new DateTime(2020,4,5), Hours = 2, OrderedRooms = new List<Room>(){room1, room5 }},
                new Order(){Id = 4, Name = "Customer4", Surname = "Surname4", StartDate = new DateTime(2020,4,3), Hours = 2, OrderedRooms = new List<Room>(){room4 }},
                new Order(){Id = 5, Name = "Customer5", Surname = "Surname5", StartDate = new DateTime(2020,4,5), Hours = 2, OrderedRooms = new List<Room>(){room4, room5 }}
            };

            DateTime someDateTime = new DateTime(2020, 4, 5);
            Room searchedRoom = room5;

            var mock = new Mock<IGenericRepository<Order>>();
            mock.Setup(o => o.Get()).Returns(someOrders.Where(o => o.StartDate.Date == someDateTime.Date));


            //Act
            var mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().OrderToDTOMapper();
            List<OrderDTO> AllOrders = mapper.Map<List<OrderDTO>>(mock.Object.Get());

            for (int i = 0; i < AllOrders.Count; i++)
            {
                var roomMapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().RoomToDTOMapper();
                List<RoomDTO> orderedRooms = roomMapper.Map<List<RoomDTO>>(someOrders.Where(o => o.Id == AllOrders[i].Id).FirstOrDefault().OrderedRooms);

                for (int j = 0; j < orderedRooms.Count; j++)
                {
                    if (orderedRooms[j].Id == searchedRoom.Id)
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


            //Assert
            for (int i = 0; i < AllOrders.Count; i++)
            {
                Assert.AreEqual(someDateTime, AllOrders[i].StartDate);
                Assert.IsTrue(someOrders.Where(o => o.Id == AllOrders[i].Id).FirstOrDefault().OrderedRooms.Contains(searchedRoom));
            }
        }

        [TestMethod]
        public void CreateNewOrderTest()
        {
            //Arrange
            List<Order> orders = new List<Order>();
            List<RoomDTO> roomDTOsToAdd = new List<RoomDTO>()
            {
                new RoomDTO(){Id = 1, Name = "Room1"},
                new RoomDTO(){Id = 2, Name = "Room2"},
                new RoomDTO(){Id = 3, Name = "Room3"}
            };

            //Act
            OrderDTO testOrderDTO = new OrderDTO() { Id = 1, Name = "Customer1", Surname = "Surname1", StartDate = new DateTime(2020, 4, 5), Hours = 1 };
            var mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().DTOToOrderMapper();
            Order testOrder = mapper.Map<Order>(testOrderDTO);

            List<Room> roomsToAdd = new List<Room>();
            mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().DTOToRoomMapper();
            for (int i = 0; i < roomDTOsToAdd.Count; i++)
            {
                roomsToAdd.Add(mapper.Map<Room>(roomDTOsToAdd[i]));
            }

            testOrder.OrderedRooms = roomsToAdd;

            var mock = new Mock<IGenericRepository<Order>>();
            mock.Setup(o => o.Create(testOrder)).Callback(() => { orders.Add(testOrder); });
            mock.Object.Create(testOrder);

            //Assert
            Assert.AreEqual(orders.Count, 1);
            for (int i = 0; i < testOrder.OrderedRooms.Count; i++)
            {
                Assert.AreEqual(testOrder.OrderedRooms.ToList()[i].Id, roomDTOsToAdd[i].Id);
            }
        }

        [TestMethod]
        public void HasIntersection()
        {
            //Arrange
            bool Flag = false;
            DateTime DesiredDate = new DateTime(2020, 4, 5, 17,0,0);
            int Hours = 2;
            List<RoomDTO> rooms = new List<RoomDTO>()
            {
                new RoomDTO(){Id = 1, Name = "Room1"},
                new RoomDTO(){Id = 2, Name = "Room2"},
                new RoomDTO(){Id = 3, Name = "Room3"},
                new RoomDTO(){Id = 4, Name = "Room4"}
            };

            List<OrderDTO> GetTestActiveOrders(DateTime ActiveDateTime, RoomDTO room)
            {
                if (room.Id == 2)
                {
                    return new List<OrderDTO>()
                    {
                        new OrderDTO(){Id = 1, Name = "Customer1", Surname = "Surname1", StartDate = new DateTime(2020,4,5,9,0,0), Hours = 1 },
                        new OrderDTO(){Id = 2, Name = "Customer2", Surname = "Surname2", StartDate = new DateTime(2020,4,5,10,0,0), Hours = 2},
                        new OrderDTO(){Id = 3, Name = "Customer3", Surname = "Surname3", StartDate = new DateTime(2020,4,5,14,0,0), Hours = 3},
                        new OrderDTO(){Id = 4, Name = "Customer4",Surname = "Surname4", StartDate = new DateTime(2020,4,5,19,0,0), Hours = 3}
                    };
                }
                else if (room.Id == 3)
                {
                    return new List<OrderDTO>()
                    {
                        new OrderDTO(){Id = 5, Name = "Customer5", Surname = "Surname5", StartDate = new DateTime(2020,4,5,10,0,0), Hours = 4},
                        new OrderDTO(){Id = 6, Name = "Customer6", Surname = "Surname6", StartDate = new DateTime(2020,4,5,22,0,0), Hours = 1}
                    };
                }
                else
                    return new List<OrderDTO>();
            }
       
            //Act
            DateTime StartDateTime = DesiredDate;
            DateTime EndDateTime = DesiredDate.AddHours(Hours);

            if (StartDateTime.Hour > 8 && EndDateTime.Hour > StartDateTime.Hour && EndDateTime.Hour < 24)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    List<OrderDTO> AllOrdersForRoom = GetTestActiveOrders(DesiredDate, rooms[i]);
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
                                Flag = true;
                        }
                        else if (StartDateTime >= OrderEndDateTime)
                        {
                            if (EndDateTime > OrderEndDateTime)
                            {

                            }
                            else
                                Flag = true;
                        }
                        else
                            Flag = true;

                    }
                }
            }
            else
                Flag = true;

            //Assert
            Assert.AreEqual(false, Flag);
        }
    }
}
