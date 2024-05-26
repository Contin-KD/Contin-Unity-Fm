using UnityEngine;

namespace TGame.Common
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/03/15
    /// 功能: 
    /// </summary>
	public static partial class Utility
    {
        public static class Math
        {
            /// <summary>
            /// 将角度转换到[0,360)区间
            /// </summary>
            /// <param name="angle"></param>
            /// <returns></returns>
            public static float NormalizeAngle(float angle)
            {
                return (angle % 360 + 360) % 360;
            }

            /// <summary>
            /// 将角度转换到(-180,180]区间
            /// </summary>
            /// <param name="angle"></param>
            /// <returns></returns>
            public static float NormalizeAngle180(float angle)
            {
                angle = NormalizeAngle(angle);
                if (angle > 180)
                {
                    angle -= 360;
                }
                return angle;
            }

            /// <summary>
            /// 四舍五入,保留小数点固定位数
            /// </summary>
            /// <param name="value">原值</param>
            /// <param name="digits">保留位数</param>
            /// <returns></returns>
            public static float Round(float value, int digits)
            {
                return (float)System.Math.Round(value, digits);
            }

            /// <summary>
            /// 获取从from到to的逆时针旋转角度
            /// </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            /// <returns></returns>
            public static float Angle360(Vector2 from, Vector2 to)
            {
                float cross = from.x * to.y - from.y * to.x;
                float angle = Vector2.Angle(from, to);
                if (cross < 0)
                {
                    angle = 360 - angle;
                }
                return angle;
            }

            /// <summary>
            /// 求一个值在一个区间内的归一化结果
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns>若区间有问题,返回0,否则返回该值得归一化结果</returns>
            public static float Normalize(float value, float min, float max)
            {
                min = Mathf.Min(min, max);
                max = Mathf.Max(min, max);
                value = Mathf.Clamp(value, min, max);

                if (min == max)
                    return 0;

                return (value - min) / (max - min);
            }

            /// <summary>
            /// 检查P点是否在由P1,P2,P3围成的三角形中
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="p3"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            public static bool CheckInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p)
            {
                Vector3 p1p = p1 - p;
                Vector3 p2p = p2 - p;
                Vector3 p3p = p3 - p;

                Vector3 crs12 = Vector3.Cross(p1p, p2p);
                Vector3 crs23 = Vector3.Cross(p2p, p3p);
                Vector3 crs31 = Vector3.Cross(p3p, p1p);

                return Vector3.Dot(crs12, crs23) > 0 &&
                       Vector3.Dot(crs23, crs31) > 0 &&
                       Vector3.Dot(crs31, crs12) > 0;
            }

            /// <summary>
            /// 检查P点是否在由P1,P2,P3,P4围成的矩形中,注意要按照某一绕序填
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="p3"></param>
            /// <param name="p4"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            public static bool CheckInRectangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p)
            {
                return CheckInTriangle(p1, p2, p3, p) ||
                       CheckInTriangle(p1, p3, p4, p);
            }

            public static bool CheckRectCircleIntersect(Vector3 rectPosition, Quaternion rectRotation, Vector3 rectSize, Vector3 circlePosition, float circleRadius, bool ignoreY = false)
            {
                if (ignoreY)
                {
                    rectPosition.y = 0;
                    rectSize.y = 0;
                    circlePosition.y = 0;
                }

                Vector3 v = circlePosition - rectPosition;
                v = Quaternion.Inverse(rectRotation) * v;
                v.x = Mathf.Abs(v.x);
                v.y = Mathf.Abs(v.y);
                v.z = Mathf.Abs(v.z);

                Vector3 h = rectSize / 2f;

                Vector3 u = v - h;
                u.x = Mathf.Max(u.x, 0);
                u.y = Mathf.Max(u.y, 0);
                u.z = Mathf.Max(u.z, 0);

                return Vector3.Dot(u, u) <= Mathf.Pow(circleRadius, 2);
            }
        }
    }
}