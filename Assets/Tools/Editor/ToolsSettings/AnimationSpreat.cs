using UnityEditor;
using UnityEngine;

public class AnimationSpreat : AssetPostprocessor
{
    private void OnPreprocessModel()
    {
        if (assetPath.Contains(ToolsSettings.Instance.Mark))
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            //动画禁止材质导入
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            //设置为Humannoid
            modelImporter.animationType = ModelImporterAnimationType.Human;
            //创建Avata
            modelImporter.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        }
    }

    private void OnPostprocessModel(GameObject g)
    {
        string cilpName = g.name + ".anim";
        Debug.Log("动画分离2" + assetPath + "   " + cilpName);
        EditorApplication.delayCall += () =>
        {
            if (assetPath.Contains(ToolsSettings.Instance.Mark))
            {
                //copy 动画
                var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);

                foreach (var obj in assets)
                {
                    Debug.Log(obj.name);
                }

                AnimationClip clip = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip)) as AnimationClip;

                var newClip = UnityEngine.Object.Instantiate(clip);
                AssetDatabase.CreateAsset(newClip, "Assets/Resources/动画" + "/" + cilpName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log("导入模型，创建控制器" + assetPath);
            }
        };
    }
}