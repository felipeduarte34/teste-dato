using TesteBackend.Application.Users.Command.CreateUser;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TesteBackend.Persistence.CommandHandlers.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public CreateUserCommandHandler(UserManager<User> userManager,
                                        RoleManager<Role> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var user = new User
            {
                Age = request.Age,
                FullName = request.FullName,
                Gender = request.Gender,
                UserName = request.UserName,
                Email = request.Email,
                SecurityStamp = request.Password
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (createUserResult.Succeeded)
            {
                var addRoleResult = await _roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    Description = "admin role"
                });

                var assignRoleResult = await _userManager.AddToRoleAsync(user, "Admin");

                return true;
            }
            else
            {
                throw new Exception(createUserResult.Errors.FirstOrDefault().Description);
            }
        }
    }
}
