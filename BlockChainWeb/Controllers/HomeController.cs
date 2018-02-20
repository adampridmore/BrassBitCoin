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

            var block = BlockChain.Miner.genesisBlock;

            var blockDto = new Repository.Block()
            {
                index = block.block.index,
                data = block.block.data,
                minedBy = block.block.minedBy,
                nonce = block.block.nonce,
                previousHash = block.block.previousHash,
                hash = block.hash
            };

            _repository.Save(blockDto);
                
            return Redirect("~/Home/About");
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
