using UnityEngine;

public static class ResourceCleaner
{
    // 调用此方法来手动触发垃圾回收
    public static void CleanUp()
    {
        // 强制垃圾回收
        System.GC.Collect();
        // 等待所有挂起的终结器完成
        System.GC.WaitForPendingFinalizers();
        // 再次强制垃圾回收以确保所有终结器都已运行
        System.GC.Collect();

        Debug.Log("Garbage collection completed.");
    }
}