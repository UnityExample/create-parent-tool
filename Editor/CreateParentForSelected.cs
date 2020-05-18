using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.EditorTools
{
    public class CreateParentForSelected
    {
        [MenuItem("Edit/Create Parent for Selected %g")]
        public static void CreateParent() {

            //get all selected transforms
            var transforms = new List<Transform>(UnityEditor.Selection.transforms);

            //nothing selected, don't do anything
            if (transforms.Count <= 0) return;

            //create a new zeroed gameObject
            var parentObject = new GameObject("New Parent");
            Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent for Selected");

            transforms.Sort((a, b) => { return a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()); });

            //if there is a shared parent, create the parent underneath it, and not in the root
            var topMostParent = transforms[0].parent;

            //search for the shared parent
            foreach(var t in transforms) { 
                if (t.parent != topMostParent) topMostParent = null;
	        }

            //move our new parent to the shared parent
            parentObject.transform.SetParent(topMostParent, false);

            //move our children to our new parent
            foreach(var t in transforms) {
                Undo.SetTransformParent(t, parentObject.transform, "Create Parent for Selected");
	        }

            UnityEditor.Selection.activeGameObject = parentObject;
	    }
    }
}
