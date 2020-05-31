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
    public class ActivitiesBLRepository : IBusinessLogicRepository<ActivityDTO>
    {
        private IUnitOfWork<AntiCafeDb> unitOfWork;
        private IMappingConfigsGenerator mappingConfigs;
        public ActivitiesBLRepository()
        {
            unitOfWork = DI_Resolver.GetDIResolver().Get<IUnitOfWork<AntiCafeDb>>();
            mappingConfigs = DI_Resolver.GetDIResolver().Get<IMappingConfigsGenerator>();
        }

        public List<ActivityDTO> Get()
        {
            return mappingConfigs.ActivityToDTOMapper().Map<List<ActivityDTO>>(unitOfWork.activitiesRepository.Get());
        }

        public List<ActivityDTO> GetWithEverything()
        {
            List<ActivityDTO> activities = Get();
            for(int i = 0;i < activities.Count;i++)
            {
                activities[i].IncludedRooms = activities[i].GetPosibleRooms();
            }
            return activities;
        }

        public ActivityDTO FindById(int id)
        {
            return mappingConfigs.ActivityToDTOMapper().Map<ActivityDTO>(unitOfWork.activitiesRepository.FindById(id));
        }

        public ActivityDTO FindByIdWithEverything(int id)
        {
            ActivityDTO activity = FindById(id);
            activity.IncludedRooms = activity.GetPosibleRooms();
            return activity;
        }

        public ActivityDTO Create(ActivityDTO activityDTO)
        {
            Activity activity = mappingConfigs.DTOToActivityMapper().Map<Activity>(activityDTO);
            if (activityDTO.IncludedRooms != null && activityDTO.IncludedRooms.Count != 0)
            {
                List<Room> rooms = new List<Room>();
                for(int i = 0;i < activityDTO.IncludedRooms.Count;i++)
                {
                    rooms.Add(unitOfWork.roomsRepository.FindById(activityDTO.IncludedRooms[i].Id));
                }
                activity.PosibleRooms = rooms;
            }
            unitOfWork.activitiesRepository.Create(activity); 
            ActivityDTO ReturnedActivityDTO = mappingConfigs.ActivityToDTOMapper().Map<ActivityDTO>(activity);
            ReturnedActivityDTO.IncludedRooms = ReturnedActivityDTO.GetPosibleRooms();
            return ReturnedActivityDTO;
        }

        public void Delete(ActivityDTO activityDTO)
        {
            int id = activityDTO.Id;
            unitOfWork.activitiesRepository.Remove(unitOfWork.activitiesRepository.FindById(id));
        }

        public void Update(ActivityDTO activityDTO)
        {
            Activity activity = mappingConfigs.DTOToActivityMapper().Map<Activity>(activityDTO);
            List<Room> rooms = new List<Room>();
            if (activityDTO.IncludedRooms != null)
            {
                for (int i = 0; i < activityDTO.IncludedRooms.Count; i++)
                {
                    rooms.Add(unitOfWork.roomsRepository.FindById(activityDTO.IncludedRooms[i].Id));
                }
                activity.PosibleRooms = rooms;
            }

            int searchedId = activityDTO.Id;
            Activity ExistingActivity = unitOfWork.activitiesRepository.FindById(searchedId);

            ExistingActivity.Name = activity.Name;
            ExistingActivity.PricePerHour = activity.PricePerHour;
            ExistingActivity.IsSpecialActivity = activity.IsSpecialActivity;
            ExistingActivity.PosibleRooms = activity.PosibleRooms;
            ExistingActivity.Description = activity.Description;
            unitOfWork.activitiesRepository.Update(ExistingActivity);
        }
    }
}
