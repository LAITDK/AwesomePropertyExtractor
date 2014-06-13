using System;
namespace APE.Interfaces
{
	public interface IProperty
	{
		string Alias { get; set; }
		TProp As<TProp>() where TProp : IProperty, new();
	}
}
