using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to depth video streams via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class DepthFunction : PerceptionFunction
    {
        IDepthDataPipeline pipeline;

        /// <summary>
        /// returns depth image data
        /// </summary>
        /// <returns></returns>
        [Description("Returns depth video data. If the argument is 0 a single image is returned. If the argument is negative, the depth pipeline is stopped.")]
        [ExampleAttribute("depth(1)", "Returns about 1 second of depth video data.")]
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
                ushort[,] data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateDepthDataPipeline();
                pipeline.NewDepthData += (sender, e) => data = e.Data;

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if(data != null)
                    {
                        pipeline.NewDepthData -= (sender, e) => data = e.Data;
                        break;
                    }
                }
                while(true);

                int height = data.GetLength(0);
                int width = data.GetLength(1);
                var double_data = new double[height, width];

                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        double_data[i, j] = data[i, j];

                return new MatrixValue(double_data);
            }
            else
            {
                var data = new List<ushort[,]>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateDepthDataPipeline();
                pipeline.NewDepthData += (sender, e) => data.Add(e.Data);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.NewDepthData -= (sender, e) => data.Add(e.Data);
                
                int nFrames = data.Count;
                if(data.Count == 0)
                    return Value.Empty;
                
                int height = data[0].GetLength(0);
                int width = data[0].GetLength(1);
                var result = new MatrixValue[nFrames];
                var double_data = new double[height, width]; 

                for(int frame = 0; frame < nFrames; frame++)
                {
                    var _data = data[frame];

                    for (int i = 0; i < height; i++)
                        for (int j = 0; j < width; j++)
                            double_data[i, j] = _data[i, j];
                    
                    result[frame] = new MatrixValue(double_data);
                }

                return new ArgumentsValue(result);
            }
        }

        /// <summary>
        /// initializes depth pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes depth pipeline")]
        [ExampleAttribute("depth(160, 120)", "Tries to initialize a depth pipeline with 160 pixel height and 120 pixel width.")]
        public StringValue Function(ScalarValue width, ScalarValue height)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateDepthDataPipeline(width.IntValue, height.IntValue);
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
        /// initializes depth pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes depth pipeline")]
        [ExampleAttribute("depth()", "Tries to initialize a depth pipeline.")]
        public StringValue Function()
        {
            return Function(new ScalarValue(-1), new ScalarValue(-1));
        }


        ~DepthFunction()
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
