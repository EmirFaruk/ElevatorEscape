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
        {KeyType.Cyan, Color.cyan },
        {KeyType.Lime, new Color(0.5f, 1, 0.5f) },
        {KeyType.Maroon, new Color(0.5f, 0, 0) },
        {KeyType.Olive, new Color(0.5f, 0.5f, 0) },
        {KeyType.Teal, new Color(0, 0.5f, 0.5f) },
        {KeyType.Navy, new Color(0, 0, 0.5f) },
        {KeyType.Beige, new Color(0.5f, 0.5f, 0.25f) },
        {KeyType.Peach, new Color(1, 0.5f, 0.25f) },
        {KeyType.Lavender, new Color(0.5f, 0.25f, 0.5f) },
        {KeyType.Coral, new Color(1, 0.5f, 0.25f) },
        {KeyType.Mint, new Color(0.5f, 1, 0.5f) },
        {KeyType.Indigo, new Color(0.25f, 0, 0.5f) },
        {KeyType.Amber, new Color(1, 0.75f, 0) },
        {KeyType.Azure, new Color(0, 0.5f, 1) },
        {KeyType.Lilac, new Color(0.5f, 0, 0.5f) },
        {KeyType.Gold, new Color(1, 0.85f, 0) },
        {KeyType.Silver, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Bronze, new Color(0.8f, 0.5f, 0.2f) },
        {KeyType.Platinum, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Diamond, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Ruby, new Color(0.75f, 0.25f, 0.25f) },
        {KeyType.Sapphire, new Color(0.25f, 0.25f, 0.75f) },
        {KeyType.Emerald, new Color(0.25f, 0.75f, 0.25f) },
        {KeyType.Topaz, new Color(0.75f, 0.75f, 0.25f) },
        {KeyType.Amethyst, new Color(0.5f, 0, 0.5f) },
        {KeyType.Opal, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Pearl, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Garnet, new Color(0.75f, 0.25f, 0.25f) },
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
        Cyan,
        Lime,
        Maroon,
        Olive,
        Teal,
        Navy,
        Beige,
        Peach,
        Lavender,
        Coral,
        Mint,
        Indigo,
        Amber,
        Azure,
        Lilac,
        Gold,
        Silver,
        Bronze,
        Platinum,
        Diamond,
        Ruby,
        Sapphire,
        Emerald,
        Topaz,
        Amethyst,
        Opal,
        Pearl,
        Garnet,
        Agate,
        Aquamarine,
        Peridot,
        Citrine,
        Onyx,
        Jasper,
        Malachite,
        LapisLazuli,
        Turquoise,
        Moonstone,
        Sunstone,
        Bloodstone,
    }
}