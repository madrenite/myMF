using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFiles.VAF;
using MFiles.VAF.Common;
using MFilesAPI;

namespace Banker
{
    public class Logging
    {
        public const bool debug = true;
    }

    /// <summary>
    /// Simple vault application to demonstrate VAF.
    /// </summary>
    public partial class VaultApplication : VaultApplicationBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VaultApplication()
        {
            SysUtils.ReportInfoToEventLog($"Constructor was called.");
        }

        /// <summary>
        /// Simple configuration member. MFConfiguration-attribute will cause this member to be loaded from the named value storage from the given namespace and key.
        /// Here we override the default alias of the Configuration class with a default of the config member.
        /// The default will be written in the named value storage, if the value is missing.
        /// Use Named Value Manager to change the configurations in the named value storage.
        /// </summary>
        [MFConfiguration("MFVaultApplication1", "config")]
        private Configuration config = new Configuration() { TestClassID = "TestClassAlias" };

        protected override void InitializeApplication(Vault vault)
        {
            base.InitializeApplication(vault);
        }

        protected override void InstallApplication(Vault vault)
        {
            base.InstallApplication(vault);
        }

        /// <summary>
        /// The method, that is run when the vault goes online.
        /// </summary>
        protected override void StartApplication()
        {
            base.StartApplication();

            // Start writing extension method output to the event log every ten seconds. The background operation will continue until the vault goes offline.
            this.BackgroundOperations.StartRecurringBackgroundOperation("Recurring Hello World Operation", TimeSpan.FromSeconds(300), () =>
             {
                    // Prepare input for the extension method.
                    //string input = "Hello from MFVaultApplication1";

                    // Execute the extension method. Wrapping code to an extension method ensures transactionality for the vault operations.
                    //string output = this.PermanentVault.ExtensionMethodOperations.ExecuteVaultExtensionMethod("TestVaultExtensionMethod", input);

                    // Report extension method output to event log.
                    SysUtils.ReportInfoToEventLog("Hello you beautiful F*cker!");
             });
        }

        /// <summary>
        /// A vault extension method, that will be installed to the vault with the application.
        /// The vault extension method can be called through the API.
        /// </summary>
        /// <param name="env">The event handler environment for the method.</param>
        /// <returns>The output string to the caller.</returns>
        [VaultExtensionMethod("TestVaultExtensionMethod", RequiredVaultAccess = MFVaultAccess.MFVaultAccessNone)]
        private string TestVaultExtensionMethod(EventHandlerEnvironment env)
        {
            // Return the input and the alias and id of the test class. If the class is missing, ID is -1.
            return env.Input + ": " + config.TestClassID.Alias + ": " + config.TestClassID.ID;
        }

        [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize)]
        private void BeforeCreatingANewObject(EventHandlerEnvironment env)
        {
            PropertyValues propertyValues = new PropertyValues();

            if (env.ObjVerEx.Class == config.Kokeiluasteluokka)
            {
                string stringValueToPlayWith = env.ObjVerEx.Properties.SearchForProperty(config.summaProperty).GetValueAsLocalizedText();

                //float valueToPlayWith = float.Parse(stringValueToPlayWith, CultureInfo.InvariantCulture.NumberFormat);
                float valueToPlayWith = (float)Convert.ToDouble(stringValueToPlayWith);
                valueToPlayWith += valueToPlayWith;

                var summaVal = env.ObjVerEx.Properties.SearchForProperty(config.summaProperty).Value.Value;
                
                CommonHelpers.LogMessageToFile("We found the following summaVal: " + summaVal.ToString());
             
                propertyValues = env.ObjVerEx.Properties;

                propertyValues.SearchForProperty(config.summaProperty).TypedValue.SetValue(MFDataType.MFDatatypeFloating, valueToPlayWith);

                env.ObjVerEx.SaveProperties(propertyValues);
            }

            CommonHelpers.LogMessageToFile("After operation, sum: " + env.ObjVerEx.Properties.SearchForProperty(config.summaProperty).GetValueAsLocalizedText());

        }





    }
}