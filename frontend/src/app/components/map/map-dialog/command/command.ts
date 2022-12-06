import { FormControl } from "@angular/forms";

export interface Command {
  name: string;
  validators: FormControl[];
}
