#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Store the text of a given UILabel component.")]
	public class NGUILabelGetText : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UILabel))]
		[Tooltip("The GameObject with the UILabel component attached.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The text of the given UILabel component.")]
		public FsmString result;

		public override void Reset()
		{
		  base.Reset();

		  gameObject = null;
		  result = null;
		}

		public override void OnEnter()
		{
			GetText();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			GetText();
		}

		void GetText()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				LogError("GameObject in " + Name + " is null!");
				return;
			}

			UILabel compLabel = go.GetComponent<UILabel>();

			if (compLabel != null)
			{
				result.Value = compLabel.text;
			} else LogError("No UILabel component attached to " + go.name + "!");
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, gameObject.GameObject);
    }
#endif
	}
}

#endif