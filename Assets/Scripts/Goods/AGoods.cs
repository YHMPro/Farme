using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
public class AGoods : Goods
{
    protected override void Awake()
    {
        _goodsType = EnumGoods.A;       
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        _mR.material.color = Color.blue;



       
    }
}
