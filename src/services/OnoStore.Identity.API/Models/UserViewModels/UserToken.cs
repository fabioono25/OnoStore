using System.Collections.Generic;

namespace OnoStore.Identity.API.Models.UserViewModels
{

    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }
}
