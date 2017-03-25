namespace IocServiceStack
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class contains  attributes of declared attributes of type.
    /// </summary>
    public sealed class TypeContextAttributes
    {
        /// <summary>
        /// Gets list of attributes
        /// </summary>
        public readonly IEnumerable<Attribute> Attributes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        public TypeContextAttributes(IEnumerable<Attribute> attributes)
        {
            Attributes = attributes;
        }

        /// <summary>
        /// Determines whether Attributes has specified attribute type.
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public bool HasAttribute(Type attrType)
        {
            if (Attributes != null)
            {
                foreach (var attr in Attributes)
                {
                    if (attr.GetType() == attrType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
