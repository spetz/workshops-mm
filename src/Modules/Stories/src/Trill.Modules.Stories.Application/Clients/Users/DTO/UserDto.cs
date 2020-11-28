using System;

namespace Trill.Modules.Stories.Application.Clients.Users.DTO
{
    internal class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }
    }
}