using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoginPanel
{
    public class StartGame : MonoBehaviour
    {
        private delegate void FAction();
        //登录
        public GameObject dengluPanel;
        public InputField zhanghao;
        public InputField password;

        public Button denglu_btn;
        public Button zhuce_btn;

        //注册
        public GameObject zhucePanel;
        public InputField yonghuming;
        public InputField mima1;
        public InputField mima2;

        public Button denglu;
        public Button zhuce;


        private Vector3 InitPosition = Vector3.zero;
        private Vector3 nextPosition = Vector3.zero;

        public JsonData jsonData;

        //错误提示

        Text loginError;
        Text registerError;
       public Text log;
        private void Start()////////////////////////////////////////////////////
        {
            //jsonData = new JsonData();
            //loginError = UnityHelper.GetTheChildNodeComponetScripts<Text>(dengluPanel, "loginError");
            //loginError.gameObject.SetActive(false);
            //registerError = UnityHelper.GetTheChildNodeComponetScripts<Text>(zhucePanel, "registerError");
            //registerError.gameObject.SetActive(false);
            ////log= UnityHelper.GetTheChildNodeComponetScripts<Text>(dengluPanel.GetComponentInParent<Transform>().gameObject, "Log");
            //zhanghao.onValueChanged.AddListener((p) => { loginError.gameObject.SetActive(false); });
            //password.onValueChanged.AddListener(p => { loginError.gameObject.SetActive(false); });
            //yonghuming.onValueChanged.AddListener(p => { registerError.gameObject.SetActive(false); });
            //mima1.onValueChanged.AddListener(p => { registerError.gameObject.SetActive(false); });
            //mima2.onValueChanged.AddListener(p => { registerError.gameObject.SetActive(false); });

            #region 按钮点击事件
            InitPosition = dengluPanel.transform.position;
            nextPosition = zhucePanel.transform.position;

            denglu_btn.onClick.AddListener(() =>
            {
                //跳转场景
                if (SerchData(zhanghao.text, int.Parse(password.text)))
                    SceneManager.LoadSceneAsync("SelectCharacter");
                else
                {
                    loginError.gameObject.SetActive(true);
                    if (loginError_text == "")
                        loginError.text = "登录失败";
                    else
                        loginError.text = loginError_text;
                }
            });
            zhuce_btn.onClick.AddListener(() =>
            {
                //注册界面
                zhucePanel.transform.position = Vector3.Lerp(nextPosition, InitPosition, 1f);
                dengluPanel.transform.position = Vector3.Lerp(InitPosition, nextPosition, 1f);
            });

            denglu.onClick.AddListener(() =>
            {
                //登录界面
                dengluPanel.transform.position = Vector3.Lerp(nextPosition, InitPosition, 1f);
                zhucePanel.transform.position = Vector3.Lerp(InitPosition, nextPosition, 1f);

            });

            zhuce.onClick.AddListener(() =>
            {
                //SaveMessege(InitJsonData(),"Lin");
                //ReadMessage("Lin");
                //注册
                //如果本地没有对应的json 文件，重新创建
                string path = JsonPath();
                log.text = path;
                if (!File.Exists(path))
                {
                    File.Create(path);
                    Debug.LogError("创建文件");
                }
                if ((yonghuming.text != "" && mima1.text != "" && mima2.text != "")
                    && (SerchData(yonghuming.text, int.Parse(mima1.text), int.Parse(mima2.text))))
                {
                    if (SaveJson(InitJsonData()))
                    {
                        dengluPanel.transform.position = Vector3.Lerp(nextPosition, InitPosition, 1f);
                        zhucePanel.transform.position = Vector3.Lerp(InitPosition, nextPosition, 1f);
                        Debug.LogError("注册成功");
                    }
                }
                else
                {
                    registerError.gameObject.SetActive(true);
                    if (_registered == "")
                        registerError.text = "注册失败";
                    else
                        registerError.text = _registered;
                }
            });

            #endregion
        }
        //保存json文件路径
        string JsonPath()
        {
            //return Application.streamingAssetsPath + "/JsonTest.json";
            //return  Application.persistentDataPath + "/JsonTest.json";
            return OperationFile.FilePath() + "JsonTest.json";
        }
        ////初始化json数据
        PlayerMessage InitJsonData()
        {
            PlayerMessage playerMessage = new PlayerMessage();
            var a = ReadJson();
            int count = 0;
            if (a == null)
                count = 0;
            else
                count = a.Count;
            playerMessage.PlayerId = count + 1;
            playerMessage.PlayerName = yonghuming.text;
            #region test
            //playerMessage.tes[1] = Tes.a;
            //playerMessage.tes[2] = Tes.b;
            //playerMessage.ass = new string[3];
            //playerMessage.ass[0] = "啥";
            //playerMessage.ass[1] = "山";
            //playerMessage.ass[2] = "套";
            //string name = "tao";
            //playerMessage.RespondCards.Add("杀");
            //playerMessage.RespondCards.Add("闪");
            #endregion
            playerMessage.PassWord = int.Parse(mima1.text);

            return playerMessage;
        }


        JsonData InitJsonData1()
        {
            JsonData jsonData = new JsonData();
            jsonData.lsPlayerMessage = ReadMessage("Lin");
            PlayerMessage playerMessage = new PlayerMessage();
            //var a = ReadJson();
            int count = 0;
            if (jsonData.lsPlayerMessage == null)
            {
                jsonData.lsPlayerMessage = new List<PlayerMessage>();
                count = 0;
            }
            else
                count = jsonData.lsPlayerMessage.Count;
            playerMessage.PlayerId = count + 1;
            playerMessage.PlayerName = yonghuming.text;

           
            #region test
            //playerMessage.tes[1] = Tes.a;
            //playerMessage.tes[2] = Tes.b;
            //playerMessage.ass = new string[3];
            //playerMessage.ass[0] = "啥";
            //playerMessage.ass[1] = "山";
            //playerMessage.ass[2] = "套";
            //string name = "tao";
            //playerMessage.RespondCards.Add("杀");
            //playerMessage.RespondCards.Add("闪");
            #endregion
            playerMessage.PassWord = int.Parse(mima1.text);
            jsonData.lsPlayerMessage.Add(playerMessage);

            return jsonData;
        }

        //  UnityEngine.Networking.UnityWebRequest();

        //登录验证
        string loginError_text = "";
        bool SerchData(string name, int password)
        {
            var temp = ReadJson();
            if (temp == null)
            {
                loginError_text = "SerchData.....ReadJson()...=" + temp;
                Debug.Log("SerchData.....ReadJson()...=" + temp);
                return false;
            }
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].PlayerName == name && temp[i].PassWord == password)
                {
                    return true;
                }
                else
                {
                    //Debug.Log("false");
                    if (temp[i].PassWord != password)
                        loginError_text = "密码错误，请重新输入！！";
                    if (temp[i].PlayerName != name)
                        loginError_text = "用户名不存在";
                }
            }
            return false;
        }
        //注册验证
        string _registered = "";
        bool SerchData(string name, int password1 = -1, int password2 = -1)
        {
            if (name == "" || password1 == -1 || password2 == -2)
                return false;
            var temp = ReadJson();
            if (temp == null && password1==password2)
            {
                Debug.Log("SerchData.....ReadJson()...=" + temp);
                return true;
            }
            foreach (var Read in temp)
            {
                if (Read.PlayerName == name)
                {
                    _registered = "用户名已存在";
                    //用户名已存在
                    return false;
                }
            }
            if (password1 != password2)
            {
                _registered = "密码不一致";
                return false;
            }
            return true;
        }

        //保存json数据到本地
        bool SaveJson(PlayerMessage playerMessage)
        {
            //如果本地没有对应的json 文件，重新创建
            if (!File.Exists(JsonPath()))
            {
                File.Create(JsonPath());
            }

            JsonData js = new JsonData();
            List<PlayerMessage> playerMessagesList = ReadJson();
            if (playerMessagesList == null)
                js.lsPlayerMessage = new List<PlayerMessage>();
            else
                js.lsPlayerMessage = playerMessagesList;

            js.lsPlayerMessage.Add(playerMessage);
            string json = JsonUtility.ToJson(js, true);
            //File.WriteAllText(JsonPath(), json);
            OperationFile.WriteFileByLine("JsonTest", json);
            Debug.Log("保存成功");
            return true;
        }
        //从本地读取json数据
        List<PlayerMessage> ReadJson()
        {
            if (!File.Exists(JsonPath()))
            {
                Debug.LogError("读取的文件不存在！创建文件");
                
                return null;
            }
            //string json = File.ReadAllText(JsonPath());
            string json = OperationFile.ReadFileByFileName("JsonTest");
            JsonData jsonTemp = new JsonData();
            jsonTemp = JsonUtility.FromJson<JsonData>(json);
            //Debug.LogError(jsonTemp);
            if (jsonTemp == null)
                return null;
            return jsonTemp.lsPlayerMessage;
        }



        List<PlayerMessage> ReadMessage(string tableName)
        {
            string a = PlayerPrefs.GetString(tableName);
            if (a=="")
            {
                return null;
            }
            print(a);
            JsonData m_R = new JsonData();
            m_R.lsPlayerMessage = new List<PlayerMessage>();
            m_R = JsonUtility.FromJson<JsonData>(a);
            print(m_R);
            foreach (var item in m_R.lsPlayerMessage)
            {
                Debug.LogError(item.PlayerName);
            }
            //JsonUnity(a)// 解码 JSON.decode(data)-- 解码
            return m_R.lsPlayerMessage;
        }

        void SaveMessege(JsonData root1, string tableName)
        {
            JsonData rootte = root1;
            string root = JsonUtility.ToJson(rootte);
            PlayerPrefs.SetString(tableName, root);
        }
    }



    public class JsonData
    {
        public List<PlayerMessage> lsPlayerMessage;
    }

    //玩家信息
    [Serializable]
    public class PlayerMessage
    {
        public int PlayerId;
        public string PlayerName;
        public int PassWord;
        //public Tes[] tes;

        //public string[] ass;
        //public List<string> RespondCards=new List<string>();
        ///public List<EquipMessage> PlayerEquip;
    }

    public enum Tes
    {
        a = 1,
        b = 2
    }


    //装备信息
    [Serializable]
    public class EquipMessage
    {
        public int EquipId;
        public string EquipName;
    }
}

public class AnimatioinT<T> where T : UnityEngine.Object
{
    public IEnumerable DeletTime(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);

    }
    public static void DeletTime(float time)
    {

        Debug.LogError("DeletTime");


    }
}


/* 对Android 端文件的操作
 
 /// 
    <summary>
   /// 安卓端复制文件
  /// </summary>
  /// <param name="fileName">文件路径</param>
 /// <returns></returns>
 IEnumerator CopyFile_Android(string fileName)
{
     WWW w = new WWW(Application.streamingAssetsPath + "/" + fileName);
        yield return w;
      if (w.error == null)
      {
          FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + fileName);
          //判断文件是否存在
          if (!fi.Exists)
          {
              FileStream fs = fi.OpenWrite();
              fs.Write(w.bytes, 0, w.bytes.Length);
              fs.Flush();
              fs.Close();
              fs.Dispose();
               debug.log("CopyTxt Success!" + "\n" + "Path: ======> " +                    Application.persistentDataPath + fileName);
             }
     }
    else
     {
         log("Error : ======> " + w.error);
      }
}
 
 
 
 */