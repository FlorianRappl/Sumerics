using System;
using YAMP.Attributes;

namespace YAMP.Core
{
    /// <summary>
    /// This class gives a base class for more sophisticated implementations of constants.
    /// </summary>
	public abstract class YConstant : IConstant
    {
        #region Members

        string name;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance with the name given by convention.
        /// The convention is given by the rule that the name of the
        /// class is the upper case version of {Name}Constant.
        /// </summary>
		public YConstant()
		{
			name = GetType().Name.Replace("Constant", string.Empty).ToUpper();
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The name for the constant.</param>
        public YConstant(string name)
        {
            this.name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a description of the constant, given by a
        /// [Description] attribute on the class.
        /// </summary>
        public string Description
        {
            get
            {
                var objects = GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objects.Length == 0)
                    return "No description available.";

                var lines = new string[objects.Length];

                for (int i = 0; i < objects.Length; i++)
			    {
                    var da = (DescriptionAttribute)objects[i];
			        lines[i] = da.Description;
			    }

                return string.Join(Environment.NewLine, lines);
            }
        }

        /// <summary>
        /// Gets an URL with information for the constant, given by a
        /// [Link] attribute on the class.
        /// </summary>
        public string HyperReference
        {
            get
            {
                var objects = GetType().GetCustomAttributes(typeof(LinkAttribute), false);

                if (objects.Length == 0)
                    return string.Empty;

                return ((LinkAttribute)objects[0]).Url;
            }
        }

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
		public string Name
		{
			get { return name; }
		}

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
		public abstract object Value
		{
			get;
        }

        #endregion
    }
}
