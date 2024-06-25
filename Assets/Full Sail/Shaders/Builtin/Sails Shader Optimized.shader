﻿Shader "Full Sails/Sails Optimized"
{
	// This shader is the same as the normal Sail Shader but has the Mask, Furl and Furled maps combined to one texture for a single lookup
    Properties
    {
        _Color				("Color",					Color)				= (1,1,1,1)				// Usual color option
        _MainTex			("Albedo (RGB)",			2D)					= "white" {}			// Main texture for the sail
        _EmblemTex			("Emblem (RGB)",			2D)					= "black" {}			// Emblem overlay
        _EmblemColor		("Emblem Color",			Color)				= (1,1,1,1)				// Color for emblem
		[HDR]_EmblemEmColor	("Emblem Emissive Color",	Color)				= (0.0, 0.0, 0.0, 0.0)	// emissive color for emblem
		_EmblemBackface		("Emblem Backface",			Range(0, 1))		= 0						// emblem applied to back of sail	
        _DamageTex			("Damage (RGB)",			2D)					= "white" {}			// Damage works through a sequnce of 3 textures for progressive damage
        _DamageTex1			("Damage1 (RGB)",			2D)					= "white" {}
        _DamageTex2			("Damage2 (RGB)",			2D)					= "white" {}
		_BumpMap			("Normal (Normal)",			2D)					= "bump" {}				// usual bump map
        _SpecColor			("Specular Color",			Color)				= (0.5,0.5,0.5,1)		// Usual specular color option
		_Glossiness			("Smoothness",				Range(0.001,1))		= 0.5					// specular smoothness value
		_Cutoff				("Alpha cutoff",			Range(0, 1))		= 0.5					// cutoff value for transparency
		_Speed				("Speed",					Range(0, 10))		= 1						// Wind strength
		_WindDir			("Wind Dir",				Vector)				= (0,0,1,0)				// wind direction vector
		_SailForward		("Sail Forward",			Vector)				= (0,0,1,0)				// local space forward vector for the sail
		_MaskMap			("Mask (RGB)",				2D)					= "white" {}			// mask texure used to control the amount of sail movement, white is full movement, black is none (red channel only is used)
		_MaskMapRev			("Mask Rev (RGB)",			2D)					= "white" {}			// mask texure used to control the amount of sail movement when wind is against the sail, white is full movement, black is none (red channel only is used)
		_SailWind			("Sail Wind",				Range(0, 1))		= 1						// How the wind fills the sail, 0 will be no movement for wind 1 will be full movement
		_FillPercent		("Fill Percent",			Range(0, 1))		= 1						// similar to above value, works together with the above value
		_SailSideways		("Sail Sideways",			Range(0, 1))		= 0						// how much the sail moves in sideways wind
		_SailLift			("Sail Lift",				Range(0, 1))		= 0						// controls how much the bottom of the sail lifts when the sail is full of wind
		_SailLiftArch		("Sail Lift Arch",			Range(0, 2))		= 0						// amount of arch added to sail bottom as sail fills with wind
		_SailLiftSideArch	("Sail Lift Side Arch",		Range(-2, 2))		= 0						// amount of arch added to sail sides as sail fills with wind
		_SailReverse		("Sail Reverse",			Range(0, 1))		= 0.2					// controls how much movement of the sail there is if the wind is head on
		_SailTaper			("Sail Taper",				Range(-4, 4))		= 0						// can be used to taper the sail
		_SailShear			("Sail Shear",				Range(-10, 10))		= 0						// can be used to shear the bottom of the sail
		_SailTilt			("Sail Tilt",				Range(-10, 10))		= 0						// tilts the bottom of the sail
		_SailTiltTop		("Sail Tilt Top",			Range(-10, 10))		= 0						// tilts the top of the sail
		_SailArch			("Sail Arch",				Range(-4, 4))		= 0						// adds an arch to the bottom of the sail
		_SailArchStart		("Sail Arch Start",			Range(0, 1))		= 0.5					// point down the sail arch starts to appear
		_SailSideArch		("Sail Side Arch",			Range(-4, 4))		= 0						// adds an arch to sides of the sail
		_SailTopArch		("Sail Top Arch",			Range(-4, 4))		= 0						// adds an arch to the top of the sail
		_FullRipple			("Full Ripple Factor",		Range(0, 1))		= 0.9					// controls how the ripple reduces as the sail fills with wind, 1 means no ripple when sail is full
		_Furl				("Furl",					Range(0, 2))		= 0						// how furled the sail is 0 - 2
		_FurledRadius		("Furl Radius",				Range(0, 4))		= 1						// radius of the furled sail, gets multiplied by the furledmap r value
		_Damage				("Damage",					Range(0, 1))		= 0						// damage value 0 is no damage 1 is full damage, lerps between the damage textures
		_VorStrength		("Vor Strength",			Range(0, 5))		= 1						// how much rippling there is
		_VorSpeed			("Vor Speed",				Range(0, 10))		= 1						// how fast is the rippling
		_VorScale			("Vor Scale",				Range(0, 10))		= 1						// size of the cells, controls the size of the ripples
		_VorSmoothness		("Vor Smooth",				Range(0, 10))		= 1						// smooths out the noise
		_VorSeed			("Vor Seed",				Range(0, 10))		= 1						// an offset value, each sail should have a different value so they dont all animate the same way
		// translucent stuff
		_Thickness			("Thickness (R)",			2D)					= "white" {}			// controls how thick the sail is for light to shine through
		_Power				("Subsurface Power",		Range(0.001, 10))	= 1.0					// controls fall off of the effect
		_Distortion			("Subsurface Distortion",	Float)				= 0.0					// normal translucency distortion value
		_Scale				("Subsurface Scale",		Float)				= 0.5
		_SubColor			("Subsurface Color",		Color)				= (1.0, 1.0, 1.0, 1.0)	// controls the tint color of light through the sail
		// Impacts
		_ImpactTex			("Impact Tex",				2D)					= "white" {}			// The texture containing the damage decal textures
		_ImpactCount		("Impact Count",			Range(0, 16))		= 0						// The number of imapcts to show on the sail
		_ImpactRipple		("Impact Ripple",			2D)					= "gray" {}				// The texture that controls the ripple effect
		_ImpactLocation		("Impact Location",			Vector)				= (0,0,10000,0)			// The location (XY) of the ripple the start time(Z) and the size(W)
		_ImpactCutoff		("Impact Alpha Cutoff",		Range(0.0, 1))		= 0.16					// alpha cutoff for impact effects

		_RippleNoise		("Ripple Noise",			2D)					= "white" {}			// The texture containing the damage decal textures
	}

    SubShader
    {
		Tags { "RenderType" = "TransparentCutout" }

        LOD 200

        CGPROGRAM
        #pragma surface surf Translucent fullforwardshadows vertex:vert addshadow
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex	: TEXCOORD0;
			float3 custnormal;
        };

        half		_Glossiness;
        fixed4		_Color;
        fixed4		_EmblemColor;
		float		_EmblemBackface;
		float4		_EmblemEmColor;
		float		_Cutoff;
        sampler2D	_MainTex;
        sampler2D	_DamageTex;
        sampler2D	_DamageTex1;
        sampler2D	_DamageTex2;
		sampler2D	_BumpMap;
		sampler2D	_EmblemTex;
		float4		_WindDir;
		float4		_SailForward;
		sampler2D	_MaskMap;
		sampler2D	_MaskMapRev;
		float		_SailWind;
		float		_FillPercent;
		float4		_MaskMap_ST;
		float4		_FurlMap_ST;
		float4		_FurledMap_ST;
		float4		_EmblemTex_ST;
		float		_VorStrength;
		float		_VorSpeed;
		float		_VorScale;
		float		_VorSmoothness;
		float		_Furl;
		float		_FurledRadius;
		//sampler2D	_FurlMap;
		//sampler2D	_FurledMap;
		float		_FullRipple;
		float		_SailLift;
		float		_SailLiftArch;
		float		_SailLiftSideArch;
		float		_Speed;
		float		_SailReverse;
		float		_SailTaper;
		float		_SailShear;
		float		_SailTilt;
		float		_SailTiltTop;
		float		_SailArch;
		float		_SailArchStart;
		float		_SailSideArch;
		float		_SailTopArch;
		sampler2D	_Thickness;
		float		_Power;
		float		_Distortion;
		float		_Scale;
		fixed4		_SubColor;
		float		_SailSideways;

		float		_ImpactCount;
		sampler2D	_ImpactTex;
		float4		_ImpactPoints[64];
		sampler2D	_ImpactRipple;
		float4		_ImpactLocation;
		float		_ImpactCutoff;

		sampler2D	_RippleNoise;
		float		_VorSeed;
		float		_YScale;

		static const float pi	= 3.141592653589793238462;
        static const float pi2	= 6.283185307179586476924;
		static const float hpi	= 3.141592653589793238462 * 0.5;

        UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _Damage)
        UNITY_INSTANCING_BUFFER_END(Props)

		#include "SailFuncs.cginc"

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_SETUP_INSTANCE_ID(v);

			//float2 uv_Furl = v.texcoord1.xy * _FurlMap_ST.xy + _FurlMap_ST.zw;
			float4 mask = tex2Dlod(_MaskMap, float4(v.texcoord1.xy, 0, 0));
			float3 defamt = Deform(v.texcoord1.xy, mask);
			float ft = 1.0;
			
			float f = mask.g;	//tex2Dlod(_FurlMap, float4(uv_Furl, 0, 0)).r;

			float rf = _Furl;
			float fa = 0;

			float xo = 1.0 - v.texcoord1.y;
			float tilt = lerp(_SailTiltTop, _SailTilt, xo);

			if ( f < rf )
			{
				ft = clamp(1.0 - (rf - f), 0, 1);

				if ( rf > 1 )
					fa = rf - 1;

				float3 furledvert;
				//float frad = _FurledRadius * tex2Dlod(_FurledMap, float4(uv_Furl.x * _FurledMap_ST.x + _FurledMap_ST.z, _FurledMap_ST.y * _FurledMap_ST.y + _FurledMap_ST.w, 0, 0)).r;
				float frad = _FurledRadius * mask.b;

				furledvert.z = (sin(pi2 - (pi2 * v.texcoord1.y)) * frad);
				furledvert.y = (cos(pi2 - (pi2 * v.texcoord1.y)) * frad) - frad;
				furledvert.x = v.vertex.x;
				furledvert.z *= _YScale;

				furledvert.y += _SailTiltTop * ((v.texcoord1.x * 2) - 1);

				float3 vert = v.vertex.xyz + (defamt * (ft * ft));

				vert.y += tilt * ((v.texcoord1.x * 2) - 1);	// * xo;	//v.texcoord1.y;
				vert.y = lerp(furledvert.y, vert.y, ft);
				vert.x *= 1.0 + (_SailTaper * xo) * ft;
				vert.x += xo * ft * _SailShear;

				v.vertex.xyz = lerp(vert, furledvert, fa);
			}
			else
			{
				float3 vert = v.vertex.xyz + (defamt * (ft * ft));

				vert.y += tilt * ((v.texcoord1.x * 2) - 1);	// * xo;	//v.texcoord1.y;
				vert.x *= 1.0 + (_SailTaper * xo) * ft;
				vert.x += xo * ft * _SailShear;

				v.vertex.xyz = vert.xyz;
			}

			o.uv_MainTex = v.texcoord1;
			o.custnormal = v.normal;
		}

        void surf(Input IN, inout SurfaceOutput o)
        {
			float dam = UNITY_ACCESS_INSTANCED_PROP(Props, _Damage);
            fixed4 c;

			if ( dam > 0 )
			{
				if ( dam < 0.333 )
					c = lerp(tex2D(_MainTex, IN.uv_MainTex), tex2D(_DamageTex, IN.uv_MainTex), dam / 0.333);
				else
				{
					if ( dam < 0.666)
						c = lerp(tex2D(_DamageTex, IN.uv_MainTex), tex2D(_DamageTex1, IN.uv_MainTex), (dam - 0.333) / 0.333);
					else
					{
						float la = (dam - 0.666) / 0.333;

						la = clamp(la, 0.0, 1.25);
						c = lerp(tex2D(_DamageTex1, IN.uv_MainTex), tex2D(_DamageTex2, IN.uv_MainTex), la);
					}
				}
			}
			else
				c = tex2D(_MainTex, IN.uv_MainTex);

			clip(c.a - _Cutoff);

			float dot1 = dot(IN.custnormal.xyz, _SailForward.xyz);

			float2 euv = IN.uv_MainTex;
			//euv.x = 1.0 - euv.x;

			fixed4 emblem = tex2D(_EmblemTex, euv * _EmblemTex_ST.xy + _EmblemTex_ST.zw);
			c.rgb *= _Color.rgb;

			if ( emblem.a > 0 )
			{
				float ea = emblem.a * _EmblemColor.a;
				if ( dot1 >= 0 )
					c.rgb = lerp(c, emblem.rgb * _EmblemColor.rgb, ea) + (_EmblemEmColor * emblem.r * ea);
				else
					c.rgb = lerp(c, emblem.rgb * _EmblemColor.rgb, ea * _EmblemBackface) + (_EmblemEmColor * emblem.r * _EmblemBackface * ea);
			}

						// Impacts
			for	( int i = 0; i < _ImpactCount; i++ )
			{
				float2 delta = IN.uv_MainTex.xy - _ImpactPoints[i].xy;

				float w = _ImpactPoints[i].z;
				float w2 = (w * 2) * 8;
				float w2v = (w * 2) * 2;

				if ( delta.x > -w && delta.x < w )
				{
					if ( delta.y > -w && delta.y < w )
					{
						float au		= floor(_ImpactPoints[i].w);
						float av		= floor(_ImpactPoints[i].w / 8);
						float2 tileuv	= float2(au * 0.125, av * 0.5);
						float2 uv		= float2((delta.x + w) / w2, (delta.y + w) / w2v);
						fixed4 hole		= tex2D(_ImpactTex, tileuv + uv);
						clip(hole.a - _ImpactCutoff);
						c.rgb = lerp(hole.rgb, c.rgb, (hole.a - _ImpactCutoff) / (1 - _ImpactCutoff));
					}
				}
			}

			float thick = tex2D(_Thickness, IN.uv_MainTex).r;

			o.Albedo = c.rgb;	// * _Color;
			o.Alpha = c.a * thick;
			o.Gloss = _Glossiness;
			o.Specular = _Glossiness * 4;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
        }

		inline fixed4 LightingTranslucent(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{	
			// You can remove these two lines, to save some instructions. They're just here for visual fidelity.
			//viewDir = normalize(viewDir);
			//lightDir = normalize(lightDir);

			// Translucency.
			half3 transLightDir	= lightDir + s.Normal * _Distortion;
			fixed3 transAlbedo;
			if ( dot(lightDir, s.Normal * _Distortion) < 0 )
			{
				float d = dot(viewDir, -transLightDir);
				float transDot		= pow(max(0, d), _Power) * _Scale;
				fixed3 transLight	= (atten * 2) * (transDot) * s.Alpha * _SubColor.rgb;
				transAlbedo	= s.Albedo * _LightColor0.rgb * transLight;
			}
			else
				transAlbedo = float3(0, 0, 0);
				
			// Regular BlinnPhong.
			half3 h				= normalize(lightDir + viewDir);
			fixed diff			= max(0, dot(s.Normal, lightDir));
			float nh			= max(0, dot(s.Normal, h));

			float spec			= pow(nh, s.Specular * 128.0) * s.Gloss;
			fixed3 diffAlbedo	= (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
			// Add the two together.
			fixed4 c;

			c.rgb = diffAlbedo + transAlbedo;
			c.a = _LightColor0.a * _SpecColor.a * atten;
			return c;
		}

        ENDCG
    }
    FallBack "Diffuse"
	CustomEditor "FullSail.SailShaderOptimizedGUI"
}