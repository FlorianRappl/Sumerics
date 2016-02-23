namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using YAMP;

    /// <summary>
    /// Is a cache for images. Speedups loading of images by caching them and
    /// creating a uniform layer of accessing various icons.
    /// </summary>
    sealed class IconFactory : TypeFactory<Value, BitmapImage>
    {
        #region The cached images

        static readonly BitmapImage _imgConversionLd = new BitmapImage(new Uri(@"..\Resources\conversion.png", UriKind.Relative));
        static readonly BitmapImage _imgConstantLd = new BitmapImage(new Uri(@"..\Resources\constant.png", UriKind.Relative));
        static readonly BitmapImage _imgFunctionLd = new BitmapImage(new Uri(@"..\Resources\function.png", UriKind.Relative));
        static readonly BitmapImage _imgPlotLd = new BitmapImage(new Uri(@"..\Resources\plot.png", UriKind.Relative));
        static readonly BitmapImage _imgSensorLd = new BitmapImage(new Uri(@"..\Resources\sensor.png", UriKind.Relative));
        static readonly BitmapImage _imgSystemLd = new BitmapImage(new Uri(@"..\Resources\system.png", UriKind.Relative));
        static readonly BitmapImage _imgTrigLd = new BitmapImage(new Uri(@"..\Resources\trigonometric.png", UriKind.Relative));
        static readonly BitmapImage _imgStatisticLd = new BitmapImage(new Uri(@"..\Resources\statistics.png", UriKind.Relative));
        static readonly BitmapImage _imgLogicLd = new BitmapImage(new Uri(@"..\Resources\logic.png", UriKind.Relative));
        static readonly BitmapImage _imgRandomLd = new BitmapImage(new Uri(@"..\Resources\random.png", UriKind.Relative));
        static readonly BitmapImage _imgUiLd = new BitmapImage(new Uri(@"..\Resources\ui.png", UriKind.Relative));
        static readonly BitmapImage _imgConversionHd = new BitmapImage(new Uri(@"..\Resources\conversion_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgConstantHd = new BitmapImage(new Uri(@"..\Resources\constant_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgFunctionHd = new BitmapImage(new Uri(@"..\Resources\function_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgPlotHd = new BitmapImage(new Uri(@"..\Resources\plot_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgSensorHd = new BitmapImage(new Uri(@"..\Resources\sensor_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgSystemHd = new BitmapImage(new Uri(@"..\Resources\system_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgTrigHd = new BitmapImage(new Uri(@"..\Resources\trigonometric_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgStatisticHd = new BitmapImage(new Uri(@"..\Resources\statistics_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgLogicHd = new BitmapImage(new Uri(@"..\Resources\logic_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgRandomHd = new BitmapImage(new Uri(@"..\Resources\random_hd.png", UriKind.Relative));
        static readonly BitmapImage _imgUiHd = new BitmapImage(new Uri(@"..\Resources\ui_hd.png", UriKind.Relative));

        static readonly Dictionary<String, ImageEntry> _images = new Dictionary<String, ImageEntry>
        {
            { "Sensor", new ImageEntry { Low = _imgSensorLd, High = _imgSensorHd } },
            { "System", new ImageEntry { Low = _imgSystemLd, High = _imgSystemHd } },
            { "Plot", new ImageEntry { Low = _imgPlotLd, High = _imgPlotHd } },
            { "Constant", new ImageEntry { Low = _imgConstantLd, High = _imgConstantHd } },
            { "Random", new ImageEntry { Low = _imgRandomLd, High = _imgRandomHd } },
            { "Statistic", new ImageEntry { Low = _imgStatisticLd, High = _imgStatisticHd } },
            { "Trigonometric", new ImageEntry { Low = _imgTrigLd, High = _imgTrigHd } },
            { "Logic", new ImageEntry { Low = _imgLogicLd, High = _imgLogicHd } },
            { "UI", new ImageEntry { Low = _imgUiLd, High = _imgUiHd } },
            { "Conversion", new ImageEntry { Low = _imgConversionLd, High = _imgConversionHd } }
        };

        static readonly Dictionary<NotificationType, BitmapImage> _notifications = new Dictionary<NotificationType, BitmapImage>
        {
            { NotificationType.Failure, new BitmapImage(new Uri(@"..\Icons\failure.png", UriKind.Relative)) },
            { NotificationType.Information, new BitmapImage(new Uri(@"..\Icons\information.png", UriKind.Relative)) },
            { NotificationType.Message, new BitmapImage(new Uri(@"..\Icons\message.png", UriKind.Relative)) },
            { NotificationType.Success, new BitmapImage(new Uri(@"..\Icons\success.png", UriKind.Relative)) }
        };

        #endregion

        #region ctor

        public IconFactory()
            : base(false)
        {
            Register<PlotValue>(_ => new BitmapImage(new Uri(@"..\Resources\plot.png", UriKind.Relative)));
            Register<MatrixValue>(_ => new BitmapImage(new Uri(@"..\Resources\matrix.png", UriKind.Relative)));
            Register<ScalarValue>(_ => new BitmapImage(new Uri(@"..\Resources\scalar.png", UriKind.Relative)));
            Register<StringValue>(_ => new BitmapImage(new Uri(@"..\Resources\string.png", UriKind.Relative)));
            Register<FunctionValue>(_ => new BitmapImage(new Uri(@"..\Resources\function.png", UriKind.Relative)));
        }

        #endregion

        #region Methods

        public static BitmapImage GetMessageImage(NotificationType type)
        {
            return _notifications[type];
        }

        public static BitmapImage GetLowImage(String category)
        {
            var entry = default(ImageEntry);

            if (_images.TryGetValue(category, out entry))
            {
                return entry.Low;
            }

            return _imgFunctionLd;
        }

        public static BitmapImage GetHighImage(String category)
        {
            var entry = default(ImageEntry);

            if (_images.TryGetValue(category, out entry))
            {
                return entry.High;
            }

            return _imgFunctionHd;
        }

        protected override BitmapImage CreateDefault()
        {
            return new BitmapImage(new Uri(@"..\Resources\unknown.png", UriKind.Relative));
        }

        #endregion

        #region Properties

        public static readonly BitmapImage FolderIcon = new BitmapImage(new Uri(@"Icons\folder.png", UriKind.Relative));

        public static readonly BitmapImage FileIcon = new BitmapImage(new Uri(@"Icons\file.png", UriKind.Relative));

        public static readonly BitmapImage HomeIcon = new BitmapImage(new Uri(@"Icons\home.png", UriKind.Relative));

        public static readonly BitmapImage HeartIcon = new BitmapImage(new Uri(@"Icons\heart.png", UriKind.Relative));

        public static readonly BitmapImage KeywordIcon = new BitmapImage(new Uri(@"..\Resources\keyword_ac.png", UriKind.Relative));

        public static readonly BitmapImage VariableIcon = new BitmapImage(new Uri(@"..\Resources\variable_ac.png", UriKind.Relative));

        public static readonly BitmapImage FunctionIcon = new BitmapImage(new Uri(@"..\Resources\function_ac.png", UriKind.Relative));

        public static readonly BitmapImage ConstantIcon = new BitmapImage(new Uri(@"..\Resources\constant_ac.png", UriKind.Relative));

        #endregion

        #region Struct

        struct ImageEntry
        {
            public BitmapImage Low;
            public BitmapImage High;
        }

        #endregion
    }
}
