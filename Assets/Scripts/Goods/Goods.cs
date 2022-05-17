using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using Farme.Tool;
using Farme.Extend;
/// <summary>
/// 物品类型
/// </summary>
public enum EnumGoods
{
    A,
    B,
    C
}
/// <summary>
/// 物品
/// </summary>
public class Goods : MonoBase
{
    protected EnumGoods _goodsType = EnumGoods.A;
    protected static Dictionary<EnumGoods, List<Goods>> _goodsDic = new Dictionary<EnumGoods, List<Goods>>();
    protected MeshRenderer _mR;
    public Vector2 Position => transform.position;
    protected override void Awake()
    {
        base.Awake();
        _mR = GetComponent<MeshRenderer>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_goodsDic.TryGetValue(_goodsType, out List<Goods> goodsLi))
        {
            if (MapBuilder.GetSingleton().GetAStarGridNeighborAStarGrids(Position).Count >= 3)
            {
                goodsLi.Add(this);
            }
        }
        else
        {
            _goodsDic.Add(_goodsType, new List<Goods>() { this });
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_goodsDic.TryGetValue(_goodsType, out List<Goods> goodsLi))
        {
            goodsLi.Remove(this);
        }
        if (MapBuilder.Exists)
        {
            AStarGrid aStarGrid = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(Position));
            aStarGrid.State = AStartGirdState.Through;
        }
    }

    private void OnMouseDown()
    {


        gameObject.Recycle(gameObject.name);
    }



    public virtual void Fall(TreasureBox treasureBox)
    {
        if (_goodsDic.TryGetValue(_goodsType, out List<Goods> goodsLi))
        {
            List<AStarGrid> aStarGrids;
            List<Goods> temp = new List<Goods>();
            foreach (var goods in goodsLi)
            {
                if (!Equals(goods, this))
                {
                    temp.Add(goods);
                }
            }
            if (temp.Count == 0)
            {
                //则跌落在开启的那个宝箱的周围   待优化成最短路径
                aStarGrids = MapBuilder.GetSingleton().GetAStarGridNeighborAStarGrids(treasureBox.Position);
                if (aStarGrids.Count > 0)
                {
                    aStarGrids[0].State = AStartGirdState.NotThrough;
                    transform.position = (Vector3)aStarGrids[0].Position.ToVecto2() + Vector3.back;
                }
            }
            else
            {
                AStarGrid start = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(treasureBox.Position));
                if (temp.Count > 1)
                {
                    AStarGrid end1 = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(temp[0].Position));
                    AStarGrid end2 = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(temp[1].Position));
                    end1.State = AStartGirdState.Through;
                    //int i = AStar.FindPath(start, end1).Count;
                    //int y = AStar.FindPath(start, end2).Count;


                    Debuger.Log(AStar.FindPath(start, end1));
                    //temp.Sort((a, b) =>
                    //{
                    //    AStarGrid end1 = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(a.Position));
                    //    end1.State = AStartGirdState.Through;
                    //    AStarGrid end2 = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(b.Position));
                    //    end2.State = AStartGirdState.Through;
                    //    //Debug.Log("a:" + a.Position);
                    //    //Debug.Log("b:" + b.Position);
                    //    //Debug.Log(AStar.FindPath(start, end1));                        //Debug.Log(AStar.FindPath(start, end2));
                    //    //Debug.Log("end1:"+end1.Position.x+","+ end1.Position.y);
                    //    //Debug.Log("end2:"+end2.Position.x+","+end2.Position.y);

                    //    if (AStar.FindPath(start, end1).Count >= AStar.FindPath(start, end2).Count)
                    //    {
                    //        end1.State = AStartGirdState.NotThrough;
                    //        end2.State = AStartGirdState.NotThrough;
                    //        return 1;
                    //    }
                    //    end1.State = AStartGirdState.NotThrough;
                    //    end2.State = AStartGirdState.NotThrough;
                    //    return -1;
                    //});
                }
                //计算最优货物周围的点作为自身的落脚点
                aStarGrids = MapBuilder.GetSingleton().GetAStarGridNeighborAStarGrids(goodsLi[0].Position);
                aStarGrids[0].State = AStartGirdState.NotThrough;
                transform.position = (Vector3)aStarGrids[0].Position.ToVecto2() + Vector3.back;
            }
           
        }
    }
}
