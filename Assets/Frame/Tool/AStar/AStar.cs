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
            return Find(start, end);
        }

        public static void FindPath(Vector2 start, Vector2 end)
        {

        }

        public static void FindPath(AStarGirdPosition start, AStarGirdPosition end)
        {

        }
     

        private static List<AStarGirdPosition> Find(AStarGrid start, AStarGrid end)
        {
            if(m_AStarGrids!=null && start!= end && start.State==AStartGirdState.Through&& start.State == end.State)//判断起始点与终点是否都为可行走点
            {
                start.Prev = null;
                start.Distance = 0;
                end.Prev = null;
                end.Distance = 0;
                AStarGrid tempAStarGrid = start;
                List<AStarGrid> referAStarGirdLi = new List<AStarGrid>();
                List<AStarGrid> filterAStarGirdLi = new List<AStarGrid>();              
                while (true)
                {                          
                    #region 计算起始点上下左右的四个点与终点的距离并依照从大到小的顺序排序
                    FindAroundAStarGrid(tempAStarGrid, start, end, referAStarGirdLi,filterAStarGirdLi);
                    referAStarGirdLi.Sort((a, b) =>//从小->大排列
                    {
                        if (a.Distance >= b.Distance)
                        {
                            return 1;
                        }
                        return -1;
                    });
                    #endregion
                    if (referAStarGirdLi.Count == 0)
                    {
                        Debuger.Log("没有路可走");
                        break;
                    }
                    tempAStarGrid = referAStarGirdLi[0];
                    filterAStarGirdLi.Add(tempAStarGrid);
                    referAStarGirdLi.Remove(tempAStarGrid);               
                    if (Equals(tempAStarGrid.Position, end.Position))//判断添加的A星格子是否为终点的格子
                    {
                        List<AStarGirdPosition> Positions = new List<AStarGirdPosition>();
                        Positions.Add(tempAStarGrid.Position);
                        while (tempAStarGrid.Prev!=null)
                        {
                            Positions.Add(tempAStarGrid.Prev.Position);
                            tempAStarGrid = tempAStarGrid.Prev;
                        }
                        Positions.Reverse();//颠倒数组
                        return Positions;
                    }
                }            
            }
            return null;
        }

        /// <summary>
        /// 寻找A星格子周围的格子坐标(上下左右四个格子)(已添加A*格子状态判断)
        /// </summary>
        /// <param name="position">A星格子坐标</param>
        private static void FindAroundAStarGrid(AStarGrid aStarGrid,AStarGrid startGrid, AStarGrid endGrid,List<AStarGrid> referAStarGirdLi, List<AStarGrid> filterAStarGirdLi)
        {
            AStarGrid tempAStarGrid;
            for (var i = 0; i < 4; i++)
            {
                tempAStarGrid = m_AStarGrids[Mathf.Clamp(aStarGrid.Position.x + (int)Mathf.Sin(i * 90 * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(0)-1), Mathf.Clamp(aStarGrid.Position.y + (int)Mathf.Cos(i * 90 * Mathf.Deg2Rad), 0, m_AStarGrids.GetLength(1)-1)];
                //判断该点是否为可通行点
                if (tempAStarGrid.State == AStartGirdState.Through )
                {                  
                    if (!referAStarGirdLi.Contains(tempAStarGrid)&& !filterAStarGirdLi.Contains(tempAStarGrid))
                    {               
                        //计算该格子与起止点和终点的格子数
                        tempAStarGrid.Distance = tempAStarGrid.Position.Distance(startGrid.Position.ToVecto2()) + tempAStarGrid.Position.Distance(endGrid.Position);
                        tempAStarGrid.Prev = aStarGrid;//与上一个格子建立连接
                        referAStarGirdLi.Add(tempAStarGrid);
                    }
                }
            }
        }
    }
}
