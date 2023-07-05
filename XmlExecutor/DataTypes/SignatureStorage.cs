using System;
using System.Collections.Generic;
using XmlExecutor.Factories;

namespace XmlExecutor.DataTypes
{
    public class SignatureStorage
    {
        private readonly IDictionary<Type, IEnumerable<MethodSignature>> _storage;

        private readonly AttributeUtils _attributeUtils;

        public SignatureStorage(AttributeUtils attributeUtils, IDictionary<Type, IEnumerable<MethodSignature>> storage)
        {
            this._storage = storage;
            _attributeUtils = attributeUtils;
        }

        public SignatureStorage(IDictionary<Type, IEnumerable<MethodSignature>> storage)
        {
            this._storage = storage;
        }

        public IEnumerable<MethodSignature> GetSignatures(Type type)
        {
            Type realType = OverloadUtils.OverLoadType(type);
            IEnumerable<MethodSignature> signatures;
            signatures = realType.IsGenericType ? _attributeUtils.GetSignatures(realType) : _storage[realType];
            return signatures;
        }
    }
}
