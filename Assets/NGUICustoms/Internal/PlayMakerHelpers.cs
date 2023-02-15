#if PLAYMAKER

/* Notes:
 * Any FsmVariable like FsmString, FsmBool, ... derives from NamedVariable, thus can be generalized by that.
 */

using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace iDecay.PlayMaker
{
	/// <summary>
	/// A collection of helper functions and extensions useful for creating custom actions.
	/// </summary>
	public static class PlayMakerHelpers
	{
		#region General

    /// <summary>
	  /// Returns whether the FsmString has a value and is not None.
	  /// </summary>
		public static bool IsSet(this FsmString str)
		{
			return !str.IsNone && !string.IsNullOrEmpty(str.Value);
		}

		#endregion

		#region FsmArray

    /// <summary>
	  /// Adds an entry to an FsmArray by resizing it and saving the changes afterwards.
	  /// </summary>
		public static void Add<T>(this FsmArray array, T entry)
		{
			if(array == null) return;

			array.SetType(entry.GetVariableType());

			if(entry != null)
			{
				array.Resize(array.Length + 1);
				array.Set(array.Length - 1, entry);
			} else 
				array.Resize(0);

			array.SaveChanges();
		}

		#endregion

		#region VariableType

		/// <summary>
		/// Tries to parse a supported variable type like "String" into 
		/// the matching NamedVariable (in this case NamedVariable.String).
		/// </summary>
		public static VariableType ParseVariableType(string strToParse)
		{
			return (VariableType)Enum.Parse(typeof(VariableType), strToParse, true);
		}

		/// <summary>
		/// Tries to parse the type of an object to its VariableType.
		/// </summary>
		public static VariableType ParseToVariableType(this object obj)
		{
			string strToParse = obj.GetType().ToString();
			strToParse = strToParse.Replace("System.", "");

			switch(strToParse)
			{
				case "Int16":
				case "Int32":
				case "Int64":
					strToParse = "Int";
					break;
				case "Object":
					strToParse = "Unknown";
					break;
			}

			return ParseVariableType(strToParse);
		}

    /// <summary>
		/// Returns the VariableType of the given variable.
		/// </summary>
		public static VariableType GetVariableType<T>(this T var)
		{
			Type type = var.GetType();

			if(type == typeof(int)) return VariableType.Int;
			if(type == typeof(float)) return VariableType.Float;
			if(type == typeof(bool)) return VariableType.Bool;
			if(type == typeof(string)) return VariableType.String;
			if(type == typeof(Color)) return VariableType.Color;
			if(type == typeof(GameObject)) return VariableType.GameObject;
			if(type == typeof(Material)) return VariableType.Material;
			if(type == typeof(Vector2)) return VariableType.Vector2;
			if(type == typeof(Vector3)) return VariableType.Vector3;
			
			return VariableType.Object;
		}

		#endregion

		#region Converter
		/////////////////

		/******************************
		********** FsmString **********
		******************************/

		/// <summary>
		/// Converts an FsmString-Array to a list.
		/// </summary>
		public static List<string> ToList(this FsmString[] fsmString)
		{
			List<string> result = new List<string>();

			for(int i = 0; i < fsmString.Length; i++)
				result.Add(fsmString[i].Value);

			return result;
		}

		/// <summary>
		/// Converts an FsmString-Array to an Array.
		/// </summary>
		public static string[] ToArray(this FsmString[] fsmString)
		{
			string[] result = new string[fsmString.Length];

			for(int i = 0; i < fsmString.Length; i++)
				result[i] = fsmString[i].Value;

			return result;
		}

		/******************************
		************ FsmVar ***********
		******************************/

		/// <summary>
		/// Converts an FsmVar-Array to a list.
		/// </summary>
		public static List<object> ToList(this FsmVar[] fsmVar)
		{
			List<object> result = new List<object>();

			for(int i = 0; i < fsmVar.Length; i++)
				result.Add(fsmVar[i].GetValue());

			return result;
		}

		/// <summary>
		/// Converts an FsmVar-Array to an Array.
		/// </summary>
		public static object[] ToArray(this FsmVar[] fsmVar)
		{
			object[] result = new object[fsmVar.Length];

			for(int i = 0; i < fsmVar.Length; i++)
				result[i] = fsmVar[i].GetValue();

			return result;
		}

		#endregion

		#region Extensions

    /// <summary>
		/// An alternative way to get the owner of an FsmOwnerDefault variable from external/Editor scripts that can't derive from FsmStateAction.
		/// </summary>
		public static GameObject GetOwner(this FsmOwnerDefault odt)
		{
			//Fsm.GetOwnerDefaultTarget() unfortunately requires to derive 
			//from FsmStateAction which is not possible here; so try it this way instead:
			GameObject go = odt.GameObject.Value;
			if(go == null) UnityEngine.Debug.LogError("GameObject is null!");

			return go;
		}

		#endregion
	}
}

#endif