open System
open System.Security.Cryptography
open System.Numerics;

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

let computeHash (b: byte[]) = b |> HashAlgorithm.Create("SHA256").ComputeHash
let toPositiveBigInteger bytes = BigInteger(Array.concat [ bytes ; [|0x0uy|]])

let hash (content : String) = 
    let bytes = content |> System.Text.ASCIIEncoding.UTF8.GetBytes |> computeHash
    (bytes |> toPositiveBigInteger), ( BitConverter.ToString( bytes ).Replace("-", "") )

let blockHash (block: Block) = 
    [block.index |> string; block.minedBy; block.data; block.previousHash; block.nonce |> string]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let isValidHash hash (previousBlock: BlockWithHash) = hash % ( BigInteger (1000000L) ) = BigInteger.Zero

let newBlock minedBy data (previousBlock: BlockWithHash) = 
    let nonce, _, hash = 
        Seq.initInfinite (fun i -> i |> int64)
        |> Seq.map(fun nonce -> 
            let block = {
                index = (previousBlock.block.index + 1L)
                minedBy = minedBy
                data = data
                nonce = nonce
                previousHash = previousBlock.hash
            }

            let hashValue , hash =  block |> blockHash
            nonce, hashValue, hash)
        |> Seq.where (fun (_, hash, _) -> isValidHash hash previousBlock )
        |> Seq.head
            
    let block = {
        index = previousBlock.block.index + 1L;
        minedBy = minedBy;
        data = data;
        nonce = nonce;
        previousHash = previousBlock.hash;
    }

    {
        block = block;
        hash = hash
    }
  
let genesisBlock =
    let block = {
        index = 1L;
        minedBy = "Genesis"
        data = "Genesis";
        previousHash = "0";
        nonce = 0L;
    }

    { 
        block = block
        hash = (blockHash block) |> snd
    }

let tuple a = (a, a)

let print x = x |> printfn "%A";x
let numbeOfBlocksToGenerate = 500
let blockchain2 =
    Seq.initInfinite id
    |> Seq.take numbeOfBlocksToGenerate
    |> Seq.map string
    |> Seq.toList
    |> Seq.mapFold (fun previousBlock data -> previousBlock |> newBlock "Adam" data |> print |> tuple ) genesisBlock
    |> fst
    |> Seq.toList

//#time "on"
blockchain2
|> Seq.iter (printfn "%A")

