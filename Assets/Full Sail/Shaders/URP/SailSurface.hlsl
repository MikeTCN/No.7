        
		float4 _ImpactPoints[64];

		void SailSurface_float(float2 uv_MainTex, float3 custnormal, out float4 c)
        {
			float dam = _Damage; 

            //fixed4 c;

			if ( dam > 0 )
			{
				if ( dam < 0.333 )
					c = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_MainTex), SAMPLE_TEXTURE2D(_DamageTex, sampler_DamageTex, uv_MainTex), dam / 0.333);
				else
				{
					if ( dam < 0.666)
						c = lerp(SAMPLE_TEXTURE2D(_DamageTex, sampler_DamageTex, uv_MainTex), SAMPLE_TEXTURE2D(_DamageTex1, sampler_DamageTex1, uv_MainTex), (dam - 0.333) / 0.333);
					else
					{
						float la = (dam - 0.666) / 0.333;

						la = clamp(la, 0.0, 1.25);
						c = lerp(SAMPLE_TEXTURE2D(_DamageTex1, sampler_DamageTex1, uv_MainTex), SAMPLE_TEXTURE2D(_DamageTex2, sampler_DamageTex2, uv_MainTex), la);
					}
				}
			}
			else
				c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_MainTex);

			clip(c.a - _Cutoff);

			float dot1 = dot(custnormal.xyz, _SailForward.xyz);

			float2 euv = uv_MainTex;
			//euv.x = 1.0 - euv.x;

			float4 emblem = SAMPLE_TEXTURE2D(_EmblemTex, sampler_EmblemTex, euv * _EmblemTex_ST.xy + _EmblemTex_ST.zw); 
			c.rgb *= _Color.rgb;

			if ( emblem.a > 0 )
			{
				float ea = emblem.a * _EmblemColor.a;
				if ( dot1 >= 0 )
					c.rgb = lerp(c.rgb, emblem.rgb * _EmblemColor.rgb, ea) + (_EmblemEmColor.rgb * emblem.r * ea);
				else
				{
					ea *= _EmblemBackface;
					c.rgb = lerp(c.rgb, emblem.rgb * _EmblemColor.rgb, ea) + (_EmblemEmColor.rgb * emblem.r * ea);
				}
			}
	
			// Impacts
			for (int i = 0; i < _ImpactCount; i++)
			{
				float2 delta = uv_MainTex.xy - _ImpactPoints[i].xy;

				float w = _ImpactPoints[i].z;
				float w2 = (w * 2) * 8;
				float w2v = (w * 2) * 2;

				if (delta.x > -w && delta.x < w)
				{
					if (delta.y > -w && delta.y < w)
					{
						float au = floor(_ImpactPoints[i].w);
						float av = floor(_ImpactPoints[i].w / 8);
						float2 tileuv = float2(au * 0.125, av * 0.5);
						float2 uv = float2((delta.x + w) / w2, (delta.y + w) / w2v);
						float4 hole = SAMPLE_TEXTURE2D(_ImpactTex, sampler_ImpactTex, tileuv + uv);
						clip(hole.a - _ImpactCutoff);
						c.rgb = lerp(hole.rgb, c.rgb, (hole.a - _ImpactCutoff) / (1 - _ImpactCutoff));
					}
				}
			}
	
			//float thick = tex2D(_Thickness, uv_MainTex).r;

			//o.Albedo = c.rgb;	// * _Color;
			//o.Alpha = c.a;// * thick;
			//o.Gloss = _Glossiness;
			//o.Normal = UnpackNormal(tex2D(_BumpMap, uv_MainTex));
        }