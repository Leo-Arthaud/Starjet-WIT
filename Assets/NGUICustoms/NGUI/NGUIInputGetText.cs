#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Store the text of a given UIInput component.")]
	public class NGUIInputGetText : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIInput))]
		[Tooltip("The GameObject with the UIInput component attached.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The text of the given UIInput component.")]
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

			UIInput compInput = go.GetComponent<UIInput>();

			if (compInput != null)
			{
				result.Value = compInput.value;
			} else LogError("No UIInput component attached to " + go.name + "!");
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