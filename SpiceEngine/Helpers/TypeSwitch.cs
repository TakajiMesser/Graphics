using System;
using System.Collections.Generic;

namespace SpiceEngine.Helpers
{
    public class TypeSwitch<U>
    {
        private Dictionary<Type, Func<U>> _returnFuncByType = new Dictionary<Type, Func<U>>();
        private Func<U> _defaultReturnFunc;

        public TypeSwitch<U> Case<T>(Func<U> returnFunc)
        {
            _returnFuncByType.Add(typeof(T), returnFunc);
            return this;
        }

        public TypeSwitch<U> Default(Func<U> returnFunc)
        {
            _defaultReturnFunc = returnFunc;
            return this;
        }

        public U Match<T>()
        {
            if (_returnFuncByType.ContainsKey(typeof(T)))
            {
                return _returnFuncByType[typeof(T)].Invoke();
            }
            else if (_defaultReturnFunc != null)
            {
                return _defaultReturnFunc.Invoke();
            }
            else
            {
                return default;
            }
        }
    }
}
