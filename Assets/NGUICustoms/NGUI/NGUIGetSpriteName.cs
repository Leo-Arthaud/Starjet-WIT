#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Store the name of the current sprite in the given UISprite component.")]
	public class NGUIGetSpriteName : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UISprite))]
		[Tooltip("The GameObject with the UISprite component attached.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The name of the current sprite.")]
		public FsmString result;

		public override void Reset()
		{
		  base.Reset();

		  gameObject = null;
		  result = null;
		}

		public override void OnEnter()
		{
			GetSpriteName();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			GetSpriteName();
		}

		void GetSpriteName()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				LogError("GameObject in " + Name + " is null!");
				return;
			}

			UISprite sprtComp = go.GetComponent<UISprite>();

			if (sprtComp != null)
			{
				result.Value = sprtComp.spriteName;
			} else LogError("No UISprite component attached to " + go.name + "!");
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