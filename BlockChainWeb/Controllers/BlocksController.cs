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

        public IActionResult Index()
        {
            var blocks = _repository.GetAll();

            return View(blocks);
        }
    }
}