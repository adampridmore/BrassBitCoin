using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChainWeb
{
    public class RepositoryFactory
    {
        public static Repository.BlockChainRepository GetBlockChainRepository()
        {
            return new Repository.BlockChainRepository("mongodb://localhost/BlockChain");
        }
    }
}
