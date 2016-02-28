namespace Sumerics.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// Generic handler definition.
    /// </summary>
    public delegate void RoutedHandler<T>(Object sender, T e) where T : RoutedEventArgs;
}
