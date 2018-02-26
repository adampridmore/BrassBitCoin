using System;
using BlockChainWeb.Helpers;
using BlockChain;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using System.Linq;

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
            return View("Upload", new UploadViewModel());
        }

        [HttpPost]
        public IActionResult Upload(string index,
                                    string minedBy,
                                    string data,
                                    string previousHash,
                                    string nonce,
                                    string hash)
        {
            var lastBlock = _repository.TryGetLastBlock();
            if (lastBlock == null)
            {
                throw new ApplicationException("No blocks in block chain");
            }

            Types.Block block = new Types.Block(
                long.Parse(index),
                minedBy,
                data,
                previousHash,
                long.Parse(nonce));

            var blockWithHash = new Types.BlockWithHash(block, hash);
            var isValidBlock = Miner.isValidBlock(blockWithHash, BlockHelpers.DtoToBlock(lastBlock));
            if (isValidBlock.IsInvalid)
            {
                var invalid = isValidBlock as Miner.IsValidBlock.Invalid;
                //throw new ApplicationException(
                //    "Invalid block: " + String.Join(Environment.NewLine, invalid.Item));
                var errors = invalid.Item.ToList();
                var model = new UploadViewModel()
                {
                    Errors = errors
                };

                return View("Upload", model);
            }

            _repository.Save(BlockHelpers.BlockToDto(blockWithHash));

            return Redirect("~/Blocks");
        }

        private static void AssertValidBlock(Repository.Block lastBlock, Types.BlockWithHash blockWithHash)
        {
            
        }

        public IActionResult Index()
        {
            return View(_repository.TryGetLastBlock());
        }
    }
}