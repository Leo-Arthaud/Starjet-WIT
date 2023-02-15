#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Change the size of a NGUI UISprite, UI2DSprite, UITexture or UILabel component. Alternatively snap to the actual sprite's size.")]
	public class NGUISetSize : FsmStateActionUpdate
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI component attached.")]
		public FsmOwnerDefault target;

		[Tooltip("The size to apply to the UISprite.")]
		public FsmVector2 size;

		[Tooltip("Adjust the scale to make it pixel-perfect (if set ignores the size values).")]
		public FsmBool makePixelPerfect;

		[Tooltip("Resize Collider to the UISprite size if any is attached.")]
		public FsmBool resizeCollider;

		public override void Reset()
		{
		  base.Reset();

			target = null;
			size = Vector2.zero;
			makePixelPerfect = false;
			resizeCollider = false;
		}

		public override void OnEnter()
		{
			DoSetDetails();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoSetDetails();
		}

		void DoSetDetails()
		{
			var go = Fsm.GetOwnerDefaultTarget(target);

			if (go == null)
			{
				LogError("GameObject in " + Name + " is null!");
				return;
			}

			UISprite sprtComp = go.GetComponent<UISprite>();
			UI2DSprite sprt2DComp = go.GetComponent<UI2DSprite>();
			UITexture texComp = go.GetComponent<UITexture>();
			UILabel lblComp = go.GetComponent<UILabel>();
			UIPanel panelComp = go.GetComponent<UIPanel>();

			if(sprtComp == null && sprt2DComp == null && texComp == null && lblComp == null && panelComp == null) {
			  LogError("No supported NGUI component attached to " + go.name + "!");
			  return;
			}

      if (sprtComp != null) {
        sprtComp.width = (int)size.Value.x;
        sprtComp.height = (int)size.Value.y;
  
        if (makePixelPerfect.Value) sprtComp.MakePixelPerfect();
        if (resizeCollider.Value) sprtComp.ResizeCollider();
  
        sprtComp.MarkAsChanged();
      }
      else if (sprt2DComp != null) {
        sprt2DComp.width = (int)size.Value.x;
        sprt2DComp.height = (int)size.Value.y;

        if (makePixelPerfect.Value) sprt2DComp.MakePixelPerfect();
        if (resizeCollider.Value) sprt2DComp.ResizeCollider();

        sprt2DComp.MarkAsChanged();
      }
      else if (texComp != null) {
        texComp.width = (int)size.Value.x;
        texComp.height = (int)size.Value.y;

        if (makePixelPerfect.Value) texComp.MakePixelPerfect();
        if (resizeCollider.Value) texComp.ResizeCollider();

        texComp.MarkAsChanged();
      }
      else if (lblComp != null) {
	      lblComp.width = (int)size.Value.x;
	      lblComp.height = (int)size.Value.y;

	      if (makePixelPerfect.Value) lblComp.MakePixelPerfect();
	      if (resizeCollider.Value) lblComp.ResizeCollider();

	      lblComp.MarkAsChanged();
      }
      else if (panelComp != null) {
        Vector3 pos = panelComp.cachedTransform.localPosition;
        panelComp.SetRect(pos.x, pos.y, size.Value.x, size.Value.y);
      }
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoNameGetProperty(this, target.GameObject, size);
    }
#endif
	}
}

#endif