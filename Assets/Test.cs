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
public class Test : MonoBehaviour
{
    private Animator m_Anim;
    private Coroutine m_C;
    public Image button;
    Coroutine co = null;
    // Start is called before the first frame update
    void Start()
    {
        //co= MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(2, () =>
        //{
        //    MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(co);
        //    Debug.Log(co);
        //});
        //Debug.Log(co);
        //MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(co);
        AssetBundleLoad.MainABName = "StandaloneWindows";
        AssetBundleLoad.PackageCatalogueFile_URL = "S:\\Unity Pro 2019.3.7f1\\MyGitProject\\Farme\\AssetBundles\\StandaloneWindows\\";
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
    Audio audio1;
    Audio audio2;
    //private void 
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-10, 0, 0),Time.fixedDeltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssetBundleLoad.LoadAssetAsync<AudioClip>("audio", "Effect", (clip) =>
            {
                audio1 = AudioManager.ApplyForAudio();
                audio1.Clip = clip;
                audio1.Play();
              //Audio  audio = gameObject.AddComponent<Audio>();
              //  audio.Clip = clip;
              //  audio.Play(1,2);
              //Debug.Log(audio);
            });
            //Debug.Log(AssetBundleLoad.LoadAsset<AudioMixer>("audio", "AuioMixer"));
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            AssetBundleLoad.LoadAssetAsync<AudioClip>("audio", "BackGround", (clip) =>
            {
                audio2 = AudioManager.ApplyForAudio();
                audio2.Clip = clip;
                AudioManager.ExcessPlay(audio1, audio2, 1, 5);
                //Audio  audio = gameObject.AddComponent<Audio>();
                //  audio.Clip = clip;
                //  audio.Play(1,2);
                //Debug.Log(audio);
            });
        }
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

        Sprite sp= ab.LoadAsset<Sprite>("组 4");
        //AssetBundle.LoadFromFile("sprite").
        Debug.Log(sp);
        //DownloadHandlerAssetBundle
        //Texture2D t2D = (uwr.downloadHandler as DownloadHandlerTexture).texture;
        button.sprite = sp;
            //Sprite.Create(t2D, new Rect(0, 0, t2D.width, t2D.height), Vector2.one / 2.0f);
        //Debug.Log(t2D);
    }
}
