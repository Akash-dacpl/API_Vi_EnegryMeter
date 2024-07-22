using DATA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA.Interfaces
{
    public interface IEM_Master
    {
        EM_Master GetEMbyId(int EM_Id);
        EM_Master GetEMbyName(string EM_Name, int Id);
        EM_Master GetEMbyIP(string IP, int Id);
        List<EM_Master> getAllEM_MAster();
        void Add(EM_Master eM_Master);
        void Update(EM_Master eM_Master);
        void Delete(EM_Master eM_Master);

    }
}
