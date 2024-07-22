using API_Vi_EnegryMeter.ViewModels;
using DATA;
using DATA.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IEM_Master _eM_Master;
        private readonly IEM_Details _eM_Details;
        private readonly ContextClass _context;

        public ReportController(IEM_Master eM_Master, IEM_Details eM_Details, ContextClass context)
        {

            _eM_Master = eM_Master;
            _eM_Details = eM_Details;
            _context = context;
        }
        //Show Report
        [HttpGet]
        [Route("get-report-bymeter")]
        public async Task<IActionResult> newGetDetails([FromQuery] int[] EmId, string FromDate, string ToDate)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(FromDate);
                DateTime toDate = Convert.ToDateTime(ToDate);

                if (EmId.Length == 0)
                {
                    return BadRequest(new { status = false, data = "EMId Not Given" });
                }

                var data = await Task.Run(() => _eM_Details.getEMbyId(EmId, fromDate, toDate));

                if (data.Count == 0)
                {
                    return Ok(new { status = false, data = "No Results Found" });
                }

                var groupedData = data
                    .Where(d => EmId.Contains(d.EM_Id))
                    .GroupBy(d => new { d.EM_Id, Date = d.TS_hours.Date })
                    .Select(g => new
                    {
                        g.Key.EM_Id,
                        g.Key.Date,
                        Readings = g.OrderBy(d => d.TS).ToList()
                    })
                    .ToList();

                List<EM_DetailsViewModel> result = new List<EM_DetailsViewModel>();

                foreach (var group in groupedData)
                {
                    DateTime dt = group.Readings.First().TS_hours;

                    while (dt.Date == group.Date)
                    {
                        var reading = group.Readings
                            .Where(r => Convert.ToDateTime(r.TS_hours.ToString("yyyy-MM-dd HH:mm")) == Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm")))
                            .OrderByDescending(r => r.TS)
                            .FirstOrDefault();

                        if (reading != null)
                        {
                            result.Add(new EM_DetailsViewModel
                            {
                                EM_Id = reading.EM_Id,
                                EM_Name = reading.EM_Master.EM_Name,
                                Sys_Voltage = reading.Sys_Voltage,
                                Sys_Curr = reading.Sys_Curr,
                                VL1_L2 = reading.VL1_L2,
                                VL2_L3 = reading.VL2_L3,
                                VL3_L1 = reading.VL3_L1,
                                Frequency = reading.Frequency,
                                ActivePower = Math.Round(reading.ActivePower),
                                PowerFactor = reading.PowerFactor,
                                Act_Imp_Energy = reading.Act_Imp_Energy,
                                NoOfInttruption = reading.NoOfInttruption,
                                TS = reading.TS_hours,
                                Kvah = Math.Round(reading.KVah)
                            });
                        }

                        dt = dt.AddMinutes(60);
                    }
                }

                return Ok(new { status = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        //Show Report
        [HttpGet]
        [Route("get-oldreport-bymeter")]
        public async Task<IActionResult> GetDetails([FromQuery] int[] EmId, string FromDate, string ToDate)
        {
            #region OldCode
            ////int ii = _context.Database.ExecuteSqlRaw("Sp_AutoDeleteHourly");
            //if (EmId == 0)
            //{
            //    var dat = _eM_Details.getEMbyDate(fdt, tdt);
            //    return Json(dat);
            //}
            //else
            //{
            //    var data = _eM_Details.getEMbyId(EmId, fdt, tdt);
            //    return Json(data);
            //}

            #endregion

            try
            {
                DateTime fdt = Convert.ToDateTime(FromDate);
                DateTime tdt = Convert.ToDateTime(ToDate);

                List<EM_DetailsViewModel> lst = new List<EM_DetailsViewModel>();

                //if (EmId == null)
                //{
                //    // return NewMethod3(fdt, tdt, lst);
                //}
                if (EmId.Length != 0)
                {
                    var data = await Task.Run(() => _eM_Details.getEMbyId(EmId, fdt, tdt));
                    if (data.Count != 0)
                    {
                        List<DateTime> lstdt = data.Select(x => x.TS_hours.Date).Distinct().ToList();
                        foreach (var Meter in EmId)
                        {
                            foreach (var date in lstdt)
                            {

                                var final = data.Where(x => x.TS.Date == date.Date && x.EM_Id == Meter).OrderBy(x => x.TS).ToList();
                                if (final.Count > 0)
                                {
                                    DateTime dt = final[0].TS_hours;// lstdt[0];//  data2[0].TS_hours;
                                    foreach (var item2 in final)
                                    {
                                        try
                                        {
                                            var v = final.Where(x => (Convert.ToDateTime(x.TS_hours.ToString("yyyy-MM-dd HH:mm"))) == Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm"))).ToList().OrderByDescending(x => x.TS).Take(1);
                                            if (v != null)
                                            {
                                                foreach (var item1 in v)
                                                {
                                                    EM_DetailsViewModel obj = new EM_DetailsViewModel();

                                                    obj.EM_Id = item1.EM_Id;
                                                    obj.EM_Name = item1.EM_Master.EM_Name;
                                                    obj.Sys_Voltage = item1.Sys_Voltage;
                                                    obj.Sys_Curr = item1.Sys_Curr;
                                                    obj.VL1_L2 = item1.VL1_L2;
                                                    obj.VL2_L3 = item1.VL2_L3;
                                                    obj.VL3_L1 = item1.VL3_L1;
                                                    obj.Frequency = item1.Frequency;
                                                    obj.ActivePower = Math.Round(item1.ActivePower);
                                                    obj.PowerFactor = item1.PowerFactor;
                                                    obj.Act_Imp_Energy = item1.Act_Imp_Energy;
                                                    obj.NoOfInttruption = item1.NoOfInttruption;
                                                    obj.TS = item1.TS_hours;
                                                    obj.Kvah = Math.Round(item1.KVah);
                                                    lst.Add(obj);
                                                }
                                                dt = dt.AddMinutes(60);
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                            return StatusCode(500, $"Internal Server Error{ex.Message}");
                                        }
                                    }
                                }

                            }



                        }
                        return Ok(new { status = true, data = lst });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "No Results Found" });
                    }
                    //foreach (var Meter in EmId)
                    //{
                    //    var firstReading = lst.Where(x => x.EM_Id == Meter).Select(x=>new { x.EM_Id,x.EM_Name,x.ActivePower}).First();
                    //    var LastReading = lst.Where(x => x.EM_Id == Meter).Select(x => new { x.EM_Id, x.EM_Name, x.ActivePower }).Last();

                    //    var TotalReading = LastReading.ActivePower - firstReading.ActivePower;
                    //}


                }
                else
                {
                    return BadRequest(new { status = false, data = "EMId Not Given" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error{ex.Message}");
            }

        }


        [HttpGet]
        [Route("get-report-summerydetails")]
        public async Task<IActionResult> GetSummeryDetails([FromQuery] int[] EmId, string FromDate, string ToDate)
        {
            if (EmId == null || EmId.Length == 0)
            {
                return BadRequest(new { status = false, data = "EMId Not Given" });
            }

            try
            {
                DateTime fdt = Convert.ToDateTime(FromDate);
                DateTime tdt = Convert.ToDateTime(ToDate);

                var data = await _eM_Details.getEMbyId(EmId, fdt, tdt);

                if (data.Count == 0)
                {
                    return Ok(new { status = false, data = "No Results Found" });
                }

                var groupedData = data
                    .GroupBy(x => new { x.EM_Id, x.TS_hours.Date })
                    .Select(g => new
                    {
                        EM_Id = g.Key.EM_Id,
                        Date = g.Key.Date,
                        FirstReading = g.OrderBy(x => x.TS).First(),
                        LastReading = g.OrderBy(x => x.TS).Last(),
                        AvgSys_Voltage = g.Average(x => x.Sys_Voltage),
                        AvgSys_Curr = g.Average(x => x.Sys_Curr),
                        AvgVL1_L2 = g.Average(x => x.VL1_L2),
                        AvgVL2_L3 = g.Average(x => x.VL2_L3),
                        AvgVL3_L1 = g.Average(x => x.VL3_L1),
                        MinActivePower = g.OrderBy(x => x.TS).Min(x => x.ActivePower),
                        MaxActivePower = g.OrderBy(x => x.TS).Max(x => x.ActivePower),

                    })
                    .ToList();


                var result = groupedData
                    .GroupBy(x => x.EM_Id)
                    .Select(g =>
                    {
                        var firstReading = g.First().FirstReading;
                        var lastReading = g.Last().LastReading;
                        var ActivePower = lastReading.ActivePower - firstReading.ActivePower;
                        decimal maxActivePower = g.Max(x => x.MaxActivePower);
                        decimal minActivePower = g.Min(x => x.MinActivePower);
                        // var ActivePower = g.Sum(x => x.LastReading.ActivePower - x.FirstReading.ActivePower);
                        if (ActivePower < 0)
                        {

                            ActivePower = (maxActivePower - firstReading.ActivePower) + (lastReading.ActivePower - minActivePower);
                        }
                        return new EM_DetailsViewModel
                        {
                            EM_Id = g.Key,
                            EM_Name = data.First(d => d.EM_Id == g.Key).EM_Master.EM_Name,
                            ActivePower = Math.Round(ActivePower),
                            Sys_Voltage = g.Average(x => x.AvgSys_Voltage),
                            Sys_Curr = g.Average(x => x.AvgSys_Curr),
                            VL1_L2 = g.Average(x => x.AvgVL1_L2),
                            VL2_L3 = g.Average(x => x.AvgVL2_L3),
                            VL3_L1 = g.Average(x => x.AvgVL3_L1),
                            FirstReading = Math.Round(firstReading.ActivePower),
                            LastReading = Math.Round(lastReading.ActivePower),
                            MinActivePower = Math.Round(minActivePower),
                            MaxActivePower = Math.Round(maxActivePower)
                        };
                    }).ToList();

                //var result = groupedData
                //    .GroupBy(x => x.EM_Id)
                //.Select(g => new EM_DetailsViewModel
                //{
                //    EM_Id = g.Key,
                //    EM_Name = data.First(d => d.EM_Id == g.Key).EM_Master.EM_Name,
                //    ActivePower = g.Sum(x => x.LastReading.ActivePower - x.FirstReading.ActivePower),
                //    Sys_Voltage = g.Average(x => x.AvgSys_Voltage),
                //    Sys_Curr = g.Average(x => x.AvgSys_Curr),
                //    VL1_L2 = g.Average(x => x.AvgVL1_L2),
                //    VL2_L3 = g.Average(x => x.AvgVL2_L3),
                //    VL3_L1 = g.Average(x => x.AvgVL3_L1),
                //    FirstReading = g.First().FirstReading.ActivePower,
                //    LastReading = g.Last().LastReading.ActivePower,
                //    MinActivePower = g.Min(x => x.MinActivePower),
                //    MaxActivePower = g.Max(x => x.MaxActivePower),

                //})
                //.ToList();


                return Ok(new { status = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        #region MyRegion
        //[HttpGet]
        //[Route("get-parameter-wise-compensation")]
        //public async Task<IActionResult> Getparameter_wise_compensation([FromQuery] int[] EmId, string FromDate, string ToDate)
        //{
        //    try
        //    {
        //        try
        //        {
        //            DateTime fdt = Convert.ToDateTime(FromDate);
        //            DateTime tdt = Convert.ToDateTime(ToDate);

        //            List<EM_DetailsViewModel> lst = new List<EM_DetailsViewModel>();

        //            if (EmId.Count() > 0)
        //            {
        //                var data = await Task.Run(() => _eM_Details.getEMbyId(EmId, fdt, tdt));
        //                if (data.Count != 0)
        //                {
        //                    List<DateTime> lstdt = data.Select(x => x.TS_hours.Date).Distinct().ToList();
        //                    foreach (var Meter in EmId)
        //                    {
        //                        foreach (var date in lstdt)
        //                        {

        //                            var final = data.Where(x => x.TS.Date == date.Date && x.EM_Id == Meter).OrderBy(x => x.TS).ToList();
        //                            if (final.Count > 0)
        //                            {
        //                                DateTime dt = final[0].TS_hours;// lstdt[0];//  data2[0].TS_hours;
        //                                foreach (var item2 in final)
        //                                {
        //                                    try
        //                                    {
        //                                        var v = final.Where(x => (Convert.ToDateTime(x.TS_hours.ToString("yyyy-MM-dd HH:mm"))) == Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm"))).ToList().OrderByDescending(x => x.TS).Take(1);
        //                                        if (v != null)
        //                                        {
        //                                            foreach (var item1 in v)
        //                                            {
        //                                                EM_DetailsViewModel obj = new EM_DetailsViewModel();

        //                                                obj.EM_Id = item1.EM_Id;
        //                                                obj.EM_Name = item1.EM_Master.EM_Name;
        //                                                obj.Sys_Voltage = item1.Sys_Voltage;
        //                                                obj.Sys_Curr = item1.Sys_Curr;
        //                                                obj.VL1_L2 = item1.VL1_L2;
        //                                                obj.VL2_L3 = item1.VL2_L3;
        //                                                obj.VL3_L1 = item1.VL3_L1;
        //                                                obj.Frequency = item1.Frequency;
        //                                                obj.ActivePower = Math.Round(item1.ActivePower);
        //                                                obj.PowerFactor = item1.PowerFactor;
        //                                                obj.Act_Imp_Energy = item1.Act_Imp_Energy;
        //                                                obj.NoOfInttruption = item1.NoOfInttruption;
        //                                                obj.TS = item1.TS_hours;

        //                                                lst.Add(obj);
        //                                            }
        //                                            dt = dt.AddMinutes(60);
        //                                        }

        //                                    }
        //                                    catch (Exception ex)
        //                                    {

        //                                        return StatusCode(500, $"Internal Server Error{ex.Message}");
        //                                    }
        //                                }
        //                            }

        //                        }



        //                    }

        //                    if (lst != null)
        //                    {
        //                        List<EM_DetailsViewModel> lst2 = new List<EM_DetailsViewModel>();
        //                        foreach (var Meter in EmId)
        //                        {
        //                            var v = lst.Where(x => x.EM_Id == Meter).ToList();
        //                            if (v.Count > 0)
        //                            {
        //                                #region Old
        //                                //var details = lst.Where(x => x.EM_Id == Meter).Select(x => new { x.EM_Id, x.EM_Name }).FirstOrDefault();

        //                                //var firstReading = lst.Where(x => x.EM_Id == Meter).Min(x => x.ActivePower);
        //                                //var LastReading = lst.Where(x => x.EM_Id == Meter).Max(x => x.ActivePower);

        //                                //var TotalReading = LastReading - firstReading;

        //                                //EM_DetailsViewModel obj = new EM_DetailsViewModel();
        //                                //obj.EM_Id = details.EM_Id;
        //                                //obj.EM_Name = details.EM_Name;
        //                                //obj.ActivePower = TotalReading;
        //                                #endregion

        //                                //var details = lst.Where(x => x.EM_Id == Meter).Select(x => new { x.EM_Id, x.EM_Name}).FirstOrDefault();

        //                                var firstReading = lst.Where(x => x.EM_Id == Meter).Select(x => new { x.EM_Id, x.EM_Name, x.ActivePower }).First();
        //                                var LastReading = lst.Where(x => x.EM_Id == Meter).Select(x => new { x.EM_Id, x.EM_Name, x.ActivePower }).Last();

        //                                var TotalReading = LastReading.ActivePower - firstReading.ActivePower;

        //                                var AvgSys_Voltage = lst.Where(x => x.EM_Id == Meter).Select(x => x.Sys_Voltage).Average();
        //                                var AvgSys_Curr = lst.Where(x => x.EM_Id == Meter).Select(x => x.Sys_Curr).Average();
        //                                var AvgVL1_L2 = lst.Where(x => x.EM_Id == Meter).Select(x => x.VL1_L2).Average();
        //                                var AvgVL2_L3 = lst.Where(x => x.EM_Id == Meter).Select(x => x.VL2_L3).Average();
        //                                var AvgVL3_L1 = lst.Where(x => x.EM_Id == Meter).Select(x => x.VL3_L1).Average();



        //                                EM_DetailsViewModel obj = new EM_DetailsViewModel();
        //                                obj.EM_Id = firstReading.EM_Id;
        //                                obj.EM_Name = firstReading.EM_Name;
        //                                obj.ActivePower = TotalReading;

        //                                obj.Sys_Voltage = AvgSys_Voltage;
        //                                obj.Sys_Curr = AvgSys_Curr;
        //                                obj.VL1_L2 = AvgVL1_L2;
        //                                obj.VL2_L3 = AvgVL2_L3;
        //                                obj.VL3_L1 = AvgVL3_L1;

        //                                lst2.Add(obj);
        //                            }

        //                        }
        //                        return Ok(new { status = true, data = lst2 });
        //                    }
        //                    else
        //                    {
        //                        return Ok(new { status = false, data = "No Results Found" });
        //                    }
        //                }
        //                else
        //                {
        //                    return Ok(new { status = false, data = "No Results Found" });
        //                }


        //            }
        //            else
        //            {
        //                return BadRequest(new { status = false, data = "EMId Not Given" });
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            return StatusCode(500, $"Internal Server Error{ex.Message}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, $"Internal Server Error = {ex.Message}");
        //    }
        //}

        #endregion


        [HttpGet]
        [Route("get-lineGraph-data")]
        public async Task<IActionResult> GetLine_GraphData([FromQuery] int[] EmId, DateTime FromDate, DateTime ToDate)
        {
            try
            {

                var data = _context.EM_Details.AsQueryable()
                    .Include(x => x.EM_Master)
                    .Where(x => x.TS.Date >= FromDate.Date && x.TS.Date <= ToDate.Date && EmId.Contains(x.EM_Id))
                    .Select(
                    x => new
                    {
                        x.EM_Id,
                        x.EM_Master.EM_Name,
                        x.ActivePower,
                        month = x.TS.ToString("MMM"),
                        monthid = x.TS.Month,
                        year = x.TS.Year
                    })
                    .ToList();

                #region MyRegion
                //List<LineGraph_EM_DetailsViewModel> lst2 = new List<LineGraph_EM_DetailsViewModel>();
                //var EMId = _context.EM_Details.Select(x => x.EM_Id).Distinct().ToList();
                //foreach (var id in EMId)
                //{
                //    var FirstAP = data.Where(x => x.EM_Id == id).Select(x => new { x.EM_Id, x.EM_Name, x.ActivePower, x.month, x.monthid, x.year }).First();
                //    var LastAP = data.Where(x => x.EM_Id == id).Select(x => new { x.EM_Id, x.EM_Name, x.ActivePower }).Last();
                //    var total = LastAP.ActivePower - FirstAP.ActivePower;
                //    LineGraph_EM_DetailsViewModel obj = new LineGraph_EM_DetailsViewModel();
                //    obj.ActivePower = total;
                //    obj.EM_Id = FirstAP.EM_Id;
                //    obj.EM_Name = FirstAP.EM_Name;
                //    obj.monthID = FirstAP.monthid;
                //    obj.Month = FirstAP.month;
                //    obj.year = FirstAP.year;
                //    lst2.Add(obj);

                //}


                //var final = data.GroupBy(t => new { ID = t.EM_Id, t.EM_Name, t.month, t.monthid, t.year })
                //    .Select(g => new
                //    {
                //        Average = g.Average(p => p.ActivePower),
                //        EM = g.Key.ID,
                //        EM_Name = g.Key.EM_Name,
                //        Month = g.Key.month,
                //        monthID = g.Key.monthid,
                //        year = g.Key.year
                //    }).OrderBy(x => x.year).ToList(); 
                #endregion

                var final = data.GroupBy(t => new { t.EM_Id, t.EM_Name, t.month, t.monthid, t.year })
                                .Select(g =>
                                  {
                                      var first = g.OrderBy(p => p.month).First();
                                      var last = g.OrderBy(p => p.month).Last();
                                      var maxActivePower = g.OrderBy(p => p.month).Max(p => p.ActivePower);
                                      var minActivePower = g.OrderBy(p => p.month).Min(p => p.ActivePower);


                                      return new
                                      {
                                          TotalActivePower = last.ActivePower - first.ActivePower,
                                          Monthly_FirstAP = first.ActivePower,
                                          Monthly_LastAP = last.ActivePower,
                                          EM = g.Key.EM_Id,
                                          EM_Name = g.Key.EM_Name,
                                          Month = g.Key.month,
                                          MonthID = g.Key.monthid,
                                          Year = g.Key.year,
                                          MinActivePower = minActivePower,
                                          MaxActivePower = maxActivePower
                                      };
                                  })
                                 .OrderBy(x => x.Year)
                                 .ToList();


                return Ok(new { status = true, data = final });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error = {ex.Message }");
            }
        }


        [HttpGet]
        [Route("get-currentdate-reading-bymeter")]
        public async Task<IActionResult> GetCurrentreading([FromQuery] int[] EmId)
        {
            if (EmId == null || EmId.Length == 0)
            {
                return BadRequest(new { status = false, data = "EMId Not Given" });
            }

            try
            {
               DateTime currrntDate = DateTime.Now.Date.AddMonths(-7).AddDays(18);
               // DateTime currrntDate = DateTime.Now;


                // Fetch data asynchronously
                var data = await Task.Run(() => _eM_Details.getEM_CurrentReading(EmId, currrntDate));

                if (data.Count == 0)
                {
                    return Ok(new { status = false, data = "No Results Found" });
                }

                var groupedData = data
                    .Where(d => EmId.Contains(d.EM_Id))
                    .GroupBy(d => new { d.EM_Id, Date = d.TS_hours.Date })
                    .Select(g => new
                    {
                        g.Key.EM_Id,
                        g.Key.Date,
                        Readings = g.OrderBy(d => d.TS).ToList()
                    })
                    .ToList();

                List<EM_DetailsViewModel> result = new List<EM_DetailsViewModel>();

                foreach (var group in groupedData)
                {
                    DateTime dt = group.Readings.First().TS_hours;

                    while (dt.Date == group.Date)
                    {
                        var reading = group.Readings
                            .Where(r => Convert.ToDateTime(r.TS_hours.ToString("yyyy-MM-dd HH:mm")) == Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm")))
                            .OrderByDescending(r => r.TS)
                            .FirstOrDefault();

                        if (reading != null)
                        {
                            result.Add(new EM_DetailsViewModel
                            {
                                EM_Id = reading.EM_Id,
                                EM_Name = reading.EM_Master.EM_Name,
                                Sys_Voltage = reading.Sys_Voltage,
                                Sys_Curr = reading.Sys_Curr,
                                VL1_L2 = reading.VL1_L2,
                                VL2_L3 = reading.VL2_L3,
                                VL3_L1 = reading.VL3_L1,
                                Frequency = reading.Frequency,
                                ActivePower = Math.Round(reading.ActivePower),
                                PowerFactor = reading.PowerFactor,
                                Act_Imp_Energy = reading.Act_Imp_Energy,
                                NoOfInttruption = reading.NoOfInttruption,
                                TS = reading.TS_hours,
                                Kvah = Math.Round(reading.KVah)
                            });
                        }

                        dt = dt.AddMinutes(60);
                    }
                }

                return Ok(new { status = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
