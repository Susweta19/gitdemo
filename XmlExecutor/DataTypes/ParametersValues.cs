using System;
using System.Collections.Generic;

namespace XmlExecutor.DataTypes
{
    public class ParametersValues
    {
        public IDictionary<string, object> Values { get; private set; }
        public ICollection<Type> GenTypes { get; private set; }
        public object ExpectedReturn { get; private set; }
        public string Hashtag { get; private set; }

        public ParametersValues(IDictionary<string, object> values, object expectedReturn = null, ICollection<Type> genTypes = null, string hashtag = null)
        {
            Values = values;
            GenTypes = genTypes;
            ExpectedReturn = expectedReturn;
            Hashtag = hashtag;
        }

    }
}
