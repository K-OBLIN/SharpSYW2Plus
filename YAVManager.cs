using System;
using NAudio;
using NAudio.Wave;

namespace SYW2Plus {
    /// <summary>
    /// This class manages yav file
    /// </summary>
    class YAVManager {
        #region Properties
        /// <summary>
        /// You can get the signature
        /// </summary>
        public Int32 Signature              { get; private set; }
        /// <summary>
        /// You can get the audio format
        /// </summary>
        public Int16 AudioFormat            { get; private set; }
        /// <summary>
        /// You can get the number of channel
        /// </summary>
        public Int16 AudioChannel           { get; private set; }
        /// <summary>
        /// You can get the sample rate
        /// </summary>
        public Int32 SampleRate             { get; private set; }
        /// <summary>
        /// You can get the average of bytes per second
        /// </summary>
        public Int32 AverageBytesPerSecond  { get; private set; }
        /// <summary>
        /// You can get the block align
        /// </summary>
        public Int16 BlockAlign             { get; private set; }
        /// <summary>
        /// You can get the bits per sample
        /// </summary>
        public Int16 BitsPerSample          { get; private set; }
        /// <summary>
        /// You can get the raw data
        /// </summary>
        public List<byte>? RawData          { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public YAVManager() {
            Signature = 0;
            AudioFormat = 0;
            AudioChannel = 0;
            SampleRate = 0;
            AverageBytesPerSecond = 0;
            BlockAlign = 0;
            BitsPerSample = 0;
            RawData = null;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the yav file
        /// </summary>
        /// <param name="file_path">The path to the yav file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool LoadYAVFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (File.Exists(file_path) == false) { return false; }

            using (var fs = new FileStream(file_path, FileMode.Open, FileAccess.Read)) {
                using (var br = new BinaryReader(fs)) {
                    // Signature
                    Signature = br.ReadInt32();

                    // Audio Format
                    AudioFormat = br.ReadInt16();

                    // Audio Channel
                    AudioChannel = br.ReadInt16();

                    // Sample Rate
                    SampleRate = br.ReadInt32();

                    // Average Bytes Per Second
                    AverageBytesPerSecond = br.ReadInt32();

                    // Block Align
                    BlockAlign = br.ReadInt16();

                    // Bits Per Sample
                    BitsPerSample = br.ReadInt16();

                    // Raw Data
                    RawData = new List<byte>();
                    while (br.BaseStream.Position != br.BaseStream.Length) {
                        RawData.Add(br.ReadByte());
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Save the yav file
        /// </summary>
        /// <param name="file_path">The path to the yav file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool SaveYAVFile(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (RawData == null || RawData.Count == 0) { return false; }    

            using (var fs = new FileStream(file_path, FileMode.Create, FileAccess.Write)) {
                using (var bw = new BinaryWriter(fs)) {
                    // Signature
                    bw.Write((Int32)18);

                    // Audio Format
                    bw.Write(AudioFormat);

                    // Audio Channel
                    bw.Write(AudioChannel);

                    // Sample Rate
                    bw.Write(SampleRate);

                    // Average Bytes Per Second
                    bw.Write(AverageBytesPerSecond);

                    // Block Align
                    bw.Write(BlockAlign);

                    // Bits Per Sample
                    bw.Write(BitsPerSample);

                    // Raw Data
                    RawData.ForEach(data => bw.Write(data));
                }
            }

            return true;
        }

        /// <summary>
        /// Save yav file as a wav file
        /// </summary>
        /// <param name="file_path">The path to the wav file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool YAVToWAV(string file_path) {
            if (string.IsNullOrEmpty(file_path) == true) { return false; }
            if (RawData == null || RawData.Count == 0) { return false; }

            var waveFormat = WaveFormat.CreateCustomFormat((WaveFormatEncoding)AudioFormat, SampleRate, AudioChannel, AverageBytesPerSecond, BlockAlign, BitsPerSample);
            var rs = new RawSourceWaveStream(new MemoryStream(RawData.ToArray()), waveFormat);

            try { WaveFileWriter.CreateWaveFile(file_path, rs); } catch { return false; } finally { rs.Close(); }

            return true;
        }

        /// <summary>
        /// Save yav file as a wav file
        /// </summary>
        /// <param name="yav_file_path">The path to the yav file</param>
        /// <param name="wav_file_path">The path to the wav file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool YAVToWAV(string yav_file_path, string wav_file_path) {
            if (string.IsNullOrEmpty(yav_file_path) == true || string.IsNullOrEmpty(wav_file_path) == true) { return false; }

            return (LoadYAVFile(yav_file_path) == true && YAVToWAV(wav_file_path) == true);
        }
        
        /// <summary>
        /// Save wav file as a yav file
        /// </summary>
        /// <param name="wav_file_path">The path to the mp3 file</param>
        /// <param name="yav_file_path">The path to the yav file</param>
        /// <returns>Successful(true), Failed(false)</returns>
        public bool WAVToYAV(string wav_file_path, string yav_file_path) {
            if (string.IsNullOrEmpty(wav_file_path) == true || string.IsNullOrEmpty(yav_file_path) == true) { return false; }
            if (File.Exists(wav_file_path) == false) { return false; }

            using (var waveReader = new WaveFileReader(wav_file_path)) {
                var waveFormat = waveReader.WaveFormat;
                if (waveFormat == null) {
                    return false;
                }

                AudioFormat = (Int16)waveFormat.Encoding;
                AudioChannel = (Int16)waveFormat.Channels;
                SampleRate = (Int32)waveFormat.SampleRate;
                AverageBytesPerSecond = (Int32)waveFormat.AverageBytesPerSecond;
                BlockAlign = (Int16)waveFormat.BlockAlign;
                BitsPerSample = (Int16)waveFormat.BitsPerSample;

                var buffer = new byte[waveReader.Length];
                var bytesRead = waveReader.Read(buffer, 0, buffer.Length - (buffer.Length % waveFormat.BlockAlign));    
                RawData = new List<byte>(buffer);    
            }

            return SaveYAVFile(yav_file_path);
        }
        #endregion
    }
}
