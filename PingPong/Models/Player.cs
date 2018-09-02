using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PingPong.Models
{
    //Model for the 'Player' table
    public class Player
    {

        public int ID { get; set; }
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "First Name is required")]
        public string LastName { get; set; }
        public int? Age { get; set; }
        [Required(ErrorMessage = "Skill Level is required")]
        [Display(Name = "Skill")]
        public string SkillLevel { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote(action: "VerifyEmail", controller: "Details")]
        public string Email { get; set; }
    }
    
    //Enum used for displaying dropdown list options for the Skill field
    //Provides a centralized location for all screens. Ideally this will later be moved into a db table for easy updates
    public enum Skill
    {
        beginner,
        intermediate,
        advanced,
        expert
    }
}
