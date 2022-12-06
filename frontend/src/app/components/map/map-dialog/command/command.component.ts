import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { Command } from './command';
import { CompumatType } from '../../../compumat/compumatType.enum';

@Component({
  selector: 'app-command',
  templateUrl: './command.component.html',
  styleUrls: ['./command.component.scss']
})
export class CommandComponent implements OnInit {

  @Input() compumat: any;
  @Output() isInputValid = new EventEmitter<boolean>();

  requiredField = new FormControl('', [Validators.required]);
  optionalField = new FormControl('', []);
  validators: Command[] = [];
  public selectedCommandIndex: number = undefined;
  public selectedCompumatIndex: number = undefined;


  commands = [
    {
      compumat: 'VENDINGMACHINE',
      cmds: [
        {
          cmdStr: "@PRINTRECEIPT",
          cmdName: "Print Receipt",
          args: [
              {
                  argName: "id",
                  argType: "number",
                  argPlaceholder: "Ex. 1",
                  validArgs: null,
                  minArg: null,
                  maxArg: null,
                  required: true
              },{
                  argName: "receiptNo",
                  argType: "text",
                  argPlaceholder: "Ex. 1562d",
                  validArgs: null,
                  minArg: null,
                  maxArg: null,
                  required: true
              },{
                  argName: "custId",
                  argType: "text",
                  argPlaceholder: "Ex. Thomas5412",
                  validArgs: null,
                  minArg: null,
                  maxArg: null,
                  required: false
              },{
                  argName: "receiptFormat",
                  argType: "text",
                  argPlaceholder: "Either A3 or A4",
                  validArgs : [
                      "A4",
                      "A3"
                  ],
                  minArg: null,
                  maxArg: null,
                  required: true
              },{
                  argName: "fontSize",
                  argType: "number",
                  argPlaceholder: "Between 12 and 24",
                  validArgs: null,
                  minArg: 12,
                  maxArg: 24,
                  required: false
              }
          ]
        },{
          cmdStr:"@TURNOFF",
          cmdName: "Turn Off",
          args: [
            {
                argName: "id",
                argType: "number",
                argPlaceholder: "Ex. 1",
                validArgs: null,
                minArg: null,
                maxArg: null,
                required: true
            },{
                argName: "custId",
                argType: "text",
                argPlaceholder: "Ex. Thomas5412",
                validArgs: null,
                minArg: null,
                maxArg: null,
                required: false
            }
        ]
        }
      ]
  },
  {
    compumat: 'GATE',
    cmds: [
      {
        cmdStr: "@OPEN",
        cmdName: "Open Gate",
        args: [
            {
                argName: "id",
                argType: "number",
                argPlaceholder: "Ex. 1",
                validArgs: null,
                minArg: null,
                maxArg: null,
                required: true
            }
        ]
      },{
        cmdStr: "@CLOSE",
        cmdName: "Close Gate",
        args: [
            {
                argName: "id",
                argType: "number",
                argPlaceholder: "Ex. 1",
                validArgs: null,
                minArg: null,
                maxArg: null,
                required: true
            }
        ]
      },{
        cmdStr: "@RESTART",
        cmdName: "Restart Gate",
        args: [
            {
                argName: "id",
                argType: "number",
                argPlaceholder: "Ex. 1",
                validArgs: null,
                minArg: null,
                maxArg: null,
                required: true

            },{
              argName: "timeStamp",
              argType: "text",
              argPlaceholder: "Ex. 1",
              validArgs: null,
              minArg: null,
              maxArg: null,
              required: true
          }
        ]
      }
    ]
  }

]

  constructor() { }

  ngOnInit(): void {
    this.selectCompumat();
    this.commands.forEach(element => {
      element.cmds.forEach(cmd => {
        cmd.args.forEach(arg => {
          if (arg.required) {
            this.validators.push({name: cmd.cmdName, validators: [new FormControl(null, [Validators.required])]});
          } else {
            this.validators.push({name: cmd.cmdName, validators: [new FormControl(null)]});
          }
        });
      }
    )});
  }

  selectCommand(commandName: string){
    this.isInputValid.emit(false);
    this.commands[this.selectedCompumatIndex].cmds.findIndex((cmd, index) => {
      if (cmd.cmdName === commandName){
        this.selectedCommandIndex = index;
      }
    }
  )}

  selectCompumat(){
    this.commands.findIndex((compumat, index) => {
      if (CompumatType[compumat.compumat] === this.compumat.data.type){
        this.selectedCompumatIndex = index;
      }
    }
  )}


  checkValidInput(index: number, value: any){
    console.log(this.validators[index].validators[0]);
    console.log(value);
    this.validators[index].validators[0].setValue(value);
    let argArr = this.commands[this.selectedCompumatIndex].cmds[this.selectedCommandIndex].args;
    let valid = true;
    argArr.forEach((element, i)  => {
      if (element.required && this.validators[i].validators[0].invalid){
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

