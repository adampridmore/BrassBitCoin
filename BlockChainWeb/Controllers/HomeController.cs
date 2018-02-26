﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using Microsoft.Extensions.Configuration;
using Repository;
using BlockChain;
using BlockChainWeb.Helpers;

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
        public IActionResult MineAndSaveBlock()
        {
            var lastBlockDto = _repository.TryGetLastBlock();
            if (lastBlockDto == null)
            {
                throw new ApplicationException("No blocks in chain");
            }

            var lastBlock = BlockHelpers.DtoToBlock(lastBlockDto);

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
                
        private void SaveBlock(Types.BlockWithHash blockWithHash)
        {
            var blockDto = BlockHelpers.BlockToDto(blockWithHash);
            
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
