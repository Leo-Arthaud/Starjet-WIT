#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Adds a child to a NGUI Element. Use this instead of 'Create Object' when creating NGUI objects." +
		 	 "Optionally set a custom name, position and/or rotation of the created instance. " +
			 "Additionaly adds the ability to reference a local GameObject as parent.")]
	public class NGUIAddChildAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The parent to add the object to. Strongly recommended to be a child of an NGUI UI Root or an UI Root itself, "
		    + "otherwise creates one in the root of the Hierarchy (wich is probably not a desired).")]
		public FsmGameObject parent;

		[Tooltip("The GameObject to add. Creates an empty GameObject if not set.")]
		public FsmGameObject childReference;

		[Tooltip("The name for the newly created GameObject. Leave as 'None' for the default value (the actual name of the GameObject).")]
		public FsmString name;

		[Tooltip("Define the child index at which position the new instance should be placed.")]
		public FsmInt atIndex;

		[Tooltip("Change the depth of the new NGUI component.")]
		public FsmInt depth;

    [ActionSection("Transform")]

		[Tooltip("Change the position of the new instance.")]
		public FsmVector3 position;

		[Tooltip("Change the rotation of the new instance.")]
		public FsmVector3 rotation;

		[Tooltip("Change the scale of the new instance.")]
		public FsmVector3 scale;

		[Tooltip("Whether the transform values should be applied in local or world space.")]
		public Space space;

    [ActionSection("Result")]

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the created object in a variable.")]
		public FsmGameObject storeChildInstance;

		[Tooltip("Whether to reposition the UITable or UIGrid the created Object is a child of " +
				 "(should be disabled when creating a lot of items to manually reposition afterwards).")]
		public FsmBool reposition;

		public override void Reset()
		{
			parent = new FsmGameObject {UseVariable = true};
			childReference = null;
			name = new FsmString { UseVariable = true };
			atIndex = new FsmInt { UseVariable = true };
			depth = new FsmInt { UseVariable = true };
			position = new FsmVector3 { UseVariable = true };
			rotation = new FsmVector3 { UseVariable = true };
			scale = new Vector3(1, 1, 1);
			space = Space.Self;
			storeChildInstance = null;
			reposition = true;
		}

		public override void OnEnter()
		{
			//if a GameObject Reference has been set, create that, otherwise create an empty GameObject
			if(!childReference.IsNone && childReference.Value != null)
			{
				storeChildInstance.Value = NGUITools.AddChild(parent.Value, childReference.Value);

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
				go.name = !name.IsNone ? name.Value : go.gameObject.name;

				storeChildInstance.Value = go;
				storeChildInstance.Value.layer = parent.Value.layer;
			}

			//re-index
			if(!atIndex.IsNone) {
			  int lastChildID = storeChildInstance.Value.transform.parent.childCount - 1;
			  storeChildInstance.Value.transform.SetSiblingIndex(Mathf.Clamp(atIndex.Value, 0, lastChildID));
			}

			//set depth
			if(!depth.IsNone) {
			  UIWidget widget = storeChildInstance.Value.GetComponent<UIWidget>();
			  if(widget == null) {
			    LogError("New child doesn't have any UIWidget component (UISprite, UITexture, ...) to set the depth off of!");
			  } else widget.depth = depth.Value;
			}

      //apply position
			if(!position.IsNone)
			{
				if(space == Space.Self) storeChildInstance.Value.transform.localPosition = position.Value;
				else storeChildInstance.Value.transform.position = position.Value;
			}

      //apply rotation
			if(!rotation.IsNone)
			{
				if(space == Space.Self) storeChildInstance.Value.transform.localRotation = Quaternion.Euler(rotation.Value);
				else storeChildInstance.Value.transform.rotation = Quaternion.Euler(rotation.Value);
			}

      //apply scale
			if(!scale.IsNone) storeChildInstance.Value.transform.localScale = scale.Value;

      //reposition Table or Grid if prevalent
			if(reposition.Value)
			{
				UITable mTable = NGUITools.FindInParents<UITable>(storeChildInstance.Value);
				if(mTable != null) mTable.repositionNow = true;

				UIGrid mGrid = NGUITools.FindInParents<UIGrid>(storeChildInstance.Value);
				if(mGrid != null) mGrid.repositionNow = true;
			}

			Finish();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      string parentValLabel = parent.Value == Owner ? "Owner" : parent.Value.name;
      return "NGUI Add Child (adv.) : " + parentValLabel + " > " + ActionHelpers.GetValueLabel(childReference);
    }
#endif
	}
}

#endif