namespace Sumerics
{
    using System;
    using System.Windows.Media.Imaging;
    using YAMP;

    /// <summary>
    /// Is a cache for images. Speedups loading of images by caching them and
    /// creating a uniform layer of accessing various icons.
    /// </summary>
    static class Icons
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

        static readonly BitmapImage _imgScalar = new BitmapImage(new Uri(@"..\Resources\scalar.png", UriKind.Relative));
        static readonly BitmapImage _imgMatrix = new BitmapImage(new Uri(@"..\Resources\matrix.png", UriKind.Relative));
        static readonly BitmapImage _imgString = new BitmapImage(new Uri(@"..\Resources\string.png", UriKind.Relative));
        static readonly BitmapImage _imgUnknown = new BitmapImage(new Uri(@"..\Resources\unknown.png", UriKind.Relative));

        static readonly BitmapImage _imgMessage = new BitmapImage(new Uri(@"..\Icons\message.png", UriKind.Relative));
        static readonly BitmapImage _imgInformation = new BitmapImage(new Uri(@"..\Icons\information.png", UriKind.Relative));
        static readonly BitmapImage _imgSuccess = new BitmapImage(new Uri(@"..\Icons\success.png", UriKind.Relative));
        static readonly BitmapImage _imgFailure = new BitmapImage(new Uri(@"..\Icons\failure.png", UriKind.Relative));

        static readonly BitmapImage _imgFolderIcon = new BitmapImage(new Uri(@"Icons\folder.png", UriKind.Relative));
        static readonly BitmapImage _imgFileIcon = new BitmapImage(new Uri(@"Icons\file.png", UriKind.Relative));
        static readonly BitmapImage _imgHomeIcon = new BitmapImage(new Uri(@"Icons\home.png", UriKind.Relative));
        static readonly BitmapImage _imgHeartIcon = new BitmapImage(new Uri(@"Icons\heart.png", UriKind.Relative));

        static readonly BitmapImage _imgKeywordAc = new BitmapImage(new Uri(@"..\Resources\keyword_ac.png", UriKind.Relative));
        static readonly BitmapImage _imgVariableAc = new BitmapImage(new Uri(@"..\Resources\variable_ac.png", UriKind.Relative));
        static readonly BitmapImage _imgFunctionAc = new BitmapImage(new Uri(@"..\Resources\function_ac.png", UriKind.Relative));
        static readonly BitmapImage _imgConstantAc = new BitmapImage(new Uri(@"..\Resources\constant_ac.png", UriKind.Relative));

        #endregion

        #region Methods

        public static BitmapImage GetLowImage(String category)
        {
            switch (category)
            {
                case "Sensor":
                    return _imgSensorLd;
                case "System":
                    return _imgSystemLd;
                case "Plot":
                    return _imgPlotLd;
                case "Constant":
                    return _imgConstantLd;
                case "Random":
                    return _imgRandomLd;
                case "Statistic":
                    return _imgStatisticLd;
                case "Trigonometric":
                    return _imgTrigLd;
                case "Logic":
                    return _imgLogicLd;
                case "UI":
                    return _imgUiLd;
                case "Conversion":
                    return _imgConversionLd;
                default:
                    return _imgFunctionLd;
            }
        }

        public static BitmapImage GetHighImage(String category)
        {
            switch (category)
            {
                case "Sensor":
                    return _imgSensorHd;
                case "System":
                    return _imgSystemHd;
                case "Plot":
                    return _imgPlotHd;
                case "Constant":
                    return _imgConstantHd;
                case "Random":
                    return _imgRandomHd;
                case "Statistic":
                    return _imgStatisticHd;
                case "Trigonometric":
                    return _imgTrigHd;
                case "Logic":
                    return _imgLogicHd;
                case "UI":
                    return _imgUiHd;
                case "Conversion":
                    return _imgConversionHd;
                default:
                    return _imgFunctionHd;
            }
        }

        public static BitmapImage GetVariableImage(Value value)
        {
            if (value is PlotValue)
                return _imgPlotLd;
            else if (value is MatrixValue)
                return _imgMatrix;
            else if (value is ScalarValue)
                return _imgScalar;
            else if (value is StringValue)
                return _imgString;
            else if (value is FunctionValue)
                return _imgFunctionLd;

            return _imgUnknown;
        }

        public static BitmapImage GetMessageImage(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Failure:
                    return _imgFailure;
                case NotificationType.Information:
                    return _imgInformation;
                case NotificationType.Success:
                    return _imgSuccess;
                default:
                    return _imgMessage;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the icon for a file.
        /// </summary>
        public static BitmapImage FileIcon { get { return _imgFileIcon; } }

        /// <summary>
        /// Gets the image for the home.
        /// </summary>
        public static BitmapImage HomeIcon { get { return _imgHomeIcon; } }

        /// <summary>
        /// Gets the image for a folder.
        /// </summary>
        public static BitmapImage FolderIcon { get { return _imgFolderIcon; } }

        /// <summary>
        /// Gets the image for a heart (favorite).
        /// </summary>
        public static BitmapImage HeartIcon { get { return _imgHeartIcon; } }

        /// <summary>
        /// Gets the autocomplete icon for a keyword.
        /// </summary>
        public static BitmapImage KeywordIcon { get { return _imgKeywordAc; } }

        /// <summary>
        /// Gets the autocomplete icon for a variable.
        /// </summary>
        public static BitmapImage VariableIcon { get { return _imgVariableAc; } }

        /// <summary>
        /// Gets the autocomplete icon for a constant.
        /// </summary>
        public static BitmapImage ConstantIcon { get { return _imgConstantAc; } }

        /// <summary>
        /// Gets the autocomplete icon for a function.
        /// </summary>
        public static BitmapImage FunctionIcon { get { return _imgFunctionAc; } }

        #endregion
    }
}
