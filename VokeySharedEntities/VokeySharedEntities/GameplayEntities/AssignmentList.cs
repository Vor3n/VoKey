using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VokeySharedEntities
{
    [Serializable]
	public class AssignmentList
	{
        [XmlIgnore]
        public List<Assignment> CompletedAssignments
        {
            get
            {
                List<Assignment> done = new List<Assignment>();
                if(_assignments == null) return done;
                foreach(Assignment a in _assignments) if(a.completed) done.Add(a);
                return done;
            }
        }
        
        [XmlIgnore]
        public List<Assignment> TodoAssignments
        {
            get
            {
                List<Assignment> todo = new List<Assignment>();
                if(_assignments == null) return todo;
                foreach(Assignment a in _assignments) if(!a.completed) todo.Add(a);
                return todo;
            }
        }
        
        public AssignmentList(){
        }
        
        private List<Assignment> _assignments;
        
        public List<Assignment> Assignments
        {
            get
            {
                return _assignments;
            }
            set
            {
                _assignments = value;
            }
        }
        
        public void Add(Assignment a)
        {
            if(_assignments == null) _assignments = new List<Assignment>();
            _assignments.Add(a);
        }
	}
}

