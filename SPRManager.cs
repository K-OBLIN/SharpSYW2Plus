using System;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// 임진록 2+ 네임스페이스
/// </summary>
namespace SYW2Plus {
    /// <summary>
    /// SPR 관리 클래스
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
        /// You can get the signature
        /// </summary>
        public UInt32 Signature             { get; private set; }
        /// <summary>
        /// You can get the frame width
        /// </summary>
        public UInt32 FrameWidth            { get; private set; }
        /// <summary>
        /// You can get the frame height
        /// </summary>
        public UInt32 FrameHeight           { get; private set; }
        /// <summary>
        /// You can get the number of frame
        /// </summary>
        public UInt32 NumberOfFrame         { get; private set; }
        /// <summary>
        /// You can get the dummy data
        /// </summary>
        public UInt32[] DummyData           { get; private set; }
        /// <summary>
        /// You can get the offset
        /// </summary>
        public UInt32[] Offsets             { get; private set; }
        /// <summary>
        /// You can get the last offset
        /// </summary>
        public UInt32 LastOffset            { get; private set; }
        /// <summary>
        /// You can get the compression size
        /// </summary>
        public UInt16[] CompressionSizes    { get; private set; }
        /// <summary>
        /// You can get the sprite width
        /// </summary>
        public UInt32 SpriteWidth           { get; private set; }
        /// <summary>
        /// You can get the sprite height
        /// </summary>
        public UInt32 SpriteHeight          { get; private set; }
        /// <summary>
        /// You can get the dummy data 2
        /// </summary>
        public UInt32[] DummyData2          { get; private set; }
        /// <summary>
        /// You can get the pixel
        /// </summary>
        public byte[]? Pixels                { get; private set; }
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
            DummyData = new UInt32[SIZE];
            Offsets = new UInt32[SIZE];
            LastOffset = 0;
            CompressionSizes = new UInt16[SIZE];
            SpriteWidth = 0;
            SpriteHeight = 0;
            DummyData2 = new UInt32[SIZE2];
            Pixels = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file_path">The path to the spr file</param>
        public SPRManager(string file_path) {
            Signature = 0;
            FrameWidth = 0;
            FrameHeight = 0;
            NumberOfFrame = 0;
            DummyData = new UInt32[SIZE];
            Offsets = new UInt32[SIZE];
            LastOffset = 0;
            CompressionSizes = new UInt16[SIZE];
            SpriteWidth = 0;
            SpriteHeight = 0;
            DummyData2 = new UInt32[SIZE2];
            Pixels = null;

            if (LoadSPRFile(file_path) == false) {
                throw new FileLoadException("Failed to load the spr file!");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the spr file
        /// </summary>
        /// <param name="file_path">The path to the spr file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool LoadSPRFile(string file_path) {
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

                    // Dummy Data
                    // I think it's ok to pass it.
                    Array.ForEach(DummyData, data => data = br.ReadUInt32());

                    // Offsets
                    Array.ForEach(Offsets, offset => offset = br.ReadUInt32());

                    // Compression Sizes
                    Array.ForEach(CompressionSizes, size => size = br.ReadUInt16());

                    // Last Offset
                    LastOffset = br.ReadUInt32();

                    // Sprite Width, Height
                    SpriteWidth = br.ReadUInt32();
                    SpriteHeight = br.ReadUInt32();

                    // Dummy Data 2
                    Array.ForEach(DummyData2, data => data = br.ReadUInt32());

                    // Pixels
                    Pixels = new byte[FrameWidth * FrameHeight * NumberOfFrame]; // new byte[SpriteWidth * SpriteHeight];
                    for (var i = 0; i < Pixels.Length;) {
                        var data = br.ReadByte();

                        if (data == 0xFE) {
                            var NumberOfRepeat = br.ReadByte();
                            for (var j = 0; j < NumberOfRepeat; ++j) { Pixels[i + j] = data; }

                            i += NumberOfRepeat;
                        } else {
                            Pixels[i] = data;
                            ++i;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Save the spr file
        /// </summary>
        /// <param name="file_path">The path to the spr file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool SaveSPRFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (Pixels == null) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Create, FileAccess.Write)) {
                using (var bw = new BinaryWriter(fs)) {
                    // Signature
                    bw.Write((UInt32)0x09);

                    // Frame Width, Height
                    bw.Write(FrameWidth);
                    bw.Write(FrameHeight);

                    // Number Of Frame
                    bw.Write(NumberOfFrame);

                    // Dummy Data
                    Array.ForEach(DummyData, bw.Write);

                    // Offsets, Compression Sizes, Pixels
                    UInt32 Offset = 0, Size = 0;
                    var Result = new List<byte>();
                    var Temp = new byte[FrameWidth];

                    for (var i = 0; i < Pixels.Length; ++i) {
                        var idx = ((i + 1) / (FrameWidth * FrameHeight)) - 1;

                        // Offsets
                        if ((i + 1) % (FrameWidth * FrameHeight) == 0) {
                            bw.BaseStream.Position = 0x4C0 + (idx << 2);
                            bw.Write(Offset);
                        }

                        Temp[i % FrameWidth] = Pixels[i];

                        if ((i + 1) % FrameWidth == 0) {
                            for (var j = 0; j < Temp.Length; ++j) {
                                var item = Temp[j];

                                if (item == 0xFE) {
                                    var NumberOfRepeat = 1;
                                    var NextItemIdx = j + 1;

                                    while (NextItemIdx < Temp.Length && Temp[NextItemIdx] == 0xFE) {
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

                                bw.BaseStream.Position = 0x970 + (idx << 1);
                                bw.Write((UInt16)(Size & 0xFF));

                                Size = 0;
                            }
                        }
                    }

                    // Last Offset
                    bw.BaseStream.Position = 0xBC8;
                    bw.Write((UInt32)Result.Count);

                    // Sprite Width, Height
                    bw.Write(SpriteWidth);
                    bw.Write(SpriteHeight);

                    // Dummy Data 2
                    Array.ForEach(DummyData2, bw.Write);

                    // Pixels
                    Result.ForEach(pixel => bw.Write(pixel));
                }
            }

            return true;
        }

        /// <summary>
        /// Save the bmp file
        /// </summary>
        /// <param name="file_path">The path to the bmp file</param>
        /// <param name="palette">Color Palette</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool SaveBMPFile(string file_path, ColorPalette palette) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (Pixels == null) { return false; }

            Bitmap bitmap = new Bitmap((Int32)SpriteWidth, (Int32)SpriteHeight, PixelFormat.Format8bppIndexed);
            bitmap.Palette = palette;

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            unsafe {
                var ptr = (byte*)data.Scan0;
                var stride = data.Stride;

                for (var j = 0; j < SpriteHeight / FrameHeight; ++j) {
                    for (var y = 0; y < FrameHeight; ++y) {
                        for (var i = 0; i < SpriteWidth / FrameWidth; ++i) {
                            for (var x = 0; x < FrameWidth; ++x) {
                                ptr[x + (y * stride) + (i * FrameWidth) + (j * FrameWidth * FrameHeight * (SpriteWidth / FrameWidth))] = Pixels[x + (y * (Int32)FrameWidth) + (i * (Int32)FrameWidth * (Int32)FrameHeight) + (j * (Int32)FrameWidth * (Int32)FrameHeight * ((Int32)SpriteWidth / (Int32)FrameWidth))];
                            }
                        }
                    }
                }
            }
            bitmap.UnlockBits(data);
            bitmap.Save(file_path);

            return true;
        }

        /// <summary>
        /// Get the spr file as a bitmap
        /// </summary>
        /// <param name="palette">Color Palette</param>
        /// <returns>Bitmap(true), null(false)</returns>
        public Bitmap? GetBitmap(ColorPalette palette) {
            if (Pixels == null) { return null; }

            Bitmap? bitmap = new Bitmap((Int32)SpriteWidth, (Int32)SpriteHeight, PixelFormat.Format8bppIndexed);
            bitmap.Palette = palette;

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            unsafe {
                var ptr = (byte*)data.Scan0;
                var stride = data.Stride;

                for (var j = 0; j < SpriteHeight / FrameHeight; ++j) {
                    for (var y = 0; y < FrameHeight; ++y) {
                        for (var i = 0; i < SpriteWidth / FrameWidth; ++i) {
                            for (var x = 0; x < FrameWidth; ++x) {
                                ptr[x + (y * stride) + (i * FrameWidth) + (j * FrameWidth * FrameHeight * (SpriteWidth / FrameWidth))] = Pixels[x + (y * (Int32)FrameWidth) + (i * (Int32)FrameWidth * (Int32)FrameHeight) + (j * (Int32)FrameWidth * (Int32)FrameHeight * ((Int32)SpriteWidth / (Int32)FrameWidth))];
                            }
                        }
                    }
                }
            }
            bitmap.UnlockBits(data);

            return bitmap;
        }

        /// <summary>
        /// Convert bmp file to spr file
        /// </summary>
        /// <param name="file_path">The path to the spr file</param>
        /// <param name="bmp">The Bitmap image</param>
        /// <param name="frame_width">The frame width</param>
        /// <param name="frame_height">The frame height</param>
        /// <param name="number_of_frame">The number of frame</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool BitmapToSPR(string file_path, Bitmap? bmp, UInt32 frame_width, UInt32 frame_height, UInt32 number_of_frame) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (bmp == null) { return false; }

            FrameWidth = frame_width;
            FrameHeight = frame_height;
            NumberOfFrame = number_of_frame;
            SpriteWidth = (UInt32)bmp.Width;
            SpriteHeight = (UInt32)bmp.Height;
            Pixels = new byte[FrameWidth * FrameHeight * NumberOfFrame];

            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            unsafe {
                var ptr = (byte*)data.Scan0;
                var stride = data.Stride;

                for (var j = 0; j < SpriteHeight / FrameHeight; ++j) {
                    for (var y = 0; y < FrameHeight; ++y) {
                        for (var i = 0; i < SpriteWidth / FrameWidth; ++i) {
                            for (var x = 0; x < FrameWidth; ++x) {
                                Pixels[x + (y * (Int32)FrameWidth) + (i * (Int32)FrameWidth * (Int32)FrameHeight) + (j * (Int32)FrameWidth * (Int32)FrameHeight * ((Int32)SpriteWidth / (Int32)FrameWidth))] = ptr[x + (y * stride) + (i * FrameWidth) + (j * FrameWidth * FrameHeight * (SpriteWidth / FrameWidth))];
                            }
                        }
                    }
                }
            }
            bmp.UnlockBits(data);

            return SaveSPRFile(file_path);
        }
        #endregion
    }
}
