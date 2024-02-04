using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using Telerik.SvgIcons;
using ZadanieRuchSA.Connected_Services.WebServiceOrlenPaczka;
using ZadanieRuchSA.Model;

namespace ZadanieRuchSA.Controllers
{
    public class OperationsController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
            }
            base.OnActionExecuting(context);
        }
        public ActionResult GetPointsByPhrase([DataSourceRequest] DataSourceRequest request, string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var paczki = CommunicationWithOrlenPaczka.pointPwRs.Where(x => x.City.ToLower().Contains(id.ToLower())
                                                                               || x.DestinationCode.ToLower().Contains(id.ToLower())
                                                                               || x.ZipCode.ToLower().Contains(id.ToLower())
                                                                               );
                var listPoints = new List<PointsListView>();
                foreach (var item in paczki)
                {
                    var plv = new PointsListView();
                    plv.CodePoint = item.DestinationCode;
                    plv.City = item.City;
                    plv.ZippCode = item.ZipCode;
                    plv.StreetName = item.StreetName;
                    plv.PointType = item.PointType;
                    plv.Description = item.Location;
                    plv.NrPSD = item.PSD.ToString();
                    listPoints.Add(plv);
                }
                var ret = listPoints.Count > 10 ? listPoints.Take(10).ToList() : listPoints.ToList();
                var dsResult = ret.ToDataSourceResult(request);

                return Json(dsResult);
            }
            return Json("");
        }
    }
}
