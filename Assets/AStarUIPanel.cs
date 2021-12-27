//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//namespace Farme.Tool
//{
//    public class AStarUIPanel : BaseMono
//    {
//        private Button m_FindPathButton;
//        private Button m_ClearPathButton;
//        private Button m_ClearBarrierButton;

//        private AStarTest m_AStartTest
//        {
//            get
//            {
//                if(MonoSingletonFactory<AStarTest>.SingletonExist)
//                {
//                    return MonoSingletonFactory<AStarTest>.GetSingleton();
//                }
//                return null;
//            }
//        }
//        protected override void Awake()
//        {
//            base.Awake();
//            RegisterComponentsTypes<Button>();

//            m_FindPathButton=GetComponent<Button>("FindPathButton");
//            m_ClearPathButton = GetComponent<Button>("ClearPathInfoButton");
//            m_ClearBarrierButton = GetComponent<Button>("ClearBarrierButton");
//        }

//        protected override void Start()
//        {
//            base.Start();
//            m_FindPathButton.onClick.AddListener(() =>
//            {
//                if(m_AStartTest != null)
//                {
//                    m_AStartTest.FindPath();
//                }
//            });
//            m_ClearPathButton.onClick.AddListener(() =>
//            {
//                if (m_AStartTest != null)
//                {
//                    m_AStartTest.ClearPath();
//                }
//            });
//            m_ClearBarrierButton.onClick.AddListener(() =>
//            {
//                if (m_AStartTest != null)
//                {
//                    m_AStartTest.ClearBarrier();
//                }
//            });
//        }
//        // Update is called once per frame
//        void Update()
//        {

//        }
//    }
//}
