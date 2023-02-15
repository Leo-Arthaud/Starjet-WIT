#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Snaps the width and height of a NGUI UISprite, UI2DSprite, UITexture or UILabel component to it's original size (making it 'Pixel Perfect').")]
	public class NGUISnap : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI component attached.")]
		public FsmOwnerDefault target;

		public override void Reset()
		{
			target = null;
		}

		public override void OnEnter()
		{
			DoSnap();
      Finish();
		}

		void DoSnap()
		{
			var go = Fsm.GetOwnerDefaultTarget(target);

			UISprite sprtComp = go.GetComponent<UISprite>();
			UI2DSprite sprt2DComp = go.GetComponent<UI2DSprite>();
			UITexture texComp = go.GetComponent<UITexture>();
			UILabel lblComp = go.GetComponent<UILabel>();

      if (sprtComp != null) {
        sprtComp.MakePixelPerfect();
        sprtComp.MarkAsChanged();
        return;
      }
      
      if (sprt2DComp != null) {
        sprt2DComp.MakePixelPerfect();
        sprt2DComp.MarkAsChanged();
        return;
      }

      if (texComp != null) {
        texComp.MakePixelPerfect();
        texComp.MarkAsChanged();
        return;
      }
      
      if (lblComp != null) {
        lblComp.MakePixelPerfect();
	      lblComp.MarkAsChanged();
        return;
      }
      
      LogError("No supported NGUI component attached to " + go.name + "!");
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, target.GameObject);
    }
#endif
	}
}

#endif