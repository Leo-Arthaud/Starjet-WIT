// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.
// License: Attribution 4.0 International(CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;
using System.Linq;

#pragma warning disable 0162

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Resources")]
	[Tooltip("Loads all assets of a given type stored at path in a Resources folder. The path is relative to any Resources folder inside the Assets folder of your project, extensions must be omitted.")]
	public class ResourcesLoadAll : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The path is relative to any Resources folder inside the Assets folder of your project.\n\nExample: Assets/Resources/Prefabs/YourFolder\nYou enter: Prefabs/YourFolder")]
		public FsmString assetPath;

		[RequiredField]
		[Tooltip("The Array to store the Assets in")]
		[UIHint(UIHint.Variable)]
		public FsmArray storeAssets;

		public FsmEvent successEvent;
		public FsmEvent failureEvent;


		public override void Reset()
		{
			assetPath = null;
			storeAssets = null;
		}


		public override void OnEnter()
		{
			bool ok = false;
			try
			{
				ok = LoadAllResources();
			}catch(UnityException e)
			{
				Debug.LogWarning(e.Message);
			}

			if (ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}

			Finish ();
		}

		public override string ErrorCheck ()
		{
			if (storeAssets.IsNone)
			{
				return "";
			}

			switch (storeAssets.ElementType)
				{
				case VariableType.GameObject:
					break;
				case VariableType.Texture:
					break;
				case VariableType.Material:
					break;
			    case VariableType.String:
					break;
			    case VariableType.Object:
					break;
				default:
				// not supported.
				return "Only GameObject, Texture, Sprites, Material and Unity Objects are supported";
				}

			return "";
		}

		public bool LoadAllResources()
		{
			switch (storeAssets.ElementType)
			{
			case VariableType.GameObject:
				    storeAssets.Values = Resources.LoadAll<GameObject>(assetPath.Value).Cast<GameObject>().ToArray();
                break;

			case VariableType.Texture:
					storeAssets.Values = Resources.LoadAll<Texture>(assetPath.Value).Cast<Texture>().ToArray();
				break;

			case VariableType.Material:
					storeAssets.Values = Resources.LoadAll<Material>(assetPath.Value).Cast<Material>().ToArray();
				break;

			case VariableType.String:
				    storeAssets.Values = Resources.LoadAll<TextAsset>(assetPath.Value).Cast<TextAsset>().Select(_asset => _asset.text).ToArray();
				break;

			case VariableType.Object:
					storeAssets.Values = Resources.LoadAll<Object>(assetPath.Value).Cast<Object>().ToArray();
				break;

			default:
				// not supported.
				return false;
			}
			return true;
		}
	}
}
