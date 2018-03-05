namespace BlockChainWeb.Models
{
    public class BlockWebModel
    {
        public int index { get; set; }
        public string minedBy { get; set; }
        public string data { get; set; }
        public string previousHash { get; set; }
        public int nonce { get; set; }
        public string hash { get; set; }
    }
}
