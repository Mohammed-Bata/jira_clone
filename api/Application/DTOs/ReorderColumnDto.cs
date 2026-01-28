using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
     public record ReorderColumnDto(
        int ColumnId,
        double? PrevOrder,
        double? NextOrder
     );
}
