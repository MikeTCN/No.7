using UnityEngine;
using UnityEditor;

public class SailsURPGUI : ShaderGUI
{
	public static bool generalParams	= false;
	public static bool emblemParams		= false;
	public static bool damageParams		= false;
	public static bool rippleParams		= false;
	public static bool furlingParams	= false;
	public static bool sailParams		= false;
	public static bool impactParams		= false;

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		Material mat = materialEditor.target as Material;

		string[] keyWords = mat.shaderKeywords;

		// General
		MaterialProperty color		= FindProperty("_Color",		properties);
		MaterialProperty maintex	= FindProperty("_MainTex",		properties);
		//MaterialProperty speccol = FindProperty("_SpecColor",		properties);
		MaterialProperty gloss		= FindProperty("_Glossiness",	properties);
		MaterialProperty bumpmap	= FindProperty("_BumpMap",		properties);

		FullSailsGUI.FoldOut(ref generalParams, "General Values");
		if ( generalParams )
		{
			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(color,	"Color");
			materialEditor.ShaderProperty(maintex,	"Main Texture");
			//materialEditor.ShaderProperty(speccol, "Specular Color");
			materialEditor.ShaderProperty(gloss,	"Glossiness");
			materialEditor.ShaderProperty(bumpmap,	"Bump Map");

			EditorGUILayout.EndVertical();
		}

		// Emblem
		FullSailsGUI.FoldOut(ref emblemParams, "Emblem Values");
		if ( emblemParams )
		{
			MaterialProperty emblemtex		= FindProperty("_EmblemTex",		properties);
			MaterialProperty emblemcolor	= FindProperty("_EmblemColor",		properties);
			MaterialProperty emblememcolor	= FindProperty("_EmblemEmColor",	properties);
			MaterialProperty emblembackface	= FindProperty("_EmblemBackface",	properties);

			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(emblemtex,		"Texture");
			materialEditor.ShaderProperty(emblemcolor,		"Color");
			materialEditor.ShaderProperty(emblememcolor,	"Emissive");
			materialEditor.ShaderProperty(emblembackface,	"Backface");

			EditorGUILayout.EndVertical();
		}

		// Damage values
		FullSailsGUI.FoldOut(ref damageParams, "Damage Values");
		if ( damageParams )
		{
			MaterialProperty damage		= FindProperty("_Damage",		properties);
			MaterialProperty damagetex1 = FindProperty("_DamageTex",	properties);
			MaterialProperty damagetex2 = FindProperty("_DamageTex1",	properties);
			MaterialProperty damagetex3 = FindProperty("_DamageTex2",	properties);
			MaterialProperty cutoff		= FindProperty("_Cutoff",		properties);

			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(damage,		"Damage");
			materialEditor.ShaderProperty(damagetex1,	"Damage Tex");
			materialEditor.ShaderProperty(damagetex2,	"Damage Tex 1");
			materialEditor.ShaderProperty(damagetex3,	"Damage Tex 2");
			materialEditor.ShaderProperty(cutoff,		"Alpha Cutoff");

			EditorGUILayout.EndVertical();
		}

		// Sail params
		FullSailsGUI.FoldOut(ref sailParams, "Sail Values");
		if ( sailParams )
		{
			MaterialProperty speed				= FindProperty("_Speed",			properties);
			MaterialProperty winddir			= FindProperty("_WindDir",			properties);
			MaterialProperty sailforward		= FindProperty("_SailForward",		properties);
			MaterialProperty maskmap			= FindProperty("_MaskMap",			properties);
			MaterialProperty maskmaprev			= FindProperty("_MaskMapRev",		properties);
			MaterialProperty sailwind			= FindProperty("_SailWind",			properties);
			MaterialProperty fillpercent		= FindProperty("_FillPercent",		properties);
			MaterialProperty sideways			= FindProperty("_SailSideways",		properties);
			MaterialProperty saillift			= FindProperty("_SailLift",			properties);
			MaterialProperty sailliftarch		= FindProperty("_SailLiftArch",		properties);
			MaterialProperty sailliftsidearch	= FindProperty("_SailLiftSideArch", properties);
			MaterialProperty sailarch			= FindProperty("_SailArch",			properties);
			MaterialProperty sailarchstart		= FindProperty("_SailArchStart",	properties);
			MaterialProperty sailsidearch		= FindProperty("_SailSideArch",		properties);
			MaterialProperty sailtoparch		= FindProperty("_SailTopArch",		properties);
			MaterialProperty sailtaper			= FindProperty("_SailTaper",		properties);
			MaterialProperty sailshear			= FindProperty("_SailShear",		properties);
			MaterialProperty sailtilt			= FindProperty("_SailTilt",			properties);
			MaterialProperty sailtilttop		= FindProperty("_SailTiltTop",		properties);
			MaterialProperty sailreverse		= FindProperty("_SailReverse",		properties);
			MaterialProperty fullripple			= FindProperty("_FullRipple",		properties);

			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(speed,			"Speed");
			materialEditor.ShaderProperty(winddir,			"Wind Direction");
			materialEditor.ShaderProperty(sailforward,		"Sail Forward");
			materialEditor.ShaderProperty(maskmap,			"Mask Map");
			materialEditor.ShaderProperty(maskmaprev,		"Mask Map Rev");
			materialEditor.ShaderProperty(sailwind,			"Sail Wind");
			materialEditor.ShaderProperty(fillpercent,		"Fill Percent");
			materialEditor.ShaderProperty(sideways,			"Sail Sideways");
			materialEditor.ShaderProperty(saillift,			"Sail Lift");
			materialEditor.ShaderProperty(sailliftarch,		"Arch frm Lift");
			materialEditor.ShaderProperty(sailliftsidearch,	"Side Arch from Lift");
			materialEditor.ShaderProperty(sailarch,			"Arch");
			materialEditor.ShaderProperty(sailarchstart,	"Arch Start");
			materialEditor.ShaderProperty(sailsidearch,		"Side Arch");
			materialEditor.ShaderProperty(sailtoparch,		"Top Arch");
			materialEditor.ShaderProperty(sailtaper,		"Taper");
			materialEditor.ShaderProperty(sailshear,		"Shear");
			materialEditor.ShaderProperty(sailtilt,			"Tilt");
			materialEditor.ShaderProperty(sailtilttop,		"Tilt Top");
			materialEditor.ShaderProperty(sailreverse,		"Reverse Amount");
			materialEditor.ShaderProperty(fullripple,		"Full Ripple Reduce");

			EditorGUILayout.EndVertical();
		}

