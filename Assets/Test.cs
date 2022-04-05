using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using UnityEngine.Networking;
using UnityEngine.UI;
using Farme.Net;
using UnityEngine.Audio;
using System.IO;
using System.Text;
using Farme.Audio;
using Farme.UI;
using System;
using Newtonsoft.Json;
using Farme.Tool;
namespace Farme.Test
{
    public class Test : MonoBehaviour
    {       
        private Animator m_Anim;
        private Coroutine m_C;
        public Image button;
        Coroutine co = null;
        string MP3path = @"C:\Users\XiaoHeTao\Desktop\Music\Wisp X - Stand With Me.mp3";
        public Image Image;
        public string content;
        public Text text;
        public Image img;

        private void Awake()
        {
            MonoSingletonFactory<ShareMono>.GetSingleton();
            //m_Anim = GetComponent<Animator>();

            //m_Anim.SetIK
            // m_Anim.SetLoo
            //GetComponent<FoldFarme>().FE.AddListener((isFold) => { Debuger.Log(isFold); });

        }

        private void OnDestroy()
        {
            Debug.Log(MonoSingletonFactory<ShareMono>.SingletonExist);
            //MonoSingletonFactory<ShareMono>.ClearSingleton();
            //Debug.Log(MonoSingletonFactory<ShareMono>.GetSingleton());
        }

        private void OnAnimatorIK(int layerIndex)
        {

        }

