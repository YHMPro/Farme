using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using Farme;
public class MapBuilder : MonoSingletonBase<MapBuilder>
{

    public int h;
    public int v;
    float offsetX = -5;
    float offsetY = -5;
    private Dictionary<string,AStarGrid> _aStarGridDic=new Dictionary<string, AStarGrid>();
    protected override void Awake()
    {
        base.Awake();
        Builder();
    }
    private void Builder()
    {
        offsetX = -h / 2f;
        offsetY = -v / 2f;
        AStarConfig.AStarOriginPosition = new Vector2(offsetX, offsetY);
        AStar.AStarGrids = new AStarGrid[h, v];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < v; j++)
            {
                if (GoLoad.Take("Cube", out GameObject go,transform))
                {
                    AStar.AStarGrids[i, j] = new AStarGrid() { Position = new AStarGirdPosition() { x = i, y = j } };
                    go.name = i + "," + j;
                    go.transform.position = new Vector3(offsetX + i * 1, offsetY + j * 1, 0);
                    _aStarGridDic.Add(i + "," + j, AStar.AStarGrids[i, j]);
                }
            }
        }
    }

    public List<AStarGrid> GetAStarGridNeighborAStarGrids(Vector2 position)
    {
        AStarGrid aStarGrid = this.GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(position));
        List<AStarGrid> aStarGridLi = new List<AStarGrid>();
        int x;//X坐标
        int y;//Y坐标
        bool isObliqueDirection;//是否为斜方向
        for (var angle = 45; angle <= 360;)
        {
            /*(1,1) (1,0)  (1,-1)  (0,1)  (0,-1)  (0,0)  (-1,0) (-1,-1) */
            isObliqueDirection = (angle % 2 == 0);
            x = Mathf.Clamp((int)aStarGrid.Position.x +//初始坐标
                   (isObliqueDirection ? (int)Mathf.Sin(angle * Mathf.Deg2Rad) : (Mathf.Sin(angle * Mathf.Deg2Rad) > 0 ? 1 : -1)), //变动坐标
                   0, AStar.AStarGrids.GetLength(0) - 1);//范围限制
            y = Mathf.Clamp((int)aStarGrid.Position.y + //初始坐标
                (isObliqueDirection ? (int)Mathf.Cos(angle * Mathf.Deg2Rad) : (Mathf.Cos(angle * Mathf.Deg2Rad) > 0 ? 1 : -1)),//范围限制
                0, AStar.AStarGrids.GetLength(1) - 1);           
            angle += 45;
            if(AStar.AStarGrids[x,y].State==AStartGirdState.Through)
            {
                if((!aStarGridLi.Contains(AStar.AStarGrids[x, y])))
                {
                    aStarGridLi.Add(AStar.AStarGrids[x, y]);
                }
            }        
        }
        return aStarGridLi;
    }

    public AStarGrid GetAStarGrid(AStarGirdPosition position)
    {
        if(_aStarGridDic.TryGetValue(position.x+","+ position.y,out AStarGrid aStarGrid))
        {
            return aStarGrid;
        }
        Debug.Log(position.x + "," + position.y);
        return null;
    }
    /// <summary>
    /// 获取所有可通行的格子
    /// </summary>
    /// <returns></returns>
    public List<AStarGrid> GetAllThroughGrid()
    {
        List<AStarGrid> aStarGrids = new List<AStarGrid>();
        foreach(AStarGrid grid in AStar.AStarGrids)
        {
            if(grid.State==AStartGirdState.Through)
            {
                aStarGrids.Add(grid);
            }
        }
        return aStarGrids;
    }
}
