using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
        public IActionResult Get([FromHeader(Name = "Accept")] string accept, [FromQuery] bool latest = true)
        {
            IList<Repository.Dto.BlockDto> blockDtos;
            if (latest)
            {
                blockDtos = GetLatest();
            }
            else
            {
                blockDtos = GetAll(accept);
            }

            if (accept.Contains("application/json"))
            {
                return Json(blockDtos
                        .Select(BlockDtoToBlockWebModel)
                        .ToList());
            }
            else
            {
                var lines = blockDtos
                     .Select(BlockChain.DtoHelpers.DtoToBlock)
                     .Select(BlockChain.MinerHelpers.sprintBlock)
                     .ToList();

                var text = string.Join("\n", lines);

                return Content(text);
            }
        }

        private IList<Repository.Dto.BlockDto> GetAll(string accept)
        {
            return repository.GetAll();
        }

        private IList<Repository.Dto.BlockDto> GetLatest()
        {
            var block = repository.TryGetLastBlock();
            if (block == null)
            {
                throw new System.ApplicationException("No blocks");
            }

            return new List<Repository.Dto.BlockDto>
            {
                block
            };
        }

        private static Models.BlockWebModel BlockDtoToBlockWebModel(Repository.Dto.BlockDto block)
        {
            return new Models.BlockWebModel
            {
                index = block.index,
                minedBy = block.minedBy,
                data = block.data,
                nonce = block.nonce,
                previousHash = block.previousHash,
                hash = block.hash
            };
        }
    }
}