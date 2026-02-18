import { Component, EventEmitter, InjectionToken, Injector, Input, Output, ViewChild } from '@angular/core';
import { ProjectColumnDto, WorkItemPatchEvent, WorkItemPreviewDto } from '../../../core/models/Project';
import { Workitem } from '../workitem/workitem';
import { workitemservice } from '../../../core/services/workitemservice';
import { CdkDrag, CdkDragDrop, CdkDropList, CdkDropListGroup, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ColumnService } from '../../../core/services/columnservice';
import { DateTime } from 'luxon';
import { CdkPortal, ComponentPortal, PortalModule } from '@angular/cdk/portal';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { Workitemdetails } from '../workitemdetails/workitemdetails';
import { AvatarColorPipe } from '../../../shared/pipes/avatar-color-pipe';
import { InitialsPipe } from '../../../shared/pipes/initials-pipe';

export const WORK_ITEM_ID = new InjectionToken<number>('WORK_ITEM_ID');
export const PROJECT_ID = new InjectionToken<number>('PROJECT_ID');


@Component({
  selector: 'app-column',
  imports: [Workitem,CdkDrag,CdkDropList,DragDropModule,PortalModule,Workitemdetails,AvatarColorPipe,InitialsPipe],
  templateUrl: './column.html',
  styleUrl: './column.scss',
})

export class Column {
  @Input() column! :ProjectColumnDto;
  @Input() projectId!:number;
  @Input() connectedTo: string[] = [];
  @Output() columnDeleted = new EventEmitter<number>();
  create : boolean = false;
  openMenuWorkItemId: number | null = null;
  openMenuColumnId:number | null = null;
  @ViewChild(CdkPortal) portal!: CdkPortal;

  types = ['Task', 'Bug', 'Feature'];
  priorities = ['Lowest', 'Low', 'Medium', 'High', 'Highest'];


  constructor(private overlay : Overlay,private injector: Injector ,private workitemService: workitemservice,private columnService: ColumnService){
    this.workitemService.itemPatch$.subscribe(patch=>{
      this.applyPatchToLocalBoard(patch);
    })
  }

  applyPatchToLocalBoard(patch: WorkItemPatchEvent){
    const item = this.column.workItems.find(i => i.id === patch.id);

    if(item){
      Object.assign(item, patch.changes);
    }
  }

  opencreate(){
    this.create = !this.create;
  }

  

  openWorkItemDetails(itemId:number){
     const config = new OverlayConfig({
      positionStrategy : this.overlay.position().global().centerHorizontally().centerVertically(),
      width:'60%',
      height:'60%',
      hasBackdrop: true
    });

    const overlayRef = this.overlay.create(config);

    const customInjector = Injector.create({
    parent: this.injector,
    providers: [
      { provide: WORK_ITEM_ID, useValue: itemId },
      { provide:PROJECT_ID, useValue:this.projectId},
      { provide: OverlayRef, useValue: overlayRef }
    ]
    
    });
    const componentPortal = new ComponentPortal(Workitemdetails, null, customInjector);
    overlayRef.attach(componentPortal);

    //overlayRef.attach(this.portal);
    overlayRef.backdropClick().subscribe(()=> overlayRef.detach());
  }


formatDate(dateString: string): string {
  return DateTime
    .fromISO(dateString)
    .toFormat('LLL dd, yyyy');
}

  onWorkItemCreated(item:WorkItemPreviewDto){
    this.column.workItems.push(item);
    this.create = false;
  }

  openOptions(id:number){
    this.openMenuWorkItemId =
    this.openMenuWorkItemId === id ? null : id;
  }

  openColumnOptions(id:number){
    this.openMenuColumnId =
    this.openMenuColumnId === id ? null : id;
  }

  deleteColumn(columnId:number){
    this.columnService.deleteColumn(columnId).subscribe({
      next:()=>{
        this.columnDeleted.emit(columnId);
      },
      error:(error)=>console.log(error)
    });
  }

  deleteWorkItem(id:number){
    this.workitemService.deleteWorkItem(id).subscribe({
      next:()=>{
        this.column.workItems = this.column.workItems.filter(wi => wi.id !== id);
      },
      error:(error)=>console.log(error)
    });
  }

  onWorkItemReorder(event:CdkDragDrop<WorkItemPreviewDto[]>){

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