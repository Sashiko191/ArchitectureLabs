using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using DB_Layer.Repositories;
using Ninject;

namespace BusinessLogic.Services
{
    public class DI_Resolver
    {
        private static IKernel NinjectResolver;

        static DI_Resolver()
        {
            NinjectResolver = new StandardKernel();
            NinjectResolver.Bind<IActivityService>().To<ActivityService>();
            NinjectResolver.Bind<IOrderService>().To<OrderService>();
            NinjectResolver.Bind<IRoomService>().To<RoomService>();
            NinjectResolver.Bind<IMappingConfigsGenerator>().To<MappingConfigsGenerator>();
            NinjectResolver.Bind<IUnitOfWork<AntiCafeDb>>().To<UnitOfWork>();
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
