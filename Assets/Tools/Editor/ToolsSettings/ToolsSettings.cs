using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class ToolsSettings : GlobalConfig<ToolsSettings>
{
    //������Դ�Զ����ദ�� fbx animationClip Avatar �Զ����࣬������Դ�Զ�����������

    [LabelWidth(30)]
    [BoxGroup("AnimClip")]
    [HorizontalGroup("AnimClip/AnimationClip")]
    [FolderPath]
    public string Anim;

    [LabelWidth(30)]
    [BoxGroup("AnimClip")]
    [HorizontalGroup("AnimClip/AnimationClip", width: 80)]
    public string Mark;

    [LabelWidth(75)]
    [FolderPath]
    [BoxGroup("AnimationClip")]
    [LabelText("AvatarPath")]
    public string Avatarfolder;

    [LabelWidth(50)]
    [FolderPath]
    [BoxGroup("Fbx")]
    [LabelText("FBXPath")]
    public string FBXfolder;
}