package io.github.leedscodedojo;

import com.blockchain.Block;
import org.junit.Test;

import static org.hamcrest.CoreMatchers.equalTo;
import static org.hamcrest.CoreMatchers.is;
import static org.junit.Assert.assertThat;

public class MyClassTests {
    @Test
    public void validate_genesis_block() {
        Block genesis = new Block();
        genesis.setIndex(0);
        genesis.setMinedBy("Genesis");
        genesis.setData("Genesis");
        genesis.setPreviousHash("0");
        genesis.setNonce(52458);
        assertThat(genesis.generateHash(), is(equalTo("000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11")));
    }

    @Test
    public void generate_block_after_genesis() {
        Block nextBlock = generateNextBlock(
                0,
                "000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11",
                "Adam",
                "Hello");

        System.out.println(nextBlock.toString());
    }

    @Test
    public void generate_next_block_on_server(){
        Block block = generateNextBlock(
                3,
                "0000D1EF7C103EA81F8191A677F65C2509A743EFBCF64B5894513FBBA4B87C8B",
                "Adam",
                "Hello");

        System.out.println(block);
    }

    private Block generateNextBlock(int previousIndex, String previousHash, String minedBy, String data) {
        Block block = new Block();
        block.setPreviousHash(previousHash);
        block.setIndex(previousIndex + 1);
        block.setMinedBy(minedBy);
        block.setData(data);

        block.setNonce(0);
        block.generateHash();
        for(int nonce = 0 ; !block.isValidHash(); nonce ++){
            block.setNonce(nonce);
            block.generateHash();
        }

        return block;
    }
}
