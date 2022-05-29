using System;
using System.IO;
using System.Linq;
using SR = System.Reflection;
using Mono.Cecil;

namespace MixMod
{
	public static class MonoExtensions
	{

		public static MethodDefinition GetMethod(this TypeDefinition self, string name)
		{
			return self.Methods.Where(m => m.Name == name).First();
		}

		public static FieldDefinition GetField(this TypeDefinition self, string name)
		{
			return self.Fields.Where(f => f.Name == name).First();
		}

		public static TypeDefinition ToDefinition(this Type self)
		{
			var module = ModuleDefinition.ReadModule(new MemoryStream(File.ReadAllBytes(self.Module.FullyQualifiedName)));
			return (TypeDefinition)module.LookupToken(self.MetadataToken);
		}

		public static MethodDefinition ToDefinition(this SR.MethodBase method)
		{
			var declaring_type = method.DeclaringType.ToDefinition();
			return (MethodDefinition)declaring_type.Module.LookupToken(method.MetadataToken);
		}

		public static FieldDefinition ToDefinition(this SR.FieldInfo field)
		{
			var declaring_type = field.DeclaringType.ToDefinition();
			return (FieldDefinition)declaring_type.Module.LookupToken(field.MetadataToken);
		}

		public static MethodReference MakeGenericMethod(this MethodReference self, params TypeReference[] arguments)
		{
			if (self.GenericParameters.Count != arguments.Length)
				throw new ArgumentException();

			var instance = new GenericInstanceMethod(self);
			foreach (var argument in arguments)
				instance.GenericArguments.Add(argument);

			return instance;
		}
	}
}
