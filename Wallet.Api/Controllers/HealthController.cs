using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { status = "ok" });
        }
    }
}