        public void OnValidate()
        {
            
            //text.text = content;
        }
        //private Audio audio;
        //protected override void LateOnEnable()
        //{
        //    ///Debug.Log(2);
        //}
        private float[,] f = new float[2, 3] { { 0, 1, 2 }, { 3, 4, 5 } };//二维数组
        private float[][] F = new float[2][] { new float[2] { 1, 2 }, new float[2] { 1, 2 } };//数组的数组
                                                                                              // Start is called before the first frame update
        protected void Start()
        {







            return;
            SceneLoad.LoadSceneAsync("Test");
            float x;
            float y;
            for (var i = 0; i < 360;)
            {
                /*(1,1) (1,0)  (1,-1)  (0,1)  (0,-1)  (0,0)  (-1,0) (-1,-1) */
                x = (i % 2 == 0) ? (int)Mathf.Sin(i * Mathf.Deg2Rad) : (Mathf.Sin(i * Mathf.Deg2Rad) > 0 ? 1 : -1);
                y = (i % 2 == 0) ? (int)Mathf.Cos(i * Mathf.Deg2Rad) : (Mathf.Cos(i * Mathf.Deg2Rad) > 0 ? 1 : -1);








                i += 45;
            }
            //List<AStarGirdPosition> poss = new List<AStarGirdPosition>();
            //poss.Add(new AStarGirdPosition() { x = 1, y = 1 });
            //poss.Add(new AStarGirdPosition() { x = 3, y = 2 });
            //poss.Add(new AStarGirdPosition() { x = 4, y = 3 });
            //poss.Add(new AStarGirdPosition() { x = 3, y = 4 });
            //List<int> numbers = new List<int>() { 0, 1, 2, 3, 5, 4 };
            //poss.Sort((a, b) => 
            //{
            //    if (a.x >= b.x)
            //    {
            //        return 0;
            //    }
            //    return -1;
            //});
            //foreach (var i in poss)
            //{
            //    Debug.Log(i.x+"______"+i.y);
            //}
            //for (var i = 0; i < 4; i++)
            //{
            //    Debug.Log((Mathf.Sin((i * 90f) * Mathf.Deg2Rad)) + "_________" + (Mathf.Cos((i * 90f) * Mathf.Deg2Rad)));
            //}
            //Debug.Log(F[0][1]);
            //string str = File.ReadAllText(Application.streamingAssetsPath + @"\UnKnowData.json");
            //UnKnowData data = JsonConvert.DeserializeObject<UnKnowData>(str);

            //Debug.Log(data.Info["1004"].exclusive[1]);
            ////UnKnowData unKnowData =new UnKnowData();

            //unKnowData.InfoDic.Add("1", new Info());

            //string json = JsonConvert.SerializeObject(unKnowData);

            //File.WriteAllText(Application.streamingAssetsPath + @"\UnKnowData.json", json,Encoding.UTF8);



            //Debug.Log(data.InfoDic);

            //WebDownloadTool.WebDownloadText(Application.streamingAssetsPath + @"\UnKnowData.json", (json) =>
            // {

            //     Debug.Log(json);
            // });



            //GC.Collect();
            //Debug.Log(1);
            //WebDownloadTool.WebDownLoadAudioClipMP3(MP3path, (clip) =>
            //{
            //    clip.UnloadAudioData();
            //    if (audio == null)
            //    {
            //        audio = AudioManager.ApplyForAudio();
            //        audio.Loop = true;
            //    }
            //    audio.Clip = clip;
            //    audio.Play();
            //});

            //if(GoLoad.Take("FarmeLockFile/WindowRoot",out GameObject go))
            //{
            //    MonoSingletonFactory<WindowRoot>.GetSingleton(go, false);
            //    if(MonoSingletonFactory<WindowRoot>.SingletonExist)
            //    {
            //        WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
            //        root.CreateWindow("MyFirstWindow", RenderMode.ScreenSpaceOverlay,(window)=> 
            //        {
            //            window.CanvasScaler.referenceResolution = new Vector2(1920,1080);
            //            //new Vector2(960, 540);

            //             window.CreatePanel<PanelTest>("PanelTest", "PanelTest", EnumPanelLayer.BOTTOM, (panel) =>
            //             {

            //             });
            //        });

            //    }


            //}

            //co= MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(2, () =>
            //{
            //    MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(co);
            //    Debug.Log(co);
            //});
            //Debug.Log(co);
            ////MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(co);
            //AssetBundleLoad.MainABName = "StandaloneWindows";
            //AssetBundleLoad.PackageCatalogueFile_URL = "S:\\Unity Pro 2019.3.7f1\\MyGitProject\\Farme\\AssetBundles\\StandaloneWindows\\";
            //"S:\\UnityPro2019.3.7f1\\Project\\Farme\\AssetBundles\\StandaloneWindows\\";

            //string filePath = "F:\\Git忽略文件语法及示例.txt";
            //StreamReader SR = new StreamReader(filePath, Encoding.UTF8);
            //string line = "";
            //int count = 0;
            //StreamWriter sw = new StreamWriter("F:\\result.txt");
            //while ((line = SR.ReadLine()) != null)
            //{
            //    count++;
            //    if (count == 3)
            //    {
            //        count = 0;
            //        sw.WriteLine(line);
            //    }
            //    Debug.Log(line);
            //}
            //SR.Close();
            //sw.Close();

            //MonoSingletonFactory<>
            //StartCoroutine(IEDownLoadTexture());

            //"S:\\Unity Pro 2019.3.7f1\\MyGitProject\\Farme\\AssetBundles\\PC\\PC";
            //"/AssetBundles/PC/audio.manifest";
            //WebDownloadTool.WebDownloadAssetBundle(AssetBundleLoad.MainAB_URL, (ab) =>
            // {

            //MonoSingletonFactory<WindowRoot>.GetSingleton().CreateWindow()

            //co=StartCoroutine(AudioPlay());

            //});

            //string textpath = "F:\\YHM\\项目\\识图用图模拟训练项目\\协作记录\\UI协作\\负责人(蔡云)\\2021.08.13\\压缩包\\24.识图用图模拟训练虚拟仿真\\切图\\button.png";
            //string abPath = "S:\\Unity Pro 2019.3.7f1\\Project\\Test\\AssetBundles\\PC\\audio";
            //WebDownloadTool.WebDownloadAssetBundle(abPath, (ab) =>
            //{

            //    //Sprite sp = ab.LoadAsset<Sprite>("组 4");
            //    //button.sprite = sp;
            //     // button.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one / 2.0f);
            // });
        }
        Audio.Audio audio1;
        Audio.Audio audio2;
        //private void 
        private void InputKeyDonw()
        {
            Debug.Log("A键按下");
        }

