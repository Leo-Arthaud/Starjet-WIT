#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the text of multiple NGUI UILabel component by converting and concatenating the given "
	       + "variables to a string and optionally adds additional strings after each string part, to the start or at the end.")]
	public class NGUILabelSetTextAdvanced : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UILabel))]
		[Tooltip("The GameObject with a UILabel component.")]
		public FsmGameObject[] gameObjects;

		[RequiredField] 
		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		[Tooltip("The variables to convert to a string and set as the label text.")]
		public FsmVar[] stringParts;

		[Tooltip("Add a string between each string part.")]
    public FsmString divider;

		[Tooltip("Add a string to the start of the resulting text.")]
		public FsmString addToFront;

		[Tooltip("Add a string to the end of the resulting text.")]
		public FsmString addToBack;

		private GameObject go;
		private UILabel label;
		private string text;
		private string generatedString = "";
		private char[] chars = null;
		private bool isGenerated = false;
		private int charCount = 0;
		private int charCapacity = 0;

		private void Clear()
		{
		  base.Reset();

			text = null;
			generatedString = "";
			chars = null;
			isGenerated = false;
			charCount = 0;
			charCapacity = 0;
		}

		public override void Reset()
		{
			base.Reset();

			gameObjects = new FsmGameObject[1];
			stringParts = new FsmVar[1];
			divider = null;
			addToFront = null;
			addToBack = null;
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
			ConvertText();

			foreach(var go in gameObjects)
			{
				if(!go.Value)
				{
					LogError("GameObject in " + Owner.name + " (" + Fsm.Name + ") is null!");
					return;
				}

				label = go.Value.GetComponent<UILabel>();

				if(label == null)
				{
					LogError("GameObject " + go.Value.name + " doesn't have a UILabel component.");
					return;
				}

				label.text = text;
			}
		}

		void ConvertText()
		{
			text = addToFront.Value;

			chars = new char[charCapacity = 32];
			charCount = 0;
			isGenerated = false;

			for(var i = 0; i < stringParts.Length - 1; i++)
			{
				stringParts[i].UpdateValue();
				string strToAdd = stringParts[i].GetValue().ToString();
				if(i < stringParts.Length - 1) strToAdd += divider;

				Append(strToAdd);
			}

			ToString();

			stringParts[stringParts.Length - 1].UpdateValue();
			Append(stringParts[stringParts.Length - 1].GetValue().ToString());

			text += ToString();
			text += addToBack.Value;
		}

		public void Append(string value)
		{
			if(charCount + value.Length > charCapacity)
			{
				charCapacity = System.Math.Max(charCapacity + value.Length, charCapacity * 2);
				char[] newChars = new char[charCapacity];
				chars.CopyTo(newChars, 0);
				chars = newChars;
			}

			int n = value.Length;

			for(int i = 0; i < n; i++) chars[charCount + i] = value[i];

			charCount += n;
			isGenerated = false;
		}

		public override string ToString()
		{
			if(!isGenerated)
			{
				generatedString = new string(chars, 0, charCount);
				isGenerated = true;
			}
			return generatedString;
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, gameObjects);
    }
#endif
	}
}

#endif