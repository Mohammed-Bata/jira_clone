import { Component, Input } from '@angular/core';
import { ProjectColumnDto, WorkItemDto } from '../../../core/models/Project';
import { Workitem } from '../workitem/workitem';
import { workitemservice } from '../../../core/services/workitemservice';
import { CdkDrag, CdkDragDrop, CdkDropList, CdkDropListGroup, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-column',
  imports: [Workitem,CdkDrag,CdkDropList],
  templateUrl: './column.html',
  styleUrl: './column.scss',
})
export class Column {
  @Input() column! :ProjectColumnDto;
  create : boolean = false;
  openMenuWorkItemId: number | null = null;

  constructor(private workitemService: workitemservice){}

  opencreate(){
    this.create = !this.create;
  }

  onWorkItemCreated(item:WorkItemDto){
    this.column.workItems.push(item);
    this.create = false;
  }

  openOptions(id:number){
    this.openMenuWorkItemId =
    this.openMenuWorkItemId === id ? null : id;
  }

  deleteWorkItem(id:number){
    this.workitemService.deleteWorkItem(id).subscribe({
      next:()=>{
        this.column.workItems = this.column.workItems.filter(wi => wi.id !== id);
      },
      error:(error)=>console.log(error)
    });
  }

  drop(event:CdkDragDrop<WorkItemDto[]>){
    moveItemInArray(this.column.workItems, event.previousIndex, event.currentIndex);


    const prevItem = this.column.workItems[event.currentIndex - 1];
    const nextItem = this.column.workItems[event.currentIndex + 1];
    const movedItem = this.column.workItems[event.currentIndex];

    const reorderDto = {
      workItemId: movedItem.id,
      PrevOrder: prevItem ? prevItem.order : null,
      NextOrder: nextItem ? nextItem.order : null
    };

    this.workitemService.reorderWorkItem(reorderDto).subscribe({
      next:(response)=>{
        console.log('Reorder successful:', response);
        movedItem.order = response.Order;
      },
      error:(error)=>{
        console.error('Reorder failed:', error);
        moveItemInArray(this.column.workItems, event.currentIndex, event.previousIndex);
      }
    });
   
  }
}