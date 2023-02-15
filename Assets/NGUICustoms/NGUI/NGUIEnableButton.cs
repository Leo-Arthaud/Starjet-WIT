#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Enables/Disables a UIButton on a GameObject. Optionally reset the Behaviour on exit.")]
	public class NGUIEnableButton : FsmStateAction
	{
		[RequiredField]
    [Tooltip("The GameObject with the UIButton component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
    [Tooltip("Whether to enable or disable the button.")]
		public FsmBool enable;

    [Tooltip("Restore the previous state of the UIButton.")]
		public FsmBool resetOnExit;

		private UIButton compButton;

		public override void Reset()
		{
			gameObject = null;
			enable = true;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			EnableButton();

			Finish();
		}

		void EnableButton()
		{
		  var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

      compButton = go.GetComponent<UIButton>();

			if (compButton == null)
			{
				LogError(go.name + " missing component: UIButton");
				return;
			}

			compButton.isEnabled = enable.Value;
		}

		public override void OnExit()
		{
			if (compButton != null && resetOnExit.Value) compButton.enabled = !enable.Value;
		}

	  public override string ErrorCheck()
	  {
	      var go = Fsm.GetOwnerDefaultTarget(gameObject);

	      if (go != null && go.GetComponent<UIButton>() == null) return go.name + " missing component: UIButton";

	      return "";
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