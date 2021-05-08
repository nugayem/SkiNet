using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private  StoreContext _context { get; }

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("NotFound")]
        public IActionResult GetNotFoundRequest()
        {
            return NotFound(); // WHem Item not found
            //return Ok();
        }
        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            var vara=_context.ProductBrands.Find(42);

            var serviceUse=vara.ToString();
            //return NotFound(); // WHem Item not found
            return Ok();
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(); // WHem Item not found
            //return Ok();
        }
        [HttpGet("badrequest/{id}")]
        public IActionResult GetBadRequest(int id)
        {            
            return Ok();
        }
    }
}