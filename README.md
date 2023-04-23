# SharpSPR
Load and Save the SPR file. Only ESL(임진록) game.

## 🤔 How to use?!
```csharp
using SPR; // include this

namespace SPR {
    class Program {
        static void Main(string[] args)
        {
            SPRManager spr = new SPRManager("test.spr");

            // spr.SaveFile("test2.spr");
            System.Console.WriteLine(spr.Signature);
        }
    }
}
```
RESULT : `9`

## SPR file Structure
|Data Type|Name|Desc|
|-----|-----|-----|
|`UInt32`|Signature|The signature number|
|`UInt32`|FrameWidth|The frame width|
|`UInt32`|FrameHeight|The frame height|
|`UInt32`|NumberOfFrame|The number of frame|
|`UInt32[]`|DummyDatas|Guessing with dummy data|
|`UInt32[]`|Offsets|The offset datas|
|`UInt32`|LastOffset|The last offset|
|`UInt16[]`|CompressionSizes|Guessing with compression size|
|`UInt32`|SpriteWidth|The sprite width|
|`UInt32`|SpriteHeight|The sprite height|
|`List<byte>`|Pixels|The pixel data|
