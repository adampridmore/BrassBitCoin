namespace Repository.Dto
{
    public class TransactionDto
    {
        public TransactionDto(string from, string to, int ammount)
        {
            this.from = from;
            this.to = to;
            this.ammount = ammount;
        }

        public string from { get; private set; }
        public string to { get; private set; }
        public int ammount { get; private set; }
    }
}
