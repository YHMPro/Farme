using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using Farme;
using Farme.Tool;
using System;
public class ControlPanel : BasePanel
{
    private ElasticBtn _treasureBoxCreateBtn;

    private ElasticBtn _barrierCreateBtn;



    protected override void Awake()
    {
        base.Awake();
        RegisterComponentsTypes<ElasticBtn>();


        _treasureBoxCreateBtn=GetComponent<ElasticBtn>("TreasureBoxBtn");
        _barrierCreateBtn = GetComponent<ElasticBtn>("BarrierBtn");
    }

    protected override void Start()
    {
        base.Start();
        _treasureBoxCreateBtn.onClick.AddListener(OnTreasureBoxClick);
        _barrierCreateBtn.onClick.AddListener(OnBarrierClick);
    }



    #region BtnEvent
    private void OnTreasureBoxClick()
    {
        //在地图上随机创建一个宝箱
        if(!GoReusePool.Take("TreasureBox",out GameObject treasureBoxGo))
        {
            if(!GoLoad.Take(PrefabURL.Cube,out treasureBoxGo))
            {
                return;
            }
            treasureBoxGo.AddComponent<TreasureBox>();
        }
        treasureBoxGo.name = "TreasureBox";
        List<AStarGrid> aStarGrids = MapBuilder.GetSingleton().GetAllThroughGrid();
        System.Random rand = new System.Random();
        treasureBoxGo.transform.position = (Vector3)aStarGrids[rand.Next(0, aStarGrids.Count - 1)].Position.ToVecto2() + Vector3.back;
    }

    private void OnBarrierClick()
    {
        //在地图上随机创建一个障碍物
        if (!GoReusePool.Take("Barrier", out GameObject barrierGo))
        {
            if (!GoLoad.Take(PrefabURL.Cube, out barrierGo))
            {
                return;
            }
            barrierGo.AddComponent<Barrier>();
        }
        barrierGo.name = "Barrier";   
        List<AStarGrid> aStarGrids = MapBuilder.GetSingleton().GetAllThroughGrid();
        System.Random rand = new System.Random();
        barrierGo.transform.position = (Vector3)aStarGrids[rand.Next(0, aStarGrids.Count - 1)].Position.ToVecto2() + Vector3.back;
    }
    #endregion
}
