using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[TypeInfoBox("<size=20>һ�����</size>")]
public class OneKeyBuildlEditor : GlobalConfig<OneKeyBuildlEditor>
{
    private const int addressableBundlesDirOrder = 3;
    private const int AddressableButtonOrder = 2;
    private const int Addressables��ַ����Order = 3;
    private const int PlayerSetting�������Order = 4;
    private const int PlayerSetting������Order = 5;
    private const string HFS = "HFS";
    private string androidPath;
    private string winPath;
#pragma warning disable CS0414
    private bool isBuild = false;
#pragma warning restore CS0414
    //#region Addressables Button

    //[TitleGroup("Addressables")]
    //[ButtonGroup("Addressables/buttons", AddressableButtonOrder)]
    //public void Groups()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Groups");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Groups");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Settings()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Settings");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Settings");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Profiles()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Profiles");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Profiles");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void EventViewer()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Event Viewer");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Event Viewer");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Anallyze()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Analyze");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Analyze");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Hosting()
    //{
    //    Debug.Log("�򿪴��ڣ�" + "Window/Asset Management/Addressables/Hosting");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Hosting");
    //}

    //[FolderPath]
    //[TitleGroup("Addressables", order: addressableBundlesDirOrder)]
    //public List<string> addressableBundlesDir;

    //[TitleGroup("Addressables")]
    //public List<AddressableAssetGroupSchema> addressableAssetGroupSchema;

    //#endregion Addressables Button

    //#region AddressableAssetSettings

    //[TitleGroup("Addressables ��ַ���� (����ĵ�ַ����)")]
    //public AddressableAssetSettings addressableConfig;

    //[TitleGroup("Addressables ��ַ���� (����ĵ�ַ����)"), ShowInInspector]
    //[InfoBox("���������")]
    //[ReadOnly]
    //public string activeProfile
    //{
    //    get
    //    {
    //        if (addressableConfig == null)
    //        {
    //            return "";
    //        }
    //        return addressableConfig.profileSettings.GetProfileName(addressableConfig.activeProfileId);
    //    }
    //}

    //[TitleGroup("Addressables ��ַ���� (����ĵ�ַ����)", null, TitleAlignments.Left, true, true, false, Addressables��ַ����Order), ShowInInspector]
    //public string hFS
    //{
    //    get
    //    {
    //        if (addressableConfig == null)
    //        {
    //            return "";
    //        }
    //        // ��ǰ����� ������

    //        return addressableConfig.profileSettings.GetValueByName(addressableConfig.activeProfileId, HFS);
    //    }
    //    set
    //    {
    //        addressableConfig.profileSettings.SetValue(addressableConfig.activeProfileId, HFS, value);
    //    }
    //}

    //#endregion AddressableAssetSettings

    [TitleGroup("PlayerSetting �������", null, TitleAlignments.Left, true, true, false, PlayerSetting�������Order)]
    public BuildConfig buildConfig;

    private List<string> m_activeScene = new List<string>();

    [ShowInInspector]
    [TitleGroup("PlayerSetting �������", null, TitleAlignments.Left, true, true, false, PlayerSetting�������Order)]
    public List<string> activeScenes
    {
        get
        {
            var tempScenes = new List<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (var item in scenes)
            {
                if (item.enabled)
                {
                    tempScenes.Add(item.path);
                }
            }
            return tempScenes;
        }
    }

    private bool isBuilding;

