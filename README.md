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
