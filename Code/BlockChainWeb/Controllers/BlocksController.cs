using System;
using BlockChain;
using Microsoft.AspNetCore.Mvc;
using BlockChainWeb.Models;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Repository.Dto;

namespace BlockChainWeb.Controllers
{
    public class BlocksController : Controller
    {
        private readonly Repository.BlockDtoRepository _repository;

        public BlocksController(Repository.BlockDtoRepository repository)
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

        private Tuple<bool, IList<string>> TryUploadBlock(int index,
                                    string minedBy,
                                    string data,
                                    string previousHash,
                                    int nonce,
                                    string hash)
        {
            var lastBlock = _repository.TryGetLastBlock();
            if (lastBlock == null)
            {
                throw new ApplicationException("No blocks in blockchain");
            }

            Types.Block block = new Types.Block(
                index,
                minedBy,
                data,
                previousHash,
                nonce);

            var blockWithHash = new Types.BlockWithHash(block, hash);
            var isValidBlock = Miner.isValidBlock(blockWithHash, DtoHelpers.DtoToBlock(lastBlock));
            if (isValidBlock.IsInvalid)
            {
                var invalid = isValidBlock as Types.IsValidBlock.Invalid;
                var errors = invalid.Item.ToList();

                return Tuple.Create<bool, IList<string>>(false, errors);
            }

            _repository.Save(DtoHelpers.BlockToDto(blockWithHash));

            return Tuple.Create<bool, IList<string>>(true, new List<string>());
        }

        [HttpPost]
        public ActionResult Upload([FromBody] PostUploadBlock block)
        {
            var validationErrors = ValidateBlock(block);
            if (validationErrors.Any())
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new
                    {
                        ok = "false",
                        errorMessage = String.Join(Environment.NewLine, validationErrors)
                    });
            }

            try
            {
                var result = TryUploadBlock(
                    int.Parse(block.index),
                    block.minedBy,
                    block.data,
                    block.previousHash,
                    int.Parse(block.nonce),
                    block.hash);

                if (result.Item1)
                {
                    return StatusCode((int)HttpStatusCode.Created,
                        new
                        {
                            ok = true,
                        });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest,
                    new
                    {
                        ok = "false",
                        errorMessage = string.Join(Environment.NewLine, result.Item2)
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new
                    {
                        ok = "false",
                        errorMessage = ex.Message
                    });
            }
        }

        private IEnumerable<string> ValidateBlock(PostUploadBlock block)
        {
            if (string.IsNullOrWhiteSpace(block.index) || !int.TryParse(block.index, out _))
            {
                yield return "Invalid index must be present and be a number.";
            }
            if (string.IsNullOrWhiteSpace(block.hash))
            {
                yield return "Invalid hash.";
            }
            if (string.IsNullOrWhiteSpace(block.data))
            {
                yield return "Invalid data.";
            }
            if (string.IsNullOrWhiteSpace(block.minedBy))
            {
                yield return "Invalid minedBy.";
            }
            if (String.IsNullOrWhiteSpace(block.nonce) || !int.TryParse(block.nonce, out _))
            {
                yield return "Invalid nonce must be present and be a number.";
            }
            if (string.IsNullOrWhiteSpace(block.previousHash))
            {
                yield return "Invalid previousHash.";
            }
        }
        
        public IActionResult Index()
        {
            return View(_repository.TryGetLastBlock());
        }

        public IActionResult Transactions()
        {
            var transactions = Transaction.getAllTransations(_repository);
            return View("Transactions", transactions);
        }
    }
}