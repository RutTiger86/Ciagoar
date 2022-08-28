using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Core.Attributes
{
    /// <summary>
    ///     Specifies the database column that a property is mapped to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomColumnAttributes : Attribute
    {
        private int _order = -1;
        private string _typeName;
        private double _minWidth = 0;
        private double _starWidth = 0;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomColumnAttributes" /> class.
        /// </summary>
        public CustomColumnAttributes()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomColumnAttributes" /> class.
        /// </summary>
        /// <param name="name">The name of the column the property is mapped to.</param>
        public CustomColumnAttributes(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name of the column the property is mapped to.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The zero-based order of the column the property is mapped to.
        /// </summary>
        public int Order
        {
            get => _order;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _order = value;
            }
        }

        /// <summary>
        ///     The database provider specific data type of the column the property is mapped to.
        /// </summary>
        [DisallowNull]
        public string TypeName
        {
            get => _typeName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException();
                }

                _typeName = value;
            }
        }

        public double MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value;
            }
        }

        public double StarWidth
        {
            get => _starWidth;
            set
            {
                _starWidth = value;
            }
        }
    }
}
