# StegoF5
Library for embedding information in a Bitmap object
## General algorithm for embedding information in BMP image
![Algorithm F5](https://github.com/tom1604/stegof5/raw/master/image/alg.png)
[*Source](https://link.springer.com/chapter/10.1007/978-3-030-30859-9_37)

## Using the library
1. Connect the library to the project
2. Uses the Bitmap extension to embed/extract information

```
    Bitmap bitmap = new Bitmap(20,20);
    var imageWithEmbeddedInformation = bitmap.EmbedInformation(7, 3, new AreaEmbeddingModel(...), new Matrix(...), "TEst");
```

### The library's functionality
* Embed information(Text, Bits Sequence, Image) in Bitmap object;
* Extract information(Text, Image) from Bitmap object;
