using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberEcommerceSite.Data.Entities
{
    public class User: IdentityUser<Guid> { }

    public class Role : IdentityRole<Guid> { }
}
