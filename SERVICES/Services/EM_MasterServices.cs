using DATA;
using DATA.Interfaces;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SERVICES.Services
{
    public class EM_MasterServices : IEM_Master
    {
        private readonly ContextClass _context;

        public EM_MasterServices(ContextClass context)
        {
            _context = context;
        }

        public void Add(EM_Master eM_Master)
        {
            _context.eM_Masters.Add(eM_Master);
            _context.SaveChanges();
        }

        public void Delete(EM_Master eM_Master)
        {
            _context.eM_Masters.Remove(eM_Master);
            _context.SaveChanges();
        }

        public List<EM_Master> getAllEM_MAster()
        {
            return _context.eM_Masters.Where(x => x.ISactive == true).OrderBy(x => x.Id).ToList();
        }

        public EM_Master GetEMbyId(int EM_Id)
        {
            return _context.eM_Masters.Where(x => x.Id == EM_Id).FirstOrDefault();
        }

        public EM_Master GetEMbyName(string EM_Name, int Id)
        {
            if (Id != 0)
            {
                return _context.eM_Masters.Where(x => x.EM_Name == EM_Name && x.Id != Id).FirstOrDefault();
            }
            else
            {
                return _context.eM_Masters.Where(x => x.EM_Name == EM_Name).FirstOrDefault();
            }

        }

        public EM_Master GetEMbyIP(string IP, int Id)
        {
            if (Id != 0)
            {
                return _context.eM_Masters.Where(x => x.IP == IP && x.Id != Id).FirstOrDefault();
            }
            else
            {
                return _context.eM_Masters.Where(x => x.IP == IP).FirstOrDefault();
            }

        }

        public void Update(EM_Master eM_Master)
        {
            _context.eM_Masters.Update(eM_Master);
            _context.SaveChanges();
        }
    }
}