		// Furling
		FullSailsGUI.FoldOut(ref furlingParams, "Furling Values");
		if ( furlingParams )
		{
			MaterialProperty furlmap		= FindProperty("_FurlMap",		properties);
			MaterialProperty furledmap		= FindProperty("_FurledMap",	properties);
			MaterialProperty furl			= FindProperty("_Furl",			properties);
			MaterialProperty furlradius		= FindProperty("_FurledRadius",	properties);

			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(furlmap,		"Furl Map");
			materialEditor.ShaderProperty(furledmap,	"Furled Map");
			materialEditor.ShaderProperty(furl,			"Furl");
			materialEditor.ShaderProperty(furlradius,	"Furled Radius");

			EditorGUILayout.EndVertical();
		}

		// Rippled
		FullSailsGUI.FoldOut(ref rippleParams, "Ripple Values");
		if ( rippleParams )
		{
			MaterialProperty vorspeed		= FindProperty("_VorSpeed",			properties);
			MaterialProperty vorseed		= FindProperty("_VorSeed",			properties);
			MaterialProperty vorstrength	= FindProperty("_VorStrength",		properties);
			MaterialProperty vorscale		= FindProperty("_VorScale",			properties);
			MaterialProperty vorsmooth		= FindProperty("_VorSmoothness",	properties);
			MaterialProperty noise			= FindProperty("_RippleNoise",		properties);

			EditorGUILayout.BeginVertical("box");

			materialEditor.ShaderProperty(vorstrength,	"Strength");
			materialEditor.ShaderProperty(vorspeed,		"Speed");
			materialEditor.ShaderProperty(vorseed,		"Seed");
			materialEditor.ShaderProperty(vorscale,		"Scale");
			materialEditor.ShaderProperty(vorsmooth,	"Smoothness");
			materialEditor.ShaderProperty(noise,		"Ripple Noise (R)");

			EditorGUILayout.EndVertical();
		}
		// Translucency
#if false
		MaterialProperty thickness	= FindProperty("_Thickness",	properties);
		MaterialProperty power		= FindProperty("_Power",		properties);
		MaterialProperty distortion	= FindProperty("_Distortion",	properties);
		MaterialProperty scale		= FindProperty("_Scale",		properties);
		MaterialProperty subcolor	= FindProperty("_SubColor",		properties);

		Header("Ripple Values");
		EditorGUILayout.BeginVertical("box");

		materialEditor.ShaderProperty(thickness,	"Thickness");
		materialEditor.ShaderProperty(power,		"Power");
		materialEditor.ShaderProperty(distortion,	"Distortion");
		materialEditor.ShaderProperty(scale,		"Scale");
		materialEditor.ShaderProperty(subcolor,		"Sub Color");

		EditorGUILayout.EndVertical();
#endif
		MaterialProperty impactcount = FindProperty("_ImpactCount", properties, false);

		if ( impactcount != null )
		{
			FullSailsGUI.FoldOut(ref impactParams, "Impacts", "Params for the Impact effects of the Sail");
			if ( impactParams )
			{
				MaterialProperty impacttex = FindProperty("_ImpactTex", properties);
				MaterialProperty ripple = FindProperty("_ImpactRipple", properties);
				MaterialProperty location = FindProperty("_ImpactLocation", properties);
				MaterialProperty impcutoff = FindProperty("_ImpactCutoff", properties);

				EditorGUILayout.BeginVertical("box");

				materialEditor.ShaderProperty(impactcount, "Impact Count");	//, "How many impacts to apply to the sail");
				materialEditor.ShaderProperty(impacttex, "Impact Tiles");	//, "The Impact tile texture");
				materialEditor.ShaderProperty(ripple, "Ripple Texture(A)");	//, "The wave effect texture");
				materialEditor.ShaderProperty(location, "Location");	//, "Location of the last hit(XY) Time(Z) Strength(W)");
				materialEditor.ShaderProperty(impcutoff, "Impact Cutoff");	//, "Impact texture alpha cutoff");

				EditorGUILayout.EndVertical();
			}
		}
	}
}
