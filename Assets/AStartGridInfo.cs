using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.Tool
{
    public class AStartGridInfo : MonoBehaviour
    {
        [SerializeField]
        public AStarGrid ASG;
        public MeshRenderer MR;
        private void Awake()
        {
            MR=GetComponent<MeshRenderer>();             
        }
    }
}
