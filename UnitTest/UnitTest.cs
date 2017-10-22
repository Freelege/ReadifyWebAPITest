using System;
using Xunit;
using ReadifyWebAPITest.Controllers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using UnitTest.Util;

namespace UnitTest
{
    public class FibonacciControllerTest:IDisposable
    {
        public FibonacciControllerTest()
        {
            string path = System.Environment.CurrentDirectory;
            File.Copy($"{path}\\Databases\\SmallBakery.mdf", $"{path}\\SmallBakery.mdf", true);
            File.Copy($"{path}\\Databases\\SmallBakery_log.ldf", $"{path}\\SmallBakery_log.ldf", true);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }
        public void Dispose()
        {
            string path = System.Environment.CurrentDirectory;
            TestUtil.DetachDB($"{path}\\SmallBakery.mdf");
            File.Delete($"{path}\\SmallBakery.mdf");
            File.Delete($"{path}\\SmallBakery_log.ldf");
        }
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public void GetTest(int x)
        {
            var controller = new FibonacciController();

            IHttpActionResult actionResult = controller.Get(Convert.ToString(x));
            var contentResult = actionResult as OkNegotiatedContentResult<long>;
           if (x == 10)
            Assert.Equal<long>(55, contentResult.Content);
           else if (x == 20)
            Assert.Equal<long>(6765, contentResult.Content);

            string connectString = @"Server = (localdb)\MSSQLLocalDB; Integrated Security = SSPI; AttachDbFilename = |DataDirectory|\SmallBakery.mdf";

            SqlConnection conn = new SqlConnection(connectString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Product", conn);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            rdr.Close();
            cmd.Dispose();
            conn.Close();
        }
    }

    public class ReverseWordsControllerTest:IDisposable
    {
        public ReverseWordsControllerTest()
        {

        }
        public void Dispose()
        {

        }

        [Theory]
        [InlineData("thank you")]
        public void GetTest(string sentence)
        {
            var controller = new ReverseWordsController();
            string expected = "knaht uoy";

            IHttpActionResult actionResult = controller.Get(sentence);
            var contentResult = actionResult as OkNegotiatedContentResult<string>;
            string actual = contentResult.Content;
            Assert.Equal(expected, actual);
        }
    }
}
