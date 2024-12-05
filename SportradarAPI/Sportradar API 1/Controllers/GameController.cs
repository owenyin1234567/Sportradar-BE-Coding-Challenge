using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Sportradar_API;
using Sportradar_API.Model;
using Sportradar_API_1.Model;
using System.Data;

namespace Sportradar_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        //private readonly AppDbContext _context;
        public readonly IConfiguration _configuration;

        public GameController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_context = context;
        }

        [HttpGet("GetGameById")]
        public Game GetGameById(int id)
        {   //ignore
            try
            {
                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("select Date(Dateofevent), StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK from Game where ID=@id", connection);
                MySqlParameter idp = new MySqlParameter("@id", MySqlDbType.Int64);
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();
                using var reader = command.ExecuteReader();
                var Game = new Game();
                while (reader.Read())
                {
                    var DateTime = (DateTime)reader.GetValue(0);
                    var Date = DateOnly.FromDateTime(DateTime);
                    var StartingTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(1));
                    var EndTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(2));
                    var HomeTeam = new Team((string)reader.GetValue(3));
                    var AwayTeam = new Team((string)reader.GetValue(4));
                    return Game = new Game(id, Date, StartingTime, EndTime, HomeTeam, AwayTeam, null, null, null);
                }
                connection.Close();
                return Game;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new Game();
            }
        }

        [HttpGet("GetAllGames")]
        public List<Game> GetAllGames()
        {   //ignore
            try
            {
                List<Game> ListGames = new List<Game>();

                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("select * from Game", connection);
                using var reader = command.ExecuteReader();
                var Game = new Game();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var DateTime = reader.GetDateTime(1);
                    var Date = DateOnly.FromDateTime(DateTime);
                    var StartingTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(2));
                    var EndTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(3));
                    var HomeTeam = new Team((string)reader.GetValue(4));
                    var AwayTeam = new Team((string)reader.GetValue(5));
                    Game = new Game(id, Date, StartingTime, EndTime, HomeTeam, AwayTeam, null, null, null);
                    ListGames.Add(Game);
                }
                connection.Close();
                return ListGames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Game>();
            }
        }

        [HttpGet("GetGameWithSportById")] 
        public Game GetGameWithSportById(int id)
        {   //returns single Game with Sport and Season Info by ID
            try
            {
                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("SELECT Game.Dateofevent , Game.StartingTime , Game.EndTime,  " +
                    "HomeTeam.Slug , AwayTeam.Slug , Sport.ID , " +
                    "HomeTeam.name , AwayTeam.name , Sport.name   " +
                    "FROM Game " +
                    "INNER JOIN Sport ON Game.Sport_Game_FK = Sport.ID " +
                    "INNER JOIN Team AS HomeTeam ON Game.HomeTeam_FK = HomeTeam.Slug " +
                    "INNER JOIN Team AS AwayTeam ON Game.AwayTeam_FK = AwayTeam.Slug " +
                    "where Game.ID = @id", connection);
                MySqlParameter idp = new MySqlParameter("@id", MySqlDbType.Int64);
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();
                using var reader = command.ExecuteReader();
                var Game = new Game();
                while (reader.Read())
                {
                    #region cw debug
                    /*
                    Console.WriteLine($"test 0 {reader.GetValue(0)} {reader.GetValue(0).GetType()}");
                    Console.WriteLine($"test 1 {reader.GetValue(1)} {reader.GetValue(1).GetType()}");
                    Console.WriteLine($"test 2 {reader.GetValue(2)} {reader.GetValue(2).GetType()}");
                    Console.WriteLine($"test 3 {reader.GetValue(3)} {reader.GetValue(3).GetType()}");
                    Console.WriteLine($"test 4 {reader.GetValue(4)} {reader.GetValue(4).GetType()}");
                    Console.WriteLine($"test 5 {reader.GetValue(5)} {reader.GetValue(5).GetType()}");
                    Console.WriteLine($"test 6 {reader.GetValue(6)} {reader.GetValue(6).GetType()}");
                    Console.WriteLine($"test 7 {reader.GetValue(7)} {reader.GetValue(7).GetType()}");
                    Console.WriteLine($"test 8 {reader.GetValue(8)} {reader.GetValue(8).GetType()}");*/
                    #endregion 

                    var DateTime = (DateTime)reader.GetValue(0);
                    var Date = DateOnly.FromDateTime(DateTime);
                    var StartingTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(1));
                    var EndTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(2));
                    var HomeTeamSlug = (string)reader.GetValue(3);
                    var AwayTeamSlug = (string)reader.GetValue(4);
                    var SportID = (int)reader.GetValue(5);
                    var HomeTeam = new Team(HomeTeamSlug, (string)reader.GetValue(6), null, null, null);
                    var AwayTeam = new Team(AwayTeamSlug, (string)reader.GetValue(7), null, null, null);
                    var Sport = new Sport(SportID, (string)reader.GetValue(8));

                    return Game = new Game(id, Date, StartingTime, EndTime, HomeTeam, AwayTeam, null, null, Sport);
                }
                connection.Close();
                return Game;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new Game();
            }
        }

        [HttpGet("GetAllGamesWithSport")]
        public List<Game> GetAllGamesWithSport()
        {   //returns List of all Game with Sport and Season Info 
            try
            {
                List<Game> ListGames = new List<Game>();

                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("SELECT Game.ID, Game.Dateofevent , Game.StartingTime , Game.EndTime,  " +
                    "HomeTeam.Slug , AwayTeam.Slug , Sport.ID , " +
                    "HomeTeam.name , AwayTeam.name , Sport.name   " +
                    "FROM Game " +
                    "INNER JOIN Sport ON Game.Sport_Game_FK = Sport.ID " +
                    "INNER JOIN Team AS HomeTeam ON Game.HomeTeam_FK = HomeTeam.Slug " +
                    "INNER JOIN Team AS AwayTeam ON Game.AwayTeam_FK = AwayTeam.Slug ",connection);
                using var reader = command.ExecuteReader();
                var Game = new Game();
                while (reader.Read())
                {
                    var ID = (int)reader.GetValue(0);
                    var DateTime = (DateTime)reader.GetValue(1);
                    var Date = DateOnly.FromDateTime(DateTime);
                    var StartingTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(2));
                    var EndTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(3));
                    var HomeTeamSlug = (string)reader.GetValue(4);
                    var AwayTeamSlug = (string)reader.GetValue(5);
                    var SportID = (int)reader.GetValue(6);
                    var HomeTeam = new Team(HomeTeamSlug, (string)reader.GetValue(7), null, null, null);
                    var AwayTeam = new Team(AwayTeamSlug, (string)reader.GetValue(8), null, null, null);
                    var Sport = new Sport(SportID, (string)reader.GetValue(9));

                    Game = new Game(ID, Date, StartingTime, EndTime, HomeTeam, AwayTeam, null, null, Sport);
                    ListGames.Add(Game);
                }
                connection.Close();
                return ListGames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Game>();
            }
        }

        [HttpPost("AddGame")]
        public bool AddGame(DateOnly dateofevent, TimeOnly startingTime, TimeOnly endTime, string hometeamslug, string awayteamslug, int seasonid, int? resultid, int sportid)
        {   //Adds Game by insert into mysql table
            //returns if success then returns true
            //not success returns false
            try
            {
                /*
                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default")); 
                connection.Open();
                using var command = new MySqlCommand("INSERT INTO Game (Dateofevent, StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK, Season_FK, Result_FK, Sport_Game_FK) VALUES (@dateofevent, @startingTime, @endTime, @hometeamslug, @awayteamslug, @seasonid, @resultid, @sportid);", connection);

                //Parameter Prepare
                command.Parameters.AddWithValue("@dateofevent", dateofevent);
                command.Parameters.AddWithValue("@startingTime", startingTime);
                command.Parameters.AddWithValue("@endTime", endTime);
                command.Parameters.AddWithValue("@hometeamslug", hometeamslug);
                command.Parameters.AddWithValue("@awayteamslug", awayteamslug);
                command.Parameters.AddWithValue("@seasonid", seasonid);
                command.Parameters.AddWithValue("@resultid", resultid);
                command.Parameters.AddWithValue("@sportid", sportid);


                command.Prepare();
                var success = command.ExecuteNonQuery();
                connection.Close();
                if (success >= 1)
                {
                    Console.WriteLine(true);
                    return true;
                }
                else
                {
                    Console.WriteLine(false);
                    return false;
                }*/
                Console.WriteLine("dateofevent.ToString" + dateofevent.ToString("yyyy/MM/dd"));

                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("INSERT INTO Game (Dateofevent, StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK, Season_FK, Result_FK, Sport_Game_FK) VALUES (@dateofevent, @startingTime, @endTime, @hometeamslug, @awayteamslug, @seasonid, @resultid, @sportid);", connection);

                //Parameter Prepare
                Console.WriteLine(dateofevent);
                Console.WriteLine(dateofevent.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@dateofevent", dateofevent.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@startingTime", startingTime);
                command.Parameters.AddWithValue("@endTime", endTime);
                command.Parameters.AddWithValue("@hometeamslug", hometeamslug);
                command.Parameters.AddWithValue("@awayteamslug", awayteamslug);
                command.Parameters.AddWithValue("@seasonid", seasonid);
                command.Parameters.AddWithValue("@resultid", resultid);
                command.Parameters.AddWithValue("@sportid", sportid);

                command.Prepare();
                int success = command.ExecuteNonQuery();
                connection.Close();

                if (success == 0)
                {
                    return false;
                }
                else if (success >= 1)
                {
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }

        }


    }
}
