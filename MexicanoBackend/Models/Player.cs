using System.Text.Json.Serialization;

namespace MexicanoBackend.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int TournamentId { get; set; }
        [JsonIgnore]
        public Tournament Tournament { get; set; }
    }
}
