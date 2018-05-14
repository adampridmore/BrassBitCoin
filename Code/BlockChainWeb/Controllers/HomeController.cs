using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using Microsoft.Extensions.Configuration;
using Repository;
using BlockChain;
using System.Collections.Generic;

namespace BlockChainWeb.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly BlockDtoRepository _repository;

        public HomeController(IConfiguration Configuration, Repository.BlockDtoRepository repository)
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

            var blocks = new List<Types.BlockWithHash>();

            blocks.Add(Miner.genesisBlock);
            blocks.Add(Miner.newBlock("Bob", "Hello", blocks.Last()));
            blocks.Add(Miner.newBlock("Adam", "Transaction,Adam,Eve,1", blocks.Last()));
            blocks.Add(Miner.newBlock("Adam", "Hello", blocks.Last()));

            blocks.ForEach(SaveBlock);

            return Redirect("~/Home/About");
        }

        [HttpPost]
        public IActionResult MineAndSaveBlock()
        {
            var lastBlockDto = _repository.TryGetLastBlock();
            if (lastBlockDto == null)
            {
                throw new ApplicationException("No blocks in chain");
            }

            var lastBlock = DtoHelpers.DtoToBlock(lastBlockDto);

            Miner.blockchain(1, "Adam", lastBlock)
                .AsQueryable()
                .ToList()
                .ForEach(SaveBlock);

            return Redirect("~/Home/About");
        }

        [HttpPost]
        public IActionResult ResetToValidBlockchain1()
        {
            _repository.DeleteAll();

            var blocks = new List<Types.BlockWithHash>();

            blocks.Add(Miner.genesisBlock);
            blocks.Add(Miner.newBlock("Bob", "", blocks.Last()));
            blocks.Add(Miner.newBlock("Bob", "Transaction,Bob,Adam,2", blocks.Last()));
            blocks.Add(Miner.newBlock("Bob", "Transaction,Adam,Eve,1", blocks.Last()));
            blocks.Add(Miner.newBlock("Adam", "Transaction,Adam,Eve,1", blocks.Last()));

            blocks.ForEach(SaveBlock);

            return CreateRedirectToAboutPage();
        }

        public IActionResult ResetToValidBlockchain2()
        {
            _repository.DeleteAll();

            var blocks = new List<Types.BlockWithHash>();

            blocks.Add(Miner.genesisBlock);
            blocks.Add(Miner.newBlock("Fred", "Transaction,Fred,Adam,1", blocks.Last()));
            blocks.Add(Miner.newBlock("Adam", "Transaction,Adam,Sally,2", blocks.Last()));

            blocks.ForEach(SaveBlock);

            return CreateRedirectToAboutPage();
        }

        [HttpPost]
        public IActionResult ResetToInvalidBlockchain()
        {
            _repository.DeleteAll();

            var blocks = new List<Types.BlockWithHash>();

            blocks.Add(Miner.genesisBlock);
            blocks.Add(Miner.newBlock("Alice", "", blocks.Last()));
            blocks.Add(Miner.newBlock("Alice", "Transaction,Alice,Bob,2", blocks.Last()));
            blocks.Add(Miner.newBlock("Alice", "Transaction,Alice,Bob,1", blocks.Last()));
            blocks.Add(Miner.newBlock("Mallory", "Transaction,Bob,Mallory,2", blocks.Last()));

            var blocksToSave = blocks.Select(DtoHelpers.BlockToDto).ToList();

            // Not a valid hash!
            blocksToSave.Last().hash = "0000FFD633A14A7C68F70A1158452BCCB9E8D994B509149DE57C1D11D0199B32";

            blocksToSave.ForEach(_repository.Save);

            return CreateRedirectToAboutPage();
        }

        private IActionResult CreateRedirectToAboutPage()
        {
            return Redirect("~/Home/About");
        }

        private void SaveBlock(Types.BlockWithHash blockWithHash)
        {
            var blockDto = DtoHelpers.BlockToDto(blockWithHash);
            
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

        public IActionResult BlockChainDefinition()
        {
            return View();
        }
    }
}
