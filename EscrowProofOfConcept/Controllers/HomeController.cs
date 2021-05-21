using EscrowProofOfConcept.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EscrowProofOfConcept.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Escrow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEscrowBrokerTransaction()
        {
            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes("hammad.ahmed@logicose.com:2068_L4cy65EqCQkFxfIjrRQggspC60zS7wN15rki6VTevzyQ0d53tTxC1hhnYQeOTXjb");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
              "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                currency = "usd",
                description = "Custom Project transfer5 ",
                items = new[]
                {
                    new
                    {
                        title = "johnwick.com",
                        description = "johnwick.com",
                        inspection_period = "86400",
                        type = "milestone",
                        quantity = "1",
                        schedule = new[]
                        {
                            new
                            {
                                payer_customer = "hammadahmed532@gmail.com",
                                amount = "350",
                                beneficiary_customer = "hammadkhatri2011@hotmail.com",
                            },
                        },
                    //},
                    //new
                    //{
                    //    title = "keanuReaves.com",
                    //    description = "keanuReaves.com",
                    //    inspection_period = "259200",
                    //    type = "milestone",
                    //    quantity = "1",
                    //    schedule = new[]
                    //    {
                    //        new
                    //        {
                    //            payer_customer = "hammadahmed532@gmail.com",
                    //            amount = "500",
                    //            beneficiary_customer = "hammadkhatri2011@hotmail.com",
                    //        },
                    //    },
                    },
                },
                parties = new[]
                {
                    new
                    {
                        customer = "me",
                        role = "broker",
                    },
                    new
                    {
                        customer = "hammadahmed532@gmail.com",
                        role = "buyer",
                    },
                    new
                    {
                        customer = "hammadkhatri2011@hotmail.com",
                        role = "seller",
                    },
                },
            };
 
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.escrow-sandbox.com/2017-09-01/transaction"),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            var model = JsonConvert.DeserializeObject<EscrowRootModel>(response.Content.ReadAsStringAsync().Result);
            var escrowViewModel = new EscrowViewModel();
            escrowViewModel.id = model.id;
            escrowViewModel.description= model.description;
            escrowViewModel.currency = model.currency;
            escrowViewModel.creationDate = model.creation_date;
            escrowViewModel.buyer = model.parties.Where(a => a.role == "buyer").FirstOrDefault().customer;
            escrowViewModel.seller = model.parties.Where(a => a.role == "seller").FirstOrDefault().customer;
            escrowViewModel.itemsAmount = model.items.Sum(a => a.schedule.Sum(v => v.amount));
            escrowViewModel.isTransactionCreated = true;
            escrowViewModel.items = model.items;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }

        [HttpPost]
        public ActionResult BuyerAgreeTransaction()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];
            HttpClient client = new HttpClient();

            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.buyer+":Optiplex123"); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                action = "agree",
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.escrow-sandbox.com/2017-09-01/transaction/"+ escrowViewModel.id),
                Method = new HttpMethod("PATCH"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.IsSuccessStatusCode) 
                escrowViewModel.isBuyerAgree = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);

        }

        [HttpPost]
        public ActionResult CreateCustomer(string name,string lastname,string email)
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];
            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes("hammad.ahmed@logicose.com:2068_L4cy65EqCQkFxfIjrRQggspC60zS7wN15rki6VTevzyQ0d53tTxC1hhnYQeOTXjb");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                phone_number = "2233458461",
                first_name = name,
                last_name = lastname,
                middle_name = "",
                address = new
                {
                    city = "San Francisco",
                    post_code = "10203",
                    country = "US",
                    line2 = "street 34",
                    line1 = "shah faisal",
                    state = "CA",
                },
                email = email,
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.escrow-sandbox.com/2017-09-01/customer"),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.IsSuccessStatusCode)
            { }
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }


        [HttpPost]
        public ActionResult SellerAgreeTransaction()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];

            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.seller+":Optiplex123");
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body2 = new
            {
                action = "agree",
            };
            var request= new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.escrow-sandbox.com/2017-09-01/transaction/"+escrowViewModel.id),
                Method = new HttpMethod("PATCH"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body2), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.IsSuccessStatusCode)
                escrowViewModel.isSellerAgree = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }


        [HttpPost]
        public ActionResult AvailablePaymentTransaction()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];
            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.buyer + ":Optiplex123"); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                "application/json"));
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(
                    "https://api.escrow-sandbox.com/2017-09-01/transaction/" + escrowViewModel.id+"/payment_methods"),
                Method = HttpMethod.Get,
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            AvailableOptions options= JsonConvert.DeserializeObject<AvailableOptions>(response.Content.ReadAsStringAsync().Result);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.IsSuccessStatusCode)
            {
                escrowViewModel.isAvailablePaymentOptions = true;
                escrowViewModel.availableOptions = options;
            }
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }


        [HttpPost]
        public ActionResult BuyerPayTransaction()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];

            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.buyer+":Optiplex123");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(
                    "https://api.escrow-sandbox.com/2017-09-01/transaction/" + escrowViewModel.id + "/payment_methods/wire_transfer"),
                Method = HttpMethod.Post,
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                escrowViewModel.isBuyerPay = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow",escrowViewModel);
        }

        [HttpPost]
        public ActionResult ApprovePayment()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];
            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes("hammad.ahmed@logicose.com:2068_L4cy65EqCQkFxfIjrRQggspC60zS7wN15rki6VTevzyQ0d53tTxC1hhnYQeOTXjb");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                method = "wire_transfer",
                amount = "1000.0",
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://integrationhelper.escrow-sandbox.com/v1/transaction/"+ escrowViewModel.id+ "/payments_in"),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
                escrowViewModel.isApprovePayment = true;

            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }

        [HttpPost]
        public ActionResult SellerDeliverMilestone()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];

            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.seller + ":Optiplex123");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
             "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                action = "ship",
                shipping_information = new
                {
                    tracking_information = new
                    {
                        carrier_contact = "1-234-567-8912",
                        carrier = "UPS",
                        tracking_id = "1Z999AA19993456784",
                    },
                },
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.escrow-sandbox.com/2017-09-01/transaction/"+ escrowViewModel.id + "/item/"+ escrowViewModel.items.FirstOrDefault().id),
                Method = new HttpMethod("PATCH"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
                escrowViewModel.isSellerDeliverItems = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }

        [HttpPost]
        public ActionResult BuyerRecieveMilestone()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];

            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.buyer + ":Optiplex123");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
              "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                action = "receive",
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(
                    "https://api.escrow-sandbox.com/2017-09-01/transaction/" + escrowViewModel.id + "/item/" + escrowViewModel.items.FirstOrDefault().id),
                Method = new HttpMethod("PATCH"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
           
            if (response.IsSuccessStatusCode)
                escrowViewModel.isBuyerRecieveItems = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }


        [HttpPost]
        public ActionResult BuyerAcceptsMilestone()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];

            HttpClient client = new HttpClient();
            byte[] auth = Encoding.ASCII.GetBytes(escrowViewModel.buyer + ":Optiplex123");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
              "Basic", Convert.ToBase64String(auth));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var body = new
            {
                action = "accept",
            };
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(
                    "https://api.escrow-sandbox.com/2017-09-01/transaction/" + escrowViewModel.id + "/item/" + escrowViewModel.items.FirstOrDefault().id),
                Method = new HttpMethod("PATCH"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            
            if (response.IsSuccessStatusCode)
                escrowViewModel.isBuyerAcceptMilestone = true;
            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }


        [HttpPost]
        public ActionResult DisbursePayment()
        {
            EscrowViewModel escrowViewModel = (EscrowViewModel)Session["EscrowViewModel"];


            Session["EscrowViewModel"] = escrowViewModel;
            return View("Escrow", escrowViewModel);
        }
    }
}