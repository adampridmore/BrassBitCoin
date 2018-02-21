using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Upload()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View(_repository.GetLastBlock());
        }
    }
}