module BlockChain.Types

type Block = {
    index :int
    minedBy: string
    data :  string
    previousHash : string
    nonce: int
}

type BlockWithHash = {
    block: Block;
    hash: string
}

type IsValidBlock = 
  | Valid 
  | Invalid of (string seq)

type Transaction = {
    from: string;
    ``to`` : string;
    ammount: int
}

type Miner = {
    name: string;
    balance: int;
}