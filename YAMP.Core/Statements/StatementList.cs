using System;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Statements
{
    class StatementList : Statement
    {
        #region Members

        protected readonly List<Statement> _statements;

        #endregion
        
        #region ctor

        /// <summary>
        /// Creates a new scope.
        /// </summary>
        public StatementList()
        {
            _statements = new List<Statement>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the statement at the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the statement.</param>
        /// <returns>The statement at the index.</returns>
        public Statement this[int index]
        {
            get { return _statements[index]; }
        }

        /// <summary>
        /// Gets the number of statements.
        /// </summary>
        public int Count
        {
            get { return _statements.Count; }
        }

        /// <summary>
        /// Gets the last inserted statement.
        /// </summary>
        public Statement Last
        {
            get { return _statements.Count != 0 ? _statements[_statements.Count - 1] : null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates the given query in the given context.
        /// </summary>
        /// <param name="ctx">The context of the evaluation.</param>
        /// <returns>The result of the query.</returns>
        public override Dynamic Evaluate(RunContext ctx)
        {
            int index = 0;
            Dynamic result = null;
            var scope = new Scope();
            ctx.CreateScope(scope);

            while (index < _statements.Count)
            {
                var statement = _statements[index++];
                var answer = statement.Evaluate(ctx);

                if (answer != null)
                {
                    result = answer;

                    if (statement.IsAssigned)
                        ctx.SetAnswer(answer);
                }

                if (ctx.ShouldStop)
                    break;
            }

            ctx.ReleaseScope();
            return ctx.ReturnValue ?? (result ?? new Dynamic());
        }

        /// <summary>
        /// Adds a statement to the list of statements.
        /// </summary>
        /// <param name="statement">The statement.</param>
        public void Add(Statement statement)
        {
            _statements.Add(statement);
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the list of statements into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            var lines = new string[_statements.Count];

            for (int i = 0; i < lines.Length; i++)
                lines[i] = _statements[i].ToCode() + ";";

            return string.Join(Environment.NewLine, lines);
        }

        #endregion
    }
}
