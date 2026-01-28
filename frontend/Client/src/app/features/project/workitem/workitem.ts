import { Component, EventEmitter, Input, Output } from '@angular/core';
import { workitemservice } from '../../../core/services/workitemservice';
import { CreateWorkItemDto } from '../../../core/models/WorkItem';
import { FormsModule } from '@angular/forms';
import { WorkItemDto } from '../../../core/models/Project';

@Component({
  selector: 'app-workitem',
  imports: [FormsModule],
  templateUrl: './workitem.html',
  styleUrl: './workitem.scss',
})
export class Workitem {
  title = '';
  @Input() columnId! : number;
  @Output() createdworkitem = new EventEmitter<WorkItemDto>

  constructor(private workItemservice:workitemservice){

  }

  createworkitem(){
    const dto:CreateWorkItemDto = {
      title: this.title,
      projectcolumnid:this.columnId,
      description:"sss",
      assignedtouserid:null,
    }

    this.workItemservice.createWorkItem(dto).subscribe({
      next:(response)=>this.createdworkitem.emit(response),
    
      error:(error)=>console.log(error)
    });
  }

}
