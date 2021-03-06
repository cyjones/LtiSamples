﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using LtiLibrary.AspNet.Lti1;
using LtiLibrary.Core.Common;
using LtiLibrary.Core.Lti1;

namespace SimpleLti.Controllers
{
    public class ConsumerController : Controller
    {
        #region LTI 1.0 Tool Consumer

        /// <summary>
        /// Send a basic LTI launch request to the Tool Provider.
        /// </summary>
        /// <remarks>
        /// This is the basic function of a Tool Consumer.
        /// </remarks>
        public ActionResult Launch()
        {
            Uri launchUri;
            var ltiRequest = new LtiRequest(LtiConstants.BasicLaunchLtiMessageType)
            {
                ConsumerKey = "12345",
                ResourceLinkId = "launch",
                Url = Uri.TryCreate(Request.Url, Url.Action("Tool", "Provider"), out launchUri) ? launchUri : null
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
            ltiRequest.LisPersonEmailPrimary = "jdoe@andyfmiller.com";
            ltiRequest.LisPersonNameFamily = "Doe";
            ltiRequest.LisPersonNameGiven = "Joan";
            ltiRequest.UserId = "1";
            ltiRequest.SetRoles(new[] { Role.Instructor });

            // Basic Outcomes service (WebApi controller)
            var controllerUrl = UrlHelper.GenerateUrl("DefaultApi", null, "Outcomes",
                new RouteValueDictionary { { "httproute", string.Empty } }, RouteTable.Routes,
                Request.RequestContext, false);
            Uri controllerUri;
            if (Uri.TryCreate(Request.Url, controllerUrl, out controllerUri))
            {
                ltiRequest.LisOutcomeServiceUrl = controllerUri.AbsoluteUri;
            }
            ltiRequest.LisResultSourcedId = "ltilibrary-jdoe-1";

            // Outcomes V2 service (WebApi controller)
            controllerUrl = UrlHelper.GenerateUrl("LineItemsApi", null, "LineItems",
                new RouteValueDictionary
                {
                    { "httproute", string.Empty }, 
                    { "contextId", ltiRequest.ContextId }
                },
                RouteTable.Routes,
                Request.RequestContext, false);
            if (Uri.TryCreate(Request.Url, controllerUrl, out controllerUri))
            {
                ltiRequest.LineItemsServiceUrl = controllerUri.AbsoluteUri;
            }
            controllerUrl = UrlHelper.GenerateUrl("LineItemsApi", null, "LineItems",
                new RouteValueDictionary
                {
                    { "httproute", string.Empty }, 
                    { "contextId", ltiRequest.ContextId },
                    { "id", LineItemsController.LineItemId }
                },
                RouteTable.Routes,
                Request.RequestContext, false);
            if (Uri.TryCreate(Request.Url, controllerUrl, out controllerUri))
            {
                ltiRequest.LineItemServiceUrl = controllerUri.AbsoluteUri;
            }
            ltiRequest.AddCustomParameter("lineitem_url", "$LineItem.url");
            ltiRequest.AddCustomParameter("lineitems_url", "$LineItems.url");

            // Tool Consumer Profile service (WebApi controller)
            controllerUrl = UrlHelper.GenerateUrl("ToolConsumerProfileApi", null, "ToolConsumerProfile",
                new RouteValueDictionary { { "httproute", string.Empty } }, RouteTable.Routes,
                Request.RequestContext, false);
            if (Uri.TryCreate(Request.Url, controllerUrl, out controllerUri))
            {
                ltiRequest.ToolConsumerProfileUrl = controllerUri.AbsoluteUri;
            }
            ltiRequest.AddCustomParameter("tc_profile_url", "$ToolConsumerProfile.url");

            return View(ltiRequest.GetViewModel("secret"));
        }

        #endregion
    }
}