using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PingPong.Models;



namespace PingPong.Controllers
{
    //Controller for managing the display of Player information
    public class DetailsController : Controller
    {
        //DBContext
        private readonly PingPongContext detailsPlayerContext;

        public DetailsController(PingPongContext context)
        {
            detailsPlayerContext = context;
        }

        // Main Details page 
        public IActionResult Index()
        {
            return View("Details");
        }


        // POST: Details/Create
        // Saves the new player information to the database. Called the Add page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Age,SkillLevel,Email")] Player player)
        {

        
            //Validate the information, and save only if valid
            if (ModelState.IsValid)
            {
                detailsPlayerContext.Add(player);
                await detailsPlayerContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Home");
            }
            //Information not valid, return to Add Player page
            return View(player);
        }

        //Lookup player and redirect to edit details page
        public async Task<IActionResult> EditLookup(string searchString, string button)
        {

            
            if (string.IsNullOrWhiteSpace(searchString) || !IsEmailValid(searchString))
            {
                return View("InvalidEmail");
            }
            //locate existing player  for editing
            var player = await detailsPlayerContext.Player.FirstOrDefaultAsync(i => i.Email == searchString);
            if (player == null)
            {
                ViewData["Email"] = searchString;
                return View("PlayerNotFound");
            }

            return View("Edit", player);
           
          
        }

        //Navigate to the Add Player page
        public IActionResult Add()
        {
            return View("Add");
        }

        //navigate to Add  player page with a pre-supplied email
        public IActionResult AddNew(string searchString, string button)
        {
            if (string.IsNullOrWhiteSpace(searchString) || !IsEmailValid(searchString))
            {
                return View("InvalidEmail");
            }
            ViewData["Email"] = searchString;
                return View("Add");


        }

        // Go to edit details for specified player
        //Triggered by edit hyperlink in table on home page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await detailsPlayerContext.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View("Edit", player);
        }

        // POST: Save modifications
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Age,SkillLevel,Email")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    detailsPlayerContext.Update(player);
                    await detailsPlayerContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerIdExists(player.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Go back to Home screen
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(player);
        }

        //Method to check if player with specified id exists
        private bool PlayerIdExists(int id)
        {
            return detailsPlayerContext.Player.Any(e => e.ID == id);
        }

        //Method to validate the uniqueness of the email supplied
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(string email)
        {
            //check if email already exists in the database
           bool exists =  detailsPlayerContext.Player.Any(e => e.Email == email);
            if (exists)
            {
                //Return error message
                return Json($"Email {email} is already in use.");
            }
            //Email is unique, return true
            return Json(true);
        }

        //Validate an email for searching/adding
        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
