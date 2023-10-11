using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Remoting.Contexts;

namespace MatchThree
{
    public class EditorHelpers
    {

        [MenuItem("Bootcamp/Hello")]
        public static void HelloEditor()
        {
            Debug.Log("Hello Bootcamp!");
        }

    }
}
