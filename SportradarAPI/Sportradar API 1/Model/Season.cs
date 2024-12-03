using System.ComponentModel.DataAnnotations;

namespace Sportradar_API.Model
{
    public class Season
    {
        [Key]
        public int ID { get; set; }
        public DateTime Year { get; set; }
        public Championship? Championship { get; set; }
        public Team? Season_Winner { get; set; }
        public Season(int iD, DateTime year, Championship? championship, Team? season_Winner)
        {
            ID = iD;
            Year = year;
            Championship = championship;
            Season_Winner = season_Winner;
        }
    }
}
