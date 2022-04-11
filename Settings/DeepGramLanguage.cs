using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deepgramwinsys.Setting
{
    public class DeepGramLanguage
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public List<DeepGramModel> Models { get; set; }
    }
}
