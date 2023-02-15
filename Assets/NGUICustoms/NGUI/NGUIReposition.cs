#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Repositions any UIGrid or UITable component on the given GameObject or any of it's ancestors.")]
	public class NGUIReposition : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to search the repositionable components from.")]
		public FsmOwnerDefault sourceGameObject;

		public override void Reset()
		{
			sourceGameObject = null;
		}

		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(sourceGameObject);
			
			UITable mTable = NGUITools.FindInParents<UITable>(go);
			if(mTable != null) mTable.repositionNow = true;

			UIGrid mGrid = NGUITools.FindInParents<UIGrid>(go);
			if(mGrid != null) mGrid.repositionNow = true;

			Finish();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, sourceGameObject.GameObject);
    }
#endif
	}
}

#endif