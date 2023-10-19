using Memory.Core.DataAccess.EntityFramework;
using Memory.DataAccess.Abstract;
using Memory.DataAccess.Concrete.EntityFramework.Context;
using Memory.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.DataAccess.Concrete.EntityFramework
{
    public class NotebookDal : RepositoryBase<Notebook>,INotebookDal
    {
        public NotebookDal(MemoryContext context):base(context)
        {
                
        }
    }
}
