#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the text of a NGUI UILabel component.")]
	public class NGUILabelSetText : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UILabel))]
		[Tooltip("The GameObject with a UILabel component.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The new text for the UILabel.")]
    public FsmString text;
    
    [Tooltip("If bigger than 0, only display the first X amount of letters and append an ellipse (...) if the string was cut off too early.")]
    public FsmInt maxLetters;

		public override void Reset()
		{
		  base.Reset();

			gameObject = null;
      text = null;
      maxLetters = null;
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

      UILabel compLabel = go.GetComponent<UILabel>();

      if(compLabel == null)
      {
        LogError("GameObject " + go.name + " doesn't have a UILabel component.");
        return;
      }
      
      string result = text.Value.Replace("\\n", "\n");

      if (maxLetters.Value > 0)
      {
        int maxLength = maxLetters.Value > result.Length ? result.Length : maxLetters.Value;
        bool wasCutOff = result.Length > maxLength;
        result = result.Substring(0, maxLetters.Value) + (wasCutOff ? "..." : "");
      }

      compLabel.text = result;
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
