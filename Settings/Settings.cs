using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deepgramwinsys.Setting
{
    public class Settings
    {
        public bool SaveCSV { get; set; }
        public bool SaveMP3 { get; set; }
        public bool ProfinityAllowed { get; set; }
        public bool CaptionHidden { get; set; }
        public bool TransparentBackground { get; set; }
        public string ApiKey { get; set; }
        public int SelectedLanguageId { get; set; }
        public int SelectedModelId { get; set; }

        public List<DeepGramLanguage>? DeepGramLanguages { get; set; }

    }
}
