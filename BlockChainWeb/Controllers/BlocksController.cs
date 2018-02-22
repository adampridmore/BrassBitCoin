using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockChainWeb.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainWeb.Controllers
{
    public class BlocksController : Controller
    {
        private readonly Repository.BlockChainRepository _repository;

        public BlocksController(Repository.BlockChainRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult BlockChain()
        {
            var blocks = _repository.GetAll();

            return View(blocks);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(string index,
                                    string minedBy,
                                    string data,
                                    string previousHash,
                                    string nonce,
                                    string hash)
        {
            var lastBlock = _repository.GetLastBlock();
            
            global::BlockChain.Types.Block block = new BlockChain.Types.Block(
                long.Parse(index),
                minedBy,
                data,
                previousHash,
                long.Parse(nonce));

            var blockWithHash = new global::BlockChain.Types.BlockWithHash(block, hash);

            if (!global::BlockChain.Miner.isValidBlock(blockWithHash, BlockHelpers.DtoToBlock(lastBlock)))
            {
                throw new ApplicationException("Invalid block");
            }

            _repository.Save(BlockHelpers.BlockToDto(blockWithHash));

            return Redirect("~/Blocks");
        }













        public IActionResult Index()
        {
            return View(_repository.GetLastBlock());
        }
    }
}