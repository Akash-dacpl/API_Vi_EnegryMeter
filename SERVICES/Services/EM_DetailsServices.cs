using DATA;
using DATA.Interfaces;
using DATA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Services
{
    public class EM_DetailsServices : IEM_Details
    {
        private readonly ContextClass _context;

        public EM_DetailsServices(ContextClass context)
        {
            _context = context;

        }

        public int EmIsExist(int EM_ID)
        {
            return _context.EM_Details.Where(x => x.EM_Id == EM_ID).ToList().Count;
        }

        public List<EM_Details> getAll()
        {
            return _context.EM_Details.Include(x => x.EM_Master).Where(x => x.EM_Master.ISactive == true).ToList();
        }

        public List<EM_Details> getEMbyDate(DateTime fdt, DateTime tdt)
        {
            return _context.EM_Details.Include(x => x.EM_Master).Where(x => x.TS.Date >= fdt.Date && x.TS.Date <= tdt.Date).OrderBy(x => x.EM_Id).ThenBy(x => x.TS_hours).ToList();

        }

        public async Task<List<EM_Details>> getEMbyId(int[] EM_idd, DateTime fdt, DateTime tdt)
        {
            string s = string.Join(",", EM_idd); //String.Concat(EM_id);
            var data = await _context.EM_Details.Include(x => x.EM_Master).Where(x => x.TS.Date >= fdt.Date && x.TS.Date <= tdt.Date && s.Contains(x.EM_Id.ToString())).ToListAsync();
            // var final = data.Where(x => s.Contains(x.EM_Id.ToString())).ToList();//.OrderByDescending(x => x.TS);
            return data;

            //return _context.EM_Details.Include(x => x.EM_Master).Where(x => x.TS.Date >= fdt.Date && x.TS.Date <= tdt.Date && EM_idd.ToString().Contains(x.EM_Id.ToString())).ToList();//.OrderByDescending(x=>x.TS)
            //return data.Where(x => EM_id.ToString().Contains(x.EM_Id.ToString())).ToList();
        }

        public async Task<List<EM_Details>> getEM_CurrentReading(int[] EM_idd,DateTime currrntDate)
        {
            string s = string.Join(",", EM_idd); //String.Concat(EM_id);
            var data = await _context.EM_Details.Include(x => x.EM_Master).Where(x => x.TS.Date == currrntDate.Date && s.Contains(x.EM_Id.ToString())).ToListAsync();
            // var final = data.Where(x => s.Contains(x.EM_Id.ToString())).ToList();//.OrderByDescending(x => x.TS);
            return data;
        }
    }
}
