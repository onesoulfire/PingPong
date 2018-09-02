using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PingPong.Models;

namespace PingPong.Controllers
{
    public class HomeController : Controller
    {

        private readonly PingPongContext homePlayerContext;

        public HomeController(PingPongContext context)
        {
            homePlayerContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View("Home",await homePlayerContext.Player.ToListAsync());
        }
        
       


        // GET: Players/Delete/5
        //Handle delete of the player
        //Will direct to a page for the user to confirm that they wish to remove the player
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Locate the player identified by the ID
            var player = await homePlayerContext.Player
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                //Not found page
                return NotFound();
            }
            //Display page confirming removal of player
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(int id)
        {
            //Locate the player identified by the ID primary key
            var player = await homePlayerContext.Player.FindAsync(id);

            //Remove the player
            homePlayerContext.Player.Remove(player);
            //Save the changes
            await homePlayerContext.SaveChangesAsync();
            //Go back to the main page
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
