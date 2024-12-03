using Microsoft.EntityFrameworkCore;
using Sportradar_API.Model;
using System.ComponentModel.DataAnnotations;

namespace Sportradar_API_1.Model
{
    public class Result
    {
        [Key]
        public int ID { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public Team? Winner { get; set; }

        public Result(int iD, int? homeGoals, int? awayGoals, Team? winner)
        {
            ID = iD;
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            Winner = winner;
        }
    }
}
