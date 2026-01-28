import { WorkItemDto } from "./Project";



export interface CreateColumnDto{
    title:string,
    projectId:number
}

export interface ReorderColumnDto{
    columnId: number;
    PrevOrder: number|null;
    NextOrder: number|null;
}

export interface ReorderResultDto{
    Order:number;
}
