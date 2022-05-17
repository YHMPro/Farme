using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGoods : Goods
{
    protected override void Awake()
    {
        _goodsType = EnumGoods.C;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _mR.material.color = Color.green;
    }
}
