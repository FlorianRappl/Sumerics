using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to face recogition via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class FaceFunction : PerceptionFunction
    {
        IFacePipeline pipeline;

        /// <summary>
        /// performs face recognition
        /// </summary>
        /// <returns></returns>
        [Description("Performs face recogition. If the argument is 0 the first recognition result is returned. If the argument is negative, the face pipeline is stopped.")]
        [ExampleAttribute("face(2)", "Returns the face recogition result for 2 seconds.")]
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
                FaceEventArgs data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateFacePipeline();
                pipeline.NewFace += (sender, e) => data = e;

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if (data != null)
                    {
                        pipeline.NewFace -= (sender, e) => data = e;
                        break;
                    }
                }
                while (true);

                var faces = new ArgumentsValue[data.nFaces];
                for (int i = 0; i < faces.Length; i++)
                {
                    var position = new MatrixValue(new double[,] { { data.Positions[i].X, data.Positions[i].Y }, { data.Positions[i].W, data.Positions[i].H } });
                    var attributes = new StringValue(string.Join("|", data.Attributes[i]));
                    var landmarks = new ArgumentsValue(data.Landmarks[i].Select(l => new ArgumentsValue(new StringValue(l.Key), new MatrixValue(new double[,] { { l.Value.X, l.Value.Y, l.Value.Z } }))).ToArray());
                }

                return new ArgumentsValue(faces);
            }
            else
            {
                var data = new List<FaceEventArgs>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateFacePipeline();
                pipeline.NewFace += (sender, e) => data.Add(e);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.NewFace -= (sender, e) => data.Add(e);

                int nFrames = data.Count;
                if (data.Count == 0)
                    return Value.Empty;
                var result = new ArgumentsValue[nFrames];

                for (int frame = 0; frame < nFrames; frame++)
                {
                    var _data = data[frame];

                    var faces = new ArgumentsValue[_data.nFaces];
                    for (int i = 0; i < faces.Length; i++)
                    {
                        var position = new MatrixValue(new double[,] { { _data.Positions[i].X, _data.Positions[i].Y }, { _data.Positions[i].W, _data.Positions[i].H } });
                        var attributes = new StringValue(string.Join("|", _data.Attributes[i]));
                        var landmarks = new ArgumentsValue(_data.Landmarks[i].Select(l => new ArgumentsValue(new StringValue(l.Key), new MatrixValue(new double[,] { { l.Value.X, l.Value.Y, l.Value.Z } }))).ToArray());
                    }

                    result[frame] = new ArgumentsValue(faces);
                }

                return new ArgumentsValue(result);
            }
        }

        /// <summary>
        /// initializes face recognition module
        /// </summary>
        /// <returns></returns>
        [Description("initializes face recogition module")]
        [ExampleAttribute("face(640, 480)", "tries to initialize a face recogition module working on a color pipeline with 640 pixel height and 480 pixel width.")]
        public StringValue Function(ScalarValue width, ScalarValue height)
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateFacePipeline(width.IntValue, height.IntValue);
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
        /// initializes face recognition module
        /// </summary>
        /// <returns></returns>
        [Description("initializes face recogition module")]
        [ExampleAttribute("face()", "tries to initialize a face recogition module.")]
        public StringValue Function()
        {
            return Function(new ScalarValue(-1), new ScalarValue(-1));
        }

        ~FaceFunction()
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
