using TesteBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TesteBackend.Domain.Entities.UsersPokemons;

namespace TesteBackend.Domain.Entities.Users
{
    public class User : IdentityUser<int>, IEntity<int>
    {
        public User()
        {
            IsActive = true;
        }

        public string FullName { get; set; }

        public int Age { get; set; }

        public GenderType Gender { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoginDate { get; set; }
        
        public ICollection<UserPokemon> UserPokemons { get; set; }
    }

    public enum GenderType
    {
        [Display(Name = "Homem")]
        Male = 1,

        [Display(Name = "Mulher")]
        Female = 2
    }
}
