using OpenTK.Audio.OpenAL;

namespace SpiceEngine.Sounds
{
    public class Sound
    {
        public int ID { get; set; }

        public byte[] Data { get; private set; }
        public ALFormat Format { get; private set; }
        public int SampleRate { get; private set; }

        //public int ChannelCount { get; set; }
        //public int BitsPerSample { get; set; }

        public Sound(byte[] data, ALFormat format, int sampleRate)
        {
            Data = data;
            Format = format;
            SampleRate = sampleRate;
        }
    }
}
