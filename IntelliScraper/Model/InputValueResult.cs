using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Model
{
    public class InputValueResult
    {
        public string inputValue = string.Empty;
        public object inputValueO = new object();
        public Type inputType = typeof(object);
        public enum ValueType{_string,RuleEntity}
    }
}
