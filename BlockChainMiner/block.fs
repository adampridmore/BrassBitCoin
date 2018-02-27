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
