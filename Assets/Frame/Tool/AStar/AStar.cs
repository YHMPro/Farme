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

        public static List<AStarGirdPosition> FindPath(Vector2 start, Vector2 end)
        {
            AStarGirdPosition AStarPosStart = AStarGirdPosition.ToAStarGirdPosition(start);
            AStarGirdPosition AStarPosEnd = AStarGirdPosition.ToAStarGirdPosition(end);
            OptimalPath(m_AStarGrids[(int)AStarPosStart.x, (int)AStarPosStart.y], m_AStarGrids[(int)AStarPosEnd.x, (int)AStarPosEnd.y]);
            return null;
        }

        public static List<AStarGirdPosition> FindPath(AStarGirdPosition start, AStarGirdPosition end)
        {
            return OptimalPath(m_AStarGrids[(int)start.x, (int)start.y], m_AStarGrids[(int)end.x, (int)end.y]);
        }
        

        private static List<AStarGirdPosition> OptimalPath(AStarGrid start, AStarGrid end)
        {
            List<AStarGirdPosition> path1 = GetPath(start,end);
            //List<AStarGirdPosition> path2 = GetPath(end, start);
            path1.Reverse();
            return path1;
                //path1.Count >= path2.Count ? path2 : path1;
        }

        private static List<AStarGirdPosition> GetPath(AStarGrid start, AStarGrid end)
        {
            if(m_AStarGrids!=null && start!= end && start.State==AStartGirdState.Through&& start.State == end.State)//判断起始点与终点是否都为可行走点
            {
                m_StartToEndDir= (end.Position-start.Position).Normalized;
                start.Prev = null;
                start.Distance = 0;
                start.ToStartDistance = 0;
                start.ToEndDistance = 0;
                end.Prev = null;
                end.Distance = 0;
                end.ToStartDistance = 0;
                end.ToEndDistance = 0;
                AStarGrid tempAStarGrid = start;
                List<AStarGrid> openLi = new List<AStarGrid>();
                List<AStarGrid> closeLi = new List<AStarGrid>();              
                while (true)
                {                          
                    #region 计算起始点上下左右的四个点与终点的距离并依照从大到小的顺序排序
                    //FindAroundAStarGrid(tempAStarGrid, start, end, referAStarGirdLi,filterAStarGirdLi);
                    FindNeighborAStarGrid(tempAStarGrid,end, openLi, closeLi);
                    openLi.Sort((a, b) =>//从小->大排列
                    {
                        if ((a.ToStartDistance+a.ToEndDistance) >= (b.ToStartDistance + b.ToEndDistance))
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
                    Debuger.Log(tempAStarGrid.Position.ToVecto2());
                    Debuger.Log(end.Position.ToVecto2());
                    if (Equals(tempAStarGrid.Position, end.Position))//判断添加的A星格子是否为终点的格子
                    {
                        List<AStarGirdPosition> Positions = new List<AStarGirdPosition>();
                        Positions.Add(tempAStarGrid.Position);
                        while (tempAStarGrid.Prev!=null)
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
        /// 寻找A星格子相邻的四个格子
        /// </summary>
        /// <param name="aStarGrid">A星格子</param>
        /// <param name="startGrid">起点</param>
        /// <param name="endGrid">终点</param>
        /// <param name="referAStarGirdLi">参考A星格子列表</param>
        /// <param name="filterAStarGirdLi">过滤A星格子列表</param>
        private static void FindAroundAStarGrid(AStarGrid aStarGrid,AStarGrid startGrid, AStarGrid endGrid,List<AStarGrid> referAStarGirdLi, List<AStarGrid> filterAStarGirdLi)
        {
            AStarGrid tempAStarGrid;
            for (var i = 0; i < 4; i++)
            {
                tempAStarGrid = m_AStarGrids[Mathf.Clamp((int)aStarGrid.Position.x + (int)Mathf.Sin(i * 90 * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(0)-1), Mathf.Clamp((int)aStarGrid.Position.y + (int)Mathf.Cos(i * 90 * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(1)-1)];
                //判断该点是否为可通行点
                if (tempAStarGrid.State == AStartGirdState.Through )
                {                  
                    if (!referAStarGirdLi.Contains(tempAStarGrid)&& !filterAStarGirdLi.Contains(tempAStarGrid))
                    {
                        //计算该格子与起止点、终点的距离  欧氏距离+曼哈顿距离+自身下一步的走向方向与起点指向终点方向的相似度
                        tempAStarGrid.Distance = tempAStarGrid.Position.EuclideanDistance(startGrid.Position) + tempAStarGrid.Position.ManhattonDistance(endGrid.Position) + (tempAStarGrid.Position - aStarGrid.Position).Normalized.Cross(m_StartToEndDir);
                        tempAStarGrid.Prev = aStarGrid;//与上一个格子建立连接
                        referAStarGirdLi.Add(tempAStarGrid);//添加到参考A星格子列表当中
                    }
                }
                else
                {
                    startGrid = aStarGrid;
                }
            }
        }



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
                AStarGrid tempAStarGrid = start;
                List<AStarGrid> openLi = new List<AStarGrid>();
                List<AStarGrid> closeLi = new List<AStarGrid>();
                while (true)
                {
                    #region 计算起始点上下左右的四个点与终点的距离并依照从大到小的顺序排序
                    //FindAroundAStarGrid(tempAStarGrid, start, end, referAStarGirdLi,filterAStarGirdLi);
                    FindNeighborAStarGrid(tempAStarGrid, end, openLi, closeLi);
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
                    Debuger.Log(tempAStarGrid.Position.ToVecto2());
                    Debuger.Log(end.Position.ToVecto2());
                    if (Equals(tempAStarGrid.Position, end.Position))//判断添加的A星格子是否为终点的格子
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
        private static void FindNeighborAStarGrid(AStarGrid target,AStarGrid end,List<AStarGrid> openLi,List<AStarGrid> closeLi)
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
                        m_AStarGrids[x, y].ToStartDistance = target.ToStartDistance + (isObliqueDirection ? 1f : m_AStarGrids[x, y].Position.EuclideanDistance(target.Position));
                        //计算离终点的距离
                        m_AStarGrids[x, y].ToEndDistance = m_AStarGrids[x, y].Position.ManhattonDistance(end.Position);
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
