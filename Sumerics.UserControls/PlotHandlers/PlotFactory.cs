using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls
{
	public static class PlotFactory
	{
		public static SumericsPlot Create(PlotValue plot)
		{
			if (plot is Plot2DValue)
				return new SumericsPlot2D((Plot2DValue)plot);
			else if (plot is PolarPlotValue)
				return new SumericsPolarPlot((PolarPlotValue)plot);
			else if (plot is ContourPlotValue)
				return new SumericsContourPlot((ContourPlotValue)plot);
			else if (plot is BarPlotValue)
				return new SumericsBarPlot((BarPlotValue)plot);
			else if (plot is ErrorPlotValue)
				return new SumericsErrorPlot((ErrorPlotValue)plot);
			else if(plot is ComplexPlotValue)
                return new SumericsComplexPlot((ComplexPlotValue)plot);
            else if (plot is HeatmapPlotValue)
                return new SumericsHeatPlot((HeatmapPlotValue)plot);
            else if (plot is SubPlotValue)
                return new SumericsSubPlot((SubPlotValue)plot);
            else if (plot is Plot3DValue)
                return new SumericsLinePlot3D((Plot3DValue)plot);
            else if (plot is SurfacePlotValue)
                return new SumericsSurfacePlot((SurfacePlotValue)plot);

			return null;
		}
	}
}
