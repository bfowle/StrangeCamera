using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class NunitTestRunner {

    [MenuItem("Unit Tests/Run Unit Tests %#r")]
    public static void RunAllTests() {
        NUnitLiteUnityRunner.RunTests();
    }

}

#else
public class NunitTestRunner {
    public static void RunAllTests() {
    }
}
#endif
