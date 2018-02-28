package com.blockchain;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import javax.xml.bind.DatatypeConverter;

public class Block{

    private long index;
    private String minedBy;
    private String data;
    private String previousHash;
    private long nonce;
    private String hash;

    public long getIndex() {
        return index;
    }

    public void setIndex(long index) {
        this.index = index;
    }

    public String getMinedBy() {
        return minedBy;
    }

    public void setMinedBy(String minedBy) {
        this.minedBy = minedBy;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getPreviousHash() {
        return previousHash;
    }

    public void setPreviousHash(String previousHash) {
        this.previousHash = previousHash;
    }

    public long getNonce() {
        return nonce;
    }

    public void setNonce(long nonce) {
        this.nonce = nonce;
    }

    public String getHash() {
        return hash;
    }

    public void setHash(String hash) {
        this.hash = hash;
    }

    public String generateHash() {
        String blockToHash = String.format("%s %s %s %s %s", index, minedBy, data, previousHash, nonce);

        MessageDigest digest = createMessageDigest();
        byte[] hash = digest.digest(blockToHash.getBytes(StandardCharsets.UTF_8));

        this.hash = DatatypeConverter.printHexBinary(hash);
        return this.hash;
    }

    private MessageDigest createMessageDigest() {
        try {
            return MessageDigest.getInstance("SHA-256");
        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException(e);
        }
    }

    public boolean isValidHash() {
        return this.hash.startsWith("0000");
    }

    @Override
    public String toString() {
        return "Block{" +
                "index=" + index +
                ", minedBy='" + minedBy + '\'' +
                ", data='" + data + '\'' +
                ", previousHash='" + previousHash + '\'' +
                ", nonce=" + nonce +
                ", hash='" + hash + '\'' +
                '}';
    }
}
