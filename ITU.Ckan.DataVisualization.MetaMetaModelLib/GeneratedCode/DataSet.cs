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

public class DataSet : NamedElement, ICkanBuilder, IEntity
{
	public virtual int limit
	{
		get;
		set;
	}

	public virtual string format
	{
		get;
		set;
	}

	public virtual string package_id
	{
		get;
		set;
	}

	public virtual bool selected
	{
		get;
		set;
	}

	public virtual IEnumerable<Property> properties
	{
		get;
		set;
	}

	public virtual string id
	{
		get;
		set;
	}

	public virtual int total
	{
		get;
		set;
	}

	public virtual string resource_id
	{
		get;
		set;
	}

	public virtual IEnumerable<Field> fields
	{
		get;
		set;
	}

	public virtual IEnumerable<Group> groups
	{
		get;
		set;
	}

	public virtual Organization organization
	{
		get;
		set;
	}

	public virtual IEnumerable<Tag> tags
	{
		get;
		set;
	}

	public virtual IEnumerable<Record> records
	{
		get;
		set;
	}

}

