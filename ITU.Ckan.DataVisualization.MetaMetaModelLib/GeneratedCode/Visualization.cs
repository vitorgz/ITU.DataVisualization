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

public class Visualization : NamedElement, IPropertable
{
	public virtual int VisualizationId
	{
		get;
		set;
	}

	public virtual Table table
	{
		get;
		set;
	}

	public virtual ICollection<Source> sources
	{
		get;
		set;
	}

	public virtual Graph graph
	{
		get;
		set;
	}

    public ICollection<Property> properties
    {
        get; set;
    }
}

