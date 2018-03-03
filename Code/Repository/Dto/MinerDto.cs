namespace Repository.Dto
{
    public class MinerDto
    {
        public MinerDto(string name, int coinsMined)
        {
            this.Name = name;
            this.CoinsMined = coinsMined;
        }
        public string Name { get; internal set; }
        
        public int CoinsMined { get; internal set; }
    }
}
