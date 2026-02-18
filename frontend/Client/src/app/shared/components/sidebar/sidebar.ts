import { CdkDragMove, DragDropModule } from '@angular/cdk/drag-drop';
import { Overlay, OverlayConfig } from '@angular/cdk/overlay';
import { CdkPortal, PortalModule } from '@angular/cdk/portal';
import { Component, HostListener, OnInit, Signal, signal, ViewChild } from '@angular/core';
import { CreateProject } from '../../../features/project/create-project/create-project';
import { ProjectService } from '../../../core/services/projectservice';
import { GetProjectsDto, ProjectDto } from '../../../core/models/Project';
import { ActivatedRoute, Route, Router, RouterLink } from '@angular/router';
import { icons } from '../../icons/icons';

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
  projects : Signal<GetProjectsDto[]>;
  loading = signal(true);
  selectedProject!: Signal<ProjectDto | null>;
  Menu = false;
  openMenuProjectId: number | null = null;


  constructor(private overlay : Overlay,private projectservice:ProjectService,private router:Router){
    this.selectedProject = this.projectservice.project;
    this.projects = this.projectservice.projects;
  }

  ngOnInit():void{
    this.projectservice.getProjects().subscribe({
      next:dtos =>{
        this.loading.set(false);
        
      },
      error: () => this.loading.set(false)
    })
  }

  openModel(event:MouseEvent){
    event.stopPropagation();
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

  getProjectIcon(projectId:number):string{
    return icons[projectId % icons.length];
  }

  openMenu(projectId:number,event:MouseEvent){
    event.stopPropagation();
    this.Menu = !this.Menu
    this.openMenuProjectId =
    this.openMenuProjectId === projectId ? null : projectId;
  }

  @HostListener('document:click')
  closeMenu() {
    this.openMenuProjectId = null;
  }

  deleteProject(projectId:number){
    
    this.projectservice.deleteProject(projectId).subscribe({
      next:()=>{
          if (this.selectedProject() && this.selectedProject()!.id === projectId) {
            this.router.navigate(['/']);
          }
      },
      error:(error) => console.error('Error deleting project:', error)
    });
  }
}
