using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MpesaIntergration.Data;
using MpesaIntergration.Models;
using Newtonsoft.Json;
using System.Text;

namespace MpesaIntergration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly IHttpClientFactory _clientFactory;
        private ApplicationDbContext _dbContext;

        public HomeController(IHttpClientFactory clientFactory, ApplicationDbContext dbContext)
        {
            _clientFactory = clientFactory;
            _dbContext = dbContext;
        }
        [HttpGet("token")]
        public async Task<string> GetToken()
        {
            var client = _clientFactory.CreateClient("mpesa");

            var authString = "BNUChaITKHhHPP8SrjT6JP2ZXKoFyi1y:DVlNxfqbZM0iMdTs";

            var encodedString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authString));

            var _url = "/oauth/v1/generate?grant_type=client_credentials";
            //var _url = "/mpesa/stkpush/v1/processrequest";

            var request = new HttpRequestMessage(HttpMethod.Get, _url);

            request.Headers.Add("Authorization", $"Basic {encodedString}");

            var response = await client.SendAsync(request);

            var mpesaResponse = await response.Content.ReadAsStringAsync();

            Token tokenObject = JsonConvert.DeserializeObject<Token>(mpesaResponse);

            return tokenObject.access_token;

       
        }


        [HttpPost("register-urls")]
        public async Task<string> RegisterMpesaUrls()
        {
            var jsonBody = JsonConvert.SerializeObject(new
            {

                //CallBackURL= "https://mydomain.com/path",
                ValidationURL = "https://mydomain.com/validation",
                ConfirmationURL = "https://mydomain.com/confirmation",
                ResponseType = "Completed",
                //ShortCode = 174379
                ShortCode = 60098
            });

            var jsonReadyBody = new StringContent(
                jsonBody.ToString(),
                Encoding.UTF8,
                "application/json"
            );

            var token = await GetToken();

            var client = _clientFactory.CreateClient("mpesa");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var url = "/mpesa/c2b/v1/registerurl";

            var response = await client.PostAsync(url, jsonReadyBody);

            return await response.Content.ReadAsStringAsync();

        }


        [HttpPost("payment-confirmation")]
        public async Task<ActionResult> PaymentConfirmation([FromBody] MpesaC2B c2bPayments)
        {
            var respond = new
            {
                ResponseCode = 0,
                ResponseDesc = "Processed"
            };

            if (ModelState.IsValid)
            {
                 _dbContext.Add(c2bPayments);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new { code = 0, errors = ModelState });
            }

            return Ok(c2bPayments);
        }

        [HttpPost("payment-validation")]
        public async Task<ActionResult> PaymentValidation([FromBody] MpesaC2B c2bPayments)
        {
            var respond = new
            {
                ResponseCode = 0,
                ResponseDesc = "Processed"
            };

            return Ok(c2bPayments);
        }

        class Token
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
        }
    }
}



