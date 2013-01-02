using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FrebParserConsole
{
    public class FrebLog
    {
        public string URL { get; set; }
        public string FrebFileName { get; set; }
        public DateTime FileCreateDate { get; set; }

        public SystemData System { get; set; }
        public EventData Event { get; set; }

    }

    public class SystemData
    {
        public string Provider { get; set; }
        public string EventID { get; set; }
        public string Version { get; set; }
        public string Level { get; set; }
        public string OPcode { get; set; }
        public string Keywords { get; set; }
        public string TimeCreated { get; set; }
        public string Computer { get; set; }
    }

    public class EventData
    {
        public string ContextID { get; set; }
        public string LineNumber { get; set; }
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        
        public string RetrieveProperty(XElement element, string matchedElementName ,string matchedAttributeName,string matchedAttributeValue)
        {
            var val = element.Elements(matchedElementName).AsEnumerable().Where(e => e.Attribute("Name").Value == matchedAttributeValue).FirstOrDefault();
            if (val != null)
            {
                //Console.WriteLine(val);
                return val.Value;
            }
            else
            {
                return "";    
            }
                        
            
        }
    }
}
