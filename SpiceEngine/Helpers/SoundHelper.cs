using OpenTK.Audio.OpenAL;
using SpiceEngine.Sounds;
using System;
using System.IO;

namespace SpiceEngine.Helpers
{
    public static class SoundHelper
    {
        public static Sound ParseFileForSound(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            switch (extension)
            {
                case ".wav":
                    return ParseWaveFile(filePath);
                default:
                    throw new NotImplementedException("Cannot handle file type " + extension);
            }
        }

        private static Sound ParseWaveFile(string filePath)
        {
            // Should probably have a null check...
            var fileStream = File.Open(filePath, FileMode.Open);

            using (var reader = new BinaryReader(fileStream))
            {
                var signature = new string(reader.ReadChars(4));
                if (signature != "RIFF") throw new NotSupportedException("Specified stream is not a wave file.");

                var riffChunkSize = reader.ReadInt32();

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE") throw new NotSupportedException("Specified stream is not a wave file.");

                var formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ") throw new NotSupportedException("Specified wave file is not supported.");

                var formatChunkSize = reader.ReadInt32();
                var audioFormat = reader.ReadInt16();
                var nChannels = reader.ReadInt16();
                var sampleRate = reader.ReadInt32();
                var byteRate = reader.ReadInt32();
                var blockAlign = reader.ReadInt16();
                var nBitsPerSample = reader.ReadInt16();

                var dataSignature = new string(reader.ReadChars(4));
                if (dataSignature != "data") throw new NotSupportedException("Specified wave file is not supported.");

                var dataChunkSize = reader.ReadInt32();
                var data = reader.ReadBytes((int)reader.BaseStream.Length);

                return new Sound(data, GetFormat(nChannels, nBitsPerSample), sampleRate);
            }
        }

        private static ALFormat GetFormat(int nChannels, int nBitsPerSample)
        {
            switch (nChannels)
            {
                case 1:
                    if (nBitsPerSample == 8)
                    {
                        return ALFormat.Mono8;
                    }
                    else
                    {
                        return ALFormat.Mono16;
                    }
                case 2:
                    if (nBitsPerSample == 8)
                    {
                        return ALFormat.Stereo8;
                    }
                    else
                    {
                        return ALFormat.Stereo16;
                    }
                default:
                    throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
