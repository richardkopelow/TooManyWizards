using System;
using System.Collections.Generic;

public class CoroutineOut<T>
{
    public bool Done { get; set; }
    public T Data { get; set; }

    public static implicit operator T(CoroutineOut<T> val)
    {
        return val.Data;
    }
}
