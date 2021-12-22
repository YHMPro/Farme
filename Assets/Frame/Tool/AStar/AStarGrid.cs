using UnityEngine;
namespace Farme.Tool
{
    /// <summary>
    /// A星格子坐标
    /// </summary>
    public struct AStarGirdPosition
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public float x;
        /// <summary>
        /// Y坐标
        /// </summary>
        public float y;
        /// <summary>
        /// 单位化
        /// </summary>
        public AStarGirdPosition Normalized
        {
            get
            {
                float length = Mathf.Sqrt(x * x + y * y);
                x = x / length;
                y = y / length;
                return this;
            }
        }
        public AStarGirdPosition(float x,float y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 计算两个A星坐标的叉积(相似度)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float Cross(AStarGirdPosition pos)
        {
            return Mathf.Abs(x * pos.y - pos.x *y);           
        }
        /// <summary>
        /// 欧氏距离(计算自身与目标坐标的距离)
        /// </summary>
        /// <param name="pos">目标A星坐标</param>
        /// <returns>欧氏距离</returns>
        public float EuclideanDistance(AStarGirdPosition pos)
        {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(pos.x - x), 2) + Mathf.Pow(Mathf.Abs( pos.y - y), 2));
        }
        /// <summary>
        /// 曼哈顿距离(计算自身与目标坐标的距离)
        /// </summary>
        /// <param name="pos">目标A星坐标</param>
        /// <returns>曼哈顿距离</returns>
        public float ManhattonDistance(AStarGirdPosition pos)
        {
            return Mathf.Abs(pos.x - x) + Mathf.Abs(pos.y - y);
        }     
        /// <summary>
        /// A星格子坐标转世界坐标
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVecto2()
        {
            return AStarConfig.AStarOriginPosition + new Vector2(x * AStarConfig.PrecisionX, y * AStarConfig.PrecisionY);
        }
        /// <summary>
        /// 世界坐标转A星格子坐标
        /// </summary>
        /// <param name="v">世界坐标</param>
        /// <returns></returns>
        public static AStarGirdPosition ToAStarGirdPosition(Vector2 v)
        {
            v = v - AStarConfig.AStarOriginPosition;
            return new AStarGirdPosition()
            {
                x = (int)(v.x / AStarConfig.PrecisionX),
                y = (int)(v.y / AStarConfig.PrecisionY)
            };
        }           
        public static AStarGirdPosition operator +(AStarGirdPosition a, AStarGirdPosition b)
        {
            return new AStarGirdPosition() { x=a.x+b.x, y=a.y+b.y};
        }
        public static AStarGirdPosition operator -(AStarGirdPosition a, AStarGirdPosition b)
        {
            return new AStarGirdPosition() { x = a.x - b.x, y = a.y - b.y };
        }
    }
    /// <summary>
    /// A星格子状态
    /// </summary>
    public enum AStartGirdState
    {
        Through,
        NotThrough,
        Other
    }
    /// <summary>
    /// A星格子
    /// </summary>
    public class AStarGrid
    {
        /// <summary>
        /// A星格子位置
        /// </summary>
        public AStarGirdPosition Position;
        /// <summary>
        /// A星格子状态
        /// </summary>
        public AStartGirdState State;
        /// <summary>
        /// 连接的上一个A星格子
        /// </summary>
        public AStarGrid Prev;
        /// <summary>
        /// 自身与起点、终点的距离之和   与起点为欧式距离   与终点为曼哈顿距离
        /// </summary>
        public float Distance;
        /// <summary>
        /// 距离起始A星格子的距离
        /// </summary>
        public float ToStartDistance;
        /// <summary>
        /// 距离终点A星格子的距离
        /// </summary>
        public float ToEndDistance;
        public AStarGrid()
        {
            Position = new AStarGirdPosition() { x=0,y=0 };
            State = AStartGirdState.Through;         
            Distance= 0;
            Prev = null;
        }
    }
}
