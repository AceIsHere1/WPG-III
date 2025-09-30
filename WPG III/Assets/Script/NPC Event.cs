using UnityEngine;

public static class NPCEvents
{
    // event global, bisa dipanggil oleh NPCSpawner
    public static System.Action OnNpcDestroyed;

    // fungsi helper untuk raise event
    public static void RaiseNpcDestroyed()
    {
        if (OnNpcDestroyed != null)
        {
            OnNpcDestroyed.Invoke();
        }
    }
}
