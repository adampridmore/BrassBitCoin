using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using Microsoft.Extensions.Configuration;
using Repository;
using BlockChain;

namespace BlockChainWeb.Controllers
{
    public class MinersController : Controller
    {
        private IConfiguration _configuration;
        private readonly BlockDtoRepository _repository;

        public MinersController(IConfiguration Configuration, BlockDtoRepository repository)
        {
            _configuration = Configuration;
            this._repository = repository;
        }

        public IActionResult Index()
        {
            var miners = Transaction.getAllMiners(_repository).ToList();

            return View(miners);
        }
    }
}
