using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record ReorderWorkItemsDto(
        int WorkItemId,
        double? PrevOrder,
        double? NextOrder
     );
   
}
