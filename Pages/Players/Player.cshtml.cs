using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;

namespace League.Pages.Players
{
    public class PlayerModel : PageModel
    {
        private readonly LeagueContext _context;

        public PlayerModel(LeagueContext context)
        {
            _context = context;
        }

        public Player Player { get; set; }

        public async Task OnGetAsync(string Id)
        {
            Player = await _context.Players.FindAsync(Id);
        }
    }
}