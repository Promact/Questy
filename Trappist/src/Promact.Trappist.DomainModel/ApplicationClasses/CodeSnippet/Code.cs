﻿using Promact.Trappist.DomainModel.Enum;

namespace Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet
{
    public class Code
    {
        public string Key;

        public string Source;

        public string Input;

        public ProgrammingLanguage Language;

        public CodeResponse CodeResponse;
    }
}