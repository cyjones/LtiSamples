using System;
using System.Web.Mvc;
using System.Web.Routing;
using LtiLibrary.AspNet.Lti1;
using LtiLibrary.Core.Common;
using LtiLibrary.Core.Lti1;

namespace SimpleLti.Controllers
{
    public class EdFiController : Controller
    {
        // GET: EdFi
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LaunchEdFi()
        {
            Uri launchUri;

            var ltiRequest = new LtiRequest(LtiConstants.BasicLaunchLtiMessageType)
            {
                ConsumerKey = "809eef5f2bd58",
                ResourceLinkId = "launch",
                Url = new Uri("http://localhost/EdFiDashboardDev_STS/Login.aspx")
            };

            // Tool
            ltiRequest.ToolConsumerInfoProductFamilyCode = "LtiLibrary";
            ltiRequest.ToolConsumerInfoVersion = "1.2";

            // Context
            ltiRequest.ContextId = "Home";
            ltiRequest.ContextTitle = "Home";
            ltiRequest.ContextType = LisContextType.CourseSection;

            // Instance
            ltiRequest.ToolConsumerInstanceGuid = Request.Url == null ? null : Request.Url.Authority;
            ltiRequest.ToolConsumerInstanceName = "LtiLibrary Sample";
            ltiRequest.ResourceLinkTitle = "Launch";
            ltiRequest.ResourceLinkDescription = "Perform a basic LTI 1.2 launch";

            // User
            ltiRequest.LisPersonEmailPrimary = "melissa.cantu@edfi.org";
            ltiRequest.LisPersonNameFamily = "Melissa";
            ltiRequest.LisPersonNameGiven = "Cantu";
            ltiRequest.UserId = "2731";
            ltiRequest.AddCustomParameter("localeducationagencyid", "867530");
            ltiRequest.AddCustomParameter("staffusi", "2731");

            return View("Launch", ltiRequest.GetViewModel("jwJApwXGIToLcONW"));
        }
    }
}