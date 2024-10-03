using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telerik.SvgIcons;
using WebServiceOrlenPaczka;
using OrlenPaczka.Connected_Services.WebServiceOrlenPaczka;
using OrlenPaczka.Model;

namespace OrlenPaczka.Controllers
{
    public class HomeController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
            }
            base.OnActionExecuting(context);
        }
        [HttpGet]
        public IActionResult Start()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Start(IFormCollection form)
        {
            var request = new GenerateLabelBusinessPackRequest();
            request.DestinationCode = form["FindPoints"];

            #region Sender
            request.SenderFirstName = form["SenderFirstName"];
            request.SenderLastName = form["SenderLastName"];
            request.SenderStreetName = form["SenderStreet"];
            request.SenderPostCode= form["SenderZipCode"];
            request.SenderCity = form["SenderCity"];
            request.SenderEMail = form["SenderEmail"];
            request.SenderPhoneNumber = form["SenderPhone"];
            #endregion

            #region Recipient
            request.FirstName = form["RecipientFirstName"];
            request.LastName = form["RecipientLastName"];
            request.StreetName = form["RecipientStreet"];
            request.PostCode = form["RecipientZipCode"];
            request.City = form["RecipientCity"];
            request.EMail = form["RecipientEmail"];
            request.PhoneNumber = form["RecipientPhone"];
            #endregion

            var ret = CommunicationWithOrlenPaczka.Send(request);

            return new FileStreamResult(new MemoryStream(ret), "application/pdf");
        }
    }
}
