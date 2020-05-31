using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Ninject;
using BusinessLogic.Services;
using PresentationLayer.Services;

namespace PresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebApiDisplayer webApiDisplayer = new WebApiDisplayer();
            var activities = webApiDisplayer.ShowActivities();
            var chosenActivity = webApiDisplayer.ChooseActivity(activities);
            var desiredDate = webApiDisplayer.ChooseDate();
            webApiDisplayer.MakeOrder(chosenActivity, desiredDate);

            Console.ReadLine();
        }
    }
}
