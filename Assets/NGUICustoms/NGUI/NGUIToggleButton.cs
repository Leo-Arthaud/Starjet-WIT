#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Toggles the state a UIButton on a GameObject. Optionally reset the Behaviour on exit.")]
	public class NGUIToggleButton : FsmStateAction
	{
		[RequiredField]
    [Tooltip("The GameObject with the UIButton component.")]
		public FsmOwnerDefault gameObject;

    [Tooltip("Restore the previous state of the UIButton.")]
		public FsmBool resetOnExit;

		private UIButton compButton;

		public override void Reset()
		{
			gameObject = null;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			ToggleButton();

			Finish();
		}

		void ToggleButton()
		{
		  var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

      compButton = go.GetComponent<UIButton>();

			if (compButton == null)
			{
				LogWarning(go.name + " missing component: UIButton");
				return;
			}

			compButton.isEnabled = !compButton.isEnabled;
		}

		public override void OnExit()
		{
			if (compButton != null && resetOnExit.Value) compButton.isEnabled = !compButton.isEnabled;
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