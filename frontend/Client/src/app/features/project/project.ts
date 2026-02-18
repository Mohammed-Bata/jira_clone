import { Component, DestroyRef, OnDestroy, OnInit, signal, Signal, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../core/services/projectservice';
import { ProjectColumnDto, ProjectDto, WorkItemPatchEvent } from '../../core/models/Project';
import { Workitem } from './workitem/workitem';
import { Column } from './column/column';
import { Createcolumn } from './createcolumn/createcolumn';
import { CdkDrag, CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { ColumnService } from '../../core/services/columnservice';
import { CdkPortal, PortalModule } from '@angular/cdk/portal';
import { Overlay, OverlayConfig } from '@angular/cdk/overlay';
import { Invitation } from './invitation/invitation';
import { switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { icons } from '../../shared/icons/icons';
import { workitemservice } from '../../core/services/workitemservice';
import { NotificationsService } from '../../core/services/notificationsservice';

@Component({
  selector: 'app-project',
  imports: [Column,Createcolumn,CdkDrag,CdkDropList,DragDropModule,PortalModule,Invitation],
  templateUrl: './project.html',
  styleUrl: './project.scss',
})
export class Project implements OnInit, OnDestroy
{
  project:Signal<ProjectDto | null>;
  loading:Signal<boolean>;
  @ViewChild(CdkPortal) portal!: CdkPortal;
  create : boolean = false;

  constructor(private route:ActivatedRoute,private projectservice:ProjectService,private columnService:ColumnService,private overlay : Overlay, private notificationservice:NotificationsService,private destroyRef: DestroyRef){
    this.project = this.projectservice.project;
    this.loading = this.projectservice.loading;

  }

  ngOnInit(): void {
     this.route.paramMap.pipe(
      switchMap(params => {
        const id = Number(params.get('id'));
        
        if(id){
          this.notificationservice.joinProjectGroup(id.toString());
        }
        
        return this.projectservice.getProject(id);
      }),
      takeUntilDestroyed(this.destroyRef) 
    ).subscribe();

  }

  ngOnDestroy(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.notificationservice.leaveProjectGroup(id);
    }
  }

  

  getProjectIcon(projectId:number):string{
      return icons[projectId % icons.length];
    }


  opencreate(){
    this.create = !this.create;
  }
  
  onColumnCreated(column:ProjectColumnDto){
    this.project()!.columns.push(column);
    this.create = false;
  }

  openModel(){
    const config = new OverlayConfig({
      positionStrategy : this.overlay.position().global().centerHorizontally().centerVertically(),
       width:'30%',
       height:'80%',
      hasBackdrop: true
    });

    const overlayRef = this.overlay.create(config);
    overlayRef.attach(this.portal);
    overlayRef.backdropClick().subscribe(()=> overlayRef.detach());
  }

  get allColumnIds(): string[] {
  return this.project()?.columns.map(col => 'list-' + col.id) || [];
}

handlecolumndelete(columnId:number){
    this.project()!.columns = this.project()!.columns.filter(col => col.id !== columnId);
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
