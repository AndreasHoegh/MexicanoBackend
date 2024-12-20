using MexicanoBackend.Models;

public class TournamentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfPlayers { get; set; }
    public int CurrentRound { get; set; }
    public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    public List<MatchDto> Matches { get; set; } = new List<MatchDto>();

    public TournamentDto() { }

    public TournamentDto(Tournament tournament)
    {
        Id = tournament.Id;
        Name = tournament.Name;
        NumberOfPlayers = tournament.NumberOfPlayers;
        CurrentRound = tournament.CurrentRound;
        Players = tournament.Players.Select(p => new PlayerDto(p)).ToList();
        Matches = tournament.Matches.Select(m => new MatchDto(m)).ToList();
    }
}

public class PlayerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }

    public PlayerDto() { }

    public PlayerDto(Player player)
    {
        Id = player.Id;
        Name = player.Name;
        Score = player.Score;
    }
}

public class MatchDto
{
    public int Id { get; set; }
    public List<int> Team1 { get; set; } = new List<int>();
    public List<int> Team2 { get; set; } = new List<int>();
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public bool IsScoreSubmitted { get; set; }

    public MatchDto() { }

    public MatchDto(Match match)
    {
        Id = match.Id;
        Team1 = match.Team1;
        Team2 = match.Team2;
        Team1Score = match.Team1Score;
        Team2Score = match.Team2Score;
        IsScoreSubmitted = match.IsScoreSubmitted;
    }
}


