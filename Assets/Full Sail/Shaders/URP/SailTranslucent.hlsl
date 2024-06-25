#ifndef SAIL_TRANSLUCENCY
#define SAIL_TRANSLUCENCY

struct CustomLightData
{
	float3	Normal;
	float4	Albedo;
	float	Alpha;
	float3	viewDir;
	float3	Specular;
	float	Gloss;
};

#ifndef SHADERGRAPH_PREVIEW
float3 CustomLightHandling(CustomLightData s, Light light)
{
	float3 radiance = light.color;
	float3 diffuse = saturate(dot(s.Normal, light.direction));
	float3 color = s.Albedo * radiance * diffuse;

	return color;
}
#endif

inline float3 LightingTranslucent(CustomLightData s)
{
#ifdef SHADERGRAPH_PREVIEW
	float3 lightDir = float3(0.5, 0.5, 0);
	float intensity = saturate(dot(s.Normal, lightDir));
	return s.Albedo * intensity;
#else
	Light mainLight = GetMainLight();

	float3 lightCol = float3(1, 1, 1);	//CustomLightHandling(s, mainLight);
	float3 lightDir = mainLight.direction;
	float atten = mainLight.distanceAttenuation;

	// Translucency.
	half3 transLightDir	= lightDir + s.Normal * _Distortion;
	float3 transAlbedo;
	if ( dot(lightDir, s.Normal * _Distortion) < 0 )
	{
		float d = dot(s.viewDir, transLightDir);
		float transDot		= pow(max(0, d), _Power) * _Scale;
		float3 transLight	= (atten * 2) * (transDot) * s.Alpha * _SubColor.rgb;
		transAlbedo			= s.Albedo * lightCol * transLight;
		//transAlbedo = float3(0, 0, 1); 
	}
	else
		transAlbedo = float3(1, 0, 0); 
		
	// Regular BlinnPhong.
	half3 h				= normalize(lightDir + s.viewDir);
	float diff			= max(0, dot(s.Normal, lightDir));
	float nh			= max(0, dot(s.Normal, h));

	float spec			= pow(nh, s.Specular * 128.0) * s.Gloss;
	//float3 diffAlbedo	= (s.Albedo * lightCol * diff + lightCol * _SpecColor.rgb * spec) * (s.atten * 2);
	float3 diffAlbedo	= (s.Albedo * lightCol * diff) * (atten * 2);
	// Add the two together.
	float4 c;

	c.rgb = diffAlbedo + transAlbedo;
	//c.a = _SpecColor.a * atten;	//_LightColor0.a * _SpecColor.a * s.atten;
	return c;
#endif	
}

void LightingTranslucent_float(float4 Albedo, float3 Normal, float3 ViewDirection, out float3 Color)
{
	CustomLightData d;
	d.Albedo	= Albedo;
	d.Normal	= Normal;
	d.Gloss		= _Glossiness;
	d.Alpha		= 1;
	d.viewDir	= ViewDirection;
	d.Specular	= float3(1, 1, 1);

	Color = LightingTranslucent(d);
}
#endif