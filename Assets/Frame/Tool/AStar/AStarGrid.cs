using System.Collections;
using System.Collections.Generic;
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
        public int x;
        /// <summary>
        /// Y坐标
        /// </summary>
        public int y;
        /// <summary>
        /// 计算A星格子与某世界坐标的距离
        /// </summary>
        /// <param name="v">世界坐标</param>
        /// <returns>Unity位置之间的距离</returns>
        public float Distance(Vector2 v)
        {
           return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(x * AStarConfig.PrecisionX - (v.x- AStarConfig.AStarOriginPosition.x)), 2) + Mathf.Pow(Mathf.Abs(y * AStarConfig.PrecisionY - (v.y- AStarConfig.AStarOriginPosition.y)), 2));
        }
        /// <summary>
        /// 计算A星格子与另一个A星格子的距离
        /// </summary>
        /// <param name="a">目标A星格子</param>
        /// <returns></returns>
        public float Distance(AStarGirdPosition a)
        {
            return Mathf.Abs(a.x-x)*AStarConfig.PrecisionX+Mathf.Abs(a.y-y)* AStarConfig.PrecisionY;
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
        public AStarGrid()
        {
            Position = new AStarGirdPosition() { x=0,y=0 };
            State = AStartGirdState.Through;         
            Distance= 0;
            Prev = null;           
        }
    }
}
