import { Component, Input, OnInit } from '@angular/core';
import { CompumatType } from '../compumat/compumatType.enum';
import { Icon } from './icon';
import { IconService } from './icon.service';

@Component({
  selector: 'app-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss']
})
export class IconComponent implements OnInit {

  @Input() type: CompumatType | undefined;
  icon: Icon;
  fillColor = '#7CFC00';

  constructor(
    private iconService: IconService,
  ) { }

  ngOnInit(): void {
    this.icon = this.iconService.getIcon(this.type);
  }

  getIconPath(): string{
    return this.icon.iconPath;
  }

  rotateArm(): void {
    if(this.type == CompumatType.GATE){

    }
  }

  getColor(): string {
    return this.fillColor;
  }
}
