using Microsoft.AspNetCore.Mvc;
using MexicanoBackend.Data;
using MexicanoBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;




namespace MexicanoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentContext _context;

        public TournamentsController(TournamentContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
        {
            var tournaments = await _context.Tournaments
                .Include(t => t.Players)
                .Include(t => t.Matches)
                .ToListAsync();

            var tournamentDtos = tournaments.Select(t => new TournamentDto(t)).ToList();

            return Ok(tournamentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentById(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Players)
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(new TournamentDto(tournament));
        }

        [HttpPost]
        public async Task<ActionResult<TournamentDto>> CreateTournament(TournamentDto tournamentDto)
        {
            var tournament = new Tournament
            {
                Name = tournamentDto.Name,
                NumberOfPlayers = tournamentDto.NumberOfPlayers,
                CurrentRound = tournamentDto.CurrentRound,
                Players = tournamentDto.Players.Select(p => new Player
                {
                    Name = p.Name,
                    Score = p.Score
                }).ToList(),
                Matches = new List<Match>()
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTournamentById), new { id = tournament.Id }, new TournamentDto(tournament));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
                return BadRequest("Tournament ID mismatch");

            var existingTournament = await _context.Tournaments
                .Include(t => t.Players)
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTournament == null)
                return NotFound($"Tournament with ID {id} not found");

            // Update tournament properties
            existingTournament.Name = tournamentDto.Name;
            existingTournament.NumberOfPlayers = tournamentDto.NumberOfPlayers;
            existingTournament.CurrentRound = tournamentDto.CurrentRound;

            // Update players
            existingTournament.Players.Clear();
            foreach (var playerDto in tournamentDto.Players)
            {
                existingTournament.Players.Add(new Player
                {
                    Name = playerDto.Name,
                    Score = playerDto.Score
                });
            }

            // Update matches
            existingTournament.Matches.Clear();
            foreach (var matchDto in tournamentDto.Matches)
            {
                var existingMatch = existingTournament.Matches.FirstOrDefault(m => m.Id == matchDto.Id);

                if (existingMatch != null)
                {
                    existingMatch.Team1 = matchDto.Team1 ?? new List<int>();
                    existingMatch.Team2 = matchDto.Team2 ?? new List<int>();
                    existingMatch.Team1Score = matchDto.Team1Score;
                    existingMatch.Team2Score = matchDto.Team2Score;
                    existingMatch.IsScoreSubmitted = matchDto.IsScoreSubmitted;
                }
                else
                {
                    existingTournament.Matches.Add(new Match
                    {
                        Team1 = matchDto.Team1 ?? new List<int>(),
                        Team2 = matchDto.Team2 ?? new List<int>(),
                        Team1Score = matchDto.Team1Score,
                        Team2Score = matchDto.Team2Score,
                        IsScoreSubmitted = matchDto.IsScoreSubmitted
                    });
                }
            }


            // Save changes
            await _context.SaveChangesAsync();

            return Ok(new TournamentDto(existingTournament));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

