#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the text of a NGUI UIInput component.")]
	public class NGUIInputSetText : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIInput))]
		[Tooltip("The GameObject with a UIInput component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The new text for the UIInput.")]
    public FsmString text;

		public override void Reset()
		{
		  base.Reset();

			gameObject = null;
      text = null;
		}

		public override void OnEnter()
		{
			SetText();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			SetText();
		}

		void SetText()
		{
		  GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if(!go)
      {
        LogError("GameObject in " + Owner.name + " (" + Fsm.Name + ") is null!");
        return;
      }

      UIInput compInput = go.GetComponent<UIInput>();

      if(compInput == null)
      {
        LogError("GameObject " + go.name + " doesn't have a UIInput component.");
        return;
      }

      compInput.value = text.Value;
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