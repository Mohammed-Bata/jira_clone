import { Component, computed, EventEmitter, Input, Output, signal } from '@angular/core';
import { workitemservice } from '../../../core/services/workitemservice';
import { CreateWorkItemDto } from '../../../core/models/WorkItem';
import { FormsModule } from '@angular/forms';
import { WorkItemDto } from '../../../core/models/Project';
import { Calendar } from '../../../shared/components/calendar/calendar';
import { DateTime } from 'luxon';
import { User } from '../../../core/models/User';
import { TeamService } from '../../../core/services/teamservice';



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
  assignedOpen = false;
  activeDay = signal<string | null>(null);
  teamMembers = signal<User[]>([]);
  selectedMember = signal<User|null>(null);
  @Input() columnId! : number;
  @Input() projectId!:number;
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

  private avatarColors = [
    '#0052CC', '#0747A6', '#0065FF', '#2684FF', 
    '#00875A', '#36B37E', '#FFAB00', '#FF5630', 
    '#6554C0', '#5243AA', '#FF8B00', '#00B8D9'
  ];

  // Pass the ID (string or number) to guarantee uniqueness
  getAvatarColor(userId: string | number): string {
    const idString = userId.toString();
    let hash = 0;
    
    for (let i = 0; i < idString.length; i++) {
      hash = idString.charCodeAt(i) + ((hash << 5) - hash);
    }

    const index = Math.abs(hash) % this.avatarColors.length;
    return this.avatarColors[index];
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

  constructor(private workItemservice:workitemservice,private teamService:TeamService) {

  }

  openTypeList(){
    this.typeListOpen = !this.typeListOpen;
  }

  createworkitem(){
    const dto:CreateWorkItemDto = {
      title: this.title,
      projectcolumnid:this.columnId,
      description:'',
      assignedtouserid:this.selectedMember() ? this.selectedMember()!.id : null,
      type: this.selectedOption,
      dueDate: this.activeDay()
    }

    this.workItemservice.createWorkItem(dto).subscribe({
      next:(response)=>this.createdworkitem.emit(response),
    
      error:(error)=>console.log(error)
    });
  }

  toggleAssigned() {
    if(!this.assignedOpen&&this.teamMembers().length===0){
      //fetch team members
      this.teamService.getTeamMembers(this.projectId).subscribe({
        next:(response)=>{
          this.teamMembers.set(response);
          console.log("fetched team members:");
          console.log(response);
        },
        error:(error)=>console.log(error)
      });
    }
    
    this.assignedOpen = !this.assignedOpen;
  }

}
