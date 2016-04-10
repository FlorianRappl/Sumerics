namespace FastColoredTextBoxNS
{
    using System;
    using System.Collections.Generic;

    internal class CommandManager
    {
        readonly Int32 _maxHistoryLength = 200;
        readonly LimitedStack<UndoableCommand> _history;
        readonly Stack<UndoableCommand> _redoStack;

        Int32 disabledCommands = 0;
        Int32 autoUndoCommands = 0;

        public TextSource TextSource { get; private set; }

        public CommandManager(TextSource ts)
        {
            _history = new LimitedStack<UndoableCommand>(_maxHistoryLength);
            _redoStack = new Stack<UndoableCommand>();
            TextSource = ts;
        }

        public void ExecuteCommand(Command cmd)
        {
            if (disabledCommands <= 0)
            {
                var undo = cmd as UndoableCommand;

                //multirange ?
                if (cmd.ts.CurrentTB.Selection.ColumnSelectionMode && undo != null)
                {
                    //make wrapper
                    cmd = new MultiRangeCommand(undo);
                }


                if (undo != null)
                {
                    //if range is ColumnRange, then create wrapper
                    undo.autoUndo = autoUndoCommands > 0;
                    _history.Push(undo);
                }

                try
                {
                    cmd.Execute();
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (undo != null)
                    {
                        _history.Pop();
                    }
                }

                _redoStack.Clear();
                TextSource.CurrentTB.OnUndoRedoStateChanged();
            }
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var cmd = _history.Pop();
                BeginDisableCommands();//prevent text changing into handlers

                try
                {
                    cmd.Undo();
                }
                finally
                {
                    EndDisableCommands();
                }

                _redoStack.Push(cmd);
            }

            //undo next autoUndo command
            if (_history.Count > 0 && _history.Peek().autoUndo)
            {
                Undo();
            }

            TextSource.CurrentTB.OnUndoRedoStateChanged();
        }

        void EndDisableCommands()
        {
            disabledCommands--;
        }

        void BeginDisableCommands()
        {
            disabledCommands++;
        }

        public void EndAutoUndoCommands()
        {
            autoUndoCommands--;

            if (autoUndoCommands == 0 && _history.Count > 0)
            {
                _history.Peek().autoUndo = false;
            }
        }

        public void BeginAutoUndoCommands()
        {
            autoUndoCommands++;
        }

        internal void ClearHistory()
        {
            _history.Clear();
            _redoStack.Clear();
            TextSource.CurrentTB.OnUndoRedoStateChanged();
        }

        internal void Redo()
        {
            if (_redoStack.Count != 0)
            {
                var cmd = default(UndoableCommand);
                BeginDisableCommands();//prevent text changing into handlers

                try
                {
                    cmd = _redoStack.Pop();

                    if (TextSource.CurrentTB.Selection.ColumnSelectionMode)
                    {
                        TextSource.CurrentTB.Selection.ColumnSelectionMode = false;
                    }

                    TextSource.CurrentTB.Selection.Start = cmd.sel.Start;
                    TextSource.CurrentTB.Selection.End = cmd.sel.End;
                    cmd.Execute();
                    _history.Push(cmd);
                }
                finally
                {
                    EndDisableCommands();
                }

                //redo command after autoUndoable command
                if (cmd.autoUndo)
                {
                    Redo();
                }

                TextSource.CurrentTB.OnUndoRedoStateChanged();
            }
        }

        public Boolean UndoEnabled 
        { 
            get { return _history.Count > 0; }
        }

        public Boolean RedoEnabled
        {
            get { return _redoStack.Count > 0; }
        }
    }
}