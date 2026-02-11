using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Domain.Entites
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; } = "";
        public string PasswordHash { get; private set; } = "";
        public string PasswordSalt { get; private set; } = "";

        public string Role { get; private set; } = "User";

        protected User() { }

        public User(string username, string passwordHash, string passwordSalt, string role = "User")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            Username = username.Trim();
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role.Trim();
        }
    }
}
