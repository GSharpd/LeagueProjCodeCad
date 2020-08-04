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

namespace League.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        public List<Player> Players { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public SelectList Teams { get; set; }

        public SelectList Positions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedTeam { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedPosition { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; }

        public string FavoriteTeam { get; set; }

        public async Task OnGetAsync()
        {
            var players = from p in _context.Players
                          select p;

            if(!string.IsNullOrEmpty(SearchString))
            {
                players = players.Where(p => p.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SelectedTeam))
            {
                players = players.Where(p => p.TeamId == SelectedTeam);
            }

            if (!string.IsNullOrEmpty(SelectedPosition))
            {
                players = players.Where(p => p.Position == SelectedPosition);
            }

            switch(SortField)
            {
                case "Number": players = players.OrderBy(p => p.Number).ThenBy(p => p.TeamId); break;
                case "Name": players = players.OrderBy(p => p.Name).ThenBy(p => p.TeamId); break;
                case "Position": players = players.OrderBy(p => p.Position).ThenBy(p => p.TeamId); break;
            }

            IQueryable<string> teamQuery = from t in _context.Teams
                                           orderby t.TeamId
                                           select t.TeamId;

            Teams = new SelectList(await teamQuery.ToListAsync());

            IQueryable<string> positionQuery = from p in _context.Players
                                               orderby p.Position
                                               select p.Position;

            Positions = new SelectList(await positionQuery.Distinct().ToListAsync());

            FavoriteTeam = HttpContext.Session.GetString("_Favorite");

            Players = await players.ToListAsync();
        }

        public string PlayerClass(Player Player)
        {
            string Class = "d-flex";
            if (Player.Depth == 1)
                Class += " starter";
            if (Player.TeamId == FavoriteTeam)
                Class += " favorite";
            return Class;
        }
    }
}