        private void ClearInputKeyDonw()
        {

            Debug.Log("清除A键按下");
        }
        IEnumerator AudioPlay()
        {
            while (true)
            {
                if (AudioClipManager.GetAudioClip("按下提示音", out AudioClip clip))
                {
                    audio1 = AudioManager.ApplyForAudio();
                    audio1.Clip = clip;
                    audio1.Play();
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        private void Update()
        {
            return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                text.text += "111";
                img.rectTransform.sizeDelta = text.rectTransform.sizeDelta;
                //StopCoroutine(co);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                //StopCoroutine(co);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                //AudioManager.ClearCache();
                //GC.Collect();
            }
        }
        private void FixedUpdate()
        {

            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(-10, 0, 0),Time.fixedDeltaTime);
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    AssetBundleLoad.LoadAssetAsync<AudioClip>("audio", "Effect", (clip) =>
            //    {
            //        audio1 = AudioManager.ApplyForAudio();
            //        audio1.Clip = clip;
            //        audio1.Play();
            //      //Audio  audio = gameObject.AddComponent<Audio>();
            //      //  audio.Clip = clip;
            //      //  audio.Play(1,2);
            //      //Debug.Log(audio);
            //    });
            //    //Debug.Log(AssetBundleLoad.LoadAsset<AudioMixer>("audio", "AuioMixer"));
            //}
            //if(Input.GetKeyDown(KeyCode.Q))
            //{
            //    AssetBundleLoad.LoadAssetAsync<AudioClip>("audio", "BackGround", (clip) =>
            //    {
            //        audio2 = AudioManager.ApplyForAudio();
            //        audio2.Clip = clip;
            //        AudioManager.ExcessPlay(audio1, audio2, 1, 5);
            //        //Audio  audio = gameObject.AddComponent<Audio>();
            //        //  audio.Clip = clip;
            //        //  audio.Play(1,2);
            //        //Debug.Log(audio);
            //    });
            //}
        }
        private string Authorization = "Authorization";
        IEnumerator IEDownLoadTexture()
        {
            UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle("S:\\Unity Pro 2019.3.7f1\\Project\\工程版本\\BrickAndFire1.1\\BrickAndFire\\Assets\\StreamingAssets\\sprite");
            //"F:\\YHM\\项目\\识图用图模拟训练项目\\协作记录\\UI协作\\负责人(蔡云)\\2021.08.13\\压缩包\\24.识图用图模拟训练虚拟仿真\\切图\\button.png"); ;
            //uwr.downloadHandler = new DownloadHandlerTexture();//创建下载程序

            uwr.SendWebRequest();//发送请求
            while (true)
            {
                if (uwr.isDone && uwr.downloadHandler.isDone)
                {
                    break;
                }
                yield return uwr.downloadProgress;
            }

            AssetBundle ab = (uwr.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

            Sprite sp = ab.LoadAsset<Sprite>("组 4");
            //AssetBundle.LoadFromFile("sprite").
            Debug.Log(sp);
            //DownloadHandlerAssetBundle
            //Texture2D t2D = (uwr.downloadHandler as DownloadHandlerTexture).texture;
            button.sprite = sp;
            //Sprite.Create(t2D, new Rect(0, 0, t2D.width, t2D.height), Vector2.one / 2.0f);
            //Debug.Log(t2D);
        }
    }


    [Serializable]
    public class UnKnowData
    {
        public Dictionary<string, Info> Info;
    }
    [Serializable]
    public class Info
    {
        public string id;
        public string name;
        public string description;
        public int grade;
        public Dictionary<string, int> effect;
        public string[] exclusive;
    }
}

