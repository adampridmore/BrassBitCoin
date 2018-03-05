using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlockChainWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Blocks")]
    public class BlocksApiController : Controller
    {
        private Repository.BlockDtoRepository repository;
        public BlocksApiController(Repository.BlockDtoRepository repository)
        {
            this.repository = repository;
        }
        public IActionResult Get([FromQuery] bool latest)
        {
            if (latest != true)
            {
                return BadRequest("latest query parameter must be specified and set to true");
            }

            var block = repository.TryGetLastBlock();
            if (block == null)
            {
                throw new System.ApplicationException("No blocks");
            }

            var blockWebModel = new Models.BlockWebModel
            {
                index = block.index,
                minedBy = block.minedBy,
                data = block.data,
                nonce = block.nonce,
                previousHash = block.previousHash,
                hash = block.hash
            };

            return Json(new List<Models.BlockWebModel>
            {
                blockWebModel
            });
        }
    }
}