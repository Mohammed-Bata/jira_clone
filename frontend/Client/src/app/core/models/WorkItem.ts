import { ItemType, Priority } from "./Project";

export interface CreateWorkItemDto{
    title:string,
    description:string|null,
    projectcolumnid:number,
    assignedtouserid:string|null,
    // priority: Priority|null;
    // dueDate?: string | null;  
    // type: ItemType|null;
}

export interface ReorderWorkItemDto{
    workItemId: number;
    columnid: number;
    PrevOrder: number|null;
    NextOrder: number|null;
}

export interface ReorderResultDto{
    Order:number;
}