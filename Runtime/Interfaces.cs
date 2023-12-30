using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adruian.CodeInjection
{

    public interface IDataListener<T>
    {
        void VariableChanged(T value);
    }

    public interface IDataCaller<T>
    {
        event Action<T> OnVariableChanged;
    }

    public interface IDataInjector
    {
        string Type { get; }
        void FindAllListeners();
    }

    public interface IDataInjection
    {
        string Type { get; }
    }
}