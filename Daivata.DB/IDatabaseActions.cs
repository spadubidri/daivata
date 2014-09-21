using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daivata.Database
{
  public interface IDatabaseActions
  {
    void BeforeQueryExecution(Query query);
    void AftreQueryExecution(Query query);
    void OnException(Exception exception);
  }
}
