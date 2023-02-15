using System.Collections.Generic;
using UnityEngine;

#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Increments the last number of the current sprite name to effectively display the next frame of a spritesheet " +
           "and optionally sends an event, if the last frame has been reached.")]
	public class NGUISetNextFrame : FsmStateAction
	{
		[RequiredField]
    [CheckForComponent(typeof(UISprite))]
		[Tooltip("The GameObject with the UISprite component.")]
		public FsmOwnerDefault gameObject;

    [Tooltip("Event to send if the last sprite of the frame sheet has been reached (if there's no more incrementation afterwards).")]
    public FsmEvent hasReachedEnd;
    
    private int id;

    public override void Reset()
		{
			gameObject = null;
      hasReachedEnd = null;
    }

    public override void OnEnter()
		{
			DoSetSprite();
			Finish();
		}

    private void DoSetSprite()
		{
      //get the UISprite component
      var go = Fsm.GetOwnerDefaultTarget(gameObject);
      UISprite compSprite = go.GetComponent<UISprite>();

      string currSprite = compSprite.spriteName;
      string lastNum = GetLastNumber(currSprite);

      if(!int.TryParse(lastNum, out id) || !currSprite.EndsWith(lastNum))
      {
        LogError("Current sprite name has no number to increment!");
        return;
      }

      string spriteBase = currSprite.Remove(currSprite.Length - lastNum.Length);
      string newSprite = spriteBase + id;
      string nextSprite = spriteBase + (id + 1);

      //set new sprite name
      compSprite.spriteName = newSprite;
      compSprite.MarkAsChanged();
      
      //send the Has Reached End event if the next frame after the new one doesn't exist on the atlas
      if (compSprite.atlas.GetSprite(nextSprite) == null) Fsm.Event(hasReachedEnd);
		}

    private string GetLastNumber(string input)
    {
      var stack = new Stack<char>();

      for (var i = input.Length - 1; i >= 0; i--)
      {
        if (!char.IsNumber(input[i])) break;
        
        stack.Push(input[i]);
      }

      return new string(stack.ToArray());
    }
	}
}

#endif