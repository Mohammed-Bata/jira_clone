import { Component, Inject, OnInit, signal } from '@angular/core';
import { WORK_ITEM_ID } from '../column/column';
import { workitemservice } from '../../../core/services/workitemservice';
import { OverlayRef } from '@angular/cdk/overlay';
import { ItemType, Priority } from '../../../core/models/Project';
import { WorkItemDto } from '../../../core/models/WorkItem';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-workitemdetails',
  imports: [FormsModule],
  templateUrl: './workitemdetails.html',
  styleUrl: './workitemdetails.scss',
})
export class Workitemdetails implements OnInit {
  item = signal<WorkItemDto|null>(null);
  loading = signal(true);
  description = '';

  constructor(@Inject(WORK_ITEM_ID) public itemId: number,private overlayRef: OverlayRef,
    private workItemService: workitemservice) { }

  ngOnInit(): void {
    this.workItemService.getWorkItemById(this.itemId).subscribe({
      next: (data) => {
        this.item.set(data);
        this.loading.set(false);
        this.description = data.description ?? '';
      },
      error: (error) => {
        console.error('Error fetching work item details:', error);
        this.loading.set(false);
      }
    });
  }

  getPriorityName(priority: Priority): string {
   return Priority[priority];
  }
  getTypeName(type: ItemType): string {
    return ItemType[type];
  }

  close() {
    this.overlayRef.detach(); // This closes the portal
  }

}
