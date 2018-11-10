using System;
using System.Collections.Generic;
using UserCRUDService;

namespace ActionResults
{
    [Serializable]
    public class UserListActionResult: ActionResult
    {
        public ICollection<UserDto> UsersList { get; set; }
    }
}
