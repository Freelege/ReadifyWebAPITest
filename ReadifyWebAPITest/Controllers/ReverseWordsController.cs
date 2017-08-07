using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Web.Http.Description;

namespace ReadifyWebAPITest.Controllers
{
    public class ReverseWordsController : ApiController
    {
        [HttpGet]
        [Route("api/ReverseWords/{sentence}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Get(string sentence)
        {
            if (sentence == null)
                return Ok<string>(null);

            if (sentence.IndexOf(' ') == -1)
                return Ok<string>(ReverseOneWord(sentence));

            try
            {
                int head = 0, tail = 0;
                StringBuilder destStr = new StringBuilder();
                while (tail < sentence.Length)
                {
                    while (head < sentence.Length && sentence.ElementAt(head) == ' ')  //Let head point to the first letter of each word
                    {
                        head++;
                        destStr.Append(' ');
                    }

                    if (head >= sentence.Length)  //The sentence tails are all spaces
                        break;

                    tail = sentence.IndexOf(' ', head);
                    if (tail == -1)   // reach the end of the sentence
                        tail = sentence.Length;

                    string word = sentence.Substring(head, tail - head);
                    string reversedWord = ReverseOneWord(word);
                    destStr.Append(reversedWord);

                    head = tail;
                }

                return Ok<string>(destStr.ToString());
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [NonAction]
        private string ReverseOneWord(string word)
        {
            Stack<char> stack = new Stack<char>();

            for (int i = 0; i < word.Length; i++)
                stack.Push(word[i]);

            StringBuilder stb = new StringBuilder();
            while (stack.Count != 0)
            {
                stb.Append(stack.Pop());
            }

            return stb.ToString();
        }
    }
}
