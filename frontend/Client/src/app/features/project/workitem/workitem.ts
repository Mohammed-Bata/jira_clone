import { Component, computed, EventEmitter, Input, Output, signal } from '@angular/core';
import { workitemservice } from '../../../core/services/workitemservice';
import { CreateWorkItemDto } from '../../../core/models/WorkItem';
import { FormsModule } from '@angular/forms';
import { WorkItemDto } from '../../../core/models/Project';
import { Calendar } from '../../../shared/components/calendar/calendar';
import { DateTime } from 'luxon';



@Component({
  selector: 'app-workitem',
  imports: [FormsModule, Calendar],
  templateUrl: './workitem.html',
  styleUrl: './workitem.scss',
})
export class Workitem {
  title = '';
  type = '';
  typeListOpen = false;
  datepickerOpen = false;
  activeDay = signal<string | null>(null);
  @Input() columnId! : number;
  @Output() createdworkitem = new EventEmitter<WorkItemDto>

  formattedDay = computed(() => {
  const dayStr = this.activeDay();
  if (!dayStr) return '';
  return DateTime.fromISO(dayStr).toFormat('LLL dd'); // e.g., Feb 10
});

  toggleDatePicker() {
    this.datepickerOpen = !this.datepickerOpen;
  }

    onDaySelected(day: any) {
    
    this.activeDay.set(day.toISODate());
  
    this.datepickerOpen = false;
  }


  options = [
    { value: 1, label: 'Task' },
    { value: 2, label: 'Bug' },
    { value: 3, label: 'Feature' }
  ];

  selectedOption = this.options[0].value;

  select(option: any) {
  this.selectedOption = option.value;
  this.typeListOpen = false;
}

getWorkItemIcon(optionValue: number) {
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
      type: this.selectedOption,
      dueDate: this.activeDay()
    }

    this.workItemservice.createWorkItem(dto).subscribe({
      next:(response)=>this.createdworkitem.emit(response),
    
      error:(error)=>console.log(error)
    });
  }

}
