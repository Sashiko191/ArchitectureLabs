using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Services;
using BusinessLogic.BL_Repositories;
using BusinessLogic.Models;
using BusinessLogic.Interfaces;
using WebApi.Utils;
using Ninject;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class ActivitiesController : ApiController
    {
        private IViewModelMappingConfigsGenerator configsGenerator;
        private IBusinessLogicRepository<ActivityDTO> ActivitiesBLRepository;

        public ActivitiesController()
        {
            IKernel webApiDIResolver = WebApiDIResolver.GetDIResolver();
            ActivitiesBLRepository = webApiDIResolver.Get<IBusinessLogicRepository<ActivityDTO>>();
            configsGenerator = webApiDIResolver.Get<IViewModelMappingConfigsGenerator>();
        }

        [HttpGet]
        public List<ActivityViewModel> Get(bool WithRooms,bool IncludeSpecial)
        {
            List<ActivityDTO> activities;
            if (WithRooms)
            {
                if (IncludeSpecial)
                    activities = ActivitiesBLRepository.GetWithEverything();
                else
                    activities = ActivitiesBLRepository.GetWithEverything().Where(a => a.IsSpecialActivity == false).ToList();
            }
            else
            {
                if (IncludeSpecial)
                    activities = ActivitiesBLRepository.Get();
                else
                    activities = ActivitiesBLRepository.GetWithEverything().Where(a => a.IsSpecialActivity == false).ToList();
            }
            List<ActivityViewModel> activityViewModels = configsGenerator.DTOToActivityViewModel().Map<List<ActivityViewModel>>(activities);
            for(int i = 0;i < activityViewModels.Count;i++)
            {
                activityViewModels[i].PosibleRooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(activities[i].IncludedRooms);
            }
            return activityViewModels;
        }

        [HttpGet]
        public ActivityViewModel Get(int id, bool WithRooms)
        {
            ActivityDTO activityDTO;
            ActivityViewModel activityViewModel;
            if (WithRooms)
            {
                activityDTO = ActivitiesBLRepository.FindByIdWithEverything(id);
                List<RoomViewModel> rooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(activityDTO.IncludedRooms);
                activityViewModel = configsGenerator.DTOToActivityViewModel().Map<ActivityViewModel>(activityDTO);
                activityViewModel.PosibleRooms = rooms;
            }
            else
            {
                activityDTO = ActivitiesBLRepository.FindById(id);
                activityViewModel = configsGenerator.DTOToActivityViewModel().Map<ActivityViewModel>(activityDTO);
            }
            return activityViewModel;
        }

        [HttpPost]
        public ActivityViewModel Post([FromBody]ActivityViewModel viewModel)
        {
            ActivityDTO activityDTO = configsGenerator.ViewModelToActivityDTO().Map<ActivityDTO>(viewModel);
            activityDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(viewModel.PosibleRooms);
            ActivityDTO CreatedActivityDTO = ActivitiesBLRepository.Create(activityDTO);
            ActivityViewModel activityViewModel = configsGenerator.DTOToActivityViewModel().Map<ActivityViewModel>(CreatedActivityDTO);
            activityViewModel.PosibleRooms = configsGenerator.DTOToRoomViewModel().Map<List<RoomViewModel>>(CreatedActivityDTO.IncludedRooms);
            return activityViewModel;
        }

        [HttpPut]
        public void Update([FromBody]ActivityViewModel viewModel)
        {
            ActivityDTO activityDTO = configsGenerator.ViewModelToActivityDTO().Map<ActivityDTO>(viewModel);
            activityDTO.IncludedRooms = configsGenerator.ViewModelToRoomDTO().Map<List<RoomDTO>>(viewModel.PosibleRooms);
            ActivitiesBLRepository.Update(activityDTO);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            ActivitiesBLRepository.Delete(ActivitiesBLRepository.FindById(id));
        }        
    }
}
