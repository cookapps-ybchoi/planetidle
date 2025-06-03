using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/DB")]
public class Enemies : ScriptableObject
{
	public List<EnemyEntity> Entities;
}
