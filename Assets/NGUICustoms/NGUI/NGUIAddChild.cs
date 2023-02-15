#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Adds a child to a NGUI Element. Use this instead of 'Create Object' when creating NGUI objects.")]
	public class NGUIAddChild : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The parent to add the object to. Strongly recommended to be a child of an NGUI UI Root or an UI Root itself, "
		    + "otherwise creates one in the root of the Hierarchy (wich is probably not a desired).")]
		public FsmGameObject parent;

		[Tooltip("The GameObject to add. Creates an empty GameObject if not set.")]
		public FsmGameObject childReference;

		[Tooltip("The name for the newly created GameObject. Leave as 'None' for the default value (the actual name of the GameObject).")]
		public FsmString name;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the created object in a variable.")]
		public FsmGameObject storeChildInstance;

		public override void Reset()
		{
			parent = new FsmGameObject {UseVariable = true};
			childReference = null;
			name = new FsmString { UseVariable = true };
			storeChildInstance = null;
		}

		public override void OnEnter()
		{
			//if a GameObject Reference has been set, create that, otherwise create an empty GameObject
			if(!childReference.IsNone && childReference.Value != null)
			{
				storeChildInstance.Value = parent.Value.AddChild(childReference.Value);

				//If the name has been set, use that for the created GameObject,
				//otherwise use the name of the set reference
				storeChildInstance.Value.name = !name.IsNone ? name.Value : childReference.Value.name;

			  //also apply the layer of the parent to the new GameObject
			  //as NGUI doesn't recommend to have children with different layers
			  storeChildInstance.Value.layer = parent.Value.layer;
			} else
			{
				var go = new GameObject("Empty GameObject");
				go.transform.parent = parent.Value.transform;
				if(!name.IsNone) go.name = name.Value;

				storeChildInstance.Value = go;
				storeChildInstance.Value.layer = parent.Value.layer;
			}

			Finish();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      string parentValLabel = parent.Value == Owner ? "Owner" : parent.Value.name;
      return "NGUI Add Child : " + parentValLabel + " > " + ActionHelpers.GetValueLabel(childReference);
    }
#endif
	}
}

#endif