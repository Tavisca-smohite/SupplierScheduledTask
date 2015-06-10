using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public sealed class SupplierConfigUpdateManagerDBContext : IDisposable
    {
        public static void UsingCommonContentDbRead(Action<SupplierConfigUpdateManagerDataContext> action)
        {
            using (var context = new SupplierConfigUpdateManagerDBContext())
            {
                action(context.Read);
            }
        }

        public static void UsingCommonContentDbWrite(Action<SupplierConfigUpdateManagerDataContext> action)
        {
            using (var context = new SupplierConfigUpdateManagerDBContext())
            {
                action(context.Write);
            }
        }

        public SupplierConfigUpdateManagerDBContext()
        {
            if (_stack == null)
            {
                _stack = new Stack<SupplierConfigUpdateManagerDBContext>();
                this.Depth = 1;
                this.Read = new SupplierConfigUpdateManagerDataContext(DBConfiguration.ReadSupplierConfigDatabaseConnection);
                this.Write = new SupplierConfigUpdateManagerDataContext(DBConfiguration.WriteSupplierConfigDatabaseConnection);
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
        private static Stack<SupplierConfigUpdateManagerDBContext> _stack;
        public SupplierConfigUpdateManagerDataContext Read { get; private set; }
        public SupplierConfigUpdateManagerDataContext Write { get; private set; }

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
