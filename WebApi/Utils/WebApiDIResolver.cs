using BusinessLogic.BL_Repositories;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Utils
{
    public class WebApiDIResolver
    {
        private static IKernel NinjectResolver;
        static WebApiDIResolver()
        {
            NinjectResolver = new StandardKernel();
            NinjectResolver.Bind<IBusinessLogicRepository<ActivityDTO>>().To<ActivitiesBLRepository>();
            NinjectResolver.Bind<IBusinessLogicRepository<OrderDTO>>().To<OrdersBLRepository>();
            NinjectResolver.Bind<IBusinessLogicRepository<EquipmentDTO>>().To<EquipmentBLRepository>();
            NinjectResolver.Bind<IBusinessLogicRepository<RoomDTO>>().To<RoomsBLRepository>();
            NinjectResolver.Bind<IViewModelMappingConfigsGenerator>().To<ViewModelMappingConfigsGenerator>();
           
            NinjectResolver.Bind<IActivityService>().To<ActivityService>();
            NinjectResolver.Bind<IOrderService>().To<OrderService>();
            NinjectResolver.Bind<IRoomService>().To<RoomService>();

        }

        public static object Get { get; set; }

        public static IKernel GetDIResolver()
        {
            if (NinjectResolver == null)
                NinjectResolver = new StandardKernel();
            return NinjectResolver;
        }
    }
}