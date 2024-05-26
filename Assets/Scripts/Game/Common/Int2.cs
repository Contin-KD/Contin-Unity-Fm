namespace UnityEngine
{
    public struct Int2
    {
        /// <summary>
        /// 返回Point2(0,0)
        /// </summary>
        public static Int2 Zero { get { return new Int2(); } }
        /// <summary>
        /// 返回Point2(1,1)
        /// </summary>
        public static Int2 One { get { return new Int2(1, 1); } }

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
            get { return x * x + y * y; }
        }

        public int x;
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Int2 operator +(Int2 lhs, Int2 rhs)
        {
            lhs.x += rhs.x;
            lhs.y += rhs.y;
            return lhs;
        }
        public static Int2 operator -(Int2 lhs, Int2 rhs)
        {
            lhs.x -= rhs.x;
            lhs.y -= rhs.y;
            return lhs;
        }

        public static Int2 operator *(Int2 p, int n)
        {
            p.x *= n;
            p.y *= n;
            return p;
        }
        public static Int2 operator /(Int2 p, int n)
        {
            p.x /= n;
            p.y /= n;
            return p;
        }
        public static bool operator ==(Int2 v1, Int2 v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Int2 v1, Int2 v2)
        {
            return !v1.Equals(v2);
        }

        public static implicit operator Vector2(Int2 p)
        {
            return new Vector2(p.x, p.y);
        }

        public static explicit operator Int2(Vector2 v)
        {
            return new Int2((int)v.x, (int)v.y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Int2 other = (Int2)obj;
            return x == other.x &&
                   y == other.y;
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = hash << 31 + x;
            hash = hash << 31 + y;
            return hash;
        }
    }
}