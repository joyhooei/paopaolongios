using UnityEngine;
using UnityEditor ;
using UnityEditor.Callbacks;

using System.IO;
using System.Collections;

public class MyAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
	[MenuItem("Reporter/Create")]
	public static void CreateReporter()
    {
        GameObject reporterObj;
        Reporter reporter;
        if (!GameObject.Find("Reporter") || GameObject.Find("Reporter").GetComponent<Reporter>() == null)
        {
            reporterObj = new GameObject();
            reporterObj.name = "Reporter";
            reporter = reporterObj.AddComponent<Reporter>();
            reporterObj.AddComponent<ReporterMessageReceiver>();
        }
        else
        {
            reporterObj = GameObject.Find("Reporter");
            reporter = reporterObj.GetComponent<Reporter>();
        }
		//reporterObj.AddComponent<TestReporter>();
        string path = "Assets/Reporter/Images/";

		reporter.images = new Images();
        reporter.images.clearImage          = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "clear.png", typeof(Texture2D));
        reporter.images.collapseImage       = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "collapse.png", typeof(Texture2D));
        reporter.images.clearOnNewSceneImage= (Texture2D)AssetDatabase.LoadAssetAtPath(path + "clearOnSceneLoaded.png", typeof(Texture2D));
        reporter.images.showTimeImage       = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "timer_1.png", typeof(Texture2D));
        reporter.images.showSceneImage      = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "UnityIcon.png", typeof(Texture2D));
        reporter.images.userImage           = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "user.png", typeof(Texture2D));
        reporter.images.showMemoryImage     = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "memory.png", typeof(Texture2D));
        reporter.images.softwareImage       = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "software.png", typeof(Texture2D));
        reporter.images.dateImage           = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "date.png", typeof(Texture2D));
        reporter.images.showFpsImage        = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "fps.png", typeof(Texture2D));
        reporter.images.showGraphImage      = (Texture2D)AssetDatabase.LoadAssetAtPath(path + ".png", typeof(Texture2D));
        reporter.images.graphImage          = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "chart.png", typeof(Texture2D));
        reporter.images.infoImage           = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "info.png", typeof(Texture2D));
        reporter.images.searchImage         = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "search.png", typeof(Texture2D));
        reporter.images.closeImage          = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "close.png", typeof(Texture2D));
        reporter.images.buildFromImage      = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "buildFrom.png", typeof(Texture2D));
        reporter.images.systemInfoImage     = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "ComputerIcon.png", typeof(Texture2D));
        reporter.images.graphicsInfoImage   = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "graphicCard.png", typeof(Texture2D));
        reporter.images.backImage           = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "back.png", typeof(Texture2D));
        reporter.images.cameraImage         = (Texture2D)AssetDatabase.LoadAssetAtPath(path + ".png", typeof(Texture2D));
        reporter.images.logImage            = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "log_icon.png", typeof(Texture2D));
        reporter.images.warningImage        = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "warning_icon.png", typeof(Texture2D));
        reporter.images.errorImage          = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "error_icon.png", typeof(Texture2D));
        reporter.images.barImage            = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "bar.png", typeof(Texture2D));
        reporter.images.button_activeImage  = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "button_active.png", typeof(Texture2D));
        reporter.images.even_logImage       = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "even_log.png", typeof(Texture2D));
        reporter.images.odd_logImage        = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "odd_log.png", typeof(Texture2D));
        reporter.images.selectedImage       = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "selected.png", typeof(Texture2D));
        reporter.images.WholeBGImage        = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "WholeBGImage.png", typeof(Texture2D));
        reporter.images.GreyBGImage         = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "GrayBG.png", typeof(Texture2D));

        reporter.images.reporterScrollerSkin = (GUISkin)AssetDatabase.LoadAssetAtPath(path + "reporterScrollerSkin.guiskin", typeof(GUISkin));

	}
	[InitializeOnLoad]
	public class BuildInfo
	{
		static BuildInfo ()
	    {
	        EditorApplication.update += Update;
	    }
	 
		static bool isCompiling = true ; 
	    static void Update ()
	    {
			if( !EditorApplication.isCompiling && isCompiling )
			{
	        	//Debug.Log("Finish Compile");
				if( !Directory.Exists( Application.dataPath + "/StreamingAssets"))
				{
					Directory.CreateDirectory( Application.dataPath + "/StreamingAssets");
				}
				string info_path = Application.dataPath + "/StreamingAssets/build_info.txt" ;
				StreamWriter build_info = new StreamWriter( info_path );
				build_info.Write(  "Build from " + SystemInfo.deviceName + " at " + System.DateTime.Now.ToString() );
				build_info.Close();
			}
			
			isCompiling = EditorApplication.isCompiling ;
	    }
	}
}
