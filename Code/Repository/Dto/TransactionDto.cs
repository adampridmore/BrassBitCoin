namespace Repository.Dto
{
    public class TransactionDto
    {
        public TransactionDto(string from, string to, int ammount, int blockIndex)
        {
            this.from = from;
            this.to = to;
            this.ammount = ammount;
            this.blockIndex = blockIndex;
        }

        public string from { get; private set; }
        public string to { get; private set; }
        public int ammount { get; private set; }
        public int blockIndex { get; private set; }
    }
}
