using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to audio streams via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class AudioFunction : PerceptionFunction
    {
        IAudioDataPipeline pipeline;

        /// <summary>
        /// retrieves audio sample
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves audio sample. If the argument is negative, the audio pipeline is stopped.")]
        [ExampleAttribute("audio(1)", "Returns an audio sample of about 1 seconds.")]
        public Value Function(ScalarValue seconds)
        {
            if (seconds.Value <= 0)
            {
                if (pipeline != null)
                {
                    pipeline.Stop();
                    pipeline.Dispose();
                    pipeline = null;
                }
                return new StringValue("success");
            }
            else
            {
                var data = new List<float[,]>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateAudioDataPipeline();
                pipeline.NewAudioData += (sender, e) => data.Add(e.Data);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.NewAudioData -= (sender, e) => data.Add(e.Data);

                var nChannels = pipeline.nChannels;

                var nData = data.Select(a => a.GetLength(1)).Sum();
                int offset = 0;

                var result = new double[nChannels, nData];

                for (int frame = 0; frame < data.Count; frame++)
                {
                    var _data = data[frame];
                    var length = _data.GetLength(1);
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            result[channel, j + offset] = _data[channel, j];
                        }
                    }
                    offset += length;
                }

                return new MatrixValue(result);
            }
        }

        /// <summary>
        /// initializes audio pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes audio pipeline")]
        [ExampleAttribute("audio(44100, 2, 1, \"Depth\")", "tries to initialize an audio pipeline with sample rate of 44100Hz, 2 channels, with volume 1, and for a device containing \"Depth\" in its name.")]
        public StringValue Function(ScalarValue sampleRate, ScalarValue nChannels, ScalarValue volume, StringValue deviceName)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateAudioDataPipeline((uint)sampleRate.IntValue, (uint)nChannels.IntValue, deviceName.Value, (float)volume.Value);
            if (pipeline.GetType().Name.Contains("Dummy"))
            {
                pipeline.Dispose();
                pipeline = null;
                return new StringValue("failed");
            }

            pipeline.Start();

            return new StringValue("success");
        }

        /// <summary>
        /// initializes audio pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes audio pipeline")]
        [ExampleAttribute("audio(44100, 2, 1)", "tries to initialize an audio pipeline with sample rate of 44100Hz, 2 channels, and with volume 1.")]
        public StringValue Function(ScalarValue sampleRate, ScalarValue nChannels, ScalarValue volume)
        {
            return Function(sampleRate, nChannels, volume, new StringValue(""));
        }

        /// <summary>
        /// initializes audio pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes audio pipeline")]
        [ExampleAttribute("audio(44100, 2)", "tries to initialize an audio pipeline with sample rate of 44100Hz and 2 channels.")]
        public StringValue Function(ScalarValue sampleRate, ScalarValue nChannels)
        {
            return Function(sampleRate, nChannels, new ScalarValue(1), new StringValue(""));
        }

        /// <summary>
        /// initializes audio pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes audio pipeline")]
        [ExampleAttribute("audio()", "tries to initialize an audio pipeline.")]
        public StringValue Function()
        {
            return Function(new ScalarValue(44100), new ScalarValue(2), new ScalarValue(1), new StringValue(""));
        }

        ~AudioFunction()
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }
        }
    }
}
