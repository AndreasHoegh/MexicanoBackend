namespace MexicanoBackend.Models;
public class Tournament
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int NumberOfPlayers { get; set; }
            public int CurrentRound { get; set; }
            public List<Player> Players { get; set; } = new List<Player>();
            public List<Match> Matches { get; set; } = new List<Match>();

    }

