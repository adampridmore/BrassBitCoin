namespace BlockChainWeb.Models
{
    public class PostUploadBlock
    {
        public string index { get; set; }
        public string minedBy { get; set; }
        public string data { get; set; }
        public string previousHash { get; set; }
        public string nonce { get; set; }
        public string hash { get; set; }
    }
}
