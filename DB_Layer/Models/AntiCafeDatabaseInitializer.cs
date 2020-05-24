using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class AntiCafeDatabaseInitializer : CreateDatabaseIfNotExists<AntiCafeDb>
    {
        protected override void Seed(AntiCafeDb db)
        {
            Room table_tennis_1 = new Room() { Id = 1, Name = "TableTennis1" };
            Room table_tennis_2 = new Room() { Id = 2, Name = "TableTennis2" };
            Room table_games_1 = new Room() { Id = 3, Name = "TableGames1" };
            Room table_games_2 = new Room() { Id = 4, Name = "TableGames2" };
            Room karaoke_1 = new Room() { Id = 5, Name = "Karaoke1" };
            Room sport_tv_1 = new Room() { Id = 6, Name = "SportTv1" };
            Room sport_tv_2 = new Room() { Id = 7, Name = "SportTv2" };
            Room console1 = new Room() { Id = 8, Name = "Console1" };
            Room console2 = new Room() { Id = 9, Name = "Console2" };
            Room universal1 = new Room() { Id = 10, Name = "Universal1" };
            Room universal2 = new Room() { Id = 11, Name = "Universal2" };
            Room universal3 = new Room() { Id = 12, Name = "Universal3" };
            Room universal4 = new Room() { Id = 13, Name = "Universal4" };

            db.Rooms.Add(table_tennis_1);
            db.Rooms.Add(table_tennis_2);
            db.Rooms.Add(table_games_1);
            db.Rooms.Add(table_games_2);
            db.Rooms.Add(karaoke_1);
            db.Rooms.Add(sport_tv_1);
            db.Rooms.Add(sport_tv_2);
            db.Rooms.Add(console1);
            db.Rooms.Add(console2);
            db.Rooms.Add(universal1);
            db.Rooms.Add(universal2);
            db.Rooms.Add(universal3);
            db.Rooms.Add(universal4);

            Activity table_tennis = new Activity() { Id = 1, Name = "Table tennis", Description = "Default Table Tennis. Net, unlimited table tennis balls and 2 table tennis rackets included.", IsSpecialActivity = false, PricePerHour = 300 };
            Activity sport_event = new Activity() { Id = 2, Name = "Sport event broadcast", Description = "Watch favourite sport events with your friends.", IsSpecialActivity = false, PricePerHour = 500 };
            Activity game_console = new Activity() { Id = 3, Name = "Game console", Description = "Xbox or PlayStation. A lot of games to choose from.", IsSpecialActivity = false, PricePerHour = 700 };
            Activity karaoke = new Activity() { Id = 4, Name = "Karaoke", Description = "Organize a world-class concert with your friends)", IsSpecialActivity = false, PricePerHour = 400 };
            Activity table_games = new Activity() { Id = 5, Name = "Table games", Description = "Jenga, chess, poker and other games to play with your friends", IsSpecialActivity = false, PricePerHour = 200 };

            db.Activitites.Add(table_tennis);
            db.Activitites.Add(sport_event);
            db.Activitites.Add(game_console);
            db.Activitites.Add(karaoke);
            db.Activitites.Add(table_games);

            Equipment xbox = new Equipment() { Id = 1, Name = "Xbox", Description = "Xbox gaming console. 4 gamepads plus several popular games" };
            Equipment ps4 = new Equipment() { Id = 2, Name = "PS4", Description = "PlayStation4 gaming console. 4 gamepads plus several popular games" };
            Equipment LTC_Karaoke_Star_4 = new Equipment() { Id = 3, Name = "LTC Karaoke Star 4", Description = "Karaoke station plus tv" };
            Equipment Tv = new Equipment() { Id = 4, Name = "TV", Description = "High resolution TV" };
            Equipment table_tennis_equipment = new Equipment() { Id = 5, Name = "TableTennisEquipment", Description = "Table, Net, Rackets, Tennis Balls" };
            Equipment table_games_bundle = new Equipment() { Id = 6, Name = "TableGamesBundle", Description = "Chess,Poker,Jenga" };

            db.CafeEquipment.Add(xbox);
            db.CafeEquipment.Add(ps4);
            db.CafeEquipment.Add(LTC_Karaoke_Star_4);
            db.CafeEquipment.Add(Tv);
            db.CafeEquipment.Add(table_tennis_equipment);
            db.CafeEquipment.Add(table_games_bundle);

            xbox.RoomsThatHaveIt.Add(console1);
            xbox.RoomsThatHaveIt.Add(universal1);
            ps4.RoomsThatHaveIt.Add(console2);
            ps4.RoomsThatHaveIt.Add(universal2);
            LTC_Karaoke_Star_4.RoomsThatHaveIt.Add(karaoke_1);
            LTC_Karaoke_Star_4.RoomsThatHaveIt.Add(universal3);
            LTC_Karaoke_Star_4.RoomsThatHaveIt.Add(universal4);
            Tv.RoomsThatHaveIt.Add(console1);
            Tv.RoomsThatHaveIt.Add(console2);
            Tv.RoomsThatHaveIt.Add(universal1);
            Tv.RoomsThatHaveIt.Add(universal2);
            Tv.RoomsThatHaveIt.Add(universal3);
            Tv.RoomsThatHaveIt.Add(universal4);
            Tv.RoomsThatHaveIt.Add(sport_tv_1);
            Tv.RoomsThatHaveIt.Add(sport_tv_2);
            Tv.RoomsThatHaveIt.Add(karaoke_1);
            table_tennis_equipment.RoomsThatHaveIt.Add(table_tennis_1);
            table_tennis_equipment.RoomsThatHaveIt.Add(table_tennis_2);
            table_games_bundle.RoomsThatHaveIt.Add(table_games_1);
            table_games_bundle.RoomsThatHaveIt.Add(table_games_2);
            table_games_bundle.RoomsThatHaveIt.Add(universal1);
            table_games_bundle.RoomsThatHaveIt.Add(universal2);
            table_games_bundle.RoomsThatHaveIt.Add(universal3);
            table_games_bundle.RoomsThatHaveIt.Add(universal4);

            table_tennis.PosibleRooms.Add(table_tennis_1);
            table_tennis.PosibleRooms.Add(table_tennis_2);
            sport_event.PosibleRooms.Add(sport_tv_1);
            sport_event.PosibleRooms.Add(sport_tv_2);
            sport_event.PosibleRooms.Add(universal1);
            sport_event.PosibleRooms.Add(universal2);
            game_console.PosibleRooms.Add(console1);
            game_console.PosibleRooms.Add(console2);
            game_console.PosibleRooms.Add(universal1);
            game_console.PosibleRooms.Add(universal2);
            karaoke.PosibleRooms.Add(karaoke_1);
            karaoke.PosibleRooms.Add(universal3);
            karaoke.PosibleRooms.Add(universal4);
            table_games.PosibleRooms.Add(table_games_1);
            table_games.PosibleRooms.Add(table_games_2);
            table_games.PosibleRooms.Add(universal1);
            table_games.PosibleRooms.Add(universal2);
            table_games.PosibleRooms.Add(universal3);
            table_games.PosibleRooms.Add(universal4);

            db.SaveChanges();
        }
    }
}
