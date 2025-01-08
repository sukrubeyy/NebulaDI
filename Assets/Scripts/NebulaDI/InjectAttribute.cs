using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectAttribute : Attribute
{
}
