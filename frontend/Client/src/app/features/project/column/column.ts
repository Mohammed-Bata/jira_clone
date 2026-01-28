import { Component, Input } from '@angular/core';
import { ProjectColumnDto, WorkItemDto } from '../../../core/models/Project';
import { Workitem } from '../workitem/workitem';
import { workitemservice } from '../../../core/services/workitemservice';
import { CdkDrag, CdkDragDrop, CdkDropList, CdkDropListGroup, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-column',
  imports: [Workitem,CdkDrag,CdkDropList,DragDropModule],
  templateUrl: './column.html',
  styleUrl: './column.scss',
})
export class Column {
  @Input() column! :ProjectColumnDto;
  @Input() connectedTo: string[] = [];
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

  onWorkItemReorder(event:CdkDragDrop<WorkItemDto[]>){

    if (event.previousContainer === event.container) {
    // Same column: just reorder
    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }else{
      transferArrayItem(
      event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      event.currentIndex
      );
    }

    const targetList = event.container.data;
    const prevItem = targetList[event.currentIndex - 1];
    const nextItem = targetList[event.currentIndex + 1];

    const reorderDto = {
      workItemId: event.item.data.id,
      columnid: this.column.id,
      PrevOrder: prevItem ? prevItem.order : null,
      NextOrder: nextItem ? nextItem.order : null
    };

    this.workitemService.reorderWorkItem(reorderDto).subscribe({
      next:(response)=>{
        console.log('Reorder successful:', response);
        event.item.data.order = response.order;
      },
      error:(error)=>{
        console.error('Reorder failed:', error);
        if (event.previousContainer === event.container) {
        moveItemInArray(event.container.data, event.currentIndex, event.previousIndex);
      } else {
        transferArrayItem(event.container.data, event.previousContainer.data, event.currentIndex, event.previousIndex);
      }
      }
    });
   
  }
}