import { Component, OnInit, signal, Signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../core/services/projectservice';
import { ProjectDto } from '../../core/models/Project';

@Component({
  selector: 'app-project',
  imports: [],
  templateUrl: './project.html',
  styleUrl: './project.scss',
})
export class Project implements OnInit {
  project:Signal<ProjectDto | null>;
  loading = signal(true);

  constructor(private route:ActivatedRoute,private projectservice:ProjectService){
    this.project = this.projectservice.project;

  }

  
  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.projectservice.getProject(id);
    this.loading.set(false);
  }


}
