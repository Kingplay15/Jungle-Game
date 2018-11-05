using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovingItemInfo
{
    public static int ItemNum;
    public static int StackNum;
    public static string ItemTag;
    public static List<int[]> ItemSpecials;
    public static int[] ItemRequirements;
    public static bool IsContainingEquippedItem = false;
}
