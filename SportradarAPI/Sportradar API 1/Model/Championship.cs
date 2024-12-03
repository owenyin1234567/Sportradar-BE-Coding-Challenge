using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportradar_API.Model
{
    public class Championship
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public Sport? Sport { get; set; }

        public Championship(int id, string name, Sport? sport)
        {
            ID = id;
            Name = name;
            Sport = sport;
        }
    }
}
