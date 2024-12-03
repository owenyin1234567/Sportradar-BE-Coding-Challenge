using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sportradar_API.Model
{
    public class Country
    {
        public string Abbr { get; set; }

        public string? Name { get; set; }

        public Country(string abbr, string? name)
        {
            Abbr = abbr;
            Name = name;
        }
    }
}
