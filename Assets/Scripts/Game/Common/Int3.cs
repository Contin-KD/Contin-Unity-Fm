namespace UnityEngine
{
    public struct Int3
    {
        /// <summary>
        /// 返回Point2(0,0)
        /// </summary>
        public static Int3 Zero { get { return new Int3(); } }
        /// <summary>
        /// 返回Point2(1,1)
        /// </summary>
        public static Int3 One { get { return new Int3(1, 1, 1); } }

        /// <summary>
        /// 模
        /// </summary>
        public float Magnitude
        {
            get { return Mathf.Sqrt(SqrMagnitude); }
        }

        /// <summary>
        /// 模的平方
        /// </summary>
        public int SqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public int x;
        public int y;
        public int z;

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Int3 operator +(Int3 lhs, Int3 rhs)
        {
            lhs.x += rhs.x;
            lhs.y += rhs.y;
            lhs.z += rhs.z;
            return lhs;
        }
        public static Int3 operator -(Int3 lhs, Int3 rhs)
        {
            lhs.x -= rhs.x;
            lhs.y -= rhs.y;
            lhs.z -= rhs.z;
            return lhs;
        }

        public static Int3 operator *(Int3 p, int n)
        {
            p.x *= n;
            p.y *= n;
            p.z *= n;
            return p;
        }
        public static Int3 operator /(Int3 p, int n)
        {
            p.x /= n;
            p.y /= n;
            p.z /= n;
            return p;
        }

        public static implicit operator Vector3(Int3 p)
        {
            return new Vector3(p.x, p.y, p.z);
        }

        public static explicit operator Int3(Vector3 v)
        {
            return new Int3((int)v.x, (int)v.y, (int)v.z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Int3 other = (Int3)obj;
            return x == other.x &&
                   y == other.y &&
                   z == other.z;
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = hash << 31 + x;
            hash = hash << 31 + y;
            hash = hash << 31 + z;
            return hash;
        }
    }
}