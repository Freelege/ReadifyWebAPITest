using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace ReadifyWebAPITest.Controllers
{
    public class TriangleTypeController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult Get(string a, string b, string c)
        {
            try
            {
                int na = Int32.Parse(a);
                int nb = Int32.Parse(b);
                int nc = Int32.Parse(c);

                return Ok<string>(TriangleType(na, nb, nc));
            }
            catch (Exception ex) when (ex is FormatException
                                        || ex is OverflowException
                                        || ex is ArgumentNullException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The request is invalid.", Encoding.UTF8, "application/json"),
                    ReasonPhrase = ex.Message
                };
                throw new HttpResponseException(resp);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        [NonAction]
        private  string TriangleType(int a, int b, int c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                return "Error";

            if (a + b <= c || a + c <= b || b + c <= a)
                return "Error";

            if (a == b && a == c)
                return "Equilateral";
            else if (a == b || a == c || b == c)
                return "Isosceles";
            else
                return "Scalene";
        }
    }
}
