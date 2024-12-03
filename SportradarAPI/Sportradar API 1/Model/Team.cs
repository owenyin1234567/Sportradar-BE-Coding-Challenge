using System.ComponentModel.DataAnnotations;

namespace Sportradar_API.Model
{
    public class Team
    {
        public string Slug { get; set; }
        public string? Name { get; set; }
        public string? OfficialName { get; set; }
        public string? Abbreviation { get; set; }
        public Country? Country { get; set; }

        public Team(string slug, string? name, string? officialName, string? abbreviation, Country? country)
        {
            Slug = slug;
            Name = name;
            OfficialName = officialName;
            Abbreviation = abbreviation;
            Country = country;
        }

        public Team(string slug)
        {
            Slug = slug;
        }
    }
}
