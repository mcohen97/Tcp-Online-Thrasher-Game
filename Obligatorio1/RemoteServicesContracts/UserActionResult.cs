using System;
using UserCRUDService;

namespace ActionResults
{
    [Serializable]
    public class UserActionResult:ActionResult
    {
        public UserDto User { get; set; }
    }
}
