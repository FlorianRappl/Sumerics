using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls
{
    public interface IScriptFileModel
    {
        string Text { get; set; }

        bool Changed { get; set; }

        void Compile();

        void Save();

        void SaveAs();

        void Close();

        void Execute();

        string TransformMathML(string query);

        ObservableCollection<AutocompleteItem> Items { get; }
    }
}
