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

        public HomeController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Index()
        {
            var mongoUrlText =_configuration["ConnectionStrings:MongoDB"];

            var repository = new Repository.BlockChainRepository(mongoUrlText);
            var model = repository.GetAll();

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
            var repository = RepositoryFactory.GetBlockChainRepository();

            repository.DeleteAll();

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


            repository.Save(blockDto);
                
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
