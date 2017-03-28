using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodingLanguageAC
    {
        public ProgrammingLanguage LanguageCode { get; set; }
        public string LanguageName { get; set; }
    }
}
