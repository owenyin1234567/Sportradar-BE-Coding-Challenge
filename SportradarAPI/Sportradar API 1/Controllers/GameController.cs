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

        public Game GetGameById(int id)
        {
            try
            {
                using var connection = new MySqlConnection(_configuration.GetConnectionString("Default"));
                connection.Open();
                MySqlParameter idp = new MySqlParameter("@id", MySqlDbType.Int64);
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();
                using var reader = command.ExecuteReader();
                var Game = new Game();

                while (reader.Read())
                {
                    var Date = DateOnly.Parse((string)reader.GetValue(1));
                    var StartingTime = TimeOnly.Parse((string)reader.GetValue(2));
                    var EndTime = TimeOnly.Parse((string)reader.GetValue(3));
                    var HomeTeam = new Team((string)reader.GetValue(4));
                    var AwayTeam = new Team((string)reader.GetValue(5));

                    return Game = new Game(id, Date, StartingTime, EndTime, HomeTeam, AwayTeam, null, null);
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


    }
}
