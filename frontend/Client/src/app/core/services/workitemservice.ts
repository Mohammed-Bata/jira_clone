import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { CreateWorkItemDto, ReorderResultDto, ReorderWorkItemDto } from "../models/WorkItem";
import { observableToBeFn } from "rxjs/internal/testing/TestScheduler";
import { catchError, Observable, tap } from "rxjs";
import { API_ENDPOINTS } from "../constants/api-endpoints";




@Injectable({
  providedIn: 'root',
})

export class workitemservice{
  
    private readonly apiUrl = environment.apiUrl;

    constructor(private http:HttpClient){

    }

    reorderWorkItem(dto:ReorderWorkItemDto):Observable<ReorderResultDto|any>{
      return this.http.patch<ReorderResultDto>(`${this.apiUrl}${API_ENDPOINTS.WORKITEMS.REORDER}`,dto)
      .pipe(
        tap((response)=>console.log(response)),
      );
    }

    createWorkItem(dto:CreateWorkItemDto):Observable<number|any>{
        return this.http.post<number>(`${this.apiUrl}${API_ENDPOINTS.WORKITEMS.CREATE}`,dto)
        .pipe(
          tap((response)=>console.log(response)),
        );
    }

    deleteWorkItem(id:number):Observable<void|any>{
      return this.http.delete<void>(`${this.apiUrl}${API_ENDPOINTS.WORKITEMS.DELETE}/${id}`)
      .pipe(
        tap((response)=>console.log(response)),
      );
    }
}