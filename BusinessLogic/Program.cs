using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DB_Layer.Models;
using DB_Layer.Repositories;

namespace BusinessLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            OrderScheduler orderScheduler = new OrderScheduler(unitOfWork);

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

            Console.WriteLine("Enter desired date. Format [Date.Month.Year]. Example: 12.03.2019 ");
            string DesiredDateString = Console.ReadLine();
            string[] splitedDate = DesiredDateString.Split('.');
            DateTime DesiredDateTime = new DateTime(Convert.ToInt32(splitedDate[2]), Convert.ToInt32(splitedDate[1]), Convert.ToInt32(splitedDate[0]));
            Console.Clear();

            var PsblRooms = interestedActivity.PosibleRooms.ToList();
            for (int i = 0; i < PsblRooms.Count; i++)
            {
                Console.WriteLine("Room ID: " + PsblRooms[i].Id + " Room Name: " + PsblRooms[i].Name);
                var posibleOrders = orderScheduler.GetActiveOrders(DesiredDateTime, PsblRooms[i]);
                for (int j = 0; j < posibleOrders.Count; j++)
                {
                    Console.WriteLine("  OrderID: " + posibleOrders[j].Id + "  StartTime: " + posibleOrders[j].StartDate.ToString("HH:mm") + " EndTime: " + posibleOrders[j].StartDate.AddHours(posibleOrders[j].Hours).ToString("HH:mm"));
                }
            }
            Console.WriteLine("\nEnter Rooms ID, Start Hour and Hours to spend.\nFormat[RoomId1,RoomId2.StartHour;AmountOfHoursToSpend].\nExample: 7,9.15;3 - Order rooms with id 7 and 9. Beginning at 15:00. Lasting 3 hours.\nDon't forget we work between 08:00 - 00:00 every day)\n");

            string[] IdsStartTimeHours = Console.ReadLine().Split(';');
            int Hours = Convert.ToInt32(IdsStartTimeHours[1]);
            string[] IdsStartTime = IdsStartTimeHours[0].Split('.');
            int StartHour = Convert.ToInt32(IdsStartTime[1]);
            string[] RoomIdStrings = IdsStartTime[0].Split(',');

            Console.WriteLine("Enter your name: ");
            string clientName = Console.ReadLine();
            Console.WriteLine("Enter your surname: ");
            string clientSurname = Console.ReadLine();

            DateTime FinaDateTime = DesiredDateTime;
            FinaDateTime = FinaDateTime.AddHours(StartHour);

            List<Room> UserOrderedRoom = new List<Room>();
            for (int i = 0; i < RoomIdStrings.Length; i++)
            {
                int curId = Convert.ToInt32(RoomIdStrings[i]);
                Room room = unitOfWork.roomsRepository.GetT(curId);
                if (room != null)
                    UserOrderedRoom.Add(room);
            }

            if (!orderScheduler.HasIntersection(FinaDateTime, Hours, UserOrderedRoom))
            {
                Order newOrder = new Order() { Name = clientName, Surname = clientSurname, Hours = Hours, StartDate = FinaDateTime, OrderedRooms = UserOrderedRoom };
                unitOfWork.ordersRepository.Create(newOrder);
                if (interestedActivity.PricePerHour != 0)
                    Console.WriteLine("Your order was successfully added. You have to pay " + interestedActivity.PricePerHour * Hours * UserOrderedRoom.Count + ". Waiting for you at agreed date");
                else
                    Console.WriteLine("Your order was succesfully added. Our manager will call you to talk about the price.");
            }
            else
                Console.WriteLine("Can't set this time");

            unitOfWork.Dispose();
            Console.ReadLine();
        }

    }
}
