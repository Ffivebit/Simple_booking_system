using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
    public class ResourceService : SQLiteDataService, IResource
    {
        public List<Resource> GetListOfAllResources()
        {
            try
            {
                return GetAllResources();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
