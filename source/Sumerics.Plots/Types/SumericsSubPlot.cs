namespace Sumerics.Plots
{
    using Sumerics.Plots.Models;
    using System;
    using YAMP;

    sealed class SumericsSubPlot : SumericsPlot
    {
        #region Fields

        readonly GridPlotModel _model;
        readonly SubPlotValue _plot;

        #endregion

        #region ctor

        public SumericsSubPlot(SubPlotValue plot) 
            : base(plot)
        {
            _model = new GridPlotModel
            {
                CanEditSeries = false,
                CanToggleGrid = false,
                Models = new SubplotModels()
            };
            _plot = plot;
            UpdateProperties();
            UpdateSeries();
        }

        #endregion

        #region Properties

        public override Object Model
        {
            get { return _model; }
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            var models = _model.Models;
            models.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var subplot = _plot[i];
                _model.Models.Add(new SubplotModel
                {
                    ColumnIndex = subplot.Column,
                    ColumnSpan = subplot.ColumnSpan,
                    RowIndex = subplot.Row,
                    RowSpan = subplot.RowSpan,
                    Controller = PlotFactory.Instance.Create(subplot.Plot)
                });
            }
        }

        protected override void UpdateProperties()
        {
            _model.Title = _plot.Title;
            _model.Rows = _plot.Rows;
            _model.Columns = _plot.Columns;
        }

        #endregion

        #region Methods

        protected override void Refresh()
        {
            //TODO
        }

        #endregion
    }
}
