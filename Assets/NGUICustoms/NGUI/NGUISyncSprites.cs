#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Synchronize the sprites of two UISprite components (e.g. for a character display that matches the In-Game sprites).")]
	public class NGUISyncSprites: FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UISprite))]
		[Tooltip("The source GameObject to get the current sprite of.")]
		public FsmOwnerDefault source;

		[RequiredField]
		[CheckForComponent(typeof(UISprite))]
    [Tooltip("The target GameObject to sync.")]
    public FsmOwnerDefault target;

    [Tooltip("Whether to also adjust snap the sprite to its original size after after switching out the sprite.")]
    public FsmBool snap;

    private UISprite _compSpriteSource, _compSpriteTarget;

		public override void Reset()
		{
		  base.Reset();

      source = null;
      target = new FsmOwnerDefault {OwnerOption = OwnerDefaultOption.SpecifyGameObject};
      snap = false;
		}

		public override void OnEnter()
		{
			_compSpriteSource = Fsm.GetOwnerDefaultTarget(source).GetComponent<UISprite>();
			_compSpriteTarget = Fsm.GetOwnerDefaultTarget(target).GetComponent<UISprite>();

			SyncAnimation();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			SyncAnimation();
		}

		private void SyncAnimation()
		{
		  if((_compSpriteSource.atlas as Object).name != (_compSpriteTarget.atlas as Object).name) {
		    LogError("Source atlas doesn't match with the target atlas! " +
		              "Please make sure that both UISprite component's use the same UIAtlas in order to synchronize the sprites.");
		    return;
		  }

			_compSpriteTarget.spriteName = _compSpriteSource.GetAtlasSprite().name;
			if (snap.Value) _compSpriteTarget.MakePixelPerfect();
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return "NGUI Sync: " + ActionHelpers.GetValueLabel(source.GameObject) + " -> " + ActionHelpers.GetValueLabel(target.GameObject);
    }
#endif
	}
}

#endif