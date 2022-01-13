using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Shared.Entities.Abstract;

namespace BlogApp.Entities.Concrete
{
    public class User:EntityBase,IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}