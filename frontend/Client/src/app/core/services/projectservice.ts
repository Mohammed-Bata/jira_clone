import { Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { CreateProjectDto, ProjectDto, GetProjectsDto } from "../models/Project";
import { catchError, Observable, tap, throwError } from "rxjs";




@Injectable({
  providedIn: 'root',
})

export class ProjectService{

  private _project = signal<ProjectDto | null>(null);
  project = this._project.asReadonly();

  private _loading = signal(true);
  loading = this._loading.asReadonly();


    private readonly apiUrl = environment.apiUrl;

    constructor(private http:HttpClient){

    }

    getProject(id:number):void{
      this.http.get<ProjectDto>(`${this.apiUrl}${API_ENDPOINTS.PROJECT.GETPROJECT}/${id}`)
      .subscribe({
        next:response =>{this._project.set(response);
          console.log(response);
          this._loading.set(false);
        },
        error: err => console.error(err)
      })
    }

    getProjects():Observable<GetProjectsDto[]|any>{
      return this.http.get<GetProjectsDto>(`${this.apiUrl}${API_ENDPOINTS.PROJECT.GETALL}`)
      .pipe(
        tap((response)=>console.log(response)), 
        catchError((error)=>this.handleError(error))
      );
    }

    createProject(dto : CreateProjectDto):Observable<number | any> {
      return this.http.post<number>(`${this.apiUrl}${API_ENDPOINTS.PROJECT.CREATE}`, dto)
      .pipe(
      tap((response) => console.log(response)),
      catchError((error) => this.handleError(error))
      );
    }

    private handleError(error:any):Observable<never>{
      const errMsg = error.error?.message || error.error;
      return throwError(() => errMsg);
    }
    
}