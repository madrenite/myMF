using MFiles.VAF;
using MFiles.VAF.Common;

namespace Banker
{
    /// <summary>
    /// Simple configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Reference to a test class.
        /// </summary>
        [MFClass(Required = false)]
        public MFIdentifier TestClassID = "FailAlias";

        [MFClass(Required = true)]
        public MFIdentifier Kokeiluasteluokka = "myF.CL.Kokeiluasteluokka";

        /// <summary>
        /// Reference to a property definition
        /// </summary>
        [MFPropertyDef(Required = false)]
        public MFIdentifier summaProperty = "myF.PD.Summa";
    }
}
