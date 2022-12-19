import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { Command, CommandValidator } from './command';
import { CompumatType } from '../../../compumat/compumatType.enum';
import { CommandService } from './command.service';
import { take } from 'rxjs/operators';
import { Compumat } from 'src/app/components/compumat/compumat';

@Component({
  selector: 'app-command',
  templateUrl: './command.component.html',
  styleUrls: ['./command.component.scss'],
})
export class CommandComponent implements OnInit {
  @Input() compumat: Compumat;
  @Output() isInputValid = new EventEmitter<boolean>();

  requiredField = new FormControl('', [Validators.required]);
  optionalField = new FormControl('', []);

  validators: CommandValidator[] = [];

  public selectedCommandIndex: number = undefined;
  public selectedCompumatIndex: number = undefined;

  commands: Command[] = [];

  constructor(
    private commandService: CommandService,
  ) {}

  ngOnInit(): void {
    this.commandService.commandData.subscribe(commands => {
      commands.forEach(cmd => {
        if (cmd.validCompumats.includes(this.compumat.type)){
          this.commands.push(cmd);
        }
      });
      console.log(this.commands);

      this.selectCompumat();
      this.commands.forEach((element) => {
        element.arguments.forEach((arg) => {
          if (arg.required) {
            this.validators.push({
              commandName: element.commandName,
              validators: [new FormControl(null, [Validators.required])],
            });
          } else {
            this.validators.push({
              commandName: element.commandName,
              validators: [new FormControl(null)],
            });
          }
        });
      });
    });
  }

  selectCommand(commandName: string) {
    this.isInputValid.emit(false);
    this.commands.findIndex((cmd, index) => {
      if (cmd.commandName === commandName) {
        this.selectedCommandIndex = index;
      }
    });
  }

  selectCompumat() {
    this.commands.findIndex((cmd, index) => {
      if (cmd.validCompumats.includes(this.compumat.type)) {
        this.selectedCompumatIndex = index;
      }
    });
  }

  checkValidInput(index: number, value: any) {
    console.log(this.validators[index].validators[0]);
    console.log(value);
    this.validators[index].validators[0].setValue(value);
    let argArr = this.commands[this.selectedCommandIndex].arguments;
    let valid = true;
    argArr.forEach((element, i) => {
      if (element.required && this.validators[i].validators[0].invalid) {
        valid = false;
      }
    });
    // if (this.validators[index].validators[0].invalid){
    //   this.isInputValid.emit(false);
    // }
    // else{
    //   this.isInputValid.emit(true);
    // }
    this.isInputValid.emit(valid);
  }
}

