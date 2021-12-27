using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.Tool
{
    /// <summary>
    /// A*寻路
    /// </summary>
    public class AStar
    {
        /// <summary>
        /// 起始点指向终点方向
        /// </summary>
        private static AStarGirdPosition m_StartToEndDir;
        /// <summary>
        /// A星格子数组
        /// </summary>
        private static AStarGrid[,] m_AStarGrids = null;
        /// <summary>
        /// A星格子数组
        /// </summary>
        public static AStarGrid[,] AStarGrids
        {
            set
            {
                m_AStarGrids = value;
            }
            get
            {
                return m_AStarGrids;
            }
        }
        public static List<AStarGirdPosition> FindPath(AStarGrid start, AStarGrid end)
        {
            return OptimalPath(start, end);
        }
        private static List<AStarGirdPosition> OptimalPath(AStarGrid start, AStarGrid end)
        {       
            return SingleWayPathFinding(start, end);
        }
        /// <summary>
        /// 双向寻路(待)
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        private static List<AStarGirdPosition> TwoWayPathFinding(AStarGrid start, AStarGrid end)
        {
            if (m_AStarGrids != null && start != end && start.State == AStartGirdState.Through && start.State == end.State)//判断起始点与终点是否都为可行走点
            {
                m_StartToEndDir = (end.Position - start.Position).Normalized;
                start.Prev = null;
                start.Distance = 0;
                start.ToStartDistance = 0;
                start.ToEndDistance = 0;
                end.Prev = null;
                end.Distance = 0;
                end.ToStartDistance = 0;
                end.ToEndDistance = 0;
                AStarGrid tempAStarGrid_StartToEnd = start;//已start为起点
                AStarGrid tempAStarGrid_EndToStart = end;//已end为起点
                List<AStarGrid> openLi_StartToEnd = new List<AStarGrid>();
                List<AStarGrid> closeLi_StartToEnd = new List<AStarGrid>();
                //List<AStarGrid> openLi_EndToStart = new List<AStarGrid>();
                //List<AStarGrid> closeLi_EndToStart = new List<AStarGrid>();
                while (true)
                {
                    #region 计算起始点的八个点与终点的距离并依照从大到小的顺序排序
                    // 从头部开始搜寻
                    FindNeighborAStarGrid(tempAStarGrid_StartToEnd, end, openLi_StartToEnd, closeLi_StartToEnd);
                    //从尾部开始搜寻
                    //FindNeighborAStarGrid(tempAStarGrid_EndToStart, tempAStarGrid_StartToEnd, openLi_EndToStart, closeLi_EndToStart);
                    //StartToEnd方向的排序
                    openLi_StartToEnd.Sort((a, b) =>//从小->大排列
                    {
                        if ((a.ToStartDistance + a.ToEndDistance) >= (b.ToStartDistance + b.ToEndDistance))
                        {
                            return 1;
                        }
                        return -1;
                    });
                    //EndToStart方向的排序
                    //openLi_EndToStart.Sort((a, b) =>//从小->大排列
                    //{
                    //    if ((a.ToStartDistance + a.ToEndDistance) >= (b.ToStartDistance + b.ToEndDistance))
                    //    {
                    //        return 1;
                    //    }
                    //    return -1;
                    //});
                    #endregion
                    if (openLi_StartToEnd.Count == 0/*|| openLi_EndToStart.Count==0*/)
                    {
                        Debuger.Log("没有路可走");
                        break;
                    }
                    tempAStarGrid_StartToEnd = openLi_StartToEnd[0];
                    //tempAStarGrid_EndToStart = openLi_EndToStart[0];
                    closeLi_StartToEnd.Add(tempAStarGrid_StartToEnd);
                    //closeLi_EndToStart.Add(tempAStarGrid_EndToStart);
                    openLi_StartToEnd.Remove(tempAStarGrid_StartToEnd);
                   // openLi_EndToStart.Remove(tempAStarGrid_EndToStart);
                    //if (Equals(tempAStarGrid_StartToEnd.Position, tempAStarGrid_EndToStart.Position))
                    {
                        //List<AStarGirdPosition> Positions = new List<AStarGirdPosition>();
                        ////获取前半段的路径
                        //tempAStarGrid_StartToEnd = closeLi_StartToEnd[closeLi_StartToEnd.Count - 2];
                        //Positions.Add(tempAStarGrid_StartToEnd.Position);
                        //while (tempAStarGrid_StartToEnd.Prev != null)
                        //{
                        //    Positions.Add(tempAStarGrid_StartToEnd.Prev.Position);
                        //    tempAStarGrid_StartToEnd = tempAStarGrid_StartToEnd.Prev;
                        //}
                        //Positions.Reverse();//归正
                        //获取后半段的路径
                        //Positions.Add(tempAStarGrid_EndToStart.Position);
                        //while (tempAStarGrid_EndToStart.Prev != null)
                        //{
                        //    Positions.Add(tempAStarGrid_EndToStart.Prev.Position);
                        //    tempAStarGrid_EndToStart = tempAStarGrid_StartToEnd.Prev;
                        //}                      
                        //return Positions;
                    }
                    if (Equals(tempAStarGrid_StartToEnd.Position, end.Position))//判断添加的A星格子是否为终点的格子
                    {
                        List<AStarGirdPosition> Positions = new List<AStarGirdPosition>();
                        Positions.Add(tempAStarGrid_StartToEnd.Position);
                        while (tempAStarGrid_StartToEnd.Prev != null)
                        {
                            Positions.Add(tempAStarGrid_StartToEnd.Prev.Position);
                            tempAStarGrid_StartToEnd = tempAStarGrid_StartToEnd.Prev;
                        }
                        Positions.Reverse();
                        return Positions;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 单向寻路
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        private static List<AStarGirdPosition> SingleWayPathFinding(AStarGrid start, AStarGrid end)
        {
            if (m_AStarGrids != null && start != end && start.State == AStartGirdState.Through && start.State == end.State)//判断起始点与终点是否都为可行走点
            {
                m_StartToEndDir = (end.Position - start.Position).Normalized;
                start.Prev = null;
                start.Distance = 0;
                start.ToStartDistance = 0;
                start.ToEndDistance = 0;
                end.Prev = null;
                end.Distance = 0;
                end.ToStartDistance = 0;
                end.ToEndDistance = 0;
                AStarGrid tempAStarGrid = end;//已end为起点
                List<AStarGrid> openLi = new List<AStarGrid>();
                List<AStarGrid> closeLi = new List<AStarGrid>();
                while (true)
                {
                    #region 计算起始点的八个点与终点的距离并依照从大到小的顺序排序
                    // 从头部开始搜寻
                    FindNeighborAStarGrid(tempAStarGrid, start, openLi, closeLi);
                    openLi.Sort((a, b) =>//从小->大排列
                    {
                        if ((a.ToStartDistance + a.ToEndDistance) >= (b.ToStartDistance + b.ToEndDistance))
                        {
                            return 1;
                        }
                        return -1;
                    });
                    
                    #endregion
                    if (openLi.Count == 0)
                    {
                        Debuger.Log("没有路可走");
                        break;
                    }
                    tempAStarGrid = openLi[0];
                    closeLi.Add(tempAStarGrid);
                    openLi.Remove(tempAStarGrid);           
                    if (Equals(tempAStarGrid.Position, start.Position))//判断添加的A星格子是否为终点的格子
                    {
                        List<AStarGirdPosition> Positions = new List<AStarGirdPosition>();
                        Positions.Add(tempAStarGrid.Position);
                        while (tempAStarGrid.Prev != null)
                        {
                            Positions.Add(tempAStarGrid.Prev.Position);
                            tempAStarGrid = tempAStarGrid.Prev;
                        }
                        return Positions;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 寻找A星格子相邻的八个A格子
        /// </summary>
        /// <param name="target">目标坐标</param>
        /// <param name="end">目标A星格子</param>
        /// <param name="openLi">开放列表(待考虑)</param>
        /// <param name="closeLi">关闭列表(确定的)</param>
        private static void FindNeighborAStarGrid(AStarGrid target,AStarGrid end, List<AStarGrid> openLi,List<AStarGrid> closeLi)
        {
            int x;//X坐标
            int y;//Y坐标
            bool isObliqueDirection;//是否为斜方向
            for (var angle = 45; angle <= 360;)
            {
                /*(1,1) (1,0)  (1,-1)  (0,1)  (0,-1)  (0,0)  (-1,0) (-1,-1) */
                isObliqueDirection = (angle % 2 == 0);
                x = Mathf.Clamp((int)target.Position.x +//初始坐标
                       (isObliqueDirection ? (int)Mathf.Sin(angle * Mathf.Deg2Rad) : (Mathf.Sin(angle * Mathf.Deg2Rad) > 0 ? 1 : -1)), //变动坐标
                       0, m_AStarGrids.GetLength(0) - 1);//范围限制
                y = Mathf.Clamp((int)target.Position.y + //初始坐标
                    (isObliqueDirection ? (int)Mathf.Cos(angle * Mathf.Deg2Rad) : (Mathf.Cos(angle * Mathf.Deg2Rad) > 0 ? 1 : -1)),//范围限制
                    0, m_AStarGrids.GetLength(1) - 1);
                if (!openLi.Contains(m_AStarGrids[x, y]) && !closeLi.Contains(m_AStarGrids[x, y]))
                {
                    if (m_AStarGrids[x, y].State == AStartGirdState.Through && (isObliqueDirection ? true :
                        (
                         m_AStarGrids
                            [
                                 Mathf.Clamp((int)target.Position.x + (int)Mathf.Sin((angle + 45f) * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(0) - 1),
                                 Mathf.Clamp((int)target.Position.x + (int)Mathf.Cos((angle + 45f) * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(1) - 1)
                            ].State == AStartGirdState.Through
                                     ||
                            m_AStarGrids
                            [
                                 Mathf.Clamp((int)target.Position.x + (int)Mathf.Sin((angle - 45f) * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(0) - 1),
                                 Mathf.Clamp((int)target.Position.x + (int)Mathf.Cos((angle - 45f) * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(1) - 1)
                            ].State == AStartGirdState.Through
                        )))
                    {
                        //计算离起始点的距离
                        m_AStarGrids[x, y].ToStartDistance = target.ToStartDistance + (isObliqueDirection ? 1f :Mathf.Sqrt(2.0f));
                        #region 不同的启发式会对应不同的结果  若ToEndDistance过小,会逐渐退化成BFS算法
                        //计算离终点的距离    目前采用的启发式为曼哈顿距离作为影响值
                        m_AStarGrids[x, y].ToEndDistance = m_AStarGrids[x, y].Position.ManhattonDistance(end.Position);
                        #endregion
                        //连接上一个A星格子
                        m_AStarGrids[x, y].Prev = target;
                        //添加到参考A星格子列表当中
                        openLi.Add(m_AStarGrids[x, y]);
                    }
                }
                angle += 45;
            }

        }
    }
}
