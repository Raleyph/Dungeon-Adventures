using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using UnityEngine;
using UnityEditor;

public class Items : MonoBehaviour {
   public enum itemTypes { Weapon, Potion, Amulet }

   [Header("Main Settings")]
   public itemTypes ItemTypes;
   [HideInInspector] public Sprite Icon;
   [HideInInspector] public string itemName;
   [HideInInspector] public int itemNum;

   // Weapon
   [HideInInspector] public float damage;
   [HideInInspector] public int broken;

   // Potion
   public enum potionType { Health, Poison, Strong } 
   [HideInInspector] public potionType Potion = potionType.Health;

   [HideInInspector] public float healthOfset;
   [HideInInspector] public bool processingEffect;

   // Amulet
   public enum amuletType { Health, Defense, Speed }
   [HideInInspector] public amuletType Amulet = amuletType.Health; 
   [HideInInspector] public float amuletValue;
    
   [CustomEditor(typeof(Items))]
   public class ItemsEditor : Editor {
      public override void OnInspectorGUI() {
         base.OnInspectorGUI();
         Items items = (Items) target;
         DrawMain(items);
         EditorGUILayout.Space();
      
         switch (items.ItemTypes) {
            case itemTypes.Weapon:
               WeaponSettings();
               break;
            case itemTypes.Potion:
               PotionSettings();
               break;
            case itemTypes.Amulet:
               AmuletSettings();
               break;
         }
         serializedObject.ApplyModifiedProperties();
      }

      void DrawMain(Items items) {
         EditorGUILayout.PropertyField(serializedObject.FindProperty("Icon"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("itemNum"));
         EditorGUILayout.Space();
      }

      void WeaponSettings() {
         EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("broken"));
      }

      void PotionSettings() {
         EditorGUILayout.PropertyField(serializedObject.FindProperty("Potion"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("healthOfset"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("processingEffect"));
      }

      void AmuletSettings() {
         EditorGUILayout.PropertyField(serializedObject.FindProperty("Amulet"));
         EditorGUILayout.PropertyField(serializedObject.FindProperty("amuletValue"));
      }
   }
}
