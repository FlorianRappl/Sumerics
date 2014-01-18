using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to color video streams via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class VideoFunction : PerceptionFunction
    {
        IColorDataPipeline pipeline;

        /// <summary>
        /// returns color image data
        /// </summary>
        /// <returns></returns>
        [Description("Returns color video data. If the argument is 0 a single image is returned. If the argument is negative, the color pipeline is stopped.")]
        [ExampleAttribute("video(1)", "Returns about 1 second of color video data.")]
        public Value Function(ScalarValue seconds)
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
                List<byte[,]> data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateColorDataPipeline();
                pipeline.NewColorData += (sender, e) => data = e.Data;

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if(data != null)
                    {
                        pipeline.NewColorData -= (sender, e) => data = e.Data;
                        break;
                    }
                }
                while(true);

                int height = data[0].GetLength(0);
                int width = data[0].GetLength(1);
                var double_data = new double[height, width];

                var result = new MatrixValue[3];
                for (int color = 0; color < 3; color++)
                {
                    for (int i = 0; i < height; i++)
                        for (int j = 0; j < width; j++)
                            double_data[i, j] = data[color][i, j];

                    result[color] = new MatrixValue(double_data);
                }
                return new ArgumentsValue(result);
            }
            else
            {
                var data = new List<List<byte[,]>>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateColorDataPipeline();
                pipeline.NewColorData += (sender, e) => data.Add(e.Data);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.NewColorData -= (sender, e) => data.Add(e.Data);
                
                int nFrames = data.Count;
                if(data.Count == 0)
                    return Value.Empty;

                int height = data[0][0].GetLength(0);
                int width = data[0][0].GetLength(1);
                var result = new ArgumentsValue[nFrames];
                var double_data = new double[height, width]; 

                for(int frame = 0; frame < nFrames; frame++)
                {
                    var _data = data[frame];

                    var _result = new MatrixValue[3];
                    for (int color = 0; color < 3; color++)
                    {
                        for (int i = 0; i < height; i++)
                            for (int j = 0; j < width; j++)
                                double_data[i, j] = _data[color][i, j];

                        _result[color] = new MatrixValue(double_data);
                    }
                    result[frame] = new ArgumentsValue(_result);
                }

                return new ArgumentsValue(result);
            }
        }

        /// <summary>
        /// initializes color pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes color pipeline")]
        [ExampleAttribute("video(160, 120)", "Tries to initialize a color pipeline with 160 pixel height and 120 pixel width.")]
        public StringValue Function(ScalarValue width, ScalarValue height)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateColorDataPipeline(width.IntValue, height.IntValue);
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
        /// initializes color pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes color pipeline")]
        [ExampleAttribute("video()", "Tries to initialize a color pipeline.")]
        public StringValue Function()
        {
            return Function(new ScalarValue(-1), new ScalarValue(-1));
        }

        ~VideoFunction()
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
