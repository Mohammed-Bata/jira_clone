import { Component, OnInit, signal, Signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../core/services/projectservice';
import { ProjectColumnDto, ProjectDto } from '../../core/models/Project';
import { Workitem } from './workitem/workitem';
import { Column } from './column/column';
import { Createcolumn } from './createcolumn/createcolumn';
import { CdkDrag, CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { ColumnService } from '../../core/services/columnservice';

@Component({
  selector: 'app-project',
  imports: [Column,Createcolumn,CdkDrag,CdkDropList,DragDropModule],
  templateUrl: './project.html',
  styleUrl: './project.scss',
})
export class Project implements OnInit {
  project:Signal<ProjectDto | null>;
  loading:Signal<boolean>;
  create : boolean = false;

  constructor(private route:ActivatedRoute,private projectservice:ProjectService,private columnService:ColumnService){
    this.project = this.projectservice.project;
    this.loading = this.projectservice.loading;
  }

  
  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.projectservice.getProject(id);
  }

  opencreate(){
    this.create = !this.create;
  }
  
  onColumnCreated(column:ProjectColumnDto){
    this.project()!.columns.push(column);
    this.create = false;
  }

  get allColumnIds(): string[] {
  return this.project()?.columns.map(col => 'list-' + col.id) || [];
}
  
  onColumnDropped(event:CdkDragDrop<ProjectColumnDto[]>){
    moveItemInArray(this.project()!.columns, event.previousIndex, event.currentIndex);


    const prevItem = this.project()?.columns[event.currentIndex - 1];
    const nextItem = this.project()?.columns[event.currentIndex + 1];
    const movedItem = this.project()?.columns[event.currentIndex];

    const reorderDto = {
      columnId: movedItem!.id,
      PrevOrder: prevItem ? prevItem.order : null,
      NextOrder: nextItem ? nextItem.order : null
    };

    this.columnService.reorder(reorderDto).subscribe({
      next:(response)=>{
        console.log('Reorder successful:', response);
        movedItem!.order = response.Order;
      },
      error:(error)=>{
        console.error('Reorder failed:', error);
        moveItemInArray(this.project()!.columns, event.currentIndex, event.previousIndex);
      }
    });
   
  }




}
