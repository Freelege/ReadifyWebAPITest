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
    public class FibonacciController : ApiController
    { 
        [HttpGet]
        [Route("api/Fibonacci/{n}")]  
        //[ResponseType(typeof(long))]
        //usage: localhost:4578/api/Fibonacci/20, if no RouteAttribute, it should be localhost:4578/api/Fibonacci?n=20
        public IHttpActionResult Get(string n)   //This WebAPI is to work out the nth fibonacci number
        {
            try
            {
                long number = Int64.Parse(n);  //Firstly need to convert string parameter to long integer
                             
                if (number < 0)
                    throw new ArgumentOutOfRangeException();
                else
                    return Ok<long>(GetFibonacci(number));   //For simple types just use Ok<>() is enough.
                   // return Json<long>(GetFibonacci(number));
            }
            catch (Exception ex) when (ex is FormatException 
                                        || ex is OverflowException 
                                        || ex is ArgumentOutOfRangeException
                                        || ex is ArgumentNullException)
            {
                //return StatusCode(HttpStatusCode.BadRequest);
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
        private long GetFibonacci(long n)
        {
            if (n == 0)   //When the input is 0, return 0. 
                return 0;
            else if (n == 1 || n == 2)
                return 1;
            else
            {
                long Fn, Fnp, Fnpp;
                Fn = Fnp = Fnpp = 1;
                for (long i = 3; i <= n; i++)
                {
                    if (Fnp > Int64.MaxValue - Fnpp)  //Overflow judgement
                        throw new OverflowException();

                    Fn = Fnp + Fnpp;  //Fn = Fn-1 + Fn-2
                    Fnpp = Fnp;
                    Fnp = Fn;
                }
                return Fn;
            }
        }
    }
}