    [TitleGroup("PlayerSetting �������")]
    public AndroidSdkVersions androidSDK_Version
    {
        get
        {
            return buildConfig.androidSDK_Version;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.androidSDK_Version = value;
            }
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    [FolderPath(AbsolutePath = true)]
    public string buildPath
    {
        get
        {
            if (buildConfig == null)
            {
                return "";
            }
            return buildConfig.buildPath;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.buildPath = value;
            }
            EditorPrefs.SetString("OdinBuild.BuildPath", value);
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    [ReadOnly]
    public BuildTargetGroup buildTarget
    {
        get
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return BuildTargetGroup.Android;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return BuildTargetGroup.iOS;
            }
            else
            {
                return BuildTargetGroup.Standalone;
            }
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    public string companyName
    {
        get
        {
            return PlayerSettings.companyName;
        }
        set
        {
            PlayerSettings.companyName = value;
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    public string productName
    {
        get
        {
            return PlayerSettings.productName;
        }
        set
        {
            PlayerSettings.productName = value;
        }
    }

    [ReadOnly]
    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    [InfoBox("��Ҫ����İ汾�ţ������޸ģ�Ĭ��������")]
    public string version
    {
        get
        {
            return PlayerSettings.bundleVersion;
        }
        set
        {
            PlayerSettings.bundleVersion = value;
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    public int versionIncrease
    {
        get
        {
            if (buildConfig == null)
            {
                return 0;
            }
            return buildConfig.versionIncrease;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.versionIncrease = value;
            }
        }
    }

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    public ScriptingImplementation scriptingBacked
    {
        get
        {
            ScriptingImplementation ScriptingBackend = PlayerSettings.GetScriptingBackend(buildTarget);
            return ScriptingBackend;
        }
        set
        {
            PlayerSettings.SetScriptingBackend(buildTarget, value);
        }
    }

    /// <summary>
    /// ���
    /// </summary>
    [TitleGroup("PlayerSetting �������")]
    [Button(90), GUIColor(0.4f, 0.8f, 1f)]
    public void BuildPackage()
    {
        //this.isBuild = !this.isBuild;
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;

        PlayerSettings.Android.targetSdkVersion = androidSDK_Version;

        #region ��ʼ���

        isBuilding = BuildPipeline.isBuildingPlayer;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = activeScenes.ToArray();
        //���Ŀ��·��
        androidPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".apk";
        winPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".exe";
        if (buildTarget == BuildTargetGroup.Android)
        {
            buildPlayerOptions.locationPathName = androidPath;
        }
        else
        {
            buildPlayerOptions.locationPathName = winPath;
        }
        //���Ŀ��ƽ̨
        if (buildTarget.ToString() == "Standalone")
        {
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        }
        else
        {
            buildPlayerOptions.target = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTarget.ToString());
        }

        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.platform);
            Debug.Log("Build succeeded: " + summary.totalSize + "bytes");
            Debug.Log("Build succeeded: " + summary.totalTime + "s");

            SucessBuild(report);
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
            FailBuild(report);
        }
        isBuilding = BuildPipeline.isBuildingPlayer;

        #endregion ��ʼ���
    }

    //[HideIf("isBuild")]
    //[TitleGroup("PlayerSetting �������")]
    //[DisableIf("isBuilding")]
    //[Button(90), GUIColor(0f, 1f, 0f)]
    //public void AddressablesBundle()
    //{
    //    #region ���Group�ļ�

    //    bool m_creatGroup = true;

    //    foreach (var dirPath in addressableBundlesDir)
    //    {
    //        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);//��ȡĿ¼���ļ�����Ϣ

    //        //���Group����û���ļ����������飬�оͽ��ļ�����ӵ�Groups��û�оʹ���

    //        var groups = addressableConfig.groups;
    //        AddressableAssetGroup accessionGroup = null;
    //        foreach (var group in groups)
    //        {
    //            Debug.Log("Group����  " + group.name);
    //            if (group.name == directoryInfo.Name)
    //            {
    //                Debug.Log("�����ļ���ͬ����Group");
    //                accessionGroup = group;
    //                m_creatGroup = false;
    //                group.Settings.DefaultGroup = group;
    //                break;
    //            }
    //        }
    //        if (m_creatGroup)
    //        {
    //            accessionGroup = addressableConfig.CreateGroup(directoryInfo.Name, true, false, true, addressableAssetGroupSchema /*typeof(BundledAssetGroupSchema),*//*typeof(ContentUpdateGroupSchema)*/);//�������������������еģ�Assets�еģ�������ȫ�µģ�����Ĭ�ϣ�
    //        }

    //        foreach (var objGuid in directoryInfo.GetFiles()) //���ļ����µ������ļ���ӵ�Groups�У�
    //        {
    //            if (objGuid.Name.Contains("meta"))//meta�ļ������ʲ�
    //            {
    //                continue;
    //            }
    //            string[] m_path = objGuid.FullName.Split(new string[] { "Assets" }, StringSplitOptions.RemoveEmptyEntries);
    //            string m_guidPath = "Assets" + m_path[1];

    //            string guid = AssetDatabase.GUIDFromAssetPath(m_guidPath).ToString();
    //            var asset = accessionGroup.Settings.CreateAssetReference(guid);//���뵽��

    //            #endregion ���Group�ļ�

    //            #region ��ӱ�ǩ
    //            addressableConfig.AddLabel(directoryInfo.Name);
    //            AddressableAssetEntry resourcesEntry = addressableConfig.FindAssetEntry(asset.AssetGUID);//�޸�/��ӣ���ǩ

    //            resourcesEntry.SetLabel(directoryInfo.Name, true);
    //        }
    //    }

    //    #endregion ��ӱ�ǩ

    //    //����Group
    //    this.isBuild = !this.isBuild;
    //}

    /// <summary>
    /// ���ʧ��
    /// </summary>
    /// <param name="report"></param>
    private void FailBuild(BuildReport report)
    {
        buildResult = "���ʧ�ܣ��뿴Console��Ϣ";
    }

    /// <summary>
    /// ����ɹ�
    /// </summary>
    /// <param name="report"></param>
    private void SucessBuild(BuildReport report)
    {
        BuildSummary summary = report.summary;
        string size = "�������Сʧ��";
        if (File.Exists(androidPath))
        {
            FileInfo apk = new FileInfo(androidPath);
            size = " " + (apk.Length / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }
        else
        {
            long m_size = 0;
            GetDirSizeByPath((buildPath + @"\" + buildTarget.ToString()).Replace(@"/", @"\"), ref m_size);
            size = " " + (m_size / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }

        string time = " " + summary.totalTime + "s";
        buildResult = "����ɹ�: " + summary.outputPath + "\n" +
            "��װ���С: " + size + "\n" +
            "���ʱ��: " + time + "\n";

        string[] versionsNum = PlayerSettings.bundleVersion.Split('.');
        int tempInt = int.Parse(versionsNum[2]) + versionIncrease;
        versionsNum[2] = tempInt.ToString();
        var tempVersionsNum = String.Join(".", versionsNum);

        PlayerSettings.bundleVersion = tempVersionsNum;
        EditorUtility.OpenWithDefaultApp(buildPath.Replace(@"/", @"\"));
    }

    [TitleGroup("PlayerSetting ������", null, TitleAlignments.Left, true, true, false, PlayerSetting������Order)]
    [ReadOnly]
    [MultiLineProperty(3), ShowInInspector]
    public string buildResult
    {
        get
        {
            return EditorPrefs.GetString("OdinBuild.buildResult");
        }
        set
        {
            EditorPrefs.SetString("OdinBuild.buildResult", value);
        }
    }

    /// <summary>
    /// ��ȡ�ļ��еĴ�С
    /// </summary>
    /// <param name="dir">�ļ���Ŀ¼</param>
    /// <param name="dirSize">�����ļ��д�С����������</param>
    private static void GetDirSizeByPath(string dir, ref long dirSize)
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            FileInfo[] files = dirInfo.GetFiles();

            foreach (var item in dirs)
            {
                GetDirSizeByPath(item.FullName, ref dirSize);
            }

            foreach (var item in files)
            {
                dirSize += item.Length;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("��ȡ�ļ���Сʧ��" + ex.Message);
        }
    }
}