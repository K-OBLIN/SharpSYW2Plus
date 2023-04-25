using System;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// 임진록 2+ 네임스페이스
/// </summary>
namespace SYW2Plus {
    /// <summary>
    /// PAL 파일 관리 클래스
    /// </summary>
    class PaletteManager {
        #region Properties
        /// <summary>
        /// You can get the file path
        /// </summary>
        public string FilePath                          { get; private set; }
        /// <summary>
        /// You can get the color palette
        /// </summary>
        public ColorPalette? Palette                    { get; private set; }
        /// <summary>
        /// You can get the default color palette
        /// </summary>
        public ColorPalette DefaultColorPalette         { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PaletteManager() {
            FilePath = string.Empty;
            Palette = null;
            InitDefaultColorPalette();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file_path">The path to the pal file</param>
        public PaletteManager(string file_path) {
            InitDefaultColorPalette();

            if (LoadPALFile(file_path) == false) {
                throw new FileNotFoundException("Failed to load the pal file!");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize the default color palette.
        /// </summary>
        private void InitDefaultColorPalette() {
            DefaultColorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
            DefaultColorPalette.Entries[0] = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            DefaultColorPalette.Entries[1] = Color.FromArgb(0xFF, 0x34, 0x5f, 0x2c);
            DefaultColorPalette.Entries[2] = Color.FromArgb(0xFF, 0x34, 0x51, 0x2c);
            DefaultColorPalette.Entries[3] = Color.FromArgb(0xFF, 0x34, 0x4a, 0x3f);
            DefaultColorPalette.Entries[4] = Color.FromArgb(0xFF, 0x2c, 0x3f, 0x37);
            DefaultColorPalette.Entries[5] = Color.FromArgb(0xFF, 0x2c, 0x42, 0x34);
            DefaultColorPalette.Entries[6] = Color.FromArgb(0xFF, 0x2c, 0x42, 0x37);
            DefaultColorPalette.Entries[7] = Color.FromArgb(0xFF, 0x29, 0x54, 0x25);
            DefaultColorPalette.Entries[8] = Color.FromArgb(0xFF, 0x25, 0x4a, 0x29);
            DefaultColorPalette.Entries[9] = Color.FromArgb(0xFF, 0x29, 0x3b, 0x29);
            DefaultColorPalette.Entries[10] = Color.FromArgb(0xFF, 0x25, 0x34, 0x30);
            DefaultColorPalette.Entries[11] = Color.FromArgb(0xFF, 0x25, 0x34, 0x37);
            DefaultColorPalette.Entries[12] = Color.FromArgb(0xFF, 0x21, 0x34, 0x34);
            DefaultColorPalette.Entries[13] = Color.FromArgb(0xFF, 0x21, 0x42, 0x1e);
            DefaultColorPalette.Entries[14] = Color.FromArgb(0xFF, 0x1e, 0x34, 0x1e);
            DefaultColorPalette.Entries[15] = Color.FromArgb(0xFF, 0x1a, 0x34, 0x29);
            DefaultColorPalette.Entries[16] = Color.FromArgb(0xFF, 0xd1, 0xaf, 0x74);
            DefaultColorPalette.Entries[17] = Color.FromArgb(0xFF, 0x88, 0x4d, 0x1e);
            DefaultColorPalette.Entries[18] = Color.FromArgb(0xFF, 0x77, 0x5f, 0x37);
            DefaultColorPalette.Entries[19] = Color.FromArgb(0xFF, 0xa8, 0x88, 0x54);
            DefaultColorPalette.Entries[20] = Color.FromArgb(0xFF, 0x99, 0x7b, 0x4a);
            DefaultColorPalette.Entries[21] = Color.FromArgb(0xFF, 0xbd, 0x9c, 0x5f);
            DefaultColorPalette.Entries[22] = Color.FromArgb(0xFF, 0x4a, 0x3b, 0x13);
            DefaultColorPalette.Entries[23] = Color.FromArgb(0xFF, 0x4d, 0x3f, 0x16);
            DefaultColorPalette.Entries[24] = Color.FromArgb(0xFF, 0x3b, 0x29, 0x0b);
            DefaultColorPalette.Entries[25] = Color.FromArgb(0xFF, 0x2c, 0x4d, 0x2c);
            DefaultColorPalette.Entries[26] = Color.FromArgb(0xFF, 0x42, 0x74, 0x37);
            DefaultColorPalette.Entries[27] = Color.FromArgb(0xFF, 0x42, 0x74, 0x34);
            DefaultColorPalette.Entries[28] = Color.FromArgb(0xFF, 0x42, 0x6a, 0x3b);
            DefaultColorPalette.Entries[29] = Color.FromArgb(0xFF, 0x3f, 0x66, 0x3b);
            DefaultColorPalette.Entries[30] = Color.FromArgb(0xFF, 0x3b, 0x5b, 0x34);
            DefaultColorPalette.Entries[31] = Color.FromArgb(0xFF, 0x37, 0x5f, 0x30);
            DefaultColorPalette.Entries[32] = Color.FromArgb(0xFF, 0xd1, 0x9c, 0x6d);
            DefaultColorPalette.Entries[33] = Color.FromArgb(0xFF, 0xb2, 0x77, 0x1a);
            DefaultColorPalette.Entries[34] = Color.FromArgb(0xFF, 0xd1, 0xbd, 0xb2);
            DefaultColorPalette.Entries[35] = Color.FromArgb(0xFF, 0xc3, 0x95, 0x21);
            DefaultColorPalette.Entries[36] = Color.FromArgb(0xFF, 0xb8, 0x7e, 0x16);
            DefaultColorPalette.Entries[37] = Color.FromArgb(0xFF, 0xc9, 0xac, 0x70);
            DefaultColorPalette.Entries[38] = Color.FromArgb(0xFF, 0xcc, 0xbd, 0xb5);
            DefaultColorPalette.Entries[39] = Color.FromArgb(0xFF, 0xc3, 0x9f, 0x51);
            DefaultColorPalette.Entries[40] = Color.FromArgb(0xFF, 0xb8, 0x74, 0x13);
            DefaultColorPalette.Entries[41] = Color.FromArgb(0xFF, 0xc0, 0x8f, 0x1a);
            DefaultColorPalette.Entries[42] = Color.FromArgb(0xFF, 0xc0, 0x99, 0x46);
            DefaultColorPalette.Entries[43] = Color.FromArgb(0xFF, 0xcc, 0xb5, 0x92);
            DefaultColorPalette.Entries[44] = Color.FromArgb(0xFF, 0xb2, 0x66, 0x0b);
            DefaultColorPalette.Entries[45] = Color.FromArgb(0xFF, 0xac, 0x51, 0x07);
            DefaultColorPalette.Entries[46] = Color.FromArgb(0xFF, 0x99, 0x30, 0x04);
            DefaultColorPalette.Entries[47] = Color.FromArgb(0xFF, 0x8f, 0x21, 0x04);
            DefaultColorPalette.Entries[48] = Color.FromArgb(0xFF, 0x77, 0x0b, 0x00);
            DefaultColorPalette.Entries[49] = Color.FromArgb(0xFF, 0x85, 0x7e, 0x70);
            DefaultColorPalette.Entries[50] = Color.FromArgb(0xFF, 0x88, 0x77, 0x63);
            DefaultColorPalette.Entries[51] = Color.FromArgb(0xFF, 0xb2, 0xa8, 0x9c);
            DefaultColorPalette.Entries[52] = Color.FromArgb(0xFF, 0x8c, 0x82, 0x70);
            DefaultColorPalette.Entries[53] = Color.FromArgb(0xFF, 0x9f, 0x95, 0x85);
            DefaultColorPalette.Entries[54] = Color.FromArgb(0xFF, 0xb2, 0xa8, 0x9c);
            DefaultColorPalette.Entries[55] = Color.FromArgb(0xFF, 0xbb, 0xb8, 0xaf);
            DefaultColorPalette.Entries[56] = Color.FromArgb(0xFF, 0x92, 0x8f, 0x82);
            DefaultColorPalette.Entries[57] = Color.FromArgb(0xFF, 0x99, 0x92, 0x88);
            DefaultColorPalette.Entries[58] = Color.FromArgb(0xFF, 0xbb, 0xaf, 0x9f);
            DefaultColorPalette.Entries[59] = Color.FromArgb(0xFF, 0x8c, 0x7b, 0x6a);
            DefaultColorPalette.Entries[60] = Color.FromArgb(0xFF, 0x8f, 0x82, 0x6d);
            DefaultColorPalette.Entries[61] = Color.FromArgb(0xFF, 0x37, 0x3b, 0x66);
            DefaultColorPalette.Entries[62] = Color.FromArgb(0xFF, 0x34, 0x37, 0x58);
            DefaultColorPalette.Entries[63] = Color.FromArgb(0xFF, 0x2c, 0x30, 0x51);
            DefaultColorPalette.Entries[64] = Color.FromArgb(0xFF, 0x92, 0x8c, 0x7b);
            DefaultColorPalette.Entries[65] = Color.FromArgb(0xFF, 0x3b, 0x30, 0x30);
            DefaultColorPalette.Entries[66] = Color.FromArgb(0xFF, 0x29, 0x16, 0x07);
            DefaultColorPalette.Entries[67] = Color.FromArgb(0xFF, 0x5f, 0x54, 0x4d);
            DefaultColorPalette.Entries[68] = Color.FromArgb(0xFF, 0x00, 0xaf, 0x00);
            DefaultColorPalette.Entries[69] = Color.FromArgb(0xFF, 0xaf, 0x00, 0x00);
            DefaultColorPalette.Entries[70] = Color.FromArgb(0xFF, 0xaf, 0xaf, 0x00);
            DefaultColorPalette.Entries[71] = Color.FromArgb(0xFF, 0x63, 0x58, 0x4d);
            DefaultColorPalette.Entries[72] = Color.FromArgb(0xFF, 0x3f, 0x37, 0x25);
            DefaultColorPalette.Entries[73] = Color.FromArgb(0xFF, 0x5b, 0x58, 0x4a);
            DefaultColorPalette.Entries[74] = Color.FromArgb(0xFF, 0x4d, 0x4a, 0x21);
            DefaultColorPalette.Entries[75] = Color.FromArgb(0xFF, 0x58, 0x46, 0x25);
            DefaultColorPalette.Entries[76] = Color.FromArgb(0xFF, 0x9c, 0x88, 0x6d);
            DefaultColorPalette.Entries[77] = Color.FromArgb(0xFF, 0x82, 0x74, 0x51);
            DefaultColorPalette.Entries[78] = Color.FromArgb(0xFF, 0x74, 0x63, 0x42);
            DefaultColorPalette.Entries[79] = Color.FromArgb(0xFF, 0x6d, 0x5f, 0x3f);
            DefaultColorPalette.Entries[80] = Color.FromArgb(0xFF, 0x8f, 0x7e, 0x5f);
            DefaultColorPalette.Entries[81] = Color.FromArgb(0xFF, 0x54, 0x4a, 0x37);
            DefaultColorPalette.Entries[82] = Color.FromArgb(0xFF, 0x6d, 0x63, 0x54);
            DefaultColorPalette.Entries[83] = Color.FromArgb(0xFF, 0x7b, 0x70, 0x5f);
            DefaultColorPalette.Entries[84] = Color.FromArgb(0xFF, 0x4d, 0x4a, 0x3b);
            DefaultColorPalette.Entries[85] = Color.FromArgb(0xFF, 0x77, 0x6d, 0x63);
            DefaultColorPalette.Entries[86] = Color.FromArgb(0xFF, 0x7e, 0x6a, 0x66);
            DefaultColorPalette.Entries[87] = Color.FromArgb(0xFF, 0x70, 0x6a, 0x5f);
            DefaultColorPalette.Entries[88] = Color.FromArgb(0xFF, 0x6a, 0x63, 0x58);
            DefaultColorPalette.Entries[89] = Color.FromArgb(0xFF, 0x3b, 0x37, 0x29);
            DefaultColorPalette.Entries[90] = Color.FromArgb(0xFF, 0x5b, 0x54, 0x42);
            DefaultColorPalette.Entries[91] = Color.FromArgb(0xFF, 0x4a, 0x46, 0x3b);
            DefaultColorPalette.Entries[92] = Color.FromArgb(0xFF, 0x6a, 0x63, 0x51);
            DefaultColorPalette.Entries[93] = Color.FromArgb(0xFF, 0x70, 0x6a, 0x5b);
            DefaultColorPalette.Entries[94] = Color.FromArgb(0xFF, 0x4d, 0x42, 0x30);
            DefaultColorPalette.Entries[95] = Color.FromArgb(0xFF, 0x42, 0x3b, 0x29);
            DefaultColorPalette.Entries[96] = Color.FromArgb(0xFF, 0x5f, 0x54, 0x42);
            DefaultColorPalette.Entries[97] = Color.FromArgb(0xFF, 0x42, 0x1e, 0x1a);
            DefaultColorPalette.Entries[98] = Color.FromArgb(0xFF, 0x46, 0x34, 0x13);
            DefaultColorPalette.Entries[99] = Color.FromArgb(0xFF, 0x54, 0x42, 0x1a);
            DefaultColorPalette.Entries[100] = Color.FromArgb(0xFF, 0x66, 0x42, 0x3b);
            DefaultColorPalette.Entries[101] = Color.FromArgb(0xFF, 0x51, 0x34, 0x2c);
            DefaultColorPalette.Entries[102] = Color.FromArgb(0xFF, 0x82, 0x58, 0x4d);
            DefaultColorPalette.Entries[103] = Color.FromArgb(0xFF, 0x74, 0x58, 0x30);
            DefaultColorPalette.Entries[104] = Color.FromArgb(0xFF, 0x85, 0x46, 0x1e);
            DefaultColorPalette.Entries[105] = Color.FromArgb(0xFF, 0x8f, 0x6d, 0x30);
            DefaultColorPalette.Entries[106] = Color.FromArgb(0xFF, 0x85, 0x5f, 0x29);
            DefaultColorPalette.Entries[107] = Color.FromArgb(0xFF, 0x99, 0x5b, 0x25);
            DefaultColorPalette.Entries[108] = Color.FromArgb(0xFF, 0x6a, 0x3b, 0x1a);
            DefaultColorPalette.Entries[109] = Color.FromArgb(0xFF, 0x66, 0x3b, 0x1a);
            DefaultColorPalette.Entries[110] = Color.FromArgb(0xFF, 0x70, 0x37, 0x1a);
            DefaultColorPalette.Entries[111] = Color.FromArgb(0xFF, 0x5f, 0x37, 0x16);
            DefaultColorPalette.Entries[112] = Color.FromArgb(0xFF, 0x77, 0x46, 0x1a);
            DefaultColorPalette.Entries[113] = Color.FromArgb(0xFF, 0x66, 0x13, 0x13);
            DefaultColorPalette.Entries[114] = Color.FromArgb(0xFF, 0x4d, 0x25, 0x1e);
            DefaultColorPalette.Entries[115] = Color.FromArgb(0xFF, 0xce, 0xb5, 0x99);
            DefaultColorPalette.Entries[116] = Color.FromArgb(0xFF, 0x5b, 0x30, 0x25);
            DefaultColorPalette.Entries[117] = Color.FromArgb(0xFF, 0x6a, 0x3b, 0x2c);
            DefaultColorPalette.Entries[118] = Color.FromArgb(0xFF, 0x70, 0x3f, 0x30);
            DefaultColorPalette.Entries[119] = Color.FromArgb(0xFF, 0x77, 0x42, 0x34);
            DefaultColorPalette.Entries[120] = Color.FromArgb(0xFF, 0x85, 0x4d, 0x51);
            DefaultColorPalette.Entries[121] = Color.FromArgb(0xFF, 0x8c, 0x51, 0x42);
            DefaultColorPalette.Entries[122] = Color.FromArgb(0xFF, 0x92, 0x58, 0x4d);
            DefaultColorPalette.Entries[123] = Color.FromArgb(0xFF, 0x99, 0x63, 0x4d);
            DefaultColorPalette.Entries[124] = Color.FromArgb(0xFF, 0xce, 0xc0, 0x9f);
            DefaultColorPalette.Entries[125] = Color.FromArgb(0xFF, 0xa8, 0x74, 0x5b);
            DefaultColorPalette.Entries[126] = Color.FromArgb(0xFF, 0xb5, 0x85, 0x6a);
            DefaultColorPalette.Entries[127] = Color.FromArgb(0xFF, 0xb8, 0x8c, 0x70);
            DefaultColorPalette.Entries[128] = Color.FromArgb(0xFF, 0x66, 0x8c, 0x8c);
            DefaultColorPalette.Entries[129] = Color.FromArgb(0xFF, 0x5f, 0x82, 0x7e);
            DefaultColorPalette.Entries[130] = Color.FromArgb(0xFF, 0x54, 0x70, 0x77);
            DefaultColorPalette.Entries[131] = Color.FromArgb(0xFF, 0x51, 0x77, 0x70);
            DefaultColorPalette.Entries[132] = Color.FromArgb(0xFF, 0x4d, 0x66, 0x6d);
            DefaultColorPalette.Entries[133] = Color.FromArgb(0xFF, 0x4a, 0x6a, 0x70);
            DefaultColorPalette.Entries[134] = Color.FromArgb(0xFF, 0x42, 0x66, 0x66);
            DefaultColorPalette.Entries[135] = Color.FromArgb(0xFF, 0x4a, 0x6a, 0x66);
            DefaultColorPalette.Entries[136] = Color.FromArgb(0xFF, 0x4a, 0x58, 0x7e);
            DefaultColorPalette.Entries[137] = Color.FromArgb(0xFF, 0x3f, 0x4a, 0x70);
            DefaultColorPalette.Entries[138] = Color.FromArgb(0xFF, 0x3f, 0x63, 0x5f);
            DefaultColorPalette.Entries[139] = Color.FromArgb(0xFF, 0x3f, 0x58, 0x5f);
            DefaultColorPalette.Entries[140] = Color.FromArgb(0xFF, 0x37, 0x42, 0x66);
            DefaultColorPalette.Entries[141] = Color.FromArgb(0xFF, 0x34, 0x4a, 0x5b);
            DefaultColorPalette.Entries[142] = Color.FromArgb(0xFF, 0x30, 0x37, 0x5b);
            DefaultColorPalette.Entries[143] = Color.FromArgb(0xFF, 0x25, 0x2c, 0x46);
            DefaultColorPalette.Entries[144] = Color.FromArgb(0xFF, 0xb5, 0xc0, 0xcc);
            DefaultColorPalette.Entries[145] = Color.FromArgb(0xFF, 0x34, 0x42, 0x5f);
            DefaultColorPalette.Entries[146] = Color.FromArgb(0xFF, 0x30, 0x3f, 0x5b);
            DefaultColorPalette.Entries[147] = Color.FromArgb(0xFF, 0x77, 0x5f, 0x51);
            DefaultColorPalette.Entries[148] = Color.FromArgb(0xFF, 0x58, 0x4a, 0x3b);
            DefaultColorPalette.Entries[149] = Color.FromArgb(0xFF, 0x51, 0x42, 0x34);
            DefaultColorPalette.Entries[150] = Color.FromArgb(0xFF, 0x58, 0x46, 0x37);
            DefaultColorPalette.Entries[151] = Color.FromArgb(0xFF, 0xaf, 0x99, 0x4d);
            DefaultColorPalette.Entries[152] = Color.FromArgb(0xFF, 0xac, 0x95, 0x4a);
            DefaultColorPalette.Entries[153] = Color.FromArgb(0xFF, 0x63, 0x54, 0x13);
            DefaultColorPalette.Entries[154] = Color.FromArgb(0xFF, 0x7b, 0x6a, 0x13);
            DefaultColorPalette.Entries[155] = Color.FromArgb(0xFF, 0x85, 0x70, 0x13);
            DefaultColorPalette.Entries[156] = Color.FromArgb(0xFF, 0x54, 0x5f, 0x5f);
            DefaultColorPalette.Entries[157] = Color.FromArgb(0xFF, 0x3f, 0x4a, 0x4a);
            DefaultColorPalette.Entries[158] = Color.FromArgb(0xFF, 0x85, 0xb5, 0xaf);
            DefaultColorPalette.Entries[159] = Color.FromArgb(0xFF, 0x70, 0x9c, 0x9c);
            DefaultColorPalette.Entries[160] = Color.FromArgb(0xFF, 0x4a, 0x51, 0x58);
            DefaultColorPalette.Entries[161] = Color.FromArgb(0xFF, 0x7b, 0x29, 0x1a);
            DefaultColorPalette.Entries[162] = Color.FromArgb(0xFF, 0x88, 0x34, 0x21);
            DefaultColorPalette.Entries[163] = Color.FromArgb(0xFF, 0x9c, 0x42, 0x29);
            DefaultColorPalette.Entries[164] = Color.FromArgb(0xFF, 0xac, 0x4d, 0x34);
            DefaultColorPalette.Entries[165] = Color.FromArgb(0xFF, 0xbb, 0x63, 0x3f);
            DefaultColorPalette.Entries[166] = Color.FromArgb(0xFF, 0xc9, 0x85, 0x58);
            DefaultColorPalette.Entries[167] = Color.FromArgb(0xFF, 0x82, 0x2c, 0x1e);
            DefaultColorPalette.Entries[168] = Color.FromArgb(0xFF, 0x92, 0x3b, 0x25);
            DefaultColorPalette.Entries[169] = Color.FromArgb(0xFF, 0xa2, 0x46, 0x2c);
            DefaultColorPalette.Entries[170] = Color.FromArgb(0xFF, 0xb5, 0x5b, 0x3b);
            DefaultColorPalette.Entries[171] = Color.FromArgb(0xFF, 0xc3, 0x70, 0x4a);
            DefaultColorPalette.Entries[172] = Color.FromArgb(0xFF, 0xd1, 0xa5, 0x6d);
            DefaultColorPalette.Entries[173] = Color.FromArgb(0xFF, 0xd4, 0xd4, 0xb8);
            DefaultColorPalette.Entries[174] = Color.FromArgb(0xFF, 0xd4, 0xc9, 0x9f);
            DefaultColorPalette.Entries[175] = Color.FromArgb(0xFF, 0xd4, 0xc0, 0x92);
            DefaultColorPalette.Entries[176] = Color.FromArgb(0xFF, 0xd4, 0xb5, 0x88);
            DefaultColorPalette.Entries[177] = Color.FromArgb(0xFF, 0xd4, 0xaf, 0x82);
            DefaultColorPalette.Entries[178] = Color.FromArgb(0xFF, 0xd4, 0xc6, 0x95);
            DefaultColorPalette.Entries[179] = Color.FromArgb(0xFF, 0xc9, 0x7b, 0x51);
            DefaultColorPalette.Entries[180] = Color.FromArgb(0xFF, 0xce, 0x8c, 0x5f);
            DefaultColorPalette.Entries[181] = Color.FromArgb(0xFF, 0xd1, 0xa5, 0x74);
            DefaultColorPalette.Entries[182] = Color.FromArgb(0xFF, 0xd1, 0xaf, 0x74);
            DefaultColorPalette.Entries[183] = Color.FromArgb(0xFF, 0xd1, 0x9c, 0x6d);
            DefaultColorPalette.Entries[184] = Color.FromArgb(0xFF, 0x34, 0x51, 0x37);
            DefaultColorPalette.Entries[185] = Color.FromArgb(0xFF, 0x51, 0x25, 0x04);
            DefaultColorPalette.Entries[186] = Color.FromArgb(0xFF, 0x4a, 0x21, 0x04);
            DefaultColorPalette.Entries[187] = Color.FromArgb(0xFF, 0x42, 0x1e, 0x00);
            DefaultColorPalette.Entries[188] = Color.FromArgb(0xFF, 0x37, 0x1a, 0x00);
            DefaultColorPalette.Entries[189] = Color.FromArgb(0xFF, 0x34, 0x16, 0x00);
            DefaultColorPalette.Entries[190] = Color.FromArgb(0xFF, 0x2c, 0x13, 0x00);
            DefaultColorPalette.Entries[191] = Color.FromArgb(0xFF, 0x1e, 0x0b, 0x00);
            DefaultColorPalette.Entries[192] = Color.FromArgb(0xFF, 0x63, 0x2c, 0x04);
            DefaultColorPalette.Entries[193] = Color.FromArgb(0xFF, 0x51, 0x66, 0x85);
            DefaultColorPalette.Entries[194] = Color.FromArgb(0xFF, 0x4a, 0x58, 0x7b);
            DefaultColorPalette.Entries[195] = Color.FromArgb(0xFF, 0x46, 0x6d, 0x66);
            DefaultColorPalette.Entries[196] = Color.FromArgb(0xFF, 0x42, 0x51, 0x74);
            DefaultColorPalette.Entries[197] = Color.FromArgb(0xFF, 0x3b, 0x46, 0x66);
            DefaultColorPalette.Entries[198] = Color.FromArgb(0xFF, 0x37, 0x4d, 0x5b);
            DefaultColorPalette.Entries[199] = Color.FromArgb(0xFF, 0x2c, 0x37, 0x54);
            DefaultColorPalette.Entries[200] = Color.FromArgb(0xFF, 0x25, 0x2c, 0x4d);
            DefaultColorPalette.Entries[201] = Color.FromArgb(0xFF, 0x1e, 0x21, 0x46);
            DefaultColorPalette.Entries[202] = Color.FromArgb(0xFF, 0x1a, 0x1e, 0x42);
            DefaultColorPalette.Entries[203] = Color.FromArgb(0xFF, 0x1a, 0x1e, 0x42);
            DefaultColorPalette.Entries[204] = Color.FromArgb(0xFF, 0x16, 0x1a, 0x3b);
            DefaultColorPalette.Entries[205] = Color.FromArgb(0xFF, 0x16, 0x16, 0x37);
            DefaultColorPalette.Entries[206] = Color.FromArgb(0xFF, 0x0f, 0x0f, 0x34);
            DefaultColorPalette.Entries[207] = Color.FromArgb(0xFF, 0x58, 0x29, 0x04);
            DefaultColorPalette.Entries[208] = Color.FromArgb(0xFF, 0x25, 0x25, 0x25);
            DefaultColorPalette.Entries[209] = Color.FromArgb(0xFF, 0x46, 0x46, 0x46);
            DefaultColorPalette.Entries[210] = Color.FromArgb(0xFF, 0x6d, 0x6d, 0x6d);
            DefaultColorPalette.Entries[211] = Color.FromArgb(0xFF, 0x8f, 0x8f, 0x8f);
            DefaultColorPalette.Entries[212] = Color.FromArgb(0xFF, 0x25, 0x13, 0x00);
            DefaultColorPalette.Entries[213] = Color.FromArgb(0xFF, 0x46, 0x2c, 0x00);
            DefaultColorPalette.Entries[214] = Color.FromArgb(0xFF, 0x6d, 0x4d, 0x00);
            DefaultColorPalette.Entries[215] = Color.FromArgb(0xFF, 0x8f, 0x5f, 0x00);
            DefaultColorPalette.Entries[216] = Color.FromArgb(0xFF, 0x25, 0x00, 0x25);
            DefaultColorPalette.Entries[217] = Color.FromArgb(0xFF, 0x3f, 0x00, 0x3f);
            DefaultColorPalette.Entries[218] = Color.FromArgb(0xFF, 0x5b, 0x00, 0x5b);
            DefaultColorPalette.Entries[219] = Color.FromArgb(0xFF, 0x77, 0x00, 0x77);
            DefaultColorPalette.Entries[220] = Color.FromArgb(0xFF, 0x00, 0x25, 0x25);
            DefaultColorPalette.Entries[221] = Color.FromArgb(0xFF, 0x00, 0x3f, 0x3f);
            DefaultColorPalette.Entries[222] = Color.FromArgb(0xFF, 0x00, 0x5b, 0x5b);
            DefaultColorPalette.Entries[223] = Color.FromArgb(0xFF, 0x00, 0x77, 0x77);
            DefaultColorPalette.Entries[224] = Color.FromArgb(0xFF, 0x25, 0x25, 0x00);
            DefaultColorPalette.Entries[225] = Color.FromArgb(0xFF, 0x46, 0x46, 0x00);
            DefaultColorPalette.Entries[226] = Color.FromArgb(0xFF, 0x6d, 0x6d, 0x00);
            DefaultColorPalette.Entries[227] = Color.FromArgb(0xFF, 0x8f, 0x8f, 0x00);
            DefaultColorPalette.Entries[228] = Color.FromArgb(0xFF, 0x00, 0x25, 0x00);
            DefaultColorPalette.Entries[229] = Color.FromArgb(0xFF, 0x00, 0x3f, 0x00);
            DefaultColorPalette.Entries[230] = Color.FromArgb(0xFF, 0x00, 0x5b, 0x00);
            DefaultColorPalette.Entries[231] = Color.FromArgb(0xFF, 0x00, 0x77, 0x00);
            DefaultColorPalette.Entries[232] = Color.FromArgb(0xFF, 0x25, 0x00, 0x00);
            DefaultColorPalette.Entries[233] = Color.FromArgb(0xFF, 0x4a, 0x00, 0x00);
            DefaultColorPalette.Entries[234] = Color.FromArgb(0xFF, 0x6d, 0x00, 0x00);
            DefaultColorPalette.Entries[235] = Color.FromArgb(0xFF, 0x8f, 0x00, 0x00);
            DefaultColorPalette.Entries[236] = Color.FromArgb(0xFF, 0x00, 0x00, 0x25);
            DefaultColorPalette.Entries[237] = Color.FromArgb(0xFF, 0x00, 0x00, 0x4a);
            DefaultColorPalette.Entries[238] = Color.FromArgb(0xFF, 0x00, 0x00, 0x6d);
            DefaultColorPalette.Entries[239] = Color.FromArgb(0xFF, 0x00, 0x00, 0x8f);
            DefaultColorPalette.Entries[240] = Color.FromArgb(0xFF, 0xc9, 0xc9, 0xc9);
            DefaultColorPalette.Entries[241] = Color.FromArgb(0xFF, 0x0f, 0x0f, 0x0f);
            DefaultColorPalette.Entries[242] = Color.FromArgb(0xFF, 0x1e, 0x1e, 0x1e);
            DefaultColorPalette.Entries[243] = Color.FromArgb(0xFF, 0x2c, 0x2c, 0x2c);
            DefaultColorPalette.Entries[244] = Color.FromArgb(0xFF, 0x3f, 0x3f, 0x3f);
            DefaultColorPalette.Entries[245] = Color.FromArgb(0xFF, 0x4d, 0x4d, 0x4d);
            DefaultColorPalette.Entries[246] = Color.FromArgb(0xFF, 0x5b, 0x5b, 0x5b);
            DefaultColorPalette.Entries[247] = Color.FromArgb(0xFF, 0x6a, 0x6a, 0x6a);
            DefaultColorPalette.Entries[248] = Color.FromArgb(0xFF, 0x7b, 0x7b, 0x7b);
            DefaultColorPalette.Entries[249] = Color.FromArgb(0xFF, 0x88, 0x88, 0x88);
            DefaultColorPalette.Entries[250] = Color.FromArgb(0xFF, 0x95, 0x95, 0x95);
            DefaultColorPalette.Entries[251] = Color.FromArgb(0xFF, 0xa2, 0xa2, 0xa2);
            DefaultColorPalette.Entries[252] = Color.FromArgb(0xFF, 0xb2, 0xb2, 0xb2);
            DefaultColorPalette.Entries[253] = Color.FromArgb(0xFF, 0xbd, 0xbd, 0xbd);
            DefaultColorPalette.Entries[254] = Color.FromArgb(0xFF, 0xd4, 0xc0, 0xd4);
            DefaultColorPalette.Entries[255] = Color.FromArgb(0xFF, 0xd4, 0xd4, 0xd4);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the pal file
        /// </summary>
        /// <param name="file_path">The path to the pal file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool LoadPALFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (File.Exists(file_path) == false) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Open, FileAccess.Read)) {
                using (var br = new BinaryReader(fs)) {
                    if (br.BaseStream.Length > 0x300) { return false; }

                    FilePath = file_path;
                    Palette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;

                    for (var i = 0; i < Palette.Entries.Length; ++i) {
                        Palette.Entries[i] = Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Save the pal file
        /// </summary>
        /// <param name="file_path">The path to the pal file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool SavePALFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (Palette == null) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Create, FileAccess.Write)) {
                using (var bw = new BinaryWriter(fs)) {
                    foreach (var color in Palette.Entries) { 
                        bw.Write(color.R); 
                        bw.Write(color.G); 
                        bw.Write(color.B); 
                    }
                }
            }

            return true;
        }
        #endregion
    }
}