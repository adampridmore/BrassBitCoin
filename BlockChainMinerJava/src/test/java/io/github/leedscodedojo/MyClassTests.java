package io.github.leedscodedojo;

import com.blockchain.Block;
import org.junit.Test;

import static org.hamcrest.CoreMatchers.is;
import static org.junit.Assert.assertThat;


/*
        The hash is made by concating all the above fields (exluding the hash) in the above order with a single space between each value (see below).

        The first block (the 'Genesis' block) is defined as:
        index = 1
        minedBy = "Genesis"
        data = "Genesis"
        previousHash = "0";
        nonce = 0
        hash = "F6B82A75721103E570B9952F5A0A872C50665CB919751A7805635D0C6855BBCB"

        So the concatinated block values used to generate the hash are:
        "1 Genesis Genesis 0 0"
        Which generates the following SHA256 hash value (which is converted to text in upper case):
        F6B82A75721103E570B9952F5A0A872C50665CB919751A7805635D0C6855BBCB

        Subsiquent block must meet the following criteria:

        index + 1 of the previous block
        minedBy is the name of the block minder (your name / id)
        data is any arbitary data to store in the block - (this would be any transactions in this block for bitcoin)
        previousHash is the hash of the previous block
        nonce (the nonsense number) a number which can be changed to make a valid hash (see below)
        hash of the block which is also a multiple of 1,000,000.
        e.g. hash % 1,000,000 = 0
        If it is not valid, try another nonce value!

*/

public class MyClassTests {

    @Test
    public void generateGenesis(){
        Block pre = new Block();
        pre.setIndex(-1);
        pre.setHash("0");

        Block genesis = generateNextBlock(pre, "Genesis", "Genesis");

        System.out.println(genesis);
    }

    @Test
    public void mine() {
        Block genesis = new Block();
        genesis.setIndex(0);
        genesis.setMinedBy("Genesis");
        genesis.setData("Genesis");
        genesis.setPreviousHash("0");
        genesis.setNonce(52458);
        genesis.setHash("000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11");
        //System.out.println(genesis.generateHash());

        Block nextBlock = generateNextBlock(genesis, "Adam", "0");
        System.out.println(nextBlock.toString());

    }

    private Block generateNextBlock(Block previousBlock, String minedBy, String data) {
        Block block = new Block();
        block.setPreviousHash(previousBlock.getHash());
        block.setIndex(previousBlock.getIndex() + 1);
        block.setMinedBy(minedBy);
        block.setData(data);

        block.setNonce(0);
        block.generateHash();
        for(int nonce = 1 ; !block.isValidHash(); nonce ++){
            block.setNonce(nonce);
            block.generateHash();
        }

        return block;
    }
}
