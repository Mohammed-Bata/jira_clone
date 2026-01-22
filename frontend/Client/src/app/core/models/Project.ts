export interface CreateProjectDto{
    Name : string | null,
    Description: string | null
}

export interface GetProjectsDto{
    id:number |null,
    name:string |null
}

export interface WorkItemDto {
  id: number;
  title: string;
  order: number;
  assignedToUserId?: string;
  priority: Priority;
  dueDate?: string | null;  // DateOnly comes as string from API
  type: ItemType;
}

export enum Priority {
  Low = 0,
  Medium = 1,
  High = 2
}

export enum ItemType {
  Task = 0,
  Bug = 1,
  Story = 2
}

export interface ProjectColumnDto {
  id: number;
  title: string;
  order: number;
  workItems: WorkItemDto[];
}

export interface ProjectDto {
  id: number;
  name: string;
  columns: ProjectColumnDto[];
}