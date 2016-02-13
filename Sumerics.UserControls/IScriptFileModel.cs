namespace Sumerics.Controls
{
    using System;
    using System.Collections.ObjectModel;

    public interface IScriptFileModel
    {
        String Text { get; set; }

        Boolean Changed { get; set; }

        void Compile();

        void Save();

        void SaveAs();

        void Close();

        void Execute();

        String TransformMathML(String query);

        ObservableCollection<AutocompleteItem> Items { get; }
    }
}
