using DATA;
using DATA.Interfaces;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SERVICES.Services
{
    public class UserServices : IUser
    {
        private readonly ContextClass _context;

        public UserServices(ContextClass context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }



        public List<User> GetAll()
        {
            return _context.Users.ToList();//Where(x=>x.Isactive==true)
        }

        public User GetUserById(int Id)
        {
            return _context.Users.Where(x => x.Id == Id).SingleOrDefault();
        }

        public User GetUserByName(string name)
        {
            return _context.Users.Where(x => x.UserName == name && x.Isactive == true).SingleOrDefault();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
