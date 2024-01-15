using System.Web.Http;
using NSwag.Annotations;

namespace NukeToolGenerationDemo
{
    public class EchoController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [SwaggerResponse(typeof(string))]
        public IHttpActionResult Ping(string message)
        {
            return this.Ok(message);
        }
    }
}
