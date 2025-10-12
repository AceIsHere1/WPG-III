using System;

public static class GameEvents
{
    public static Action OnSesajenDisposed;
    public static System.Action OnNpcServed;
    public static System.Action OnGameWon;

    public static void RaiseSesajenDisposed() => OnSesajenDisposed?.Invoke();
    public static void RaiseNpcServed() => OnNpcServed?.Invoke();
    public static void RaiseGameWon() => OnGameWon?.Invoke();
}
