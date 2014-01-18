﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    ///     This is a WPF wrapper of OxyPlot.BarSeries
    /// </summary>
    public class BarSeries : BarSeriesBase<BarItem>
    {
        /// <summary>
        ///     The bar width property.
        /// </summary>
        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register(
            "BarWidth", typeof(double), typeof(BarSeries), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        ///     Initializes static members of the <see cref="BarSeries" /> class.
        /// </summary>
        static BarSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(BarSeries), new PropertyMetadata("{0} {1}: {2}", AppearanceChanged));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BarSeries" /> class.
        /// </summary>
        public BarSeries()
        {
            this.InternalSeries = new OxyPlot.BarSeries();
        }

        /// <summary>
        ///     Gets or sets the bar width.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return (double)this.GetValue(BarWidthProperty);
            }

            set
            {
                this.SetValue(BarWidthProperty, value);
            }
        }

        /// <summary>
        ///     Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.BarSeries)series;
            s.BarWidth = this.BarWidth;
        }
    }
}