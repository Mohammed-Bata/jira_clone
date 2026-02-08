import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { workitemservice } from '../../../core/services/workitemservice';
import { CreateWorkItemDto } from '../../../core/models/WorkItem';
import { FormsModule } from '@angular/forms';
import { WorkItemDto } from '../../../core/models/Project';
import { MatDatepickerModule, MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-workitem',
  imports: [FormsModule, MatDatepickerModule, MatNativeDateModule],
  templateUrl: './workitem.html',
  styleUrl: './workitem.scss',
})
export class Workitem {
  title = '';
  type = '';
  typeListOpen = false;
  @Input() columnId! : number;
  @Output() createdworkitem = new EventEmitter<WorkItemDto>

  dueDate = signal<Date | null>(null);

  onDateChange(event: MatDatepickerInputEvent<Date>) {
    this.dueDate.set(event.value);
    console.log("New Due Date selected:", event.value?.toISOString());
  }

  options = [
    { value: '1', label: 'Task' },
    { value: '2', label: 'Bug' },
    { value: '3', label: 'Feature' }
  ];

  selectedOption = this.options[0].value;

  select(option: any) {
  this.selectedOption = option.value;
  this.typeListOpen = false;
}

getWorkItemIcon(optionValue: string) {
  return `../../../../assets/icons/WorkitemTypes/${this.options.find(o => o.value === optionValue)?.label}.svg`;
}

  constructor(private workItemservice:workitemservice){

  }

  openTypeList(){
    this.typeListOpen = !this.typeListOpen;
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
