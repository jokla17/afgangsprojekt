import { FormControl, Validator } from "@angular/forms";
import { CompumatType } from "src/app/components/compumat/compumatType.enum";

export interface Command {
  commandName: string,
  commandString: string,
  validCompumats: CompumatType[],
  arguments: Argument[],
}

export interface Argument {
  argName: string,
  argType: string,
  argPlaceholder: string,
  validArgs: any[],
  minArg: any,
  maxArg: any,
  required: boolean
}

export interface CommandValidator{
  commandName: string,
  validators: FormControl[],
}
