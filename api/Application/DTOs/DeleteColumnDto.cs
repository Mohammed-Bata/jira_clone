using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record DeleteColumnDto(int ColumnId, int? TargetColumnId, List<int>? WorkItemIds);
    
}
