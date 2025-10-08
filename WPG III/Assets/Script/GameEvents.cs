using System;

public static class GameEvents
{
    public static Action OnSesajenDisposed;

    public static void RaiseSesajenDisposed() => OnSesajenDisposed?.Invoke();
}
