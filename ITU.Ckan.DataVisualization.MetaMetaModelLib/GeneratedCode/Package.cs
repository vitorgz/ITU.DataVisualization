﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Package : NamedElement, ICkanBuilder, IEntity
{
	public virtual IEnumerable<Property> properties
	{
		get;
		set;
	}

	public virtual string author
	{
		get;
		set;
	}

	public virtual object state
	{
		get;
		set;
	}

	public virtual bool selected
	{
		get;
		set;
	}

	public virtual IEnumerable<DataSet> dataSets
	{
		get;
		set;
	}

}

