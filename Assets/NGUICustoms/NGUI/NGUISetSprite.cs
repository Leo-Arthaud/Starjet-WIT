using UnityEngine;

#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Changes the sprite of a UISprite component.")]
	public class NGUISetSprite : FsmStateAction
	{
		[RequiredField]
    [CheckForComponent(typeof(UISprite))]
		[Tooltip("The GameObject with the UISprite component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The name of the new sprite.")]
		public FsmString newSpriteName;
    
    [Tooltip("Adjust the scale to make it pixel-perfect (snap the width & height to the actual sprite's size).")]
    public FsmBool makePixelPerfect;
    
    [Tooltip("Whether it should go back to the previous sprite when leaving this state.")]
    public FsmBool resetOnExit;

    private UISprite compSprite;
    private string _prevSprite;
    private Vector2 _prevSize;

		public override void Reset()
		{
			gameObject = null;
			newSpriteName = null;
      makePixelPerfect = false;
      resetOnExit = false;

      _prevSprite = string.Empty;
      _prevSize = new Vector2();
    }

    public override void OnEnter()
		{
			DoSetSprite();
			Finish();
		}
    
    public override void OnExit()
    {
      if (!resetOnExit.Value || string.IsNullOrEmpty(_prevSprite)) return;
      
      compSprite.spriteName = _prevSprite;
      compSprite.width = (int)_prevSize.x;
      compSprite.height = (int)_prevSize.y;
      compSprite.MarkAsChanged();
    }

		private void DoSetSprite()
		{
			if(string.IsNullOrEmpty(newSpriteName.Value))
			{
				LogError("\"New Sprite Name\" is empty! Please specify.");
				return;
			}
      
      //get the UISprite component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
      compSprite = go.GetComponent<UISprite>();

      //store previous sprite name & size
      _prevSprite = compSprite.spriteName;
      _prevSize = new Vector2(compSprite.width, compSprite.height);
      
      //set new sprite name
      compSprite.spriteName = newSpriteName.Value;
      if (makePixelPerfect.Value) compSprite.MakePixelPerfect();
      compSprite.MarkAsChanged();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, newSpriteName);
    }
#endif
	}
}

#endif