using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.Tool
{
    public class AStarTest : MonoBehaviour
    {
        public int h;
        public int v;
        public Material Blue;
        public Material Black;
        public Material White;
        public Material Green;
        public Material Red;
        public Material Yellow;
        float offsetX = -5;
        float offsetY = -5;
        Dictionary<string,MeshRenderer> m_MRDic=new Dictionary<string,MeshRenderer>();
        // Start is called before the first frame update
        void Start()
        {
            offsetX = -h / 2f;
            offsetY = -v / 2f;

            AStarConfig.AStarOriginPosition=new Vector2(offsetX, offsetY);
            AStar.AStarGrids = new AStarGrid[h, v];     
            for (int i=0; i < h; i++)
            {
                for(int j=0; j < v; j++)
                {
                    if(GoLoad.Take("Cube",out GameObject go))
                    {
                        AStar.AStarGrids[i, j] = new AStarGrid() { Position = new AStarGirdPosition() { x = i, y = j } };
                        go.name = i+","+j;
                        go.transform.position = new Vector3(offsetX+i * 1, offsetY+ j * 1, 0);
                        m_MRDic.Add(go.name, go.GetComponent<MeshRenderer>());
                    }
                }
            }
        }
        MeshRenderer mr;
        bool m_Down = false;
        AStarGrid m_Start;
        AStarGrid m_End;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                {
                    mr = hit.collider.GetComponent<MeshRenderer>();
                    mr.material = Black;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                mr.material = Blue;
                string[] indexs = mr.name.Split(',');
                int x = int.Parse(indexs[0]);
                int y = int.Parse(indexs[1]);
                m_Start = new AStarGrid() { Position = new AStarGirdPosition() { x = x, y = y } };
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                mr.material = Yellow;
                string[] indexs = mr.name.Split(',');
                int x = int.Parse(indexs[0]);
                int y = int.Parse(indexs[1]);
                m_End = new AStarGrid() { Position = new AStarGirdPosition() { x = x, y = y } };
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                List<AStarGirdPosition> paths=  AStar.FindPath(m_Start, m_End);
                if (paths != null)
                {
                    foreach (var pos in paths)
                    {
                        if (m_MRDic.TryGetValue(pos.x + "," + pos.y, out var MR))
                        {
                            MR.material = Green;
                        }
                    }
                }
              
                
                

            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                foreach (var mr in m_MRDic.Values)
                {
                    mr.material = White;                
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debuger.Log(mr.name + "设置为障碍物");
                mr.material = Red;
                string[] indexs= mr.name.Split(',');
                int x = int.Parse(indexs[0]);
                int y = int.Parse(indexs[1]);
                AStar.AStarGrids[x, y].State = AStartGirdState.NotThrough;
            }
        }
    }
}
