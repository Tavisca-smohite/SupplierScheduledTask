using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public sealed class SupplierDataManagerDBContext :IDisposable
    {
        public static void UsingCommonContentDbRead(Action<SupplierDataManagerDataContext> action)
        {
            using (var context = new SupplierDataManagerDBContext())
            {
                action(context.Read);
            }
        }

        public static void UsingCommonContentDbWrite(Action<SupplierDataManagerDataContext> action)
        {
            using (var context = new SupplierDataManagerDBContext())
            {
                action(context.Write);
            }
        }

        public SupplierDataManagerDBContext()
        {
            if (_stack == null)
            {
                _stack = new Stack<SupplierDataManagerDBContext>();
                this.Depth = 1;
                this.Read = new SupplierDataManagerDataContext(DBConfiguration.ReadKayMatricesDatabaseConnection);
                this.Write = new SupplierDataManagerDataContext(DBConfiguration.WriteKayMatricesDatabaseConnection);
            }
            else
            {
                var parent = _stack.Peek();
                // Increment level of node.
                this.Depth = parent.Depth + 1;
                // Copy data context from the parent
                this.Read = parent.Read;
                this.Write = parent.Write;
            }
            _stack.Push(this);
        }

        public int Depth { get; private set; }

        public bool IsRoot
        {
            get { return this.Depth == 1; }
        }

        [ThreadStatic]
        private static Stack<SupplierDataManagerDBContext> _stack;
        public SupplierDataManagerDataContext Read { get; private set; }
        public SupplierDataManagerDataContext Write { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            var context = _stack.Pop();
            if (context.IsRoot == true)
            {
                context.Read.Dispose();
                context.Write.Dispose();
                _stack = null;
            }
        }

        #endregion
    }
}
