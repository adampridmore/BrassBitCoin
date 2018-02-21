using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using Microsoft.Extensions.Configuration;
using Repository;
using BlockChain;

namespace BlockChainWeb.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly BlockChainRepository _repository;

        public HomeController(IConfiguration Configuration, Repository.BlockChainRepository repository)
        {
            _configuration = Configuration;
            this._repository = repository;
        }

        public IActionResult Index()
        {
            return View();
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

            var genesisBlock = Miner.genesisBlock;
            SaveBlock(genesisBlock);

            Miner.blockchain(5, genesisBlock)
                .AsQueryable()
                .ToList()
                .ForEach(SaveBlock);

            return Redirect("~/Home/About");
        }

        [HttpPost]
        public IActionResult MineBlock()
        {
            var lastBlockDto = _repository.GetLastBlock();

            var lastBlock = DtoToBlock(lastBlockDto);

            Miner.blockchain(1, lastBlock)
                .AsQueryable()
                .ToList()
                .ForEach(SaveBlock);

            return Redirect("~/Home/About");
        }

        public IActionResult CoinOwners()
        {
            var coinOwners = _repository.GetCoinOwners();

            return View(coinOwners);
        }

        private Types.BlockWithHash DtoToBlock(Block lastBlockDto)
        {
            var block = new Types.Block(lastBlockDto.index, lastBlockDto.minedBy, lastBlockDto.data, lastBlockDto.previousHash, lastBlockDto.nonce);
            return new Types.BlockWithHash(block, lastBlockDto.hash);
        }

        private void SaveBlock(Types.BlockWithHash genesisBlock)
        {
            var blockDto = new Block()
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
