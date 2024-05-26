using UnityEngine;

/// <summary>
/// 作者: Teddy
/// 时间: 2018/06/07
/// 功能: 
/// </summary>
public static class VectorExt
{
    public static Vector3 ToXZPlane(this Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }

    public static string ToFullString(this Vector3 vector)
    {
        return $"[{vector.x},{vector.y},{vector.z}]";
    }
}