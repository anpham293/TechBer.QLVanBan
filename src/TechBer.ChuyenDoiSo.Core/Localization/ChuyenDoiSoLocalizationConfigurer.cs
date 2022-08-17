using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo.Localization
{
    public static class ChuyenDoiSoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ChuyenDoiSoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ChuyenDoiSoLocalizationConfigurer).GetAssembly(),
                        "TechBer.ChuyenDoiSo.Localization.ChuyenDoiSo"
                    )
                )
            );
        }
    }
}