import { CdkDragMove, DragDropModule } from '@angular/cdk/drag-drop';
import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  imports: [DragDropModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  protected defaultWidth = 500;
  protected currentWidth = signal(this.defaultWidth);
  open = signal(false);

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
