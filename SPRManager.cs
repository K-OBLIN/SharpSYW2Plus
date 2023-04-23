using System;

namespace SPR
{
    /// <summary>
    /// The SPR file management class
    /// </summary>
    class SPRManager {
        #region Constants
        /// <summary>
        /// Array Size 1
        /// </summary>
        public const UInt32 SIZE = 300;
        /// <summary>
        /// Array Size 2
        /// </summary>
        public const UInt32 SIZE2 = 8;            
        #endregion

        #region Properties
        /// <summary>
        /// You can get or set a signature
        /// </summary>
        public UInt32 Signature { get; set; }
        /// <summary>
        /// You can get or set a frame width
        /// </summary>
        public UInt32 FrameWidth { get; set; }
        /// <summary>
        /// You can get or set a frame height
        /// </summary>
        public UInt32 FrameHeight { get; set; }
        /// <summary>
        /// You can get or set a number of frame
        /// </summary>
        public UInt32 NumberOfFrame { get; set; }
        /// <summary>
        /// You can get or set a dummy data
        /// </summary>
        public UInt32[]? DummyDatas { get; set; }
        /// <summary>
        /// You can get or set a offset
        /// </summary>
        public UInt32[]? Offsets { get; set; }
        /// <summary>
        /// You can get or set a last offset
        /// </summary>
        public UInt32 LastOffset { get; set; }
        /// <summary>
        /// You can get or set a compression size
        /// </summary>
        public UInt16[]? CompressionSizes { get; set; }
        /// <summary>
        /// You can get or set a sprite width
        /// </summary>
        public UInt32 SpriteWidth { get; set; }
        /// <summary>
        /// You can get or set a sprite height
        /// </summary>
        public UInt32 SpriteHeight { get; set; }
        /// <summary>
        /// You can get or set a dummy data
        /// </summary>
        public UInt32[]? DummyDatas2 { get; set; }
        /// <summary>
        /// You can get or set a pixel data
        /// </summary>
        public List<byte>? Pixels { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SPRManager() {
            Signature = 0;
            FrameWidth = 0;
            FrameHeight = 0;
            NumberOfFrame = 0;
            DummyDatas = null;
            Offsets = null;
            LastOffset = 0;
            CompressionSizes = null;
            SpriteWidth = 0;
            SpriteHeight = 0;
            DummyDatas2 = null;
            Pixels = null;
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="file_path">The path to the SPR file</param>
        public SPRManager(string file_path) {
            if (LoadFile(file_path) == false) {
                throw new FileLoadException("Failed to load SPR file.");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the SPR file
        /// </summary>
        /// <param name="file_path">The path to the SPR file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool LoadFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (File.Exists(file_path) == false) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Open, FileAccess.Read)) {
                using (var br = new BinaryReader(fs)) {
                    // Signature
                    Signature = br.ReadUInt32();
                    if (Signature != 0x09) { return false; }

                    // Frame Width, Height
                    FrameWidth = br.ReadUInt32();
                    FrameHeight = br.ReadUInt32();

                    // Number Of Frame
                    NumberOfFrame = br.ReadUInt32();

                    // Dummy Datas
                    // I think it's ok to pass it.
                    DummyDatas = DummyDatas ?? new UInt32[SIZE];
                    for (var i = 0; i < SIZE; ++i) { DummyDatas[i] = br.ReadUInt32(); }

                    // Offsets
                    Offsets = Offsets ?? new UInt32[SIZE];
                    for (var i = 0; i < SIZE; ++i) { Offsets[i] = br.ReadUInt32(); }

                    // Compression Sizes
                    CompressionSizes = CompressionSizes ?? new UInt16[SIZE];
                    for (var i = 0; i < SIZE; ++i) { CompressionSizes[i] = br.ReadUInt16(); }

                    // Last Offset
                    LastOffset = br.ReadUInt32();

                    // Sprtie Width, Height
                    SpriteWidth = br.ReadUInt32();
                    SpriteHeight = br.ReadUInt32();

                    // Dummy Datas 2
                    DummyDatas2 = DummyDatas2 ?? new UInt32[SIZE2];
                    for (var i = 0; i < SIZE2; ++i) { DummyDatas2[i] = br.ReadUInt32(); }

                    // Pixels
                    Pixels?.Clear();
                    Pixels = Pixels ?? new List<byte>();
                    while(true) {
                        if (br.BaseStream.Position == br.BaseStream.Length) { break; }

                        var b = br.ReadByte();
                        if (b == 0xFE) {
                            var NumberOfRepeat = br.ReadByte();
                            for (var i = 0; i < NumberOfRepeat; ++i) { Pixels.Add(b); }
                        } else {
                            Pixels.Add(b);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Save the SPR file
        /// </summary>
        /// <param name="file_path">The path to the SPR file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool SaveFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Create, FileAccess.Write)) {
                using (var bw = new BinaryWriter(fs)) {
                    // Signature
                    bw.Write(Signature);

                    // Frame Width, Height
                    bw.Write(FrameWidth);
                    bw.Write(FrameHeight);

                    // Number Of Frame
                    bw.Write(NumberOfFrame);

                    // Dummy Datas
                    if (DummyDatas == null) { throw new NullReferenceException(); }
                    Array.ForEach(DummyDatas, bw.Write);

                    // Offsets, Compression Sizes, Pixels
                    if (Pixels == null) { throw new NullReferenceException(); }
                    UInt32 Offset = 0, Size = 0;
                    List<byte> Result = new List<byte>();
                    List<byte> Temp = new List<byte>();
                    for (var i = 0; i < Pixels.Count; ++i) {
                        // Offsets
                        if ((i + 1) % (FrameWidth * FrameHeight) == 0) {
                            var idx = ((i + 1) / (FrameWidth * FrameHeight)) - 1;
                            bw.BaseStream.Position = 0x4C0 + (idx << 2);
                            bw.Write(Offset);
                        }

                        Temp.Add(Pixels[i]);

                        if ((i + 1) % FrameWidth == 0) {
                            for (var j = 0; j < Temp.Count; ++j) {
                                var item = Temp[j];

                                if (item == 0xFE) {
                                    var NumberOfRepeat = 1;

                                    var NextItemIdx = j + 1;
                                    while (NextItemIdx < Temp.Count && Temp[NextItemIdx] == 0xFE) {
                                        NumberOfRepeat += 1;
                                        NextItemIdx += 1;
                                    }

                                    Result.Add(item);
                                    Result.Add((byte)NumberOfRepeat);

                                    item = Temp[NextItemIdx - 1];
                                    
                                    Size += 2;
                                    j = NextItemIdx - 1;
                                } else {
                                    Result.Add(item);
                                    Size += 1;
                                }
                            }

                            if ((i + 1) % (FrameWidth * FrameHeight) == 0) {
                                Offset += Size;

                                var idx = ((i + 1) / (FrameWidth * FrameHeight)) - 1;
                                bw.BaseStream.Position = 0x970 + (idx << 1);

                                var compression_size = Size & 0xFF;
                                bw.Write((UInt16)compression_size);

                                Size = 0;
                            }

                            Temp.Clear();
                        }
                    }

                    // Last Offset
                    bw.BaseStream.Position = 0xBC8;
                    bw.Write((UInt32)Result.Count);

                    // Sprite Width, Height
                    bw.Write(SpriteWidth);
                    bw.Write(SpriteHeight);

                    // Dummy Datas 2
                    if (DummyDatas2 == null) { throw new NullReferenceException(); }
                    Array.ForEach(DummyDatas2, bw.Write);

                    // Pixels
                    Result.ForEach(item => bw.Write(item));
                }
            }

            return true;
        }
        #endregion
    }
}
