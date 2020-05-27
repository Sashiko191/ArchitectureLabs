using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Services;
using BusinessLogic.Interfaces;
using Ninject;
using BusinessLogic.Services;

namespace PresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IActivityService>().To<ActivityService>();
            ninjectKernel.Bind<IOrderService>().To<OrderService>();
            ninjectKernel.Bind<IRoomService>().To<RoomService>();


            UIDisplayer uIDisplayer = new UIDisplayer();
            var lst = uIDisplayer.ShowActivities();
            ActivityDTO activityDTO = uIDisplayer.ChooseActivity(lst);
            DateTime DesiredDateTime = uIDisplayer.ChooseDate();
            uIDisplayer.MakeOrder(activityDTO, DesiredDateTime);
            Console.ReadLine();
        }
    }
}
