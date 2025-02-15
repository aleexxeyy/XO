using Game.Services;
using Microsoft.AspNetCore.Mvc;

namespace Game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XOController
    {
        private readonly IXOService _xoService;

        public XOController(IXOService xoService)
        {
            _xoService = xoService; 
        }
    }
}
