using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PresentationLayer.WebApiViewModels;

namespace PresentationLayer.Services
{
    public class WebApiDisplayer
    {
        public static string URL = "http://localhost:53805/";
        private HttpClient HttpClient = new HttpClient();
        public List<ActivityViewModel> ShowActivities()
        {
            Console.WriteLine("WELCOME TO THE ANTICAFE");
            Console.WriteLine("Our Activities. Choose from list: ");

            HttpResponseMessage httpResponse = HttpClient.GetAsync(URL + "api/Activities/?withRooms=true&IncludeSpecial=false").Result;
            List<ActivityViewModel> activities = JsonConvert.DeserializeObject<List<ActivityViewModel>>(httpResponse.Content.ReadAsStringAsync().Result);

            for (int i = 0; i < activities.Count; i++)
            {
                Console.WriteLine(i + ") " + activities[i].Name + ". " + activities[i].Description);
                if (i == activities.Count - 1)
                    Console.WriteLine(i + 1 + ") Special. Create special event");
            }
            return activities;
        }
        public ActivityViewModel ChooseActivity(List<ActivityViewModel> activities)
        {
            Console.WriteLine("Enter the number of activity you are interested in: ");
            int activity_number = Convert.ToInt32(Console.ReadLine());
            ActivityViewModel interestedActivity;
            if (activity_number == activities.Count)
            {
                Console.WriteLine("Enter name of the special activity");
                string special_activity_name = Console.ReadLine();
                Console.WriteLine("Enter description of the special activity");
                string special_activity_description = Console.ReadLine();

                HttpResponseMessage httpResponse = HttpClient.GetAsync(URL + "api/Rooms/?WithActivities=false&WithEquipment=true&WithOrders=false").Result;
                List<RoomViewModel> AllRooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(httpResponse.Content.ReadAsStringAsync().Result);

                for (int i = 0; i < AllRooms.Count; i++)
                {
                    Console.Write("RoomID: " + AllRooms[i].Id + " Name: " + AllRooms[i].Name + " Equipment: ");
                    List<EquipmentViewModel> equipmentList = AllRooms[i].RoomEquipment;
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
                List<RoomViewModel> SpecialRooms = new List<RoomViewModel>();
                for (int i = 0; i < RoomIds.Length; i++)
                {
                    RoomViewModel sp_room = AllRooms.Where(r => r.Id == (Convert.ToInt32(RoomIds[i]))).FirstOrDefault();
                    SpecialRooms.Add(sp_room);
                }

                interestedActivity = new ActivityViewModel() { Name = special_activity_name, Description = special_activity_description, IsSpecialActivity = true, PosibleRooms = SpecialRooms };

                var content = new StringContent(JsonConvert.SerializeObject(interestedActivity), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = HttpClient.PostAsync(URL + "api/Activities/", content).Result;
                interestedActivity = JsonConvert.DeserializeObject<ActivityViewModel>(responseMessage.Content.ReadAsStringAsync().Result);
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
        public void MakeOrder(ActivityViewModel interestedActivity, DateTime DesiredDateTime)
        {
            List<RoomViewModel> PosibleRooms = interestedActivity.PosibleRooms;

            OrderSearchViewModel orderSearchViewModel = new OrderSearchViewModel() { DesiredDateTime = DesiredDateTime, RoomViewModels = PosibleRooms };
            var content = new StringContent(JsonConvert.SerializeObject(orderSearchViewModel), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = HttpClient.PostAsync(URL + "Rooms/FillRooms", content).Result;
            PosibleRooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(httpResponse.Content.ReadAsStringAsync().Result);

            for (int i = 0; i < PosibleRooms.Count; i++)
            {
                Console.WriteLine("Room ID: " + PosibleRooms[i].Id + " Room Name: " + PosibleRooms[i].Name);
                if (PosibleRooms[i].RoomOrders != null)
                {
                    var posibleOrders = PosibleRooms[i].RoomOrders;

                    for (int j = 0; j < posibleOrders.Count; j++)
                    {
                        Console.WriteLine("  OrderID: " + posibleOrders[j].Id + "  StartTime: " + posibleOrders[j].StartDate.ToString("HH:mm") + " EndTime: " + posibleOrders[j].StartDate.AddHours(posibleOrders[j].Hours).ToString("HH:mm"));
                    }
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

            List<RoomViewModel> UserOrderedRoom = new List<RoomViewModel>();
            for (int i = 0; i < RoomIdStrings.Length; i++)
            {
                int curId = Convert.ToInt32(RoomIdStrings[i]);
                RoomViewModel room = PosibleRooms.Where(r => r.Id == curId).FirstOrDefault();
                if (room != null)
                    UserOrderedRoom.Add(room);
            }

            OrderViewModel newOrder = new OrderViewModel() { Name = clientName, Surname = clientSurname, Hours = Hours, StartDate = FinaDateTime, OrderedRooms = UserOrderedRoom };
            var content2 = new StringContent(JsonConvert.SerializeObject(newOrder), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = HttpClient.PostAsync(URL + "api/Orders/", content2).Result;
            OrderViewModel CreatedOrder = JsonConvert.DeserializeObject<OrderViewModel>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            if (CreatedOrder != null)
            {
                if (interestedActivity.PricePerHour != 0)
                    Console.WriteLine($"Your order was successfully added. Order Id: {CreatedOrder.Id} You have to pay " + interestedActivity.PricePerHour * Hours * UserOrderedRoom.Count + ". Waiting for you at agreed date");
                else
                    Console.WriteLine("Your order was succesfully added. Our manager will call you to talk about the price.");
            }
            else
                Console.WriteLine("Invalid Time Option");
            Console.ReadLine();
        }
    }
}
