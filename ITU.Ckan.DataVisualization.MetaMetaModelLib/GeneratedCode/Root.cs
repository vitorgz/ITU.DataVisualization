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

public class Root : ICkanBuilder, IEntity
{
	public virtual int RootId
	{
		get;
		set;
	}

	public virtual ICollection<Visualization> visualizations
	{
		get;
		set;
	}

	public virtual ICollection<Graph> graphs
	{
		get;
		set;
	}

}

