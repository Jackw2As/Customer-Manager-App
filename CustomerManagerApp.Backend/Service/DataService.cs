using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Service
{
    public class DataService
    {
        /// <summary>
        /// I don't want the Repository to be accessable to the Presentation Layer because 
        /// I don't want the Presentation Layer to decide when to call the repository or which 
        /// version to implement.
        /// 
        /// As a side effect I also get better de-coupling which will make future changes easier. 
        /// 
        /// So I created this DataService class that will handle using the 
        /// </summary>

        public DataService()
        {
            throw new NotImplementedException();
        }
    }
}
