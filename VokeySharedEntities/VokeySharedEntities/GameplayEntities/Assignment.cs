using System;
using System.Collections.Generic;

namespace VokeySharedEntities
{
    [Serializable]
	public class Assignment
	{
        public Guid roomToPlayIn; 
        public List<Guid> itemsToFind;
        public string name;
        public string summary;
        public Guid id;  
        public bool completed = false;
            
           public Assignment()
        {
        }
            
		public Assignment(string assignmentName, string assignmentSummary, Guid roomToPlay, List<Guid> objectsToFind)
		{
            name = assignmentName;
            summary = assignmentSummary;
            roomToPlayIn = roomToPlay;
            itemsToFind = objectsToFind;
            id = Guid.NewGuid();
		}
	}
}

