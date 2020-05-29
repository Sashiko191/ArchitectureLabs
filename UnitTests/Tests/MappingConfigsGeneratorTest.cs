using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Ninject;
using AutoMapper;
using DB_Layer.Models;
using BusinessLogic.Models;

namespace UnitTests.Tests
{
    [TestClass]
    public class MappingConfigsGeneratorTest
    {
        [TestMethod]
        public void RoomToDTOMapperTest()
        {
            //Arrange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper RoomTODTOMapper = generator.RoomToDTOMapper();

            //Act           
            Room room = new Room() { Id = 1, Name = "Room1" };
            RoomDTO roomDTO = RoomTODTOMapper.Map<RoomDTO>(room);

            //Assert
            Assert.AreEqual(room.Id, roomDTO.Id);
            Assert.AreEqual(room.Name, roomDTO.Name);
        }

        [TestMethod]
        public void DTOToRoomMapper()
        {
            //Arange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper DTOToRoomMapper = generator.DTOToRoomMapper();

            //Act
            RoomDTO roomDTO = new RoomDTO() { Id = 1, Name = "Room1" };
            Room room = DTOToRoomMapper.Map<Room>(roomDTO);

            //Assert
            Assert.AreEqual(roomDTO.Id, room.Id);
            Assert.AreEqual(roomDTO.Name, room.Name);
        }

        [TestMethod]
        public void OrderToDTOMapper()
        {
            //Arange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper OrderToDTOMapper = generator.OrderToDTOMapper();

            //Act
            DateTime dateTime = DateTime.Now;
            Order order = new Order() { Id = 1, Hours = 2, Name = "SomeName", Surname = "SomeSurname", StartDate = dateTime };
            OrderDTO orderDTO = OrderToDTOMapper.Map<OrderDTO>(order);

            //Assert
            Assert.AreEqual(order.Id, orderDTO.Id);
            Assert.AreEqual(order.Name, orderDTO.Name);
            Assert.AreEqual(order.Hours, orderDTO.Hours);
            Assert.AreEqual(order.Surname, orderDTO.Surname);
            Assert.AreEqual(order.StartDate, orderDTO.StartDate);
        }

        [TestMethod]
        public void DTOToOrderMapper()
        {
            //Arange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper DTOToOrderMapper = generator.DTOToOrderMapper();

            //Act
            DateTime dateTime = DateTime.Now;
            OrderDTO orderDTO = new OrderDTO() { Id = 1, Hours = 2, Name = "SomeName", Surname = "SomeSurname", StartDate = dateTime };
            Order order = DTOToOrderMapper.Map<Order>(orderDTO);

            //Assert
            Assert.AreEqual(orderDTO.Id, order.Id);
            Assert.AreEqual(orderDTO.Name, order.Name);
            Assert.AreEqual(orderDTO.Hours, order.Hours);
            Assert.AreEqual(orderDTO.Surname, order.Surname);
            Assert.AreEqual(orderDTO.StartDate, order.StartDate);
        }

        [TestMethod]
        public void ActivityToDTOMapper()
        {
            //Arange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper ActivityToDTOMapper = generator.ActivityToDTOMapper();

            //Act
            Activity activity = new Activity() { Id = 1, Name = "SomeName", Description = "SomeDescription", IsSpecialActivity = true, PricePerHour = 100 };
            ActivityDTO activityDTO = ActivityToDTOMapper.Map<ActivityDTO>(activity);

            //Assert
            Assert.AreEqual(activity.Id, activityDTO.Id);
            Assert.AreEqual(activity.Name, activityDTO.Name);
            Assert.AreEqual(activity.IsSpecialActivity, activityDTO.IsSpecialActivity);
            Assert.AreEqual(activity.PricePerHour, activityDTO.PricePerHour);
            Assert.AreEqual(activity.Description, activityDTO.Description);
        }

        [TestMethod]
        public void DTOToActivityMapper()
        {
            //Arange
            IMappingConfigsGenerator generator = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
            Mapper DTOToActivityMapper = generator.DTOToActivityMapper();

            //Act
            ActivityDTO activityDTO = new ActivityDTO() { Id = 1, Name = "SomeName", Description = "SomeDescription", IsSpecialActivity = true, PricePerHour = 100 };
            Activity activity = DTOToActivityMapper.Map<Activity>(activityDTO);

            //Assert
            Assert.AreEqual(activityDTO.Id, activity.Id);
            Assert.AreEqual(activityDTO.Name, activity.Name);
            Assert.AreEqual(activityDTO.IsSpecialActivity, activity.IsSpecialActivity);
            Assert.AreEqual(activityDTO.PricePerHour, activity.PricePerHour);
            Assert.AreEqual(activityDTO.Description, activity.Description);
        }
    }
}
