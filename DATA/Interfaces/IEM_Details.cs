using DATA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Interfaces
{
    public interface IEM_Details
    {
        List<EM_Details> getAll();

        //List<EM_Details> GetbyClustrNm(string clstrNm);

        int EmIsExist(int EM_ID);

       Task<List<EM_Details>> getEMbyId(int[] EM_id, DateTime fdt, DateTime tdt);
        Task<List<EM_Details>> getEM_CurrentReading(int[] EM_idd, DateTime currrntDate);
        List<EM_Details> getEMbyDate(DateTime fdt, DateTime tdt);
    }
}
