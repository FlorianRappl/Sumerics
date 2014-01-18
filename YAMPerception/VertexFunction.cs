using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to vertex video streams via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class VertexFunction : PerceptionFunction
    {
        IVertexDataPipeline pipeline;

        /// <summary>
        /// returns vertex image data
        /// </summary>
        /// <returns></returns>
        [Description("Returns vertex video data. If the argument is 0 a single image is returned. If the argument is negative, the vertex pipeline is stopped.")]
        [ExampleAttribute("vertex(1)", "Returns about 1 second of vertex video data.")]
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
                NewVertexDataEventsArgs data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateVertexDataPipeline();
                pipeline.NewVertexData += (sender, e) => data = e;

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if(data != null)
                    {
                        pipeline.NewVertexData -= (sender, e) => data = e;
                        break;
                    }
                }
                while(true);

                int height = data.Depth.GetLength(0);
                int width = data.Depth.GetLength(1);
                var double_data = new double[height, width];

                var t = new MatrixValue[3];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        double_data[i, j] = data.X[i, j];
                t[0] = new MatrixValue(double_data);
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        double_data[i, j] = data.Y[i, j];
                t[1] = new MatrixValue(double_data);
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        double_data[i, j] = data.Depth[i, j];
                t[2] = new MatrixValue(double_data);

                return new ArgumentsValue(t);
            }
            else
            {
                var data = new List<NewVertexDataEventsArgs>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateVertexDataPipeline();
                pipeline.NewVertexData += (sender, e) => data.Add(e);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.NewVertexData -= (sender, e) => data.Add(e);
                
                int nFrames = data.Count;
                if(data.Count == 0)
                    return Value.Empty;

                int height = data[0].Depth.GetLength(0);
                int width = data[0].Depth.GetLength(1);
                var result = new ArgumentsValue[nFrames];
                var double_data = new double[height, width]; 

                for(int frame = 0; frame < nFrames; frame++)
                {
                    var _data = data[frame];

                    var t = new MatrixValue[3];
                    for (int i = 0; i < height; i++)
                        for (int j = 0; j < width; j++)
                            double_data[i, j] = _data.X[i, j];
                    t[0] = new MatrixValue(double_data);
                    for (int i = 0; i < height; i++)
                        for (int j = 0; j < width; j++)
                            double_data[i, j] = _data.Y[i, j];
                    t[1] = new MatrixValue(double_data);
                    for (int i = 0; i < height; i++)
                        for (int j = 0; j < width; j++)
                            double_data[i, j] = _data.Depth[i, j];
                    t[2] = new MatrixValue(double_data);

                    result[frame] = new ArgumentsValue(t);
                }

                return new ArgumentsValue(result);
            }
        }

        /// <summary>
        /// initializes vertex pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes vertex pipeline")]
        [ExampleAttribute("vertex(160, 120)", "Tries to initialize a vertex pipeline with 160 pixel height and 120 pixel width.")]
        public StringValue Function(ScalarValue width, ScalarValue height)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateVertexDataPipeline(width.IntValue, height.IntValue);
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
        /// initializes vertex pipeline
        /// </summary>
        /// <returns></returns>
        [Description("initializes vertex pipeline")]
        [ExampleAttribute("vertex()", "Tries to initialize a vertex pipeline.")]
        public StringValue Function()
        {
            return Function(new ScalarValue(-1), new ScalarValue(-1));
        }


        ~VertexFunction()
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
