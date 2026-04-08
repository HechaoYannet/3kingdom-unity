using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// json操作
/// </summary>
public class OperationFile
{
    /// <summary>
    /// 平台（PC/移动端）路径
    /// </summary>
    /// <returns></returns>
    public static string FilePath()
    {
        string strReturnPlayformPath = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
                strReturnPlayformPath = Application.streamingAssetsPath+"/";
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                strReturnPlayformPath = Application.persistentDataPath+"/";
                break;
        }
        return strReturnPlayformPath;
    }
    static public  string  ReadFileByFileName(string fileName)
    {
        string path = FilePath() + fileName+".json";
        //Debug.LogError(path);
        UnityWebRequest request = UnityWebRequest.Get(path);
        request.SendWebRequest();
        while(true)
        {
            if (request.downloadHandler.isDone)
                return request.downloadHandler.text;
        }
    }

   static public void WriteFileByLine(string  fileName , string str_info)
    {
        string path = FilePath() + fileName + ".json";
        StreamWriter streamWriter = new StreamWriter(path);
        //将转化后的字符串写入目标文件
        streamWriter.Write(str_info);
        streamWriter.Flush();
        //关闭StreamWriter
        streamWriter.Close();
        streamWriter.Dispose();
    }

}
