using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FishingForum.Areas.Identity.Data;

// Add profile data for application users by adding properties to the FishingForumUser class
public class FishingForumUser : IdentityUser
{
    [PersonalData]
    public string Alias { get; set; }
}

