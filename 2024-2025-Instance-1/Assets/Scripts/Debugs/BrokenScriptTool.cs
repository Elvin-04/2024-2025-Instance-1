using UnityEditor;
using UnityEngine;

namespace Debugs
{
    public class BrokenScriptTool : MonoBehaviour
    {
        private bool _found;

        [ContextMenu("Find Broken Scripts")]
        private void FindBrokenScripts()
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        Debug.LogError($"Missing script found on GameObject: {obj.name}", obj);
                        _found = true;
                    }
                    else
                    {
                        SerializedObject so = new(component);
                        SerializedProperty prop = so.GetIterator();
                        while (prop.NextVisible(true))
                        {
                            if (prop.propertyType == SerializedPropertyType.ObjectReference &&
                                prop.objectReferenceValue == null && prop.objectReferenceInstanceIDValue != 0)
                            {
                                Debug.LogError(
                                    $"Broken reference found in component: {component.GetType().Name} on GameObject: {obj.name}",
                                    obj);
                                _found = true;
                            }
                        }
                    }
                }
            }

            if (!_found)
            {
                Debug.Log("No missing scripts or broken references found.");
            }
        }
    }
}