using API_Vi_EnegryMeter.ViewModels.EM_Master;
using DATA.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.Controllers.EM_Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class EM_MasterController : ControllerBase
    {
        private readonly IEM_Master _eM_Master;
        private readonly IEM_Details _eM_Details;

        public EM_MasterController(IEM_Master eM_Master, IEM_Details eM_Details)
        {
            _eM_Master = eM_Master;
            _eM_Details = eM_Details;
        }

        [HttpGet]
        [Route("get-MeterMaster-details")]
        public async Task<IActionResult> Index()
        {
            try
            {
                //  var sess = HttpContext.Session.GetString("Username");

                var data =await Task.Run(()=> _eM_Master.getAllEM_MAster());
                var final = data.Select(result => new EM_Master_ViewModel()
                {
                    Id = result.Id,
                    EM_Name = result.EM_Name,
                    EM_Make = result.EM_Make,
                    EM_Model = result.EM_Model,
                    EM_Supplier = result.EM_Supplier,
                    EM_SN = result.EM_SN,
                    IP = result.IP,
                    DOP = result.DOP,
                    ISactive = result.ISactive,
                    CreatedBy=result.CreatedBy,
                    CalibrationDate=result.CalibrationDate
                   
                });

                var model = new EM_Master_List_ViewModel()
                {
                    eM_MastersView = final
                };

                return Ok(new { Status = true, data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        // [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("add-meter-master")]
        public async Task<IActionResult> Create(EM_Master_ViewModel model)
        {
            try
            {
                // ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    string meterName = model.EM_Name.Trim();
                    int meterid = model.Id;
                    string meterIp= model.IP.Trim();

                    bool checkName = await CheckIsNameExists(meterName, meterid);
                    bool checkIP = await CheckIsIpExists(meterIp, meterid);


                    if (!checkName || !checkIP)
                    {
                        return BadRequest(new { status = false, message = $"Meter {meterName} or IP {meterIp} is already in use." });
                    }
                    else
                    {


                        DATA.Models.EM_Master obj = new DATA.Models.EM_Master();
                        obj.EM_Name = model.EM_Name.Trim();
                        obj.EM_Make = model.EM_Make.Trim();
                        obj.EM_Model = model.EM_Model.Trim();
                        obj.EM_Supplier = model.EM_Supplier.Trim();
                        obj.EM_SN = model.EM_SN.Trim();
                        obj.IP = model.IP.Trim();
                        obj.DOP = model.DOP;
                        obj.ISactive = model.ISactive;
                        //obj.CreatedBy = HttpContext.Session.GetString("Username");
                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = null;


                        _eM_Master.Add(obj);
                        //TempData["message"] = "Record Save Successfully.";
                        //return RedirectToAction("index");
                        return Ok(new { status = true, messege = "Record Save Successfully.", data = model });

                    }

                }
                else
                {
                    return StatusCode(500, "ModelState is InValid");
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("get-metermaster-byid")]
        public async Task<IActionResult> Edit(int EM_Id)
        {
            try
            {
                var data =  await Task.Run(() => _eM_Master.GetEMbyId(EM_Id));
                if (data != null)
                {
                    EM_Master_Edit_ViewModel model = new EM_Master_Edit_ViewModel()
                    {
                        Id = data.Id,
                        EM_Name = data.EM_Name,
                        EM_Make = data.EM_Make,
                        EM_Model = data.EM_Model,
                        EM_Supplier = data.EM_Supplier,
                        EM_SN = data.EM_SN,
                        IP = data.IP,
                        DOP = data.DOP,
                        ISactive = data.ISactive,
                        CalibrationDate=data.CalibrationDate
                        
                    };
                    return Ok(new { status = true, data = model });
                }
                else
                {
                    return Ok(new { status = false, data = "Meter by Id not Found" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }

        }

        [HttpPut]
        [Route("save-metermaster-detailsById")]
        public async Task<IActionResult> Edit(EM_Master_Edit_ViewModel model)
        {
            try
            {
                string meterName = model.EM_Name.Trim();
                int meterid = model.Id;
              
                bool checkName = await CheckIsNameExists(meterName, meterid);
               
                if (!checkName)
                {
                    return BadRequest(new { status = false, message = $"Meter {meterName} is already in use." });
                }
                else
                {
                    var data = _eM_Master.GetEMbyId(model.Id);
                    if (data != null)
                    {

                        data.EM_Name = model.EM_Name.Trim();
                        data.EM_Make = model.EM_Make.Trim();
                        data.EM_Model = model.EM_Model.Trim();
                        data.EM_Supplier = model.EM_Supplier.Trim();
                        data.EM_SN = model.EM_SN.Trim();
                        data.IP = model.IP.Trim();
                        data.DOP = model.DOP;
                        data.ISactive = model.ISactive;
                        data.CreatedBy = data.CreatedBy;
                        data.CreatedDate = data.CreatedDate;
                        //data.UpdatedBy = HttpContext.Session.GetString("Username");
                        data.UpdatedBy = model.UpdatedBy;
                        data.UpdatedDate = DateTime.Now;
                        data.CalibrationDate = model.CalibrationDate;
                        _eM_Master.Update(data);
                        //TempData["message"] = "Record Update Successfully.";
                        // return RedirectToAction("index");
                        return Ok(new { status = true, data = "Record Update Successfully" });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "Meter by Id not Found" });
                    }   
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

        }

        [HttpDelete]
        [Route("delete-metermaster")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                int count =await Task.Run(()=> _eM_Details.EmIsExist(Id));
                if (count == 0)
                {
                    var data = _eM_Master.GetEMbyId(Id);
                    if (data != null)
                    {
                        _eM_Master.Delete(data);
                        //TempData["message"] = "Record Deleted Successfully.";
                        return Ok(new { status = true, data = "Record Deleted Successfully" });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "Meter not Found" });
                    }
                }
                else
                {
                    // TempData["message"] = "Something went wrong, meter in use.";
                    return Ok(new { status = false, data = "Something went wrong, meter in use" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

            // return RedirectToAction("index");
        }

        [HttpGet]
        [Route("check-metermaster-IsNameExists")]
        public async Task<IActionResult> IsNameExists(string EM_Name, int Id)
        {
            try
            {
                Id = 0;
                var EM =await Task.Run(()=> _eM_Master.GetEMbyName(EM_Name, Id));

                if (EM == null)
                {
                    return Ok(new { status = true, data = $"Meter {EM_Name} Name Not in Use " });
                }
                else
                {
                    // return Json($"Meter  {EM_Name} is already in use");
                    return Ok(new { status = false, data = $"Meter  {EM_Name} is already in use" });

                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

        }


        [HttpGet]
        [Route("check-metermaster-IsIpExists")]
        public async Task<IActionResult> IsIpExists(string IP, int Id)
        {
            try
            {
                Id = 0;
                var EM = await Task.Run(() => _eM_Master.GetEMbyIP(IP, Id)) ;

                if (EM == null)
                {
                    return Ok(new { status = true, data = $"Meter Ip - {IP}  Not in Use" });
                }
                else
                {
                    //return Json($"Meter Ip - {IP} is already in use");
                    return Ok(new { status = false, data = $"Meter Ip - {IP} is already in use" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

        }



        public async Task<bool> CheckIsNameExists(string EM_Name, int Id)
        {
            try
            {
                var EM = await Task.Run(() => _eM_Master.GetEMbyName(EM_Name, Id));

                if (EM == null)
                {
                    return true;
                }
                else
                {
                    // return Json($"Meter  {EM_Name} is already in use");
                    return false;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public async Task<bool> CheckIsIpExists(string IP, int Id)
        {
            try
            {
                var EM =await Task.Run(()=> _eM_Master.GetEMbyIP(IP, Id));

                if (EM == null)
                {
                    return true;
                }
                else
                {
                    //return Json($"Meter Ip - {IP} is already in use");
                    return false;
                }
            }
            catch (Exception ex)
            {

               throw ex;
            }

        }
    }
}
