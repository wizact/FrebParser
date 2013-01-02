using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FrebParserConsole
{
    public static class ExtentionClass
    {
        public static String RetrieveValue(this XElement element)
        {
            return (element!=null?element.Value:"null");
        }
    }
}
