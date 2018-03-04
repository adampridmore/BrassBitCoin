namespace Repository.Dto
{
    public class TransactionDto
    {
        public TransactionDto(string from, string to, int amount, int blockIndex)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.blockIndex = blockIndex;
        }

        public string from { get; private set; }
        public string to { get; private set; }
        public int amount { get; private set; }
        public int blockIndex { get; private set; }
    }
}
