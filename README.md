# Leeds Code Dojo - Mining for BrassBitCoin 

The challenge it to mine and create some Yorkshire-BrassBitCoins.

## Steps:

1. Validate the genesis coin
1. Mine a new coin / block
1. Upload to the first (and only) Yorkshire BrassBitCoin node.
1. Create a coin / block with some transactions.
1. Send Adam some coins!

# Step 1 - Validate the genesis coin
Try and generate the hash for the genesis coin. See _Yorkshire_BrassBitCoin_BlockChain_Specification_ 'First Block (The Genesis Block)' for details on the genesis coin and how to generate the hash.

Hint: Write a function that when passed the block values (in a record type?) returns the hash as a string. Does this hash match the hash in the specification for the genesis block?

# Step 2 - Mine a new coin / block
Try and generate a new coin that has the genesis block hash as the parent hash value. It's a case of trying different _nonce_ values until you fine a valid hash that starts with four 0's. Typically it takes a few thousand attempts. 

_Note_: I'd guess that statistically you'd need to try on average ~32,000. 2 bytes => 2^16 / 2 => 32000

Hint: Call the function you wrote in step 1 with new block values. Use your name as the _minedBy_ value - so we can tell who's mined what. You can put anything in the _data_ value (such as _'hello'_ we're not going to use it yet.) Set the index to 1. Set the _previousHash_ to the hash of the genesis block. 

Try different _nonce_ values until you find one where the hash starts with four zero's. If you find one, congratulations this is a new valid BrassBitCoin! Also you've earnt yourself one new shiny BrasBitCoin coin.

# Step 3 - Upload a coin

Mine and upload the **next** coin on the BlockChain server.

You'll need to find the latest block and mine new coin with that as the parent hash and index. You can get the latest block here:

https://yorkshire-brassbitcoin.azurewebsites.net/Blocks

Upload you coin to the BrassBitCoin server here;

https://yorkshire-brassbitcoin.azurewebsites.net/Blocks/Upload

If it's successful you should now have a coin in your balance here;

https://yorkshire-brassbitcoin.azurewebsites.net/Miners

And be the miner of the last coin at the bottom here;

https://yorkshire-brassbitcoin.azurewebsites.net/Blocks/BlockChain

# Step 4 - Block with Transactions

Now mine another coin, but this time we'll add some transactions to the _data_ field. See the specification for the format of this.

Why not give one of your coins to 'Adam'. I'm sure he'll appreciate it. Set the _from_ = [your miner name]), _to_ = 'Adam' and _amount_ = 1. Upload it, and you should see it in the last block chain, and in the transactions here:

https://yorkshire-brassbitcoin.azurewebsites.net/Blocks/Transactions

# Further steps

### More zeros
 Try settings the difficulty harder (e.g. more zeros). How long does it take to mine?
### Can you create a server / node?
1. Create a list with a valid set of blocks in the blockchain.
1. Calculate the miner balances from the block. Iterate through the blocks giving +1 coin to each miner for each block they have mined.
1. And Apply all the transactions in the blockchain to the balances for each miner.
1. ...
