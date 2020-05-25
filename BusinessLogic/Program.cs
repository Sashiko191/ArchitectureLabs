using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DB_Layer.Models;

namespace BusinessLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            Console.WriteLine("WELCOME TO THE ANTICAFE");
            Console.WriteLine("Our Activities. Choose from list: ");
            var activities = unitOfWork.activitiesRepository.GetAll();
            for (int i = 0; i < activities.Count; i++)
            {
                Console.WriteLine(i + ") " + activities[i].Name + ". " + activities[i].Description);
                if (i == activities.Count - 1)
                    Console.WriteLine(i + 1 + ") Special. Create special event");
            }

            Console.WriteLine("Enter the number of activity you are interested in: ");
            int activity_number = Convert.ToInt32(Console.ReadLine());
            Console.Read();
        }

    }
}
