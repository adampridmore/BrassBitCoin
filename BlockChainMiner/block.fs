module Block

type Block = {
    index :int64
    minedBy: string
    data :  string
    previousHash : string
    nonce: int64
}

type BlockWithHash = {
    block: Block;
    hash: string
}
