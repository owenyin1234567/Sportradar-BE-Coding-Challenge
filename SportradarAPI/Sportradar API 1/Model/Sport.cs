using System.ComponentModel.DataAnnotations;

namespace Sportradar_API.Model
{
    public class Sport
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public Sport(int iD, string? name)
        {
            ID = iD;
            Name = name;
        }
    }
}
