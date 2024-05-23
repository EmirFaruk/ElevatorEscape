using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyData : MonoBehaviour
{
    #region Editor Code
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (KeyMaterials.Length != Enum.GetNames(typeof(KeyType)).Length)
        {
            KeyMaterials = new KeyMaterialStruct[Enum.GetNames(typeof(KeyType)).Length];
            print("Asigned Key Materials Lenght");
        }
        if (KeyMaterials[0].name != ((KeyType)0).ToString())
            for (int i = 0; i < Enum.GetNames(typeof(KeyType)).Length; i++)
                KeyMaterials[i].name = ((KeyType)i).ToString();
    }
#endif
    #endregion

    public static KeyData Instance => instance;
    private static KeyData instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public KeyMaterialStruct[] KeyMaterials = new KeyMaterialStruct[Enum.GetNames(typeof(KeyType)).Length];
    [Serializable]
    public struct KeyMaterialStruct
    {
        public string name;
        public Material[] materials;
    }

    //public Material[] KeyMaterials = new Material[Enum.GetNames(typeof(KeyType)).Length];
    public static Dictionary<KeyType, Color> KeyColors = new Dictionary<KeyType, Color>()
    {
        {KeyType.Red, Color.red },
        {KeyType.Green, Color.green },
        {KeyType.Blue, Color.blue },
        {KeyType.Purple, new Color(0.5f, 0, 0.5f) },
        {KeyType.Yellow, Color.yellow },
        {KeyType.Orange, new Color(1, 0.5f, 0) },
        {KeyType.White, Color.white },
        {KeyType.Black, Color.black },
        {KeyType.Brown, new Color(0.5f, 0.25f, 0) },
        {KeyType.Pink, new Color(1, 0.5f, 0.5f) },

    };

    public enum KeyType
    {
        Red,
        Blue,
        Green,
        Purple,
        Yellow,
        Orange,
        White,
        Black,
        Brown,
        Pink,

    }
}