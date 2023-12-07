using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LocFarma.Areas.Identity.Data;

// Add profile data for application users by adding properties to the LocFarmaUser class
public class LocFarmaUser : IdentityUser
{
    public string? Nome { get; internal set; }
}

