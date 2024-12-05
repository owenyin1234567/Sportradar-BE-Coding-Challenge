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
            //used by Homepage.html
            try
            {
                var Game = new Game();
                using (MySqlConnection connection = new MySqlConnection(
                    _configuration.GetConnectionString("Default")))
                {
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
                }                
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

                using (MySqlConnection connection = new MySqlConnection(
                    _configuration.GetConnectionString("Default")))
                {
                    connection.Open();
                    using var command = new MySqlCommand("SELECT Game.ID, Game.Dateofevent , Game.StartingTime , Game.EndTime,  " +
                        "HomeTeam.Slug , AwayTeam.Slug , Sport.ID , " +
                        "HomeTeam.name , AwayTeam.name , Sport.name   " +
                        "FROM Game " +
                        "INNER JOIN Sport ON Game.Sport_Game_FK = Sport.ID " +
                        "INNER JOIN Team AS HomeTeam ON Game.HomeTeam_FK = HomeTeam.Slug " +
                        "INNER JOIN Team AS AwayTeam ON Game.AwayTeam_FK = AwayTeam.Slug ", connection);
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
                }
                return ListGames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Game>();
            }
        }

        [HttpPost("AddGame")]
        public bool AddGame(DateOnly dateofevent, TimeOnly? startingTime, TimeOnly? endTime, string? hometeamslug, string? awayteamslug, int? seasonid, int? resultid, int? sportid)
        {   //Adds Game by insert into mysql table
            //returns true if sucessfull else returns false
            //not success returns false
            try
            {


                using (MySqlConnection connection = new MySqlConnection(
                    _configuration.GetConnectionString("Default")))
                {
                    var command = new MySqlCommand("INSERT INTO Game (Dateofevent, StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK, Season_FK, Result_FK, Sport_Game_FK) VALUES (@dateofevent, @startingTime, @endTime, @hometeamslug, @awayteamslug, @seasonid, @resultid, @sportid);", connection);
                    connection.Open();

                    //Parameter Prepare
                    command.Parameters.AddWithValue("@dateofevent", dateofevent.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@startingTime", startingTime);
                    command.Parameters.AddWithValue("@endTime", endTime);
                    command.Parameters.AddWithValue("@hometeamslug", hometeamslug);
                    command.Parameters.AddWithValue("@awayteamslug", awayteamslug);
                    command.Parameters.AddWithValue("@seasonid", seasonid);
                    command.Parameters.AddWithValue("@resultid", resultid);
                    command.Parameters.AddWithValue("@sportid", sportid);

                    command.Prepare();
                    var rowsaffected = command.ExecuteNonQuery();
                    if (rowsaffected >= 1) { return true; }
                    else { return false; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }

        }

        [HttpPost("AddResult")]
        public bool AddResult(int? homegoals, int? awaygoals, string? winner)
        {    //returns true if sucessfull else returns false
            try
            {
                using (MySqlConnection connection = new MySqlConnection(
    _configuration.GetConnectionString("Default")))
                {
                    using var command = new MySqlCommand("INSERT INTO result (homegoals, awaygoals, winner) VALUES ( @homegoals, @awaygoals, @winner);", connection);
                    connection.Open();

                    //Parameter Prepare
                    if (homegoals == null)
                    {
                        command.Parameters.AddWithValue("@homegoals", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@homegoals", homegoals);
                    }

                    if (awaygoals == null)
                    {
                        command.Parameters.AddWithValue("@awaygoals", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@awaygoals", awaygoals);
                    }

                    if (winner == null)
                    {
                        command.Parameters.AddWithValue("@winner", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@winner", winner);
                    }

                    command.Prepare();
                    var rowsaffected = command.ExecuteNonQuery();
                    if (rowsaffected >= 1) { return true; }
                    else { return false; }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }

        }

    }
}
