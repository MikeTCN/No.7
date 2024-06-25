using UnityEditor;
using UnityEngine;

public class FullSailsGUI
{
	static GUIStyle headerStyle;
	static GUIStyle foldoutStyle;

	public static bool dragging = false;

	static public void Header(string name)
	{
		if ( headerStyle == null )
		{
			headerStyle = new GUIStyle();
			headerStyle.normal.textColor	= Color.grey;
			headerStyle.fontSize			= 16;
			headerStyle.fontStyle			= FontStyle.Bold;
		}
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.LabelField(name, headerStyle);
		EditorGUILayout.EndVertical();
	}

	static GUIStyle buttonStyle;

	static public bool BigButton(string name, string tip = "")
	{
		if ( buttonStyle == null )
		{
			buttonStyle = new GUIStyle(EditorStyles.miniButton);
			buttonStyle.fontSize	= 16;
			buttonStyle.fontStyle	= FontStyle.Bold;
			buttonStyle.fixedHeight = 40;
		}

		if ( GUILayout.Button(new GUIContent(name, tip), buttonStyle) )
			return true;

		return false;
	}

	static public void FoldOut(ref bool open, string name, string tip = "")
	{
		if ( headerStyle == null )
		{
			headerStyle = new GUIStyle();
			headerStyle.normal.textColor	= Color.white;
			headerStyle.fontSize			= 16;
			headerStyle.fontStyle			= FontStyle.Bold;
		}

		if ( foldoutStyle == null )
		{
			foldoutStyle = new GUIStyle(EditorStyles.foldout);
			Color col = new Color(0.8f, 0.8f, 0.8f, 1.0f);  //.white;
			foldoutStyle.normal.textColor		= col;
			foldoutStyle.fontSize				= 16;
			foldoutStyle.fontStyle				= FontStyle.Bold;
			//foldoutStyle.fixedWidth = 16;
			foldoutStyle.fontStyle				= FontStyle.Bold;
			foldoutStyle.normal.textColor		= col;
			foldoutStyle.onNormal.textColor		= col;
			foldoutStyle.hover.textColor		= col;
			foldoutStyle.onHover.textColor		= col;
			foldoutStyle.focused.textColor		= col;
			foldoutStyle.onFocused.textColor	= col;
			foldoutStyle.active.textColor		= col;
			foldoutStyle.onActive.textColor		= col;
		}

		EditorGUILayout.BeginVertical("box");
		open = EditorGUILayout.Foldout(open, new GUIContent(name, tip), true, foldoutStyle);
		EditorGUILayout.EndVertical();
	}
}