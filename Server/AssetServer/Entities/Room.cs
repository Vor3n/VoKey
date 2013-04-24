using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AssetServer.Entities
{
    public class Room : Entity
    {
        private string _name;
        private XmlSerializer s = null;
        private Type type = null;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        [XmlAttribute("RoomId")]
        public Guid roomId;

        [XmlArrayAttribute("FindableObjects")]
        public ObservableCollection<FindableObject> objects;

        public Room()
        {
            type = typeof(Room);

        }

        public Room(string name)
        {
            type = typeof(Room);
            _name = name;
            roomId = Guid.NewGuid();
            objects = new ObservableCollection<FindableObject>();
        }

        public void Add(FindableObject fo)
        {
            objects.Add(fo);
        }


        public XmlDocument Serialize(Room rootclass)
        {
            string xml = StringSerialize(rootclass);
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            return doc;
        }

        public XmlDocument Serialize(ObservableCollection<Room> collection)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            foreach (Room r in collection)
            {
                string xml = StringSerialize(r);
                doc.LoadXml(xml);
            }

            return doc;
        }

        private string StringSerialize(Room rootclass)
        {
            TextWriter w = WriterSerialize(rootclass);
            string xml = w.ToString();
            w.Close();
            return xml.Trim();
        }

        private TextWriter WriterSerialize(Room rootclass)
        {
            TextWriter w = new StringWriter();
            this.s = new XmlSerializer(this.type);
            s.Serialize(w, rootclass);
            w.Flush();
            return w;
        }


        
    }
}
