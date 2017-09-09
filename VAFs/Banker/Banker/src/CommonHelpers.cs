using System;
using System.Collections.Generic;
using MFiles.VAF;
using MFiles.VAF.Common;
using MFilesAPI;

namespace Banker
{
    public static class CommonHelpers
    {
        /// <summary>
        /// </summary>
        public static List<ObjVerEx> SearchHelperEx(int? propertyID, MFDataType dataType, int? objectTypeID, int? classID, int? objectID, Vault vault)
        {
            MFSearchBuilder searchBuilder = new MFSearchBuilder(vault);
            if (objectTypeID != null)
            { searchBuilder.ObjType(objectTypeID); }
            if (classID != null)
            { searchBuilder.Class(classID); }
            if (propertyID != null && objectID != null)
            { searchBuilder.Property(propertyID, dataType, objectID); }

            searchBuilder.Deleted(false);

            return searchBuilder.FindEx();
        }

        /// <summary>
        /// Search helper method.
        /// </summary>
        /// <param name="PD">Property ID wanted to be found</param>
        /// <param name="ObjTypeID">Object Type ID wanted to be found</param>
        /// <param name="ObjVerID">ID being searched</param>
        /// <param name="Template">Is template</param>
        /// <returns>
        /// Found Objects
        /// </returns>
        public static ObjectSearchResults SearchHelperLookup(int PD, int ObjTypeID, int ObjVerID, bool Template, Vault vault)
        {
            MFSearchBuilder oSCs = new MFSearchBuilder(vault);
            oSCs.ObjType(ObjTypeID);
            oSCs.Deleted(false);
            if (Template)
            {
                oSCs.Property((int)MFBuiltInPropertyDef.MFBuiltInPropertyDefIsTemplate, MFDataType.MFDatatypeBoolean, Template);
            }
            oSCs.Property(PD, MFDataType.MFDatatypeLookup, ObjVerID);

            return oSCs.Find();
        }

        /// <summary>
        /// </summary>
        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        public static void LogMessageToFile(string msg)
        {
            if (Logging.debug)
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(GetTempPath() + "vaultApp_" + DateTime.Now.ToShortDateString() + ".txt");
                try
                {
                    string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }
    }
}
