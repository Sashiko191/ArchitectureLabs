using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;

namespace PresentationLayer.Services
{
    public class UIDisplayer
    {
        private IActivityService activityService;
        private IOrderService orderService;
        private IRoomService roomService;
        public UIDisplayer()
        {
            activityService = new ActivityService();
            orderService = new OrderService();
            roomService = new RoomService();
        }
        public List<ActivityDTO> ShowActivities()
        {
            Console.WriteLine("WELCOME TO THE ANTICAFE");
            Console.WriteLine("Our Activities. Choose from list: ");
            var activities = activityService.GetAllActivities();
            for (int i = 0; i < activities.Count; i++)
            {
                Console.WriteLine(i + ") " + activities[i].Name + ". " + activities[i].Description);
                if (i == activities.Count - 1)
                    Console.WriteLine(i + 1 + ") Special. Create special event");
            }
            return activities;
        }
        public ActivityDTO ChooseActivity(List<ActivityDTO> activities)
        {
            Console.WriteLine("Enter the number of activity you are interested in: ");
            int activity_number = Convert.ToInt32(Console.ReadLine());
            ActivityDTO interestedActivity;
            if (activity_number == activities.Count)
            {
                Console.WriteLine("Enter name of the special activity");
                string special_activity_name = Console.ReadLine();
                Console.WriteLine("Enter description of the special activity");
                string special_activity_description = Console.ReadLine();

                List<RoomDTO> AllRooms = roomService.GetAllRooms();
                for (int i = 0; i < AllRooms.Count; i++)
                {
                    Console.Write("RoomID: " + AllRooms[i].Id + " Name: " + AllRooms[i].Name + " Equipment: ");
                    List<EquipmentDTO> equipmentList = AllRooms[i].GetRoomEquipment();
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
                List<RoomDTO> SpecialRooms = new List<RoomDTO>();
                for (int i = 0; i < RoomIds.Length; i++)
                {
                    RoomDTO r = roomService.GetRoom(Convert.ToInt32(RoomIds[i])); //unitOfWork.roomsRepository.GetT();
                    SpecialRooms.Add(r);
                }

                interestedActivity = new ActivityDTO() { Name = special_activity_name, Description = special_activity_description, IsSpecialActivity = true };
                interestedActivity = activityService.CreateNewActivity(interestedActivity, SpecialRooms);
            }
            else
            {
                interestedActivity = activities[activity_number];
            }
            Console.Clear();
            return interestedActivity;
        }
        public DateTime ChooseDate()
        {
            Console.WriteLine("Enter desired date. Format [Date.Month.Year]. Example: 12.03.2019 ");
            string DesiredDateString = Console.ReadLine();
            string[] splitedDate = DesiredDateString.Split('.');
            DateTime DesiredDateTime = new DateTime(Convert.ToInt32(splitedDate[2]), Convert.ToInt32(splitedDate[1]), Convert.ToInt32(splitedDate[0]));
            Console.Clear();
            return DesiredDateTime;
        }
        public void MakeOrder(ActivityDTO interestedActivity, DateTime DesiredDateTime)
        {
            var PsblRooms = interestedActivity.GetPosibleRooms();
            for (int i = 0; i < PsblRooms.Count; i++)
            {
                Console.WriteLine("Room ID: " + PsblRooms[i].Id + " Room Name: " + PsblRooms[i].Name);
                var posibleOrders = orderService.GetActiveOrders(DesiredDateTime, PsblRooms[i]);
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

            List<RoomDTO> UserOrderedRoom = new List<RoomDTO>();
            for (int i = 0; i < RoomIdStrings.Length; i++)
            {
                int curId = Convert.ToInt32(RoomIdStrings[i]);
                RoomDTO room = roomService.GetRoom(curId);
                if (room != null)
                    UserOrderedRoom.Add(room);
            }

            if (!orderService.HasIntersection(FinaDateTime, Hours, UserOrderedRoom))
            {
                OrderDTO newOrder = new OrderDTO() { Name = clientName, Surname = clientSurname, Hours = Hours, StartDate = FinaDateTime };
                orderService.CreateNewOrder(newOrder, UserOrderedRoom);

                if (interestedActivity.PricePerHour != 0)
                    Console.WriteLine("Your order was successfully added. You have to pay " + interestedActivity.PricePerHour * Hours * UserOrderedRoom.Count + ". Waiting for you at agreed date");
                else
                    Console.WriteLine("Your order was succesfully added. Our manager will call you to talk about the price.");
            }
            else
                Console.WriteLine("Can't set this time");

            Console.ReadLine();
        }
    }
}
