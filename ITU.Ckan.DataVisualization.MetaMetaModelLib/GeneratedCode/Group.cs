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

public class Group : NamedElement, IPropertable
{
	public virtual ICollection<Property> properties
	{
		get;
		set;
	}

	public virtual string display_name
	{
		get;
		set;
	}

	public virtual string description
	{
		get;
		set;
	}

	public virtual string title
	{
		get;
		set;
	}

	public virtual int GroupId
	{
		get;
		set;
	}

	public virtual ICollection<DataSet> DataSet
	{
		get;
		set;
	}

}

