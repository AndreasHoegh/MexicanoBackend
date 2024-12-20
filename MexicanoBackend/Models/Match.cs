namespace MexicanoBackend.Models
{
    public class Match
    {
        public int Id { get; set; }
        public List<int> Team1 { get; set; } // PlayerIds for team1
        public List<int> Team2 { get; set; } // PlayerIds for team2
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public bool IsScoreSubmitted { get; set; }
    }

}
