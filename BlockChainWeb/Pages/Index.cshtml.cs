using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlockChainWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string _url = "mongodb://localhost/BlockChain";

        public IList<Repository.Block> AllBlocks {get;set;}

        public void OnGet()
        {
            var repository = new Repository.BlockChainRepository(_url);

            AllBlocks = repository.GetAll();
        }
    }
}
