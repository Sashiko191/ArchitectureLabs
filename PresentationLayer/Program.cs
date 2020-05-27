using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Services;

namespace PresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            UIDisplayer uIDisplayer = new UIDisplayer();
            var lst = uIDisplayer.ShowActivities();
            ActivityDTO activityDTO = uIDisplayer.ChooseActivity(lst);
            DateTime DesiredDateTime = uIDisplayer.ChooseDate();
            uIDisplayer.MakeOrder(activityDTO, DesiredDateTime);
            Console.ReadLine();
        }
    }
}
