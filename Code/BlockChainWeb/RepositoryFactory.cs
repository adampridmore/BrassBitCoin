namespace BlockChainWeb
{
    public class RepositoryFactory
    {
        public static Repository.BlockDtoRepository GetBlockChainRepository()
        {
            return new Repository.BlockDtoRepository("mongodb://localhost/BlockChain");
        }
    }
}
