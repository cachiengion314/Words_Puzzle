#define ENABLE_LOGS
public static class LogController
{
    public static void Debug(object logMsg)
    {
#if ENABLE_LOGS
        UnityEngine.Debug.Log(logMsg);
#endif
    }
}
