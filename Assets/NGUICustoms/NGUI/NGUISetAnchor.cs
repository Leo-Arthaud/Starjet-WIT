#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Anchor a UIWidget or deriving component to the specified GameObject and optionally adjust the offsets.")]
	public class NGUISetAnchor : FsmStateActionUpdate
	{
		[RequiredField]
    [CheckForComponent(typeof(UIWidget))]
		[Tooltip("The target GameObject with the NGUI component attached.")]
		public FsmOwnerDefault gameObject;

    [RequiredField]
		[Tooltip("Set the target GameObject to anchor to.")]
		public FsmGameObject target;

    [Tooltip("Customize the left offset from the target GameObject.")]
    public FsmInt leftOffset;
    
    [Tooltip("Customize the right offset from the target GameObject.")]
    public FsmInt rightOffset;
    
    [Tooltip("Customize the bottom offset from the target GameObject.")]
    public FsmInt bottomOffset;
    
    [Tooltip("Customize the top offset from the target GameObject.")]
    public FsmInt topOffset;

    [ObjectType(typeof(UIRect.AnchorUpdate))]
    [Tooltip("Optionally change how the anchor should be updated.")]
    public FsmEnum updateAnchors;

    public override void Reset()
		{
		  base.Reset();

			gameObject = null;
			target = null;
      leftOffset = new FsmInt {UseVariable = true};
      rightOffset = new FsmInt {UseVariable = true};
      bottomOffset = new FsmInt {UseVariable = true};
      topOffset = new FsmInt {UseVariable = true};
      updateAnchors = new FsmEnum {UseVariable = true};
		}

		public override void OnEnter()
		{
			DoSetAnchor();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoSetAnchor();
		}

		private void DoSetAnchor()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				LogError("GameObject in " + Owner.name + " is null!");
				return;
			}

			UIWidget widget = go.GetComponent<UIWidget>();

      if (widget == null) return;
      
      widget.SetAnchor(target.Value);
      widget.leftAnchor.relative = 0f;
      widget.rightAnchor.relative = 1f;
      widget.bottomAnchor.relative = 0f;
      widget.topAnchor.relative = 1f;
      
      if (!leftOffset.IsNone) widget.leftAnchor.absolute += leftOffset.Value;
      if (!rightOffset.IsNone) widget.rightAnchor.absolute += rightOffset.Value;
      if (!bottomOffset.IsNone) widget.bottomAnchor.absolute += bottomOffset.Value;
      if (!topOffset.IsNone) widget.topAnchor.absolute += topOffset.Value;
      
      if (!updateAnchors.IsNone) widget.updateAnchors = (UIRect.AnchorUpdate)updateAnchors.Value;
    }

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoNameGetProperty(this, gameObject.GameObject, target);
    }
#endif
	}
}

#endif