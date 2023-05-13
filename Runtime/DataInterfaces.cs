using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adruian.CodeInjection
{
    public interface IDataListener<T>
    {
        public Type Type => typeof(T);

        void VariableChanged(T value);
    }

    public interface IDataCaller<T>
    {
        event Action<T> OnVariableChanged;
    }

    public interface IDataInjector
    {
        void FindAllListeners();
    }
}