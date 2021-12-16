using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.Tool
{
    public class AStarConfig 
    {
        /// <summary>
        /// A星原点的位置(世界坐标)
        /// </summary>
        public static Vector2 AStarOriginPosition;
        /// <summary>
        /// X精度
        /// </summary>
        private static float m_PrecisionX = 1;
        /// <summary>
        /// Y精度
        /// </summary>
        private static float m_PrecisionY = 1;
        /// <summary>
        /// X精度
        /// </summary>
        public static float PrecisionX
        {
            get { return m_PrecisionX; }    
            set { m_PrecisionX = Mathf.Clamp(value, 0.01f,value);}
        }
        /// <summary>
        /// Y精度
        /// </summary>
        public static float PrecisionY
        {
            get { return m_PrecisionY; }        
            set { m_PrecisionY = Mathf.Clamp(value, 0.01f, value); }
        }
    }
}
