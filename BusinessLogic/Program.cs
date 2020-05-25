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
            Activity interestedActivity;
            if (activity_number == activities.Count)
            {
                Console.WriteLine("Enter name of the special activity");
                string special_activity_name = Console.ReadLine();
                Console.WriteLine("Enter description of the special activity");
                string special_activity_description = Console.ReadLine();

                List<Room> AllRooms = unitOfWork.roomsRepository.GetAll();
                for (int i = 0; i < AllRooms.Count; i++)
                {
                    Console.Write("RoomID: " + AllRooms[i].Id + " Name: " + AllRooms[i].Name + " Equipment: ");
                    List<Equipment> equipmentList = AllRooms[i].RoomEquipment.ToList();
                    for (int j = 0; j < equipmentList.Count; j++)
                    {
                        if (j == equipmentList.Count - 1)
                            Console.Write(equipmentList[j].Name);
                        else
                            Console.Write(equipmentList[j].Name + ", ");
                    }
                    Console.Write("\n");
                }

                Console.WriteLine("\nEnter ID of the rooms your want to order. Separete id with commas. Format[ID1,ID2]. Example 3,6");
                string[] RoomIds = Console.ReadLine().Split(',');
                List<Room> SpecialRooms = new List<Room>();
                for (int i = 0; i < RoomIds.Length; i++)
                {
                    Room r = unitOfWork.roomsRepository.GetT(Convert.ToInt32(RoomIds[i]));
                    SpecialRooms.Add(r);
                }

                interestedActivity = new Activity() { Name = special_activity_name, Description = special_activity_description, IsSpecialActivity = true, PosibleRooms = SpecialRooms };
                unitOfWork.activitiesRepository.Create(interestedActivity);
            }
            else
            {
                interestedActivity = activities[activity_number];
            }
            Console.Clear();
            Console.ReadLine();
        }

    }
}
