using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using Ninject;
using BusinessLogic.Models;
using DB_Layer.Models;
using AutoMapper;
using Moq;
using DB_Layer.Interfaces;

namespace UnitTests.Tests
{
    [TestClass]
    public class ActivityServiceTest
    {
        [TestMethod]
        public void GetAllActivitiesTest()
        {
            //Arrange
            List<Activity> activities = new List<Activity>()
            {
                new Activity(){Id = 1, Name = "FirstActivity", Description = "FirstDescription", IsSpecialActivity = false, PricePerHour = 100},
                new Activity(){Id = 2, Name = "SecondActivity", Description = "SecondDescription", IsSpecialActivity = false, PricePerHour = 200},
                new Activity(){Id = 3, Name = "ThirdActivity", Description = "ThirdDescription", IsSpecialActivity = true, PricePerHour = 100},
                new Activity(){Id = 4, Name = "ForthActivity", Description = "ForthDescription", IsSpecialActivity = false, PricePerHour = 300},
                new Activity(){Id = 5, Name = "FifthActivity", Description = "FifthDescription", IsSpecialActivity = true, PricePerHour = 100},
                new Activity(){Id = 6, Name = "SixthActivity", Description = "SixthDescription", IsSpecialActivity = false, PricePerHour = 500},
            };

            var mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().ActivityToDTOMapper();

            //Act
            var activityRepositoryMock = new Mock<IGenericRepository<Activity>>();
            activityRepositoryMock.Setup(a => a.Get(It.IsAny<Func<Activity,bool>>())).Returns(activities.Where(q => q.IsSpecialActivity == false).ToList());

            var IUnitOfWorkMock = new Mock<IUnitOfWork<AntiCafeDb>>();
            IUnitOfWorkMock.Setup(u => u.activitiesRepository).Returns(activityRepositoryMock.Object);

            IActivityService activityService = new ActivityService(IUnitOfWorkMock.Object, DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>());
            List<ActivityDTO> activityDTOs = activityService.GetAllActivities();

            //Assert
            activities = activities.Where(a => a.IsSpecialActivity == false).ToList();
            Assert.AreNotEqual(activityDTOs.Count, 0);
            for (int i = 0; i < activityDTOs.Count; i++)
            {
                Assert.AreEqual(activityDTOs[i].Id, activities[i].Id);
                Assert.AreEqual(activityDTOs[i].Name, activities[i].Name);
                Assert.AreEqual(activityDTOs[i].Description, activities[i].Description);
                Assert.AreEqual(activityDTOs[i].PricePerHour, activities[i].PricePerHour);
                Assert.AreEqual(activityDTOs[i].IsSpecialActivity, activities[i].IsSpecialActivity);
            }
        }

        [TestMethod]
        public void CreateNewActivityTest()
        {
            //Arrange
            ActivityDTO newActivity = new ActivityDTO() { Id = 1, Name = "NewActivity", IsSpecialActivity = false, PricePerHour = 100 };
            List<RoomDTO> roomsToAdd = new List<RoomDTO>()
            {
                new RoomDTO(){Id = 2, Name = "Room2"},
            };

            List<Room> roomsOriginal = new List<Room>()
            {
                new Room(){Id = 2, Name = "Room2"},
            };

            var mapper = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>().DTOToActivityMapper();
            Activity activity1 = mapper.Map<Activity>(newActivity);
            int id = 2;

            //Act
            var activityRepositoryMock = new Mock<IGenericRepository<Activity>>();
            activityRepositoryMock.Setup(a => a.Create(activity1));

            var roomRepositoryMock = new Mock<IGenericRepository<Room>>();
            roomRepositoryMock.Setup(r => r.FindById(id)).Returns(roomsOriginal.Where(r => r.Id == id).FirstOrDefault());

            var IUnitOfWorkMock = new Mock<IUnitOfWork<AntiCafeDb>>();
            IUnitOfWorkMock.Setup(u => u.activitiesRepository).Returns(activityRepositoryMock.Object);
            IUnitOfWorkMock.Setup(u => u.roomsRepository).Returns(roomRepositoryMock.Object);

            IActivityService activityService = new ActivityService(IUnitOfWorkMock.Object, DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>());
            ActivityDTO createdActivity = activityService.CreateNewActivity(newActivity, roomsToAdd);

            //Assert
            Assert.AreEqual(createdActivity.Id, newActivity.Id);
            Assert.AreEqual(createdActivity.Name, newActivity.Name);
            Assert.AreEqual(createdActivity.IsSpecialActivity, newActivity.IsSpecialActivity);
            Assert.AreEqual(createdActivity.PricePerHour, newActivity.PricePerHour);
        }
    }
}
