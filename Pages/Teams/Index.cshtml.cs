using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace League.Pages.Teams
{
    public class IndexModel : PageModel
    {
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        public List<Conference> Conferences { get; set; }

        public List<Division> Divisions { get; set; }

        public List<Team> Teams { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FavoriteTeam { get; set; }

        public SelectList AllTeams { get; set; }

        public async Task OnGetAsync()
        {
            Conferences = await _context.Conferences.ToListAsync();
            Divisions = await _context.Divisions.ToListAsync();
            Teams = await _context.Teams.ToListAsync();

            IQueryable<string> teamQuery = from t in _context.Teams
                                           orderby t.TeamId
                                           select t.TeamId;

            AllTeams = new SelectList(await teamQuery.ToListAsync());

            if(FavoriteTeam != null)
            {
                HttpContext.Session.SetString("_Favorite", FavoriteTeam);
            }
            else
            {
                FavoriteTeam = HttpContext.Session.GetString("_Favorite");
            }
        }

        public List<Division> GetConferenceDivisions(string ConferenceId)
        {
            return Divisions.Where(d => d.ConferenceId.Equals(ConferenceId)).OrderBy(d => d.Name).ToList();
        }

        public List<Team> GetDivisionTeams(string DivisioId)
        {
            return Teams.Where(t => t.DivisionId.Equals(DivisioId)).OrderByDescending(t => t.Win).ToList();
        }
    }
}