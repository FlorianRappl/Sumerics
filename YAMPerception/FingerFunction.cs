using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using PCSDK;

namespace YAMPerception
{
    [Description("Provides access to finger and hand tracking via the Intel Perceptual Computing SDK™.")]
    [Kind("Perception")]
    public class FingerFunction : PerceptionFunction
    {
        IGesturePipeline pipeline;

        /// <summary>
        /// performs finger recognition
        /// </summary>
        /// <returns></returns>
        [Description("Performs finger and hand tracking. If the argument is 0 the first tracking result is returned. If the argument is negative, the finger pipeline is stopped.")]
        [ExampleAttribute("finger(5)", "Returns the finger tracking result for 5 seconds.")]
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
                Tuple<DateTime, FingerEventArgs> data = null;

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateGesturePipeline();
                pipeline.FingerChanged += (sender, e) => data = new Tuple<DateTime, FingerEventArgs>(DateTime.Now, e);

                if (!pipeline.IsRunning)
                    pipeline.Start();
                do
                {
                    System.Threading.Thread.Sleep(100);
                    if (data != null)
                    {
                        pipeline.FingerChanged -= (sender, e) => data = new Tuple<DateTime, FingerEventArgs>(DateTime.Now, e);
                        break;
                    }
                }
                while (true);

                return fingerDataToValue(data);
            }
            else
            {
                var data = new List<Tuple<DateTime, FingerEventArgs>>();

                if (pipeline == null)
                    pipeline = PipelineFactory.CreateGesturePipeline();
                pipeline.FingerChanged += (sender, e) => data.Add(new Tuple<DateTime, FingerEventArgs>(DateTime.Now, e));

                if (!pipeline.IsRunning)
                    pipeline.Start();
                System.Threading.Thread.Sleep((int)(seconds.Value * 1000));
                pipeline.FingerChanged -= (sender, e) => data.Add(new Tuple<DateTime, FingerEventArgs>(DateTime.Now, e));

                return new ArgumentsValue(
                    new ArgumentsValue(data.Where(d => d.Item2.Primary).Select(d => fingerDataToValue(d)).ToArray()),
                    new ArgumentsValue(data.Where(d => !d.Item2.Primary).Select(d => fingerDataToValue(d)).ToArray())
                    );
            }
        }

        ArgumentsValue fingerDataToValue(Tuple<DateTime, FingerEventArgs> data)
        {
            var double_data = new double[6, 3];
            for (int i = 0; i < 5; i++)
            {
                if (data.Item2.Fingers[i] != null)
                {
                    double_data[i, 0] = data.Item2.Fingers[i].X;
                    double_data[i, 1] = data.Item2.Fingers[i].Y;
                    double_data[i, 2] = data.Item2.Fingers[i].Z;
                }
            }
            if (data.Item2.Hand != null)
            {
                double_data[5, 0] = data.Item2.Hand.X;
                double_data[5, 1] = data.Item2.Hand.Y;
                double_data[5, 2] = data.Item2.Hand.Z;
            }

            return new ArgumentsValue(new ScalarValue(DateTime.Now.Ticks), new MatrixValue(double_data));
        }

        /// <summary>
        /// initializes finger recognition module
        /// </summary>
        /// <returns></returns>
        [Description("initializes finger and hand tracking module")]
        [ExampleAttribute("finger()", "tries to initialize a finger tracking module.")]
        public StringValue Function()
        {
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
                pipeline = null;
            }

            pipeline = PipelineFactory.CreateGesturePipeline();
            if (pipeline.GetType().Name.Contains("Dummy"))
            {
                pipeline.Dispose();
                pipeline = null;
                return new StringValue("failed");
            }

            pipeline.Start();

            return new StringValue("success");
        }

        ~FingerFunction()
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
