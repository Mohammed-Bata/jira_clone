import { CdkDragMove, DragDropModule } from '@angular/cdk/drag-drop';
import { Overlay, OverlayConfig } from '@angular/cdk/overlay';
import { CdkPortal, PortalModule } from '@angular/cdk/portal';
import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { CreateProject } from '../../../features/project/create-project/create-project';
import { ProjectService } from '../../../core/services/projectservice';
import { GetProjectsDto } from '../../../core/models/Project';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [DragDropModule,PortalModule,CreateProject,RouterLink],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar implements OnInit {
  protected defaultWidth = 300;
  protected currentWidth = signal(this.defaultWidth);
  open = signal(false);
  @ViewChild(CdkPortal) portal!: CdkPortal;
  projects = signal<GetProjectsDto[]>([]);
  loading = signal(true);

  constructor(private overlay : Overlay,private projectservice:ProjectService){}

  ngOnInit():void{
    this.projectservice.getProjects().subscribe({
      next:dtos =>{
        this.projects.set(dtos);
        this.loading.set(false);
        console.log(dtos);
      },
      error: () => this.loading.set(false)
    })
  }

  openModel(){
    const config = new OverlayConfig({
      positionStrategy : this.overlay.position().global().centerHorizontally().centerVertically(),
      width:'60%',
      height:'60%',
      hasBackdrop: true
    });

    const overlayRef = this.overlay.create(config);
    overlayRef.attach(this.portal);
    overlayRef.backdropClick().subscribe(()=> overlayRef.detach());
  }

  protected onDragMoved(event : CdkDragMove){
    this.currentWidth.set(event.pointerPosition.x);

    const element = event.source.element.nativeElement;

    element.style.transform = 'none';
  }


  toggle() {
    this.open.update(v => !v);
    console.log(this.open());
  }
}
