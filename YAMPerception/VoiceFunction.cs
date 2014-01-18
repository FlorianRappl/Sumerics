using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to voice recognition via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class VoiceFunction : PerceptionFunction
    {
        IVoicePipeline pipeline;

        /// <summary>
        /// performs voice recognition
        /// </summary>
        /// <returns></returns>
        [Description("Performs voice recognition. If the argument is 0 the first recognition result is returned. If the argument is negative, the voice pipeline is stopped.")]
        [ExampleAttribute("voice(5)", "Returns the voice recognition result for 5 seconds.")]
        public StringValue Function(ScalarValue seconds)
        {
            if (seconds.Value < 0)
            {
                if (pipeline != null)
                {
                    pipeline.Stop();
                    pipeline.Dispose();
                    pipeline = null;
                }
                return new StringValue("success");
            }
            else if (seconds.Value == 0.0)
            {
                string data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateVoicePipeline();
                pipeline.VoiceCommand += (sender, e) => data = e.Label;

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if (data != null)
                    {
                        pipeline.VoiceCommand -= (sender, e) => data = e.Label;
                        break;
                    }
                }
                while (true);

                return new StringValue(data);
            }
            else
            {
                var data = new List<string>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateVoicePipeline();
                pipeline.VoiceCommand += (sender, e) => data.Add(e.Label);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.VoiceCommand -= (sender, e) => data.Add(e.Label);

                return new StringValue(string.Join("|", data));
            }
        }

        /// <summary>
        /// initializes voice recognition module
        /// </summary>
        /// <returns></returns>
        [Description("initializes voice recognition module")]
        [ExampleAttribute("voice(\"red|green|blue\")", "tries to initialize a voice recognition module for the three commands \"red\", \"green\", and \"blue\".")]
        public StringValue Function(StringValue commands)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateVoicePipeline(new string[][] { commands.Value.Split('|') });
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
        /// initializes voice recognition module
        /// </summary>
        /// <returns></returns>
        [Description("initializes voice recognition module")]
        [ExampleAttribute("voice()", "tries to initialize a voice recognition module for dictation.")]
        public StringValue Function()
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateVoicePipeline();
            if (pipeline.GetType().Name.Contains("Dummy"))
            {
                pipeline.Dispose();
                pipeline = null;
                return new StringValue("failed");
            }

            pipeline.Start();

            return new StringValue("success");
        }

        ~VoiceFunction()
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
