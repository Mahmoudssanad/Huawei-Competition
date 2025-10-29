
using GreenEye.Domain.Entities;
using GreenEye.Infrasturcture.Identity;

namespace GreenEye.Infrasturcture.Conversion
{
    public static class UserConversion
    {
        public static User ToUser(ApplicationUser user) => new User
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            TelephoneNumber = user.PhoneNumber,
            Address = user.Address
        };

        public static (ApplicationUser?, IEnumerable<ApplicationUser>?) FromUser(User? user, IEnumerable<User> users)
        {
            // return single user
            if(user is not null || users is null)
            {
                var singleUser = new ApplicationUser
                {
                    Id = user!.Id!,
                    UserName = user.Name,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.TelephoneNumber
                };
                return (singleUser, null);
            }

            // return users
            if(user is null || users is not null)
            {
                var _users = users.Select(x => new ApplicationUser
                {
                    Id = user!.Id!,
                    UserName = user.Name,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.TelephoneNumber
                });
                return (null, _users);
            }
            return (null, null);
        }
            
    }
}
