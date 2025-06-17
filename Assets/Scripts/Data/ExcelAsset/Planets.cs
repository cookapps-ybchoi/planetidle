using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/DB")]
public class Planets : ScriptableObject
{
	public List<PlanetEntity> Entities;
}
