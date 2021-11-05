using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using UnityEngine.Networking;
using UnityEngine.UI;
using Farme.Net;
public class Test : MonoBehaviour
{
    private Animator m_Anim;
    private Coroutine m_C;
    public Image button;
    // Start is called before the first frame update
    void Start()
    {
       
        //StartCoroutine(IEDownLoadTexture());


        string textpath = "F:\\YHM\\项目\\识图用图模拟训练项目\\协作记录\\UI协作\\负责人(蔡云)\\2021.08.13\\压缩包\\24.识图用图模拟训练虚拟仿真\\切图\\button.png";
        string abPath = "S:\\Unity Pro 2019.3.7f1\\Project\\工程版本\\BrickAndFire1.1\\BrickAndFire\\Assets\\StreamingAssets\\sprite";
         WebDownloadTool.WebDownloadAssetBundle(abPath, (ab) =>
         {
             Sprite sp = ab.LoadAsset<Sprite>("组 4");
             button.sprite = sp;
             // button.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one / 2.0f);
         });
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
