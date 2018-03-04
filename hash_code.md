# Code to generate a SHA-256 hash

## Python:
``` Python
import hashlib
hash_object = hashlib.sha256(b'Hello World').hexdigest()
print(hash_object)
```
## DotNet (C# or F#)

``` csharp
var text = "Hello World";
var bytes = System.Text.ASCIIEncoding.UTF8.GetBytes(text);
var hashBytes = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes);
var hashText = System.BitConverter.ToString(hashBytes).Replace("-", "");

Assert.Equal("A591A6D40BF420404A011733CFB7B190D62C65BF0BCDA32B57B277D9AD9F146E", hashText);
```

## Java
``` Java
java.security.MessageDigest digest = java.security.MessageDigest.getInstance("SHA-256");
byte[] hash = digest.digest(blockToHash.getBytes(StandardCharsets.UTF_8));
this.hash = javax.xml.bind.DatatypeConverter.printHexBinary(hash);
```