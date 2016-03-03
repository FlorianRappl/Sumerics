namespace Sumerics
{
    using Sumerics.Resources;
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

        static readonly BitmapImage _imgConversionLd = Icons.conversion.ToBitmapImage();
        static readonly BitmapImage _imgConstantLd = Icons.constant.ToBitmapImage();
        static readonly BitmapImage _imgFunctionLd = Icons.function.ToBitmapImage();
        static readonly BitmapImage _imgPlotLd = Icons.plot.ToBitmapImage();
        static readonly BitmapImage _imgSensorLd = Icons.sensor.ToBitmapImage();
        static readonly BitmapImage _imgSystemLd = Icons.system.ToBitmapImage();
        static readonly BitmapImage _imgTrigLd = Icons.trigonometric.ToBitmapImage();
        static readonly BitmapImage _imgStatisticLd = Icons.statistics.ToBitmapImage();
        static readonly BitmapImage _imgLogicLd = Icons.logic.ToBitmapImage();
        static readonly BitmapImage _imgRandomLd = Icons.random.ToBitmapImage();
        static readonly BitmapImage _imgUiLd = Icons.ui.ToBitmapImage();
        static readonly BitmapImage _imgConversionHd = Icons.conversion_hd.ToBitmapImage();
        static readonly BitmapImage _imgConstantHd = Icons.constant_hd.ToBitmapImage();
        static readonly BitmapImage _imgFunctionHd = Icons.function_hd.ToBitmapImage();
        static readonly BitmapImage _imgPlotHd = Icons.plot_hd.ToBitmapImage();
        static readonly BitmapImage _imgSensorHd = Icons.sensor_hd.ToBitmapImage();
        static readonly BitmapImage _imgSystemHd = Icons.system_hd.ToBitmapImage();
        static readonly BitmapImage _imgTrigHd = Icons.trigonometric_hd.ToBitmapImage();
        static readonly BitmapImage _imgStatisticHd = Icons.statistics_hd.ToBitmapImage();
        static readonly BitmapImage _imgLogicHd = Icons.logic_hd.ToBitmapImage();
        static readonly BitmapImage _imgRandomHd = Icons.random_hd.ToBitmapImage();
        static readonly BitmapImage _imgUiHd = Icons.ui_hd.ToBitmapImage();

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
            Register<PlotValue>(_ => Icons.plot.ToBitmapImage());
            Register<MatrixValue>(_ => Icons.matrix.ToBitmapImage());
            Register<ScalarValue>(_ => Icons.scalar.ToBitmapImage());
            Register<StringValue>(_ => Icons._string.ToBitmapImage());
            Register<FunctionValue>(_ => Icons.function.ToBitmapImage());
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
            return Icons.unknown.ToBitmapImage();
        }

        #endregion

        #region Properties

        public static readonly BitmapImage FolderIcon = new BitmapImage(new Uri(@"..\Icons\folder.png", UriKind.Relative));

        public static readonly BitmapImage FileIcon = new BitmapImage(new Uri(@"..\Icons\file.png", UriKind.Relative));

        public static readonly BitmapImage HomeIcon = new BitmapImage(new Uri(@"..\Icons\home.png", UriKind.Relative));

        public static readonly BitmapImage HeartIcon = new BitmapImage(new Uri(@"..\Icons\heart.png", UriKind.Relative));

        public static readonly BitmapImage KeywordIcon = Icons.keyword_ac.ToBitmapImage();

        public static readonly BitmapImage VariableIcon = Icons.variable_ac.ToBitmapImage();

        public static readonly BitmapImage FunctionIcon = Icons.function_ac.ToBitmapImage();

        public static readonly BitmapImage ConstantIcon = Icons.constant_ac.ToBitmapImage();

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
