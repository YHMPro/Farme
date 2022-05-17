using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Extend;
using Farme.Tool;
public class Barrier : MonoBehaviour
{
    private MeshRenderer _mR;
    public Vector2 Position => transform.position;
    private void Awake()
    {
        _mR = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        AStarGrid aStarGrid = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(Position));
        aStarGrid.State = AStartGirdState.NotThrough;
    }
    // Start is called before the first frame update
    void Start()
    {
        _mR.material.color = Color.black;



    }
    private void OnMouseDown()
    {      
        //释放其他物品 
        gameObject.Recycle(gameObject.name);
    }
    private void OnDisable()
    {
        if (MapBuilder.Exists)
        {
            AStarGrid aStarGrid = MapBuilder.GetSingleton().GetAStarGrid(AStarGirdPosition.ToAStarGirdPosition(Position));
            aStarGrid.State = AStartGirdState.Through;
        }
    }
}
