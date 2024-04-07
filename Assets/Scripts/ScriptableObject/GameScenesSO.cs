using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName ="Game Scene/GameScenesSO")]
public class GameScenesSO : ScriptableObject
{
    public AssetReference sceneReference;
    public SceneType sceneType;

}