#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEditor;
using Egamea.ConjureScape.CustomLODSystem;

namespace Egamea.ConjureScape
{
	[AddComponentMenu("ConjureScape/Add Terrain Optimizer")]
	public class TerrainOptimizer : MonoBehaviour
	{
		/// <summary>
		/// The range of the terrain detail set by the user.
		/// </summary>
		[Range(0, 10)] 
		public int TerrainDetail = 10;
		/// <summary>
		/// The current active terrain.
		/// </summary>
		private Terrain _currentTerrain;
		/// <summary>
		/// Reference to the LevelOfDetailDistancesScript.
		/// </summary>
		private LevelOfDetailDistances _levelOfDetailDistancesScript;
		
		/// <summary>
		/// Apply the wanted terrain detail level.
		/// </summary>
		public void ApplyTerrainDetail(int wantedID)
		{
			_currentTerrain = Terrain.activeTerrain;
			_levelOfDetailDistancesScript = GameObject.FindObjectOfType<LevelOfDetailDistances>();
			
			if (_currentTerrain)
			{
				switch (wantedID)
				{
					case 1:
						SetTerrainQualitySettings(_currentTerrain,11.0f,20.0f,0.1f,0);
						break;
					case 2:
						SetTerrainQualitySettings(_currentTerrain,10.0f,30.0f,0.2f,1);
						break;
					case 3:
						SetTerrainQualitySettings(_currentTerrain,8.0f,40.0f,0.3f,2);
						break;
					case 4:
						SetTerrainQualitySettings(_currentTerrain,8.0f,50.0f,0.4f,3);
						break;
					case 5:
						SetTerrainQualitySettings(_currentTerrain,7.0f,60.0f,0.5f,4);
						break;
					case 6:
						SetTerrainQualitySettings(_currentTerrain,6.0f,70.0f,0.6f,5);
						break;
					case 7:
						SetTerrainQualitySettings(_currentTerrain,5.0f,80.0f,0.7f,6);
						break;
					case 8:
						SetTerrainQualitySettings(_currentTerrain,4.0f,90.0f,0.8f,7);
						break;
					case 9:
						SetTerrainQualitySettings(_currentTerrain,3.0f,100.0f,0.9f,8);
						break;
					case 10:
						SetTerrainQualitySettings(_currentTerrain,2.0f,110.0f,1.0f,9);
						break;
				}
				
				//Do this so that all the windows (game view and scene view) updates with the new settings.
				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}
			else
			{
				Debug.LogError("Terrain Component missing! Please insure there is a terrain in the scene.");
			}
		}

		/// <summary>
		/// Method used to set the quality settings for the current terrain being passed. 
		/// </summary>
		/// <param name="terrain"></param>
		/// <param name="pixelError"></param>
		/// <param name="detailDistance"></param>
		/// <param name="detailDensity"></param>
		/// <param name="qualitySettingsLevel"></param>
		private void SetTerrainQualitySettings(Terrain terrain, float pixelError, float detailDistance, float detailDensity, int qualitySettingsLevel)
		{
			terrain.heightmapPixelError = pixelError;
			terrain.detailObjectDistance = detailDistance;
			terrain.detailObjectDensity = detailDensity;
			QualitySettings.SetQualityLevel(qualitySettingsLevel, true);

			//Apply the LevelOfDetailDistances according to the qualitySettingsLevel.
			_levelOfDetailDistancesScript.LOD_0_Distance =
				_levelOfDetailDistancesScript.LevelOfDetailDistancePresets.LOD_DistancesArray[qualitySettingsLevel].LOD_0_Distance;
			
			_levelOfDetailDistancesScript.LOD_1_Distance =
				_levelOfDetailDistancesScript.LevelOfDetailDistancePresets.LOD_DistancesArray[qualitySettingsLevel].LOD_1_Distance;
			
			_levelOfDetailDistancesScript.LOD_2_Distance =
				_levelOfDetailDistancesScript.LevelOfDetailDistancePresets.LOD_DistancesArray[qualitySettingsLevel].LOD_2_Distance;
			
			//Set the LevelOfDetailDistances script to dirty to save the values that was set. 
			EditorUtility.SetDirty(_levelOfDetailDistancesScript);
			
			//Set the reference to the currentQualityLevel on the LevelOfDetailDistances script.
			_levelOfDetailDistancesScript.CurrentQualityLevel = qualitySettingsLevel;
		}
	}
}
#endif