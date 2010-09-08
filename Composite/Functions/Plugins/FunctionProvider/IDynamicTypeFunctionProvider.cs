using System.Collections.Generic;


namespace Composite.Functions.Plugins.FunctionProvider
{
    /// <summary>    
    /// </summary>
    /// <exclude />
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)] 
    public interface IDynamicTypeFunctionProvider : IFunctionProvider
	{
        IEnumerable<IFunction> DynamicTypeDependentFunctions { get; }
	}
}
