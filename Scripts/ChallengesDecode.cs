using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChallengesDecode {
    public static List<string> TurnIntoList (string str) {
        string[] array = str.Split (',');
        List<string> list = new List<string> ();
        for (int i = 0; i < array.Length; i++) {
            ////Debug.log(array[i]);
            list.Add (array[i]);
        }
        return list;
    }
    public static string TurnIntoString (List<string> list) {
        string str = list[0];
        for (int i = 1; i < list.Count; i++) {
            str = str + "," + list[i];
        }
        return str;
    }
    public static List<int> TurnIntoIntList (List<string> list) {
        List<int> intList = new List<int> ();
        for (int i = 0; i < list.Count; i++) {
            intList.Add (int.Parse (list[i]));
        }
        return intList;
    }
}