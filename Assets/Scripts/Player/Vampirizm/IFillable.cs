using System;

public interface IFillable
{
    event Action<float> ValueChanged;
}