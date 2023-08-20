using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    DatabaseSingleton dbSingleton;

    void Start()
    {
        dbSingleton = DatabaseSingleton.Instance;

        // Initialize Firebase
        if (dbSingleton.IsFirebaseInitialized() == false)
        {
            dbSingleton.InitializeFirebase();
        }
    }
}
