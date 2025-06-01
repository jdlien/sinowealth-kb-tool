using System.IO;
using Mono.Cecil;

namespace WindControls;

public class CustomResolver : BaseAssemblyResolver
{
	private DefaultAssemblyResolver _defaultResolver;

	public CustomResolver()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		_defaultResolver = new DefaultAssemblyResolver();
	}

	public override AssemblyDefinition Resolve(AssemblyNameReference name)
	{
		//IL_0013: Expected O, but got Unknown
		try
		{
			return ((BaseAssemblyResolver)_defaultResolver).Resolve(name);
		}
		catch (AssemblyResolutionException ex)
		{
			AssemblyResolutionException ex2 = ex;
			if (name.Name.Equals("Windows.Devices"))
			{
				Stream resourceStream = FileHelper.GetResourceStream("MakeUpgradeTool.Files.Windows.Devices.winmd");
				return AssemblyDefinition.ReadAssembly(resourceStream);
			}
			return ((BaseAssemblyResolver)_defaultResolver).Resolve(name);
		}
	}
}
