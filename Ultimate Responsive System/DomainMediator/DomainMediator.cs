using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.DomainSpecific
{
    public class DomainMediator : MarshalByRefObject
    {
        public void LoadDlls(string[] dllPaths)
        {
            AssemblyLoader.LoadAssemblies(dllPaths);
        }

        public string GenerateKeywordsReport()
        {
            return KeywordsExtractor.GenerateKeywordsReport();
        }
    }
}
