using System;

namespace MoneyOps.Infrastructure.Auditing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotAudit : Attribute
    {
        
    }
}