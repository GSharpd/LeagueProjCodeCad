using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;

namespace League.Pages
{
  public class IndexModel : PageModel
  {
    private readonly LeagueContext _context;

    public IndexModel(LeagueContext context)
    {
      _context = context;
    }

        public League.Models.League League { get; set; }

    public async Task OnGetAsync()
    {
        League = await _context.Leagues.FirstOrDefaultAsync();
    }
  }
}
