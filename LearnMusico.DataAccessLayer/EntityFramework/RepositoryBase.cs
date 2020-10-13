using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object lockobject = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            lock (lockobject)
            {
                if (context == null)
                {
                    context = new DatabaseContext();
                }
            }

        }

    }
}
