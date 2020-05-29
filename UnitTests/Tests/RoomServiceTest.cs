using BusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Ninject;
using DB_Layer.Models;
using AutoMapper;
using Moq;
using DB_Layer.Interfaces;

namespace UnitTests.Tests
{
    [TestClass]
    public class RoomServiceTest
    {
        [TestMethod]
        public void GetAllRoomsTest()
        {
            //Arrange
            List<Room> rooms = new List<Room>()
            {
                new Room(){Id = 1, Name = "Room1"},
                new Room(){Id = 2, Name = "Room2"},
                new Room(){Id = 3, Name = "Room3"},
                new Room(){Id = 4, Name = "Room4"}
            };
            Mapper mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().RoomToDTOMapper();

            //Act
            var RoomRepositoryMock = new Mock<IGenericRepository<Room>>();
            RoomRepositoryMock.Setup(m => m.Get()).Returns(rooms);

            var IUnitOfWorkMock = new Mock<IUnitOfWork<AntiCafeDb>>();
            IUnitOfWorkMock.Setup(i => i.roomsRepository).Returns(RoomRepositoryMock.Object);

            IRoomService roomService = new RoomService(IUnitOfWorkMock.Object, DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>());
            List<RoomDTO> mappedRooms = roomService.GetAllRooms();

            //Assert
            for (int i = 0;i < mappedRooms.Count;i++)
            {
                Assert.AreEqual(mappedRooms[i].Id, rooms[i].Id);
                Assert.AreEqual(mappedRooms[i].Name, rooms[i].Name);
            }

            Assert.AreEqual(mappedRooms.Count, rooms.Count);
        }

        [TestMethod]
        public void GetRoomTest()
        {
            //Arrange
            List<Room> rooms = new List<Room>()
            {
                new Room(){Id = 1, Name = "Room1"},
                new Room(){Id = 2, Name = "Room2"},
                new Room(){Id = 3, Name = "Room3"}
            };
            int id = 2;
            Room room = rooms.Where(r => r.Id == id).FirstOrDefault();
            Mapper mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().RoomToDTOMapper();

            //Act
            var RoomRepositoryMock = new Mock<IGenericRepository<Room>>();
            RoomRepositoryMock.Setup(m => m.FindById(id)).Returns(rooms.Where(t => t.Id == id).FirstOrDefault());

            var IUnitOfWorkMock = new Mock<IUnitOfWork<AntiCafeDb>>();
            IUnitOfWorkMock.Setup(i => i.roomsRepository).Returns(RoomRepositoryMock.Object);

            IRoomService roomService = new RoomService(IUnitOfWorkMock.Object, DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>());
            RoomDTO roomDTO = roomService.GetRoom(id);

            //Assert
            Assert.AreEqual(roomDTO.Id, room.Id);
            Assert.AreEqual(roomDTO.Name, room.Name);     
        }
    }
}
