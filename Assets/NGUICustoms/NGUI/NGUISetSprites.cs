#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Changes the sprite of multiple UISprite components at the same time.")]
	public class NGUISetSprites : FsmStateAction
	{
		[RequiredField]
		[Tooltip("UISprites to override.")]
		public FsmGameObject[] sprites;

		[RequiredField]
		[Tooltip("The name of the new sprite.")]
		public FsmString newSpriteName;

		[Tooltip("Enable or disable all UISprite Components.")]
		public FsmBool enableComponent;

		[Tooltip("Enable or disable all specified GameObjects.")]
		public FsmBool enableGameObjects;

		public override void Reset()
		{
			sprites = new FsmGameObject[3];
			newSpriteName = null;
			enableComponent = new FsmBool() { UseVariable = true };
			enableGameObjects = new FsmBool() { UseVariable = true };
		}

		//required since PlayMaker 1.8.5 to use OnLateUpdate()
		public override void OnPreprocess()
		{
#if PLAYMAKER_1_8_5_OR_NEWER
			Fsm.HandleLateUpdate = true;
#endif
		}

		public override void OnEnter()
		{
			DoSetSprite();
			Finish();
		}

		private void DoSetSprite()
		{
			if(string.IsNullOrEmpty(newSpriteName.Value))
			{
				LogError("\"New Sprite Name\" is empty. Please specify.");
				return;
			}
			
			foreach(var go in sprites)
			{
				// exit if objects are null
				if(go == null)
				{
					LogWarning("One of the elements is null/not defined.");
					return;
				}

				//get the UISprite component
				UISprite sprite = go.Value.GetComponent<UISprite>();

				//exit if no component found
				if(sprite == null)
				{
					LogError("No UISprite component found on " + go.Value.name);
					return;
				}

				if(!enableGameObjects.IsNone) 
					go.Value.SetActive(enableGameObjects.Value);

				if(!enableComponent.IsNone) 
					sprite.enabled = enableComponent.Value;

				//set new sprite name
				sprite.spriteName = newSpriteName.Value;
			}
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