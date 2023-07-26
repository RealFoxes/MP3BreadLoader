using MP3BreadLoader.Core.Models;
using NAudio.Wave;
using NAudio.Vorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MP3BreadLoader.Core.Services
{
	public class AudioHandlerService
	{
        private const int refresh = 23;
		public AudioHandlerService()
		{
            
        }
        public FrequenciesDataModel ProcessOggFile(byte[] fileBytes)
        {
            var frequenciesChannels = new List<FrequenciesChannelsModel>();
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                using (VorbisWaveReader reader = new VorbisWaveReader(memoryStream))
                {
                    int sampleRate = reader.WaveFormat.SampleRate;
                    int bytesPerFrame = reader.WaveFormat.Channels * reader.WaveFormat.BitsPerSample / 8;

                    int frameBytes = (int)((refresh / 1000.0) * sampleRate) * bytesPerFrame;

                    int peakValue = 0;

                    while (reader.Position < reader.Length)
                    {
                        byte[] frameData = new byte[frameBytes];
                        int bytesRead = reader.Read(frameData, 0, frameBytes);

                        if (bytesRead <= 0)
                            break;

                        int[] leftArr = new int[32];
                        int[] rightArr = new int[32];

                        for (int i = 0; i < 32; i++)
                        {
                            float leftValue = BitConverter.ToSingle(frameData, i * bytesPerFrame);
                            float rightValue = BitConverter.ToSingle(frameData, (i + 32) * bytesPerFrame);

                            int leftDb = (int)Math.Round(20 * Math.Log10(Math.Abs(leftValue) + 1e-20));
                            int rightDb = (int)Math.Round(20 * Math.Log10(Math.Abs(rightValue) + 1e-20));

                            leftArr[i] = Math.Max(0, Math.Min(100, leftDb + 100));
                            rightArr[i] = Math.Max(0, Math.Min(100, rightDb + 100));

                            peakValue = Math.Max(peakValue, leftArr[i]);
                            peakValue = Math.Max(peakValue, rightArr[i]);
                        }

                        frequenciesChannels.Add(new FrequenciesChannelsModel { Left = leftArr, Right = rightArr });
                    }

                    var frequenciesData = new FrequenciesDataModel
                    {
                        Refresh = refresh,
                        Frequencies = frequenciesChannels,
                        Peak = peakValue
                    };

                    return frequenciesData;
                }
            }
        }
    }
}
