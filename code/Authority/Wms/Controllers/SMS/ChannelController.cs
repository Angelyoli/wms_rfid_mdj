using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.SMS.DbModel;
using THOK.SMS.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Common.WebUtil;
using THOK.Common.NPOI.Models;
using THOK.Common.NPOI.Service;

namespace Wms.Controllers.SMS
{
    public class ChannelController : Controller
    {
        [Dependency]
        public IChannelService ChannelService { get; set; }
        //
        // GET: /Channel/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public JsonResult Details(int page, int rows,Channel channel)
        {
            channel.ChannelName = channel.ChannelName ?? "";
            channel.ChannelType = channel.ChannelType ?? "";
            channel.Status = channel.Status ?? "";
            var channelDetails = ChannelService.GetDetails(page, rows, channel);
            return Json(channelDetails, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Channel/Create
        [HttpPost]
        public ActionResult Create(Channel channel)
        {
            string strResult = string.Empty;
            bool bResult = ChannelService.Add(channel, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Channel/Edit/
        public ActionResult Edit(Channel channel)
        {
            string strResult = string.Empty;
            bool bResult = ChannelService.Edit(channel, out strResult);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Channel/Delete/
        [HttpPost]
        public ActionResult Delete(string channelCode)
        {
            string strResult = string.Empty;
            bool bResult = ChannelService.Delete(channelCode, out strResult);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //打印
        public FileStreamResult CreateExcelToClient()
        {
            string channelName = Request.QueryString["channelName"] ?? "";
            string channelType = Request.QueryString["channelType"] ?? "";
            string status = Request.QueryString["status"] ?? "";
            string groupNo = Request.QueryString["groupNo"] ?? "";

            ExportParam ep = new ExportParam();
            ep.DT1 = ChannelService.GetChannel(channelName,channelType,status,groupNo);
            ep.HeadTitle1 = "烟道信息";
            return PrintService.Print(ep);
        }
    }
}
