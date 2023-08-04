using UnityEditor;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	/// <summary>
	/// RELEASE NOTES:
	///		2.6.0-BETA:
	///			+ NOTE: this OneClick requires OneClickBase v2.6.0.0+.
	///			~ Adjusted default blendshape values to 0 - 1 scale vs Unity
	///				standard 0 - 100. ReadyPlayerMe responded to inquiry
	///				indicating they are sticking with this 0 - 1 weight scale.
	///				Utility to convert back and forth will not be included in
	///				this package.
	///		2.5.1-BETA:
	///			+ Support for atlas-generated-models with single mesh.
	///			+ Utility to adjust blendshape weight configurations
	///				from 0 - 100 to 0 - 1. Current version of RPM GLTF import
	///				uses 0 - 1 instead of Unity standard 0 - 100.
	///		2.5.0-BETA : Initial release.
	/// ==========================================================================
	/// PURPOSE: This script provides simple, simulated lip-sync input to the
	///		Salsa component from text/string values. For the latest information
	///		visit crazyminnowstudio.com.
	/// ==========================================================================
	/// DISCLAIMER: While every attempt has been made to ensure the safe content
	///		and operation of these files, they are provided as-is, without
	///		warranty or guarantee of any kind. By downloading and using these
	///		files you are accepting any and all risks associated and release
	///		Crazy Minnow Studio, LLC of any and all liability.
	/// ==========================================================================
	/// </summary>
	public class OneClickReadyPlayerMeEditor : Editor
	{
		public delegate void SalsaOneClickChoice(GameObject gameObject, AudioClip audioClip);
		public static SalsaOneClickChoice SalsaOneClickSetup = OneClickReadyPlayerMe.Setup;

		public delegate void EyesOneClickChoice(GameObject gameObject);
		public static EyesOneClickChoice EyesOneClickSetup = OneClickReadyPlayerMeEyes.Setup;

		[MenuItem("GameObject/Crazy Minnow Studio/SALSA LipSync/One-Clicks/ReadyPlayerMe")]
		public static void OneClickSetup_RPM()
		{
			SalsaOneClickSetup = OneClickReadyPlayerMe.Setup;
			EyesOneClickSetup = OneClickReadyPlayerMeEyes.Setup;

			OneClickSetup();
		}

		public static void OneClickSetup()
		{
			GameObject go = Selection.activeGameObject;
			if (go == null)
			{
				Debug.LogWarning(
					"NO OBJECT SELECTED: You must select an object in the scene to apply the OneClick to.");
				return;
			}

#if UNITY_2018_3_OR_NEWER
				if (PrefabUtility.IsPartOfAnyPrefab(go))
				{
					if (EditorUtility.DisplayDialog(
													OneClickBase.PREFAB_ALERT_TITLE,
													OneClickBase.PREFAB_ALERT_MSG,
													"YES", "NO"))
					{
						PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
						ApplyOneClick(go);
					}
				}
				else
				{
					ApplyOneClick(go);
				}
#else
			ApplyOneClick(go);
#endif
		}

		private static void ApplyOneClick(GameObject go)
		{
			SalsaOneClickSetup(go, AssetDatabase.LoadAssetAtPath<AudioClip>(OneClickBase.RESOURCE_CLIP));
			EyesOneClickSetup(Selection.activeGameObject);
		}
	}
}