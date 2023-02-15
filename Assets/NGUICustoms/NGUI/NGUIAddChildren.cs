#if PLAYMAKER

using UnityEngine;
using iDecay.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Add multiple children up until a certain amount and optionally ignore already existing instances.")]
	public class NGUIAddChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The parent to add the object to. Strongly recommended to be a child of an NGUI UI Root or an UI Root itself, "
		    + "otherwise creates one in the root of the Hierarchy (wich is probably not a desired).")]
		public FsmOwnerDefault parent;

		[Tooltip("The GameObject to add. Creates an empty GameObject if not set.")]
		public FsmGameObject childReference;

		[Tooltip("How many instances to create.")]
		public FsmInt amount;

		[Tooltip("Optionally give each instance a different name than the one of the provided GameObject/Prefab. Leave as 'None' to ignore.")]
		public FsmString customName;
		
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.GameObject)]
		[Tooltip("Store the created objects in a GameObject array.")]
		public FsmArray storeInstances;
		
		[Tooltip("Whether to ignore already existing instances. Otherwise creates new GameObjects regardless.")]
		public FsmBool ignoreExisting;

		[Tooltip("Whether to reposition the UITable or UIGrid the created Object is a child of.")]
		public FsmBool reposition;

		public override void Reset()
		{
			parent = null;
			childReference = null;
			amount = 5;
			customName = new FsmString { UseVariable = true };
			storeInstances = null;
			ignoreExisting = true;
			reposition = true;
		}

		public override void OnEnter()
		{
			storeInstances.Clear();

			GameObject src = Fsm.GetOwnerDefaultTarget(parent);
			bool hasReference = !childReference.IsNone && childReference.Value != null;
			string baseName = hasReference ? childReference.Value.name : "Empty GameObject";

			if (!customName.IsNone && !string.IsNullOrEmpty(customName.Value)) baseName = customName.Value;
			
			for (int i = 1; i < amount.Value + 1; i++)
			{
				string newName = baseName + " " + i;
				
				//skip if instance already exists
				var existing = src.transform.Find(newName);
				
				if (ignoreExisting.Value && existing != null)
				{
					//store reference to the existing instance anyway
					if(!storeInstances.IsNone) storeInstances.Add(existing.gameObject);
					
					continue;
				}
				
				//if an GameObject Reference has been set, create that, otherwise create an empty GameObject
				GameObject go;

				if(hasReference)
				{
					go = src.AddChild(childReference.Value);
					go.name = newName;
				} else
				{
					go = new GameObject(newName);
					go.transform.parent = src.transform;
				}
				
				if(!storeInstances.IsNone) storeInstances.Add(go);
			}

      //reposition Table or Grid if prevalent
			if(reposition.Value)
			{
				UITable mTable = NGUITools.FindInParents<UITable>(src);
				if(mTable != null) mTable.repositionNow = true;

				UIGrid mGrid = NGUITools.FindInParents<UIGrid>(src);
				if (mGrid != null) mGrid.repositionNow = true;
			}

			Finish();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, childReference);
    }
#endif
	}
}

#endif