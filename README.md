# SharpSPR
Load and Save the SPR file. Only SYW2+(임진록) game.

## 🤔 How to use?!
```csharp
using SYW2Plus; // include this

namespace SPR {
    class Program {
        static void Main(string[] args)
        {
            SPRManager spr = new SPRManager("test.spr");

            // spr.SaveSPRFile("test2.spr");
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
|`UInt32[]`|DummyData|Guessing with dummy data|
|`UInt32[]`|Offsets|The offset datas|
|`UInt32`|LastOffset|The last offset|
|`UInt16[]`|CompressionSizes|Guessing with compression size|
|`UInt32`|SpriteWidth|The sprite width|
|`UInt32`|SpriteHeight|The sprite height|
|`UInt32[]`|DummyData2|Guessing with dummy data|
|`byte[]?`|Pixels|The pixel data|

## YAV file Structure
|Data Type|Name|Desc|
|-----|-----|-----|
|`Int32`|Signature|The signature number|
|`Int16`|AudioFormat|The audio's format|
|`Int16`|AudioChannel|The number of channel|
|`Int32`|SampleRate|Sample Rate|
|`Int32`|AverageBytesPerSecond|Average Bytes Per Second|
|`Int16`|BlockAlign|Block Align|
|`Int16`|BitsPerSample|Bits Per Sample|
|`List<Byte>?`|RawData|PCM Raw Data|
