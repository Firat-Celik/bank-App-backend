using bankApp.Model;
using bankApp.Model.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Web;

namespace bankApp.Controllers
{
    [EnableCors("CORS")]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        #region Constructor
         
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(  IHttpContextAccessor httpContextAccessor)
        { 
            _httpContextAccessor = httpContextAccessor;
        }

        #region AccountData   

        List<Account> accountList = new List<Account>{
             new Account{ hesapNo=1,hesapAdi="Ziraat Bank xxxx Þubesi", bakiye=1000.00M,paraBirimi="TL"},
             new Account{ hesapNo=2,hesapAdi="Türkiye Finans  xxxx Þubesi", bakiye=2500.00M,paraBirimi="TL"},
             new Account{ hesapNo=3,hesapAdi="Deniz Bank xxxx Þubesi", bakiye=3700.00M,paraBirimi="TL"},
             new Account{ hesapNo=4,hesapAdi="Halk Bank xxxx Þubesi", bakiye=10000.00M,paraBirimi="TL"},
             new Account{ hesapNo=5,hesapAdi="Türkiye Ýþ Bankasý xxxx Þubesi", bakiye=12000.00M,paraBirimi="TL"},
             new Account{ hesapNo=6,hesapAdi="Ýng Bank xxxx Þubesi", bakiye=500.00M,paraBirimi="TL"},
             new Account{ hesapNo=7,hesapAdi="QNB Finans Bank xxxx Þubesi", bakiye=4000.00M,paraBirimi="TL"},
              };
        #endregion

        #endregion Constructor

        #region EndPoints

        [HttpGet, Route("GetAccounts")]
        public IEnumerable<Account> Get()
        {
            var List1 = HttpContext.Session.GetString("accountList");
            if (List1 == null)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(accountList);
                HttpContext.Session.SetString("accountList", json);
            }
            var data = HttpContext.Session.GetString("accountList");
            var accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(data);
            return accounts.OrderBy(x => x.hesapNo).ToList();


        }

        [HttpPost, Route("DepositMoney")]
        public async Task<bool> DepositMoney([FromForm] AddMoneyInputDto input)
        {
            if (!input.hesapNo.HasValue || !input.Tutar.HasValue)
            {
                throw new Exception("Lütfen Gerekli Bilgileri Giriniz!");

            }
            var data1 = HttpContext.Session.GetString("accountList");

            if (data1 == null)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(accountList);
                HttpContext.Session.SetString("accountList", json);


            }
            var data = HttpContext.Session.GetString("accountList");
            var accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(data);
            var updateData = accounts.FirstOrDefault(x => x.hesapNo == input.hesapNo);
            updateData.bakiye = (decimal)(updateData.bakiye + input.Tutar);
            var newList = accounts.Where(x => x.hesapNo != input.hesapNo).ToList();
            List<Account> updatedList = new List<Account>();
            updatedList.AddRange(newList);
            updatedList.Add(updateData);
            HttpContext.Session.Remove("accountList");
            var jsonn = System.Text.Json.JsonSerializer.Serialize(updatedList);
            HttpContext.Session.SetString("accountList", jsonn);
            return true;
        }

        [HttpPost, Route("WithdrawMoney")]
        public async Task<bool> WithdrawMoney([FromForm] AddMoneyInputDto input)
        {
            if (!input.hesapNo.HasValue || !input.Tutar.HasValue)
            {
                throw new Exception("Lütfen Gerekli Bilgileri Giriniz!");

            }
            var data1 = HttpContext.Session.GetString("accountList");

            if (data1 == null)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(accountList);
                HttpContext.Session.SetString("accountList", json);


            }
            var data = HttpContext.Session.GetString("accountList");
            var accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(data);
            var updateData = accounts.FirstOrDefault(x => x.hesapNo == input.hesapNo);
            updateData.bakiye = (decimal)(updateData.bakiye - input.Tutar);
            var newList = accounts.Where(x => x.hesapNo != input.hesapNo).ToList();
            List<Account> updatedList = new List<Account>();
            updatedList.AddRange(newList);
            updatedList.Add(updateData);
            HttpContext.Session.Remove("accountList");
            var jsonn = System.Text.Json.JsonSerializer.Serialize(updatedList);
            HttpContext.Session.SetString("accountList", jsonn);
            return true;
        }

        #endregion EndPoints
    }
}