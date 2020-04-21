using UnityEngine;

namespace UnityEditor.Tilemaps
{
    /// <summary>
    /// This Brush instances and places a randomly selected Prefabs onto the targeted location and parents the instanced object to the paint target. Use this as an example to quickly place an assorted type of GameObjects onto structured locations.
    /// </summary>
    [CreateAssetMenu(fileName = "Prefab Random brush", menuName = "Brushes/Prefab Random brush")]
    [CustomGridBrush(false, true, false, "Prefab Random Brush")]
    public class PrefabRandomBrush : BasePrefabBrush
    {
        private const float k_PerlinOffset = 100000f;
        /// <summary>
        /// The selection of Prefabs to paint from
        /// </summary>
        public GameObject[] m_Prefabs;

        /// <summary>
        /// Paints Prefabs into a given position within the selected layers.
        /// The PrefabBrush overrides this to provide Prefab painting functionality.
        /// </summary>
        /// <param name="grid">Grid used for layout.</param>
        /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject.</param>
        /// <param name="position">The coordinates of the cell to paint data to.</param>
        /// 
        public bool avoidRepeat2Last = true;
        int last = -1;
        int secondLast = -1;

        public bool randomScale = true;

        public float minScale = 1;
        public float maxScale = 2;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            int index = -1;

            uint iterations = 0;
            do
            {
                iterations++;
                index = Random.Range(0, m_Prefabs.Length - 1); //Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
                if (iterations > 1000)
                    return;
            
            } while (index == last || index == secondLast);
          
            secondLast = last;
            last = index;

            Prefab = m_Prefabs[index];

            scaleFactor = 1;            
            if(randomScale)
                scaleFactor = Random.Range(minScale, maxScale);

            base.Paint(grid, brushTarget, position);


        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }
    }

    /// <summary>
    /// The Brush Editor for a Prefab Brush.
    /// </summary>
    [CustomEditor(typeof(PrefabRandomBrush))]
    public class PrefabRandomBrushEditor : BasePrefabBrushEditor
    {
        private PrefabRandomBrush prefabBrush { get { return target as PrefabRandomBrush; } }

        private SerializedProperty m_Prefabs;
        private SerializedProperty avoidRepeat2Last;
        private SerializedProperty randomScale;
        private SerializedProperty minScale;
        private SerializedProperty maxScale;


        protected override void OnEnable()
        {
            base.OnEnable();
            m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
            avoidRepeat2Last = m_SerializedObject.FindProperty("avoidRepeat2Last");
            randomScale = m_SerializedObject.FindProperty("randomScale");
            minScale = m_SerializedObject.FindProperty("minScale");
            maxScale = m_SerializedObject.FindProperty("maxScale");

        }

        /// <summary>
        /// Callback for painting the inspector GUI for the PrefabBrush in the Tile Palette.
        /// The PrefabBrush Editor overrides this to have a custom inspector for this Brush.
        /// </summary>
        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
            m_SerializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_Prefabs, true);
            EditorGUILayout.PropertyField(avoidRepeat2Last, true);
            EditorGUILayout.PropertyField(randomScale, true);

            if(randomScale.boolValue == true)
            {
                EditorGUILayout.PropertyField(minScale, true);
                EditorGUILayout.PropertyField(maxScale, true);
            }

            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}