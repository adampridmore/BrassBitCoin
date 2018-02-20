using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using Microsoft.Extensions.Configuration;

namespace BlockChainWeb.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly Repository.BlockChainRepository _repository;

        public HomeController(IConfiguration Configuration, Repository.BlockChainRepository repository)
        {
            _configuration = Configuration;
            this._repository = repository;
        }

        public IActionResult Index()
        {
            var model = _repository.GetAll();

            return View(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [HttpPost]
        public IActionResult ResetData()
        {
            _repository.DeleteAll();

            var genesisBlock = BlockChain.Miner.genesisBlock;
            SaveBlock(genesisBlock);

            BlockChain.Miner.blockchain(5, genesisBlock)
                .AsQueryable()
                .ToList()
                .ForEach(SaveBlock);

            return Redirect("~/Home/About");
        }

        private void SaveBlock(BlockChain.Miner.BlockWithHash genesisBlock)
        {
            var blockDto = new Repository.Block()
            {
                createdTimeStampUtc = DateTime.UtcNow,
                index = genesisBlock.block.index,
                data = genesisBlock.block.data,
                minedBy = genesisBlock.block.minedBy,
                nonce = genesisBlock.block.nonce,
                previousHash = genesisBlock.block.previousHash,
                hash = genesisBlock.hash
            };

            _repository.Save(blockDto);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
