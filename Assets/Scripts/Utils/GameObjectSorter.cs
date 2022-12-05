using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSorter : IComparer {
    int IComparer.Compare(System.Object x, System.Object y) {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }
}
