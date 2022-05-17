using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGoods : Goods
{
    protected override void Awake()
    {
        _goodsType = EnumGoods.B;
        base.Awake();

        

    }

    protected override void Start()
    {
        base.Start();
        _mR.material.color = Color.red;
    }
}
