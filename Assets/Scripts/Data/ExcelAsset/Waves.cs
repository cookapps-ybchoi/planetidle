using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/DB")]
public class Waves : ScriptableObject
{
	public List<WaveEntity> Entities;
}
