using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Extend;
using Farme.Tool;
using Farme;
public class TreasureBox : MonoBehaviour
{
    private MeshRenderer _mR;
    public Vector2 Position => transform.position;  
    private void Awake()
    {
        _mR=GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        AStarGrid aStarGrid = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(Position));
        aStarGrid.State = AStartGirdState.NotThrough;
    }
    // Start is called before the first frame update
    void Start()
    {
        _mR.material.color = Color.yellow;



    }

    private void OnDisable()
    {
        if (MapBuilder.Exists)
        {
            AStarGrid aStarGrid = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(Position));
            aStarGrid.State = AStartGirdState.Through;
        }
    }


    private void OnMouseDown()
    {
        //释放其他物品 
        if(!GoReusePool.Take("AGoods",out GameObject aGoodsGo))
        {
            if(!GoLoad.Take(PrefabURL.Cube,out aGoodsGo))
            { 

            }
            aGoodsGo.AddComponent<AGoods>();
        }
        aGoodsGo.name = "AGoods";
        aGoodsGo.GetComponent<AGoods>().Fall(this);
        //if (!GoReusePool.Take("BGoods", out GameObject bGoodsGo))
        //{
        //    if (!GoLoad.Take(PrefabURL.Cube, out bGoodsGo))
        //    {

        //    }
        //    bGoodsGo.AddComponent<BGoods>();
        //}
        //bGoodsGo.name = "BGoods";
        //bGoodsGo.GetComponent<BGoods>().Fall(this);
       
        //if (!GoReusePool.Take("CGoods", out GameObject cGoodsGo))
        //{
        //    if (!GoLoad.Take(PrefabURL.Cube, out cGoodsGo))
        //    {

        //    }
        //    cGoodsGo.AddComponent<CGoods>();
        //}
        //cGoodsGo.name = "CGoods";
        //cGoodsGo.GetComponent<CGoods>().Fall(this);

        gameObject.Recycle(gameObject.name);
    }


}
