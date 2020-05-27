﻿using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.UnitOfWorkRealization;
using DB_Layer.Models;
using DB_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ActivityService : IActivityService
    {
        private UnitOfWork unitOfWork;
        private MappingConfigsGenerator mapConfigsGenerator;

        public ActivityService()
        {
            unitOfWork = UnitOfWork.GetUnitOfWork();
            mapConfigsGenerator = new MappingConfigsGenerator();
        }
        public List<ActivityDTO> GetAllActivities()
        {        
            var mapper = mapConfigsGenerator.ActivityToDTOMapper();
            List<ActivityDTO> activities = mapper.Map<List<ActivityDTO>>(unitOfWork.activitiesRepository.Get(a => a.IsSpecialActivity == false));
            return activities;
        }
        public ActivityDTO CreateNewActivity(ActivityDTO activityDTO, List<RoomDTO> roomDTOs)
        {
            var mapper = mapConfigsGenerator.DTOToActivityMapper();
            Activity newActivity = mapper.Map<Activity>(activityDTO);

            List<Room> rooms = new List<Room>();
            for (int i = 0; i < roomDTOs.Count; i++)
            {
                rooms.Add(unitOfWork.roomsRepository.FindById(roomDTOs[i].Id));
            }

            newActivity.PosibleRooms = rooms;
            unitOfWork.activitiesRepository.Create(newActivity);

            mapper = mapConfigsGenerator.ActivityToDTOMapper();
            return mapper.Map<ActivityDTO>(newActivity);
        }
    }
}