namespace Sumerics.Plots.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class SubplotModels : IEnumerable<SubplotModel>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        readonly ObservableCollection<SubplotModel> _models;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { ((INotifyCollectionChanged)_models).CollectionChanged += value; }
            remove { ((INotifyCollectionChanged)_models).CollectionChanged -= value; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { ((INotifyPropertyChanged)_models).PropertyChanged += value; }
            remove { ((INotifyPropertyChanged)_models).PropertyChanged -= value; }
        }

        public SubplotModels()
        {
            _models = new ObservableCollection<SubplotModel>();
        }

        public void Clear()
        {
            _models.Clear();
        }

        public void Add(SubplotModel model)
        {
            _models.Add(model);
        }

        public IEnumerator<SubplotModel> GetEnumerator()
        {
            return _models.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
