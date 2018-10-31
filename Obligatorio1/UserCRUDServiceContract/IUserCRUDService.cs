﻿using System.Collections.Generic;

namespace UserABM
{
    public interface IUserCRUDService
    {
        void AddUser(UserDto userModel);

        void ModifyUser(string oldNickname, UserDto newData);

        void DeleteUser(string nickname);

        UserDto GetUser(string nickname);

        ICollection<UserDto> GetAllUsers();
    }

}
