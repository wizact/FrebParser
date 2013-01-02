using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FrebParserConsole
{
    internal class FrebHelper
    {
        string _NameSpace = "{http://schemas.microsoft.com/win/2004/08/events/event}";
        string _EventDataNameSpace = "{http://schemas.microsoft.com/win/2004/08/events/event}";

        internal void ExtractInfoFromLogFiles(string filename)
        {
            string strFileName = filename;
            XDocument frebDoc = XDocument.Load(strFileName);
            
            
            //Get /Event
            var query = from x in frebDoc.Root.Elements(_NameSpace + "Event")
                        select x;

            FrebLog frebLog = new FrebLog();
            //Set url and root props...
            frebLog.URL = frebDoc.Element("failedRequest").Attribute("url").Value;
            //frebLog.FileCreateDate = ;
            frebLog.FrebFileName = filename;

            FindValues(query, frebLog);
        }

        private void FindValues(IEnumerable<XElement> query, FrebLog frebLog)
        {
            //Get /Event/System/Level that are Level 1, 2, or 3
            string[] eventIDsToLoad = new string[] { "1", "2", "3" };
            var filteredEvents = query.Where(e => eventIDsToLoad.Contains(e.Element(_NameSpace + "System").Element(_NameSpace + "Level").Value)).ToList();

            foreach (var item in filteredEvents)
            {

                frebLog.System = new SystemData()
                {
                    Provider = (item.Element(_NameSpace + "System").Element(_NameSpace + "Provider").Attribute("Name").Value),
                    EventID = (item.Element(_NameSpace + "System").Element(_NameSpace + "EventID").RetrieveValue()),
                    Version = (item.Element(_NameSpace + "System").Element(_NameSpace + "Version").RetrieveValue()),
                    Level = (item.Element(_NameSpace + "System").Element(_NameSpace + "Level").RetrieveValue()),
                    OPcode = (item.Element(_NameSpace + "System").Element(_NameSpace + "Opcode").RetrieveValue()),
                    Keywords = (item.Element(_NameSpace + "System").Element(_NameSpace + "Keywords").RetrieveValue()),
                    TimeCreated = (item.Element(_NameSpace + "System").Element(_NameSpace + "TimeCreated").Attribute("SystemTime").Value),
                    Computer = (item.Element(_NameSpace + "System").Element(_NameSpace + "Computer").RetrieveValue())
                };

                EventData eventData = new EventData();
                eventData.ContextID = eventData.RetrieveProperty(item.Element(_EventDataNameSpace + "EventData"), _EventDataNameSpace + "Data", _EventDataNameSpace + "Name", "ContextId");
                eventData.LineNumber = eventData.RetrieveProperty(item.Element(_EventDataNameSpace + "EventData"), _EventDataNameSpace + "Data", _EventDataNameSpace + "Name", "LineNumber");
                eventData.Description = eventData.RetrieveProperty(item.Element(_EventDataNameSpace + "EventData"), _EventDataNameSpace + "Data", _EventDataNameSpace + "Name", "Description");
                eventData.ErrorCode = eventData.RetrieveProperty(item.Element(_EventDataNameSpace + "EventData"), _EventDataNameSpace + "Data", _EventDataNameSpace + "Name", "ErrorCode");

                frebLog.Event = eventData;

                FrebLogDB frebTable = new FrebLogDB();
                frebTable.URL = frebLog.URL;
                frebTable.Filename = frebLog.FrebFileName;

                frebTable.Provider = frebLog.System.Provider;
                frebTable.EventID = frebLog.System.EventID;
                frebTable.Version = frebLog.System.Version;
                frebTable.FLevel = frebLog.System.Level;
                frebTable.OPcode = frebLog.System.OPcode;
                frebTable.Keywords = frebLog.System.Keywords;


                if (!string.IsNullOrEmpty(frebLog.System.TimeCreated))
                {
                    string[] strTimeSplit = frebLog.System.TimeCreated.Split(new string[] { "T" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strTimeSplit.Count() > 1)
                    {
                        frebTable.DateCreated = strTimeSplit[0];
                        frebTable.TimeCreated = strTimeSplit[1];
                    }

                }


                frebTable.Computer = frebLog.System.Computer;

                frebTable.ContextID = frebLog.Event.ContextID;
                frebTable.LineNumber = frebLog.Event.LineNumber;
                frebTable.Description = frebLog.Event.Description;
                frebTable.ErrorCode = frebLog.Event.ErrorCode;

                if ((string.IsNullOrEmpty(frebTable.LineNumber) && string.IsNullOrEmpty(frebTable.Description)))
                    continue;


                PersistLogInfo(frebTable);
                break;

            }
        }

        private static void PersistLogInfo(FrebLogDB frebTable)
        {
            frebEntities db = new frebEntities();
            db.AddToFrebLogDBs(frebTable);
            db.SaveChanges();
        }
    }
}
