using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sample_ECDIPG.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
         

        [HttpPost]
        public JsonResult SendCustomerToIPG(long Amount, string Language, string BuyID)
        {
            DateTime DateNow = DateTime.Now;

            Models.IPGECD IPGECD = new Models.IPGECD();
         
            Models.IPGECD.IPGResult Result = IPGECD.PayRequest("Test", "96100000", BuyID, Amount, DateNow.ToString("yyyy/MM/dd"), DateNow.ToString("HH:mm"), "http://localhost:42202/Home/ResiveCustomerFromIPG", Language);

            return Json(Result);
           
        }

        //آدرس برگشت
        [HttpPost]
        public ActionResult ResiveCustomerFromIPG(Models.IPGECD.PayStartResult Result)
        {
            Models.IPGECD.IPGResult Model_Result = new Models.IPGECD.IPGResult();

            Models.IPGECD IPGECD = new Models.IPGECD();


            
            long Customer_Amount=10000;

            try
            {
                
                if (Result.State != 1)
                {

                    ViewBag.Error = Result.ErrorCode + " - " + Result.ErrorDescription;
                    return View("ErrorPage");
                }
               

                //در این قسمت با استفاده از شناسه خرید میتوانید خرید مشتری را یافت کنید و سپس مبلغ مشتری  با مبلغ تراکنش چک می شود اگر مبلغ مشتری با مبلغ تراکنش یکسان نباشد متد برگشت وجه صدا زده میشود
            if(Result.Amount !=Customer_Amount)
            {
                IPGECD.PayReverse(Result.Token);

                 ViewBag.Error = "مبلغ کسر شده در تراکنش با مبلغ مشتری یکسان نمی باشد";
                    return View("ErrorPage");
            }

            
              Models.IPGECD.IPGResult Model_ResultConf=IPGECD.PayConfirmation(Result.Token);

              if (Model_ResultConf.State != 1)
                   {
                       ViewBag.Error = Model_Result.ErrorCode + " - " + Model_Result.ErrorDescription;
                       return View("ErrorPage");
                   }

                   return View("SuccessTransaction");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ErrorPage");
               
            }

           
        }

    }
}