using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Sportradar_API;
using Sportradar_API.Model;
using Sportradar_API_1.Model;

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
        {
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
                    Console.WriteLine("test" + reader.GetValue(1));
                    Console.WriteLine("test" + reader.GetValue(1).GetType());
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
                Console.WriteLine(ex.Message);
                return new Game();
            }
        }

        [HttpGet("GetAllGames")]
        public List<Game> GetAllGames()
        {
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
                    Console.WriteLine("test" + reader.GetValue(1));
                    Console.WriteLine("test" + reader.GetValue(1).GetType());
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
                Console.WriteLine(ex.Message);
                return new List<Game>();
            }
        }

        [HttpGet("GetGameWithResultandSeasonById")] // implememnt
        public Game GetGameWithResultandSeasonById(int id)
        {
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
                    Console.WriteLine($"test 0 {reader.GetValue(0)} {reader.GetValue(0).GetType()}");
                    Console.WriteLine($"test 1 {reader.GetValue(1)} {reader.GetValue(1).GetType()}");
                    Console.WriteLine($"test 2 {reader.GetValue(2)} {reader.GetValue(2).GetType()}");
                    Console.WriteLine($"test 3 {reader.GetValue(3)} {reader.GetValue(3).GetType()}");
                    Console.WriteLine($"test 4 {reader.GetValue(4)} {reader.GetValue(4).GetType()}");
                    Console.WriteLine($"test 5 {reader.GetValue(5)} {reader.GetValue(5).GetType()}");
                    Console.WriteLine($"test 6 {reader.GetValue(6)} {reader.GetValue(6).GetType()}");
                    Console.WriteLine($"test 7 {reader.GetValue(7)} {reader.GetValue(7).GetType()}");
                    Console.WriteLine($"test 8 {reader.GetValue(8)} {reader.GetValue(8).GetType()}");


                    var DateTime = (DateTime)reader.GetValue(0);
                    var Date = DateOnly.FromDateTime(DateTime);
                    var StartingTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(1));
                    var EndTime = TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(2));
                    var HomeTeamSlug = (string)reader.GetValue(3);
                    var AwayTeamSlug = (string)reader.GetValue(4);
                    var SportID = int.Parse((string)reader.GetValue(5));
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
                Console.WriteLine(ex.Message);
                return new Game();
            }
        }

        [HttpGet("GetAllGamesWithResultandSeason")] //implement
        public List<Game> GetAllGamesWithResultandSeason()
        {
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
                    Console.WriteLine("test" + reader.GetValue(1));
                    Console.WriteLine("test" + reader.GetValue(1).GetType());
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
                Console.WriteLine(ex.Message);
                return new List<Game>();
            }
        }

        [HttpPost("AddGame")] //Test
        public bool AddGame(DateOnly dateofevent, TimeOnly startingTime, TimeOnly endTime, string hometeamslug, string awayteamslug, int seasonid, int sportid)
        {
            try
            {
                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                using var command = new MySqlCommand("INSERT INTO Game (Dateofevent, StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK, Season_FK, Sport_Game_FK) VALUES (@dateofevent, @startingTime, @endTime, @hometeamslug, @awayteamslug, @seasonid,  @sportid);", connection);

                //Parameter Prepare
                MySqlParameter Dateofevent = new MySqlParameter("@dateofevent", MySqlDbType.Date);
                command.Parameters.AddWithValue("@dateofevent", Dateofevent);
                MySqlParameter StartingTime = new MySqlParameter("@startingTime", MySqlDbType.Time);
                command.Parameters.AddWithValue("@startingTime", StartingTime);
                MySqlParameter EndTime = new MySqlParameter("@endTime", MySqlDbType.Time);
                command.Parameters.AddWithValue("@endTime", EndTime);
                MySqlParameter Hometeamslug = new MySqlParameter("@hometeamslug", MySqlDbType.VarChar, 256);
                command.Parameters.AddWithValue("@hometeamslug", Hometeamslug);
                MySqlParameter Awayteamslug = new MySqlParameter("@awayteamslug", MySqlDbType.VarChar, 256);
                command.Parameters.AddWithValue("@awayteamslug", Awayteamslug);
                MySqlParameter Seasonid = new MySqlParameter("@seasonid", MySqlDbType.Time);
                command.Parameters.AddWithValue("@seasonid", Seasonid);
                MySqlParameter Sportid = new MySqlParameter("@sportid", MySqlDbType.Time);
                command.Parameters.AddWithValue("@sportid", Sportid);

                command.Prepare();
                int success = command.ExecuteNonQuery();
                if (success == 0)
                {
                    return false;
                }
                else if (success >= 1)
                {
                    return true;
                }

                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }


    }
}
