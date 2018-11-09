using System.Collections.Generic;
using UserCRUDService;

namespace ActionResults
{
    public class UserListActionResult: ActionResult
    {
        public ICollection<UserDto> UsersList { get; set; }
    }
}
