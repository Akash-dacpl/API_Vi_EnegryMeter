using DATA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA.Interfaces
{
    public interface IUser
    {
        User GetUserByName(string name);
        User GetUserById(int Id);
        List<User> GetAll();
        void Add(User user);
        void Update(User user);


    }
}
