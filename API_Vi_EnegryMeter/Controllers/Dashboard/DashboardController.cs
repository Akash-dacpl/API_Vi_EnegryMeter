using API_Vi_EnegryMeter.ViewModels;
using DATA.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IEM_Details _eM_Details;

        public DashboardController(IEM_Details eM_Details)
        {
            _eM_Details = eM_Details;
        }
       
        [HttpGet]
        [Route("get-meterReadings-Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data =await Task.Run(()=> _eM_Details.getAll());
                List<EM_DetailsViewModel> EM_lst = new List<EM_DetailsViewModel>();

                var v = data.Select(x => new { x.EM_Id }).Distinct().ToList();
                if (v != null)
                {
                    foreach (var EM in v)
                    {
                        var finalData = data.OrderByDescending(x => x.TS).Where(x => x.EM_Id == EM.EM_Id).FirstOrDefault();
                        if (finalData != null)
                        {
                            var abc = new EM_DetailsViewModel()
                            {
                                EM_Id = finalData.EM_Id,
                                EM_Name = finalData.EM_Master.EM_Name,
                                Sys_Voltage = finalData.Sys_Voltage,
                                Sys_Curr = finalData.Sys_Curr,
                                VL1_L2 = finalData.VL1_L2,
                                VL2_L3 = finalData.VL2_L3,
                                VL3_L1 = finalData.VL3_L1,
                                Frequency = finalData.Frequency,
                                ActivePower = Math.Round(finalData.ActivePower),
                                PowerFactor = finalData.PowerFactor,
                                Act_Imp_Energy = finalData.Act_Imp_Energy,
                                TS = finalData.TS,
                                IsAlive = finalData.EM_Master.IsAlive
                            };

                            EM_lst.Add(abc);

                        }
                        else
                        {
                            return Ok(new { status = false, data = "Meter Data Not Avalible" });
                        }
                    }
                    //ViewData["ClusterName"] = clstrName;
                    //  return View(@"~/Pages/Shared/_EM.cshtml", lstCluster);
                   // ViewData["Heading"] = "DASHBOARD";
                    return Ok(new { status = true, data = EM_lst.OrderBy(x => x.EM_Id) });
                }
                else
                {
                    return Ok(new { status=false , data ="Meter Id Not Avalible"});
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error:{ex.Message}");
            }

           
           
        }

        //public IActionResult EneryMeters(string clstrName)
        //{
        //    //var data = _eM_Details.GetbyClustrNm(clstrName);
        //    //if (data != null)
        //    //{
        //    //    List<ClustersViewModel> lstCluster = new List<ClustersViewModel>();

        //    //    var v = data.Select(x => x.EM_Name).Distinct();
        //    //    if (v != null)
        //    //    {
        //    //        foreach (var EM in v)
        //    //        {
        //    //            var finalData = data.OrderByDescending(x => x.TimeStamp).Where(x => x.EM_Name == EM).FirstOrDefault();
        //    //            if (finalData != null)
        //    //            {

        //    //                var abc = new ClustersViewModel()
        //    //                {
        //    //                    cluster = finalData.Cluster,
        //    //                    EM_Name = finalData.EM_Name,
        //    //                    Current = finalData.Current,
        //    //                    AverageCurrent = finalData.AverageCurrent,
        //    //                    PhaseCurrents = finalData.PhaseCurrents,
        //    //                    UnbalanceCurrent = finalData.UnbalanceCurrent,
        //    //                    Voltage = finalData.Voltage,
        //    //                    AverageVoltage = finalData.AverageVoltage,
        //    //                    UnbalanceVoltage = finalData.UnbalanceVoltage,
        //    //                    Frequency = finalData.Frequency,
        //    //                    PowerFactor = finalData.PowerFactor,
        //    //                    ActivePower = finalData.ActivePower,
        //    //                    ActiveEnergy = finalData.ActiveEnergy,
        //    //                    Percentage_of_load = finalData.Percentage_of_load,
        //    //                    TimeStamp = finalData.TimeStamp.ToString("dd/MM/yyyy hh:mm:ss")

        //    //                };

        //    //                lstCluster.Add(abc);

        //    //            }
        //    //        }
        //    //        ViewData["ClusterName"] = clstrName;
        //    //        //  return View(@"~/Pages/Shared/_EM.cshtml", lstCluster);
        //    //        return View(lstCluster);
        //    //    }


        //    //}
        //   // return View();

        //}
       
        [HttpGet]
        [Route("get-meterReadings-details")]
        public async Task<IActionResult> GetEM()
        {
            try
            {
                var data = await Task.Run(() => _eM_Details.getAll());
                List<EM_DetailsViewModel> EM_lst = new List<EM_DetailsViewModel>();

                var v = data.Select(x => new { x.EM_Id }).Distinct().ToList();
                if (v != null)
                {
                    foreach (var EM in v)
                    {
                        var finalData = data.OrderByDescending(x => x.TS).Where(x => x.EM_Id == EM.EM_Id).FirstOrDefault();
                        if (finalData != null)
                        {
                            var abc = new EM_DetailsViewModel()
                            {
                                EM_Id = finalData.EM_Id,
                                EM_Name = finalData.EM_Master.EM_Name,
                                Sys_Voltage = finalData.Sys_Voltage,
                                Sys_Curr = finalData.Sys_Curr,
                                VL1_L2 = finalData.VL1_L2,
                                VL2_L3 = finalData.VL2_L3,
                                VL3_L1 = finalData.VL3_L1,
                                Frequency = finalData.Frequency,
                                ActivePower = Math.Round(finalData.ActivePower),
                                PowerFactor = finalData.PowerFactor,
                                Act_Imp_Energy = finalData.Act_Imp_Energy,
                                TS = finalData.TS,
                                IsAlive = finalData.EM_Master.IsAlive,
                                LastUpdate = finalData.EM_Master.UpdatedDate

                            };

                            EM_lst.Add(abc);

                        }
                    }
                    //ViewData["ClusterName"] = clstrName;
                    //  return View(@"~/Pages/Shared/_EM.cshtml", lstCluster);

                }
                return Ok(new { status = true, data = EM_lst.OrderBy(x => x.EM_Id) });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
      
        [HttpGet]
        [Route("get-meterReadings-Last24Hours")]
        public async Task<IActionResult> GetEM_Last24Hours()
        {
            try
            {
                DateTime now = DateTime.Now;

                var data = await Task.Run(() => _eM_Details.getEMbyDate(now.AddHours(-24), now));

                List<EM_DetailsViewModel> EM_lst = new List<EM_DetailsViewModel>();

                var v = data.Select(x => new { x.EM_Id }).Distinct().ToList();
                if (v != null)
                {
                    foreach (var EM in v)
                    {
                        var finalData = data.OrderByDescending(x => x.TS).Where(x => x.EM_Id == EM.EM_Id).FirstOrDefault();
                        if (finalData != null)
                        {
                            var abc = new EM_DetailsViewModel()
                            {
                                EM_Id = finalData.EM_Id,
                                EM_Name = finalData.EM_Master.EM_Name,
                                Sys_Voltage = finalData.Sys_Voltage,
                                Sys_Curr = finalData.Sys_Curr,
                                VL1_L2 = finalData.VL1_L2,
                                VL2_L3 = finalData.VL2_L3,
                                VL3_L1 = finalData.VL3_L1,
                                Frequency = finalData.Frequency,
                                ActivePower = finalData.ActivePower,
                                PowerFactor = finalData.PowerFactor,
                                Act_Imp_Energy = finalData.Act_Imp_Energy,
                                TS = finalData.TS,
                                IsAlive = finalData.EM_Master.IsAlive

                            };

                            EM_lst.Add(abc);

                        }
                    }
                    //ViewData["ClusterName"] = clstrName;
                    //  return View(@"~/Pages/Shared/_EM.cshtml", lstCluster);

                } 


                return Ok(new { status = true, data = EM_lst.OrderBy(x => x.EM_Id) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}
